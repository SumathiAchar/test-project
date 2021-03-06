/****************************************************/

--Method Name : AddEditContractDocs    
--Module      : Add New Contract    
--ALTER d By  : Girija    
--Date        : 09-Aug-2013    
--Modified By :     
--Modified Date:     
--Description : Add Edit a new contract Docs    

/****************************************************/

CREATE PROCEDURE [dbo].[AddEditContractDocs](
       @ContractDocID   BIGINT,
       @ContractID      BIGINT,
       @ContractContent VARBINARY(MAX),
       @FileName        VARCHAR(100),
       @DocumentId      UNIQUEIDENTIFIER,
       @UserName        VARCHAR(50))
AS
BEGIN    
        DECLARE @Currentdate   DATETIME = GETUTCDATE(),
                @ClaimToolDesc NVARCHAR(200);
IF ISNULL(@ContractDocID, 0) = 0   
    BEGIN     
                DECLARE @TmpTable TABLE( ContractDocID BIGINT,
                                         DocumentId    UNIQUEIDENTIFIER );  
  --Inserting contract doc information  
    INSERT INTO [dbo].[ContractDocs]    
                       ( [InsertDate],
                         [UpdateDate],
                         [ContractID],
                         [ContractContent],
                         [DocumentId],
                         [FileName]
     )    
                OUTPUT inserted.ContractDocID,
                       inserted.DocumentId
                       INTO @TmpTable
                VALUES( @Currentdate, NULL, @ContractID, @ContractContent, @DocumentId, @FileName );    
	 --Insert AuditLog information 
                SELECT @ClaimToolDesc = 'Added attachment: ' + @FileName;
                EXEC InsertAuditLog
                     @UserName,
                     'Modify',
                     'Contract',
                     @ClaimToolDesc,
                     @Contractid,
                     1;
                SELECT *
                FROM @TmpTable;
            END;
ELSE   
BEGIN    
  --Updating contract doc information  
                UPDATE [dbo].[ContractDocs]
                  SET UpdateDate = @Currentdate,
                      ContractContent = @ContractContent,
                      [FileName] = @FileName,
  [DocumentId] = @DocumentId   
                WHERE ContractDocID = @ContractDocID;
                SELECT @@ROWCOUNT;
            END;
    END;