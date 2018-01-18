/****************************************************/  
--Method Name : DeleteTaskClaimDataBasedOnTaskID
--Module      : Adjudication  
--ALTER d By  : Vishesh  
--Date        : 20-Dec-2013  
--Modified By : 
--Modify Date : 
--Description : Delete Task Claim data based on Task ID

--EXEC DeleteTaskClaimDataBasedOnTaskID 42
/****************************************************/  
CREATE PROCEDURE [dbo].[DeleteTaskClaimDataBasedOnTaskID]  
(
@TaskID BIGINT
)  
AS  
BEGIN 
	DELETE TaskClaim WHERE TaskID= @TaskID 
END






GO
