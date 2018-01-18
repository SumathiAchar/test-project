CREATE TABLE [dbo].[PaymentTypeCap](
	[PaymentTypeDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[Threshold] [float] NULL,
	[Percentage] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentTypeDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]