CREATE PROCEDURE [dbo].[GetAllReviewedOption]  
  
AS  
/****************************************************************************  
 *   Name         : GetAllReviewedOption  
 *   Author       : YASHJEET  
 *   Date         : 11/NOV/2015
 *   Module       :	Get All Reviewed Option
 *   Description  : Get All Reviewed Option Information from database  
    --EXEC [dbo].[GetAllReviewedOption]
 *****************************************************************************/   
BEGIN  
  
 SELECT ReviewedOptionID,ReviewedOption FROM [ref.ReviewedOption] 
  
END 

GO
