CREATE TABLE [dbo].[ActiveSSINumbers](
	[ssinumber] [bigint] NOT NULL,
	[claims] [int] NULL,
	[remits] [int] NULL,
	[acceptsRTA] [int] NULL,
	[Description] [varchar](100) NULL,
	[earliestLoadDate] [datetime] NULL,
	[enableAlerts] [int] NULL,
 CONSTRAINT [PK_activeSSINumbers] PRIMARY KEY CLUSTERED 
(
	[ssinumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
