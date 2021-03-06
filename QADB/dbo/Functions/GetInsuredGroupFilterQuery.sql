
-- =============================================
-- Author:		<Author,,Name>
-- ALTER  date: <ALTER  Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE  FUNCTION [dbo].[GetInsuredGroupFilterQuery]
(
	@ClaimFieldID BIGINT,
	@OperatorType VARCHAR(10),
	@Values VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
	BEGIN

		DECLARE @Query VARCHAR(MAX) =  '';
		IF @ClaimFieldID  = 7	-- 7	Insured’s ID
		BEGIN 
			SET @Query =  [dbo].[GetFilterCodeQuery](@Values,' ID.CertificationNumber ', @OperatorType , @ClaimFieldID );
		END 

		IF @ClaimFieldID  = 20	-- 20	Insured’s group
		BEGIN 
			SET @Query =  [dbo].[GetFilterCodeQuery](@Values,' ID.GroupNumber ', @OperatorType  , @ClaimFieldID);
		END 

		RETURN @Query;
	END


GO
