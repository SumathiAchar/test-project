CREATE TABLE [dbo].[ClaimColumnOptions](
	[ClaimColumnOptionId] [int] IDENTITY(1,1) NOT NULL,
	[ColumnName] [varchar](200) NOT NULL,
 CONSTRAINT [PK_ClaimColumnOptions] PRIMARY KEY CLUSTERED 
(
	[ClaimColumnOptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

