CREATE TABLE [dbo].[AuditLogs](
	[AuditLogId] [int] IDENTITY(1,1) NOT NULL,
	[LoggedDate] [datetime] NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Action] [nchar](20) NOT NULL,
	[ObjectType] [nvarchar](50) NOT NULL,
	[FacilityName] [nvarchar](50) NOT NULL,
	[ModelName] [nvarchar](50) NULL,
	[ContractName] [nvarchar](150) NULL,
	[ServiceTypeName] [nvarchar](150) NULL,
	[Description] [nvarchar](MAX) NULL,
PRIMARY KEY CLUSTERED 
(
	[AuditLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



