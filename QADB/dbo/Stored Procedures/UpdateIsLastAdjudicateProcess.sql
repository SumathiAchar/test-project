/****************************************************/

--Method Name : UpdateIsLastAdjudicateProcessed  
--Module      : SelectClaims    
--ALTER d By  : Manjunath.B    
--Date        : 18-Oct-2013
--Description: This script will update the IsLastProcessedDate flag

/****************************************************/

--EXEC UpdateIsLastAdjudicateProcessed 512   
CREATE PROCEDURE [dbo].[UpdateIsLastAdjudicateProcess](
@Taskid BIGINT)
AS
BEGIN
	DECLARE
	   @Count INT;

	SET @Count = 0;

	SELECT
		   @Count = COUNT(*)
	  FROM TRACKTASKS WITH (NOLOCK)
	  WHERE ISLASTPROCESSEDDATE = 1
		 OR ISLASTPROCESSEDDATE IS NULL;

	IF @Count > 0
		BEGIN
			UPDATE TRACKTASKS
			SET
				ISLASTPROCESSEDDATE = 0
			  WHERE
					ISLASTPROCESSEDDATE = 1
				 OR ISLASTPROCESSEDDATE IS NULL;
		END;
	UPDATE TRACKTASKS
	SET
		ISLASTPROCESSEDDATE = 1
	  WHERE
			TASKID = @Taskid;
END;
GO
