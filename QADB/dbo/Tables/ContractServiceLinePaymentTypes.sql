CREATE TABLE [dbo].[ContractServiceLinePaymentTypes](
	[ContractServiceLinePaymentTypeId] [bigint] IDENTITY(1,1) NOT NULL,
	[PaymentTypeDetailID] [bigint] NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[PaymentTypeID] [bigint] NULL,
	[ContractServiceLineID] [bigint] NULL,
	[ContractID] [bigint] NULL,
	[ServiceLineTypeID] [bigint] NULL,
	[ContractServiceTypeID] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractServiceLinePaymentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ContractServiceLinePaymentTypes]  WITH CHECK ADD FOREIGN KEY([ContractID])
REFERENCES [dbo].[Contracts] ([ContractID])
GO
ALTER TABLE [dbo].[ContractServiceLinePaymentTypes]  WITH CHECK ADD FOREIGN KEY([ContractServiceTypeID])
REFERENCES [dbo].[ContractServiceTypes] ([ContractServiceTypeID])
GO
ALTER TABLE [dbo].[ContractServiceLinePaymentTypes]  WITH CHECK ADD FOREIGN KEY([PaymentTypeID])
REFERENCES [dbo].[ref.PaymentTypes] ([PaymentTypeID])
GO
ALTER TABLE [dbo].[ContractServiceLinePaymentTypes]  WITH CHECK ADD FOREIGN KEY([ServiceLineTypeID])
REFERENCES [dbo].[ref.ServiceLineTypes] ([ServiceLineTypeID])
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractServiceLinePaymentTypes_ContractID]
    ON [dbo].[ContractServiceLinePaymentTypes]([ContractID] ASC)
    INCLUDE([ContractServiceLineID], [ServiceLineTypeID], [ContractServiceTypeID]);
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractServiceLinePaymentTypes_PaymentTypeID]
    ON [dbo].[ContractServiceLinePaymentTypes]([PaymentTypeID] ASC)
    INCLUDE([PaymentTypeDetailID], [ContractID], [ContractServiceTypeID]);
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractServiceLinePaymentTypes_ServiceLineTypeID]
    ON [dbo].[ContractServiceLinePaymentTypes]([ServiceLineTypeID] ASC)
    INCLUDE([ContractServiceLineID], [ContractID], [ContractServiceTypeID]);
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractServiceLinePaymentTypes_ContractServiceLineID]
	ON [dbo].[ContractServiceLinePaymentTypes] ([ContractServiceLineID])
	INCLUDE ([ContractID],[ServiceLineTypeID],[ContractServiceTypeID])
GO
