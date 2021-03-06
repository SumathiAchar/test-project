CREATE PROCEDURE [dbo].[GetAllJobs](
@FacilityID BIGINT, 
@RunningStatus INT, 
@Take INT, 
@Skip INT
)
AS
SET NOCOUNT ON;
BEGIN
	     -- READ UNCOMMITTED option is used to avoid table being blocked during Copy Model and Copy Contract process
	   SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	   DECLARE @MaxRecord INT = 100;
	 
	   DECLARE @Jobs TABLE(
	   JOBID   BIGINT
	   );

	   -- running status 100: completed, 129:Running, 132:Resumed 131:Cancelled, 301: debug, 999:All
	
	   INSERT INTO @Jobs
	   SELECT TOP(@MaxRecord)
			Tasks.TaskID			
		FROM
			dbo.TrackTasks Tasks
			LEFT JOIN ContractHierarchy CH ON CH.NodeId = Tasks.ModelId			
		WHERE RunningStatus NOT IN (131, 301)
			AND (RunningStatus IN (100,128,129,130,132,133))
			AND IsUserDefined = 1
			AND Tasks.FacilityID = @FacilityID
		ORDER BY
				Tasks.InsertDate DESC 
					  
	   SELECT 
			ROW_NUMBER() OVER(ORDER BY InsertDate ASC) RowNumber, 
			Tasks.TaskID AS JOBID, 
			Tasks.UserName AS USERNAME, 
			RequestName, 
			RunningStatus AS STATUS, 
			Tasks.ModelId, 
			Tasks.InsertDate,
			Tasks.TotalClaimCount AS NoOfClaimsSelected, 
			Tasks.AdjudicatedClaimCount AS NoOfClaimsAdjudicated, 
			dbo.GetCriteriaHover(Tasks.SelectCriteria,Tasks.Datefrom,Tasks.Dateto,Tasks.Datetype) As Criteria,
			CASE
				WHEN RunningStatus = 129
					OR RunningStatus = 132 THEN ISNULL(Tasks.ElapsedTime, 0) + DATEDIFF(SS, Tasks.LastUpdatedDateForElapsedTime, GETUTCDATE())
				ELSE ISNULL(Tasks.ElapsedTime, 0)
			END AS ElapsedTime,
			Tasks.IsVerified,
			CH.NodeText AS ModelName
	   FROM 
		  @Jobs Jobs
		  INNER JOIN dbo.TrackTasks Tasks ON Jobs.JOBID = Tasks.TaskId
		  LEFT JOIN ContractHierarchy CH ON CH.NodeId = Tasks.ModelId
	   WHERE 
		  ((@RunningStatus = 999) OR (Tasks.RunningStatus = @RunningStatus))
	   ORDER BY RowNumber DESC 
	   OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY

	   SELECT COUNT(Jobs.JOBID) AS TaskIDTotal  
		  FROM @Jobs Jobs
		  INNER JOIN dbo.TrackTasks Tasks ON Jobs.JOBID = Tasks.TaskId
	   WHERE 
		  ((@RunningStatus = 999) OR (Tasks.RunningStatus = @RunningStatus))
END;
