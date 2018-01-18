/****************************************************************************  
 *   Name         : CreateBackgroundAdjudicationTask
 *   Author       : Sheshagiri  
 *   Date         : 02/03/2015  
 *   Alter By     : 
 *   Alter Date   : 
 *   Module       : Background adjudication
 *   Description  : Picks claimids based on matching contract codes with claimdata and update Task and Taskclaims table.


EXEC  CreateBackgroundAdjudicationTask 3,200
 *****************************************************************************/

CREATE PROCEDURE [dbo].[CreateBackgroundAdjudicationTask](
       @FacilityID                         INT,
       @BatchSizeForBackgroundAdjudication INT )
AS
    BEGIN
        --FIXED-JS-FEB2015 Comment should be provided to logical Statements
        SET NOCOUNT ON;
        -- READ UNCOMMITTED option is used to avoid table being blocked during Copy Model and Copy Contract process
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        DECLARE @TaskID BIGINT = 0;
        BEGIN TRY
            BEGIN TRANSACTION;
            DECLARE @TmpFacilityID BIGINT;
            SET @TmpFacilityID = ( SELECT TOP 1 FacilityID
                                   FROM dbo.TrackTasks
                                   WHERE [FacilityID] = @FacilityID
                                     AND [RunningStatus] IN( 129 )
                                   ORDER BY InsertDate DESC );
            IF(( SELECT COUNT(*)
                 FROM dbo.TrackTasks
                 WHERE RunningStatus IN( 128, 129, 132 )
                   AND IsUserDefined = 1 ) > 0 )
                BEGIN
                    SET @TaskID = -1;
                END;
            ELSE
            IF( @TmpFacilityID = @FacilityID )
                BEGIN
                    SET @TaskID = 0;
                END;
            ELSE
                BEGIN
                    DECLARE @PrimaryNodeID         INT = 0,
                            @ContractID            BIGINT = 0,
                            @MainQuery             NVARCHAR(MAX) = CAST('' AS NVARCHAR(MAX)),
                            @LoopStart             BIGINT,
                            @ContractCount         BIGINT = 0,
                            @TotalClaimsPicked     INT = 0,
                            @RemainingClaimCount   INT,
                            @IsReadjudicate        BIT = 0,
                            @ReadjudicateLoopStart BIGINT;

                    -- This table will hold all active ContractIDs											    
                    DECLARE @TempContracts TABLE( RowNumber  BIGINT,
                                                  ContractID BIGINT );
                    DECLARE @TempClaims AS CLAIMCONTRACTID; -- ClaimContractID is User Defined Table Type. Used to pass table variable as paramter during dynamic query execution with Sp_ExecuteSql command
                    -- Get primary model node id
                    SELECT @PrimaryNodeID = NodeID
                    FROM ContractHierarchy
                    WHERE IsPrimaryNode = 1
                      AND FacilityID = @FacilityID;

                    -- Fetch All active contracts for a selected Primary Model
                    INSERT INTO @TempContracts
                           ( RowNumber,
                             ContractID
                           )
                           SELECT RowNumber,
                                  ContractID
                           FROM dbo.GetAllActiveContractsByPrimaryModelID( @PrimaryNodeID );

                    -- Hold total contracts
                    SELECT @ContractCount = COUNT(1)
                    FROM @TempContracts;
                    SET @LoopStart = 1;
                    -- Iterate over all contract
                    WHILE @LoopStart <= @ContractCount
                        BEGIN
                            SELECT @ContractID = ISNULL(ContractId, 0)
                            FROM @TempContracts
                            WHERE ISNULL(RowNumber, 0) = @LoopStart;
				
                            ------Build main query ------------------------ This can converted as user defind function that returns base query for all contract Criteria.

                            SELECT @MainQuery = dbo.GetFilterCodeQueryByContractID( @ContractID, @FacilityID, @PrimaryNodeID, @BatchSizeForBackgroundAdjudication, @TotalClaimsPicked, @IsReadjudicate );

                            ------Build sub query based on contract codes------------------------
                            INSERT INTO @TempClaims
                                   ( ClaimId,
                                     ContractID
                                   )
                            EXEC SYS.sp_executesql
                                 @MainQuery,
                                 N'@tempclaims ClaimContractID READONLY',
                                 @tempclaims;
                            SELECT @TotalClaimsPicked = COUNT(*)
                            FROM @TempClaims;
                            SET @RemainingClaimCount = @BatchSizeForBackgroundAdjudication - @TotalClaimsPicked;
                            IF( @RemainingClaimCount > 0 )
                                BEGIN
                                    IF(( SELECT COUNT(*)
                                         FROM RetainedClaims RC
                                         WHERE RC.ContractID = @ContractID
                                           AND NOT EXISTS( 
                                                           SELECT CA.ClaimID
                                                           FROM ContractAdjudications CA
                                                           WHERE CA.ModelID = @PrimaryNodeID
                                                             AND CA.ClaimID = RC.ClaimID )) > 0 )
                                        BEGIN
                                            --If the claim falls under different contract by satisfying contract conditions and same claim is reatined for different contract, then delete the
                                            --previous claim from @TempClaims and insert the value for the retained claim
                                            DELETE FROM @TempClaims
                                            WHERE ClaimID IN( SELECT TOP ( @RemainingClaimCount ) RC.ClaimID AS ClaimID
                                                              FROM RetainedClaims RC
                                                              WHERE RC.ContractID = @ContractID
                                                                AND NOT EXISTS( 
                                                                                SELECT CA.ClaimID
                                                                                FROM ContractAdjudications CA
                                                                                WHERE CA.ModelID = @PrimaryNodeID
                                                                                  AND CA.ClaimID = RC.ClaimID ));
                                            INSERT INTO @TempClaims
                                                   ( ContractID,
                                                     ClaimID
                                                   )
                                                   SELECT TOP ( @RemainingClaimCount ) @ContractID AS ContractID,
                                                                                       RC.ClaimID AS ClaimID
                                                   FROM RetainedClaims RC
                                                   WHERE RC.ContractID = @ContractID
                                                     AND NOT EXISTS( 
                                                                     SELECT CA.ClaimID
                                                                     FROM ContractAdjudications CA
                                                                     WHERE CA.ModelID = @PrimaryNodeID
                                                                       AND CA.ClaimID = RC.ClaimID );
                                        END;
                                END;
                            SELECT @TotalClaimsPicked = COUNT(*)
                            FROM @TempClaims;
                            -- Check if total claims picked meets batch size
                            IF @Totalclaimspicked >= @Batchsizeforbackgroundadjudication
                                BEGIN
                                    --Create background adjudication task
                                    EXEC dbo.AddBackgroundAdjudicationTask
                                         @FacilityID,
                                         @PrimaryNodeID,
                                         @TaskID OUTPUT;
                                    INSERT INTO TaskClaims
                                           ( ClaimID,
                                             TaskID,
                                             IsAdjudicated,
                                             IsPicked,
                                             RowID,
                                             AdjudicateContractID
                                           )
                                           SELECT DISTINCT ClaimID,
                                                           @TaskID,
                                                           0,
                                                           0,
                                                           ROW_NUMBER() OVER(ORDER BY TC.ClaimID ASC),
                                                           ContractID
                                           FROM @TempClaims TC;
                                    --Break the loop as max claims for background adjudication is reached
                                    BREAK;
                                END;
                            SET @Loopstart = @Loopstart + 1;
                        END;

                    -- Claims picked running through all contracts and is less than batch size
                    IF( @Totalclaimspicked >= 0 )
                  AND @Loopstart > @Contractcount
                        BEGIN
                            SET @ReadjudicateLoopStart = 1;
                            SET @IsReadjudicate = 1;
                            WHILE @ReadjudicateLoopStart <= @ContractCount
                                BEGIN
                                    SELECT @ContractID = ISNULL(ContractId, 0)
                                    FROM @TempContracts
                                    WHERE ISNULL(RowNumber, 0) = @ReadjudicateLoopStart;
                                    IF(( SELECT COUNT(*)
                                         FROM RetainedClaims RC
                                         WHERE RC.ContractID = @ContractID
                                           AND EXISTS( 
                                                       SELECT CA.ClaimID
                                                       FROM AdjudicatedClaimsContractID CA
                                                       WHERE CA.ModelID = @PrimaryNodeID
                                                         AND CA.ClaimID = RC.ClaimID
                                                         AND IsClaimAdjudicated = 0 )) > 0 )
                                        BEGIN
                                            INSERT INTO @TempClaims
                                                   ( ContractID,
                                                     ClaimID
                                                   )
                                                   SELECT TOP ( @RemainingClaimCount ) @ContractID AS ContractID,
                                                                                       RC.ClaimID AS ClaimID
                                                   FROM RetainedClaims RC
                                                   WHERE RC.ContractID = @ContractID
                                                     AND EXISTS( 
                                                                 SELECT CA.ClaimID
                                                                 FROM AdjudicatedClaimsContractID CA
                                                                 WHERE CA.ModelID = @PrimaryNodeID
                                                                   AND CA.ClaimID = RC.ClaimID
                                                                   AND IsClaimAdjudicated = 0 );
                                            SELECT @TotalClaimsPicked = COUNT(1)
                                            FROM @TempClaims;
                                        END;
                                    SELECT @MainQuery = dbo.GetFilterCodeQueryByContractID( @ContractID, @FacilityID, @PrimaryNodeID, @BatchSizeForBackgroundAdjudication, @TotalClaimsPicked, @IsReadjudicate );
                                    INSERT INTO @TempClaims
                                           ( ClaimId,
                                             ContractID
                                           )
                                    EXEC SYS.sp_executesql
                                         @MainQuery,
                                         N'@tempclaims ClaimContractID READONLY',
                                         @tempclaims;
                                    SELECT @TotalClaimsPicked = COUNT(*)
                                    FROM @TempClaims;
                                    SET @ReadjudicateLoopStart = @ReadjudicateLoopStart + 1;
                                END;
                            IF( @TotalClaimsPicked > 0
                            AND @ReadjudicateLoopStart > @Contractcount
                              )
                                BEGIN
                                    --Create background adjudication task
                                    EXEC dbo.AddBackgroundAdjudicationTask
                                         @FacilityID,
                                         @PrimaryNodeID,
                                         @TaskID OUTPUT;
                                    INSERT INTO TaskClaims
                                           ( ClaimID,
                                             TaskID,
                                             IsAdjudicated,
                                             IsPicked,
                                             RowID,
                                             AdjudicateContractID
                                           )
                                           SELECT DISTINCT ClaimID,
                                                           @TaskID,
                                                           0,
                                                           0,
                                                           ROW_NUMBER() OVER(ORDER BY TC.ClaimID ASC),
                                                           ContractID
                                           FROM @TempClaims TC;
                                END;
                        END;
                END;;
            IF @@Trancount > 0
                BEGIN
                    COMMIT TRANSACTION;
                END;
        END TRY
        BEGIN CATCH
            BEGIN
                ROLLBACK TRANSACTION;
                EXEC RaiseErrorInformation;
            END;
        END CATCH;
           
        --Return Task ID
        SELECT @TaskID;
    END;