CREATE TABLE [dbo].[MedicareIPAcuteOptionChilds](
	[MedicareIPAcuteOptionChildID] [int] IDENTITY(1,1) NOT NULL,
	[MedicareIPAcuteOptionChildCode] [varchar](50) NOT NULL,
	[MedicareIPAcuteOptionChildName] [varchar](100) NOT NULL,
	[MedicareIPAcuteOptionID] [int] NOT NULL,
 CONSTRAINT [PK_MedicareIPAcuteOptionChilds] PRIMARY KEY CLUSTERED 
(
	[MedicareIPAcuteOptionChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[MedicareIPAcuteOptionChilds]  WITH CHECK ADD  CONSTRAINT [FK_MedicareIPAcuteOptionChilds_MedicareIPAcuteOptionChilds] FOREIGN KEY([MedicareIPAcuteOptionID])
REFERENCES [dbo].[MedicareIPAcuteOptions] ([MedicareIPAcuteOptionID])
GO

ALTER TABLE [dbo].[MedicareIPAcuteOptionChilds] CHECK CONSTRAINT [FK_MedicareIPAcuteOptionChilds_MedicareIPAcuteOptionChilds]
GO