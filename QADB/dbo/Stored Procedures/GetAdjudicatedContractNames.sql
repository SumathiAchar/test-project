CREATE PROCEDURE [dbo].[GetAdjudicatedContractNames](@ModelID  BIGINT,
                                                     @UserText VARCHAR(20))
AS
/****************************************************/
--Method Name : GetAdjudicatedContractNames 
--Created By  : Shivakumar  
--Date        : 5-May-2016 
--Modified By :   
--Modified Date:   
--Description : Get Adjudicated ContractNames based on the selection of model, contract
--EXEC GetAdjudicatedContractNames  11672,''
/****************************************************/
BEGIN
         SELECT DISTINCT
                C.ContractID,
                CONVERT( VARCHAR(100), C.ContractName)+' '+CONVERT(VARCHAR(100), C.StartDate, 101)+'-'+CONVERT(VARCHAR(100), C.EndDate, 101) AS ContractStartEndDate
         FROM Contracts C
              INNER JOIN [dbo].[ContractAdjudications] CA ON C.ContractID = CA.ContractID
         WHERE CA.ModelID = @ModelID
               AND IsActive = 1
               AND IsDeleted = 0
               AND CONVERT(VARCHAR(100), C.ContractName)+' '+CONVERT(VARCHAR(100), C.StartDate, 101)+'-'+CONVERT(VARCHAR(100), C.EndDate, 101) LIKE '%'+@UserText+'%'
         ORDER BY CONVERT( VARCHAR(100), C.ContractName)+' '+CONVERT(VARCHAR(100), C.StartDate, 101)+'-'+CONVERT(VARCHAR(100), C.EndDate, 101);
     END;