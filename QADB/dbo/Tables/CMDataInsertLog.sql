CREATE TABLE [dbo].[CMDataInsertLog](
[startTime] [datetime] NOT NULL,
	[endTime] [datetime] NOT NULL,
	[elapsedSeconds] [int] NULL,
	[claimsInserted] [int] NULL,
	[remitsInserted] [int] NULL,
	[SSInumber] [bigint] NULL,
	[totalGBUsed] [float] NULL,
	[message] [varchar](4000) NULL
) ON [PRIMARY]

GO
