/****************************************************/

--Created By  : Dev  
--Date        : 1-Aug-2013  
--Description : View to retrieve Claim Data Details

/****************************************************/


CREATE VIEW ClaimData
AS
     SELECT [ClaimID],
            [ClaimLink],
            [ssinumber],
            [ClaimType],
            [ClaimState],
            [ClaimStatus],
            [PayerSequence],
            [PatAcctNum],
            [ClaimTotal],
            [EstAmtDue],
            [PatPaid],
            [PatAmtDue],
            [StatementFrom],
            [StatementThru],
            ISNULL(CONVERT( INT,
                            CASE
                                WHEN CD.BillType LIKE '11%'
                                THEN
                          (
                              SELECT FLOOR(SUM(VC.amt))
                              FROM _ValueCodeData VC
                              WHERE VC.ClaimID = CD.ClaimID
                                    AND VC.valuecode IN('80', '81')
                          )
                                ELSE DATEDIFF(DAY, [StatementFrom], [StatementThru])
                            END), DATEDIFF(DAY, [StatementFrom], [StatementThru])) AS LOS,
            [ClaimDate],
            [BillDate],
            [LastFiledDate],
            [BillType],
            [DRG],
            [PriICDDCode],
            [PriICDPCode],
            [PriPayerName],
            [SecPayerName],
            [TerPayerName],
            [FTN],
            [NPI],
            [RenderingPHY],
            [RefPHY],
            [AttendingPHY],
            [lastRemitID],
            [DischargeStatus],
            [ProviderZip],
            UserDefinedField1 AS CustomField1,
            UserDefinedField2 AS CustomField2,
            UserDefinedField3 AS CustomField3,
            UserDefinedField4 AS CustomField4,
            UserDefinedField5 AS CustomField5,
            UserDefinedField6 AS CustomField6
     FROM [$(CONTRACTMANAGEMENT_QA)].dbo._ClaimData CD;