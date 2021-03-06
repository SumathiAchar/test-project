
/****************************************************/
  
--Method Name : [GetAllPayersByContractID]  
--Module      : Report module
--ALTER d By  : Vishesh 
--Modified By :   
--ModifiedDATE:  
--Date        : 02-Sep-2013  
--Description : Get All payers by contract ID

/****************************************************/

CREATE FUNCTION dbo.GetQuotedPayersByContractID
(
	@ContractId BIGINT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @ValueStringData varchar(MAX)   
	SELECT @ValueStringData = COALESCE(@ValueStringData + ',', '') + '''' + replace(CP.PayerName,'''', '''''') + ''''
	FROM (SELECT DISTINCT PayerName FROM dbo.ContractPayers WITH (NOLOCK) 
	WHERE ContractId = @ContractId) AS CP
	RETURN @ValueStringData;
END;