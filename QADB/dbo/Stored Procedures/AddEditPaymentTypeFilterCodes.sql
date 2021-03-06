CREATE PROCEDURE dbo.AddEditPaymentTypeFilterCodes(
@Contractservicetypeid BIGINT, 
@Contractid BIGINT)
AS
BEGIN
DECLARE
	   @Currentdate DATETIME = GETUTCDATE();
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
							 PaymentTypeCodes)
			VALUES(@Contractservicetypeid, 
				   @Currentdate, 
				   @Contractid, 
				   dbo.GetPaymentTypeCodesById(NULL, @Contractservicetypeid));
		END;
	ELSE
		BEGIN
			IF @Contractservicetypeid IS NOT NULL
				BEGIN
					-- Update Service Type Codes.
					UPDATE ServiceTypePaymentTypeFilterCodes
					SET
						PaymentTypeCodes = dbo.GetPaymentTypeCodesById(NULL, @Contractservicetypeid), 
						UpdateDate = @Currentdate
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
							 PaymentTypeCodes)
			VALUES(NULL, 
				   @Currentdate, 
				   @Contractid, 
				   dbo.GetPaymentTypeCodesById(@Contractid, NULL));
		END;
	ELSE
		BEGIN
			IF @Contractid IS NOT NULL
				BEGIN
					-- Update Contract Codes.
					UPDATE ServiceTypePaymentTypeFilterCodes
					SET
						PaymentTypeCodes = dbo.GetPaymentTypeCodesById(@Contractid, NULL), 
						UpdateDate = @Currentdate
					  WHERE
							ContractServiceTypeId IS NULL
						AND ContractID = @Contractid;
				END;
		END;
END;	