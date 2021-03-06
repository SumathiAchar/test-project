﻿CREATE TABLE [dbo].[ref.Modules](
	[ModuleId] [int] NOT NULL,
	[ModuleName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ref.Modules] PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
