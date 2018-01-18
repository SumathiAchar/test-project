CREATE TABLE [dbo].[PaymentTypeCustomTable](
	[PaymentTypeDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[ClaimFieldDocId] [bigint] NOT NULL,
	[ClaimFieldId] [bigint] NOT NULL,
	[Formula] [varchar](5000) NOT NULL,
	[MultiplierFirst] [varchar](5000) NULL,
	[MultiplierSecond] [varchar](5000) NULL,
	[MultiplierThird] [varchar](5000) NULL,
	[MultiplierFour] [varchar](5000) NULL,
	[MultiplierOther] [varchar](5000) NULL,
	[IsObserveServiceUnit] [bit] NOT NULL,
	[ObserveServiceUnitLimit] [varchar](5000) NULL,
	[IsPerDayOfStay] [bit] NOT NULL,
	[IsPerCode] [bit] NOT NULL,
 CONSTRAINT [PK_PaymentTypeCustomTool] PRIMARY KEY CLUSTERED 
(
	[PaymentTypeDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PaymentTypeCustomTable]  WITH CHECK ADD  CONSTRAINT [FK_PaymentTypeCustomTable_ClaimFieldDocs] FOREIGN KEY([ClaimFieldDocId])
REFERENCES [dbo].[ClaimFieldDocs] ([ClaimFieldDocID])
GO

ALTER TABLE [dbo].[PaymentTypeCustomTable] CHECK CONSTRAINT [FK_PaymentTypeCustomTable_ClaimFieldDocs]
GO

ALTER TABLE [dbo].[PaymentTypeCustomTable]  WITH CHECK ADD  CONSTRAINT [FK_PaymentTypeCustomTable_ref.ClaimField] FOREIGN KEY([ClaimFieldId])
REFERENCES [dbo].[ref.ClaimField] ([ClaimFieldID])
GO

ALTER TABLE [dbo].[PaymentTypeCustomTable] CHECK CONSTRAINT [FK_PaymentTypeCustomTable_ref.ClaimField]
GO

