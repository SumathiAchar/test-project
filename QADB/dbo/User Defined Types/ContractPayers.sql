/****** Object:  UserDefinedTableType [dbo].[ContractPayers]    Script Date: 8/4/2015 5:01:43 PM ******/

CREATE TYPE [dbo].[ContractPayers] AS TABLE
([RowId]        [INT] IDENTITY(1, 1)
                      NOT NULL,
 [ModelName]    [VARCHAR](500) NULL,
 [ContractID]   [BIGINT] NULL,
 [ContractName] [VARCHAR](500) NULL,
 [PayerName]    [VARCHAR](MAX) NULL,
 [IsActive]     [BIT] NULL,
 [PayerCode]    [VARCHAR](500) NULL
);