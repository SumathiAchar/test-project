
/****************************************************/    
--Created By  : Manjunath.B  
--Create Date : 7-Oct-2013  
--Modified By : Raj
--Modified Date : 14-Oct-2013
--Description : Get Claim Date By ID
/****************************************************/   
CREATE PROCEDURE dbo.GetClaimDataByID 
(
@ClaimId BIGINT
)
AS
BEGIN
	-- Get Claimdata
	SELECT *	
	FROM Claimdata
	WHERE claimid = @ClaimId

	-- Get claimchargedata
	SELECT *
	FROM claimchargedata
	WHERE claimid = @ClaimId

	-- Get ICDDData
	SELECT *
	FROM ICDDData
	WHERE claimid = @ClaimId

	-- Get ICDPData
	SELECT *
	FROM ICDPData
	WHERE claimid = @ClaimId

	-- Get ClaimPhysicianData
	SELECT *
	FROM ClaimPhysicianData
	WHERE claimid = @ClaimId

	-- Get InsuredData
	SELECT *
	FROM InsuredData
	WHERE claimid = @ClaimId

	-- Get ValueCodeData
	SELECT *
	FROM ValueCodeData
	WHERE claimid = @ClaimId

	-- Get OccurrenceCodeData
	SELECT *
	FROM OccurrenceCodeData
	WHERE claimid = @ClaimId

	-- Get ConditionCodeData
	SELECT *
	FROM ConditionCodeData
	WHERE claimid = @ClaimId
		
END
GO
