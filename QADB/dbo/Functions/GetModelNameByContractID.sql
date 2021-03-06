CREATE FUNCTION [dbo].[GetModelNameByContractID]
(
@ContractID BIGINT
)
RETURNS VARCHAR(300)
AS
BEGIN

	DECLARE
	   @IsModelNode BIT = 0;
	DECLARE
	   @NodeId BIGINT;
	DECLARE
	   @ModelText VARCHAR(300);

	SELECT
		   @NodeId = NodeId
	  FROM dbo.Contracts
	  WHERE ContractId = @ContractID;

	WHILE @IsModelNode = 0 AND @NodeId IS NOT NULL
		BEGIN
			DECLARE
			   @ParentID BIGINT;
			SELECT
				   @ParentID = ParentID
			  FROM dbo.ContractHierarchy
			  WHERE NodeId = @NodeId;

			DECLARE
			   @IsPrimaryNode BIT;
			SELECT
				   @IsPrimaryNode = IsPrimaryNode
			  FROM dbo.ContractHierarchy
			  WHERE NodeId = @ParentID;

			IF @IsPrimaryNode IS NOT NULL
				BEGIN
					SET @IsModelNode = 1;
				END;

			SET @NodeId = @ParentID;
		END;
	SELECT
		   @ModelText = NodeText
	  FROM dbo.ContractHierarchy
	  WHERE NodeId = @NodeId;
	RETURN @ModelText;
END;
GO
