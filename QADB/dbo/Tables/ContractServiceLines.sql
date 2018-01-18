CREATE TABLE [dbo].[ContractServiceLines](
	[ContractServiceLineID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[IncludedCode] [varchar](max) NULL,
	[ExcludedCode] [varchar](max) NULL,
 CONSTRAINT [PK__Contract__7C472C8F1A248938] PRIMARY KEY CLUSTERED 
(
	[ContractServiceLineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]