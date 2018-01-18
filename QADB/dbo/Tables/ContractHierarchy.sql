CREATE TABLE [dbo].[ContractHierarchy](
	[NodeID] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentID] [bigint] NOT NULL,
	[NodeText] [varchar](100) NULL,
	[IsPrimaryNode] [bit] NULL,
	[FacilityID] [bigint] NULL,
	[UserName] [varchar](100) NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[NodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ContractHierarchy] ADD  CONSTRAINT [DF_ContractHierarchy_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractHierarchy_ParentID_IsDeleted_IsPrimaryNode_UserName]
    ON [dbo].[ContractHierarchy]([ParentID] ASC, [IsDeleted] ASC, [IsPrimaryNode] ASC, [UserName] ASC);
GO