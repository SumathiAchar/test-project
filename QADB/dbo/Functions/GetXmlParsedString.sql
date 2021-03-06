/****************************************************/

--Created By  : Dev  
--Date        : 19-Feb-2016  
--Altered By  : Asif Ali
--Description : Function to Replace Special Character from string
--select [dbo].[GetXmlParsedString] ('Mega Life & Health')  
--Variables
-- @StringToParse: any string 

/****************************************************/
CREATE FUNCTION [dbo].[GetXmlParsedString]
(@StringToParse VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
     BEGIN
         SET @StringToParse = REPLACE((@StringToParse), '&', '&amp;');
	    SET @StringToParse = REPLACE((@StringToParse), '<', '&lt;');
	    SET @StringToParse = REPLACE((@StringToParse), '>', '&gt;');
         RETURN @StringToParse;
     END;