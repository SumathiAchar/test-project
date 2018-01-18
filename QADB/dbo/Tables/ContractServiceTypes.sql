CREATE TABLE [dbo].[ContractServiceTypes](
	[ContractServiceTypeID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[ContractID] [bigint] NOT NULL,
	[ContractServiceTypeName] [varchar](100) NOT NULL,
	[Notes] [varchar](max) NULL,
	[IsCarveOut] [bit] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractServiceTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ContractServiceTypes]  WITH CHECK ADD FOREIGN KEY([ContractID])
REFERENCES [dbo].[Contracts] ([ContractID])
GO
ALTER TABLE [dbo].[ContractServiceTypes] ADD  CONSTRAINT [DF_ContractServiceTypes_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractServiceTypes_ContractID]
    ON [dbo].[ContractServiceTypes]([ContractID] ASC)
    INCLUDE([ContractServiceTypeID], [IsDeleted], [ContractServiceTypeName], [IsCarveOut]);
GO
