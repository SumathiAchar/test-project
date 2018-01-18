/****************************************************/

--Method Name : AddClaimsForATask  
--Module      : Adjudication
--Created By  : Raj
--Created Date: 8-Aug-2014
--Modified By :   
--Modify Date :  
--Description : Add Claims into taskclaim table based on taskid

/****************************************************/

CREATE PROCEDURE [dbo].[AddClaimsForATask](
       @TaskID BIGINT )
AS
    BEGIN
        SET NOCOUNT ON;
        DECLARE @PickedClaimCount BIGINT = 0;
        DECLARE @RunningStatus INT;
	   DECLARE @TaskClaims AS ClaimList;
        SELECT @RunningStatus = RunningStatus
        FROM TrackTasks
        WHERE TaskId = @TaskID;
        IF( @RunningStatus = 132 )
            BEGIN
                SELECT @PickedClaimCount = COUNT(*)
                FROM TaskClaims WITH ( NOLOCK )
                WHERE TaskId = @TaskID
                  AND IsPicked = 1
                  AND IsAdjudicated = 0;
            END;
        IF( @PickedClaimCount = 0 )
            BEGIN
                DECLARE @ClaimCount BIGINT;
                SELECT @ClaimCount = COUNT(*)
                FROM TaskClaims WITH ( NOLOCK )
                WHERE TaskID = @TaskID;

                DECLARE @FacilityID BIGINT;
                SELECT @FacilityID = FacilityID
                FROM TrackTasks WITH ( NOLOCK )
                WHERE TaskID = @TaskID;

			 DECLARE @ModelID BIGINT;
			 SELECT @ModelID = ModelID
                FROM TrackTasks WITH ( NOLOCK )
                WHERE TaskID = @TaskID;

                IF @ClaimCount > 0
                SELECT @ClaimCount;
                ELSE
                    BEGIN
                        DECLARE @IsUserDefined BIT;
                        SELECT @IsUserDefined = IsUserDefined
                        FROM TrackTasks WITH ( NOLOCK )
                        WHERE TaskID = @TaskID;
                        IF @IsUserDefined = 1
                            BEGIN
                                DECLARE @IsDataPickedForAdjudication BIT;
                                DECLARE @ExistingStatus INT;
                                SELECT @IsDataPickedForAdjudication = IsDataPickedForAdjudication,
                                       @ExistingStatus = RunningStatus
                                FROM TrackTasks WITH ( NOLOCK )
                                WHERE TaskID = @TaskID;
                                IF( ISNULL(@IsDataPickedForAdjudication, 0) = 0 )
                                    BEGIN
    
                                        --Update Data picked status to -1
                                        UPDATE TrackTasks
                                          SET IsDataPickedForAdjudication = -1
                                        WHERE TaskID = @TaskID;
                                        DECLARE @DateType INT;
                                        DECLARE @DateFrom DATETIME;
                                        DECLARE @DateTo DATETIME;
                                        DECLARE @SelectCriteria VARCHAR(MAX);
                                        SELECT @DateType = DateType,
                                               @DateFrom = DateFrom,
                                               @DateTo = DateTo,
                                               @SelectCriteria = SelectCriteria
                                        FROM TrackTasks WITH ( NOLOCK )
                                        WHERE TaskID = @TaskID;  
    
                                       
                                        --INSERT INTO TaskClaims 
                                        DECLARE @MainQry VARCHAR(MAX);
                                        SET @MainQry = ( [dbo].[GetSubQueryForClaimFilters]( @DateType, @DateFrom, @DateTo, @SelectCriteria, @FacilityID,@ModelID));
                                        SET @MainQry = STUFF(@MainQry, CHARINDEX('SELECT DISTINCT CD.claimid C FROM ClaimData AS CD', @MainQry), LEN('SELECT DISTINCT CD.claimid C FROM ClaimData AS CD'), '');
								SET @MainQry = 'SELECT DISTINCT CD.ClaimID FROM ClaimData AS CD WITH (NOLOCK) ' + @MainQry + ' ORDER BY CD.ClaimID';
                                       
								INSERT INTO @TaskClaims
								EXEC (@MainQry);
								  
								INSERT INTO TaskClaims 
								SELECT DISTINCT ClaimID, CONVERT(VARCHAR, @TaskID), 0, 0, RowNumber, NULL FROM @TaskClaims	
                                        SELECT @@ROWCOUNT;
                                        IF( @ExistingStatus = 130 )
                                            BEGIN
                                                UPDATE TC
                                                  SET TC.IsPicked = -1
                                                FROM dbo.TaskClaims TC WITH ( NOLOCK )
                                                WHERE TC.TaskID = @TaskID
                                                  AND TC.IsPicked = 0;
                                            END;
				
                                        --Update Data picked status
                                        UPDATE TrackTasks
                                          SET IsDataPickedForAdjudication = 1,
                                              TotalClaimCount = ( SELECT COUNT(TC.ClaimID)
                                                                  FROM dbo.TaskClaims TC WITH ( NOLOCK )
                                                                  WHERE TC.TaskID = @TaskID )
                                        WHERE TaskID = @TaskID;
                                    END;
                                ELSE
                                SELECT 0;
                            END;
                    END;
            END;
        ELSE
        SELECT 0;
    END;
GO