CREATE FUNCTION GetICDCode (@ClaimId bigint)
RETURNS varchar(max)
AS
BEGIN
DECLARE @List VARCHAR(max)
 
SELECT @List = COALESCE(@List + ',', '') + CAST(ICDDCode AS VARCHAR) from   ICDDData 
 
where claimid=@ClaimId and instance != 'P' and ICDDCode not like '%NONE%'
       RETURN @List
END
 