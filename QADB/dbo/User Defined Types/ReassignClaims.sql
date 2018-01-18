CREATE TYPE [dbo].[ReassignClaims] AS TABLE
(
	ClaimID BIGINT, 
	ModelID BIGINT,
	ContractID BIGINT,
	IsRetained BIT,
	IsSelected BIT
)
GO
