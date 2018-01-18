
CREATE PROCEDURE [dbo].[GetAllClaimFieldOperators]  
  
AS  
/****************************************************************************  
 *   Name         : GetAllClaimFieldOperators  
 *   Author       : Vishesh  
 *   Date         : 19/Aug/2013  
 *   Module       : Get All ClaimField Operators  
 *   Description  : Get AllClaimField Operators Information from database  
 --EXEC [dbo].[GetAllClaimFieldOperators]
 *****************************************************************************/   
BEGIN  
  
 SELECT OperatorID,OperatorType  FROM [ref.ClaimFieldOperators]  
  
END 







GO
