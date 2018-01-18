CREATE VIEW dbo.ClaimPayVarData
AS SELECT
		  C.ClaimID ClaimID, 
		  CH.NodeText Model, 
		  CA.ModelID, 
		  CA.ContractID,
		  C.PatAcctNum, 
		  CA.InsertDate AdjudicatedDate, 
		  CA.ClaimTotalCharges, 
		  C.SSInumber SSInumber, 
		  C.ClaimType ClaimType, 
		  C.ClaimState ClaimState, 
		  C.PayerSequence PayerSequence, 
		  C.StatementFrom StatementFrom, 
		  C.StatementThru StatementThru, 
		  C.ClaimDate ClaimDate, 
		  C.BillDate BillDate, 
		  C.LastFiledDate LastFiledDate, 
		  C.BillType BillType, 
		  C.DRG DRG, 
		  C.PriICDDCode PriICDDcode, 
		  C.PriICDPcode PriICDPcode, 
		  C.PriPayerName PriPayerName, 
		  C.SecPayerName SecPayerName, 
		  C.TerPayerName TerPayerName, 
		  CASE
			WHEN LEN(C.LastRemitID) > 0 THEN 'YES'
		  ELSE 'NO'
		  END IsRemitLinked, 
		  R.ClaimStat, 
		  R.CalAllow, 
		  R.CalNonCov,
		  R.ICN, 
		  C.Claimtotal Claimtotal, 
		  CA.AdjudicatedValue ExpectedPayment, 
		  CASE
			WHEN C.LastremitID IS NOT NULL THEN R.ProvPay
		  ELSE Hpmt.Amount
		  END ActualPayment,
		  CASE
			WHEN C.LastremitID IS NOT NULL THEN R.Calcont
		  ELSE Hadj.Amount
		  END ActualContractualAdjustment, 
		  CASE
			WHEN CA.AdjudicatedValue IS NOT NULL THEN C.ClaimTotal - CA.AdjudicatedValue
		  ELSE NULL
		  END ExpectedContractualAdjustment, 
		  CASE
			WHEN C.LastremitID IS NOT NULL THEN R.PatientResponsibility
		  ELSE NULL
		  END PatientResponsibility, 
		  CASE
			WHEN C.LastremitID IS NOT NULL THEN R.Calcont
		  ELSE Hadj.Amount
		  END - (CASE
					WHEN CA.AdjudicatedValue IS NOT NULL THEN(C.ClaimTotal - CA.AdjudicatedValue)
				 ELSE NULL
				 END)ContractualVariance, 
		  CA.AdjudicatedValue - (CASE
									WHEN C.LastremitID IS NOT NULL THEN R.ProvPay
								 ELSE Hpmt.Amount
								 END + (CASE
											WHEN C.LastremitID IS NOT NULL THEN R.PatientResponsibility
										ELSE 0
										END))PaymentVariance, 
		  CASE
			WHEN C.LastremitID IS NOT NULL THEN R.ProvPay
			ELSE Hpmt.Amount 
			END AS Remit,
		  C.ClaimLink ClaimLink, 
		  C.LastremitID linkedRemitID,
   C.CustomField1,
   C.CustomField2,
   C.CustomField3,
   C.CustomField4,
   C.CustomField5,
   C.CustomField6,
   C.NPI,
   C.DischargeStatus,
   CA.ContractAdjudicationID
	  FROM
		  ClaimData C WITH (NOLOCK) LEFT JOIN dbo.contractadjudications CA WITH (NOLOCK) ON C.ClaimID = CA.ClaimID
																				   AND CA.ContractServiceTypeID IS NULL
																				   AND CA.ClaimServiceLineID IS NULL
												 LEFT JOIN RemitData R WITH (NOLOCK) ON C.lastRemitID = R.RemitID
												 LEFT JOIN vwHIStransactions Hadj ON Hadj.ClaimID = C.ClaimID
																				 AND Hadj.TransactionType = 'A'
												 LEFT JOIN vwHIStransactions Hpmt ON Hpmt.ClaimID = C.ClaimID
																				 AND hpmt.TransactionType = 'P'
												 LEFT JOIN ContractHierarchy CH WITH (NOLOCK) ON CH.NodeID = CA.ModelID;