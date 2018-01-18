CREATE PROCEDURE DeleteLetterTemplateByID(
       @LetterTemplateID INT,
       @UserName         VARCHAR(50))
AS
    BEGIN
        DECLARE @TransactionName VARCHAR(100) = 'DeleteLetterTemplateByID',
                @ClaimToolDesc   VARCHAR(1000);
        BEGIN TRY
            BEGIN TRAN @TransactionName;
            SELECT @ClaimToolDesc = 'Letter Template: ' + Name
            FROM LetterTemplates
            WHERE LetterTemplateId = @LetterTemplateID; 

            --Insert AuditLog information 
            EXEC InsertAuditLog
                 @UserName,
                 'Delete',
                 'Appeal Letter Templates',
                 @ClaimToolDesc,
                 @LetterTemplateID,
                 2;

            -- Delete letter template based on lettertemplateid
            DELETE FROM LetterTemplates
            WHERE LetterTemplateID = @LetterTemplateID;
            COMMIT TRANSACTION @TransactionName;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;