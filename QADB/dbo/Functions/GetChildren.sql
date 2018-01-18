CREATE FUNCTION [dbo].[GetChildren](
 @ParentID BIGINT )
RETURNS @TblTree TABLE( ParentID BIGINT,
                        ChildID  BIGINT
                        PRIMARY KEY( ParentID, ChildID ))
AS
     BEGIN
     WITH GetChildrenInfo
          AS (
          SELECT p.ParentID,
                 p.NodeID
          FROM dbo.ContractHierarchy p WITH ( NOLOCK )
          WHERE p.ParentID = @ParentID
          UNION ALL
          SELECT c.ParentID,
                 c.NodeID
          FROM dbo.ContractHierarchy c WITH ( NOLOCK )
               INNER JOIN GetChildrenInfo GetChildrenInfo ON GetChildrenInfo.NodeID = c.ParentID )
          INSERT INTO @TblTree
                 SELECT *
                 FROM GetChildrenInfo;
          RETURN;
     END;
GO
