
/****************************************************/
  
--Method Name : [CheckContractsToMove]  
--Module      : Contract
--ALTER d By  : Vishesh 
--Modified By :   
--ModifiedDATE:  
--Date        : 18-Oct-2013  
--Description : It check the contracts start date & end date and move them accordingly

/****************************************************/

CREATE PROCEDURE [dbo].[CheckContractsToMove](
@Modelnodeid BIGINT)
AS
BEGIN

	DECLARE
	   @Currentdate DATE = CONVERT(DATE, GETUTCDATE(), 105);
	DECLARE
	   @Temp_Table TABLE(
						 ROWCOUNTER INT IDENTITY(1, 1), 
						 STARTDATE DATE, 
						 ENDDATE DATE, 
						 CONTRACTID BIGINT, 
						 NODEID BIGINT, 
						 CONTRACTNAME VARCHAR(500));

	INSERT INTO @Temp_Table
	SELECT
		   C.STARTDATE, 
		   C.ENDDATE, 
		   C.CONTRACTID, 
		   C.NODEID, 
		   C.CONTRACTNAME
	  FROM
		   dbo.GetChildren(@Modelnodeid) INNER JOIN CONTRACTS AS C ON CHILDID = C.NODEID
	  WHERE C.ISDELETED = 0
		AND ((@Currentdate < CONVERT(DATE, C.STARTDATE, 105)
		   OR @Currentdate > CONVERT(DATE, C.ENDDATE, 105))
		 AND C.ISEXPIRED = 0
		  OR @Currentdate >= CONVERT(DATE, C.STARTDATE, 105)
		 AND @Currentdate <= CONVERT(DATE, C.ENDDATE, 105)
		 AND C.ISEXPIRED = 1);

	DECLARE
	   @Rowcount INT = 1;
	DECLARE
	   @Contractidtohandle BIGINT;

	SELECT
		   @Contractidtohandle = CONTRACTID
	  FROM @Temp_Table
	  WHERE ROWCOUNTER = @Rowcount;

	WHILE @Contractidtohandle IS NOT NULL
		BEGIN
			EXEC DBO.MOVECONTRACTBASEDONSTARTDATEANDENDDATE @Contractidtohandle;
			SET @Contractidtohandle = NULL;
			SET @Rowcount = @Rowcount + 1;

			SELECT
				   @Contractidtohandle = CONTRACTID
			  FROM @Temp_Table
			  WHERE ROWCOUNTER = @Rowcount;
		END;
END;
GO
