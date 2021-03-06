
/****************************************************************************  
 *   Name         : AddEditContractModifiedReason  1,4,'Test Notes'
 *   Author       : Vishesh  
 *   Date         : 13/Aug/2013  
 *   Module       : Add/Edit Alerts  
 *   Description  : Insert/Update Contract Modified Reason Information into database  
 *****************************************************************************/  
  
CREATE PROCEDURE [dbo].[AddEditContractModifiedReason]  
(  
    @ContractReasonCodeID BIGINT  
   ,@ContractID BIGINT  
   ,@Text VARCHAR(MAX)  
)  
AS  
  
BEGIN  

  DECLARE @TmpTable TABLE (InsertedId BIGINT)  
  --Insert Contract Modified Reason informations  
  INSERT INTO [dbo].[ContractModifiedReasons] (  
					[ContractModifiedReasonCodeID]  
				   ,[ContractID]  
				   ,[ReasonNotes]  
     )   
  OUTPUT inserted.ContractModifiedReasonID INTO @TmpTable 
  VALUES  
   (  
    @ContractReasonCodeID  
   ,@ContractID  
   ,@Text  
   )  

 SELECT * FROM @TmpTable  
  
END  







GO
