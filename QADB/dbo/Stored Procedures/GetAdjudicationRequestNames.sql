-- =============================================
-- Author:		Manjunath cycle
-- Create date: 01/06/2014
-- Description:	Get adjudication request name based on model id and user
-- EXEC [dbo].[GetAdjudicationRequestNames] 11985,'jay'
-- =============================================
CREATE PROCEDURE [dbo].[GetAdjudicationRequestNames](
       @ModelId                        BIGINT,
       @Runningstatus                  INT,
       @NoOfDaysToDismissCompletedJobs INT )
AS
    BEGIN
        SET NOCOUNT ON;
        -- READ UNCOMMITTED option is used to avoid table being blocked 
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        DECLARE @Currentdate DATETIME = GETUTCDATE();
        SELECT RequestName AS RequestName,
               TaskID
        FROM tracktasks WITH ( NOLOCK )
        WHERE ModelID = @ModelId
          AND RunningStatus NOT IN( 131, 133 )
          AND IsUserDefined = 1
          AND ( RUNNINGSTATUS = @Runningstatus
            AND @Runningstatus <> 999
             OR @Runningstatus = 999
              )
          AND DATEADD(DAY, @NoOfDaysToDismissCompletedJobs * -1, ISNULL(@Currentdate, GETUTCDATE())) <= InsertDate
        ORDER BY InsertDate DESC;
    END;