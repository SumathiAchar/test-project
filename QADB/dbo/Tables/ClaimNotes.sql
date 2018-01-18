CREATE TABLE [dbo].[ClaimNotes]
(
	[ClaimNoteID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[ClaimID] [bigint] NOT NULL,
	[ClaimNoteText] [varchar](100) NULL,
	[UserName] [nvarchar](100) NULL,
	[IsDeleted] [bit] Default 0
)
