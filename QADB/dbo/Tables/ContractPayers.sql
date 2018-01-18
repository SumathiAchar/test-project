CREATE TABLE [dbo].[ContractPayers](
	[ContractPayerID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[ContractID] [bigint] NOT NULL,
	[PayerName] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractPayerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ContractPayers]  WITH CHECK ADD FOREIGN KEY([ContractID])
REFERENCES [dbo].[Contracts] ([ContractID])
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractPayers_ContractID]
ON [dbo].[ContractPayers] ([ContractID])
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractPayers_PayerName]
ON [dbo].[ContractPayers] ([PayerName])
GO