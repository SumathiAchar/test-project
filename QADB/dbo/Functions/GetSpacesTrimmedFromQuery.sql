CREATE FUNCTION [dbo].[GetSpacesTrimmedFromQuery]
(@ClaimChargeCondition VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
     BEGIN
         WHILE CHARINDEX('  ', @ClaimChargeCondition) > 0
             BEGIN
                 SET @ClaimChargeCondition = REPLACE(@ClaimChargeCondition, '  ', ' ');
             END;
         RETURN @ClaimChargeCondition;
     END;
