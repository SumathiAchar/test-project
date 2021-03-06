-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetOperatorValueBasedOnClaimFieldID]
(
	@ClaimFieldID BIGINT, 
	@OperatorID BIGINT
)
RETURNS VARCHAR(10)
AS

	BEGIN
	DECLARE @DefaultOperatorID BIGINT = 3; -- for equal operator ID is 3
	DECLARE @OperatorType VARCHAR(10);
	-- Patient  Account Number = 1 , Type of Bill (I) = 2, Revenue Code(I) = 3, HCPCS/RATE/HIPPS = 4, Payer Name  = 6, 
	-- Insured’s ID = 7, DRG(I) = 8, Statement covers period(I)- Dates of service(P)  = 16, Place of Service(P) = 9, 
	-- ICD-9 Diagnosis = 12, ICD-9 Procedure(I) = 13, Attending Physician(I) = 14, Value Codes(I) = 17, 
	-- Occurrence Code(I) = 18, Condition Codes(I) = 19 , Insured’s group = 20, 
			IF @ClaimFieldID = 1 OR @ClaimFieldID  = 2 OR @ClaimFieldID = 3 OR @ClaimFieldID = 4 OR @ClaimFieldID = 6 
			OR @ClaimFieldID =  7 OR @ClaimFieldID = 8 OR @ClaimFieldID = 9 OR @ClaimFieldID = 10 
			OR @ClaimFieldID = 11 OR @ClaimFieldID = 12 OR @ClaimFieldID = 13 OR @ClaimFieldID = 14 
			OR @ClaimFieldID = 17 OR @ClaimFieldID = 18 OR @ClaimFieldID = 19 OR @ClaimFieldID = 20
				BEGIN 
					IF @OperatorID = 3 OR @OperatorID = 1 -- 1 = Not Equal & 3 = Equal operator ID
						BEGIN 
							SELECT @OperatorType = OperatorType FROM  [ref.claimfieldoperators]  WHERE  OperatorID = @OperatorID
						END 
					ELSE 
						BEGIN 
							SELECT @OperatorType = OperatorType FROM  [ref.claimfieldoperators]  WHERE  OperatorID = @DefaultOperatorID
						END
				END 
			ELSE 
				BEGIN 
					SELECT @OperatorType = OperatorType FROM  [ref.claimfieldoperators]  WHERE  OperatorID = @OperatorID
				END
		RETURN @OperatorType;
	END




GO
