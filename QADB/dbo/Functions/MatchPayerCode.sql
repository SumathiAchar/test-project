CREATE FUNCTION [dbo].[MatchPayerCode]
(@PayerCode   VARCHAR(MAX),
 @CustomField VARCHAR(MAX)
)
RETURNS BIT
AS
     BEGIN
         DECLARE @Temptable TABLE(Code VARCHAR(MAX));
         DECLARE @Rangetable TABLE(value VARCHAR(MAX));
         DECLARE @StartRange VARCHAR(MAX);
         DECLARE @EndRange VARCHAR(MAX);
         DECLARE @MaxLength INT;
         DECLARE @IsMatch BIT;
         SET @IsMatch = 0;
         INSERT INTO @Temptable
                SELECT *
                FROM dbo.Split(@PayerCode, ',');
         UPDATE @Temptable
           SET
               Code = REPLACE(Code, '*', '%');
         SELECT @StartRange = code
         FROM @Temptable
         WHERE CHARINDEX(':', Code) > 0;
         INSERT INTO @Rangetable
                SELECT *
                FROM dbo.Split(@StartRange, ':');
         SELECT TOP 1 @StartRange = value
         FROM @Rangetable;
         DELETE TOP (1)
         FROM @Rangetable;
         SELECT TOP 1 @EndRange = value
         FROM @Rangetable;
         DELETE TOP (1)
         FROM @Rangetable;
         IF(LEN(@StartRange) > LEN(@EndRange))
             BEGIN
                 IF(LEN(@StartRange) < LEN(@CustomField))
                     SET @MaxLength = LEN(@CustomField);
                 ELSE
                 SET @MaxLength = LEN(@StartRange);
             END;
         ELSE
             BEGIN
                 IF(LEN(@EndRange) < LEN(@CustomField))
                     SET @MaxLength = LEN(@CustomField);
                 ELSE
                 SET @MaxLength = LEN(@EndRange);
             END;
         SET @StartRange = RIGHT(SPACE(@MaxLength) + @StartRange, @MaxLength);
         SET @EndRange = RIGHT(SPACE(@MaxLength) + @EndRange, @MaxLength);
         IF EXISTS
         (
             SELECT 1
             FROM @Temptable
             WHERE @CustomField LIKE Code
         )
             SET @IsMatch = 1;
         ELSE
             BEGIN
                 SET @CustomField = RIGHT(SPACE(@MaxLength) + @CustomField, @MaxLength);
                 SET @IsMatch = IIF(@CustomField >= @StartRange
                                    AND @CustomField <= @EndRange, 1, 0);
             END;
         RETURN @IsMatch;
     END;