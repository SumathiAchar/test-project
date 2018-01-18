CREATE TABLE [dbo].[ZipCarrierMap](
	[State] [varchar](10) NULL,
	[ZipCode] [varchar](10) NOT NULL,
	[Carrier] [varchar](10) NOT NULL,
	[Locality] [varchar](10) NULL,
	[RuralInd] [varchar](10) NULL,
	[LabCBLocality] [varchar](10) NULL,
	[RuralInd2] [varchar](10) NULL,
	[Plus4Flag] [int] NULL,
	[Year/Qtr] [int] NULL,
	[CarrierLoc] [varchar](10) NULL,
 CONSTRAINT [PK_ZipCarrierMap] PRIMARY KEY CLUSTERED 
(
	[ZipCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO