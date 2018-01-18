/****************************************************************************  
 *   Name         : GetRequestCriteriaByContractID
 *   Author       : Sheshagiri  
 *   Date         : 02/17/2015  
 *   Alter Date   : 
 *   Module       : Background adjudication
 *   Description  : Builds request Criterias for a given ContractID. EX: '24|2|12463~2|3|350'

EXEC GetRequestCriteriaByContractID 40502
 *****************************************************************************/
CREATE FUNCTION dbo.GetRequestCriteriaByContractID
(
	@ContractID BIGINT
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
		DECLARE @Criterias NVARCHAR(MAX) = CAST('' AS NVARCHAR(MAX));
			
		-- FIXED-JS-FEB2015 Conversion to BIGINT is not required
		-- operator 2:greater than, 3:equal to, 4:less than
		-- 7 represents "Claim Fields", 8 represents "Table selections" and less than 7 are other claim selection items from claim tool
		-- Below query fetches the contract/service type criteria and put all of those in a temporary table
		--include code of all claimselection except claimfield and tableselection union
		--exclude code of all claimselection except claimfield and tableselection union 
		--union claim tool selection criteria union
		--table selection criteria
					WITH Temp
						AS (SELECT
								   servicelinemaster.ClaimFieldId, 
								   '3' AS OperatorType, 
								   servicelines.IncludedCode IdentifierValues
							  FROM
								   dbo.ContractServiceLinePaymentTypes paymenttypes  JOIN dbo.ContractServiceLines servicelines  ON paymenttypes.contractservicelineid = servicelines.contractservicelineid
																				JOIN dbo.[ref.ServiceLineTypes] servicelinemaster  ON paymenttypes.servicelinetypeid = servicelinemaster.servicelinetypeid
							  WHERE paymenttypes.ContractID = @ContractID
								AND paymenttypes.ServiceLineTypeID < 7 
								AND servicelines.IncludedCode IS NOT NULL
							UNION ALL
							SELECT
								   paymenttypes.ServiceLineTypeID, 
								   '2' AS OperatorType, 
								   servicelines.excludedcode IdentifierValues
							  FROM
								   dbo.ContractServiceLinePaymentTypes paymenttypes  JOIN dbo.ContractServiceLines servicelines  ON paymenttypes.ContractServiceLineID = servicelines.ContractServiceLineID
							  WHERE paymenttypes.ContractID = @ContractID
								AND paymenttypes.ServiceLineTypeID < 7 
								AND servicelines.ExcludedCode IS NOT NULL
							UNION ALL
							SELECT
								   claimfield.ClaimFieldID, 
								   operators.OperatorID, 
								   claimfield.[values] IdentifierValue
							  FROM
								   dbo.ContractServiceLinePaymentTypes paymenttypes  JOIN dbo.ContractServiceLineClaimFieldSelection claimfield  ON paymenttypes.ContractServiceLineID = claimfield.ContractServiceLineID
																				JOIN dbo.[ref.ClaimFieldOperators] operators  ON claimfield.OperatorID = operators.OperatorID
							  WHERE paymenttypes.ContractID = @ContractID
								AND paymenttypes.ServiceLineTypeID = 7
							UNION ALL
							SELECT
								   tablesselection.ClaimFieldId, 
								   operators.OperatorID AS OperatorType, 
								   IdentifierValue = STUFF((
															SELECT
																   ',' + claimvalues.Identifier
															  FROM dbo.ClaimFieldValues claimvalues 
															  WHERE tablesselection.ClaimFieldDocId = claimvalues.ClaimFieldDocID
															  FOR
															  XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')
							  FROM
								   dbo.ContractServiceLinePaymentTypes paymenttypes  JOIN dbo.ContractServiceLineTableSelection tablesselection  ON paymenttypes.ContractServiceLineID = tablesselection.ContractServiceLineID	
								   JOIN dbo.[ref.ClaimFieldOperators] operators  ON tablesselection.Operator  = operators.OperatorID
							  WHERE paymenttypes.ContractID = @ContractID
								AND paymenttypes.ServiceLineTypeID = 8 
							  GROUP BY
									   tablesselection.ClaimFieldId, 
									   operators.OperatorID,
									   tablesselection.ClaimFieldDocId)

				--Below statement prepares criteria as it is in the tracktasks table. example: 24|3|139013278~24|2|139013279
						SELECT
							   @Criterias = 
							   COALESCE(@Criterias + '~', '') + CAST(t.ClaimFieldId AS VARCHAR(10)) + '|' + CAST(t.OperatorType AS VARCHAR(100)) + '|' + STUFF((
																																							SELECT
																																								   ',' + t2.IdentifierValues
																																							  FROM Temp t2
																																							  WHERE t.ClaimFieldId = t2.ClaimFieldId
																																								AND t.OperatorType = t2.OperatorType
																																							  FOR
																																							  XML PATH('')), 1, 1, '')  --AS pid
						  FROM Temp t
						  GROUP BY
								   ClaimFieldId, 
								   OperatorType;
					
					IF(@Criterias like '~%')
					BEGIN
						SET @Criterias = RIGHT(@Criterias, LEN(@Criterias) - 1)
					END

					RETURN @Criterias

END