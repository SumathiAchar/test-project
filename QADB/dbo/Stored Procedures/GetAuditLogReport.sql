CREATE PROCEDURE [dbo].[GetAuditLogReport](
      @UserName             VARCHAR(100),
      @FacilityName         VARCHAR(100),
      @StartDate            DATETIME,
      @EndDate              DATETIME,
      @MaxLinesForCsvReport INT )
AS    

/****************************************************************************      
*   Name         : GetAuditLogReport    
*   Author       : Prasad      
*   Date         : 19/NOV/2015     
*   Modified By  :   
*   Module       : Audit log report
*   Description  : Get audit log report 
*   EXEC [dbo].[GetAuditLogReport] 'jay', 'Baxter Regional Medical Center', '2015-11-20 00:00:00','2015-11-20 23:59:59',10000   
 *****************************************************************************/

     BEGIN
        SET NOCOUNT ON;
        DECLARE @RecordsCountThreshold INT = -1,
                @AuditLogCount         INT = 0,
			 @End DATE=GETUTCDATE(),
			 @Start DATE=GETUTCDATE(),
			 @Description VARCHAR(MAX);
        IF( @StartDate IS NOT NULL
        AND @EndDate IS NOT NULL
          )
            BEGIN
		  
                SELECT @AuditLogCount = COUNT(*)
                FROM AuditLogs WITH ( NOLOCK )
                WHERE UserName = @UserName
                  AND FacilityName = @FacilityName
                  AND LoggedDate BETWEEN @StartDate AND DATEADD(d, 1, @EndDate);
                IF( @AuditLogCount >= @MaxLinesForCsvReport )
                    BEGIN
                        SELECT @RecordsCountThreshold;
                    END;
                ELSE
                    BEGIN
                        SELECT *
                        FROM AuditLogs WITH ( NOLOCK )
                        WHERE( FacilityName = @FacilityName                            
                             )
                         AND LoggedDate BETWEEN @StartDate AND DATEADD(d, 1, @EndDate) ORDER BY LoggedDate DESC;
                    END;
				--If start date and end date is valid.
				 SET @Description='Report: Audit Log, Reporting Criteria : Date Of Service From: '+ CONVERT(VARCHAR(50),@StartDate,110) + '  to  ' + CONVERT(VARCHAR(50),@EndDate,110);
            END;
        ELSE
            BEGIN
                SELECT @AuditLogCount = COUNT(*)
                FROM AuditLogs WITH ( NOLOCK )
                WHERE UserName = @UserName
                  AND FacilityName = @FacilityName;
                IF( @AuditLogCount >= @MaxLinesForCsvReport )
                    BEGIN
                        SELECT @RecordsCountThreshold;
                    END;
                ELSE
                    BEGIN
                        SELECT *
                        FROM AuditLogs WITH ( NOLOCK )
                        WHERE FacilityName = @FacilityName ORDER BY LoggedDate DESC;
                           END;
				 --If start date and end date is not valid.
				    SET @End=CONVERT(VARCHAR(50),@End,110);
				    SET @Start= CONVERT(VARCHAR(50),DATEADD(YY,-3,@End),110);
				    SET @Description='Report: Audit Log, Reporting Criteria : Date Of Service From: '+ CONVERT(VARCHAR(50),@Start,110) + '  to  ' + CONVERT(VARCHAR(50),@End,110);
				 
            END;

		    --Inserts the details of the logging user into AuditLogs table
                INSERT INTO DBO.AuditLogs
                       ( LoggedDate,
                         UserName,
                         ACTION,
                         ObjectType,
                         FacilityName,
                         ModelName,
                         ContractName,
                         ServiceTypeName,
                         Description
                       )
                VALUES( GETUTCDATE(), @UserName, 'View', 'Application', @FacilityName, '', '', '',@Description);
    END;