CREATE TABLE [dbo].[ref.AdjudicationOrVarianceStatuses](
	[StatusID] [int] NOT NULL,
	[StatusCode] [varchar](10) NOT NULL,
	[StatusDefinition] [varchar](100) NOT NULL,
	[IsVarianceStatus] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]