
/****************************************************/
  
--Method Name : [GetNodeIDBasedOnStartAndEndDate]  
--Module      : Contract Hierarchy
--ALTER d By  : Vishesh 
--Modified By :   
--ModifiedDATE:  
--Date        : 23-Sep-2013  
--Description : Get nodeId based On start and end date

/****************************************************/

CREATE FUNCTION [dbo].[GetNodeIDBasedOnStartAndEndDate]
(
@StartDate DATETIME, 
@EndDate DATETIME, 
@NodeID BIGINT
)
RETURNS BIGINT
AS
BEGIN

	SELECT
		   @StartDate = CONVERT(DATE, @StartDate, 105);
	SELECT
		   @EndDate = CONVERT(DATE, @EndDate, 105);

	DECLARE
	   @NonCurrentContractText VARCHAR(100);
	DECLARE
	   @CurrentContractText VARCHAR(100);
	DECLARE
	   @InActiveContractText VARCHAR(100);
	DECLARE
	   @CurrentDate DATETIME = CONVERT(DATE, GETUTCDATE(), 105);
	-- By default at the time of creating new contract it has to go into inactive contracts
	-- & based on start date and end date it has to fall into Current or Non-Current model. 
  
	-- To get the row "4	2	InActive Contracts" present in ContractHierarchyMasterData
	SELECT
		   @InActiveContractText = NodeText
	  FROM dbo.ContractHierarchyMasterData
	  WHERE NodeID = 4;
  
	-- To get the row "5	3	Current Contracts" present in ContractHierarchyMasterData
	SELECT
		   @CurrentContractText = NodeText
	  FROM dbo.ContractHierarchyMasterData
	  WHERE NodeID = 5;

	-- To get the row "6	3	NonCurrent Contracts" present in ContractHierarchyMasterData
	SELECT
		   @NonCurrentContractText = NodeText
	  FROM dbo.ContractHierarchyMasterData
	  WHERE NodeID = 6;

	DECLARE
	   @NodeIDToHandle BIGINT;
	DECLARE
	   @ContractNodeID BIGINT;
	DECLARE
	   @NodeIDToReturn BIGINT;
	DECLARE
	   @temp_ContractHierarchyDataTable TABLE(
											  NodeId BIGINT, 
											  ParentId BIGINT, 
											  NodeText VARCHAR(MAX));
	SELECT
		   @NodeIDToHandle = @NodeID;

	WITH ReportData
		AS (SELECT
				   ContractHierarchy.NodeId, 
				   ContractHierarchy.NodeText, 
				   ContractHierarchy.ParentId
			  FROM dbo.ContractHierarchy
			  WHERE ContractHierarchy.ParentId = @NodeIDToHandle
				AND ContractHierarchy.NodeText = @InActiveContractText
				AND ContractHierarchy.IsDeleted = 0
			UNION ALL
			SELECT
				   ContractHierarchy.NodeId, 
				   ContractHierarchy.NodeText, 
				   ContractHierarchy.ParentId
			  FROM
				   ReportData INNER JOIN dbo.ContractHierarchy ON ContractHierarchy.ParentId = ReportData.NodeId
														  AND ContractHierarchy.IsDeleted = 0)
		INSERT INTO @temp_ContractHierarchyDataTable(
					NodeId, 
					ParentId, 
					NodeText)
		SELECT
			   NodeId, 
			   ParentId, 
			   NodeText
		  FROM ReportData;

	IF @StartDate <= @CurrentDate
   AND @EndDate >= @CurrentDate
		BEGIN
			SELECT
				   @NodeIDToReturn = NodeId
			  FROM @temp_ContractHierarchyDataTable
			  WHERE NodeText = @CurrentContractText;
		END
	ELSE
		BEGIN
			SELECT
				   @NodeIDToReturn = NodeID
			  FROM @temp_ContractHierarchyDataTable
			  WHERE NodeText = @NonCurrentContractText;
		END;

	RETURN @NodeIDToReturn;

END;
GO
