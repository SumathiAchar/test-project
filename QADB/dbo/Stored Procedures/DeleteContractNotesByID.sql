CREATE PROC dbo.DeleteContractNotesByID(
       @ContractNoteID BIGINT,
       @UserName       VARCHAR(50))
AS
    BEGIN
        DECLARE  @TransactionName VARCHAR(100) = 'DeleteContractNotesByID', @Contractid BIGINT;

	   --FIXED-OCT15 Use sql Transaction for Add,Update and delete in AuditLog related SP
		BEGIN TRY
            BEGIN TRAN @TransactionName;
	   --select contract id based on contract note id
        SELECT @Contractid = ContractID
        FROM ContractNotes
        WHERE ContractNoteID = @ContractNoteID;

        --Insert AuditLog information 
      

        DELETE FROM ContractNotes
        WHERE ContractNoteID = @ContractNoteID;

			COMMIT TRANSACTION @TransactionName;
        END TRY
       
	    BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
         END CATCH;

	      EXEC InsertAuditLog
             @UserName,
             'Modify',
             'Contract',
             'Deleted note',
             @Contractid,
             1;
    END;
GO