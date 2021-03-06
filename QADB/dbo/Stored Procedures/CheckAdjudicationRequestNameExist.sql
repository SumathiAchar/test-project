-- =============================================
-- Author:		Manjunath cycle
-- Create date: 01/06/2014
-- Description:	Get adjudication request name based on model id and user
-- =============================================
CREATE PROCEDURE [dbo].[CheckAdjudicationRequestNameExist]
-- Add the parameters for the stored procedure here
@RequestName VARCHAR(100),
@FacilityID  BIGINT
AS
     BEGIN
         -- READ UNCOMMITTED option is used to avoid table being blocked 
         SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
         SELECT COUNT(*)
         FROM tracktasks
         WHERE RequestName = @RequestName
               AND FacilityID = @FacilityID
               AND IsUserDefined = 1
               AND RunningStatus <> 131
               AND DATEDIFF(DAY, LastUpdatedDateforElapsedTime, GETUTCDATE()) < 30;
     END;
GO