CREATE PROCEDURE [dbo].[AddEditQueryName](@FacilityID   INT,
                                         @UserID       INT,
                                         @QueryID      INT,
                                         @QueryName    VARCHAR(100),
                                         @FacilityName VARCHAR(200),
                                         @UserName     VARCHAR(200),
                                         @Criteria     VARCHAR(1000))
AS
/****************************************************************************
 *   Name         : AddEditQueryName
 *   Author       : Shivakumar
 *   Date         : 14-Jun-2016
 *   Module       : Report Selection
 *   Description  : Save/ update query name Details
 *****************************************************************************/
     BEGIN
         SET NOCOUNT ON;
         DECLARE @Currentdate DATETIME= GETUTCDATE();
         DECLARE @Transactionname VARCHAR(100)= 'AddEditQueryNames';
         DECLARE @Description AS VARCHAR(500);
	    DECLARE @AuditCriteria Table( SelectCriteria varchar(1000))
         BEGIN TRY
             BEGIN TRAN @Transactionname;
             --If @QueryID is zero that means insert.
             IF(@QueryID = 0)
                 BEGIN
                     IF NOT EXISTS
                     (
                         SELECT 1
                         FROM [dbo].[ReportQuery]
                         WHERE QueryName = @QueryName
                               AND FacilityId = @FacilityID
                               AND UserId = @UserID
                               AND IsDeleted = 0
                     )
                         BEGIN
                             -- Insert ReportQuery details
                             INSERT INTO [dbo].[ReportQuery]
                             (FacilityId,
                              UserId,
                              QueryName,
                              Criteria,
                              IsDeleted,
                              InsertDate,
                              UpdateDate
                             )
                             VALUES
                             (@FacilityID,
                              @UserID,
                              @QueryName,
                              @Criteria,
                              0,
                              @Currentdate,
                              NULL
                             );
                             --If one then inserted is successfull.
                             SELECT 1;

                             -- Inserting the Audit log 
					    INSERT INTO @AuditCriteria(SelectCriteria) 
					    EXEC [dbo].[GetCriteria] @Criteria,'','',null
					    SET @Criteria= (SELECT * FROM @AuditCriteria);
                             SET @Description = @QueryName+': '+@Criteria;
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
                              'Add',
                              'Reports-Query',
                              @FacilityName,
                              NULL,
                              NULL,
                              NULL,
                              @Description
                             );
                         END;
                     ELSE
                         BEGIN
                             ----If -1 then inserted is not successfull.
                             SELECT-1;
                         END;
                 END;
             ELSE
                 BEGIN
                     IF NOT EXISTS
                     (
                         SELECT 1
                         FROM [dbo].[ReportQuery]
                         WHERE QueryName = @QueryName
                               AND FacilityId = @FacilityID
                               AND UserId = @UserID
                               AND IsDeleted = 0
                               AND ReportQueryId <> @QueryID
                     )
                         BEGIN
                             UPDATE [dbo].[ReportQuery]
                               SET
                                   Criteria = @Criteria,
                                   QueryName = @QueryName,
                                   UpdateDate = @Currentdate
                             WHERE ReportQueryId = @QueryID;
                             --If two then updated is successfull.
                             SELECT 2;
                             -- Inserting the Audit log 
					     INSERT INTO @AuditCriteria(SelectCriteria) 
					    EXEC [dbo].[GetCriteria] @Criteria,'','',null
					    SET @Criteria= (SELECT * FROM @AuditCriteria);
                             SET @Description = @QueryName+': '+@Criteria;
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
                              'Modify',
                              'Reports-Query',
                              @FacilityName,
                              NULL,
                              NULL,
                              NULL,
                              @Description
                             );
                         END;
                     ELSE
                         BEGIN
                             ----If -1 then updated is not successfull.
                             SELECT-1;
                         END;
                 END;
             COMMIT TRANSACTION @Transactionname;
         END TRY
         BEGIN CATCH
             --RollBack Transaction
             ROLLBACK TRAN @Transactionname;
             EXEC RaiseErrorInformation;
         END CATCH;
     END;