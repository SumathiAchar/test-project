CREATE TABLE [dbo].[Contracts](
	[ContractID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[ContractName] [varchar](100) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[FacilityID] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsClaimStartDate] [bit] NOT NULL,
	[IsProfessionalClaim] [bit] NULL,
	[IsInstitutionalClaim] [bit] NULL,
	[IsModified] [bit] NULL,
	[NodeID] [bigint] NULL,
	[UserName] [varchar](100) NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsExpired] [bit] NOT NULL,
	[ThresholdDaysToExpireAlters] [int] NULL,
	[PayerCode] [varchar](500),
	[CustomField] [int]
PRIMARY KEY CLUSTERED 
(
	[ContractID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Contracts] ADD  CONSTRAINT [DF_Contracts_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Contracts] ADD  CONSTRAINT [DF_Contracts_IsExpired]  DEFAULT ((0)) FOR [IsExpired]
GO

ALTER TABLE [dbo].[Contracts] ADD  DEFAULT ((60)) FOR [ThresholdDaysToExpireAlters]
GO

ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD FOREIGN KEY([NodeID])
REFERENCES [dbo].[ContractHierarchy] ([NodeID])
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_Contracts_NodeID_IsDeleted]
ON [dbo].[Contracts] ([NodeID],[IsDeleted])
GO