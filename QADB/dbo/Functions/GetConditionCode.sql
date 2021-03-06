
CREATE FUNCTION GetConditionCode (@ClaimId bigint)
RETURNS varchar(max)
AS
BEGIN
DECLARE @CondList VARCHAR(80)
 
       SELECT @CondList = COALESCE(@CondList + ',', '') + CAST(ConditionCode AS VARCHAR) from  
       ConditionCodeData    where claimid=@ClaimId 
       RETURN @CondList
END
GO