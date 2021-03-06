CREATE  FUNCTION [dbo].[GetServiceLineCodesCount]      
(      
 @ServiceLineTypeId BIGINT      
) 
returns int     
AS      
/****************************************************************************        
 *   Name         : GetServiceLineCodesCount  3     
 *   Author       : mmachina        
 *   Date         : 23/Aug/2013        
 *   Module       : Get ServiceLine Codes        
 *   Description  : Get Codes  of  ServiceLines       
 *****************************************************************************/        
 BEGIN 
 DECLARE @CodeCount INT =0      
      
  ----ServiceLine BillTypeCodes data      
  IF @ServiceLineTypeId = 1      
   BEGIN      
    SELECT       
        @CodeCount=COUNT(Code)            
    FROM dbo.BillTypes          
   END      
      
   --ServiceLine RevenueCodes data      
   IF @ServiceLineTypeId = 2      
   BEGIN      
    SELECT       
        @CodeCount=COUNT(Code)           
    FROM dbo.RevenueCodes      
   END      
      
   --ServiceLine CPTCodes data      
  IF @ServiceLineTypeId = 3      
   BEGIN      
    SELECT     
        @CodeCount=COUNT(Code)          
    FROM dbo.HCPCScodes       
   END      
      
   --ServiceLine DRGCodes data      
  IF @ServiceLineTypeId = 4      
   BEGIN      
    SELECT      
       @CodeCount=COUNT(Code)          
    FROM dbo.DRGCodes      
   END      
      
   --ServiceLine DiagnosisCodes data      
  IF @ServiceLineTypeId = 5      
   BEGIN      
    SELECT      
       @CodeCount=COUNT(Code)        
    FROM dbo.DiagnosisCodes      
   END      
      
   --ServiceLine ProcedureCodes data      
  IF @ServiceLineTypeId = 6      
   BEGIN      
    SELECT      
      @CodeCount=COUNT(Code)      
    FROM dbo.ProcedureCodes    
   END      
      
     
      return @CodeCount;
END   







GO
