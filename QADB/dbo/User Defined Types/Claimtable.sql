
/****** Object:  UserDefinedTableType [dbo].[Claimtable]    Script Date: 1/29/2014 7:34:45 PM ******/

CREATE TYPE dbo.Claimtable AS TABLE(
									Claimid BIGINT NULL, 
									StatementFrom DATETIME NULL, 
									StatementThru DATETIME NULL, 
									Payer VARCHAR(100)NULL, 
									ClaimType VARCHAR(100)NULL);