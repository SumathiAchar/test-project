CREATE PROCEDURE [dbo].[GetAllServiceLinesByContractID]  
(  
    @ContractID BIGINT  
)  
AS  
/****************************************************************************  
 *   Name         : GetAllServiceLinesByContractId  
 *   Author       : Vishesh  
 *   Date         : 13/Aug/2013  
 *   Module       : ContractServiceLines  
 *   Description  : Get All ContractServiceLines Information from database
 --EXEC [dbo].[GetAllServiceLinesByContractID] 11427
 *****************************************************************************/  
BEGIN  
SELECT cs.ContractServiceLineID,cs.InsertDate,
cs.UpdateDate,
ServiceLineTypeID,
ContractServiceTypeID,
csps.ContractID,
FacilityID,
IncludedCode,
'' [Description],
c.IsActive [Status],
ExcludedCode  
FROM ContractServiceLines cs
INNER JOIN ContractServiceLinePaymentTypes  csps ON csps.ContractServiceLineID = cs.ContractServiceLineId
INNER JOIN Contracts c ON c.ContractID=csps.ContractID
WHERE c.ContractID = @ContractID 

END  








GO
