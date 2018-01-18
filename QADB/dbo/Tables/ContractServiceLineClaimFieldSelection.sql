CREATE TABLE [dbo].[ContractServiceLineClaimFieldSelection](
	[ContractServiceLineID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[ClaimFieldID] [bigint] NOT NULL,
	[OperatorID] [int] NOT NULL,
	[Values] [varchar](5000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractServiceLineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ContractServiceLineClaimFieldSelection]  WITH CHECK ADD FOREIGN KEY([ClaimFieldID])
REFERENCES [dbo].[ref.ClaimField] ([ClaimFieldID])
GO
ALTER TABLE [dbo].[ContractServiceLineClaimFieldSelection]  WITH CHECK ADD FOREIGN KEY([OperatorID])
REFERENCES [dbo].[ref.ClaimFieldOperators] ([OperatorID])
GO
/****** Object:  Index [IDX_FK__ContractS__Claim__0BB1B5A5]    Script Date: 1/29/2014 7:34:52 PM ******/
CREATE NONCLUSTERED INDEX [IDX_FK__ContractS__Claim__0BB1B5A5] ON [dbo].[ContractServiceLineClaimFieldSelection]
(
	[ClaimFieldID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_FK__ContractS__Opera__0ABD916C]    Script Date: 1/29/2014 7:34:52 PM ******/
CREATE NONCLUSTERED INDEX [IDX_FK__ContractS__Opera__0ABD916C] ON [dbo].[ContractServiceLineClaimFieldSelection]
(
	[OperatorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]