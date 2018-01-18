CREATE FUNCTION [dbo].[GetPayerCodeQueryByContractID] 
(
	@ContractID BIGINT
)
RETURNS NVARCHAR(MAX)
AS

BEGIN
DECLARE @PayerQuery NVARCHAR(MAX)=''
DECLARE @FilterCodeQuery NVARCHAR(MAX)=''
DECLARE @UdfTextValue NVARCHAR(MAX) = '';

SET @UdfTextValue = (SELECT CASE WHEN C.CustomField = 29 THEN 
							'CD.CustomField1'
					   ELSE CASE WHEN C.CustomField = 30 THEN 
							'CD.CustomField2'
				       ELSE CASE WHEN C.CustomField = 31 THEN 
							'CD.CustomField3'
				       ELSE CASE WHEN C.CustomField = 32 THEN 
							'CD.CustomField4'
				       ELSE CASE WHEN C.CustomField = 33 THEN 
							'CD.CustomField5'
				       ELSE CASE WHEN C.CustomField = 34 THEN 
							'CD.CustomField6'
				       ELSE ''
					   END
					   END
					   END
					   END
					   END
					   END
				FROM Contracts C WHERE ContractId = @ContractID)

SELECT @FilterCodeQuery = [dbo].[GetFilterCodeQuery] ((SELECT TOP 1 PayerCode FROM Contracts WHERE Contractid= @ContractID),(@UdfTextValue),'=',
	(CONVERT(VARCHAR(MAX), (SELECT CustomField FROM CONTRACTS WHERE CONTRACTID = @ContractID))));

SET @PayerQuery =
  CASE WHEN ((SELECT PayerCode FROM Contracts WHERE ContractID = @ContractID) IS NOT NULL AND (SELECT TOP 1 PayerName FROM DBO.CONTRACTPAYERS CP WHERE ContractID =@ContractID) IS NOT NULL) 
            THEN 'EXISTS (SELECT PayerName FROM DBO.CONTRACTPAYERS CP WHERE ContractID = ' + CAST(@ContractID AS VARCHAR(100)) + ' AND REPLACE(CP.PayerName, CHAR(160), CHAR(32)) = REPLACE(CD.PriPayerName, CHAR(160), CHAR(32))) AND ' +  @FilterCodeQuery
       WHEN (SELECT TOP 1 PayerCode FROM Contracts WHERE ContractID = @ContractID) IS NOT NULL
			THEN 
				@FilterCodeQuery
       WHEN  (SELECT TOP 1 PayerName FROM DBO.CONTRACTPAYERS CP WHERE ContractID = @ContractID) IS NOT NULL
			THEN 'EXISTS (SELECT PayerName FROM DBO.CONTRACTPAYERS CP WHERE ContractID = ' + CAST(@ContractID AS VARCHAR(100)) + ' AND REPLACE(CP.PayerName, CHAR(160), CHAR(32)) = REPLACE(CD.PriPayerName, CHAR(160), CHAR(32)))'
  END 
  RETURN @PayerQuery
END
