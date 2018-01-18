/****************************************************/

--Method Name : GetClaimDataByClaimIds
--Module      : Adjudication   
--ALTER d By  : Raj   
--Date        : 12-Sep-2013 
--Modified By : Raj 
--Modify Date : 16-Sep-2013 
--Description : Get Claims Data By based on passed comma separated claim ids 
--EXEC GetClaimDataByClaimIds '477930331',100

/****************************************************/

CREATE PROCEDURE [dbo].[GetClaimDataByClaimIds](
       @ClaimIDs VARCHAR(MAX),
       @TaskID   BIGINT )
WITH RECOMPILE
AS
    BEGIN
        SET NOCOUNT ON;
        DECLARE @ModelID BIGINT;
        SELECT @ModelID = ModelID
        FROM TrackTasks
        WHERE TaskID = @TaskID;
        DECLARE @ClaimTable TABLE( ClaimID BIGINT PRIMARY KEY );
        DECLARE @InActiveRetainContract TABLE( ContractID BIGINT );
        INSERT INTO @ClaimTable
               SELECT *
               FROM dbo.split( @ClaimIDs, ',' );

        /****START****Declare Claim Table *********/

        DECLARE @tblClaimData TABLE( ClaimID                   BIGINT PRIMARY KEY,
                                     ClaimLink                 BIGINT,
                                     ssinumber                 INT,
                                     ClaimType                 VARCHAR(20),
                                     ClaimState                VARCHAR(100),
                                     PayerSequence             INT,
                                     PatAcctNum                VARCHAR(24),
                                     ClaimTotal                MONEY,
                                     EstAmtDue                 MONEY,
                                     PatPaid                   MONEY,
                                     PatAmtDue                 MONEY,
                                     StatementFrom             DATETIME,
                                     StatementThru             DATETIME,
                                     ClaimDate                 DATETIME,
                                     BillDate                  DATETIME,
                                     LastFiledDate             DATETIME,
                                     BillType                  VARCHAR(4),
                                     DRG                       VARCHAR(3),
                                     PriICDDCode               NCHAR(30),
                                     PriICDPCode               VARCHAR(30),
                                     PriPayerName              VARCHAR(100),
                                     SecPayerName              VARCHAR(100),
                                     TerPayerName              VARCHAR(100),
                                     IsDeletedClaim            BIT,
                                     FTN                       VARCHAR(25),
                                     NPI                       VARCHAR(25),
                                     RenderingPHY              VARCHAR(1),
                                     RefPHY                    VARCHAR(1),
                                     AttendingPHY              VARCHAR(1),
                                     ProviderZip               VARCHAR(10),
                                     DischargeStatus           VARCHAR(10),
                                     CustomField1              VARCHAR(25),
                                     CustomField2              VARCHAR(25),
                                     CustomField3              VARCHAR(25),
                                     CustomField4              VARCHAR(25),
                                     CustomField5              VARCHAR(25),
                                     CustomField6              VARCHAR(25),
                                     MRN                       VARCHAR(50),
                                     ICN                       NVARCHAR(30),
                                     ContractID                BIGINT NULL,
                                     Los                       INT,
                                     Age                       TINYINT,
                                     PatientResponsibility     MONEY,
                                     IsClaimAdjudicated        BIT,
                                     LastAdjudicatedContractID BIGINT,
                                     BackgroundContractID      BIGINT );
	
        /****END****Declare Claim Table *********/

        /****START****Declare ClaimChargeData Table *********/

        DECLARE @tblClaimChargeData TABLE( ClaimID          BIGINT NOT NULL,
                                           Line             INT NULL,
                                           RevCode          VARCHAR(4) NULL,
                                           HCPCSCode        VARCHAR(30) NULL,
                                           ServiceFromDate  DATETIME NOT NULL,
                                           ServiceThruDate  DATETIME NOT NULL,
                                           Units            INT NULL,
                                           Amount           MONEY NULL,
                                           NonCoveredCharge MONEY NULL,
                                           CoveredCharge    MONEY NULL,
                                           HCPCSmods        VARCHAR(20),
                                           PlaceOfService   VARCHAR(10));


	   
        --Checks if the Claims are retained to inactive contracts and delete  those contracts from RetainedClaims table
        INSERT INTO @InActiveRetainContract
               SELECT RC.ContractId AS ContractID
               FROM dbo.ClaimData CD WITH ( NOLOCK )
                    JOIN @ClaimTable AS CT ON CT.ClaimID = CD.ClaimID
                    INNER JOIN dbo.RetainedClaims AS RC WITH ( NOLOCK ) ON RC.ClaimID = CD.ClaimID
                                                                       AND RC.ModelID = @ModelID
                    INNER JOIN Contracts C ON C.ContractID = RC.ContractID
               WHERE C.IsActive = 0;
        DELETE FROM RetainedClaims
        WHERE EXISTS( SELECT *
                      FROM @InActiveRetainContract IARC
                      WHERE IARC.ContractID = RetainedClaims.ContractID );

        /****END****Declare ClaimChargeData Table *********/

        INSERT INTO @tblClaimData
               SELECT CD.ClaimID,
                      CD.ClaimLink,
                      CD.ssinumber,
                      CD.ClaimType,
                      CD.ClaimState,
                      CD.PayerSequence,
                      CD.PatAcctNum,
                      CD.ClaimTotal,
                      CD.EstAmtDue,
                      CD.PatPaid,
                      CD.PatAmtDue,
                      CD.StatementFrom,
                      CD.StatementThru,
                      CD.ClaimDate,
                      CD.BillDate,
                      CD.LastFiledDate,
                      CD.BillType,
                      RIGHT('000' + ISNULL(CD.DRG, ''), 3),
                      CD.PriICDDCode,
                      CD.PriICDPCode,
                      REPLACE(CD.PriPayerName, CHAR(160), CHAR(32)),
                      CD.SecPayerName,
                      CD.TerPayerName,
                      0,
                      CD.FTN,
                      CD.NPI,
                      CD.RenderingPHY,
                      CD.RefPHY,
                      CD.AttendingPHY,
                      CD.ProviderZip,
                      CD.DischargeStatus,
                      CD.CustomField1,
                      CD.CustomField2,
                      CD.CustomField3,
                      CD.CustomField4,
                      CD.CustomField5,
                      CD.CustomField6,
                      PD.MedRecNo,
                      R.ICN,
                      ISNULL(TRC.ContractID, RC.ContractID) AS ContractID,
                      CD.Los,
                      dbo.CalculateAge( CAST(PD.DOB AS DATE), CAST(CD.StatementThru AS DATE)) AS Age,
                      ISNULL(R.PatientResponsibility, 0),
                      ISNULL(ACG.IsClaimAdjudicated, 0),
                      ISNULL(LastAdjudicatedContractID, 0) AS LastAdjudicatedContractID,
                      ISNULL(AdjudicateContractID, 0) AS BackgroundContractID
               FROM dbo.ClaimData CD WITH ( NOLOCK )
                    JOIN @ClaimTable AS CT ON CT.ClaimID = CD.ClaimID
                    LEFT JOIN dbo.PatientData AS PD WITH ( NOLOCK ) ON PD.ClaimID = CD.ClaimID
                    LEFT JOIN dbo.RemitData AS R WITH ( NOLOCK ) ON CD.lastRemitID = R.RemitID
                    LEFT JOIN dbo.TaskRetainedClaims AS TRC WITH ( NOLOCK ) ON TRC.ClaimID = CD.ClaimID
                                                                           AND TRC.TaskID = @TaskID
                    LEFT JOIN dbo.RetainedClaims AS RC WITH ( NOLOCK ) ON RC.ClaimID = CD.ClaimID
                                                                      AND RC.ModelID = @ModelID
                    LEFT JOIN Contracts C ON RC.ContractID = C.contractId
                    LEFT JOIN dbo.AdjudicatedClaimsContractID AS ACG ON ACG.ClaimID = CD.ClaimID
                                                                    AND ACG.ModelID = @ModelID
                    LEFT JOIN dbo.TaskClaims AS TC ON TC.TaskID = @TaskID
                                                  AND TC.ClaimID = CD.ClaimID;

        --select claim charge data
        INSERT INTO @tblClaimChargeData
               SELECT CCD.ClaimID,
                      Line,
                      CASE
                          WHEN RevCode LIKE '0%'
                          THEN RIGHT(RevCode, LEN(RevCode) - 1)
                          ELSE RevCode
                      END RevCode,
                      HCPCSCode,
                      ServiceFromDate,
                      ServiceThruDate,
                      Units,
                      Amount,
                      NonCoveredCharge,
                      CoveredCharge,
                      HCPCSmods,
                      PlaceOfService
               FROM dbo.ClaimChargeData CCD WITH ( NOLOCK )
                    JOIN @ClaimTable AS CT ON CT.ClaimID = CCD.ClaimID;
	
	
        --Select Claim Data
        SELECT *
        FROM @tblClaimData;

        --Select Claim Charge Data
        SELECT *
        FROM @tblClaimChargeData
        ORDER BY Line;

        --Select Diagnosis Code
        SELECT ICDD.ClaimID,
               Instance,
               ICDDCode
        FROM dbo.ICDDData ICDD WITH ( NOLOCK )
             JOIN @ClaimTable AS CT ON CT.ClaimID = ICDD.ClaimID;

        --Select Procedure Code	
        SELECT ICDP.ClaimID,
               Instance,
               ICDPCode
        FROM dbo.ICDPData ICDP WITH ( NOLOCK )
             JOIN @ClaimTable AS CT ON CT.ClaimID = ICDP.ClaimID;

        --Select Procedure Code	
        SELECT CPD.*
        FROM dbo.ClaimPhysicianData CPD WITH ( NOLOCK )
             JOIN @ClaimTable AS CT ON CT.ClaimID = CPD.ClaimID;

        --Select Insured Code	
        SELECT ID.*
        FROM dbo.InsuredData ID WITH ( NOLOCK )
             JOIN @ClaimTable AS CT ON CT.ClaimID = ID.ClaimID;

        --Select Value Code 	
        SELECT VC.*
        FROM dbo.ValueCodeData VC WITH ( NOLOCK )
             JOIN @ClaimTable AS CT ON CT.ClaimID = VC.ClaimID;

        --Select Occurrence Code 
        SELECT OC.ClaimID,
               Instance,
               OccurrenceCode AS OccurenceCode
        FROM dbo.OccurrenceCodeData OC WITH ( NOLOCK )
             JOIN @ClaimTable AS CT ON CT.ClaimID = OC.ClaimID;

        --Select ConditionCode Code 	  
        SELECT CC.*
        FROM dbo.ConditionCodeData CC WITH ( NOLOCK )
             JOIN @ClaimTable AS CT ON CT.ClaimID = CC.ClaimID;

        --Select MedicareLap Fee Schedule data
        SELECT HCPCS,
               Amount,
               ProviderZip,
               claimid,
               HCPCSmods
        FROM( 
              SELECT ROW_NUMBER() OVER(PARTITION BY mlab.HCPCS,
                                                    mlab.Modifier,
                                                    cd.ProviderZip,
                                                    cd.claimid ORDER BY mlab.EffDate DESC) RowNumber,
                     cd.claimid AS claimid,
                     cd.ProviderZip AS ProviderZip,
                     ccd.HCPCSCode AS HCPCS,
                     mlab.Amount AS Amount,
                     mlab.EffDate,
                     mlab.Modifier AS HCPCSmods
              FROM @tblClaimData CD
                   JOIN zipcarriermap zcm WITH ( NOLOCK ) ON zcm.zipcode = cd.ProviderZip
                   LEFT JOIN @tblClaimChargeData CCD ON cd.claimid = ccd.claimid
                   JOIN McareLabFeeSched mlab WITH ( NOLOCK ) ON mlab.Carrier = zcm.Carrier
                                                             AND mlab.loc = zcm.Locality
                                                             AND ccd.HCPCSCode = mlab.HCPCS
              WHERE cd.ProviderZip IS NOT NULL
                AND mlab.EffDate <= cd.StatementThru
                AND YEAR(mlab.EffDate) = YEAR(cd.StatementThru)) AS MCareFeeSchedule
        WHERE RowNumber = 1;

        -- Fetch Patient Data   
        SELECT PD.ClaimID,
               PD.LastName,
               PD.FirstName,
               PD.Middle,
               PD.DOB,
               PD.STATUS,
                  CASE
                      WHEN PD.Sex = ' '
                      THEN 0
                      WHEN PD.Sex = 'M'
                      THEN 1
                      WHEN PD.Sex = 'F'
                      THEN 2
                      ELSE-1
                  END AS Sex,
               PD.Medicare
        FROM PatientData PD WITH ( NOLOCK )
             INNER JOIN @tblClaimData CD ON PD.ClaimID = CD.ClaimID;
    END;