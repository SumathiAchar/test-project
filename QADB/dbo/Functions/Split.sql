--select top 10 * from dbo.Split_Check('Chennai,Bangalore,Mumbai',',')
CREATE FUNCTION [dbo].[Split](
                @String    VARCHAR(MAX),
                @Delimiter CHAR(1))
RETURNS @temptable TABLE( items VARCHAR(MAX))
AS
     BEGIN
     IF LEN(LTRIM(@String)) > 0
         BEGIN
             INSERT INTO @temptable
                    SELECT Delimited.Data.value( '.', 'VARCHAR(MAX)' ) AS 'Strng'
                    FROM( 
                          SELECT CAST('<Data>' + REPLACE(@String, @Delimiter, '</Data><Data>') + '</Data>' AS XML)) AS Data( Splited )
                        CROSS APPLY Data.Splited.nodes( 'Data' ) AS Delimited( Data );
         END;
                    RETURN;
             END;
GO