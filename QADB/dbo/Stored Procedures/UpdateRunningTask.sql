CREATE Procedure [dbo].[UpdateRunningTask]
(
	@TaskId BIGINT,
	@IsRunning BIT
)
AS
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION;
	DECLARE @ModelId BIGINT = (SELECT ModelId FROM TrackTasks WHERE TaskId = @TaskId)
	IF((SELECT COUNT(*) FROM RunningTask WHERE ModelId = @ModelId) = 0)
		BEGIN
			INSERT INTO dbo.RunningTask(
				ModelId,
				IsRunning
				)
			VALUES(@ModelId
				  ,@IsRunning 
				  )
		END
	ELSE
		BEGIN
			UPDATE dbo.RunningTask SET IsRunning = @IsRunning WHERE ModelId = @ModelId
		END
	IF @@Trancount > 0
	BEGIN
		COMMIT TRANSACTION;
	END;
	END TRY
	BEGIN CATCH
		BEGIN
			ROLLBACK TRANSACTION;
			EXEC RaiseErrorInformation
		END;
	END CATCH;
END