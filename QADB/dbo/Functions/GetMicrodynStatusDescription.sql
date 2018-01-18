CREATE FUNCTION [dbo].[GetMicrodynStatusDescription](
@EditErrorCodes VARCHAR(MAX),
@PricerErrorCodes INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
DECLARE @ErrorDescription VARCHAR(MAX)
DECLARE @Codes TABLE(Code INT)

IF @EditErrorCodes IS NOT NULL
BEGIN
	INSERT INTO @Codes
	SELECT DISTINCT * FROM dbo.Split(@EditErrorCodes, ',')

	SELECT @ErrorDescription = COALESCE(@ErrorDescription + ',', '') + 'APCEdit: ' + error.ErrorDescription 
	FROM dbo.[ref.MicrodynEditErrorCodes] error WITH (NOLOCK) JOIN @codes code on error.ErrorCode = code.Code
END

IF @PricerErrorCodes IS NOT NULL
BEGIN
	SELECT @ErrorDescription = COALESCE(@ErrorDescription + ',', '') + 'APCPricer: ' + error.ErrorDescription 
	FROM [dbo].[ref.MicrodynPricerErrorCodes] error WITH (NOLOCK) WHERE error.ErrorCode = @PricerErrorCodes
END

RETURN @ErrorDescription
END;