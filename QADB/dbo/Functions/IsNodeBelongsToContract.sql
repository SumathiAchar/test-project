/****************************************************/

--Method Name : IsNodeBelongsToContract
--Module      : Contract Hierarchy
--ALTER d By  : Vishesh 
--Modified By :   
--ModifiedDATE:  
--Date        : 23-Sep-2013  
--Description : To check whether node belongs to a contract or not

/****************************************************/

CREATE FUNCTION [dbo].[IsNodeBelongsToContract](
                @NodeID BIGINT )
RETURNS BIT
AS
     BEGIN
     IF EXISTS( SELECT 1
                FROM Contracts
                WHERE NodeID = @NodeID )
     RETURN 1;
     RETURN 0;
     END;
GO