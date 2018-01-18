CREATE TABLE [dbo].[ref.ClaimField](
	[ClaimFieldID] [bigint] NOT NULL,
	[Text] [varchar](100) NULL,
	[IsClaimField] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ClaimFieldID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ref.ClaimField] ADD  CONSTRAINT [DF_ref.ClaimField_ClaimFieldID]  DEFAULT ((1)) FOR [ClaimFieldID]
GO
ALTER TABLE [dbo].[ref.ClaimField] ADD  CONSTRAINT [DF_ref.ClaimField_IsClaimField]  DEFAULT ((1)) FOR [IsClaimField]
