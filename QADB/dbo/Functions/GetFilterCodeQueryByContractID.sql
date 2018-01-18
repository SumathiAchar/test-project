/****************************************************************************  
 *   Name         : GetFilterCodeQueryByContractID
 *   Author       : Sheshagiri  
 *   Date         : 02/03/2015  
 *   Alter Date   : 
 *   Module       : Background adjudication
 *   Description  : Builds filter code query based on ContractID

EXEC GetFilterCodeQueryByContractID 40502,3,11976,200,100
 *****************************************************************************/
CREATE FUNCTION dbo.GetFilterCodeQueryByContractID
(
	@ContractID BIGINT,
	@FacilityID INT,
	@PrimaryNodeID INT = 0,
	@BatchSizeForBackgroundAdjudication INT,
	@TotalClaimsPicked INT = 0,
	@IsReadjudicate BIT
)
RETURNS NVARCHAR(MAX)
AS

--FIXED-JS-FEB2015 Comment should be provided to logical Statements 
BEGIN
		DECLARE
		@IsProfessional BIT = 0,
				@IsInstitutionalClaim BIT = 0,
				@IsClaimStartDate BIT = 0,
				@ContractStartDate DATETIME = GETUTCDATE(),
				@ContractEndDate DATETIME = GETUTCDATE()

		SELECT
				@ContractID = ContractId, 
				@IsProfessional = isprofessionalclaim, 
				@IsInstitutionalClaim = IsInstitutionalClaim, 
				@IsClaimStartDate = isclaimstartdate, 
				@ContractStartDate = startdate, 
				@ContractEndDate = enddate
		FROM
				Contracts c where c.ContractID = @ContractID;
		------Build main query ------------------------
					DECLARE
					   @Criterias NVARCHAR(MAX) = '',
					   @Basequerystring NVARCHAR(MAX) = '',
					   @Querystring NVARCHAR(MAX) = '',
					   @PayerQuery NVARCHAR(MAX) = '',
					   @Mainquery NVARCHAR(MAX) = '';

				-- FIXED-JS-FEB2015 Create separate function for select each query				
				-- FIXED-JS-FEB2015 Conversion to BIGINT is not required. 
				--SSH When column datatype is of BIGINT and comparision is with numeric(INT value 10,20), BIGINT conversion can improve performance (Not sure if index on BIGINT column get utilized)

					-- Builds request Criterias string for a given ContractID. EX: '24|2|12463~2|3|350'
					SELECT @Criterias = dbo.GetRequestCriteriaByContractID(@ContractID)

					IF LEN(LTRIM(RTRIM(@Criterias))) = 0
						SET @Criterias = NULL;
					-- Builds sub query based on @Criterias and @FacilityID
					SET @Basequerystring = dbo.GetSubQueryForClaimFiltersForBackground(-1, NULL, NULL, @Criterias, @FacilityID, @ContractID, @IsReadjudicate, @PrimaryNodeID);

					-- Build contract creteria sub query which needs to be matched with claim data.
					-- Note background adjudication will match claimdata based on contract creteria that includes ClaimType, Contract Start/Thru date,payer and filter codes added using claim tool while building contract.
					SET @Querystring = @Querystring + ' CD.ClaimStatus <> ''unbilled''';
					-- Builds the Payer code
					SELECT @PayerQuery = [dbo].[GetPayerCodeQueryByContractID](@ContractID)
					SET @Querystring = @Querystring + ' AND ' + @PayerQuery + ' AND';

					-- Pick either StatementFrom or StatementThru based on flag
					IF @IsClaimStartDate = 1
						BEGIN
							SET @Querystring = @Querystring + '( CD.StatementFrom BETWEEN ''' + CONVERT(VARCHAR(12), @ContractStartDate, 101) + ''' AND ''' + CONVERT(VARCHAR(12), @ContractEndDate, 101) + ''' ';
						END;
					ELSE
						BEGIN
							SET @Querystring = @Querystring + '( CD.StatementThru BETWEEN ''' + CONVERT(VARCHAR(12), @ContractStartDate, 101) + ''' AND ''' + CONVERT(VARCHAR(12), @ContractEndDate, 101) + ''' ';
						END;
				
					IF @IsProfessional = 1
						BEGIN
							SET @Querystring = @Querystring + 'AND (CD.ClaimType= ''PHYS - Physician'' ';
						END;
					IF @IsInstitutionalClaim = 1
						BEGIN
							IF @IsProfessional = 1 -- When both InstitutionalClaim and Professional are set.
								BEGIN
									SET @Querystring = @Querystring + ' OR ';
								END;
							ELSE
								BEGIN
									SET @Querystring = @Querystring + ' AND (';
								END;
							SET @Querystring = @Querystring + 'CD.ClaimType=''HOSP - Hospital'' ';
						END;

					IF (@IsProfessional = 1 OR @IsInstitutionalClaim = 1)
						SET @Querystring = @Querystring + ' )';

					SET @Mainquery = @Basequerystring + ' AND ' + @Querystring;
					-- Do not pick claims with adjudicated, Unadjudicated, missing contract/servicetype and errored out claim status
					-- AVS.StatusID IN (1,2, 4, 5, 6, 7, 17,3,18) represents errored out claims
					--If Contract is changed, claims has to readjudicate,it need not to check ContractAdjudication table if the claim is adjudicated previously
					IF(@IsReadjudicate = 0)
					SET @Mainquery = @Mainquery + ' AND CD.ClaimId NOT IN (SELECT CA.ClaimId FROM dbo.ContractAdjudications CA  
													JOIN dbo.ContractHierarchy CH on CH.NodeID = CA.ModelID  
													LEFT JOIN dbo.Contracts C  ON C.ContractId=CA.ContractId AND ClaimServiceLineId IS NULL AND CA.ContractServiceTypeID IS NULL 
													JOIN dbo.[ref.AdjudicationOrVarianceStatuses] AS AVS ON AVS.StatusID = CA.ClaimStatus 
													WHERE AVS.StatusID IN (1,2, 4, 5, 6, 7, 17,3,18) 
													AND CA.ClaimServiceLineID IS NULL AND CA.ContractServiceTypeID IS NULL
													AND CA.ModelID = ' + CAST(@PrimaryNodeID AS VARCHAR(100))+' AND CH.FacilityId=' + CAST(@FacilityID AS VARCHAR(100)) + ')';
					SET @Mainquery = @Mainquery + ')';

					--- Replace first occurance of DISTINCT keyword with DISTINCT TOP batchsize generated through sub query function.
					SET @Mainquery = STUFF(@Mainquery, CHARINDEX('SELECT DISTINCT CD.Claimid', @Mainquery), LEN('SELECT DISTINCT CD.claimid'), 'SELECT DISTINCT TOP ' + CAST(@BatchSizeForBackgroundAdjudication - @TotalClaimsPicked AS VARCHAR(100)) + ' CD.Claimid');
					-- Do not pick (duplicate) claim which are already picked with different contract under model.
					SET @Mainquery = @Mainquery + ' AND NOT EXISTS (SELECT ClaimList.ClaimId FROM @tempclaims ClaimList WHERE ClaimList.ClaimId = CD.Claimid)';
					RETURN @Mainquery;
END;