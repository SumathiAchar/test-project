CREATE PROCEDURE [dbo].[GetAllClaimFieldsMaintainPayment](
@Showfields INT)
AS    

BEGIN
		SELECT
				CLAIMFIELDID, 
				TEXT
			FROM [REF.CLAIMFIELD]
			WHERE 
			CLAIMFIELDID in(21,22,23,35, -1)
			AND IsClaimField=0;
END;