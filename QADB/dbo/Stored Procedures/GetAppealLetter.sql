-- =============================================
--Name        : GetAppealLetter
--Author	  : Dev
--Created By  : Raj
--Created Date: 4-Nov-2014
--Description : Getting requested claim data as well as letter data
--EXEC GetAppealLetter 60,40342,-1,'2011-11-06 13:15:27.153','2014-11-06 13:15:27.153',N'-99|2|116954',NULL,N'5b2cc86a-b194-45c9-b1c2-77abc66d8631',N'jay',50
-- =============================================
CREATE PROCEDURE dbo.GetAppealLetter(@LetterTemplateID  BIGINT,
                                     @ModelID           BIGINT,
                                     @DateType          INT,
                                     @DateFrom          DATETIME,
                                     @DateTo            DATETIME,
                                     @SelectCriteria    VARCHAR(1000),
                                     @RequestedUserID   UNIQUEIDENTIFIER,
                                     @RequestedUserName VARCHAR(100),
                                     @MaxRecordLimit    INT)
AS
     BEGIN
         SET NOCOUNT ON;
         DECLARE @InsurerData TABLE
         (ClaimID            BIGINT,
          PrimaryMemberID    VARCHAR(25),
          PrimaryGroupNumber VARCHAR(50)
         );

	    --To Convert Special Character to Xml strings
        SET @SelectCriteria= [dbo].[GetXmlParsedString] (@SelectCriteria)

         -- READ UNCOMMITTED option is used to avoid table being blocked during Copy Model and Copy Contract process
         SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
         DECLARE @ClaimIDQuery NVARCHAR(MAX), @ClaimSubQuery NVARCHAR(MAX)= ' ', @ClaimCount INT, @FacilityID BIGINT, @FacilityName VARCHAR(200), @CurrentDate DATETIME= GETUTCDATE(), @TimeZone VARCHAR(5)= 'LOCAL', @ISODateStyle INT= 121, @PHIAccessLogBatchsize INT= 3000, @ClaimToolDesc VARCHAR(1000);
         DECLARE @ClaimList AS CLAIMLIST;
         DECLARE @Criteria TABLE(SelectedCriteria VARCHAR(MAX));
         SELECT @FacilityID = FacilityID
         FROM dbo.ContractHierarchy
         WHERE NodeID = @ModelID;
         SELECT @FacilityName = NodeText
         FROM dbo.ContractHierarchy
         WHERE FacilityID = @FacilityID
               AND ParentID = 0;
         SELECT @ClaimSubQuery = dbo.GetSubQueryForClaimFilters(@DateType, @DateFrom, @DateTo, @SelectCriteria, @FacilityID,@ModelID);
         SET @ClaimSubQuery = STUFF(@ClaimSubQuery, CHARINDEX('SELECT DISTINCT CD.claimid C FROM ClaimData AS CD', @ClaimSubQuery), LEN('SELECT DISTINCT CD.claimid C FROM ClaimData AS CD'), '');
         IF CHARINDEX('-99', @SelectCriteria) > 0
             BEGIN
                 SET @ClaimSubQuery = STUFF(@ClaimSubQuery, CHARINDEX(' JOIN Facility_SSINumber AS FS ON CONVERT(INT,FS.SSINumber) = CONVERT(INT,CD.SSINumber) ', @ClaimSubQuery), LEN(' JOIN Facility_SSINumber AS FS ON CONVERT(INT,FS.SSINumber) = CONVERT(INT,CD.SSINumber) '), '');
                 SET @ClaimSubQuery = STUFF(@ClaimSubQuery, CHARINDEX('AND FS.FacilityID = '+CONVERT(VARCHAR(20), @FacilityID), @ClaimSubQuery), LEN('AND FS.FacilityID = '+CONVERT(VARCHAR(20), @FacilityID)), '');
             END;
	
	
         --Get Claim Sub query based on selected criteria
         SET @ClaimIDQuery = 'SELECT DISTINCT CD.ClaimID FROM ClaimData AS CD '+@ClaimSubQuery; 
         --Get selected claims into @ClaimList Table 					
         INSERT INTO @ClaimList
         EXECUTE sp_executesql
                 @ClaimIDQuery;
         SELECT @ClaimCount = COUNT(*)
         FROM @ClaimList;
         IF @ClaimCount > @MaxRecordLimit
             BEGIN
                 SELECT-1 AS CountThreshold;
             END;
         ELSE
             BEGIN

                 --Select Template text based on @LetterTemplateID
                 SELECT TemplateText AS LetterTemplaterText
                 FROM dbo.LetterTemplates
                 WHERE LetterTemplateID = @LetterTemplateID;
                 WITH InsuredData
                      AS(SELECT ROW_NUMBER() OVER(PARTITION BY CD.ClaimID ORDER BY CD.ClaimID) AS rownumber,
                                CD.ClaimId,
                                INSD.CertificationNumber AS PrimaryMemberID,
                                INSD.GroupNumber AS PrimaryGroupNumber
                         FROM dbo.ClaimData AS CD
                              INNER JOIN @Claimlist CL ON CL.ClaimID = cd.ClaimID
                              LEFT JOIN dbo.InsuredData AS INSD ON INSD.ClaimID = CD.ClaimID)
                      INSERT INTO @InsurerData
                      (ClaimID,
                       PrimaryMemberID,
                       PrimaryGroupNumber
                      )
                             SELECT ClaimId,
                                    PrimaryMemberID,
                                    PrimaryGroupNumber
                             FROM InsuredData
                             WHERE rownumber = 1;
			
                 --Select claim data 
                 SELECT CD.BillDate,
                        CD.BillType,
                        CD.ClaimID,
                        C.ContractName,
                        CD.DRG,
                        CA.AdjudicatedValue AS ExpectedAllowed,
                        CD.FTN,
                        '' AS GroupNumber,
                        IND.PrimaryMemberID AS PrimaryMemberID,
                        NULL AS SecondaryMemberID,
                        NULL AS TertiaryMemberID,
                        IND.PrimaryGroupNumber AS PrimaryGroupNumber,
                        NULL AS SecondaryGroupNumber,
                        NULL AS TertiaryGroupNumber,
                        PD.MedRecNo AS MedRecNumber,
                        CD.NPI,
                        PD.AcctNum AS PatientAccountNumber,
                        PD.DOB AS PatientDOB,
                        PD.FirstName AS PatientFirstName,
                        PD.LastName AS PatientLastName,
                        PD.Middle AS PatientMiddle,
                        RD.PatientResponsibility,
                        CD.PriPayerName,
                        REPLACE(REPLACE(@FacilityName, CHAR(150), '-'), CHAR(150), '-') AS ProviderName,
                        RD.checkdate AS RemitCheckDate,
                        RD.ProvPay AS RemitPayment,
                        RD.ICN,
                        CD.StatementFrom,
                        CD.StatementThru,
                        CD.Los AS Los,
                        CD.ClaimTotal,
				    dbo.CalculateAge(CAST(PD.DOB AS DATE), CAST(CD.StatementThru AS DATE)) AS Age
                 FROM ClaimData CD
                      JOIN @ClaimList CL ON CD.ClaimID = CL.ClaimID
                      LEFT JOIN ContractAdjudications CA ON CA.ClaimID = CD.ClaimID
                                                            AND CA.ModelId = @ModelID
                                                            AND CA.ContractServiceTypeID IS NULL
                                                            AND CA.ClaimServiceLineID IS NULL
                      LEFT JOIN Contracts C WITH (NOLOCK) ON C.ContractID = CA.ContractID
                      LEFT JOIN @InsurerData IND ON IND.ClaimId = CD.ClaimId
                      LEFT JOIN PatientData PD ON PD.ClaimID = CD.ClaimID
                      LEFT JOIN RemitData RD ON CD.lastRemitID = RD.RemitID;						


                 --Hippa Login
                 EXEC dbo.AddPHIAccessAuditTrails
                      @ClaimList,
                      @RequestedUserID,
                      @RequestedUserName,
                      @PHIAccessLogBatchsize,
                      @TimeZone,
                      @ISODateStyle;  

                 --Audit Logging
                 INSERT INTO @Criteria
                 EXEC GetCriteria
                      @SelectCriteria,
                      @DateFrom,
                      @DateTo,
                      @DateType;
                 SELECT @ClaimToolDesc = 'Report: Letter Template, Reporting Criteria : '+
                 (
                     SELECT TOP 1 SelectedCriteria
                     FROM @Criteria
                 );
                 EXEC InsertAuditLog
                      @Requestedusername,
                      'View',
                      'Reports',
                      @ClaimToolDesc,
                      @Modelid,
                      6;
             END;
         SET NOCOUNT OFF;
     END;