CREATE TABLE [dbo].[McareLabFeeSched](
	[EffDate] [datetime] NOT NULL,
	[State] [varchar](5) NOT NULL,
	[Carrier] [varchar](10) NOT NULL,
	[Loc] [varchar](10) NOT NULL,
	[CarrierLoc] [varchar](10) NOT NULL,
	[HCPCS] [varchar](5) NOT NULL,
	[Modifier] [varchar](10) NOT NULL,
	[HCPCSMod] [varchar](20) NOT NULL,
	[Amount] [money] NOT NULL,
 CONSTRAINT [PK_McareLabFeeSched] PRIMARY KEY CLUSTERED 
(
	[EffDate] ASC,
	[CarrierLoc] ASC,
	[HCPCSMod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IDX_McareLabFeeSched_HCPCS_Carrier_Loc_Amount]
ON [dbo].[McareLabFeeSched] ([HCPCS])
INCLUDE ([Carrier],[Loc],[Amount])

GO