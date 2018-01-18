CREATE PROCEDURE [dbo].[UpdateVerifiedSatusForJobs]
(
@TaskID INT
)
AS
BEGIN

UPDATE TrackTasks
	SET IsVerified = 1
	WHERE TaskID = @TaskID AND RunningStatus = 100 --100:completed job

END

GO
