/****************************************************/

--Method Name AddEditContractPayerInfo   select * from [ContractInfo]  
--Module      Add New Contract    
--ALTER d By  Ragini Bhandari    
--Date    09-Aug-2013    
--Description Add Edit a new contract payer info    

/****************************************************/

CREATE PROCEDURE [dbo].[AddEditContractPayerInfo](
       @ContractPayerInfoId   BIGINT,
       @ContractID            BIGINT,
       @ContractPayerID       BIGINT,
       @ContractPayerInfoName VARCHAR(100),
       @MailAddress1          VARCHAR(100),
       @MailAddress2          VARCHAR(100),
       @City                  VARCHAR(100),
       @State                 VARCHAR(100),
       @Zip                   VARCHAR(100),
       @Phone1                VARCHAR(100),
       @Phone2                VARCHAR(100),
       @Fax                   VARCHAR(100),
       @Email                 VARCHAR(100),
       @Website               VARCHAR(100),
       @TaxID                 VARCHAR(100),
       @NPI                   VARCHAR(100),
       @Memo                  VARCHAR(100),
       @ProvderID             VARCHAR(100),
       @PlanId                VARCHAR(100),
       @UserName              VARCHAR(100))
AS
    BEGIN
        DECLARE @Currentdate DATETIME = GETUTCDATE(),
                @Action      NCHAR(10),
                @Description VARCHAR(100);
        IF ISNULL(@ContractPayerInfoId, 0) = 0
            BEGIN
                SET @Action = 'Modify';
                SET @Description = 'Added Contact : ' + @ContractPayerInfoName;
                DECLARE @TmpTable TABLE( InsertedId BIGINT );
                INSERT INTO [dbo].[ContractInfo]
                       (    
                --[ContractPayerInfoID]  
                [ContractInfoName],
                [InsertDate],
                [UpdateDate],
                [ContractID],
                [MailAddress1],
                [MailAddress2],
                [City],
                [State],
                [Zip],
                [Phone1],
                [Phone2],
                [Fax],
                [Email],
                [Website],
                [TaxID],
                [NPI],
                [Memo],
                [ProvderID],
                [PlanId]
                       )
                OUTPUT inserted.ContractInfoID
                       INTO @TmpTable
                VALUES( @ContractPayerInfoName, @Currentdate, NULL, @ContractID, @MailAddress1, @MailAddress2, @City, @State, @Zip, @Phone1, @Phone2, @Fax, @Email, @Website, @TaxID, @NPI, @Memo, @ProvderID, @PlanId );
                SELECT *
                FROM @TmpTable;
            END;
        ELSE
            BEGIN
                SET @Action = 'Modify';
                SET @Description = 'Contact modified : ' + @ContractPayerInfoName;
                UPDATE ContractInfo
                  SET ContractInfoName = @ContractPayerInfoName,
                      UpdateDate = @Currentdate,
                      MailAddress1 = @MailAddress1,
                      MailAddress2 = @MailAddress2,
                      City = @City,
                      [State] = @State,
                      Zip = @Zip,
                      Phone1 = @Phone1,
                      Phone2 = @Phone2,
                      Fax = @Fax,
                      Email = @Email,
                      Website = @Website,
                      TaxID = @TaxID,
                      NPI = @NPI,
                      Memo = @Memo,
                      ProvderID = @ProvderID,
                      PlanId = @PlanId
                WHERE ContractInfoID = @ContractPayerInfoId;

                --Return Inserted ContractPayerInfoId
                SELECT @ContractPayerInfoId;
            END;    
    
        --Insert AuditLog information 
        EXEC InsertAuditLog
             @UserName,
             @Action,
             'Contract',
             @Description,
             @Contractid,
             1;
    END;