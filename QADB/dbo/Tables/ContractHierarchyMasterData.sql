CREATE TABLE [dbo].[ContractHierarchyMasterData](
	[NodeID] [bigint] NOT NULL,
	[ParentID] [bigint] NOT NULL,
	[NodeText] [varchar](100) NULL,
	[IsPrimaryNode] [bit] NULL,
	[FacilityID] [bigint] NULL,
 CONSTRAINT [PK_ContractHierarchyMasterData] PRIMARY KEY CLUSTERED 
(
	[NodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]