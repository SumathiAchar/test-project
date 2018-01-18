CREATE FUNCTION [dbo].[GetLatestServiceTypeCodesById]
(
@Contractid BIGINT, 
@Contractservicetypeid BIGINT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
DECLARE
  @Temp_Contractfiltertable TABLE(
  ServiceLineTypeId BIGINT, 
  IncludedCodesValueString VARCHAR(MAX), 
  PaymenttypeId BIGINT);

DECLARE
  @Valuestringdata VARCHAR(MAX);
DECLARE
  @Valueexcludedstringdata VARCHAR(MAX);
DECLARE
  @Fullservicelinecodedata VARCHAR(MAX);
DECLARE
  @Stlist VARCHAR(MAX);
  
--------------------- For BillType ServiceLine------------------------  

SELECT
  @Valuestringdata = CS.IncludedCode, 
  @Valueexcludedstringdata = CS.ExcludedCode
 FROM
  dbo.ContractServiceLinePaymentTypes AS CSL WITH (NOLOCK) JOIN ContractServiceLines AS CS WITH (NOLOCK)ON CS.ContractServiceLineID = CSL.ContractServiceLineID
 WHERE(CSL.ContractID = @Contractid
OR @Contractservicetypeid = CSL.ContractServiceTypeID)
  AND CSL.ServiceLineTypeId = 1;

IF @Valueexcludedstringdata IS NOT NULL
BEGIN
SET @Valueexcludedstringdata = '<> ' + @Valueexcludedstringdata;
END;
IF @Valuestringdata IS NOT NULL
BEGIN
SET @Valuestringdata = '= ' + @Valuestringdata;
END;

SET @Fullservicelinecodedata = CONCAT(COALESCE(@Valuestringdata, ''), CHAR(13) + CHAR(10), COALESCE(@Valueexcludedstringdata, ''));
INSERT INTO @Temp_Contractfiltertable(
ServiceLineTypeId, 
IncludedCodesValueString)
VALUES(1, 
  @Fullservicelinecodedata);

SELECT
  @Valuestringdata = NULL;
SELECT
  @Valueexcludedstringdata = NULL;
SELECT
  @Fullservicelinecodedata = NULL;
--------------------- For RevenueCode ServiceLine ------------------------  

SELECT
  @Valuestringdata =  CS.IncludedCode, 
  @Valueexcludedstringdata =  CS.ExcludedCode
 FROM
  dbo.ContractServiceLinePaymentTypes AS CSL WITH (NOLOCK) JOIN dbo.ContractServiceLines AS CS WITH (NOLOCK)ON CS.ContractServiceLineID = CSL.ContractServiceLineID
 WHERE(CSL.ContractID = @Contractid
OR @Contractservicetypeid = CSL.ContractServiceTypeID)
  AND CSL.ServiceLineTypeId = 2;


IF @Valueexcludedstringdata IS NOT NULL
BEGIN
SET @Valueexcludedstringdata = '<> ' + @Valueexcludedstringdata;
END;
IF @Valuestringdata IS NOT NULL
BEGIN
SET @Valuestringdata = '= ' + @Valuestringdata;
END;

SET @Fullservicelinecodedata = CONCAT(COALESCE(@Valuestringdata, ''), CHAR(13) + CHAR(10), COALESCE(@Valueexcludedstringdata, ''));
INSERT INTO @Temp_Contractfiltertable(
ServiceLineTypeId, 
IncludedCodesValueString)
VALUES(2, 
  @Fullservicelinecodedata);

SELECT
  @Valuestringdata = NULL;
SELECT
  @Valueexcludedstringdata = NULL;
SELECT
  @Fullservicelinecodedata = NULL;
--------------------- For CPT ServiceLine------------------------  
SELECT
  @Valuestringdata =  CS.IncludedCode, 
  @Valueexcludedstringdata =  CS.ExcludedCode
 FROM
  dbo.ContractServiceLinePaymentTypes AS CSL WITH (NOLOCK) JOIN dbo.ContractServiceLines AS CS WITH (NOLOCK)ON CS.ContractServiceLineID = CSL.ContractServiceLineID
 WHERE(CSL.ContractID = @Contractid
OR @Contractservicetypeid = CSL.ContractServiceTypeID)
  AND CSL.ServiceLineTypeId = 3;

IF @Valueexcludedstringdata IS NOT NULL
BEGIN
SET @Valueexcludedstringdata = '<> ' + @Valueexcludedstringdata;
END;
IF @Valuestringdata IS NOT NULL
BEGIN
SET @Valuestringdata = '= ' + @Valuestringdata;
END;

SET @Fullservicelinecodedata = CONCAT(COALESCE(@Valuestringdata, ''), CHAR(13) + CHAR(10), COALESCE(@Valueexcludedstringdata, ''));
INSERT INTO @Temp_Contractfiltertable(
ServiceLineTypeId, 
IncludedCodesValueString)
VALUES(3, 
  @Fullservicelinecodedata);

SELECT
  @Valuestringdata = NULL;
SELECT
  @Valueexcludedstringdata = NULL;
SELECT
  @Fullservicelinecodedata = NULL;
--------------------- For DRG ServiceLine------------------------  
SELECT
  @Valuestringdata =  CS.IncludedCode, 
  @Valueexcludedstringdata =  CS.ExcludedCode
 FROM
  dbo.ContractServiceLinePaymentTypes AS CSL WITH (NOLOCK) JOIN dbo.ContractServiceLines AS CS WITH (NOLOCK)ON CS.ContractServiceLineID = CSL.ContractServiceLineID
 WHERE(CSL.ContractID = @Contractid
OR @Contractservicetypeid = CSL.ContractServiceTypeID)
  AND CSL.ServiceLineTypeId = 4;

IF @Valueexcludedstringdata IS NOT NULL
BEGIN
SET @Valueexcludedstringdata = '<> ' + @Valueexcludedstringdata;
END;
IF @Valuestringdata IS NOT NULL
BEGIN
SET @Valuestringdata = '= ' + @Valuestringdata;
END;

SET @Fullservicelinecodedata = CONCAT(COALESCE(@Valuestringdata, ''), CHAR(13) + CHAR(10), COALESCE(@Valueexcludedstringdata, ''));
INSERT INTO @Temp_Contractfiltertable(
ServiceLineTypeId, 
IncludedCodesValueString)
VALUES(4, 
  @Fullservicelinecodedata);

SELECT
  @Valuestringdata = NULL;
SELECT
  @Valueexcludedstringdata = NULL;
SELECT
  @Fullservicelinecodedata = NULL;
--------------------- For DiagnosisCode ServiceLine------------------------  
SELECT
  @Valuestringdata =  CS.IncludedCode, 
  @Valueexcludedstringdata =  CS.ExcludedCode
 FROM
  dbo.ContractServiceLinePaymentTypes AS CSL WITH (NOLOCK) JOIN dbo.ContractServiceLines AS CS WITH (NOLOCK)ON CS.ContractServiceLineID = CSL.ContractServiceLineID
 WHERE(CSL.CONTRACTID = @Contractid
OR @Contractservicetypeid = CSL.ContractServiceTypeID)
  AND CSL.ServiceLineTypeId = 5;

IF @Valueexcludedstringdata IS NOT NULL
BEGIN
SET @Valueexcludedstringdata = '<> ' + @Valueexcludedstringdata;
END;
IF @Valuestringdata IS NOT NULL
BEGIN
SET @Valuestringdata = '= ' + @Valuestringdata;
END;

SET @Fullservicelinecodedata = CONCAT(COALESCE(@Valuestringdata, ''), CHAR(13) + CHAR(10), COALESCE(@Valueexcludedstringdata, ''));
INSERT INTO @Temp_Contractfiltertable(
ServiceLineTypeId, 
IncludedCodesValueString)
VALUES(5, 
  @Fullservicelinecodedata);

SELECT
  @Valuestringdata = NULL;
SELECT
  @Valueexcludedstringdata = NULL;
SELECT
  @Fullservicelinecodedata = NULL;
--------------------- For Procedure Code ServiceLine------------------------  
SELECT
  @Valuestringdata =  CS.IncludedCode, 
  @Valueexcludedstringdata =  CS.ExcludedCode
 FROM
  dbo.ContractServiceLinePaymentTypes AS CSL WITH (NOLOCK) JOIN dbo.ContractServiceLines AS CS WITH (NOLOCK)ON CS.ContractServiceLineID = CSL.ContractServiceLineID
 WHERE(CSL.ContractID = @Contractid
OR @Contractservicetypeid = CSL.ContractServiceTypeID)
  AND CSL.ServiceLineTypeId = 6;

IF @Valueexcludedstringdata IS NOT NULL
BEGIN
SET @Valueexcludedstringdata = '<> ' + @Valueexcludedstringdata;
END;
IF @Valuestringdata IS NOT NULL
BEGIN
SET @Valuestringdata = '= ' + @Valuestringdata;
END;

SET @Fullservicelinecodedata = CONCAT(COALESCE(@Valuestringdata, ''), CHAR(13) + CHAR(10), COALESCE(@Valueexcludedstringdata, ''));
INSERT INTO @Temp_Contractfiltertable(
ServiceLineTypeId, 
IncludedCodesValueString)
VALUES(6, 
  @Fullservicelinecodedata);

SELECT
  @Valuestringdata = NULL;
SELECT
  @Valueexcludedstringdata = NULL;
SELECT
  @Fullservicelinecodedata = NULL;
--------------------- For Claim Fields ServiceLine ------------------------  
SELECT
  @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') + CF.TEXT + ' ' + CFO.OPERATORTYPE + ' ' + CS.[VALUES]
 FROM
  dbo.ContractServiceLinePaymentTypes AS CSL WITH (NOLOCK) JOIN dbo.ContractServiceLineClaimFieldSelection AS CS WITH (NOLOCK)ON CS.ContractServiceLineID = CSL.ContractServiceLineID
JOIN [ref.ClaimField] AS CF WITH (NOLOCK)ON CF.ClaimFieldID = CS.ClaimFieldID
JOIN [ref.ClaimFieldOperators] AS CFO WITH (NOLOCK)ON CFO.OperatorID = CS.OperatorID
 WHERE(CSL.CONTRACTID = @Contractid
OR @Contractservicetypeid = CSL.ContractServiceTypeID)
  AND CSL.ServiceLineTypeId = 7;


IF @Valuestringdata IS NOT NULL
BEGIN
INSERT INTO  @Temp_Contractfiltertable(
ServiceLineTypeId, 
IncludedCodesValueString)
VALUES(7, 
  @Valuestringdata);
END;

SELECT
  @Valuestringdata = NULL;  



--------------------- For Table Selection ServiceLine------------------------  
SELECT
@Valuestringdata =  COALESCE(@Valuestringdata + ', ', '') + CF.[Text] + ' ' + CFO.OPERATORTYPE + ' ' + CFD.TABLENAME
  
 FROM
  dbo.ContractServiceLinePaymentTypes AS CSL WITH (NOLOCK) JOIN dbo.ContractServiceLineTableSelection AS CS WITH (NOLOCK)ON CS.ContractServiceLineID = CSL.ContractServiceLineID
JOIN ClaimFieldDocs AS CFD WITH (NOLOCK) ON CFD.ClaimFieldDocID = CS.ClaimFieldDocId
JOIN [Ref.ClaimField] AS CF WITH (NOLOCK) ON CF.ClaimFieldID = CS.ClaimFieldId
JOIN [REF.CLAIMFIELDOPERATORS] AS CFO ON CFO.OPERATORID = CS.OPERATOR
 WHERE(CSL.CONTRACTID = @Contractid
OR @Contractservicetypeid = CSL.ContractServiceTypeID)
  AND CSL.ServiceLineTypeId = 8;


IF @Valuestringdata IS NOT NULL
BEGIN
INSERT INTO @Temp_Contractfiltertable(
ServiceLineTypeId, 
IncludedCodesValueString)
VALUES(8, 
  @Valuestringdata);
END;

SELECT
  @Valuestringdata = NULL;
WITH FilterCodes
AS (SELECT distinct @Contractid Contractid,
  @Contractservicetypeid Contractservicetypeid, 
  SLT.ServiceLineName AS FILTERNAME, 
  ISNULL(TempTable.IncludedCodesValueString, '') AS FilterValues,
  (SELECT Max(v) 
   FROM (VALUES (CSL.InsertDate), (CSC.InsertDate), (CST.InsertDate)) AS value(v)) as InsertDate,
   (SELECT Max(v) 
   FROM (VALUES (CSL.UpdateDate), (CSC.UpdateDate), (CST.UpdateDate)) AS value(v)) as UpdateDate
 FROM
  @Temp_Contractfiltertable AS TempTable JOIN dbo.[REF.SERVICELINETYPES] AS SLT WITH (NOLOCK)ON SLT.ServiceLineTypeId = TempTable.ServiceLineTypeId
 JOIN dbo.ContractServiceLinePaymentTypes CSP WITH (NOLOCK)ON CSP.ServiceLineTypeId = TempTable.ServiceLineTypeId
 LEFT JOIN [dbo].[ContractServiceLines] CSL WITH (NOLOCK)ON CSL.ContractServiceLineId = CSP.ContractServiceLineId
 LEFT JOIN [dbo].[ContractServiceLineClaimFieldSelection] CSC WITH (NOLOCK)ON CSC.ContractServiceLineId = CSP.ContractServiceLineId
 LEFT JOIN [dbo].[ContractServiceLineTableSelection] CST WITH (NOLOCK)ON CST.ContractServiceLineId = CSP.ContractServiceLineId
 WHERE CONTRACTID = @Contractid
OR CONTRACTSERVICETYPEID = @Contractservicetypeid) 
 
SELECT top 1 @Stlist = COALESCE(@Stlist + ', ', '') + FILTERNAME + ' ' + FilterValues FROM FilterCodes 
order by CASE WHEN ISNULL(Insertdate,GetDate()) > ISNULL(UpdateDate,GetDate()) THEN Insertdate ELSE UpdateDate END desc;
	

	
	RETURN @Stlist;
END;
