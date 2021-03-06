CREATE PROCEDURE dbo.GETALLFACILITIES(
@Facilityid VARCHAR(1000))
AS  

/****************************************************************************  
 *   Name         : GetAllFacilities 
 *   Author       : Prasadd
 *   Modified By  : Vishesh  
 *   Date         : 18/Sep/2013  
 *   Module       : Get All Facilities  
 *   Description  : Get All Facilities Information from database  
 -- EXEC [dbo].[GetAllFacilities] '2,3'
 *****************************************************************************/

BEGIN

	-- READ UNCOMMITTED option is used to avoid table being blocked during Copy Model and Copy Contract process
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	DECLARE
	   @Facilitytable TABLE(
							FACILITYID BIGINT);
	INSERT INTO @Facilitytable
	SELECT
		   *
	  FROM dbo.SPLIT(@Facilityid, ',');

	SELECT
		   CH.NODEID AS NODEID, 
		   CH.NODETEXT AS FACILITYNAME, 
		   CH.FACILITYID AS FACILITYID
	  FROM
		   dbo.CONTRACTHIERARCHY AS CH JOIN @Facilitytable AS FT ON FT.FACILITYID = CH.FACILITYID
	  WHERE CH.PARENTID = 0;
END;
