﻿CREATE TABLE [dbo].[PaymentTypePerCase](
	[PaymentTypeDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[Rate] [float] NULL,
	[MaxCasesPerDay] [int] NULL,
 CONSTRAINT [PK_PaymentTypePerCase] PRIMARY KEY CLUSTERED 
(
	[PaymentTypeDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]