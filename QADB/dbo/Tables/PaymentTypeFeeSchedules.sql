CREATE TABLE [dbo].[PaymentTypeFeeSchedules](
	[PaymentTypeDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[FeeSchedule] [float] NULL,
	[NonFeeSchedule] [float] NULL,
	[ClaimFieldDocID] [bigint] NOT NULL,
	[IsObserveUnits] [bit]  DEFAULT 0 NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentTypeDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PaymentTypeFeeSchedules]  WITH CHECK ADD  CONSTRAINT [FK_PaymentTypeFeeSchedules_ClaimFieldDocs] FOREIGN KEY([ClaimFieldDocID])
REFERENCES [dbo].[ClaimFieldDocs] ([ClaimFieldDocID])
GO

ALTER TABLE [dbo].[PaymentTypeFeeSchedules] CHECK CONSTRAINT [FK_PaymentTypeFeeSchedules_ClaimFieldDocs]
GO