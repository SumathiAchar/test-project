CREATE FUNCTION [dbo].[CompareClaimConditionStr](
                @Code     VARCHAR(100),
                @Values   VARCHAR(100),
                @Operator INT )
RETURNS INT
AS
     BEGIN
     DECLARE @Flag INT = -2;
     DECLARE @TmpCode VARCHAR(100);
     DECLARE @TblTmp TABLE( ID  INT IDENTITY(1, 1)
                                    PRIMARY KEY,
                            VAL VARCHAR(MAX));
     DECLARE @Cnt INT;
     INSERT INTO @TblTmp
            SELECT *
            FROM dbo.Split( @Code, ',' );
     SELECT @Cnt = COUNT(*)
     FROM @TblTmp;
     WHILE( @cnt > 0 )
         BEGIN
             SELECT @TmpCode = VAL
             FROM @TblTmp
             WHERE ID = @Cnt;
             IF(( @Operator = 1
              AND @TmpCode <> @Values
                )
             OR ( @Operator = 2
              AND @TmpCode > @Values
                )
             OR ( @Operator = 3
              AND @TmpCode = @Values
                )
             OR ( @Operator = 4
              AND @TmpCode < @Values
                )
               )
                 BEGIN
                     SET @Flag = 0;
                     BREAK;
                 END;
             ELSE
                 BEGIN
                     SET @Flag = 1;
                 END;
             SET @cnt = @cnt - 1;
         END;
             RETURN @Flag;
                     END;