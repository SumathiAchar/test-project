/****************************************************************************
 *   Name         : ExportAdjudicatedClaims
 *   Module       : Export total adjudicated value of claims
 *   Parameters	  : @SSINumber - A varchar field accepts comma separated SSI numbers. 
					@IsExported - A flag when set to 1, it marks those records as exported and be ignored during next exports. 	
 *   Description  : Exports adjudicated value of all claim.
					Exported claims will be logged to ExportedAdjudicatedData table.
					Claims belongs to the passed SSI numbers will only be considered.
					Only claims adjudicated to primary model of passed SSI numbers will be picked for export
					If @IsExported is passed as 1, it will update IsExported column in ContractAdjudications table. 
					Once IsExported column is updated with value 1, these claims will be ignored during next export unless these claims are manually readjudicated
 *   Literals     : ClaimStatus = 3 -> Adjudicated claim status 
					IsExported = 0 -> Claim which has a 0 for IsExported field defines that the claim is not exported. This is a column in contractadjudications table
					IsPrimaryNode = 1 -> Primary node of a facility 
 *	 Execute	  : EXEC ExportAdjudicatedClaims NULL, 0
*****************************************************************************/

CREATE PROCEDURE [dbo].[ExportAdjudicatedClaims]
(
	@SSINumber NVARCHAR(100),
	@IsExported INT
)
AS
BEGIN
	DECLARE @ModelIds TABLE (NodeId BIGINT)
	DECLARE @AdjudicationIds TABLE (AdjudicationId BIGINT)

	BEGIN TRANSACTION;
	BEGIN TRY
		IF(ISNULL(@SSINumber, '') = '')
		BEGIN
			INSERT INTO @ModelIds
			SELECT NodeId FROM ContractHierarchy WITH (NOLOCK) WHERE IsPrimaryNode = 1
		END
		ELSE 
		BEGIN
			INSERT INTO @ModelIds
			SELECT NodeId FROM ContractHierarchy ch WITH (NOLOCK) 
			JOIN Facility_SSINumber fs WITH (NOLOCK) ON ch.FacilityId = fs.FacilityId
			WHERE fs.SSINumber IN (SELECT ITEMS FROM dbo.Split(@SSINumber, ',')) 
			AND ch.IsPrimaryNode = 1			
		END

		INSERT INTO @AdjudicationIds
		SELECT ContractAdjudicationId FROM ContractAdjudications WITH (NOLOCK)
		WHERE ModelId IN (SELECT NodeId FROM @ModelIds)
		AND IsExported = 0
		AND ContractServiceTypeID IS NULL 
		AND ClaimServiceLineID IS NULL
		AND ClaimStatus = 3

		IF(@IsExported = 1)
		BEGIN
			UPDATE ContractAdjudications 
			SET IsExported = @IsExported
			WHERE IsExported = 0 AND ContractAdjudicationId IN (SELECT AdjudicationId FROM @AdjudicationIds)

			INSERT INTO ExportedAdjudicatedData
			SELECT ClaimId, InsertDate, GETDATE() FROM ContractAdjudications WITH (NOLOCK) 
			WHERE ContractAdjudicationId IN (SELECT AdjudicationId FROM @AdjudicationIds)
		END
		
		SELECT  CP.ClaimId,
				CP.SSINumber,
				CP.PatAcctNum AS PatientAccountNumber,
				CP.NPI,
				CP.ClaimType,
				CP.ClaimState,
				CP.PayerSequence,
				CP.BillType,
				CP.DRG,
				CP.PriICDDcode,
				CP.PriICDPcode,
				CP.PriPayerName,
				CP.SecPayerName,
				CP.TerPayerName,
				CP.IsRemitLinked,
				CP.ClaimStat,
				CP.ClaimLink,
				CP.LinkedRemitID,
				CP.DischargeStatus,
				CP.CustomField1,
				CP.CustomField2,
				CP.CustomField3,
				CP.CustomField4,
				CP.CustomField5,
				CP.CustomField6,
				CP.Claimtotal,
				CP.ExpectedPayment AS AdjudicatedValue,
				CP.ActualPayment,
				CP.PatientResponsibility,
				CP.CalAllow AS RemitAllowedAmt,
				CP.CalNonCov AS RemitNonCovered,
				CP.ExpectedContractualAdjustment AS CalculatedAdj,
				CP.ActualContractualAdjustment AS ActualAdj,
				CP.ContractualVariance,
				CP.PaymentVariance,
				CP.AdjudicatedDate,
				CP.StatementFrom,
				CP.StatementThru,
				CP.BillDate,
				CP.ClaimDate,
				CP.LastFiledDate
		FROM dbo.ClaimPayVarData CP WITH (NOLOCK) 
		WHERE CP.ContractAdjudicationId IN (SELECT AdjudicationId FROM @AdjudicationIds)
	
		COMMIT TRANSACTION;
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION;
		EXEC RaiseErrorInformation
	END CATCH;
END