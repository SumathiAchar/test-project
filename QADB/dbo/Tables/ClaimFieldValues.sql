CREATE TABLE [dbo].[ClaimFieldValues](
	[ClaimFieldValueID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[FacilityID] [bigint] NOT NULL,
	[ClaimFieldDocID] [bigint] NOT NULL,
	[Identifier] [varchar](100) NULL,
	[Value] [varchar](MAX) NULL,
PRIMARY KEY CLUSTERED 
(
	[ClaimFieldValueID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ClaimFieldValues]  WITH CHECK ADD FOREIGN KEY([ClaimFieldDocID])
REFERENCES [dbo].[ClaimFieldDocs] ([ClaimFieldDocID])
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ClaimFieldValues_FacilityId_ClaimFieldDocID]
ON [dbo].[ClaimFieldValues] ([FacilityId],[ClaimFieldDocID])
INCLUDE ([Identifier],[Value])
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ClaimFieldValues_ClaimFieldDocID]
ON [dbo].[ClaimFieldValues] ([ClaimFieldDocID])
INCLUDE ([ClaimFieldValueID])
GO