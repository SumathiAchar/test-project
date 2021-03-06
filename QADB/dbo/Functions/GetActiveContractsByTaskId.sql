-- =============================================
-- Author:		Manjunath
-- ALTER  date: 13/9/2013
-- Description:	Getting active contracts by task id
-- =============================================
CREATE FUNCTION dbo.GetActiveContractsByTaskId(
@Taskid BIGINT)
RETURNS 

--Temporary table for storing active contract ids
@Tblcontractids TABLE(
					  ContractID BIGINT)
AS
BEGIN
	DECLARE
	   @Count INT;
	DECLARE
	   @NodeID BIGINT;
	DECLARE
	   @ModelID BIGINT;

	--For storing primary nodes
	DECLARE
	   @TblNodes TABLE(
					   RowID INT IDENTITY(1, 1), 
					   NodeID BIGINT);

	--For storing child nodes
	DECLARE
	   @Tbltree TABLE(
					 ParentID INT, 
					  ChildID INT);

	IF @TaskID IS NULL
		BEGIN

			INSERT INTO @TblNodes
			SELECT
				   NodeID
			  FROM dbo.ContractHierarchy
			  WHERE IsPrimaryNode = 1;
			SET @Count = (SELECT
								 COUNT(*)
							FROM @TblNodes);

			--Getting child nodes for each primary nodes
			WHILE @Count > 0
				BEGIN
					SET @NodeID = (SELECT
										  NodeID
									 FROM @TblNodes
									 WHERE RowID = @Count);
					INSERT INTO @TblTree
					SELECT
						   *
					  FROM dbo.GetChildren(@NodeID);
					SET @Count = @Count - 1;
				END;

			--Getting active projects for each child noode
			INSERT INTO @TblContractIds
			SELECT
				   ContractID
			  FROM
				   @TblTree Tree INNER JOIN dbo.Contracts CTR ON Tree.ChildID = CTR.NodeID
			  WHERE CTR.IsActive = 1
				AND IsDeleted = 0;
		END;
	ELSE
		BEGIN
			--getting modelid with respect to the taskid
			SET @Modelid = (SELECT
								   ModelID
							  FROM TrackTasks
							  WHERE TaskID = @TaskID);

			--Getting active projects for childnodes
			INSERT INTO @TblContractIDs
			SELECT
				   ContractID
			  FROM
				   dbo.GetChildren(@ModelID) Tree INNER JOIN dbo.Contracts CTR ON Tree.ChildID = CTR.NodeID
			  WHERE CTR.IsActive = 1
				AND IsDeleted = 0;

		END;
	RETURN;
END;
GO
