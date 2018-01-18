CREATE TABLE [dbo].[ClaimFieldDocs](
	[ClaimFieldDocID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[FacilityID] [bigint] NOT NULL,
	[FileName] [varchar](100) NULL,
	[TableName] [varchar](100) NULL,
	[ColumnHeaderFirst] [varchar](MAX) NULL,
	[ColumnHeaderSecond] [varchar](100) NULL,
	[ClaimFieldID] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[ClaimFieldDocID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[ClaimFieldDocs]  WITH CHECK ADD FOREIGN KEY([ClaimFieldID])
REFERENCES [dbo].[ref.ClaimField] ([ClaimFieldID])
GO
