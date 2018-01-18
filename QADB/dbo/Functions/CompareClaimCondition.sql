CREATE FUNCTION [dbo].[CompareClaimCondition]
(
@Code VARCHAR(100), 
@Values VARCHAR(100), 
@Operator INT
)
RETURNS INT
AS
BEGIN  
	--DECLARE @Flag INT =0  
	IF @Operator = 1
		BEGIN
			IF @Code <> @Values
				BEGIN
					RETURN 0;
				END
			ELSE
				BEGIN
					RETURN 1;
				END;
		END;
	IF @Operator = 2
		BEGIN
			IF @Code > @Values
				BEGIN
					RETURN 0;
				END
			ELSE
				BEGIN
					RETURN 1;
				END;
		END;
	IF @Operator = 3
		BEGIN
			IF @Code = @Values
				BEGIN
					RETURN 0;
				END
			ELSE
				BEGIN
					RETURN 1;
				END;
		END;
	IF @Operator = 4
		BEGIN
			IF @Code < @Values
				BEGIN
					RETURN 0;
				END
			ELSE
				BEGIN
					RETURN 1;
				END;
		END;
	RETURN -1;
END;
GO
