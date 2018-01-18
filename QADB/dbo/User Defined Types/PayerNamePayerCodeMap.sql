/****** Object:  UserDefinedTableType [dbo].[PayerNamePayerCodeMap]    Script Date: 8/14/2015 5:48:40 PM ******/

CREATE TYPE [dbo].[PayerNamePayerCodeMap] AS TABLE
([ModelName]       [VARCHAR](MAX) NULL,
 [ContractName]    [VARCHAR](MAX) NULL,
 [ContractId]      [BIGINT] NULL,
 [PayerName]       [VARCHAR](MAX) NULL,
 [PayerCode]       [VARCHAR](500) NULL,
 [CustomField]     INT NULL,
 [CustomFieldText] VARCHAR(30) NULL,
 [IsActive]        [BIT] NULL
);