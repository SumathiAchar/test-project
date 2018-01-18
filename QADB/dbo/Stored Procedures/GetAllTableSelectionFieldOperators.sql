CREATE PROCEDURE [dbo].[GetAllTableSelectionFieldOperators]  
  
AS  
/****************************************************************************  
 *   Name         : GetAllTableSelectionFieldOperators 
 *   Author       : Sumathi  
 *   Date         : 31/Mar/2015  
 *   Module       : Get Field Operators  
 *   Description  : Get fields operators required for Table selection  
 *****************************************************************************/  

BEGIN  
  
 SELECT OperatorID,OperatorType  FROM [ref.ClaimFieldOperators]  
 where OperatorType = '=' OR OperatorType = '<>' ORDER BY OperatorID DESC
END
GO
