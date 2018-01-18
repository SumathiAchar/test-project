
CREATE FUNCTION GetPriICDDCode (@ClaimId bigint)
RETURNS varchar(6)
AS
BEGIN
DECLARE @PriICDDCode VARCHAR(6)
 
select @PriICDDCode = ICDDCode from icdddata where claimid = @ClaimId and instance = 'P'and ICDDCode not like '%NONE%'
       RETURN @PriICDDCode
END
GO