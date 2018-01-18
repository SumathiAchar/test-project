CREATE TABLE [dbo].[MedicareIPAcuteOptions](
	[MedicareIPAcuteOptionID] [int] IDENTITY(1,1) NOT NULL,
	[MedicareIPAcuteOptionCode] [varchar](50) NOT NULL,
	[MedicareIPAcuteOptionName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_MedicareIPAcuteOptions] PRIMARY KEY CLUSTERED 
(
	[MedicareIPAcuteOptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO