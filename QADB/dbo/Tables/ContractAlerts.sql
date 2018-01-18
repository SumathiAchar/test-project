CREATE TABLE [dbo].[ContractAlerts](
	[ContractAlertID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[ContractID] [bigint] NOT NULL,
	[IsDismissed] [bit] NULL,
	[EndDate] [datetime] NOT NULL,
	[IsVerified] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractAlertID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ContractAlerts]  WITH CHECK ADD FOREIGN KEY([ContractID])
REFERENCES [dbo].[Contracts] ([ContractID])
