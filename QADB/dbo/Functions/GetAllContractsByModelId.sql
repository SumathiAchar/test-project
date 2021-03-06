-- =============================================
-- Author:		Manjunath
-- ALTER  date: 13/9/2013
-- Description:	Getting active contracts by task id
-- =============================================
CREATE FUNCTION [dbo].[GetAllContractsByModelId](
@ModelNodeID BIGINT)
RETURNS 

--Temporary table for storing active contract ids
@TblContractIDs TABLE(
					  ContractID BIGINT)
AS
BEGIN
	DECLARE
	   @Count INT;
	DECLARE
	   @NodeID BIGINT;
	DECLARE
	   @ModelID BIGINT;

	BEGIN
	--Gets contract id based on model id
		INSERT INTO @TblContractIDs
		SELECT
			   ContractID
		  FROM
			   dbo.GetChildren(@ModelNodeID) Tree INNER JOIN dbo.Contracts CTR ON Tree.ChildID = CTR.NodeID
		  WHERE IsDeleted = 0;

	END;
	RETURN;
END;
GO
