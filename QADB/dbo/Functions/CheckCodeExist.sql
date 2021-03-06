
CREATE FUNCTION [dbo].[CheckCodeExist]
(
@CodesTocheck VARCHAR(8000), 
@String VARCHAR(8000)
)
RETURNS BIT
AS
BEGIN

	DECLARE
	   @IsExist BIT;
	SET @IsExist = 0;


	DECLARE
	   @TempTable TABLE(
						ITEMS VARCHAR(8000)); 

	--Get Comma Separted string to table
	--DECLARE @String VARCHAR(MAX)
	DECLARE
	   @Delimiter CHAR(1);
	--SET @String='302,304,310-315,33*'
	SET @Delimiter = ',';
	DECLARE
	   @Idx INT;
	DECLARE
	   @Slice VARCHAR(8000);

	SELECT
		   @Idx = 1;
	IF LEN(@String) < 1
	OR @String IS NULL
		BEGIN
			RETURN @IsExist;
		END;


	WHILE @Idx != 0
		BEGIN
			SET @Idx = CHARINDEX(@Delimiter, @String);
			IF @Idx != 0
				BEGIN
					SET @Slice = LEFT(@String, @Idx - 1);
				END
			ELSE
				BEGIN
					SET @Slice = @String;
				END;

			IF LEN(@Slice) > 0
				BEGIN


					DECLARE
					   @FindRangeIndex INT;
					DECLARE
					   @FindStarIndex INT;
					DECLARE
					   @FindRangeText VARCHAR(1);
					DECLARE
					   @FindStarText VARCHAR(1);

					SET @Findrangetext = '-';
					SET @FindStarText = '*';

					SET @FindRangeIndex = CHARINDEX(@FindRangeText, @Slice);
					SET @FindStarIndex  = CHARINDEX(@FindStarText, @Slice);

					IF @FindRangeIndex = 0
				   AND @FindStarIndex = 0
						BEGIN
							INSERT INTO @TempTable(
										Items)
							VALUES(@Slice);
						END;
					ELSE
						BEGIN
							IF @FindRangeIndex > 0
								BEGIN
									DECLARE
									   @FromValue VARCHAR(8000);
									DECLARE
									   @ToValue VARCHAR(8000);
									SET @FromValue = LEFT(@Slice, @Findrangeindex - 1);
									SET @ToValue = RIGHT(@Slice, LEN(@Slice) - @Findrangeindex);
									DECLARE
									   @RangeCount INT;
									SELECT
										   @RangeCount = 0;
									WHILE @RangeCount <= CONVERT(INT, @Tovalue) - CONVERT(INT, @Fromvalue)
										BEGIN
											INSERT INTO @TempTable(
														ITEMS)
											VALUES(@FromValue + @RangeCount);
											SET @RangeCount = @RangeCount + 1;
										END;
								END;
							ELSE
								BEGIN
									IF @FindStarIndex > 0
										BEGIN

											DECLARE
											   @TempStarValue VARCHAR(8000);
											SET @TempStarValue = LEFT(@Slice, @FindStarIndex - 1);
											DECLARE
											   @Starcount INT;
											SELECT
												   @Starcount = 0;
											WHILE @Starcount <= 10
												BEGIN

													INSERT INTO @TempTable(
																ITEMS)
													VALUES(CONVERT(VARCHAR(8000), @TempStarValue) + CONVERT(VARCHAR(10), @Starcount));

													SET @Starcount = @Starcount + 1;
													IF @Starcount = 10
														BEGIN BREAK
														END;
												END;
										END;
								END;
						END;

				END;

			SET @String = RIGHT(@String, LEN(@String) - @Idx);
			IF LEN(@String) = 0
				BEGIN BREAK
				END;
		END; 

	--SELECT * from @temptable

	DECLARE
	   @Noofoccurance INT;

	SELECT
		   @NoOfOccurance = COUNT(*)
	  FROM @TempTable
	  WHERE ITEMS IN(
					 SELECT TOP 10
							*
					   FROM dbo.Split(@CodesToCheck, ','));

	IF @NoOfOccurance > 0
		BEGIN
			SET @IsExist = 1
		END;

	RETURN @IsExist;
END;


--DECLARE @isExist bit
--DECLARE @CodesToCheck varchar(8000)
--DECLARE @String varchar(8000)
--SET @CodesToCheck = '405,415,301'
--SET @String = '301,302,305-315,33*'
--SET @isExist= dbo.CheckCodeExist(@CodesToCheck , @String)
--SELECT @isExist
GO
