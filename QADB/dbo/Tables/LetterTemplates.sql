CREATE TABLE [dbo].[LetterTemplates](
	[LetterTemplateID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[TemplateText] [nvarchar](max) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UserName] [varchar](100) NULL,
	[FacilityId] BIGINT NULL, 
    CONSTRAINT [PK_LetterTemplates] PRIMARY KEY CLUSTERED 
(
	[LetterTemplateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO