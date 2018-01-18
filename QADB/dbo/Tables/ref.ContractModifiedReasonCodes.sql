CREATE TABLE [dbo].[ref.ContractModifiedReasonCodes](
	[ContractModifiedReasonCodeID] [bigint] NOT NULL,
	[Text] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractModifiedReasonCodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]