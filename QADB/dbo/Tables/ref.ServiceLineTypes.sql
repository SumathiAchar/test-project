CREATE TABLE [dbo].[ref.ServiceLineTypes](
	[ServiceLineTypeID] [bigint] NOT NULL,
	[ServiceLineName] [varchar](100) NULL,
	[ClaimFieldId] INT DEFAULT ((0)) NULL,
PRIMARY KEY CLUSTERED 
(
	[ServiceLineTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]