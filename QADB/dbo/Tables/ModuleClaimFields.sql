CREATE TABLE [dbo].[ref.ModuleClaimFields](
	[ModuleId] [int] NOT NULL,
	[ClaimFieldId] [bigint] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ref.ModuleClaimFields]  WITH CHECK ADD  CONSTRAINT [FK_ModuleClaimFields_ref.ClaimField] FOREIGN KEY([ClaimFieldId])
REFERENCES [dbo].[ref.ClaimField] ([ClaimFieldID])
GO

ALTER TABLE [dbo].[ref.ModuleClaimFields] CHECK CONSTRAINT [FK_ModuleClaimFields_ref.ClaimField]
GO

ALTER TABLE [dbo].[ref.ModuleClaimFields]  WITH CHECK ADD  CONSTRAINT [FK_ModuleClaimFields_ref.Modules] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[ref.Modules] ([ModuleId])
GO

ALTER TABLE [dbo].[ref.ModuleClaimFields] CHECK CONSTRAINT [FK_ModuleClaimFields_ref.Modules]
GO