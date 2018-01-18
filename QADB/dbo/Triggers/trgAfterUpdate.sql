CREATE TRIGGER dbo.trgAfterUpdate ON dbo.Contracts
	FOR UPDATE
AS
DECLARE
   @Contractid BIGINT;
DECLARE
   @Thresholdtoexpirealters INT;
DECLARE
   @Currentdate DATETIME = GETUTCDATE();


--Select updated Contract Id
SELECT
	   @Contractid = I.ContractID, 
	   @Thresholdtoexpirealters = I.ThresholdDaysToExpireAlters
  FROM INSERTED I; 

--Update Contrat Alert Data
UPDATE DBO.CONTRACTALERTS
SET
	ISDISMISSED = NULL, 
	ENDDATE = C.ENDDATE, 
	ISVERIFIED = 0
  FROM DBO.CONTRACTALERTS CA JOIN DBO.CONTRACTS C ON C.CONTRACTID = CA.CONTRACTID
  WHERE
		CA.ENDDATE <> C.ENDDATE
	AND C.ContractID = @Contractid;

IF @Thresholdtoexpirealters IS NOT NULL
	BEGIN
		INSERT INTO DBO.CONTRACTALERTS(
						 INSERTDATE, 
						 UPDATEDATE, 
						 CONTRACTID, 
						 ENDDATE)
		SELECT
			   C.INSERTDATE, 
			   NULL, 
			   C.CONTRACTID, 
			   C.ENDDATE
		  FROM DBO.CONTRACTS C
		  WHERE C.ContractID = @Contractid
			AND C.ISACTIVE = 1
			AND C.ISDELETED = 0
			AND C.ENDDATE <= DATEADD(DAY, @Thresholdtoexpirealters, @Currentdate)
			AND C.CONTRACTID NOT IN(
									SELECT
										   CA.CONTRACTID
									  FROM CONTRACTALERTS CA);
	END;
