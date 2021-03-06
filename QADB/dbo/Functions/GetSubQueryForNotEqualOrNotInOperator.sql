CREATE FUNCTION [dbo].[GetSubQueryForNotEqualOrNotInOperator] 
(
	@ClaimChargeCondition varchar(MAX),
	@TableName VARCHAR (MAX )
)
RETURNS  VARCHAR(max)
AS
BEGIN
	DECLARE @subquery NVARCHAR(max) = '';
	SET @ClaimChargeCondition = dbo.GetSpacesTrimmedFromQuery (@ClaimChargeCondition);
	SET @ClaimChargeCondition = REPLACE(@ClaimChargeCondition,'NOT','');
	SET @ClaimChargeCondition = REPLACE(@ClaimChargeCondition,'CCD','Charge');
	SET @ClaimChargeCondition = REPLACE(@ClaimChargeCondition,') AND (',') OR (');	

		SET @tableName = 'ClaimChargeData';
		SET @subquery = @subquery + 'NOT EXISTS (SELECT Charge.ClaimID
							   FROM '+@tableName+' AS Charge 
							   WHERE (' + @ClaimChargeCondition+ ') 
								AND Charge.ClaimID = CD.ClaimID)';
	RETURN @subquery;
END;
