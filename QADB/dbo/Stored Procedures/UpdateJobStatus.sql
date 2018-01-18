CREATE PROCEDURE [dbo].[UpdateJobStatus](
       @TaskID        BIGINT,
       @RunningStatus INT,
       @UserName      VARCHAR(50))
AS
    BEGIN
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        DECLARE @TransactionName VARCHAR(100) = 'UpdateJobStatus';
        BEGIN TRY
            BEGIN TRAN @TransactionName;
            DECLARE @CurrentDate DATETIME;
            SET @CurrentDate = GETUTCDATE();
            DECLARE @ExistingStatus INT;
            DECLARE @TotalClaimCount BIGINT;
            DECLARE @ClaimCount BIGINT;
            DECLARE @LastUpdatedDateForElapsedTime DATETIME;
            DECLARE @ElapsedTime BIGINT;
            DECLARE @ExistingElapsedTime BIGINT;
            DECLARE @IsDataPickedForAdjudication BIT;
            DECLARE @IsUserDefined BIT;
            DECLARE @TaskIdTable TABLE( Id     INT IDENTITY(1, 1),
                                        TaskId BIGINT );
            DECLARE @TaskCount INT;
            DECLARE @LoopCounter INT = 1;
            DECLARE @CurrentTaskId BIGINT;
            DECLARE @ModelId                INT,
                    @ClaimToolDesc          VARCHAR(1000),
                    @Action                 VARCHAR(100),
                    @IsBackgroundAdjucation BIT;
            IF( @TaskID = 0 )
                BEGIN
                    INSERT INTO @TaskIdTable
                           SELECT TaskId
                           FROM TrackTasks WITH ( NOLOCK )
                           WHERE RunningStatus = 100;
                END;
            ELSE
                BEGIN
                    INSERT INTO @TaskIdTable
                           SELECT @TaskID;
                END;
            SELECT @TaskCount = COUNT(*)
            FROM @TaskIdTable;
            WHILE( @LoopCounter <= @TaskCount )
                BEGIN
                    SELECT @CurrentTaskId = TaskId
                    FROM @TaskIdTable
                    WHERE Id = @LoopCounter;
			
                    --Getting TotalClaimCount,ExistingStatus,IsDataPickedForAdjudication,LastUpdatedDateForElapsedTime,ElapsedTime,IsUserDefined,ModelID from TrackTask based on given taskId
                    SELECT @TotalClaimCount = TotalClaimCount,
                           @ExistingStatus = RunningStatus,
                           @IsDataPickedForAdjudication = IsDataPickedForAdjudication,
                           @LastUpdatedDateForElapsedTime = LastUpdatedDateForElapsedTime,
                           @ExistingElapsedTime = ISNULL(ElapsedTime, 0),
                           @IsUserDefined = IsUserDefined,
                           @ModelId = ModelID
                    FROM TrackTasks WITH ( NOLOCK )
                    WHERE TaskID = @CurrentTaskId;
			
                    --Calculating Elapsed Time
                    SET @ElapsedTime = @ExistingElapsedTime;
                    SET @ElapsedTime = @ElapsedTime + DATEDIFF(ss, @LastUpdatedDateForElapsedTime, @CurrentDate);
 
                    --Checking if RunningStatus is 100 and ExistingStatus not 131 and Updating the job status and  ElapsedTime in TrackTask table.         
                    IF( @RunningStatus = 100
                    AND @ExistingStatus <> 131
                    AND (( SELECT COUNT(*)
                           FROM TaskClaims WITH ( NOLOCK )
                           WHERE taskid = @TaskID
                             AND isadjudicated = 0 ) <= 0 )
                    AND @IsDataPickedForAdjudication = 1
                      )
                        BEGIN
                            IF(( @ExistingStatus <> 100
                             AND @ExistingStatus <> 132
                               )
                              )
                                BEGIN
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus,
                                          UpdateDate = @CurrentDate,
                                          ElapsedTime = @ElapsedTime,
                                          LastUpdatedDateForElapsedTime = @CurrentDate
                                    WHERE TaskID = @TaskID;
                                    DELETE TC
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID;

                                    ---- Delete From TaskRetainedClaims
                                    DELETE TRC
                                    FROM TaskRetainedClaims TRC WITH ( NOLOCK )
                                    WHERE TRC.TaskID = @TaskID;
                                END;
                            ELSE
                                BEGIN
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus,
                                          ElapsedTime = @ElapsedTime,
                                          LastUpdatedDateForElapsedTime = @CurrentDate,
                                          UpdateDate = @CurrentDate
                                    WHERE TaskID = @TaskID;
                                    DELETE TC
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID;

                                    ---- Delete From TaskRetainedClaims
                                    DELETE TRC
                                    FROM TaskRetainedClaims TRC WITH ( NOLOCK )
                                    WHERE TRC.TaskID = @TaskID;
                                END;
                        END;
                                    --Checking if any job is running or resumed 

                    ELSE
                    IF((( SELECT COUNT(*)
                          FROM TrackTasks
                          WHERE RunningStatus = 129
                            AND IsUserDefined = 1 ) > 0 )
                    OR (( SELECT COUNT(*)
                          FROM TrackTasks
                          WHERE RunningStatus = 132
                            AND IsUserDefined = 1 ) > 0 )
                      )
                        BEGIN
                            IF( @RunningStatus = 133 ) --Error/Exception
                                BEGIN
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus
                                    WHERE TaskID = @TaskID;
                                    DELETE TC
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID;
                                    UPDATE RunningTask
                                      SET IsRunning = 0
                                    WHERE ModelId = @ModelId;


                                    ---- Delete From TaskRetainedClaims
                                    DELETE TRC
                                    FROM TaskRetainedClaims TRC WITH ( NOLOCK )
                                    WHERE TRC.TaskID = @TaskID;
                                END;
                                    --Checking if job previously run and setting status to resumed 

                            ELSE
                            IF( @RunningStatus = 129
                            AND @TotalClaimCount = 0
                            AND @IsDataPickedForAdjudication = 1
                            AND @ExistingStatus NOT IN( 128, 129 )
                            AND ( SELECT COUNT(*)
                                  FROM TrackTasks
                                  WHERE( RunningStatus = 129
                                      OR RunningStatus = 132
                                       )
                                   AND ModelID = @ModelId ) = 0
                              )
                                BEGIN
                                    UPDATE TC
                                      SET TC.IsPicked = 0
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID
                                      AND TC.IsPicked = -1
                                      AND TC.IsAdjudicated = 0;
                                    UPDATE TrackTasks
                                      SET RunningStatus = 132,
                                          LastUpdatedDateForElapsedTime = @CurrentDate
                                    WHERE TaskID = @TaskID;
                                END;
                                    --Checking if job previously run and then setting status to resumed 

                            ELSE
                            IF( @RunningStatus = 129
                            AND @IsDataPickedForAdjudication = 1
                            AND @ExistingStatus IN( 128 )
                            AND @ExistingElapsedTime <> 0
                            AND ( SELECT COUNT(*)
                                  FROM TrackTasks
                                  WHERE( RunningStatus = 129
                                      OR RunningStatus = 132
                                       )
                                   AND ModelID = @ModelId ) = 0
                              )
                                BEGIN
                                    UPDATE TC
                                      SET TC.IsPicked = 0
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID
                                      AND TC.IsPicked = -1
                                      AND TC.IsAdjudicated = 0;
                                    UPDATE TrackTasks
                                      SET RunningStatus = 132,
                                          LastUpdatedDateForElapsedTime = @CurrentDate
                                    WHERE TaskID = @TaskID;
                                END;
                                    --Checking no job is running for given model  and setting status to running

                            ELSE
                            IF( @RunningStatus = 129
                            AND ( SELECT COUNT(*)
                                  FROM TrackTasks WITH (NOLOCK)
                                  WHERE( RunningStatus = 129
                                      OR RunningStatus = 132
                                       )
                                   AND ModelID = @ModelId ) = 0
                              )
                                BEGIN
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus,
                                          LastUpdatedDateForElapsedTime = @CurrentDate
                                    WHERE TaskID = @TaskID;
                                END;
                                    --setting status to requested

                            ELSE
                            IF( @RunningStatus = 129 )
                                BEGIN
                                    UPDATE TC
                                      SET TC.IsPicked = -1
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID
                                      AND TC.IsAdjudicated = 0;
                                    UPDATE TrackTasks
                                      SET RunningStatus = 128
                                    WHERE TaskID = @TaskID;
                                END;
                                    --Checking no job is running for given model  and setting status to resumed

                            ELSE
                            IF( @RunningStatus = 132
                            AND ( SELECT COUNT(*)
                                  FROM TrackTasks WITH (NOLOCK)
                                  WHERE( RunningStatus = 129
                                      OR RunningStatus = 132
                                       )
                                   AND ModelID = @ModelId ) = 0
                              )
                                BEGIN
                                    UPDATE TC
                                      SET TC.IsPicked = 0
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID
                                      AND TC.IsPicked = -1
                                      AND TC.IsAdjudicated = 0;
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus,
                                          LastUpdatedDateForElapsedTime = @CurrentDate
                                    WHERE TaskID = @TaskID;
                                END;
                                    --setting status to requested

                            ELSE
                            IF( @RunningStatus = 132 )
                                BEGIN
                                    UPDATE TC
                                      SET TC.IsPicked = -1
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID
                                      AND TC.IsAdjudicated = 0;
                                    UPDATE TrackTasks
                                      SET RunningStatus = 128
                                    WHERE TaskID = @TaskID;
                                END;
                                    --setting status to paused or requested

                            ELSE
                            IF( @RunningStatus = 130
                            AND @ExistingStatus = 128
                              )
                                BEGIN
                                    UPDATE TC
                                      SET TC.IsPicked = 0
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID
                                      AND TC.IsPicked = -1
                                      AND TC.IsAdjudicated = 0;
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus,
                                          LastUpdatedDateForElapsedTime = @CurrentDate
                                    WHERE TaskID = @TaskID;
                                END;
                                    --setting status to paused if job was not in complete stage

                            ELSE
                            IF( @RunningStatus = 130
                            AND @ExistingStatus <> 100
                              )
                                BEGIN
                                    UPDATE TC
                                      SET TC.IsPicked = -1
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID
                                      AND TC.IsAdjudicated = 0;
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus,
                                          ElapsedTime = @ElapsedTime
                                    WHERE TaskID = @TaskID;
                                END;
                            ELSE
                            IF( @RunningStatus = 131 ) -- Cancelled
                                BEGIN
                                    DELETE TC
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID;


                                    ---- Delete From TaskRetainedClaims
                                    DELETE TRC
                                    FROM TaskRetainedClaims TRC WITH ( NOLOCK )
                                    WHERE TRC.TaskID = @TaskID;
                                END;
                                    --setting status to complete for background adjudication

                            ELSE
                            IF( @RunningStatus = 100
                            AND @IsUserDefined = 0
                              )
                                BEGIN
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus,
                                          ElapsedTime = @ElapsedTime
                                    WHERE TaskID = @TaskID;
                                    DELETE TC
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID;

                                    ---- Delete From TaskRetainedClaims
                                    DELETE TRC
                                    FROM TaskRetainedClaims TRC WITH ( NOLOCK )
                                    WHERE TRC.TaskID = @TaskID;
                                END;;;;;;;;;;
                        END;
      
                            --if no job is running or resumed 

                    ELSE
                        BEGIN
                            ----------------------
                            IF( @RunningStatus = 133 ) --Error/Exception
                                BEGIN
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus
                                    WHERE TaskID = @TaskID;
                                    DELETE TC
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID;

                                    ---- Delete From TaskRetainedClaims
                                    DELETE TRC
                                    FROM TaskRetainedClaims TRC WITH ( NOLOCK )
                                    WHERE TRC.TaskID = @TaskID;
                                    UPDATE RunningTask
                                      SET IsRunning = 0
                                    WHERE ModelId = @ModelId;
                                END;
                            ELSE
                            IF( @RunningStatus = 130
                            AND @ExistingStatus = 128
                              ) --Paused
                                BEGIN
                                    UPDATE TC
                                      SET TC.IsPicked = -1
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID
                                      AND TC.IsAdjudicated = 0;
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus
                                    WHERE TaskID = @TaskID;
                                END;
                            ELSE
                            IF( @RunningStatus = 130
                            AND @ExistingStatus <> 100
                              ) --Paused
                                BEGIN
                                    UPDATE TC
                                      SET TC.IsPicked = -1
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID
                                      AND TC.IsAdjudicated = 0;
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus,
                                          ElapsedTime = @ElapsedTime
                                    WHERE TaskID = @TaskID;
                                END;
                            ELSE
                            IF( @RunningStatus = 132 ) --Resumed
                                BEGIN
                                    UPDATE TC
                                      SET TC.IsPicked = 0
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID
                                      AND TC.IsPicked = -1
                                      AND TC.IsAdjudicated = 0;
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus,
                                          LastUpdatedDateForElapsedTime = @CurrentDate
                                    WHERE TaskID = @TaskID;
                                END;
                            ELSE
                            IF( @RunningStatus = 131
                            AND @ExistingStatus <> 100
                              ) -- Cancelled
                                BEGIN
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus
                                    WHERE TaskID = @TaskID;
                                    DELETE TC
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID;

                                    ---- Delete From TaskRetainedClaims
                                    DELETE TRC
                                    FROM TaskRetainedClaims TRC WITH ( NOLOCK )
                                    WHERE TRC.TaskID = @TaskID;
                                END;
                                    --Checking if job previously run and then setting status to resumed

                            ELSE
                            IF( @RunningStatus = 129
                            AND @TotalClaimCount = 0
                            AND @IsDataPickedForAdjudication = 1
                            AND @ExistingStatus NOT IN( 128, 129 )
                              )
                                BEGIN
                                    UPDATE TC
                                      SET TC.IsPicked = 0
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID
                                      AND TC.IsPicked = -1
                                      AND TC.IsAdjudicated = 0;
                                    UPDATE TrackTasks
                                      SET RunningStatus = 132,
                                          LastUpdatedDateForElapsedTime = @CurrentDate
                                    WHERE TaskID = @TaskID;
                                END;
                                    --Checking if job previously run and and status is requestd then setting status to resumed

                            ELSE
                            IF( @RunningStatus = 129
                            AND @IsDataPickedForAdjudication = 1
                            AND @ExistingStatus IN( 128 )
                            AND @ExistingElapsedTime <> 0
                              )
                                BEGIN
                                    UPDATE TC
                                      SET TC.IsPicked = 0
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID
                                      AND TC.IsPicked = -1
                                      AND TC.IsAdjudicated = 0;
                                    UPDATE TrackTasks
                                      SET RunningStatus = 132,
                                          LastUpdatedDateForElapsedTime = @CurrentDate
                                    WHERE TaskID = @TaskID;
                                END;
                            ELSE
                            IF( @RunningStatus = 129 )--setting status to running
                                BEGIN
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus,
                                          LastUpdatedDateForElapsedTime = @CurrentDate
                                    WHERE TaskID = @TaskID;
                                END;
                            ELSE
                            IF( @RunningStatus <> 100 )--setting status to Complete
                                BEGIN
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus
                                    WHERE TaskID = @TaskID;
                                END;
                                    --setting status to complete for background adjudication

                            ELSE
                            IF( @RunningStatus = 100
                            AND @IsUserDefined = 0
                              )
                                BEGIN
                                    UPDATE TrackTasks
                                      SET RunningStatus = @RunningStatus,
                                          ElapsedTime = @ElapsedTime
                                    WHERE TaskID = @TaskID;
                                    DELETE TC
                                    FROM TaskClaims TC WITH ( NOLOCK )
                                    WHERE TC.TaskID = @TaskID;

                                    ---- Delete From TaskRetainedClaims
                                    DELETE TRC
                                    FROM TaskRetainedClaims TRC WITH ( NOLOCK )
                                    WHERE TRC.TaskID = @TaskID;
                                END;;;;;;;;;
                        END;;
                    IF( @RunningStatus = 131
                    AND @IsUserDefined = 1
                      )
                        BEGIN
                            SET @Action = 'Delete';
                            UPDATE TrackTasks
                              SET RunningStatus = @RunningStatus
                            WHERE TaskID = @TaskID;
                                  
                            --Audit Logging
                            SELECT @ClaimToolDesc = 'Job Name: ' + RequestName
                            FROM TrackTasks WITH (NOLOCK)
                            WHERE TaskId = @TaskID;
                            EXEC InsertAuditLog
                                 @UserName,
                                 @Action,
                                 'Jobs',
                                 @ClaimToolDesc,
                                 @TaskID,
                                 4;
                        END;
                    SET @LoopCounter = @LoopCounter + 1;
                END;
	   
            --Check Any Transaction happened than commit transaction
            COMMIT TRANSACTION @TransactionName;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;