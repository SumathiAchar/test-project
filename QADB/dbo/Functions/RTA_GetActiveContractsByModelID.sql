-- =============================================
-- Author:		Yashjeet
-- ALTER  date: 3/16/2015
-- Description:	Getting active contracts by Model id
-- =============================================
CREATE FUNCTION [dbo].[RTA_GetActiveContractsByModelID](
@ModelID BIGINT,
@PayerName VARCHAR(100))
RETURNS 
--Temporary table for storing active contract ids
@TblContractIDs TABLE(
					  ContractID BIGINT
					 )
AS 
BEGIN
	--Insert ContractId into @TblContractIDs table
	INSERT INTO @TblContractIDs
	SELECT C.ContractID 
		FROM dbo.GetChildren(@ModelID) N INNER JOIN dbo.Contracts C WITH (NOLOCK) ON N.ChildID = C.NodeID
			INNER JOIN ContractPayers CP WITH (NOLOCK) ON  C.ContractID=CP.ContractID
			WHERE C.IsActive = 1 AND C.IsDeleted = 0 AND CP.PayerName=@PayerName;

RETURN;
END