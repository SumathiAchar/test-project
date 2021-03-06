CREATE FUNCTION [dbo].[ValidateFiltersForContractOrServiceType]
(
@BillTypeCodes VARCHAR(8000), 
@RevenueCodes VARCHAR(8000), 
@CPTOrHCPCSCodes VARCHAR(8000), 
@DRGCodes VARCHAR(8000), 
@DiagnosisCodes VARCHAR(8000), 
@ProcedureCodes VARCHAR(8000), 
@ContractID BIGINT, 
@ContractServiceTypeID BIGINT
)
RETURNS BIT
AS
BEGIN

	DECLARE
	   @IsRowValidForAll BIT = 1;
	DECLARE
	   @IsValidCode BIT = 1;
	DECLARE
	   @IsContractServiceTypeIDFound BIT;
	DECLARE
	   @IsClaimSelectionIDFound BIT;
	DECLARE
	   @IsClaimTableIDFound BIT;

	-------------------------------------1	Bill Type-------------------------------------------------------------------------
	SELECT
		   @IsValidCode = IsValidCode, 
		   @IsContractServiceTypeIDFound = IsIDFound
	  FROM dbo.CheckCodesInContractServiceTypes(1, @ContractID, @BillTypeCodes, @ContractServiceTypeID);

	IF @IsValidCode <> 1
		BEGIN
			SELECT
				   @IsValidCode = IsValidCode, 
				   @IsClaimSelectionIDFound = IsIDFound
			  FROM dbo.CheckCodesClaimFieldSelections(@ContractID, @ContractServiceTypeID, 2, @BillTypeCodes);

			IF @IsValidCode <> 1
				BEGIN
					IF @IsContractServiceTypeIDFound = 1
					OR @IsClaimSelectionIDFound = 1
						BEGIN
							RETURN 0
						END;
				END;
		END;

	SET @IsContractServiceTypeIDFound = NULL;
	SET @IsClaimSelectionIDFound = NULL;
	SET @IsClaimTableIDFound = NULL;

	-------------------------------------2	Revenue Code-------------------------------------------------------------------------
	SELECT
		   @IsValidCode = IsValidCode, 
		   @IsContractServiceTypeIDFound = IsIDFound
	  FROM dbo.CheckCodesInContractServiceTypes(2, @ContractID, @RevenueCodes, @ContractServiceTypeID);

	IF @IsValidCode <> 1
		BEGIN
			SELECT
				   @IsValidCode = IsValidCode, 
				   @IsClaimSelectionIDFound = IsIDFound
			  FROM dbo.CheckCodesClaimFieldSelections(@ContractID, @ContractServiceTypeID, 3, @RevenueCodes);

			IF @IsValidCode <> 1
				BEGIN
					IF @IsContractServiceTypeIDFound = 1
					OR @IsClaimSelectionIDFound = 1
						BEGIN
							RETURN 0
						END;
				END;
		END;

	SET @IsContractServiceTypeIDFound = NULL;
	SET @IsClaimSelectionIDFound = NULL;
	SET @IsClaimTableIDFound = NULL;

	-------------------------------------3	CPT-------------------------------------------------------------------------
	SELECT
		   @IsValidCode = IsValidCode, 
		   @IsContractServiceTypeIDFound = IsIDFound
	  FROM dbo.CheckCodesInContractServiceTypes(3, @ContractID, @CPTOrHCPCSCodes, @ContractServiceTypeID);

	IF @IsValidCode <> 1
		BEGIN
			SELECT
				   @IsValidCode = IsValidCode, 
				   @IsClaimSelectionIDFound = IsIDFound
			  FROM dbo.CheckCodesClaimFieldSelections(@ContractID, @ContractServiceTypeID, 4, @CPTOrHCPCSCodes);

			IF @IsValidCode <> 1
				BEGIN
					IF @IsContractServiceTypeIDFound = 1
					OR @IsClaimSelectionIDFound = 1
						BEGIN
							RETURN 0
						END;
				END;
		END;

	SET @IsContractServiceTypeIDFound = NULL;
	SET @IsClaimSelectionIDFound = NULL;
	SET @IsClaimTableIDFound = NULL;

	---------------------------------------4	DRG-------------------------------------------------------------------------
	SELECT
		   @IsValidCode = IsValidCode, 
		   @IsContractServiceTypeIDFound = IsIDFound
	  FROM dbo.CheckCodesInContractServiceTypes(4, @ContractID, @DRGCodes, @ContractServiceTypeID);

	IF @IsValidCode <> 1
		BEGIN
			SELECT
				   @IsValidCode = IsValidCode, 
				   @IsClaimSelectionIDFound = IsIDFound
			  FROM dbo.CheckCodesClaimFieldSelections(@ContractID, @ContractServiceTypeID, 8, @DRGCodes);

			IF @IsValidCode <> 1
				BEGIN
					IF @IsContractServiceTypeIDFound = 1
					OR @IsClaimSelectionIDFound = 1
						BEGIN
							RETURN 0
						END;
				END;
		END;

	SET @IsContractServiceTypeIDFound = NULL;
	SET @IsClaimSelectionIDFound = NULL;
	SET @IsClaimTableIDFound = NULL;

	---------------------------------------5	Diagnosis Code-------------------------------------------------------------------------
	SELECT
		   @IsValidCode = IsValidCode, 
		   @IsContractServiceTypeIDFound = IsIDFound
	  FROM dbo.CheckCodesInContractServiceTypes(5, @ContractID, @DiagnosisCodes, @ContractServiceTypeID);

	IF @IsValidCode <> 1
		BEGIN
			SELECT
				   @IsValidCode = IsValidCode, 
				   @IsClaimSelectionIDFound = IsIDFound
			  FROM dbo.CheckCodesClaimFieldSelections(@ContractID, @ContractServiceTypeID, 12, @DiagnosisCodes);

			IF @IsValidCode <> 1
				BEGIN
					IF @IsContractServiceTypeIDFound = 1
					OR @IsClaimSelectionIDFound = 1
						BEGIN
							RETURN 0
						END;
				END;
		END;

	SET @IsContractServiceTypeIDFound = NULL;
	SET @IsClaimSelectionIDFound = NULL;
	SET @IsClaimTableIDFound = NULL;
	---------------------------------------6	Procedure Code-------------------------------------------------------------------------
	SELECT
		   @IsValidCode = IsValidCode, 
		   @IsContractServiceTypeIDFound = IsIDFound
	  FROM CheckCodesInContractServiceTypes(6, @ContractID, @ProcedureCodes, @ContractServiceTypeID);

	IF @IsValidCode <> 1
		BEGIN
			SELECT
				   @IsValidCode = IsValidCode, 
				   @IsClaimSelectionIDFound = IsIDFound
			  FROM dbo.CheckCodesClaimFieldSelections(@ContractID, @ContractServiceTypeID, 13, @ProcedureCodes);

			IF @IsValidCode <> 1
				BEGIN
					IF @IsContractServiceTypeIDFound = 1
					OR @IsClaimSelectionIDFound = 1
						BEGIN
							RETURN 0
						END;
				END;
		END;

	SET @IsContractServiceTypeIDFound = NULL;
	SET @IsClaimSelectionIDFound = NULL;
	SET @IsClaimTableIDFound = NULL;

	RETURN @IsRowValidForAll;
END;


--DECLARE	@BillTypeCodes  VARCHAR(8000)
--DECLARE	@RevenueCodes VARCHAR(8000)
--DECLARE	@CPTOrHCPCSCodes VARCHAR(8000)
--DECLARE	@DRGCodes VARCHAR(8000)
--DECLARE	@DiagnosisCodes VARCHAR(8000)
--DECLARE	@ProcedureCodes VARCHAR(8000)
--DECLARE	@ContractID  BIGINT
--DECLARE	@ContractServiceTypeID BIGINT

--DECLARE @IsValidCode BIT
--SET @ContractID = 545
--SET @IsValidCode = dbo.ValidateFiltersForContractOrServiceType(@BillTypeCodes, '299'
--	,@CPTOrHCPCSCodes,@DRGCodes,@DiagnosisCodes,@ProcedureCodes,@ContractID,0);

--	SELECT @IsValidCode;
GO
