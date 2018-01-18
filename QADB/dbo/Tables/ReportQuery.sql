CREATE TABLE [dbo].[ReportQuery](
	[ReportQueryId] [int] IDENTITY(1,1) NOT NULL,
	[FacilityId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[QueryName] [varchar](100) NOT NULL,
	[Criteria] [varchar](max) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[InsertDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_ReportQuery] PRIMARY KEY CLUSTERED 
(
	[ReportQueryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO



