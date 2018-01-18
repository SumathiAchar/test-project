CREATE PROCEDURE [dbo].[GetAllStopLossConditions]  
  
AS  
/****************************************************************************  
 *   Name         : GetAllStopLossConditions  
 *   Author       : Prasad  
 *   Date         : 12/Feb/2014  
 *   Module       : Get All StopLoss Conditions  
 *   Description  : Get All StopLoss Conditions Information from database  
 -- EXEC [dbo].[GetAllStopLossConditions]
 *****************************************************************************/  
BEGIN  
  
 SELECT StopLossConditionID,StopLossConditionName  FROM [dbo].[ref.StopLossCondition]
  
END

GO
