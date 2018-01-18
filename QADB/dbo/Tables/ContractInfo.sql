CREATE TABLE [dbo].[ContractInfo](
	[ContractInfoID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[ContractID] [bigint] NOT NULL,
	[ContractInfoName] [varchar](100) NULL,
	[MailAddress1] [varchar](100) NULL,
	[MailAddress2] [varchar](100) NULL,
	[City] [varchar](100) NULL,
	[State] [varchar](100) NULL,
	[Zip] [char](10) NULL,
	[Phone1] [varchar](12) NULL,
	[Phone2] [varchar](12) NULL,
	[Fax] [varchar](12) NULL,
	[Email] [varchar](100) NULL,
	[Website] [varchar](100) NULL,
	[TaxID] [varchar](12) NULL,
	[NPI] [varchar](12) NULL,
	[Memo] [varchar](100) NULL,
	[ProvderID] [varchar](100) NULL,
	[PlanID] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractInfoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ContractInfo]  WITH CHECK ADD FOREIGN KEY([ContractID])
REFERENCES [dbo].[Contracts] ([ContractID])
GO
/****** Object:  Index [IDX_FK__ContractI__Contr__5B0E7E4A]    Script Date: 1/29/2014 7:34:52 PM ******/
CREATE NONCLUSTERED INDEX [IDX_FK__ContractI__Contr__5B0E7E4A] ON [dbo].[ContractInfo]
(
	[ContractID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]