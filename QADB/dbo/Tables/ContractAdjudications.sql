CREATE TABLE [dbo].[ContractAdjudications](
	[ContractAdjudicationID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[ContractID] [bigint] NULL,
	[ContractServiceTypeID] [bigint] NULL,
	[ClaimID] [bigint] NOT NULL,
	[ClaimServiceLineID] INT NULL,
	[ClaimStatus] [int] NOT NULL,
	[AdjudicatedValue] [money] NULL,
	[ClaimTotalCharges] [money] NULL,
	[IsClaimChargeData] [bit] NULL,
	[ModelID] [bigint] NULL,
	[IsInitialEntry] BIT NOT NULL, 
    [PaymentTypeDetailID] BIGINT NULL, 
    [PaymentTypeID] INT NULL, 
	[MicrodynEditErrorCodes] VARCHAR(500),
	[MicrodynPricerErrorCodes] INT,
	[MicrodynEditReturnRemarks] VARCHAR(1000),
	[IsExported] BIT NULL DEFAULT 0
PRIMARY KEY CLUSTERED 
(
	[ContractAdjudicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
    [MedicareSequesterAmount] MONEY NULL
) ON [PRIMARY]
GO
/****** Object:  Index [IDX_ContractAdjudications_ContractAdjudicationID_ContractServiceTypeID]    Script Date: 1/29/2014 7:34:52 PM ******/
CREATE NONCLUSTERED INDEX [IDX_ContractAdjudications_ContractAdjudicationID_ContractServiceTypeID] ON [dbo].[ContractAdjudications]
(
	[ClaimID] ASC,
	[ModelID] ASC
)
INCLUDE ( 	[ContractAdjudicationID],
	[ContractID],
	[ContractServiceTypeID],
	[ClaimServiceLineID],
	[ClaimStatus],
	[AdjudicatedValue],
	[IsClaimChargeData]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ContractAdjudications] ADD  DEFAULT ((0)) FOR [IsInitialEntry]
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractAdjudications_ModelID]
ON [dbo].[ContractAdjudications] ([ModelID])
INCLUDE ([AdjudicatedValue],[ClaimTotalCharges])
WITH (ONLINE = ON)
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractAdjudications_ClaimId_ClaimServicelineId_ContractId_VarianceStatus]
ON [dbo].[ContractAdjudications] (ClaimId, ClaimServicelineId,ContractId)
WITH (ONLINE = ON)
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractAdjudications_ClaimStatus]
ON [dbo].[ContractAdjudications] (ClaimStatus)
WITH (ONLINE = ON)
GO

ALTER TABLE [dbo].[ContractAdjudications]  
WITH CHECK ADD CONSTRAINT [FK_ContractAdjudications_ContractHierarchy] FOREIGN KEY([ModelID])
REFERENCES [dbo].[ContractHierarchy] ([NodeID])
GO

ALTER TABLE [dbo].[ContractAdjudications] ADD CONSTRAINT [FK_ContractAdjudications_ContractServiceTypeID] FOREIGN KEY ([ContractServiceTypeID]) REFERENCES [dbo].[ContractServiceTypes] ([ContractServiceTypeID])
GO
ALTER TABLE [dbo].[ContractAdjudications] ADD CONSTRAINT [FK_ContractAdjudications_ContractID] FOREIGN KEY ([ContractID]) REFERENCES [dbo].[Contracts] ([ContractID])
GO
ALTER TABLE [dbo].[ContractAdjudications] ADD CONSTRAINT [FK_ContractAdjudications_ClaimStatus_StatusID] FOREIGN KEY ([ClaimStatus]) REFERENCES [dbo].[ref.AdjudicationOrVarianceStatuses] ([StatusID])
GO

CREATE NONCLUSTERED INDEX [IDX_ContractAdjudications_ContractServiceTypeID_ClaimServiceLineID_ModelID]
ON [dbo].[ContractAdjudications] ([ContractServiceTypeID],[ClaimServiceLineID],[ModelID])
INCLUDE ([ClaimID])
WITH (ONLINE = ON)
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractServiceType_ClaimServiceLine_Status_IsExported]
ON [dbo].[ContractAdjudications] ([ContractServiceTypeID],[ClaimServiceLineID],[ClaimStatus],[IsExported])
INCLUDE ([ContractAdjudicationID],[ModelID])
WITH (ONLINE = ON)
GO