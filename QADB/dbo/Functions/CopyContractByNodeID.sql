-- =============================================
-- Author:		<Author,,Name>
-- ALTER  date: <ALTER  Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[CopyContractByNodeID]()
RETURNS INT
AS
BEGIN
	DECLARE
	   @Table TABLE(
					NodeID INT);

	INSERT INTO @Table
	SELECT
		   20;
	DECLARE
	   @DataTableObj INT;

	IF(SELECT TOP 1
			  NodeID
		 FROM @Table) = 234
		BEGIN
			RETURN 1;
		END
	ELSE
		BEGIN
			RETURN 2;
		END;

	RETURN 0;
END;
GO
