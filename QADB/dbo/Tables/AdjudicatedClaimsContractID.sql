CREATE TABLE [dbo].[AdjudicatedClaimsContractID]
(
	[ClaimID] [bigint] NOT NULL,
	[ModelID] [bigint] NOT NULL,
	[LastAdjudicatedContractID] [bigint] NOT NULL,
	[IsClaimAdjudicated] [bit] NOT NULL
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [ClaimID_ModelID]
    ON [dbo].[AdjudicatedClaimsContractID]([ClaimID] ASC, [ModelID] ASC);
GO