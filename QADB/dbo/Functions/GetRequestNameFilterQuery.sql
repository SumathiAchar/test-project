-- =============================================
--Author	  : Dev
--Altered By  : Sheshagiri
--Altered DATE: 4/7/2014
--Description : Converted Claim id in (..) to exists clause.
-- =============================================
CREATE FUNCTION [dbo].[GetRequestNameFilterQuery]
(
@OperatorType VARCHAR(10), 
@Values VARCHAR(MAX),
@ModelID  BIGINT
)
RETURNS VARCHAR(MAX)
AS
BEGIN

	DECLARE
	   @ValueStringData VARCHAR(MAX);
	SELECT
		   @ValueStringData = COALESCE(@ValueStringData + ' UNION ALL ', '') + dbo.GetSubQueryForClaimFilters(DateType, DateFrom, DateTo, SelectCriteria, FacilityID,@ModelID)
	  FROM dbo.TrackTasks
	  WHERE TaskID = @Values
	  ORDER BY
			   TaskID DESC;

	IF LEN(@ValueStringData)IS NOT NULL
		BEGIN
			--SET @ValueStringData =   ' SELECT DISTINCT * FROM ( ' + @ValueStringData + ' ) AS Data '
			SET @ValueStringData = ' SELECT DISTINCT * FROM ( ' + @ValueStringData + ' ) AS Data where Data.C = CD.claimid'
		END;
	RETURN @ValueStringData;
END;
GO
