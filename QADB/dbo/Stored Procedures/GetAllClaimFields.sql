CREATE PROCEDURE dbo.GetAllClaimFields(
@ShowFields INT)
AS    

/****************************************************************************    
 *   Name         : GetAllClaimFields   
 *   Author       : Vishesh    
 *   Date         : 19/Aug/2013    
 *   Module       : Get All Claim Fields    
 *   Description  : Get ClaimFields Information from database    
 --EXEC [dbo].[GetAllClaimFields] NULL
 *****************************************************************************/

BEGIN
	IF @ShowFields IS NULL
		BEGIN -- show only standard claimfields
			SELECT
				   ClaimFieldID, 
				   [Text]
			  FROM [ref.ClaimField]
			  WHERE IsClaimField = 1;
		END
	ELSE
		BEGIN
			IF @ShowFields < 0--Show standard claimfields and Reports Selection claimfields
				BEGIN 
					IF	@ShowFields=-99
					BEGIN
					  SELECT
							 ClaimFieldID, 
							 [Text]
						FROM [ref.ClaimField]
						WHERE IsClaimField = 1
					  UNION
					  SELECT
							 ClaimFieldID, 
							 [Text]
						FROM [ref.ClaimField]
						WHERE ClaimFieldID BETWEEN 25 AND 28
					  UNION
					  SELECT
							 ClaimFieldID, 
							 [Text]
						FROM [ref.ClaimField]
						WHERE ClaimFieldID =@ShowFields;
					END
					ELSE
					BEGIN
						SELECT
							 ClaimFieldID, 
							 [Text]
						FROM [ref.ClaimField]
						WHERE IsClaimField = 0
							AND  ClaimFieldID >= -1;
					END
				END;
			ELSE
				BEGIN
					IF @ShowFields = 0--Show standard claimfields and Import claimfields
						BEGIN
							SELECT
								   ClaimFieldID, 
								   [Text]
							  FROM [ref.ClaimField]
							  WHERE 
								ClaimFieldID IN (21,22,23,35)
								AND IsClaimField=0;
						END;
				END;
		END;
END;