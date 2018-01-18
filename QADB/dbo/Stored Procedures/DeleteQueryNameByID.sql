CREATE PROCEDURE [dbo].[DeleteQueryNameByID](@QueryID      INT,
                                             @QueryName    VARCHAR(100),
                                             @FacilityName VARCHAR(200),
                                             @UserName     VARCHAR(200))
AS

/****************************************************************************
 *   Name         : DeleteQueryNameByID
 *   Author       : Shivakumar
 *   Date         : 14-Jun-2016
 *   Module       : Report Selection
 *   Description  : Delete query name Details
 *	Test		   : Exec DeleteQueryNameByID 2
 *****************************************************************************/

     SET NOCOUNT ON;
     BEGIN
         DECLARE @AuditCriteria TABLE(SelectCriteria VARCHAR(1000));
         DECLARE @Transactionname VARCHAR(100)= 'DeleteQueryNameByID';
         DECLARE @Description VARCHAR(500);
         DECLARE @Criteria VARCHAR(1000);
         DECLARE @Currentdate DATETIME= GETUTCDATE();
         BEGIN TRY
             BEGIN TRAN @Transactionname;
             UPDATE [dbo].[ReportQuery]
               SET
                   IsDeleted = 1
             WHERE ReportQueryId = @QueryID;
             --If 1 delete is successfull.
             SELECT 1;
             -- Inserting the Audit log
             SET @Criteria =
             (
                 SELECT Criteria
                 FROM ReportQuery
                 WHERE ReportQueryId = @QueryID
             );
             INSERT INTO @AuditCriteria(SelectCriteria)
             EXEC [dbo].[GetCriteria]
                  @Criteria,
                  '',
                  '',
                  NULL;
             SET @Description = @QueryName+': '+
             (
                 SELECT SelectCriteria
                 FROM @AuditCriteria
             );
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
             (@Currentdate,
              @UserName,
              'Delete',
              'Reports-Query',
              @FacilityName,
              NULL,
              NULL,
              NULL,
              @Description
             );
             COMMIT TRANSACTION @Transactionname;
         END TRY
         BEGIN CATCH
             --RollBack Transaction
             ROLLBACK TRAN @Transactionname;
             EXEC RaiseErrorInformation;
         END CATCH;
     END;