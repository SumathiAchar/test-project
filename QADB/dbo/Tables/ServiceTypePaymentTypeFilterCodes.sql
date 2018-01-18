CREATE TABLE dbo.ServiceTypePaymentTypeFilterCodes(
			 FilterCodeID BIGINT IDENTITY(1, 1)
								 NOT NULL, 
			 ContractServiceTypeID BIGINT NULL, 
			 InsertDate DATETIME NOT NULL, 
			 UpdateDate DATETIME NULL, 
			 ContractID BIGINT NULL, 
			 ServiceTypeCodes VARCHAR(MAX)NULL, 
			 PaymentTypeCodes VARCHAR(MAX)NULL, 
			 PRIMARY KEY CLUSTERED(FilterCodeID ASC)
				 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)ON [PRIMARY])
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

GO

ALTER TABLE dbo.ServiceTypePaymentTypeFilterCodes
		WITH CHECK
ADD
			 FOREIGN KEY(ContractID)REFERENCES dbo.Contracts(
											   ContractID);
GO

ALTER TABLE dbo.ServiceTypePaymentTypeFilterCodes
		WITH CHECK
ADD
			 FOREIGN KEY(ContractServiceTypeID)REFERENCES dbo.ContractServiceTypes(
														  ContractServiceTypeID);
GO

CREATE NONCLUSTERED INDEX NCI_ServiceTypePaymentTypeFilterCodes_ContractID ON dbo.ServiceTypePaymentTypeFilterCodes(ContractID)INCLUDE(ContractServiceTypeID, ServiceTypeCodes, PaymentTypeCodes);
GO

CREATE NONCLUSTERED INDEX NCI_ServiceTypePaymentTypeFilterCodes_ContractServiceTypeID ON dbo.ServiceTypePaymentTypeFilterCodes(ContractServiceTypeID)INCLUDE(ContractID, ServiceTypeCodes, PaymentTypeCodes);
GO