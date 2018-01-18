CREATE TABLE [dbo].[ClaimReviewed](
	[ClaimID] [bigint] NOT NULL
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ClaimReviewed]
ON [dbo].[ClaimReviewed] (ClaimID)
GO
