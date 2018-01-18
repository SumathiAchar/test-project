/****************************************************/
  
--Method Name : AddClaimNote 
--Module      : Add New ClaimNote  
--Created By  : Asif Ali  
--Date        : 7-oct-2016  
--Modified By :   
--Modify Date :  
--Date        :7-oct-2016   
--Description : Add a new claim Note  

/****************************************************/
CREATE PROCEDURE [dbo].[AddClaimNote]
(  
	@ClaimID BIGINT,  
	@ClaimNoteText VARCHAR(100),  
	@UserName varchar(100),
	@FacilityName varchar(100)
)   
AS  
BEGIN
DECLARE @Currentutcdatetime DATETIME = GETUTCDATE(),@ClaimToolDesc nvarchar(500),@TransactionName VARCHAR(100) = 'AddClaimNote';
		BEGIN
		BEGIN TRAN @TransactionName;
			DECLARE
			   @Tmptable TABLE(
							   InsertedId BIGINT, 
							   InsertDate DATETIME);
			INSERT INTO dbo.ClaimNotes(
							 InsertDate, 
							 UpdateDate, 
							 ClaimID, 
							 ClaimNoteText, 
							 UserName)
			OUTPUT
				   inserted.ClaimNoteID, 
				   @Currentutcdatetime
				   INTO @Tmptable
			VALUES(@Currentutcdatetime,  
				   NULL, 
				   @ClaimID, 
				   @ClaimNoteText, 
				   @Username);

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
                              'ADD',
                              'Claim Notes',
                              @FacilityName,
                              NULL,
                              NULL,
                              NULL,
                              'Claim Notes Added'
                             );
			SELECT
				   *
			  FROM @Tmptable;

			  COMMIT TRANSACTION @TransactionName;
		END;
END;

