﻿CREATE TABLE [dbo].[RTA_EdiResponses](
	[ID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[EdiRequestID] [BIGINT] NOT NULL,
	[ResponseOutput] [VARCHAR](max) NOT NULL,
	[ResponseDateTime] [DATETIME] NOT NULL,
	[RTACodeID] [INT] NULL,
	[ResponseType] [VARCHAR](5) NOT NULL,
	[CalcAllowedAmt] [MONEY] NULL,
 CONSTRAINT [PK_RTArequest] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[RTA_EdiResponses]  WITH CHECK ADD  CONSTRAINT [FK_RTA_EdiResponses_RTA_EdiRequests] FOREIGN KEY([EdiRequestID])
REFERENCES [dbo].[RTA_EdiRequests] ([ID])
GO

ALTER TABLE [dbo].[RTA_EdiResponses]  WITH CHECK ADD  CONSTRAINT [FK_RTA_EdiResponses_RTA_Code] FOREIGN KEY([RTACodeID])
REFERENCES [dbo].[RTA_Code] ([ID])
GO