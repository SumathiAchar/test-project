/****************************************************************************
 *   Name         : GetAppealTemplates
 *   Author       : Raj
 *   Date         : 4-Nov-2014
 *   Module       : Letter Template
 *   Description  : Get all appeal letter template
 *****************************************************************************/

CREATE PROCEDURE dbo.GetAppealTemplates
@FacilityId BIGINT
AS
BEGIN
	SELECT 
		LetterTemplateID, 
		Name, FacilityId
		FROM dbo.LetterTemplates
		where FacilityId = @FacilityId
			ORDER BY Name
END;




