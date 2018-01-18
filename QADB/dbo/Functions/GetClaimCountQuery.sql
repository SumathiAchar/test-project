-- =============================================
--Author		: Sheshagiri
--Created DATE	: 3/12/2015
--Description	: This scalar function will build claim count query based on selected criteria
--Usage			: dbo.GetClaimCountQuery(-1,'2012-03-12 18:09:22','2015-03-12 18:09:22',N'-99|2|10490',3,0)
--Literals
--	-99: If user selects "Request Adjudication Name" from UI 	
-- =============================================
CREATE FUNCTION dbo.GetClaimCountQuery(
@Datetype INT, 
@Datefrom DATETIME, 
@Dateto DATETIME, 
@Selectcriteria VARCHAR(MAX), 
@Facilityid BIGINT, 
@Modelid BIGINT)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE
	   @Adjudicationrequestnameclaimfield VARCHAR(3) = '-99', 
	   @Claimsubqry NVARCHAR(MAX) = ' ', 
	   @Claimcountquery NVARCHAR(MAX) = ' ';

	SELECT
		   @Claimsubqry = dbo.GetSubQueryForClaimFilters(@Datetype, @Datefrom, @Dateto, @Selectcriteria, @Facilityid,@Modelid); 
	-- Clear first occurrence of query block
	SET @Claimsubqry = STUFF(@Claimsubqry, CHARINDEX('SELECT DISTINCT CD.claimid C FROM ClaimData AS CD', @Claimsubqry), LEN('SELECT DISTINCT CD.claimid C FROM ClaimData AS CD'), '');

	-- For Adjudication Request Name clear first occurrence of facility join and where clause
	IF CHARINDEX(@Adjudicationrequestnameclaimfield, @Selectcriteria) > 0
		BEGIN
			SET @Claimsubqry = STUFF(@Claimsubqry, CHARINDEX(' JOIN Facility_SSINumber AS FS ON CONVERT(INT,FS.SSINumber) = CONVERT(INT,CD.SSINumber) ', @Claimsubqry), LEN(' JOIN Facility_SSINumber AS FS ON CONVERT(INT,FS.SSINumber) = CONVERT(INT,CD.SSINumber) '), '');
			SET @Claimsubqry = STUFF(@Claimsubqry, CHARINDEX('AND FS.FacilityID = ' + CONVERT(VARCHAR(20), @Facilityid), @Claimsubqry), LEN('AND FS.FacilityID = ' + CONVERT(VARCHAR(20), @Facilityid)), '');
		END;

	SET @Claimcountquery = 'SELECT DISTINCT CD.ClaimID FROM ClaimData AS CD ' + @Claimsubqry;

	RETURN @Claimcountquery;
END;