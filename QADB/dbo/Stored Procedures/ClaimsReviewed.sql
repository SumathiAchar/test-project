/****************************************************************************
 *   Name         : ClaimsReviewed
 *   Module       : Basic Work flow/Open claims
 *   Description  : Insert/Delete Clamid Information into ClaimReviewed table
 --Exec ClaimsReviewed 127515,1,25014,'jay'
 *****************************************************************************/
CREATE PROCEDURE [dbo].[ClaimsReviewed](    
      @XmlclaimsReviewedList XML
      )
AS
    BEGIN
        SET NOCOUNT ON;
        DECLARE @ClaimToolDesc   VARCHAR(200),
                @Loop            INT = 1,
                @CNT             INT,
                @Transactionname VARCHAR(100) = 'ClaimsReviewed',
			 @ModelID  BIGINT,
			 @UserName VARCHAR(50);
        
	  --Declare table for Temp ClaimDetails
        DECLARE @Temp_ClaimDetails TABLE(ACTION     VARCHAR(100),
                                         ClaimID    BIGINT,
                                         PatAcctNum VARCHAR(24),
                                         ID         INT IDENTITY(1, 1));

	   DECLARE @Tmp_XMLData TABLE ( ClaimID    BIGINT,
							  IsReviewed  BIT	);
	   INSERT INTO @Tmp_XMLData
        SELECT V.X.value( './ClaimId[1]', 'BIGINT' ) AS ClaimId,
               V.X.value( './IsReviewed[1]', 'BIT' ) AS IsReviewed
        FROM @XmlclaimsReviewedList.nodes( '//ClaimsReviewed' ) AS V( X );


	    SELECT TOP(1) @ModelID = V.X.value( './ModelId[1]', 'BIGINT' ),
             @UserName = V.X.value( './UserName[1]', 'VARCHAR(50)' )
        FROM @XmlclaimsReviewedList.nodes( '//ClaimsReviewed' ) AS V( X );

        BEGIN TRY
            BEGIN TRAN @Transactionname;
            MERGE ClaimReviewed CR
            USING @Tmp_XMLData TMP
            ON CR.ClaimId = TMP.ClaimId
            WHEN MATCHED AND TMP.IsReviewed = 0
                  THEN DELETE
            WHEN NOT MATCHED AND TMP.IsReviewed = 1
                  THEN INSERT( ClaimId ) VALUES( ClaimId )
            OUTPUT $action,
                   ISNULL(inserted.ClaimID, deleted.ClaimID),
                   NULL
                   INTO @Temp_ClaimDetails;
            UPDATE @Temp_ClaimDetails
              SET PatAcctNum = CD.PatAcctNum,
                  ACTION = CASE
                               WHEN Tmp.ACTION = 'DELETE'
                               THEN 'Reviewed - unchecked ' + CAST(CD.PatAcctNum AS CHAR)
                               ELSE 'Reviewed - checked ' + CAST(CD.PatAcctNum AS CHAR)
                           END
            FROM @Temp_ClaimDetails Tmp
                 LEFT JOIN [dbo].[ClaimData] CD ON CD.ClaimID = Tmp.ClaimID;
            SELECT @CNT = @@ROWCOUNT;
            WHILE( @Loop <= @CNT )
                BEGIN
                    SELECT @ClaimToolDesc = ACTION
                    FROM @Temp_ClaimDetails
                    WHERE ID = @Loop;
                    EXEC InsertAuditLog
                         @UserName,
                         'Modify',
                         'Claims',
                         @ClaimToolDesc,
                         @ModelID,
                         6;
                    SET @Loop+=1;
                END;
            COMMIT TRANSACTION @Transactionname;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @Transactionname;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;