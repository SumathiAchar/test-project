

-- =============================================
-- Author:		<Author,,Name>
-- ALTER  date: <ALTER  Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE  FUNCTION [dbo].[GetOccurrenceAndConditionCodeFilterQuery]
(
	@ClaimFieldID BIGINT,
	@OperatorType VARCHAR(10),
	@Values VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS 
	BEGIN
		DECLARE @Query VARCHAR(MAX) =  '';
		IF @ClaimFieldID = 18  -- 18 Occurrence Code(I)
			BEGIN 
			  
				SET @Query = [dbo].[GetFilterCodeQuery]( @Values,' OCD.OccurrenceCode ', @OperatorType , @ClaimFieldID);
				SET @Query = @Query + ' AND ISNULL(OCD.OccurrenceCode, '''') <> '''''
				RETURN @Query;
			END
		IF @ClaimFieldID = 19  -- 19 Condition Codes(I)
			BEGIN  
				SET @Query = [dbo].[GetFilterCodeQuery]( @Values,' CC.ConditionCode ', @OperatorType  , @ClaimFieldID);
				SET @Query = @Query + ' AND ISNULL(CC.ConditionCode, '''') <> '''''
			END
		RETURN @Query;
	END
GO
