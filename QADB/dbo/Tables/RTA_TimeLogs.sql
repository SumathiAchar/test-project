
CREATE TABLE [dbo].[RTA_TimeLogs](
	[LogID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[TimeTaken] [bigint] NOT NULL,
	[RequestType] [varchar](30) NULL,
	[EdiResponseID] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RTA_TimeLogs]  WITH CHECK ADD  CONSTRAINT [FK_RTA_TimeLogs_RTA_EdiResponses] FOREIGN KEY([EdiResponseID])
REFERENCES [dbo].[RTA_EdiResponses] ([ID])
GO
