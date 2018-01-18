/****************************************************************************
 *   Name         : GetAllContractServiceTypes
 *   Author       : Raj
 *   Date         : 19/Aug/2013
 *   Module       : ServiceTypes
 *   Description  : Get all Contract Service Types' Id and Name
 --EXEC [dbo].[GetAllContractServiceTypes] 25513
 *****************************************************************************/
CREATE PROCEDURE  [dbo].[GetAllContractServiceTypes] 	 
(
	@ContractID BIGINT
) 
AS
BEGIN
	
		   SELECT [ContractServiceTypeId],[ContractServiceTypeName] FROM [dbo].[ContractServiceTypes] WHERE ContractID = @ContractID AND IsDeleted = 0
		   
END








GO
