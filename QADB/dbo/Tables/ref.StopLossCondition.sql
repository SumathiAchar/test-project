CREATE TABLE [dbo].[ref.StopLossCondition](
	[StopLossConditionID] [int] NOT NULL,
	[StopLossConditionName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ref.StopLossCondition] PRIMARY KEY CLUSTERED 
(
	[StopLossConditionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]