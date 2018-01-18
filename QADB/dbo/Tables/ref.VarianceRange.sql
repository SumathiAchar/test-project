CREATE TABLE [dbo].[ref.VarianceRange](
	 VarianceRangeID    TINYINT NOT NULL,
                                      NegativeStartValue FLOAT NULL,
                                      NegativeEndValue   FLOAT NOT NULL,
                                      PositiveStartValue FLOAT NOT NULL,
                                      PositiveEndValue   FLOAT NULL 
) 
GO
