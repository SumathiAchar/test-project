-- =============================================
--Author	  : Dev
--Altered By  : Raj
--Altered DATE: 12-Jun-2014
--Description : Excluded Facility_SSINumber join for -99 Select Criteria
--Added RequestedUserID and RequestedUserName with reference to HIPAA logging feature
--EXEC [dbo].[GetClaimAdjudicationReport] 11985,-1,'2011-03-01','2014-03-01', '-99|2|20140428_0721',20,1,NULL, '8BC0676D-A5A0-440C-A216-A82BC63E79C7', 'jay'
-- =============================================
CREATE PROCEDURE [dbo].[GetClaimAdjudicationReport](
       @ModelID           BIGINT,
       @DateType          INT,
       @DateFrom          DATETIME,
       @DateTo            DATETIME,
       @SelectCriteria    VARCHAR(1000),
       @PageSize          INT,
       @PageIndex         INT,
       @RequestedUserID   UNIQUEIDENTIFIER,
       @RequestedUserName VARCHAR(100),
       @MaxRecordLimit    INT )
AS
    BEGIN
        SET NOCOUNT ON;
        -- Replace Any special Character present in @SelectCriteria
        SET @SelectCriteria = [dbo].[GetXmlParsedString]( @SelectCriteria );
        -- READ UNCOMMITTED option is used to avoid table being blocked during Copy Model and Copy Contract process
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        DECLARE @FacilityID            BIGINT,
                @ClaimSubQuery         NVARCHAR(MAX) = ' ',
                @ClaimIDQuery          NVARCHAR(MAX),
                @ClaimCount            INT,
                @TotalClaimCount       INT,
                @Dateviewd             DATETIME,
                @FacilityName          VARCHAR(500),
                @Maxcount              BIGINT,
                @RecordsCountThreshold INT = -1,
                @ClaimCountQuery       NVARCHAR(MAX),
                @TimeZone              VARCHAR(5) = 'LOCAL',
                @ISODateStyle          INT = 121,
                @PHIAccessLogBatchsize INT = 3000,
                @ClaimToolDesc         VARCHAR(1000);
        DECLARE @Claimlist AS CLAIMLIST;
        DECLARE @Criteria TABLE( SelectedCriteria VARCHAR(MAX));
        DECLARE @Reporttable TABLE( Row_Id                  BIGINT IDENTITY(1, 1)
                                                                   PRIMARY KEY,
                                    PatAcctNum              VARCHAR(100),
                                    ContractID              BIGINT,
                                    ClaimId                 VARCHAR(200),
                                    ContractName            VARCHAR(500),
                                    PriPayerName            VARCHAR(500),
                                    BillType                VARCHAR(4),
                                    DRG                     VARCHAR(3),
                                    LOS                     INT,
                                    StatementFrom           DATETIME,
                                    StatementThru           DATETIME,
                                    ClaimTotal              MONEY,
                                    ClaimTotalCharges       MONEY,
                                    ReimbursementAmount     MONEY,
                                    CalculatedAdj           MONEY,
                                    ActualPmt               MONEY,
                                    ClaimServiceLineID      INT,
                                    RevCode                 VARCHAR(4),
                                    HCPCSCode               VARCHAR(30),
                                    HCPCSModifier           VARCHAR(20),
                                    ServiceFromDate         DATETIME,
                                    ServiceThruDate         DATETIME,
                                    Units                   INT,
                                    AdjudicationStatus      VARCHAR(1000),
                                    ServiceType             VARCHAR(250),
                                    ModelID                 BIGINT,
                                    ContractAdjudicationID  BIGINT,
                                    ContractServiceTypeID   BIGINT,
                                    IsClaimChargeData       BIT,
                                    PatientResponsibility   MONEY,
                                    MedicareSequesterAmount MONEY,
                                    PlaceOfService          VARCHAR(10),
                                    ClaimType               VARCHAR(2024),
                                    ActulAdj                MONEY,
                                    ContractualVariance     MONEY,
                                    PaymentVariance         MONEY );
        DECLARE @Reportresult TABLE( Row_Id                  BIGINT PRIMARY KEY,
                                     PatAcctNum              VARCHAR(100),
                                     ContractID              BIGINT,
                                     ClaimId                 VARCHAR(200),
                                     ContractName            VARCHAR(500),
                                     PriPayerName            VARCHAR(500),
                                     BillType                VARCHAR(4),
                                     DRG                     VARCHAR(3),
                                     LOS                     INT,
                                     StatementFrom           DATETIME,
                                     StatementThru           DATETIME,
                                     ClaimTotalCharges       MONEY,
                                     ReimbursementAmount     MONEY,
                                     CalculatedAdj           MONEY,
                                     ActualPmt               MONEY,
                                     ClaimServiceLineID      INT,
                                     RevCode                 VARCHAR(4),
                                     HCPCSCode               VARCHAR(30),
                                     HCPCSModifier           VARCHAR(20),
                                     ServiceFromDate         DATETIME,
                                     ServiceThruDate         DATETIME,
                                     Units                   INT,
                                     AdjudicationStatus      VARCHAR(1000),
                                     ServiceType             VARCHAR(250),
                                     ModelID                 BIGINT,
                                     ContractAdjudicationID  BIGINT,
                                     ContractServiceTypeID   BIGINT,
                                     IsClaimChargeData       BIT,
                                     CodeSelection           VARCHAR(MAX),
                                     PaymentType             VARCHAR(MAX),
                                     FacilityName            VARCHAR(500),
                                     MaxCount                BIGINT,
                                     IsFormulaError          BIT,
                                     PatientResponsibility   MONEY,
                                     MedicareSequesterAmount MONEY,
                                     PlaceOfService          VARCHAR(10),
                                     ClaimType               VARCHAR(2024),
                                     ActulAdj                MONEY,
                                     ContractualVariance     MONEY,
                                     PaymentVariance         MONEY );

	    
        -- Get facility name and facility based on model selected
        SELECT @FacilityName = hierarchy.NodeText,
               @FacilityID = hierarchy.FacilityID
        FROM dbo.ContractHierarchy hierarchy WITH ( NOLOCK )
             INNER JOIN dbo.ContractHierarchy parenthierarchy WITH ( NOLOCK ) ON hierarchy.NodeID = parenthierarchy.ParentID
        WHERE parenthierarchy.NodeID = @ModelID;

        ---- Build Claim selection query based on ClaimFilters
        SELECT @ClaimIDQuery = dbo.GetClaimCountQuery( @DateType, @DateFrom, @DateTo, @SelectCriteria, @FacilityID, @ModelID );
        -- insert matching claim id's based on search criteria
        SET @ClaimIDQuery = @ClaimIDQuery + ' AND EXISTS (SELECT * FROM ContractAdjudications CA WITH(NOLOCK) WHERE CA.ClaimID = CD.ClaimID and CA.ModelID =' + CONVERT(VARCHAR(20), @ModelID) + ' )';

        ---- insert matching claim ids based on search criteria 					
        INSERT INTO @Claimlist
        EXECUTE sp_executesql
                @ClaimIDQuery;
        SELECT @ClaimCount = @@ROWCOUNT;
        IF @ClaimCount > @MaxRecordLimit
            BEGIN
                SELECT @RecordsCountThreshold AS CountThreshold;
            END;
        ELSE
            BEGIN
                -- Retrieve all claim detail records for selected claim id's
                INSERT INTO @Reporttable
                       SELECT DISTINCT RD.PatAcctNum,
                                       RD.ContractID,
                                       RD.ClaimIdentity,
                                       RD.ContractName,
                                       RD.PriPayerName,
                                       RD.BillType,
                                       RD.DRG,
                                       RD.LOS,
                                       RD.StatementFrom,
                                       RD.StatementThru,
                                       RD.ClaimTotal,
                                       RD.ClaimTotalCharges,
                                       RD.ReimbursementAmount,
                                       RD.CalculatedAdj,
                                       RD.ActualPmt,
                                       RD.ClaimServiceLineID,
                                       RD.RevCode,
                                       RD.HCPCSCode,
                                       RD.HCPCSmods AS HCPCSModifier,
                                       RD.ServiceFromDate,
                                       RD.ServiceThruDate,
                                       RD.Units,
                                       RD.AdjudicationStatus,
                                       CST.ContractServiceTypeName AS ServiceType,
                                       RD.ModelID,
                                       RD.ContractAdjudicationID,
                                       CST.ContractServiceTypeID,
                                       RD.IsClaimChargeData,
                                       RD.PatientResponsibility,
                                       RD.MedicareSequesterAmount,
                                       RD.PlaceOfService,
                                       RD.ClaimType,
                                       RD.ActulAdj,
                                       RD.ContractualVariance,
                                       RD.PaymentVariance
                       FROM( 
                             SELECT DISTINCT C.ContractID,
                                    CD.ClaimID AS ClaimIdentity,
                                    CD.PatAcctNum,
                                    CD.PriPayerName AS PriPayerName,
                                    C.ContractName AS ContractName,
                                    CD.BillType,
                                    CD.DRG,
                                    CD.LOS AS LOS,
                                    CD.StatementFrom AS StatementFrom,
                                    CD.StatementThru AS StatementThru,
                                    CD.ClaimTotal,
                                    CA.ClaimTotalCharges AS ClaimTotalCharges,
                                    CA.AdjudicatedValue AS ReimbursementAmount,
                                    ISNULL(CA.ClaimTotalCharges, 0) - CA.AdjudicatedValue AS CalculatedAdj,
                                                                                             CASE
                                                                                                 WHEN CD.LastremitID IS NOT NULL
                                                                                                 THEN RD.ProvPay
                                                                                                 ELSE Hpmt.Amount
                                                                                             END AS ActualPmt,
                                                                                                    CASE
                                                                                                        WHEN CCDTable.RevCode LIKE '0%'
                                                                                                        THEN RIGHT(CCDTable.RevCode, LEN(CCDTable.RevCode) - 1)
                                                                                                        ELSE CCDTable.RevCode
                                                                                                    END RevCode,
                                    CCDTable.HCPCSCode,
                                    CCDTable.HCPCSmods,
                                    CCDTable.ServiceFromDate,
                                    CCDTable.ServiceThruDate,
                                    CCDTable.Units AS Units,
                                                      CASE
                                                          WHEN CA.MicrodynEditErrorCodes IS NULL
                                                           AND CA.MicrodynPricerErrorCodes IS NULL
                                                          THEN AVS.StatusDefinition + ' ' + ISNULL(CA.MicrodynEditReturnRemarks, '')
                                                          ELSE AVS.StatusDefinition + '. ' + ISNULL(dbo.GetMicrodynStatusDescription( CA.MicrodynEditErrorCodes, CA.MicrodynPricerErrorCodes ), '') + ISNULL(CA.MicrodynEditReturnRemarks, '')
                                                      END AdjudicationStatus,
                                    CA.ModelID,
                                    CA.IsClaimChargeData,
                                    CA.ContractServiceTypeID,
                                    CA.ContractAdjudicationID,
                                    CA.ClaimServiceLineID,
                                    RD.PatientResponsibility,
                                    CA.MedicareSequesterAmount,
                                    CCDTable.PlaceOfService,
                                    CD.ClaimType,
                                    ClaimHeader.ActualContractualAdjustment AS ActulAdj,
                                    ClaimHeader.ContractualVariance AS ContractualVariance,
                                    ClaimHeader.PaymentVariance AS PaymentVariance
                             FROM ClaimData AS CD WITH ( NOLOCK )
                                  LEFT JOIN dbo.ClaimPayVarData ClaimHeader ON ClaimHeader.ClaimID = CD.ClaimID
                                                                           AND ClaimHeader.ModelID = @ModelID
                                  JOIN ContractAdjudications CA WITH ( NOLOCK ) ON CA.ClaimID = CD.ClaimID
                                                                               AND CA.ModelID = @ModelID
                                  JOIN Facility_SSINumber AS FS WITH ( NOLOCK ) ON CONVERT( INT, FS.SSINumber) = CONVERT(INT, CD.SSINumber)
                                  LEFT JOIN Contracts AS C WITH ( NOLOCK ) ON C.ContractID = CA.ContractID
                                  LEFT JOIN ClaimChargeData AS CCDTable WITH ( NOLOCK ) ON CA.ClaimID = CCDTable.ClaimID
                                                                                       AND CCDTable.Line = CA.ClaimServiceLineID
                                  LEFT JOIN dbo.[ref.AdjudicationOrVarianceStatuses] AS AVS WITH ( NOLOCK ) ON AVS.StatusID = CA.ClaimStatus
                                  INNER JOIN @Claimlist ClaimLevelResult ON ClaimLevelResult.claimid = CD.Claimid
                                  LEFT JOIN dbo.RemitData RD WITH ( NOLOCK ) ON CD.lastRemitID = RD.RemitID
                                  LEFT JOIN dbo.vwHIStransactions Hadj WITH ( NOLOCK ) ON Hadj.ClaimID = CD.ClaimID
                                                                                      AND Hadj.TransactionType = 'A'
                                  LEFT JOIN dbo.vwHIStransactions Hpmt WITH ( NOLOCK ) ON Hpmt.ClaimID = CD.ClaimID
                                                                                      AND hpmt.TransactionType = 'P'
                             WHERE FS.FacilityID = @FacilityID ) AS RD
                           LEFT JOIN ContractServiceTypes AS CST WITH ( NOLOCK ) ON RD.ContractServiceTypeID = CST.ContractServiceTypeID
                       ORDER BY RD.ContractAdjudicationID;
                IF @ClaimCount > @MaxRecordLimit
                    BEGIN
                        SELECT @RecordsCountThreshold AS CountThreshold;
                    END;
                ELSE
                    BEGIN
                        SELECT @Maxcount = @ClaimCount;
                        INSERT INTO @Reportresult
                               SELECT DISTINCT Row_Id,
                                      PatAcctNum,
                                      ContractID,
                                      ClaimId,
                                      ContractName,
                                      PriPayerName,
                                      BillType,
                                      DRG,
                                      LOS,
                                      StatementFrom,
                                      StatementThru,
                                      ClaimTotalCharges,
                                      ReimbursementAmount,
                                      CalculatedAdj,
                                      ActualPmt,
                                      ClaimServiceLineID,
                                      RevCode,
                                      HCPCSCode,
                                      HCPCSModifier,
                                      ServiceFromDate,
                                      ServiceThruDate,
                                      Units,
                                      AdjudicationStatus,
                                      CASE
                                          WHEN AdjudicationStatus <> 'Un-adjudicated'
                                          THEN ServiceType
                                          ELSE ''
                                      END AS ServiceType,
                                      ModelID,
                                      ContractAdjudicationID,
                                      ContractServiceTypeID,
                                      IsClaimChargeData,
                                      CASE
                                          WHEN AdjudicationStatus <> 'Un-adjudicated'
                                          THEN CodeSelection
                                          ELSE ''
                                      END AS CodeSelection,
                                             CASE
                                                 WHEN AdjudicationStatus <> 'Un-adjudicated'
                                                 THEN REPLACE(REPLACE(PaymentType, '[', ''), ']', '')
                                                 ELSE ''
                                             END AS PaymentType,
                                      FacilityName,
                                      MaxCount,
                                      IsFormulaError,
                                      PatientResponsibility,
                                      MedicareSequesterAmount,
                                      PlaceOfService,
                                      ClaimType,
                                      ActulAdj,
                                      ContractualVariance,
                                      PaymentVariance
                               FROM( 
                                     SELECT ROW_NUMBER() OVER(ORDER BY Row_Id ASC) AS RowID,
                                            RD.*,
                                            IIF(RD.IsClaimChargeData = 0
                                            AND RD.ContractServiceTypeID IS NULL, CT.ServiceTypeCodes, CST.ServiceTypeCodes) AS CodeSelection,
                                                                                                                                CASE
                                                                                                                                    WHEN smartbox.ExpressionResult IS NULL
                                                                                                                                     AND smartbox.CustomExpressionResult IS NULL
                                                                                                                                    THEN IIF(RD.IsClaimChargeData = 0
                                                                                                                                         AND RD.ContractServiceTypeID IS NULL, RTRIM(LTRIM(CT.PaymentTypeCodes)), IIF(PATINDEX('%Multiplier =%', CST.PaymentTypeCodes) > 0, REPLACE(CST.PaymentTypeCodes, SUBSTRING(CST.PaymentTypeCodes, PATINDEX('%Multiplier =%', CST.PaymentTypeCodes) - 2, 220), ''), LTRIM(RTRIM(CST.PaymentTypeCodes))))
                                                                                                                                    ELSE CASE
                                                                                                                                             WHEN smartbox.ExpressionResult IS NOT NULL
                                                                                                                                              AND smartbox.CustomExpressionResult IS NOT NULL
                                                                                                                                             THEN IIF(RD.IsClaimChargeData = 0
                                                                                                                                                  AND RD.ContractServiceTypeID IS NULL, REPLACE(REPLACE(CT.PaymentTypeCodes COLLATE Latin1_General_CS_AS, smartbox.Expression, smartbox.ExpandedExpression + ' = ' + CONVERT( VARCHAR(MAX), smartbox.ExpressionResult)) COLLATE Latin1_General_CS_AS, smartbox.CustomExpression, smartbox.CustomExpandedExpression + ' = ' + CASE
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 WHEN CHARINDEX('-', CONVERT(VARCHAR(MAX), smartbox.CustomExpressionResult)) > 0
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 THEN REPLACE(CONVERT(VARCHAR(MAX), smartbox.CustomExpressionResult), '-', '($') + ')'
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 ELSE CONVERT(VARCHAR(MAX), smartbox.CustomExpressionResult)
                                                                                                                                                                                                                                                                                                                                                                                                                                                                             END), REPLACE(REPLACE(CST.PaymentTypeCodes COLLATE Latin1_General_CS_AS, smartbox.Expression, smartbox.ExpandedExpression + ' = ' + CONVERT(VARCHAR(MAX), smartbox.ExpressionResult)) COLLATE Latin1_General_CS_AS, smartbox.CustomExpression, smartbox.CustomExpandedExpression + ' = ' + CASE
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            WHEN CHARINDEX('-', CONVERT(VARCHAR(MAX), smartbox.CustomExpressionResult)) > 0
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            THEN REPLACE(CONVERT(VARCHAR(MAX), smartbox.CustomExpressionResult), '-', '($') + ')'
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            ELSE CONVERT(VARCHAR(MAX), smartbox.CustomExpressionResult)
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        END))
                                                                                                                                             ELSE CASE
                                                                                                                                                      WHEN smartbox.ExpressionResult IS NOT NULL
                                                                                                                                                       AND smartbox.CustomExpressionResult IS NULL
                                                                                                                                                      THEN IIF(RD.IsClaimChargeData = 0
                                                                                                                                                           AND RD.ContractServiceTypeID IS NULL, REPLACE(CT.PaymentTypeCodes COLLATE Latin1_General_CS_AS, smartbox.Expression, smartbox.ExpandedExpression + ' = ' + CASE
                                                                                                                                                                                                                                                                                                                          WHEN CHARINDEX('-', CONVERT(VARCHAR(MAX), smartbox.ExpressionResult)) > 0
                                                                                                                                                                                                                                                                                                                          THEN REPLACE(CONVERT(VARCHAR(MAX), smartbox.ExpressionResult), '-', '($') + ')'
                                                                                                                                                                                                                                                                                                                          ELSE FORMAT(CAST(smartbox.ExpressionResult AS DECIMAL(38, 2)), 'g18')
                                                                                                                                                                                                                                                                                                                      END), REPLACE(IIF(PATINDEX('%Multiplier =%', CST.PaymentTypeCodes) > 0, REPLACE(CST.PaymentTypeCodes, SUBSTRING(CST.PaymentTypeCodes, PATINDEX('%Multiplier =%', CST.PaymentTypeCodes) - 2, 220), ''), CST.PaymentTypeCodes) COLLATE Latin1_General_CS_AS, smartbox.Expression, smartbox.ExpandedExpression + ' = ' + CASE
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                WHEN CHARINDEX('-', CONVERT(VARCHAR(MAX), smartbox.ExpressionResult)) > 0
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                THEN REPLACE(CONVERT(VARCHAR(MAX), smartbox.ExpressionResult), '-', '($') + ')'
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                ELSE FORMAT(CAST(smartbox.ExpressionResult AS DECIMAL(38, 4)), 'g18')
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            END + IIF(smartbox.Multiplier > 0, ', Multiplier ' + smartbox.MultiplierExpandedExpression, '') + IIF(smartbox.LimitExpandedExpression IS NOT NULL, +', Limit=' + ISNULL(smartbox.LimitExpandedExpression, '') + ' = ' + CASE
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               WHEN CHARINDEX('-', CONVERT(VARCHAR(MAX), smartbox.LimitExpressionResult)) > 0
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               THEN REPLACE(CONVERT(VARCHAR(MAX), CAST(ROUND(smartbox.LimitExpressionResult, 0) AS INT)), '-', '($') + ')'
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               ELSE CONVERT(VARCHAR(MAX), CAST(ROUND(smartbox.LimitExpressionResult, 0) AS INT))
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           END, '') + CASE smartbox.IsLimitError
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          WHEN 1
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          THEN '(Formula error)'
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          ELSE ''
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      END))
                                                                                                                                                      ELSE IIF(RD.IsClaimChargeData = 0
                                                                                                                                                           AND RD.ContractServiceTypeID IS NULL, REPLACE(CT.PaymentTypeCodes COLLATE Latin1_General_CS_AS, smartbox.CustomExpression, smartbox.CustomExpandedExpression + ' = ' + CASE
                                                                                                                                                                                                                                                                                                                                      WHEN CHARINDEX('-', CONVERT(VARCHAR(MAX), smartbox.CustomExpressionResult)) > 0
                                                                                                                                                                                                                                                                                                                                      THEN REPLACE(CONVERT(VARCHAR(MAX), smartbox.CustomExpressionResult), '-', '($') + ')'
                                                                                                                                                                                                                                                                                                                                      ELSE FORMAT(CAST(smartbox.CustomExpressionResult AS DECIMAL(38, 4)), 'g18')
                                                                                                                                                                                                                                                                                                                                  END), REPLACE(IIF(PATINDEX('%Multiplier =%', CST.PaymentTypeCodes) > 0, REPLACE(CST.PaymentTypeCodes, SUBSTRING(CST.PaymentTypeCodes, PATINDEX('%Multiplier =%', CST.PaymentTypeCodes) - 2, 220), ''), CST.PaymentTypeCodes) COLLATE Latin1_General_CS_AS, smartbox.CustomExpression, smartbox.CustomExpandedExpression + ' = ' + CASE
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        WHEN CHARINDEX('-', CONVERT(VARCHAR(MAX), smartbox.CustomExpressionResult)) > 0
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        THEN REPLACE(CONVERT(VARCHAR(MAX), smartbox.CustomExpressionResult), '-', '($') + ')'
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        ELSE FORMAT(CAST(smartbox.CustomExpressionResult AS DECIMAL(38, 4)), 'g18')
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    END + IIF(smartbox.Multiplier > 0, ', Multiplier ' + smartbox.MultiplierExpandedExpression, '') + IIF(smartbox.LimitExpandedExpression IS NOT NULL, +', Limit=' + ISNULL(smartbox.LimitExpandedExpression, '') + ' = ' + CASE
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       WHEN CHARINDEX('-', CONVERT(VARCHAR(MAX), smartbox.LimitExpressionResult)) > 0
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       THEN REPLACE(CONVERT(VARCHAR(MAX), CAST(ROUND(smartbox.LimitExpressionResult, 0) AS INT)), '-', '($') + ')'
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       ELSE CONVERT(VARCHAR(MAX), CAST(ROUND(smartbox.LimitExpressionResult, 0) AS INT))
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   END, '') + CASE smartbox.IsLimitError
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  WHEN 1
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  THEN '(Formula error)'
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  ELSE ''
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              END))
                                                                                                                                                  END
                                                                                                                                         END
                                                                                                                                END AS PaymentType,
                                            @FacilityName AS FacilityName,
                                            @Maxcount AS MaxCount,
                                            smartbox.ExpressionResult AS ExpressionResult,
                                            smartbox.IsThresholdFormulaError AS IsFormulaError
                                     FROM @Reporttable AS RD
                                          LEFT JOIN ServiceTypePaymentTypeFilterCodes AS CST WITH ( NOLOCK ) ON CST.ContractServiceTypeID = RD.ContractServiceTypeID
                                          LEFT JOIN ServiceTypePaymentTypeFilterCodes AS CT WITH ( NOLOCK ) ON CT.ContractID = RD.ContractID
                                          LEFT JOIN SmartBox smartbox WITH ( NOLOCK ) ON RD.ContractAdjudicationID = smartbox.ContractAdjudicationID ) AS Main
                               ORDER BY Row_Id ASC; -- IMP : Don't remove/change first column (Row_Id) ordering. Report designer code is depending on order of result

                        EXEC GetSummaryReport
                             @ClaimList,
                             @ModelID;
                    END;
                --HIPAA logging--
                EXEC dbo.AddPHIAccessAuditTrails
                     @ClaimList,
                     @RequestedUserID,
                     @RequestedUserName,
                     @PHIAccessLogBatchsize,
                     @TimeZone,
                     @ISODateStyle;
                INSERT INTO @Criteria
                EXEC GetCriteria
                     @SelectCriteria,
                     @Datefrom,
                     @Dateto,
                     @Datetype;
                SELECT @ClaimToolDesc = 'Report: Claim Adjudication, Reporting Criteria : ' + ( 
                                                                                                SELECT TOP 1 ISNULL(SelectedCriteria, '')
                                                                                                FROM @Criteria );
                EXEC InsertAuditLog
                     @Requestedusername,
                     'View',
                     'Reports',
                     @ClaimToolDesc,
                     @Modelid,
                     6;
            END;
        SELECT *
        FROM @Reportresult
        ORDER BY PatAcctNum,
                 ContractAdjudicationID;
        IF @ClaimCount <= @MaxRecordLimit
            BEGIN
                WITH ClaimSummary
                     AS( SELECT DISTINCT CASE
                                             WHEN ClaimHeader.ExpectedPayment IS NULL
                                             THEN 'Un-adjudicated'
                                             WHEN ClaimHeader.Remit IS NULL
                                              AND ClaimHeader.ExpectedPayment IS NOT NULL
                                             THEN C.ContractName + '(No Remit)'
                                             ELSE C.ContractName
                                         END AS ContractName,
                                         ClaimHeader.ClaimId AS ClaimID,
                                         ClaimHeader.ClaimTotalCharges AS ClaimTotalCharges,
                                         ClaimHeader.PatientResponsibility,
                                         ClaimHeader.ExpectedPayment AS CalculatedAllowed,
                                         ClaimHeader.ActualPayment AS ActualPmt,
                                         ClaimHeader.ExpectedContractualAdjustment AS CalculatedAdj,
                                         ClaimHeader.ActualContractualAdjustment AS ActulAdj,
                                         ClaimHeader.ContractualVariance,
                                         ClaimHeader.PaymentVariance
                         FROM dbo.ClaimPayVarData ClaimHeader
                              INNER JOIN @Claimlist ClaimLevelResult ON ClaimLevelResult.ClaimID = ClaimHeader.ClaimID
                              JOIN dbo.Facility_SSINumber AS FS ON CONVERT( INT, FS.SSINumber) = CONVERT(INT, ClaimHeader.SSINumber)
                              LEFT JOIN dbo.Contracts C WITH ( NOLOCK ) ON C.ContractID = ClaimHeader.ContractID
                         WHERE ClaimHeader.ModelId = @ModelID
                           AND FS.FacilityID = @FacilityID )
                     SELECT ContractName,
                            COUNT(ClaimID) AS ClaimCount,
                            SUM(ClaimTotalCharges) AS ClaimTotalCharges,
                            SUM(CalculatedAllowed) AS CalculatedAllowed,
                            SUM(ActualPmt) AS ActualPmt,
                            SUM(PatientResponsibility) AS PatientResponsibility,
                            SUM(ROUND(ContractualVariance, 2)) AS ContractualVariance,
                            SUM(CalculatedAdj) AS CalculatedAdj,
                            SUM(ActulAdj) AS ActulAdj,
                            SUM(ROUND(PaymentVariance, 2)) AS PaymentVariance,
                            @FacilityName AS FacilityName
                     FROM ClaimSummary
                     GROUP BY ContractName
                     ORDER BY ContractName;
            END;
    END;