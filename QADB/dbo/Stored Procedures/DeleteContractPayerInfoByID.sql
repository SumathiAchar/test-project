CREATE PROCEDURE [dbo].[DeleteContractPayerInfoByID]   
   (@contractPayerInfoID BIGINT,
    @ContractId BIGINT,
    @UserName Varchar(50)
   )  
AS 
BEGIN 
	DECLARE @ContactName VARCHAR(100) = (SELECT ContractInfoName FROM ContractInfo WHERE ContractInfoID = @contractPayerInfoID)  
	DECLARE @Description VARCHAR(100) = ('Deleted Contact : ' + @ContactName)
	DELETE FROM ContractInfo   
	WHERE  ContractInfoID = @contractPayerInfoID 
	--Insert AuditLog information 
	EXEC InsertAuditLog @UserName, 'Delete', 'Contract', @Description ,@ContractId, 1
END  
