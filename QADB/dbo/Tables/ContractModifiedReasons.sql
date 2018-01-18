CREATE TABLE [dbo].[ContractModifiedReasons](
	[ContractModifiedReasonID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContractModifiedReasonCodeID] [bigint] NOT NULL,
	[ContractID] [bigint] NOT NULL,
	[ReasonNotes] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractModifiedReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ContractModifiedReasons]  WITH CHECK ADD FOREIGN KEY([ContractID])
REFERENCES [dbo].[Contracts] ([ContractID])
GO

ALTER TABLE [dbo].[ContractModifiedReasons]  WITH CHECK ADD FOREIGN KEY([ContractModifiedReasonCodeID])
REFERENCES [dbo].[ref.ContractModifiedReasonCodes] ([ContractModifiedReasonCodeID])
GO

CREATE NONCLUSTERED INDEX [IDX_NCLS_ContractModifiedReasons_ContractID]
ON [dbo].[ContractModifiedReasons] ([ContractID])
GO