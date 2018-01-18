CREATE PROCEDURE [dbo].[AddReassignedClaimJob](@RequestName      VARCHAR(100),
                                               @UserName         VARCHAR(100),
                                               @DateType         INT,
                                               @DateFrom         DATETIME,
                                               @DateTo           DATETIME,
                                               @ReassignClaims   REASSIGNCLAIMS READONLY,
                                               @HeaderModelId    BIGINT,
                                               @HeaderContractId BIGINT,
                                               @SelectAll        BIT,
                                               @SearchCriteria   VARCHAR(MAX),
                                               @SelectAllHeader  BIT)
AS
     BEGIN
         DECLARE @ModelList AS MODELLIST, @TransactionName VARCHAR(100)= 'AddReassignedClaimJob', @ModelCount INT, @Index INT= 1, @ClaimList AS CLAIMLIST, @RequestNameWithModel VARCHAR(100), @FacilityID BIGINT, @ModelID BIGINT, @TrackTaskID BIGINT, @Loop INT= 1, @TotalClaimCountQuery NVARCHAR(MAX), @AllClaimList AS CLAIMLIST, @AllReassignClaims AS REASSIGNCLAIMS, @PrimaryModelId BIGINT, @TaskModelId BIGINT, @HeaderPrimaryContractId BIGINT, @ReassignCount INT, @RetainedCount INT, @IsPrimaryContractChanged BIT= 0;
         BEGIN TRY
             SELECT @FacilityID = FacilityId
             FROM ContractHierarchy CH WITH (NOLOCK)
             WHERE NodeId =
             (
                 SELECT TOP (1) ModelID
                 FROM @ReassignClaims
             );
             SELECT @PrimaryModelId = NodeId
             FROM [dbo].[ContractHierarchy] WITH (NOLOCK)
             WHERE FacilityID = @FacilityID
                   AND IsPrimaryNode = 1;
             IF(@SelectAll = 1)
                 BEGIN
                     SELECT @TotalClaimCountQuery = dbo.GetClaimCountQuery(@DateType, @DateFrom, @DateTo, @SearchCriteria, @FacilityID, NULL);
                    
                     --Getting all the Claims in ClaimList
                     INSERT INTO @AllClaimList
                     EXECUTE sp_executesql
                             @TotalClaimCountQuery;
                     IF(@SelectAllHeader = 0) --Select Header not checked
                         BEGIN
                             DELETE FROM @AllClaimList
                             WHERE claimID IN
                             (
                                 SELECT Ac.claimID
                                 FROM @AllClaimList Ac
                                      INNER JOIN RetainedClaims rc ON rc.claimID = Ac.claimID
                                 WHERE Ac.ClaimID NOT IN
                                 (
                                     SELECT ClaimID
                                     FROM @ReassignClaims
                                 )
                             );
                             SET @HeaderPrimaryContractId = 0; --Primary Contract not Selected
                         END;
                     ELSE --Select Header checked or primary Contract changed 
                         BEGIN
                             IF EXISTS
                             (
                                 SELECT 1
                                 FROM @ReassignClaims RC
                                      JOIN RetainedClaims RTN WITH (NOLOCK) ON RC.ClaimID = RTN.ClaimID --Primary Contract Selected
                                 WHERE RC.IsRetained = 1
                             )
                                 BEGIN
                                     SELECT TOP 1 @HeaderPrimaryContractId = RC.ContractID
                                     FROM @ReassignClaims RC
                                          JOIN RetainedClaims RTN WITH (NOLOCK) ON RC.ClaimID = RTN.ClaimID
                                     WHERE RC.IsRetained = 1;
                                     SELECT @ReassignCount = COUNT(RC.ClaimID)
                                     FROM @ReassignClaims RC
                                          JOIN RetainedClaims RTN WITH (NOLOCK) ON RC.ClaimID = RTN.ClaimID
                                     WHERE RC.IsRetained = 1;
                                     SELECT @RetainedCount = COUNT(RC.ClaimID)
                                     FROM @ReassignClaims RC
                                          JOIN RetainedClaims RTN WITH (NOLOCK) ON RC.ClaimID = RTN.ClaimID
                                     WHERE RC.ContractID = @HeaderPrimaryContractId;
                                     IF(@ReassignCount = @RetainedCount)
                                         BEGIN
                                             SET @IsPrimaryContractChanged = 1;
                                         END;
                                 END;
                             ELSE
                             SET @HeaderPrimaryContractId = 0; --Select header clicked and Primary Contract not Selected
                         END;
                     IF(@HeaderModelId = 0)
                         BEGIN
                             SET @HeaderModelId = @PrimaryModelId;
                         END;
                     INSERT INTO @AllReassignClaims
                            SELECT AC.ClaimID,
                                   CASE RC.ClaimID
                                       WHEN AC.ClaimID
                                       THEN @PrimaryModelId
                                       ELSE @HeaderModelId
                                   END,
                                   CASE @SelectAllHeader
                                       WHEN 1 --Select header clicked Or Primary Contract Selected
                                       THEN IIF(RC.ClaimID = AC.ClaimID, IIF(@HeaderContractId = @HeaderPrimaryContractId
                                                                             OR @IsPrimaryContractChanged = 1, @HeaderPrimaryContractId, RC.ContractId), @HeaderContractId)
                                       ELSE @HeaderContractId
                                   END,
                                   IIF(RC.ClaimID IS NULL, 0, 1),
                                   1
                            FROM @AllClaimList AC
                                 LEFT JOIN RetainedClaims RC WITH (NOLOCK) ON RC.ClaimID = AC.ClaimID
                            WHERE AC.ClaimID NOT IN
                            (
                                SELECT ClaimID
                                FROM @ReassignClaims
                            );
                     --FIXED-JAN16 Remove below condition and add that code into upper condition as both are same.
                     INSERT INTO @AllReassignClaims
                            SELECT *
                            FROM @ReassignClaims;
                 END;
             IF(@SelectAll = 0)
                 BEGIN
                     INSERT INTO @AllReassignClaims
                            SELECT *
                            FROM @ReassignClaims
                            WHERE IsSelected = 1;
                 END;
					  	  
             --Inserting Reassigned claims
             EXEC AddReassignedClaims
                  @UserName,
                  @AllReassignClaims;
	  
             ---Getting Distinct modelId in ModelList
             INSERT INTO @ModelList
                    SELECT DISTINCT
                           ModelID
                    FROM @AllReassignClaims
                    WHERE ModelId <> 0
                          AND IsSelected = 1;
	 
             --Getting Count of ModelID
             SET @ModelCount = @@RowCount;
	  
             ---Loop Begin
             BEGIN TRY
                 BEGIN TRAN @TransactionName;
                 WHILE @Index <= @ModelCount
                     BEGIN
                         DECLARE @SelectCriteria VARCHAR(MAX)= '24|3|', --24|3 used to add claim criteria
                         @ModelName VARCHAR(100), @ClaimListText VARCHAR(MAX)= '', @TotalClaimCount INT;

                         --Setting Model name, Model id and Facility id
                         SELECT @ModelName = CH.NodeText,
                                @ModelID = CH.NodeID,
                                @FacilityID = CH.FacilityID
                         FROM ContractHierarchy CH WITH (NOLOCK)
                              INNER JOIN @ModelList ML ON CH.NodeID = ML.ModelID
                         WHERE ML.RowNumber = @Index
                               AND CH.IsDeleted = 0;

                         --Getting all the Claims in ClaimList
                         INSERT INTO @ClaimList
                                SELECT DISTINCT
                                       ClaimID
                                FROM @AllReassignClaims RC
                                     INNER JOIN @ModelList ML ON RC.ModelID = ML.ModelID
                                WHERE ML.RowNumber = @Index
                                      AND RC.IsSelected = 1;
                         IF(@SearchCriteria IS NULL)
                             SET @SearchCriteria = '';
                         IF(@SelectAll = 1
                            AND @ModelID = @HeaderModelId)
                             BEGIN
                                 DECLARE @UnSelectedClaims VARCHAR(MAX);
                                 SELECT @UnSelectedClaims = COALESCE(@UnSelectedClaims+', ', '')+CONVERT( VARCHAR(15), ClaimID)
                                 FROM @AllReassignClaims
                                 WHERE ModelId <> @HeaderModelId
                                       OR IsSelected = 0;
                                 IF(ISNULL(@UnSelectedClaims, '') <> '')
                                     BEGIN
                                         SET @SelectCriteria = IIF(@SearchCriteria = '', '24|1|'+@UnSelectedClaims, @SearchCriteria+'~24|1|'+@UnSelectedClaims);
                                     END;
                                 ELSE
                                     BEGIN
                                         SET @SelectCriteria = @SearchCriteria;
                                     END;
                             END;
                         ELSE
                             BEGIN

                                 --setting ClaimListText
                                 SELECT @ClaimListText = @ClaimListText+COALESCE(CAST(ClaimID AS VARCHAR(MAX))+',', '')
                                 FROM @ClaimList;
                                 SELECT @ClaimListText = LEFT(@ClaimListText, DATALENGTH(@ClaimListText) - 1);

                                 --Setting SelectCriteria
                                 SET @SelectCriteria = @SelectCriteria + @ClaimListText;
                             END;
	  	  

                         --Setting Request Name
                         SET @RequestNameWithModel = @RequestName+'_'+@ModelName;
                         SELECT @TotalClaimCount = COUNT(*)
                         FROM @ClaimList; 
                         --Inserting into TrackTasks.
                         INSERT INTO TrackTasks
                         (InsertDate,
                          RequestName,
                          IsUserDefined,
                          RunningStatus,
                          SelectCriteria,
                          FacilityID,
                          ModelID,
                          DateType,
                          DateFrom,
                          DateTo,
                          UserName,
                          Priority,
                          TotalClaimCount,
                          IsDataPickedForAdjudication
                         )
                         VALUES
                         (GETUTCDATE(),
                          @RequestNameWithModel,
                          1,
                          128,
                          @SelectCriteria,
                          @FacilityID,
                          @ModelID,
                          @DateType,
                          @DateFrom,
                          @DateTo,
                          @UserName,
                          0,
                          @TotalClaimCount,
                          1
                         );
                         SET @TrackTaskID = @@IDENTITY;

                          --Inserting into TaskClaims.
                         INSERT INTO TaskClaims
                         (ClaimID,
                          TaskID,
                          IsAdjudicated,
                          IsPicked,
					 RowID
                         )
                                SELECT ClaimID,
                                       @TrackTaskID,
                                       0,
                                       0
							    , ROW_NUMBER() OVER (ORDER BY ClaimID ASC)
                                FROM @ClaimList;



                         --Inserting into TaskClaims.
                         INSERT INTO TaskRetainedClaims
                         (TaskID,
                          ClaimID,
                          ContractID
                         )
                                SELECT @TrackTaskID,
                                       TC.ClaimID,
                                       RC.ContractID
                                FROM @ClaimList TC
                                     INNER JOIN @AllReassignClaims RC ON TC.ClaimID = RC.ClaimID
                                WHERE RC.ContractID != 0;
	                      
                         -- Add ModelId to RunningTasks
                         SELECT @TaskModelId = ModelID
                         FROM @ModelList
                         WHERE RowNumber = @Index;
                         EXEC [dbo].[AddRunningTask]
                              @TaskModelId;

                         --Delete From @ClaimList
                         DELETE @ClaimList;
                         SET @Index = @Index + 1;
                     END;
                 COMMIT TRANSACTION @TransactionName;
             END TRY
             BEGIN CATCH
                 ROLLBACK TRAN @TransactionName;
                 EXEC RaiseErrorInformation;
             END CATCH;
         END TRY
         BEGIN CATCH
             EXEC RaiseErrorInformation;
         END CATCH;
     END;