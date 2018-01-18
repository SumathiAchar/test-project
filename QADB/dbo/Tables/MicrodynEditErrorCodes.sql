CREATE TABLE [dbo].[ref.MicrodynEditErrorCodes](
	[ErrorCode] [int] NOT NULL,
	[ErrorDescription] [varchar](1000) NOT NULL,
 CONSTRAINT [PK_ref.MicrodynErrorCodes] PRIMARY KEY CLUSTERED 
(
	[ErrorCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]