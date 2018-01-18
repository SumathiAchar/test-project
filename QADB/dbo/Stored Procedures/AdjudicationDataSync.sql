CREATE PROCEDURE [dbo].[AdjudicationDataSync]
AS
  BEGIN
	
	DECLARE @Claimlist AS CLAIMLIST;

	INSERT INTO @Claimlist
	SELECT DISTINCT ClaimID from ContractAdjudicationsBridge
	WHERE EXISTS (SELECT ClaimId FROM ContractAdjudications)

  END


