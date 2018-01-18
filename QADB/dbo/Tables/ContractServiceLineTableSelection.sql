CREATE TABLE [dbo].[ContractServiceLineTableSelection](
	[ContractServiceLineID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[ClaimFieldId] [bigint] NULL,
	[ClaimFieldDocId] [bigint] NULL,
	[Operator] INT NULL
    PRIMARY KEY CLUSTERED 
(
	[ContractServiceLineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ContractServiceLineTableSelection]  WITH CHECK ADD FOREIGN KEY([ClaimFieldDocId])
REFERENCES [dbo].[ClaimFieldDocs] ([ClaimFieldDocID])

GO
ALTER TABLE [dbo].[ContractServiceLineTableSelection] ADD CONSTRAINT [FK_ContractServiceLineTableSelection_ClaimFieldId] FOREIGN KEY ([ClaimFieldId]) REFERENCES [dbo].[ref.ClaimField] ([ClaimFieldId])
GO