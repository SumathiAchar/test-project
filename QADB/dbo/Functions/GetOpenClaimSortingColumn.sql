-- ===================================================================
-- Author		 : Janakiraman
-- Created Date : 16/06/2016
-- Description	: This return sort query based on given sortfield
-- ===================================================================
CREATE FUNCTION [dbo].[GetOpenClaimSortingColumn](@SortField     VARCHAR(100),
                                                 @IsAllReviewed BIT)
RETURNS NVARCHAR(1000)
AS
     BEGIN
         DECLARE @SortQuery NVARCHAR(MAX);
         IF(@SortField IS NOT NULL)
             BEGIN
                 IF(@SortField = 'PatientAccountNumber')
                     BEGIN
                         SET @SortQuery = 'CD.PatAcctNum AS PatientAccountNumber';
                     END;
                 ELSE
                 IF(@SortField = 'AdjudicatedDate')
                     BEGIN
                         SET @SortQuery = 'CA.InsertDate AS AdjudicatedDate';
                     END;
                 ELSE
                 IF(@SortField = 'SSInumber')
                     BEGIN
                         SET @SortQuery = 'CD.ssinumber AS SSInumber';
                     END;
                 ELSE
                 IF(@SortField = 'ClaimType')
                     BEGIN
                         SET @SortQuery = 'CD.ClaimType AS ClaimType';
                     END;
                 ELSE
                 IF(@SortField = 'ClaimState')
                     BEGIN
                         SET @SortQuery = 'CD.ClaimState AS ClaimState';
                     END;
                 ELSE
                 IF(@SortField = 'PayerSequence')
                     BEGIN
                         SET @SortQuery = 'CD.PayerSequence AS PayerSequence';
                     END;
                 ELSE
                 IF(@SortField = 'ClaimTotal')
                     BEGIN
                         SET @SortQuery = 'CD.ClaimTotal AS ClaimTotal';
                     END;
                 ELSE
                 IF(@SortField = 'StatementFrom')
                     BEGIN
                         SET @SortQuery = 'CD.StatementFrom AS StatementFrom';
                     END;
                 ELSE
                 IF(@SortField = 'StatementThru')
                     BEGIN
                         SET @SortQuery = 'CD.StatementThru AS StatementThru';
                     END;
                 ELSE
                 IF(@SortField = 'Los')
                     BEGIN
                         SET @SortQuery = 'CD.Los AS Los';
                     END;
                 ELSE
                 IF(@SortField = 'ClaimDate')
                     BEGIN
                         SET @SortQuery = 'CD.ClaimDate AS ClaimDate';
                     END;
                 ELSE
                 IF(@SortField = 'BillDate')
                     BEGIN
                         SET @SortQuery = 'CD.BillDate AS BillDate';
                     END;
                 ELSE
                 IF(@SortField = 'LastFiledDate')
                     BEGIN
                         SET @SortQuery = 'CD.LastFiledDate AS LastFiledDate';
                     END;
                 ELSE
                 IF(@SortField = 'BillType')
                     BEGIN
                         SET @SortQuery = 'CD.BillType AS BillType';
                     END;
                 ELSE
                 IF(@SortField = 'DRG')
                     BEGIN
                         SET @SortQuery = 'CD.DRG AS DRG';
                     END;
                 ELSE
                 IF(@SortField = 'PriICDDcode')
                     BEGIN
                         SET @SortQuery = 'D.ICDDCode AS PriICDDcode';
                     END;
                 ELSE
                 IF(@SortField = 'PriICDPCode')
                     BEGIN
                         SET @SortQuery = 'P.ICDPCode AS PriICDPCode';
                     END;
                 ELSE
                 IF(@SortField = 'priPayerName')
                     BEGIN
                         SET @SortQuery = 'CD.PriPayerName AS priPayerName';
                     END;
                 ELSE
                 IF(@SortField = 'secPayerName')
                     BEGIN
                         SET @SortQuery = 'CD.SecPayerName AS secPayerName';
                     END;
                 ELSE
                 IF(@SortField = 'TerPayerName')
                     BEGIN
                         SET @SortQuery = 'CD.TerPayerName AS TerPayerName';
                     END;
                 ELSE
                 IF(@SortField = 'isRemitLinked')
                     BEGIN
                         SET @SortQuery = 'CASE
								    WHEN LEN(CD.lastRemitID) > 0
								    THEN ''YES''
								    ELSE ''NO''
								END isRemitLinked';
                     END;
                 ELSE
                 IF(@SortField = 'ClaimStatus')
                     BEGIN
                         SET @SortQuery = 'R.ClaimStat AS ClaimStatus';
                     END;
                 ELSE
                 IF(@SortField = 'AdjudicatedValue')
                     BEGIN
                         SET @SortQuery = 'CA.AdjudicatedValue AS AdjudicatedValue';
                     END;
                 ELSE
                 IF(@SortField = 'ActualPayment')
                     BEGIN
                         SET @SortQuery = 'CASE
								    WHEN CD.LastremitID IS NOT NULL
								    THEN R.ProvPay
								    ELSE Hpmt.Amount
								END ActualPayment';
                     END;
                 ELSE
                 IF(@SortField = 'ActualContractualAdjustment')
                     BEGIN
                         SET @SortQuery = 'CASE
								    WHEN CD.LastremitID IS NOT NULL
								    THEN R.Calcont
								    ELSE Hadj.Amount
								END ActualContractualAdjustment';
                     END;
                 ELSE
                 IF(@SortField = 'ExpectedContractualAdjustment')
                     BEGIN
                         SET @SortQuery = 'CASE
								    WHEN CA.AdjudicatedValue IS NOT NULL
								    THEN CD.ClaimTotal - CA.AdjudicatedValue
								    ELSE NULL
								END ExpectedContractualAdjustment';
                     END;
                 ELSE
                 IF(@SortField = 'PatientResponsibility')
                     BEGIN
                         SET @SortQuery = ' CASE
								    WHEN CD.LastremitID IS NOT NULL
								    THEN R.PatientResponsibility
								    ELSE NULL
								END PatientResponsibility';
                     END;
                 ELSE
                 IF(@SortField = 'ContractualVariance')
                     BEGIN
                         SET @SortQuery = 'CASE
								    WHEN CD.LastremitID IS NOT NULL
								    THEN R.Calcont
								    ELSE Hadj.Amount
								END - (CASE
										 WHEN CA.AdjudicatedValue IS NOT NULL
										 THEN(CD.ClaimTotal - CA.AdjudicatedValue)
										 ELSE NULL
									  END) AS ContractualVariance';
                     END;
                 ELSE
                 IF(@SortField = 'PaymentVariance')
                     BEGIN
                         SET @SortQuery = 'CA.AdjudicatedValue - (CASE
                                                   WHEN CD.LastremitID IS NOT NULL
                                                   THEN R.ProvPay
                                                   ELSE Hpmt.Amount
                                               END + (CASE
                                                          WHEN CD.LastremitID IS NOT NULL
                                                          THEN R.PatientResponsibility
                                                          ELSE 0
                                                      END)) PaymentVariance';
                     END;
                 ELSE
                 IF(@SortField = 'RemitAllowedAmt')
                     BEGIN
                         SET @SortQuery = 'R.CalAllow AS RemitAllowedAmt';
                     END;
                 ELSE
                 IF(@SortField = 'RemitNonCovered')
                     BEGIN
                         SET @SortQuery = 'R.CalNonCov AS RemitNonCovered';
                     END;
                 ELSE
                 IF(@SortField = 'ICN')
                     BEGIN
                         SET @SortQuery = 'R.ICN AS ICN';
                     END;
                 ELSE
                 IF(@SortField = 'claimLink')
                     BEGIN
                         SET @SortQuery = 'CD.ClaimLink AS claimLink';
                     END;
                 ELSE
                 IF(@SortField = 'LinkedRemitID')
                     BEGIN
                         SET @SortQuery = 'CD.lastRemitID AS LinkedRemitID';
                     END;
                 ELSE
                 IF(@SortField = 'ClaimID')
                     BEGIN
                         SET @SortQuery = 'CD.ClaimID AS ClaimID';
                     END;
                 ELSE
                 IF(@SortField = 'DischargeStatus')
                     BEGIN
                         SET @SortQuery = 'CD.DischargeStatus AS DischargeStatus';
                     END;
                 ELSE
                 IF(@SortField = 'CustomField1')
                     BEGIN
                         SET @SortQuery = 'CD.CustomField1';
                     END;
                 ELSE
                 IF(@SortField = 'CustomField2')
                     BEGIN
                         SET @SortQuery = 'CD.CustomField2';
                     END;
                 ELSE
                 IF(@SortField = 'CustomField3')
                     BEGIN
                         SET @SortQuery = 'CD.CustomField3';
                     END;
                 ELSE
                 IF(@SortField = 'CustomField4')
                     BEGIN
                         SET @SortQuery = 'CD.CustomField4';
                     END;
                 ELSE
                 IF(@SortField = 'CustomField5')
                     BEGIN
                         SET @SortQuery = 'CD.CustomField5';
                     END;
                 ELSE
                 IF(@SortField = 'CustomField6')
                     BEGIN
                         SET @SortQuery = 'CD.CustomField6';
                     END;
                 ELSE
                 IF(@SortField = 'NPI')
                     BEGIN
                         SET @SortQuery = 'CD.NPI';
                     END;
                 ELSE
                 IF(@SortField = 'MemberID')
                     BEGIN
                         SET @SortQuery = 'ID.CertificationNumber AS MemberID';
                     END;
                 ELSE
                 IF(@SortField = 'MRN')
                     BEGIN
                         SET @SortQuery = 'PD.MedRecNo AS MRN';
                     END;
                 ELSE
                 IF(@SortField = 'IsReviewed')
                     BEGIN
                         SET @SortQuery = 'CR.ClaimID AS IsReviewed';
                     END;
                 ELSE
                 IF(@SortField = 'CheckDate')
                     BEGIN
                         SET @SortQuery = 'R.Checkdate As CheckDate';
                     END;
                 ELSE
                 IF(@SortField = 'CheckNumber')
                     BEGIN
                         SET @SortQuery = 'R.CheckNumber As CheckNumber';
                     END;
                 ELSE
                 IF(@SortField = 'ContractName'
                    OR @SortField = 'AdjudicatedContractName')
                     BEGIN
                         SET @SortQuery = 'C.ContractName As ContractName';
                     END;
                 ELSE
                 IF(@SortField = 'IsAllReviewed')
                     BEGIN
                         SET @SortQuery = 'CASE
								    WHEN ISNULL('+CAST(ISNULL(@IsAllReviewed, 0) AS VARCHAR(2))+', 0) > 0
								    THEN 0
								    ELSE 1
								END AS IsAllReviewed';
                     END;
                 ELSE
                 IF(@SortField = 'Age')
                     BEGIN
                         SET @SortQuery = 'dbo.CalculateAge(CAST(PD.DOB AS DATE), CAST(CD.StatementThru AS DATE)) AS Age';
                     END;
			  ELSE IF(@SortField = 'InsuredsGroupNumber')
                     BEGIN
                         SET @SortQuery = 'ID.GroupNumber AS InsuredsGroupNumber';
                     END;
             END;
         ELSE
             BEGIN
                 SET @SortQuery = 'CD.PatAcctNum AS PatientAccountNumber';
             END;
         RETURN @SortQuery;
     END;