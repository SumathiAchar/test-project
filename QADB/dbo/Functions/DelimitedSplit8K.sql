CREATE FUNCTION [dbo].[DelimitedSplit8K]
(
@Pstring VARCHAR(8000), 
@Pdelimiter CHAR(1)
)
RETURNS TABLE
AS
RETURN

	WITH E1(
		 N)
		AS (SELECT
				   1
			UNION ALL
			SELECT
				   1
			UNION ALL
			SELECT
				   1
			UNION ALL
			SELECT
				   1
			UNION ALL
			SELECT
				   1
			UNION ALL
			SELECT
				   1
			UNION ALL
			SELECT
				   1
			UNION ALL
			SELECT
				   1
			UNION ALL
			SELECT
				   1
			UNION ALL
			SELECT
				   1), E2(
		 N)
		AS (SELECT
				   1
			  FROM E1 A, E1 B), E4(
		 N)
		AS (SELECT
				   1
			  FROM E2 A, E2 B), CTETALLY(
		 N)
		AS (SELECT
				   ROW_NUMBER()OVER(ORDER BY N)
			  FROM E4)
		SELECT
			   ROW_NUMBER()OVER(ORDER BY N)AS ITEMNUMBER, 
			   SUBSTRING(@Pstring, N, CHARINDEX(@Pdelimiter, @Pstring + @Pdelimiter, N) - N)AS ITEM
		  FROM CTETALLY
		  WHERE N < LEN(@Pstring) + 2
			AND SUBSTRING(@Pdelimiter + @Pstring, N, 1) = @Pdelimiter;
GO
