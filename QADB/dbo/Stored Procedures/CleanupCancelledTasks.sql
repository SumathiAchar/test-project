/****************************************************/

--Method Name : CleanupCancelledTasks
--Module      : BackgroundAdjudication  
--Created By  : Shesh  
--Date        : 20-Jan-2015
--Modified By : 
--Modify Date : 
--Description :  When ever windows service is stopped, delete claims from from taskclaims and update pending task's RUNNING status to CANCELLED state
--EXEC CleanupCancelledTasks 

/****************************************************/

CREATE PROCEDURE dbo.CleanupCancelledTasks
AS
    BEGIN
        DECLARE @Backgroundadjudicationtasklist AS TABLE( TaskID BIGINT );
        INSERT INTO @Backgroundadjudicationtasklist
               SELECT TaskID
               FROM TrackTasks WITH ( NOLOCK )
               WHERE IsUserDefined = 0
                 AND RunningStatus = 129; -- 129 : Running

        --- Delete claims from task claims.
        DELETE ClaimPicked
        FROM TaskClaims ClaimPicked
             INNER JOIN @Backgroundadjudicationtasklist TaskList ON TaskList.TaskID = ClaimPicked.TaskID;

        -- Update pending task's RUNNING status to CANCELLED state
        UPDATE TasksCalled
          SET TasksCalled.RunningStatus = 131 -- 131 : Cancelled
        FROM TrackTasks TasksCalled
             INNER JOIN @Backgroundadjudicationtasklist TaskList ON TaskList.TaskID = TasksCalled.TaskID;

        -- Updating the running task model id to 0. so that it will pick the next set of claims
        UPDATE RunningTaskCalled
          SET RunningTaskCalled.IsRunning = 0
        FROM RunningTask RunningTaskCalled
             INNER JOIN @Backgroundadjudicationtasklist TaskList ON (SELECT ModelID FROM TrackTasks where TaskID = TaskList.TaskID) = RunningTaskCalled.ModelId;
    END;
GO