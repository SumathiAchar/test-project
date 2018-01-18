CREATE TABLE [dbo].[PHIAccessAuditTrails]
    (
      [ID] [INT] IDENTITY(1, 1)
                 NOT NULL ,
      [UserAccessed] [UNIQUEIDENTIFIER] NOT NULL ,
      [UserName] [VARCHAR](200) NULL ,
      [ClaimsViewed] [VARCHAR](MAX) NULL ,
      [DateViewed] [DATETIME] NULL ,
      CONSTRAINT [PK_PHIAccessAuditTrail] PRIMARY KEY CLUSTERED ( [ID] ASC )
        WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
               IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
               ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
    )
ON  [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

