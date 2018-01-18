CREATE FUNCTION [dbo].[GetCustomMultipliers](@MultiplierFirst  VARCHAR(5000),
                                            @MultiplierSecond VARCHAR(5000),
                                            @MultiplierThird  VARCHAR(5000),
                                            @MultiplierFour   VARCHAR(5000),
                                            @MultiplierOther  VARCHAR(5000))
RETURNS VARCHAR(MAX)
AS

/****************************************************************************  
 *   Name         : GetCustomMultipliers
 *   Author       : shivakumar 
 *   Date         : 7/4/2016  
 *   Module       : Payment type custom table.
 *   Description  : Gets multipliers in custom table in form of string.
 *****************************************************************************/

     BEGIN
         DECLARE @Multiplier VARCHAR(MAX)= '';
         IF(@MultiplierFirst IS NOT NULL)
             BEGIN
                 SELECT @Multiplier = 'First: '+@MultiplierFirst;
             END;
         IF(@MultiplierSecond IS NOT NULL)
             BEGIN
                 SELECT @Multiplier+=', Second: '+@MultiplierSecond;
             END;
         IF(@MultiplierThird IS NOT NULL)
             BEGIN
                 SELECT @Multiplier+=', Third: '+@MultiplierThird;
             END;
         IF @MultiplierFour IS NOT NULL
             BEGIN
                 SELECT @Multiplier+=', Fourth: '+@MultiplierFour;
             END;
         IF @MultiplierOther IS NOT NULL
             BEGIN
                 SELECT @Multiplier+=', Others: '+@MultiplierOther;
             END;
         IF(@Multiplier <> '')
             BEGIN
                 SELECT @Multiplier = ', Multiplier ='+@Multiplier;
             END;
         RETURN @Multiplier;
     END;
