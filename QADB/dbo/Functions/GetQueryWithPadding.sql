CREATE FUNCTION dbo.GetQueryWithPadding(
                @Columname     VARCHAR(100),
                @Inputa        VARCHAR(MAX),
                @Inputb        VARCHAR(100),
                @OperatorValue VARCHAR(10))
RETURNS VARCHAR(MAX)
AS
     BEGIN
     DECLARE @Maxinputlen INT = CASE
                                    WHEN LEN(@Inputa) > LEN(@Inputb)
                                    THEN LEN(@Inputa)
                                    ELSE LEN(@Inputb)
                                END,
             @Spaces      VARCHAR(100) = '',
             @I           INT = 0,
             @Condition   VARCHAR(100) = ' ',
             @ReturnQuery VARCHAR(MAX) = '';
     IF @OperatorValue = '<>'
     SET @Condition = ' NOT ';
     SELECT @Spaces = @Spaces + REPLICATE(' ', @Maxinputlen);
     SELECT @Inputa = RIGHT(@Spaces + @Inputa, @Maxinputlen);
     SELECT @Inputb = RIGHT(@Spaces + @Inputb, @Maxinputlen);
     SET @ReturnQuery = '(RIGHT(''' + @spaces + ''' + LTRIM(RTRIM(' + @ColumName + ')),' + CONVERT(VARCHAR(50), @maxInputLen) + ')' + @Condition + ' BETWEEN ''' + @INPUTA + ''' AND ''' + @INPUTB + ''' AND  LEN(LTRIM(RTRIM(' + @ColumName + ')))' + '<=' + CONVERT(VARCHAR(50), @maxInputLen) + ')';
     RETURN @Returnquery;
     END;