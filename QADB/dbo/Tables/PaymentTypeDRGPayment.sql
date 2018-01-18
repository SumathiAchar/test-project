CREATE TABLE [dbo].[PaymentTypeDRGPayment](
	[PaymentTypeDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[BaseRate] [float] NULL,
	[ClaimFieldDocID] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentTypeDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PaymentTypeDRGPayment]  WITH CHECK ADD  CONSTRAINT [FK_PaymentTypeDRGPayment_ClaimFieldDocs] FOREIGN KEY([ClaimFieldDocID])
REFERENCES [dbo].[ClaimFieldDocs] ([ClaimFieldDocID])
GO

ALTER TABLE [dbo].[PaymentTypeDRGPayment] CHECK CONSTRAINT [FK_PaymentTypeDRGPayment_ClaimFieldDocs]
