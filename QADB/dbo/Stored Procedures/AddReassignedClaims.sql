CREATE PROCEDURE [dbo].[AddReassignedClaims](
       @UserName       VARCHAR(100),
       @ReassignClaims REASSIGNCLAIMS READONLY )
AS
    BEGIN
        DECLARE @TransactionName VARCHAR(100) = 'AddReassignedClaims',
                @LogText         VARCHAR(MAX) = '',
                @ContractCount   INT,
                @LogLoop         INT = 1,
                @ContractName    VARCHAR(100),
                @ContractID      BIGINT;
        DECLARE @ClaimDetails TABLE( [ACTION]   VARCHAR(100),
                                     ClaimID    BIGINT,
                                     ContractID BIGINT,
                                     ID         INT IDENTITY(1, 1));
        DECLARE @PatAccountNumbers TABLE( PatAcctNum VARCHAR(MAX),
                                          ID         INT IDENTITY(1, 1));
        DECLARE @Contracts TABLE( ContractID BIGINT,
                                  ID         INT IDENTITY(1, 1));
        BEGIN TRY
            BEGIN TRAN @TransactionName;
		        
            --- Insert in to RetainedClaims.
            MERGE RetainedClaims RC
            USING @ReassignClaims TRC
            ON RC.ClaimID = TRC.ClaimID
            WHEN MATCHED AND TRC.IsRetained = 0
                  THEN DELETE
            WHEN NOT MATCHED AND TRC.IsRetained = 1
                  THEN INSERT( ClaimID,
                               ModelID,
                               ContractID ) VALUES( TRC.ClaimID, TRC.ModelID, TRC.ContractID )
            WHEN MATCHED AND( TRC.ContractID != RC.ContractID
                          AND TRC.IsRetained = 1
                            )
                  THEN UPDATE SET RC.ContractID = TRC.ContractID
            OUTPUT $action,
                   ISNULL(inserted.ClaimID, deleted.ClaimID),
                   ISNULL(inserted.ContractID, deleted.ContractID)
                   INTO @ClaimDetails;
	    
            -- Inserting Distinct ClaimID in Temp Table 
            INSERT INTO @Contracts
                   SELECT DISTINCT( ContractID )
                   FROM @ReassignClaims WHERE ContractID > 0;
            SELECT @ContractCount = @@RowCount;
	   
            -- Starting loop to insert Audit log.
            WHILE @LogLoop <= @ContractCount
                BEGIN
	  
                    --Getting ContractID to insert in audit log.
                    SELECT @ContractID = ContractID
                    FROM @Contracts
                    WHERE ID = @LogLoop;      	   

                    --Insert into PatAcctNum in Temp Tables.
                    INSERT INTO @PatAccountNumbers
                           SELECT CD.PatAcctNum
                           FROM [dbo].[ClaimData] CD
                                INNER JOIN @ClaimDetails ClaimDetails ON ClaimDetails.ClaimID = CD.ClaimID
                                INNER JOIN @Contracts Contracts ON Contracts.ContractID = ClaimDetails.ContractID
                           WHERE ClaimDetails.ACTION = 'INSERT'
                             AND Contracts.ID = @LogLoop;

                    -- Checking if any Claims is mark for Retained
                    IF(@@RowCount > 0 )
                        BEGIN
                            SELECT @LogText = @LogText + COALESCE(CAST(PatAcctNum AS VARCHAR(MAX)) + ',', '')
                            FROM @PatAccountNumbers;
                            SELECT @LogText = LEFT(@LogText, DATALENGTH(@LogText) - 1);
                            SET @LogText = 'Linked claims: ' + @LogText;
                            EXEC InsertAuditLog
                                 @UserName,
                                 'Modify',
                                 'Claims',
                                 @LogText,
                                 @ContractID,
                                 1;
                        END;
                    DELETE FROM @PatAccountNumbers;
                    SET @LogText = '';
                    INSERT INTO @PatAccountNumbers
                           SELECT CD.PatAcctNum
                           FROM [dbo].[ClaimData] CD
                                INNER JOIN @ClaimDetails Temp ON Temp.ClaimID = CD.ClaimID
                                INNER JOIN @Contracts Contracts ON Contracts.ContractID = Temp.ContractID
                           WHERE Temp.ACTION = 'DELETE'
                             AND Contracts.ID = @LogLoop;
                    IF(@@RowCount > 0 )
                        BEGIN
                            SELECT @LogText = @LogText + COALESCE(CAST(PatAcctNum AS VARCHAR(MAX)) + ',', '')
                            FROM @PatAccountNumbers;
                            SELECT @LogText = LEFT(@LogText, DATALENGTH(@LogText) - 1);
                            SET @LogText = 'UnLinked claims: ' + @LogText;
                            EXEC InsertAuditLog
                                 @UserName,
                                 'Modify',
                                 'Claims',
                                 @LogText,
                                 @ContractID,
                                 1;
                        END;
                    DELETE FROM @PatAccountNumbers;
                    SET @LogText = '';
                    SET @LogLoop = @LogLoop + 1;
                END;
            COMMIT TRANSACTION @TransactionName;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;