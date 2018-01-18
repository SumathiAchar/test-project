CREATE VIEW [dbo].[vwHISTransactions]
	AS SELECT 
			ClaimEncounterID [ClaimID], 
			SSINumber, 
			TransactionType, 
			SUM(Amount) [Amount] 
		FROM [$(CONTRACTMANAGEMENT_QA)].dbo._HISTransactions 
		GROUP BY ClaimEncounterID,SSINumber,TransactionType
