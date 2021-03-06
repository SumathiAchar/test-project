
/****************************************************/
  
--Method Name : AddEditContractNotes  
--Module      : Add New ContractNotes  
--ALTER d By  : Girija  
--Date        : 10-Aug-2013  
--Modified By :   
--Modify Date :  
--Date        : 10-Aug-2013  
--Description : Add Edit a new contract Notes  

/****************************************************/

CREATE PROCEDURE [dbo].[AddEditContractNotes]  
(  
	@ContractNoteID BIGINT,  
	-- @InsertDate DATETIME,  
	--@UpdateDate DATETIME,  
	@ContractID BIGINT,  
	@NoteText VARCHAR(MAX),  
	@Status INT,
	@UserName varchar(100)
)   
AS  
BEGIN
DECLARE @Currentutcdatetime DATETIME = GETUTCDATE(),@ClaimToolDesc nvarchar(500);
IF ISNULL(@Contractnoteid, 0) = 0 
		BEGIN
			DECLARE
			   @Tmptable TABLE(
							   InsertedId BIGINT, 
							   InsertDate DATETIME);
			INSERT INTO dbo.ContractNotes(
							 InsertDate, 
							 UpdateDate, 
							 ContractID, 
							 NoteText, 
							 UserName)
			OUTPUT
				   inserted.ContractNoteID, 
				   @Currentutcdatetime
				   INTO @Tmptable
			VALUES(@Currentutcdatetime,  
				   NULL, 
				   @Contractid, 
				   @Notetext, 
				   @Username);

			--Insert AuditLog information 
			SET @ClaimToolDesc= 'Added note: '+ @Notetext;
			EXEC InsertAuditLog @UserName,'Modify','Contract',@ClaimToolDesc ,@Contractid, 1
			SELECT
				   *
			  FROM @Tmptable;
		END;
	ELSE
		BEGIN
			UPDATE dbo.ContractNotes
			SET
				UpdateDate = @Currentutcdatetime, 
				UserName = @Username,
				--ContractID=@ContractID,  
				NoteText = @Notetext
			  WHERE
					ContractNoteID = @Contractnoteid;
			SELECT
				   @@Rowcount;
		END;
END;

