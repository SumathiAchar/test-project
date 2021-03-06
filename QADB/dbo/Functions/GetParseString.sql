--REVIEW-APRIL16-AA Please add header section with author and purpose
CREATE FUNCTION [dbo].[Getparsestring](
	@Section SMALLINT,
	@Delimiter CHAR,
@Text VARCHAR(MAX))
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE
	   @NextPos SMALLINT, 
	   @LastPos SMALLINT, 
	   @Found SMALLINT;

IF @Section > 0
		BEGIN
			SELECT
				   @Text = REVERSE(@Text)
		END;
	SELECT
		   @NextPos = CHARINDEX(@DeLimiter, @Text, 1), 
		   @LastPos = 0, 
		   @Found = 1;

	WHILE @Nextpos > 0
	  AND ABS(@Section) <> @Found
		SELECT
			   @Lastpos = @Nextpos, 
			   @Nextpos = CHARINDEX(@Delimiter, @Text, @NextPos + 1), 
			   @Found = @Found + 1;
	RETURN CASE
		   WHEN @Found <> ABS(@Section)
			 OR @Section = 0 THEN NULL
		   WHEN @Section > 0 THEN REVERSE(SUBSTRING(@Text, @LastPos + 1, CASE
																		 WHEN @NextPos = 0 THEN DATALENGTH(@Text) - @Lastpos
																		 ELSE @NextPos - @LastPos - 1
																		 END))
		   ELSE SUBSTRING(@Text, @LastPos + 1, CASE
											   WHEN @NextPos = 0 THEN DATALENGTH(@Text) - @LastPos
											   ELSE @NextPos - @LastPos - 1
											   END)
		   END;

END;
GO
