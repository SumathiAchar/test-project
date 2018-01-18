
/****************************************************/  
--Method Name : CheckDuplicateLetterTemplateName
--Module      : Letter Template  
--Created By  : Raj  
--Date        : 30-Oct-2014
--Description : Check letter name is already exist in DB or not
/****************************************************/ 

CREATE PROCEDURE dbo.CheckDuplicateLetterTemplateName(
	@Name VARCHAR(200),
	@FacilityId INT
)
AS
BEGIN	
	SELECT COUNT(*) FROM dbo.LetterTemplates WHERE Name = @Name AND FacilityId=@FacilityId 
END