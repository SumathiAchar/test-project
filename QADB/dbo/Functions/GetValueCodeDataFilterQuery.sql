
-- =============================================
-- Author:		<Author,,Name>
-- ALTER  date: <ALTER  Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE  FUNCTION [dbo].[GetValueCodeDataFilterQuery]
(
	@ClaimFieldID BIGINT,
	@OperatorType VARCHAR(10),
	@Values VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS 
	BEGIN
		DECLARE @Query VARCHAR(MAX) =   [dbo].[GetFilterCodeQuery]( @Values,' VCD.ValueCode ', @OperatorType , @ClaimFieldID);
		SET @Query = @Query + ' AND ISNULL(VCD.ValueCode, '''') <> '''''
		RETURN @Query;
	END
GO
