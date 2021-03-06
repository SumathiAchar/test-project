
/****************************************************/
  
--Method Name : [GetNodeIDBasedOnStartAndEndDate]  
--Module      : Contract Hierarchy
--ALTER d By  : Vishesh 
--Modified By :   
--ModifiedDATE:  
--Date        : 23-Sep-2013  
--Description : Get nodeId based On start and end date

/****************************************************/

CREATE FUNCTION [dbo].[GetPrimaryModelNodeIDByContractID]
(
@ContractID BIGINT
)
RETURNS BIGINT
AS
BEGIN

	DECLARE
	   @Temp_TablePrimary TABLE(
								NodeID BIGINT);
	DECLARE
	   @ContractNodeID BIGINT;

	SELECT
		   @ContractNodeID = NodeID
	  FROM dbo.Contracts
	  WHERE ContractID = @ContractID;

	-- Getting all the nodes present in a facility based on contracts ID
	INSERT INTO @Temp_TablePrimary
	SELECT
		   CH.NodeID
	  FROM
		   dbo.ContractHierarchy AS CH JOIN dbo.Contracts AS C ON C.FacilityID = CH.FacilityID
													  AND CH.IsDeleted = 0
	  WHERE CH.IsPrimaryNode IS NOT NULL
		AND C.ContractID = @ContractID;

	DECLARE
	   @NodeIDToFindParentNode BIGINT;

	-- Taking top 1 node to start the loop
	SELECT TOP 1
		   @NodeIDToFindParentNode = NodeID
	  FROM @Temp_TablePrimary
	  ORDER BY
			   NodeID;

	DECLARE
	   @NodeIDData BIGINT;
	WHILE @NodeIDToFindParentNode IS NOT NULL
		BEGIN
			SET @NodeIDToFindParentNode = @NodeIDToFindParentNode;
			WITH ReportData
				AS (SELECT
						   ContractHierarchy.NodeId, 
						   ContractHierarchy.NodeText, 
						   ContractHierarchy.ParentId
					  FROM ContractHierarchy
					  WHERE ContractHierarchy.ParentId = @NodeIDToFindParentNode
						AND ContractHierarchy.IsDeleted = 0
					UNION ALL
					SELECT
						   ContractHierarchy.NodeId, 
						   ContractHierarchy.NodeText, 
						   ContractHierarchy.ParentId
					  FROM
						   ReportData INNER JOIN dbo.ContractHierarchy ON ContractHierarchy.ParentId = ReportData.NodeId
																  AND ContractHierarchy.IsDeleted = 0)
				SELECT
					   @NodeIDData = NodeID
				  FROM ReportData
				  WHERE NodeID = @ContractNodeID;
			IF @NodeIDData IS NOT NULL
				BEGIN BREAK;
				END
			ELSE
				BEGIN 
					-- Taking top 1 node to set the loop to next element
					SET @NodeIDData = NULL;
					SELECT TOP 1
						   @NodeIDToFindParentNode = NodeID
					  FROM @Temp_TablePrimary
					  WHERE NodeID > @NodeIDToFindParentNode
					  ORDER BY
							   NodeID;
				END;
		END;

	RETURN @NodeIDToFindParentNode;
END;
GO
