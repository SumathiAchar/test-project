CREATE TABLE [dbo].[ContractDocs](
	[ContractDocID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[ContractID] [bigint] NOT NULL,
	[FileName] [varchar](100) NULL,
	[ContractContent] [varbinary](max) NULL,
[DocumentId] UNIQUEIDENTIFIER NULL, 
    PRIMARY KEY CLUSTERED 
(
	[ContractDocID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ContractDocs]  WITH CHECK ADD FOREIGN KEY([ContractID])
REFERENCES [dbo].[Contracts] ([ContractID])
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractDocs_ContractID]
ON [dbo].[ContractDocs] ([ContractID])
GO