CREATE TABLE [dbo].[PaymentTypeASCFeeSchedules](
	[PaymentTypeDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[Primary] [float] NULL,
	[Secondary] [float] NULL,
	[Tertiary] [float] NULL,
	[Quaternary] [float] NULL,
	[Others] [float] NULL,
	[NonFeeSchedule] [FLOAT] NULL,
	[ClaimFieldDocID] [bigint] NOT NULL,
	[SelectedOption] int NOT NULL DEFAULT(1)
PRIMARY KEY CLUSTERED 
(
	[PaymentTypeDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PaymentTypeASCFeeSchedules]  WITH CHECK ADD  CONSTRAINT [FK_PaymentTypeASCFeeSchedules_ClaimFieldDocs] FOREIGN KEY([ClaimFieldDocID])
REFERENCES [dbo].[ClaimFieldDocs] ([ClaimFieldDocID])
GO

ALTER TABLE [dbo].[PaymentTypeASCFeeSchedules] CHECK CONSTRAINT [FK_PaymentTypeASCFeeSchedules_ClaimFieldDocs]
GO