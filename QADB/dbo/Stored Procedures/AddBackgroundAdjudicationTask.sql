/****************************************************************************  
 *   Name         : AddBackgroundAdjudicationTask
 *   Author       : Sheshagiri  
 *   Date         : 02/17/2015  
 *   Alter By     : 
 *   Alter Date   : 
 *   Module       : Background adjudication
 *   Description  : Creates background adjudication task for given model and facility


EXEC  AddBackgroundAdjudicationTask 3,1,4501
 *****************************************************************************/
CREATE PROCEDURE dbo.AddBackgroundAdjudicationTask(
@FacilityID BIGINT,
@PrimaryNodeID BIGINT,
@TaskID BIGINT OUTPUT
)
AS
BEGIN
	DECLARE @CurrentDate DATETIME = GETUTCDATE()

		--Create background adjudication task
		INSERT INTO dbo.TrackTasks(
							InsertDate, 
							UpdateDate, 
							UserName, 
							RequestName, 
							IsUserDefined, 
							RunningStatus, 
							Priority, 
							SelectCriteria, 
							FacilityID, 
							ModelID, 
							DateType, 
							DateFrom, 
							DateTo, 
							IdJob, 
							StartTime, 
							EndTime, 
							LastUpdatedDateForElapsedTime, 
							IsVerified)
		VALUES(@Currentdate, 
				NULL, 
				NULL, 
				'TrackTask_' + CAST(@Facilityid AS VARCHAR(100)) + '_' + CONVERT(VARCHAR(40), @CurrentDate, 109), 
				0, 
				129, 
				0, 
				NULL, 
				@FacilityID, 
				@PrimaryNodeID, 
				NULL, 
				NULL, 
				NULL, 
				0, 
				NULL, 
				NULL, 
				@CurrentDate, 
				0);
		SET @TaskID =  SCOPE_IDENTITY();

		-- Add model id into running task table
		EXEC [dbo].[AddRunningTask] @PrimaryNodeID 
END

