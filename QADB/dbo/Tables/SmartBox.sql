CREATE TABLE [dbo].[SmartBox](
	[SmartBoxId] [int] IDENTITY(1,1) NOT NULL,
	[ContractAdjudicationId] [bigint] NOT NULL,
	[ExpressionResult] [float] NULL,
	[CalculatedAlowedAmount] [float] NULL,
	[IsThresholdFormulaError] [bit] NULL,
	[Expression] [varchar](5000) NULL,
	[ExpandedExpression] [varchar](8000) NULL,
	[CustomExpression] [varchar](5000) NULL,
	[CustomExpandedExpression] [varchar](8000) NULL,
	[CustomExpressionResult] [float] NULL, 
	[IsMultiplierFormulaError]     BIT            DEFAULT ((0)) NOT NULL,
	[MultiplierExpandedExpression] [varchar](8000) NULL,
	[MultiplierExpressionResult] [float] NULL,
	[Multiplier] [int] NULL,
	[IsLimitError]                 BIT            DEFAULT ((0)) NOT NULL,
	[LimitExpandedExpression] [varchar](8000) NULL,
	[LimitExpressionResult] [float] NULL,
 CONSTRAINT [PK_SmartBox] PRIMARY KEY CLUSTERED 
(
	[SmartBoxId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SmartBox]  WITH CHECK ADD  CONSTRAINT [FK_SmartBox_ContractAdjudications] FOREIGN KEY([ContractAdjudicationId])
REFERENCES [dbo].[ContractAdjudications] ([ContractAdjudicationID])
GO

ALTER TABLE [dbo].[SmartBox] CHECK CONSTRAINT [FK_SmartBox_ContractAdjudications]
GO
