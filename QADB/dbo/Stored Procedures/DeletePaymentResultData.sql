/****************************************************/

--Method Name : DeletePaymentResultData
--Module      : Adjudication  
--Created By  : Raj  
--Date        : 2-Aug-2014
--Modified By : 
--Modify Date : 
--Description : Delete Payment Result Data from ContractAdjudication table
--EXEC dbo.DeletePaymentResultData 113285,'123,134'

/****************************************************/

CREATE PROCEDURE [dbo].[DeletePaymentResultData](@TaskID   BIGINT,
                                                @ClaimIDs VARCHAR(MAX))
AS
     SET NOCOUNT ON;
     BEGIN
         DECLARE @ModelID BIGINT, @RunningStatus INT;

         --Get model id & running status
         SELECT @ModelID = ModelID,
                @RunningStatus = RunningStatus
         FROM dbo.TrackTasks WITH (NOLOCK)
         WHERE TaskID = @TaskID;

         -- Check status if its paused or not (130=pause)
         IF(@RunningStatus NOT IN(130))
             BEGIN
                 DECLARE @ClaimTable TABLE(ClaimID BIGINT);
                 INSERT INTO @ClaimTable
                        SELECT *
                        FROM dbo.split(@ClaimIDs, ',');
                 DECLARE @ContractAdjudicationIds TABLE(ContractAdjudicationId BIGINT PRIMARY KEY);
                 INSERT INTO @ContractAdjudicationIds
                        SELECT CA.ContractAdjudicationId
                        FROM [dbo].[ContractAdjudications] CA WITH (NOLOCK)
                             JOIN @ClaimTable AS CT ON CT.ClaimID = CA.ClaimID
                        WHERE CA.ModelID = @ModelID;
                 DELETE sb
                 FROM SmartBox AS sb WITH (NOLOCK)
                      JOIN @ContractAdjudicationIds AS CA ON sb.ContractAdjudicationId = CA.ContractAdjudicationId;
                 DELETE CA
                 FROM [dbo].[ContractAdjudications] AS CA WITH (NOLOCK)
                      JOIN @ContractAdjudicationIds AS CAIds ON CAIds.ContractAdjudicationId = CA.ContractAdjudicationId;
             END;
         ELSE
             BEGIN
                 UPDATE TaskClaims
                   SET
                       IsPicked = -1
                 WHERE TaskID = @TaskID
                       AND CONVERT(VARCHAR(100), ClaimID) IN(@ClaimIDs);
             END;
         SELECT @RunningStatus;
     END;