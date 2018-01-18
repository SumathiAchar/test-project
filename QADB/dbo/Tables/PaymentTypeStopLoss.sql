CREATE TABLE [dbo].[PaymentTypeStopLoss](
	[PaymentTypeDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[Threshold] [varchar](5000) NOT NULL,
	[Percentage] [float] NOT NULL,
	[Days] [varchar](200) NULL,
	[RevCode] [varchar](2000) NULL,
	[CPTCode] [varchar](2000) NULL,
	[StopLossConditionID] [int] NOT NULL,
	[IsExcessCharge] [bit] NOT NULL
 CONSTRAINT [PK_PaymentTypeStopLoss] PRIMARY KEY CLUSTERED 
(
	[PaymentTypeDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PaymentTypeStopLoss]  WITH CHECK ADD  CONSTRAINT [FK_PaymentTypeStopLoss_ref.StopLossCondition] FOREIGN KEY([StopLossConditionID])
REFERENCES [dbo].[ref.StopLossCondition] ([StopLossConditionID])
GO

ALTER TABLE [dbo].[PaymentTypeStopLoss] CHECK CONSTRAINT [FK_PaymentTypeStopLoss_ref.StopLossCondition]
GO
ALTER TABLE [dbo].[PaymentTypeStopLoss] ADD  CONSTRAINT [DF_PaymentTypeStopLoss_IsExcessCharge]  DEFAULT ((0)) FOR [IsExcessCharge]
