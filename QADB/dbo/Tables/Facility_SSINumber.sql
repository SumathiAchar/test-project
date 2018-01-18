CREATE TABLE [dbo].[Facility_SSINumber](
	[FacilityID] [bigint] NOT NULL,
	[SSINumber] [bigint] NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_Facility_SSINumber] PRIMARY KEY CLUSTERED 
(
	[FacilityID] ASC,
	[SSINumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

