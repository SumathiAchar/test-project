CREATE TABLE [dbo].[ClaimAvailableColumns](
	[ClaimAvailableColumnId] [int] IDENTITY(1,1) NOT NULL,
	[ClaimColumnOptionId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Order] [int] NOT NULL,
 CONSTRAINT [PK_ClaimAvailableColumns] PRIMARY KEY CLUSTERED 
(
	[ClaimAvailableColumnId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ClaimAvailableColumns]  WITH CHECK ADD  CONSTRAINT [FK_ClaimAvailableColumns_ClaimColumnOptions] FOREIGN KEY([ClaimColumnOptionId])
REFERENCES [dbo].[ClaimColumnOptions] ([ClaimColumnOptionId])
GO

ALTER TABLE [dbo].[ClaimAvailableColumns] CHECK CONSTRAINT [FK_ClaimAvailableColumns_ClaimColumnOptions]