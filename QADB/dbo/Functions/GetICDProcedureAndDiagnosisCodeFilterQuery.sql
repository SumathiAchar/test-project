

-- =============================================
-- Author:		<Author,,Name>
-- ALTER  date: <ALTER  Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE  FUNCTION [dbo].[GetICDProcedureAndDiagnosisCodeFilterQuery]
(
	@ClaimFieldID BIGINT,
	@OperatorType VARCHAR(10),
	@Values VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS 
	BEGIN
		DECLARE @Query VARCHAR(MAX) =  '';
		IF @ClaimFieldID  = 12	-- 12	ICD-9 Diagnosis
		BEGIN 
			SET @Query =  [dbo].[GetFilterCodeQuery]( @Values,' ICD.ICDDCode ', @OperatorType , @ClaimFieldID);
			SET @Query = @Query + ' AND ISNULL(ICD.ICDDCode, '''') <> '''''
		END 
		
		IF @ClaimFieldID  = 13  -- 13	ICD-9 Procedure(I)
			BEGIN 
				SET  @Query =  [dbo].[GetFilterCodeQuery](@Values,' ICP.ICDPCode ', @OperatorType , @ClaimFieldID);
				SET @Query = @Query + ' AND ISNULL(ICP.ICDPCode, '''') <> '''''
			END 
		RETURN @Query;
	END


GO
