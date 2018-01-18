/****************************************************/
  --Method Name : DeleteClaimNoteByID 
--Module      : Delete a ClaimNote  
--Created By  : Asif Ali  
--Date        : 7-oct-2016  
--Modified By :   
--Modify Date :  
--Date        :7-oct-2016   
--Description : Delete a new claim Note  

/****************************************************/
CREATE PROCEDURE [dbo].[DeleteClaimNoteByID]
(
	@ClaimNoteID BIGINT,
	@UserName VARCHAR(100),
	@FacilityName VARCHAR(100)
)
As
BEGIN
 DECLARE  @TransactionName VARCHAR(100) = 'DeleteClaimNoteByID', @ClaimID BIGINT;


BEGIN TRY
            BEGIN TRAN @TransactionName;
        SELECT @ClaimID = ClaimID
        FROM ClaimNotes
        WHERE ClaimNoteID = @claimNoteID;

        --Insert AuditLog information 
         INSERT INTO [dbo].[AuditLogs]
                             (LoggedDate,
                              Username,
                              ACTION,
                              ObjectType,
                              FacilityName,
                              ModelName,
                              ContractName,
                              ServiceTypeName,
                              Description
                             )
                             VALUES
                             (GETUTCDATE(),
                              @UserName,
                              'Modify',
                              'Claim Notes',
                              @FacilityName,
                              NULL,
                              NULL,
                              NULL,
                              'Claim Notes Deleted'
                             );

        UPDATE ClaimNotes SET IsDeleted=1, UpdateDate=GETUTCDATE()
	   WHERE ClaimNoteID = @claimNoteID;
        
	   	COMMIT TRANSACTION @TransactionName;
END TRY
       
	    BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;
GO
