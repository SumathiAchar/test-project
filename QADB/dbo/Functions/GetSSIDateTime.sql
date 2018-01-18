-- =============================================
--Author		: Sheshagiri
--Created DATE	: 3/12/2015
--Description	: This scalar function will provide Date time based on zone. As per TQ Review comment date time implementation is done at common place
--USAGE			: SELECT dbo.GetSSIDateTime('LOCAL',121);SELECT dbo.GetSSIDateTime('UTC',121);
-- =============================================
CREATE FUNCTION dbo.GetSSIDateTime(
@Timezone VARCHAR(5) = 'LOCAL', 
@Formatcode INT = 103)
RETURNS VARCHAR(100)
AS
BEGIN
	DECLARE
	   @Currentdate DATETIME;
	IF @Timezone = 'LOCAL'
		BEGIN
			SET @Currentdate = GETDATE();
		END
	ELSE
		BEGIN
			SET @Currentdate = GETUTCDATE();
		END;

	RETURN CONVERT(VARCHAR(100), @Currentdate, @Formatcode);
END;



