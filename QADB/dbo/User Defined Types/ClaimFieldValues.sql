CREATE TYPE [dbo].[ClaimFieldValues] AS TABLE(
	[InsertDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[FacilityID] [bigint] NULL,
	[ClaimFieldDocID] [bigint] NULL,
	[Identifier] [varchar](max) NULL,
	[Value] [varchar](max) NULL
)
GO
