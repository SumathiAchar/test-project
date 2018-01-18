CREATE PROCEDURE [dbo].[AddRunningTask]
(
	@ModelId BIGINT
) 
AS 
BEGIN 
	IF((SELECT COUNT(*) FROM RunningTask WHERE ModelId = @ModelId) = 0)
		BEGIN
			INSERT INTO dbo.RunningTask(
				ModelId,
				IsRunning
				)
			VALUES(@ModelId
				  ,0 
				  )
		END 
END