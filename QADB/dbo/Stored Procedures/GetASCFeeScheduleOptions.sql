/****************************************************/
  
--Method Name : GetASCFeeScheduleOptions  
--Module      : Payment Type ASC Fee Schedule
--Created By  : Sumathi
--Created Date: 16-July-2015
--Modified By :   
--Modify Date :  
--Description : Get selection options to the payment type ASC fee schedule

/****************************************************/


CREATE PROCEDURE [dbo].[GetASCFeeScheduleOptions] 
AS  
BEGIN  
SELECT * FROM [dbo].[ASCFeeScheduleOptions]
END 

GO
