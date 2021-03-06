/****************************************************************************
 *   Name         : AddEditLetterTemplate
 *   Author       : Raj
 *   Date         : 30-Oct-2014
 *   Module       : Add/Edit Letter Template
 *   Description  : Insert or Update Letter Template Information into database
 *****************************************************************************/

CREATE PROCEDURE [dbo].[AddEditLetterTemplate](
       @LetterTemplateID BIGINT,
       @Name             VARCHAR(200),
       @TemplateText     VARCHAR(MAX),
       @UserName         VARCHAR(100),
       @FacilityID       BIGINT )
AS
    BEGIN
        SET NOCOUNT ON;
        DECLARE @CurrentDate   DATETIME = GETUTCDATE(),
                @Action        VARCHAR(10),
                @ClaimToolDesc NVARCHAR(1000);

        --If @LetterTemplatelID equal to 0 than need to insert new record
        IF( @LetterTemplateID = 0 )
            BEGIN
                DECLARE @Tmptable TABLE( InsertedID BIGINT );

                --insert into LetterTemplates table
                INSERT INTO dbo.LetterTemplates
                       ( Name,
                         TemplateText,
                         InsertDate,
                         UpdateDate,
                         UserName,
                         FacilityID
                       )
                OUTPUT INSERTED.LetterTemplateID
                       INTO @Tmptable -- inserting id into @TmpTable
                VALUES( @Name, @TemplateText, @CurrentDate, NULL, @UserName, @FacilityID );

                --Select inserted value
                SELECT InsertedID
                FROM @TmpTable;
                SET @Action = 'Add';
                SET @ClaimToolDesc = 'Letter Template Added: ' + @Name;
                SELECT @LetterTemplateID = InsertedID
                FROM @Tmptable;
            END
                --Else update existing record
                ;
        ELSE
            BEGIN
                --update LetterTemplate information
                UPDATE dbo.LetterTemplates
                  SET UpdateDate = @CurrentDate,
                      TemplateText = @TemplateText,
                      FacilityID = @FacilityID
                WHERE LetterTemplateID = @LetterTemplateID;
                SELECT @LetterTemplateID;
                SET @Action = 'Modify';
                SET @ClaimToolDesc = 'Letter Template Modified: ' + @Name;
            END;
        --Audit Logging
        SELECT @UserName,
               @ClaimToolDesc,
               @Action,
               @ClaimToolDesc;
        EXEC InsertAuditLog
             @UserName,
             @Action,
             'Appeal Letter Templates',
             @ClaimToolDesc,
             @LetterTemplateID,
             2;
    END;