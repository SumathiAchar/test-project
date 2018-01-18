/****************************************************/

--Method Name : AddEditClaimFieldDocs  
--Module      : Add New Contract  
--Created By  : Vishesh  
--Date        : 07-Sep-2013  
--Modified By :   
--Modified Date:   
--Description : Add Edit a Claim Field Docs  

/****************************************************/

CREATE PROCEDURE [dbo].[AddEditClaimFieldDocs](@ClaimFieldDocID     BIGINT,
                                               @FacilityID          BIGINT,
                                               @FileName            VARCHAR(100),
                                               @TableName           VARCHAR(100),
                                               @ColumnHeaderFirst   VARCHAR(MAX),
                                               @ColumnHeaderSecond  VARCHAR(100),
                                               @ClaimFieldID        BIGINT,
                                               @XmlClaimFieldValues [dbo].[ClaimFieldValues] READONLY,
                                               @UserName            VARCHAR(50))
AS
    BEGIN
        SET NOCOUNT ON;
        DECLARE @CurrentDate    DATETIME = GETUTCDATE(),
                @TableNameCount INT,
                @ClaimToolDesc  VARCHAR(1000);
        SELECT @ClaimFieldDocID = ClaimFieldDocID
        FROM ClaimFieldDocs
        WHERE TableName = @TableName
          AND FacilityID = @FacilityID;
        DECLARE @Transactionname VARCHAR(100) = 'AddEditClaimFieldDocs';
        BEGIN TRY
            BEGIN TRAN @Transactionname;
            IF ISNULL(@ClaimFieldDocID, 0) = 0
                BEGIN
                    DECLARE @Tmptable TABLE( INSERTEDID BIGINT );
                    INSERT INTO dbo.ClaimFieldDocs
                           ( InsertDate,
                             UpdateDate,
                             FacilityID,
                             [FileName],
                             TableName,
                             ColumnHeaderFirst,
                             ColumnHeaderSecond,
                             ClaimFieldID
                           )
                    OUTPUT INSERTED.ClaimFieldDocID
                           INTO @Tmptable
                    VALUES( @CurrentDate, NULL, @FacilityID, @FileName, @TableName, @ColumnHeaderFirst, @ColumnHeaderSecond, @ClaimFieldID );
                    SELECT @ClaimFieldDocID = INSERTEDID
                    FROM @Tmptable;
                END;

            --IF @XmlClaimFieldValues IS NOT NULL
            --BEGIN
            INSERT INTO dbo.ClaimFieldValues
                   ( InsertDate,
                     UpdateDate,
                     FacilityID,
                     ClaimFieldDocID,
                     Identifier,
                     [Value]
                   )
                   SELECT @CurrentDate,
                          NULL,
                          @FacilityID,
                          @ClaimFieldDocID,
                          IDENTIFIER,
                          VALUE
                   FROM @XmlClaimFieldValues;
            --END;
            -- Audit Logging
            SELECT @ClaimFieldDocID AS INSERTEDID;
            SELECT @ClaimToolDesc = 'File Name: ' + C.[FileName] + ', Table Type:' + CASE
                                                                                         WHEN C.ClaimFieldID = 21
                                                                                         THEN 'ASC Fee Schedule'
                                                                                         WHEN C.ClaimFieldID = 22
                                                                                         THEN 'Fee Schedule'
                                                                                         WHEN C.ClaimFieldID = 23
                                                                                         THEN 'DRG Schedule'
                                                                                         WHEN C.ClaimFieldID = 35
                                                                                         THEN 'Custom Table'
                                                                                     END + ', Table Name:' + TableName
            FROM ClaimFieldDocs C
            WHERE C.ClaimFieldDocId = @ClaimFieldDocID;
            EXEC InsertAuditLog
                 @UserName,
                 'Add',
                 'Payment Tables',
                 @ClaimToolDesc,
                 @ClaimFieldDocID,
                 5;

            --Check Any Transaction happened than commit transaction
            COMMIT TRANSACTION @Transactionname;
        END TRY
        BEGIN CATCH		
            --RollBack Transaction
            ROLLBACK TRAN @Transactionname;
            EXEC RaiseErrorInformation;
            --As well as delete all data inserted in ClaimFieldDocs and ClaimFieldValues tables
            DELETE FROM dbo.ClaimFieldValues
            WHERE ClaimFieldDocID = @ClaimFieldDocID;
            DELETE FROM dbo.ClaimFieldDocs
            WHERE ClaimFieldDocID = @ClaimFieldDocID;
        END CATCH;
    END;