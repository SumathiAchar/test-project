/****** Object:  UserDefinedTableType [dbo].[ClaimDataPrimaryPayerList]    Script Date: 8/4/2015 5:01:43 PM ******/

CREATE TYPE [dbo].[ClaimDataPrimaryPayerList] AS TABLE
([PayerName]             [VARCHAR](500) NULL,
 [Code]                  [VARCHAR](500) NULL,
 [ClaimCount]            [BIGINT] NULL,
 [ClaimTotal]            [MONEY] NULL,
 [FirstStatementThrough] [DATETIME] NULL,
 [FirstBilledDate]       [DATETIME] NULL
);
GO