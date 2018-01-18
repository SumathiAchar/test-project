CREATE TABLE [dbo].[TrackTasks](
	[TaskID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[RequestName] [varchar](100) NULL,
	[IsUserDefined] [bit] NOT NULL,
	[RunningStatus] [int] NOT NULL,
	[Priority] [int] NULL,
	[SelectCriteria] [varchar](max) NULL,
	[FacilityID] [bigint] NULL,
	[ModelID] [bigint] NULL,
	[DateType] [int] NULL,
	[DateFrom] [datetime] NULL,
	[DateTo] [datetime] NULL,
	[IdJob] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[UserName] [varchar](100) NULL,
	[IsLastProcessedDate] [bit] NULL,
	[TotalClaimCount] [bigint] NOT NULL,
	[AdjudicatedClaimCount] BIGINT NOT NULL, 
	[IsDataPickedForAdjudication] INT NULL,
	[LastUpdatedDateForElapsedTime] [datetime] NULL,
	[ElapsedTime] [bigint] NULL,
	[IsVerified] [bit] NULL,
    PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[TrackTasks] ADD  CONSTRAINT [DF_TrackTasks_TotalClaimCount]  DEFAULT ((0)) FOR [TotalClaimCount]
GO
ALTER TABLE [dbo].[TrackTasks] ADD  CONSTRAINT [DF_TrackTasks_AdjudicatedClaimCount]  DEFAULT ((0)) FOR [AdjudicatedClaimCount]
GO
ALTER TABLE [dbo].[TrackTasks] ADD CONSTRAINT [FK_TrackTasks_ModelID_NodeID] FOREIGN KEY ([ModelID]) REFERENCES [dbo].[ContractHierarchy] ([NodeID])
GO