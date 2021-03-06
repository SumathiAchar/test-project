CREATE PROC [dbo].[DeleteContractDocByID](
       @ContractDocID BIGINT,
       @UserName      VARCHAR(50),
	  @FacilityID BIGINT)
AS
    BEGIN
        DECLARE @TransactionName VARCHAR(100) = 'DeleteContractDocByID',
                @ContractID      BIGINT,
                @FileName        VARCHAR(150),
                @ClaimToolDesc   NVARCHAR(1000),
			 @FacilityName    VARCHAR(100),
			 @ContractName            VARCHAR(150),
                @ModelName               VARCHAR(50);
        --select contract id based on doc id
        BEGIN TRY
            BEGIN TRAN @TransactionName;
            SELECT @ContractID = ContractID,
                   @FileName = FileName
            FROM ContractDocs
            WHERE ContractDocID = @ContractDocID;
	  
	  
        --get facilityName
	   SELECT @FacilityName = NodeText
                FROM ContractHierarchy
                WHERE FacilityID = @FacilityID AND ParentID=0;

       SELECT @ModelName = dbo.GetModelNameByContractID( @ContractID);
	  SELECT @ContractName=ContractName from Contracts where ContractID=@ContractID;

            --Insert AuditLog information 
            SET @ClaimToolDesc = 'Deleted attachment: ' + @FileName;
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
                VALUES( GETUTCDATE(), @UserName, 'View', 'Contract', @FacilityName, @ModelName, @ContractName, NULL, @ClaimToolDesc );
            --delete contract doc 
            DELETE FROM ContractDocs
            WHERE ContractDocID = @ContractDocID;
            COMMIT TRANSACTION @TransactionName;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;
 GO
