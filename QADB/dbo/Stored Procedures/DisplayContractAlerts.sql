CREATE PROCEDURE dbo.DisplayContractAlerts(
@FacilityID BIGINT,
@DaysToDismissAlerts INT)
AS

/****************************************************************************  
 *   Name         : DisplayContractAlerts
 *   Author       : mmachina  
 *   Date         : 4/Sep/2013  
 *   Module       : Displaying Contract Alerts
 *   Description  : Displaying Contract Alerts

 EXEC DisplayContractAlerts N'1,2,3,4,5,6,7', NULL, 100
 *****************************************************************************/

BEGIN
	-- READ UNCOMMITTED option is used to avoid table being blocked during Copy Model and Copy Contract process
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	SET NOCOUNT ON;
	DECLARE
	   @Currentdate DATETIME = GETUTCDATE();
	  	
	--------------------------fetching data from ContractAlerts Information from Contracts table----------------------------	
	
		BEGIN
			SELECT
				   ISNULL(DBO.GETALLPAYERSBYCONTRACTID(C.CONTRACTID), '') + ' ' + ISNULL('(' + C.PayerCode + ')', '') AS PAYERNAME, 
				   C.CONTRACTNAME + ' #' + CONVERT(VARCHAR(5), DATEPART(YY, C.STARTDATE)) + '-' + CONVERT(VARCHAR(5), DATEPART(YY, C.ENDDATE) % 100)AS CONTRACTNAME, 
				   C.ENDDATE, 
				   CA.CONTRACTID, 
				   ISVERIFIED, 
				   CONTRACTALERTID
			  FROM
				   DBO.CONTRACTS C JOIN DBO.CONTRACTALERTS CA ON C.CONTRACTID = CA.CONTRACTID
															 AND C.ISDELETED = 0
			  WHERE C.ISACTIVE = 1
				AND C.ENDDATE <= DATEADD(DAY, C.ThresholdDaysToExpireAlters, @Currentdate)
				AND CAST(C.StartDate AS DATE) <= CAST(@CurrentDate AS DATE)
				AND CAST(C.EndDate AS DATE) >= CAST(@CurrentDate AS DATE)
				AND CA.ISDISMISSED IS NULL
				AND DATEADD(DAY, @DaysToDismissAlerts, C.ENDDATE) >= @Currentdate
				AND C.FacilityID = @FacilityID
			  ORDER BY
					   ENDDATE ASC;
		END;
END;