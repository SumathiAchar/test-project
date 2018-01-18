/****************************************************************************
 *   Name         : ClaimsAllReviewed
 *   Module       : Basic Work flow/Open claims
 *   Description  : Insert Clamid's Information into ClaimReviewed table
 --Exec ClaimsAllReviewed '-99|3|1214',25014,0,null,null,'jay'
 *****************************************************************************/

CREATE PROCEDURE [dbo].[ClaimsAllReviewed](
       @Selectcriteria VARCHAR(MAX),
       @Modelid        VARCHAR(100),
       @Datetype       INT,
       @Startdate      DATETIME,
       @Enddate        DATETIME,
       @Username       VARCHAR(100))
AS
    BEGIN
        SET NOCOUNT ON;

        -- READ UNCOMMITTED option is used to avoid table being blocked during Copy Model and Copy Contract process
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

        -- Implemented input parameters are localized to address "Parameter Sniffing" 
        DECLARE @SelectCriteriaLocal VARCHAR(MAX),
                @ModelIdLocal        VARCHAR(100),
                @DateTypeLocal       INT,
                @StartDateLocal      DATETIME,
                @EndDateLocal        DATETIME,
                @UserNameLocal       VARCHAR(100);
        SET @SelectCriteriaLocal = @SelectCriteria;
        SET @ModelIdLocal = @ModelId;
        SET @DateTypeLocal = @DateType;
        SET @StartDateLocal = @StartDate;
        SET @EndDateLocal = @EndDate;
        SET @UserNameLocal = @UserName;
        DECLARE @Tblselectcriteria TABLE( rowid          INT IDENTITY(1, 1),
                                          selectcriteria VARCHAR(MAX));
        DECLARE @ClaimListWithPagination AS CLAIMLIST;
        DECLARE @ClaimIDList AS CLAIMLIST;
        DECLARE @Criteria TABLE( SelectedCriteria VARCHAR(MAX));
        DECLARE @Facilityid               BIGINT,
                @Totalclaimcountquery     NVARCHAR(MAX),
                @Claimcount               INT,
                @TaskId                   BIGINT,
                @RequestCreteriaDelimiter CHAR(1) = '|',
                @ClaimToolDesc            VARCHAR(1000);
        IF @DateTypeLocal = 0 -- 0-Open claims from Job alert
            BEGIN
                -- Split request creteria by '|' (Ex : -99|2|214719) 

                INSERT INTO @Tblselectcriteria
                       SELECT items
                       FROM dbo.split( @SelectCriteriaLocal, @RequestCreteriaDelimiter );
                SELECT TOP 1 @TaskId = selectcriteria
                FROM @Tblselectcriteria
                ORDER BY rowid DESC;

                --Read job task details based on request criteria task id

                SELECT @Claimcount = TotalClaimCount,
                       @StartDateLocal = datefrom,
                       @EndDateLocal = dateto,
                       @Facilityid = facilityid,
                       @ModelIdLocal = modelid
                FROM dbo.TrackTasks
                WHERE TaskID = @TaskId;
            END;
        ELSE -- >0 -Open claims from Request adjudication tab

            BEGIN
                SELECT @Facilityid = FacilityID
                FROM dbo.ContractHierarchy
                WHERE NodeID = @ModelIdLocal;
            END;

        ---- Build Claim selection query based on ClaimFilters

        SELECT @TotalClaimCountQuery = dbo.GetClaimCountQuery( @DateTypeLocal, @StartDateLocal, @EndDateLocal, @SelectCriteriaLocal, @Facilityid, @ModelIdLocal );

        -- insert matching claim ids based on search criteria

        INSERT INTO @ClaimIDList
        EXECUTE sp_executesql
                @Totalclaimcountquery;
        SELECT @Claimcount = @@ROWCOUNT;
        DELETE CL
        FROM @ClaimIDList CL
        WHERE EXISTS( SELECT 1
                      FROM ClaimReviewed
                      WHERE ClaimID = CL.ClaimID );
        SELECT @Claimcount = @Claimcount - @@ROWCOUNT;
        IF( @Claimcount > 0 )
            BEGIN
                -- Inserting data into the ClaimReviewed table
                INSERT INTO ClaimReviewed
                       SELECT CLAIMID
                       FROM @ClaimIDList;
                -- Insert audit log
                INSERT INTO DBO.AuditLogs
                       ( LoggedDate,
                         UserName,
                         ACTION,
                         ObjectType,
                         FacilityName,
                         ModelName,
                         ContractName,
                         ServiceTypeName,
                         [Description]
                       )
                       SELECT GETUTCDATE(),
                              @UserName,
                              'Modify',
                              'Claims',
                              FacilityName,
                              ModelName,
                              ContractName,
                              ServiceTypeName,
                              'Reviewed - checked ' + CONVERT( VARCHAR(30), CD.PatAcctNum)
                       FROM @ClaimIDList CIL
					   INNER JOIN ClaimData CD ON CIL.ClaimID=CD.ClaimID
                            CROSS JOIN dbo.GetAuditLogInfoByID( @ModelID, 6 );
            END;
    END;