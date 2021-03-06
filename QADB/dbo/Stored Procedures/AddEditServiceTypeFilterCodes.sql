CREATE PROCEDURE dbo.AddEditServiceTypeFilterCodes(
@Contractservicetypeid BIGINT, 
@Contractid BIGINT)
AS
BEGIN
DECLARE @CurrentDate DATETIME = GETUTCDATE();
	IF @Contractservicetypeid = 0
		BEGIN
			SET @Contractservicetypeid = NULL
		END;

	IF @Contractid = 0
		BEGIN
			SET @Contractid = NULL
		END;

	IF @Contractservicetypeid IS NOT NULL
   AND NOT EXISTS(SELECT
						 *
					FROM ServiceTypePaymentTypeFilterCodes
					WHERE @Contractservicetypeid IS NULL
					   OR ContractServiceTypeID = @Contractservicetypeid)
		BEGIN
			-- Insert service type codes.
			INSERT INTO ServiceTypePaymentTypeFilterCodes(
							 ContractServiceTypeID, 
							 InsertDate, 
							 ContractID, 
							 ServiceTypeCodes)
			VALUES(@Contractservicetypeid, 
				   @CurrentDate, 
				   @Contractid, 
				   dbo.GetServiceTypeCodesById(NULL, @Contractservicetypeid));
		END;
	ELSE
		BEGIN
			IF @Contractservicetypeid IS NOT NULL
				BEGIN
					-- Update Service Type Codes.
					UPDATE ServiceTypePaymentTypeFilterCodes
					SET
						ServiceTypeCodes = dbo.GetServiceTypeCodesById(NULL, @Contractservicetypeid), 
						UpdateDate = @CurrentDate
					  WHERE
							ContractServiceTypeId = @Contractservicetypeid;
				END;

		END;

	IF @Contractid IS NOT NULL
   AND NOT EXISTS(SELECT
						 *
					FROM ServiceTypePaymentTypeFilterCodes
					WHERE ContractID = @Contractid)
		BEGIN
			-- Insert Contract codes.
			INSERT INTO ServiceTypePaymentTypeFilterCodes(
							 ContractServiceTypeID, 
							 InsertDate, 
							 ContractID, 
							 ServiceTypeCodes)
			VALUES(NULL, 
				   @CurrentDate, 
				   @Contractid, 
				   dbo.GetServiceTypeCodesById(@Contractid, NULL));
		END;
	ELSE
		BEGIN
			IF @Contractid IS NOT NULL
				BEGIN
					-- Update Contract Codes.
					UPDATE ServiceTypePaymentTypeFilterCodes
					SET
						ServiceTypeCodes = dbo.GetServiceTypeCodesById(@Contractid, NULL), 
						UpdateDate = @CurrentDate
					  WHERE
							ContractServiceTypeId IS NULL
						AND ContractID = @Contractid;
				END;
		END;
END;	