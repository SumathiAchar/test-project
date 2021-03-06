--Method Name : DeleteContractByID
--Module      : Delete Contract By Node ID
--ALTER d By  : Vishesh
--Date        : 26-Aug-2013
--Modified By : 
--Modified Date  :
--Description Delete contracts

/****************************************************/

CREATE PROCEDURE [dbo].[DeleteContractByID](
       @NodeID     BIGINT,
       @IsContract BIT,
       @UserName   VARCHAR(50))
AS
    BEGIN
        SET NOCOUNT ON;
        DECLARE @CurrentDate   DATETIME,
                @ClaimToolDesc NVARCHAR(1000),
                @FacilityId    BIGINT,
                @IsPrimaryNode INT,
                @ModelName     VARCHAR(50);
        SET @CurrentDate = ( SELECT GETUTCDATE());
        DECLARE @TransactionName VARCHAR(100) = 'DeleteContractByID';
        BEGIN TRY
            BEGIN TRANSACTION @TransactionName;
            DECLARE @ContractId BIGINT;
            SELECT @ContractId = ContractId
            FROM Contracts
            WHERE NodeId = @NodeId;
            IF @ContractId IS NOT NULL
                BEGIN
                    UPDATE Contracts
                      SET IsDeleted = 1
                    WHERE ContractID = @ContractId;
                    UPDATE ContractHierarchy
                      SET IsDeleted = 1
                    WHERE NodeID = @NodeID;
                    --Insert AuditLog information 
                    SELECT @UserName = UserName,
                           @ClaimToolDesc = 'Deleted contract:  ' + ContractName
                    FROM Contracts
                    WHERE ContractID = @ContractId;
                    IF( @IsContract = 1 )
                        BEGIN
                            EXEC InsertAuditLog
                                 @UserName,
                                 'Delete',
                                 'Contract',
                                 @ClaimToolDesc,
                                 @ContractId,
                                 1;
                        END;
                END;
            ELSE
                BEGIN
                    UPDATE ContractHierarchy
                      SET IsDeleted = 1
                    WHERE NodeID = @NodeID;

                    --INSERT INTO AUDIT LOG--

                    SELECT @FacilityId = FacilityID,
                           @ModelName = NodeText,
                           @IsPrimaryNode = IsPrimaryNode
                    FROM ContractHierarchy
                    WHERE NodeId = @NodeId
                      AND IsDeleted = 1;
                    IF( @IsContract = 0
                    AND @IsPrimaryNode = 0
                      )
                        BEGIN
                            INSERT INTO DBO.AuditLogs
                                   ( LoggedDate,
                                     UserName,
                                     ACTION,
                                     ObjectType,
                                     FacilityName,
                                     ModelName,
                                     ContractName,
                                     ServiceTypeName,
                                     Description
                                   )
                            VALUES( GETUTCDATE(), @UserName, 'Delete', 'Model', ( SELECT NodeText
                                                                                  FROM ContractHierarchy
                                                                                  WHERE FacilityID = @FacilityId
                                                                                    AND ParentID = 0 ), @ModelName, '', '', '' );
                        END;
                END;

            --Check Any Transaction happened than commit transaction
            COMMIT TRANSACTION @TransactionName;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;
GO