CREATE PROCEDURE [dbo].[GetAllLetterTemplates]
(
	@Take int,
	@Skip int
)
AS
BEGIN
	
	;WITH TemplateData AS
	(
		SELECT
		   ROW_NUMBER() OVER(ORDER BY LetterTemplateID ASC) RowNumber, 
		   LetterTemplateID, 
		   Name, 
		   UserName,
		   TemplateText, 
		   InsertDate
	  FROM LetterTemplates
	)
	SELECT 
		   LetterTemplateID, 
		   Name, 
		   UserName,
		   TemplateText, 
		   InsertDate
	  FROM TemplateData
	  ORDER BY LetterTemplateID DESC,RowNumber ASC OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
	
	SELECT COUNT(LetterTemplateID) TemplatesTotal FROM LetterTemplates
END;
