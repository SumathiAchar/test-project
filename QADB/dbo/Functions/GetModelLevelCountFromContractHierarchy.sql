
/****************************************************/
  
--Method Name : GetModelLeveLCountFromContractHierarchy
--Module      : Contract Hierarchy  
--ALTER d By  : Vishesh  
--Date        : 01-Oct-2013  
--Modified By : 
--Modify Date : 
--Description : To get model level count from contract hierarchy
-- SELECT * FROM [GetModelLevelCountFromContractHierarchy] (1,NULL)

/****************************************************/

CREATE FUNCTION [dbo].[GetModelLevelCountFromContractHierarchy]
(
@FacilityID BIGINT
)
RETURNS @temp_ContractHierarchyFinalDataTable TABLE(
													NodeId BIGINT, 
													ParentId BIGINT, 
													NodeText VARCHAR(100), 
													AppendString VARCHAR(100), 
													IsContract BIT, 
													IsPrimaryNode BIT, 
													ContractID BIGINT, 
													FacilityID BIGINT)
AS
BEGIN

	DECLARE
	   @LastNodeId BIGINT = 0;
	DECLARE
	   @NodeIDToHandle BIGINT;
	DECLARE
	   @ParentIdToHandle BIGINT;

	-- Get the primary node of facility and then get all the model one by one with count of contracts within it
	SELECT TOP 1
		   @NodeIDToHandle = NodeId, 
		   @ParentIdToHandle = ParentId
	  FROM ContractHierarchy
	  WHERE FacilityID = @FacilityID
		AND IsPrimaryNode = 1;

	-- As long as we have Nodes we need to keep counting
	WHILE @NodeIDToHandle IS NOT NULL
		BEGIN

			--------------------------------------------------------------------------------------------------------------------------
			--------------------------------------------------------------------------------------------------------------------------

			DECLARE
			   @temp_ContractHierarchyDataTable TABLE(
													  NodeId BIGINT, 
													  ParentId BIGINT, 
													  ContractCount BIGINT);


			WITH ReportData
				AS (SELECT
						   ContractHierarchy.NodeId, 
						   ContractHierarchy.NodeText, 
						   ContractHierarchy.ParentId
					  FROM ContractHierarchy
					  WHERE ContractHierarchy.ParentId = @NodeIDToHandle
					UNION ALL
					SELECT
						   ContractHierarchy.NodeId, 
						   ContractHierarchy.NodeText, 
						   ContractHierarchy.ParentId
					  FROM
						   ReportData INNER JOIN ContractHierarchy ON ContractHierarchy.ParentId = ReportData.NodeId)

				INSERT INTO @temp_ContractHierarchyDataTable(
							NodeId, 
							ParentId, 
							ContractCount)
				SELECT
					   NodeId, 
					   ParentId, 
					   (
							SELECT
								   COUNT(*)
							  FROM Contracts AS CON
							  WHERE CON.NodeId = ReportData.NodeId
								AND CON.IsDeleted = 0)
					   AS ContractCount
				  FROM ReportData;

			DECLARE
			   @ContractCount BIGINT;
			SELECT
				   @ContractCount = SUM(ContractCount)
			  FROM @temp_ContractHierarchyDataTable;


			INSERT INTO @temp_ContractHierarchyFinalDataTable
			SELECT
				   @NodeIDToHandle AS NodeId, 
				   @ParentIdToHandle AS ParentId, 
				   CH.NodeText, 
				   CASE
				   WHEN @ContractCount = 0 THEN NULL
				   ELSE '(' + CONVERT(VARCHAR(MAX), @ContractCount) + ')'
				   END AS AppndString, 
				   NULL, 
				   CH.IsPrimaryNode, 
				   NULL, 
				   @FacilityID
			  FROM ContractHierarchy AS CH
			  WHERE NodeID = @NodeIDToHandle 
			  
			  AND IsPrimaryNode=0
			  UNION ALL
			  SELECT
				   @NodeIDToHandle AS NodeId, 
				   @ParentIdToHandle AS ParentId, 
				   CH.NodeText, 
				   CASE
				   WHEN @ContractCount = 0 THEN NULL
				   ELSE '(' + CONVERT(VARCHAR(MAX), @ContractCount) + ')'
				   END AS AppndString, 
				   NULL, 
				   CH.IsPrimaryNode, 
				   NULL, 
				   @FacilityID
			  FROM ContractHierarchy AS CH
			  WHERE NodeID = @NodeIDToHandle 
			  AND IsPrimaryNode=1;
			-- To Drop TEMP Table
			DELETE @temp_ContractHierarchyDataTable;

			--------------------------------------------------------------------------------------------------------------------------
			--------------------------------------------------------------------------------------------------------------------------

	
			-- set the last Node handled to the one which handled right now
			SET @LastNodeId = @NodeIDToHandle;
			SET @NodeIDToHandle = NULL;
			
				BEGIN
					SELECT TOP 1
						   @NodeIDToHandle = NodeId, 
						   @ParentIdToHandle = ParentId
					  FROM ContractHierarchy
					  WHERE NodeId > @LastNodeId
						AND FacilityID = @FacilityID
						AND IsPrimaryNode IS NOT NULL
						AND IsDeleted = 0
					  ORDER BY
							   NodeId;
				END;
		END;

	RETURN;
END;
GO
