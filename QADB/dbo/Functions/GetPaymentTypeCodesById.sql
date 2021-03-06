CREATE FUNCTION [dbo].[GetPaymentTypeCodesById](
                @ContractID            BIGINT,
                @ContractServiceTypeID BIGINT )
RETURNS VARCHAR(MAX)
AS
     BEGIN
     DECLARE @Temp_ContractFilterTable TABLE( RowNumber                INT IDENTITY(1, 1),
                                              ServiceLineTypeID        BIGINT,
                                              IncludedCodesValueString VARCHAR(MAX),
                                              PaymenttypeId            BIGINT );
     DECLARE @ValueStringData VARCHAR(MAX);
     DECLARE @Valueexcludedstringdata VARCHAR(MAX);
     DECLARE @Fullservicelinecodedata VARCHAR(MAX);
     DECLARE @Stlist VARCHAR(MAX);
  
	--REVIEW-2016-R3-S2 we can use IIF in-place of CASE for Modifier condition 
     --------------------- For ASC Fee Schedules Payment Type------------------------  
     SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') +' Percentage  = ' + ISNULL(CONVERT( VARCHAR(MAX), PTFS.[PRIMARY]), '') + '%, ' + ISNULL(CONVERT(VARCHAR(MAX), PTFS.SECONDARY), '') + '%, ' + ISNULL(CONVERT(VARCHAR(MAX), PTFS.TERTIARY), '') + '%, ' + ISNULL(CONVERT(VARCHAR(MAX), PTFS.QUATERNARY), '') + '%, ' + ISNULL(CONVERT(VARCHAR(MAX), PTFS.OTHERS), '') + '%, NFS= ' + ISNULL(CONVERT(VARCHAR(MAX), PTFS.NONFEESCHEDULE), '') + '%, ' + ' Table Name = ' + CFD.TABLENAME + ', ' + +ASCOP.ASCFeeScheduleOptionName
     FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
          JOIN dbo.PAYMENTTYPEASCFEESCHEDULES AS PTFS WITH ( NOLOCK ) ON PTFS.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
          JOIN CLAIMFIELDDOCS AS CFD WITH ( NOLOCK ) ON CFD.CLAIMFIELDDOCID = PTFS.CLAIMFIELDDOCID
          JOIN ASCFEESCHEDULEOPTIONS AS ASCOP ON PTFS.SelectedOption = ASCOP.ASCFeeScheduleOptionId
     WHERE( CSL.CONTRACTID = @ContractID
         OR @ContractServiceTypeID = ContractServiceTypeID
          )
      AND CSL.PaymentTypeID = 1;
     IF @Valuestringdata IS NOT NULL
         BEGIN
             INSERT INTO @Temp_ContractFilterTable
                    ( PaymentTypeID,
                      IncludedCodesValueString
                    )
             VALUES( 1, @Valuestringdata );
         END;
             SELECT @Valuestringdata = NULL;  
   
             --------------------- For DRG Payment Payment Type------------------------  
             SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') + 'Base Rate = $' + CONVERT( VARCHAR(MAX), REPLACE(RTRIM(REPLACE(REPLACE(RTRIM(LTRIM(STR(PTDRG.BASERATE, 15, 2))), ' ', '0'), '.', ' ')), ' ', '.')) + ', ' + 'Table Name = ' + CFD.TABLENAME + ''
             FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                  JOIN dbo.PAYMENTTYPEDRGPAYMENT AS PTDRG WITH ( NOLOCK ) ON PTDRG.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                  JOIN CLAIMFIELDDOCS AS CFD WITH ( NOLOCK ) ON CFD.CLAIMFIELDDOCID = PTDRG.CLAIMFIELDDOCID
             WHERE( CSL.CONTRACTID = @ContractID
                 OR @ContractServiceTypeID = ContractServiceTypeID
                  )
              AND CSL.PaymentTypeID = 2;
             IF @Valuestringdata IS NOT NULL
                 BEGIN
                     INSERT INTO @Temp_ContractFilterTable
                            ( PaymentTypeID,
                              IncludedCodesValueString
                            )
                     VALUES( 2, @Valuestringdata );
                 END;
                     SELECT @Valuestringdata = NULL;  
                     --------------------- For Fee Schedules Payment Type------------------------  
                     SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') +' FS = ' + ISNULL(CONVERT( VARCHAR(MAX), PTFS.FEESCHEDULE), '') + '%, ' + 'NFS= ' + ISNULL(CONVERT(VARCHAR(MAX), PTFS.NONFEESCHEDULE), '') + '%' + ', ' + ' Table Name = ' + CFD.TABLENAME ++ CASE PTFS.IsObserveUnits
                            WHEN 1
                            THEN ', Observe Service Units'
                            ELSE ''
                            END
                     FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                          JOIN dbo.PAYMENTTYPEFEESCHEDULES AS PTFS WITH ( NOLOCK ) ON PTFS.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                          JOIN CLAIMFIELDDOCS AS CFD WITH ( NOLOCK ) ON CFD.CLAIMFIELDDOCID = PTFS.CLAIMFIELDDOCID
                     WHERE( CSL.CONTRACTID = @ContractID
                         OR @ContractServiceTypeID = ContractServiceTypeID
                          )
                      AND CSL.PaymentTypeID = 3;
                     IF @Valuestringdata IS NOT NULL
                         BEGIN
                             INSERT INTO @Temp_ContractFilterTable
                                    ( PaymentTypeID,
                                      IncludedCodesValueString
                                    )
                             VALUES( 3, @Valuestringdata );
                         END;
                             SELECT @Valuestringdata = NULL;  
                             --------------------- For Medicare IP Payment Payment Type ------------------------  
                             SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') + 'Percentage =' + CONVERT( VARCHAR(MAX), PTMIPP.INPATIENT) + '%, Formula =' + PTMIPP.Formula
                             FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                                  JOIN dbo.PAYMENTTYPEMEDICAREIPPAYMENT AS PTMIPP WITH ( NOLOCK ) ON PTMIPP.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                             WHERE( CSL.CONTRACTID = @ContractID
                                 OR @ContractServiceTypeID = ContractServiceTypeID
                                  )
                              AND CSL.PaymentTypeID = 4;
                             IF @Valuestringdata IS NOT NULL
                                 BEGIN
                                     INSERT INTO @Temp_ContractFilterTable
                                            ( PaymentTypeID,
                                              IncludedCodesValueString
                                            )
                                     VALUES( 4, @Valuestringdata );
                                 END;
                                     SELECT @Valuestringdata = NULL;  
                                     --------------------- For Medicare OP Payment Payment Type  ------------------------  
                                     SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') + 'Percentage =' + CONVERT( VARCHAR(MAX), PTMOPP.OUTPATIENT) + '%'
                                     FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                                          JOIN dbo.PAYMENTTYPEMEDICAREOPPAYMENT AS PTMOPP WITH ( NOLOCK ) ON PTMOPP.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                                     WHERE( CSL.CONTRACTID = @ContractID
                                         OR @ContractServiceTypeID = ContractServiceTypeID
                                          )
                                      AND CSL.PaymentTypeID = 5;
                                     IF @Valuestringdata IS NOT NULL
                                         BEGIN
                                             INSERT INTO @Temp_ContractFilterTable
                                                    ( PaymentTypeID,
                                                      IncludedCodesValueString
                                                    )
                                             VALUES( 5, @Valuestringdata );
                                         END;
                                             SELECT @Valuestringdata = NULL;  
 
                                             --------------------- For Medicare Lab Fee Schedule Payment Type ------------------------  
                                             SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '')+ ' Percentage =' + CONVERT( VARCHAR(MAX), PTMLFSP.PERCENTAGE) + '%'
                                             FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                                                  JOIN dbo.PAYMENTTYPEMEDICARELABFEESCHEDULE AS PTMLFSP WITH ( NOLOCK ) ON PTMLFSP.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                                             WHERE( CSL.CONTRACTID = @ContractID
                                                 OR @ContractServiceTypeID = ContractServiceTypeID
                                                  )
                                              AND CSL.PaymentTypeID = 13;
                                             IF @Valuestringdata IS NOT NULL
                                                 BEGIN
                                                     INSERT INTO @Temp_ContractFilterTable
                                                            ( PaymentTypeID,
                                                              IncludedCodesValueString
                                                            )
                                                     VALUES( 13, @Valuestringdata );
                                                 END;
                                                     SELECT @Valuestringdata = NULL;  

                                                     --------------------- For Per Case Payment Type ----------------------------  
                                                     SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') + 'Rate = $' + CONVERT( VARCHAR(MAX), REPLACE(RTRIM(REPLACE(REPLACE(RTRIM(LTRIM(STR(PTPC.RATE, 15, 2))), ' ', '0'), '.', ' ')), ' ', '.')) + CASE
                                                                                                                                                                                                                                                                      WHEN PTPC.MAXCASESPERDAY IS NOT NULL
                                                                                                                                                                                                                                                                      THEN ', Max Cases Per Day = ' + CONVERT(VARCHAR(MAX), PTPC.MAXCASESPERDAY)
                                                                                                                                                                                                                                                                      ELSE ''
                                                                                                                                                                                                                                                                  END
                                                     FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                                                          JOIN dbo.PAYMENTTYPEPERCASE AS PTPC WITH ( NOLOCK ) ON PTPC.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                                                     WHERE( CSL.CONTRACTID = @ContractID
                                                         OR @ContractServiceTypeID = ContractServiceTypeID
                                                          )
                                                      AND CSL.PaymentTypeID = 6;
                                                     IF @Valuestringdata IS NOT NULL
                                                         BEGIN
                                                             INSERT INTO @Temp_ContractFilterTable
                                                                    ( PaymentTypeID,
                                                                      IncludedCodesValueString
                                                                    )
                                                             VALUES( 6, @Valuestringdata );
                                                         END;
                                                             SELECT @Valuestringdata = NULL;  
  
                                                             --------------------- For Per Diem Payment Type ------------------------  
                                                             SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') + 'Days =' + CONCAT(PTPD.DAYSFROM, '-', PTPD.DAYSTO) + ', ' + 'Rate = $' + CONVERT( VARCHAR(MAX), REPLACE(RTRIM(REPLACE(REPLACE(RTRIM(LTRIM(STR(PTPD.RATE, 15, 2))), ' ', '0'), '.', ' ')), ' ', '.'))
                                                             FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                                                                  JOIN dbo.PAYMENTTYPEPERDIEM AS PTPD WITH ( NOLOCK ) ON PTPD.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                                                             WHERE( CSL.CONTRACTID = @ContractID
                                                                 OR @ContractServiceTypeID = ContractServiceTypeID
                                                                  )
                                                              AND CSL.PaymentTypeID = 7;
                                                             IF @Valuestringdata IS NOT NULL
                                                                 BEGIN
                                                                     INSERT INTO @Temp_ContractFilterTable
                                                                            ( PaymentTypeID,
                                                                              IncludedCodesValueString
                                                                            )
                                                                     VALUES( 7, @Valuestringdata );
                                                                 END;
                                                                     SELECT @Valuestringdata = NULL;  
  
                                                                     --------------------- For Percentage Discount Payment Payment Type ------------------------  
                                                                     SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') + '=' + CONVERT( VARCHAR(MAX), PTPDP.PERCENTAGE) + '%'
                                                                     FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                                                                          JOIN dbo.PAYMENTTYPEPERCENTAGEDISCOUNTPAYMENT AS PTPDP WITH ( NOLOCK ) ON PTPDP.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                                                                     WHERE( CSL.CONTRACTID = @ContractID
                                                                         OR @ContractServiceTypeID = ContractServiceTypeID
                                                                          )
                                                                      AND CSL.PaymentTypeID = 8;
																												
                                                                     IF @Valuestringdata IS NOT NULL
                                                                         BEGIN
                                                                             INSERT INTO @Temp_ContractFilterTable
                                                                                    ( PaymentTypeID,
                                                                                      IncludedCodesValueString
                                                                                    )
                                                                             VALUES( 8, @Valuestringdata );
                                                                         END;
                                                                             SELECT @Valuestringdata = NULL;  
    
                                                                             --------------------- For Per Visit Payment Type ------------------------  
                                                                             SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') + 'Rate = $' + CONVERT( VARCHAR(MAX), REPLACE(RTRIM(REPLACE(REPLACE(RTRIM(LTRIM(STR(PTPV.RATE, 15, 2))), ' ', '0'), '.', ' ')), ' ', '.'))
                                                                             FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                                                                                  JOIN dbo.PAYMENTTYPEPERVISIT AS PTPV WITH ( NOLOCK ) ON PTPV.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                                                                             WHERE( CSL.CONTRACTID = @ContractID
                                                                                 OR @ContractServiceTypeID = ContractServiceTypeID
                                                                                  )
                                                                              AND CSL.PaymentTypeID = 9;
                                                                             IF @Valuestringdata IS NOT NULL
                                                                                 BEGIN
                                                                                     INSERT INTO @Temp_ContractFilterTable
                                                                                            ( PaymentTypeID,
                                                                                              IncludedCodesValueString
                                                                                            )
                                                                                     VALUES( 9, @Valuestringdata );
                                                                                 END;
                                                                                     SELECT @Valuestringdata = NULL;  
   
                                                                                     --------------------- For Stop Loss Payment Type ------------------------  
                                                                                     SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') +' Threshold = [' + PTSL.THRESHOLD + '] , ' + 'Percentage = ' + CONVERT( VARCHAR(MAX), PTSL.PERCENTAGE) + '%' + +CASE
                                                                                                                                                                                                                                                                          WHEN PTSL.DAYS IS NOT NULL
                                                                                                                                                                                                                                                                          THEN ' , Days = ' + PTSL.DAYS
                                                                                                                                                                                                                                                                          ELSE ''
                                                                                                                                                                                                                                                                      END + CASE
                                                                                                                                                                                                                                                                                WHEN PTSL.REVCODE IS NOT NULL
                                                                                                                                                                                                                                                                                THEN ' , Rev Codes = ' + PTSL.REVCODE
                                                                                                                                                                                                                                                                                ELSE ''
                                                                                                                                                                                                                                                                            END + CASE
                                                                                                                                                                                                                                                                                      WHEN PTSL.CPTCODE IS NOT NULL
                                                                                                                                                                                                                                                                                      THEN ' , CPT/HCPCS Codes = ' + PTSL.CPTCODE
                                                                                                                                                                                                                                                                                      ELSE ''
                                                                                                                                                                                                                                                                                  END + CASE
                                                                                                                                                                                                                                                                                            WHEN PTSL.ISEXCESSCHARGE = 1
                                                                                                                                                                                                                                                                                            THEN+' , Condition = ' + CASE
                                                                                                                                                                                                                                                                                                                         WHEN PTSL.STOPLOSSCONDITIONID = 1
                                                                                                                                                                                                                                                                                                                         THEN 'Total Charge Lines, Excess Charges'
                                                                                                                                                                                                                                                                                                                         WHEN PTSL.STOPLOSSCONDITIONID = 2
                                                                                                                                                                                                                                                                                                                         THEN 'Per Charge Line, Excess Charges'
                                                                                                                                                                                                                                                                                                                         WHEN PTSL.STOPLOSSCONDITIONID = 3
                                                                                                                                                                                                                                                                                                                         THEN 'Per Day of Stay, Excess Charges'
                                                                                                                                                                                                                                                                                                                         ELSE ''
                                                                                                                                                                                                                                                                                                                     END
                                                                                                                                                                                                                                                                                            ELSE+' , Condition = ' + CASE
                                                                                                                                                                                                                                                                                                                         WHEN PTSL.STOPLOSSCONDITIONID = 1
                                                                                                                                                                                                                                                                                                                         THEN 'Total Charge Lines'
                                                                                                                                                                                                                                                                                                                         WHEN PTSL.STOPLOSSCONDITIONID = 2
                                                                                                                                                                                                                                                                                                                         THEN 'Per Charge Line'
                                                                                                                                                                                                                                                                                                                         WHEN PTSL.STOPLOSSCONDITIONID = 3
                                                                                                                                                                                                                                                                                                                         THEN 'Per Day of Stay'
                                                                                                                                                                                                                                                                                                                         ELSE ''
                                                                                                                                                                                                                                                                                                                     END
                                                                                                                                                                                                                                                                                        END
                                                                                     FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                                                                                          JOIN dbo.PAYMENTTYPESTOPLOSS AS PTSL WITH ( NOLOCK ) ON PTSL.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                                                                                     WHERE( CSL.CONTRACTID = @ContractID
                                                                                         OR @ContractServiceTypeID = ContractServiceTypeID
                                                                                          )
                                                                                      AND CSL.PaymentTypeID = 10;
                                                                                     IF @Valuestringdata IS NOT NULL
                                                                                         BEGIN
                                                                                             INSERT INTO @Temp_ContractFilterTable
                                                                                                    ( PaymentTypeID,
                                                                                                      IncludedCodesValueString
                                                                                                    )
                                                                                             VALUES( 10, @Valuestringdata );
                                                                                         END;
                                                                                             SELECT @Valuestringdata = NULL;  
  
                                                                                             --------------------- For CAP Payment Type  ------------------------  
                                                                                             SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') + 'Threshold =$' + CONVERT( VARCHAR(MAX), REPLACE(RTRIM(REPLACE(REPLACE(RTRIM(LTRIM(STR(PTC.THRESHOLD, 15, 2))), ' ', '0'), '.', ' ')), ' ', '.'))
                                                                                             FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                                                                                                  JOIN dbo.PAYMENTTYPECAP AS PTC WITH ( NOLOCK ) ON PTC.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                                                                                             WHERE( CSL.CONTRACTID = @ContractID
                                                                                                 OR @ContractServiceTypeID = ContractServiceTypeID
                                                                                                  )
                                                                                              AND CSL.PaymentTypeID = 12;
                                                                                             IF @Valuestringdata IS NOT NULL
                                                                                                 BEGIN
                                                                                                     INSERT INTO @Temp_ContractFilterTable
                                                                                                            ( PaymentTypeID,
                                                                                                              IncludedCodesValueString
                                                                                                            )
                                                                                                     VALUES( 12, @Valuestringdata );
                                                                                                 END;
                                                                                                     SELECT @Valuestringdata = NULL;

                                                                                                     --------------------- For Lesser Of Payment Type ------------------------  
                                                                                                     SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') + ( CASE PTPDP.ISLESSEROF
                                                                                                                                                                             WHEN 1
                                                                                                                                                                             THEN 'Lesser Of = '
                                                                                                                                                                             ELSE 'Greater Of = '
                                                                                                                                                                         END ) + CONVERT( VARCHAR(MAX), PTPDP.PERCENTAGE) + '%'
                                                                                                     FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                                                                                                          JOIN dbo.PaymentTypeLesserOf AS PTPDP WITH ( NOLOCK ) ON PTPDP.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                                                                                                     WHERE( CSL.CONTRACTID = @ContractID
                                                                                                         OR @ContractServiceTypeID = ContractServiceTypeID
                                                                                                          )
                                                                                                      AND CSL.PaymentTypeID = 11;
                                                                                                     IF @Valuestringdata IS NOT NULL
                                                                                                         BEGIN
                                                                                                             INSERT INTO @Temp_ContractFilterTable
                                                                                                                    ( PaymentTypeID,
                                                                                                                      IncludedCodesValueString
                                                                                                                    )
                                                                                                             VALUES( 11, @Valuestringdata );
                                                                                                         END;
                                                                                                             SELECT @Valuestringdata = NULL;  
                                                                                                             -------------------------------------For Custom Payment------------------------------------------------------
                                                                                                             SELECT @Valuestringdata = COALESCE(@Valuestringdata + ', ', '') + ' Table Name = ' + CFD.TABLENAME + ', ' + 'ClaimField =' + cf.[Text] + ', ' + 'Formula = [' + PTFS.Formula + ']'
																							 +CASE
																							  WHEN PTFS.IsObserveServiceUnit = 1
																							  THEN ', Observe Service Units Count'
																							  ELSE ''
																							  END
																							 +CASE 
																							  WHEN PTFS.IsPerDayOfStay  = 1
																							  THEN ', Scope: Per Day of Stay'
																							  ELSE ''
																							  END
																							 +CASE 
																							  WHEN PTFS.IsPerCode  = 1
																							  THEN ', Scope: Per Code'
																							  ELSE ''
																							  END
																							 +ISNULL([dbo].[GetCustomMultipliers](PTFS.MultiplierFirst,PTFS.MultiplierSecond,PTFS.MultiplierThird,PTFS.MultiplierFour,PTFS.MultiplierOther),'')
																							 +CASE 
																							  WHEN PTFS.ObserveServiceUnitLimit IS Not Null
																							  THEN ', Limit:'+PTFS.ObserveServiceUnitLimit
																							  ELSE ''
																							  END
                                                                                                             FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH ( NOLOCK )
                                                                                                                  JOIN dbo.PaymentTypeCustomTable AS PTFS WITH ( NOLOCK ) ON PTFS.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                                                                                                                  JOIN CLAIMFIELDDOCS AS CFD WITH ( NOLOCK ) ON CFD.CLAIMFIELDDOCID = PTFS.CLAIMFIELDDOCID
                                                                                                                  JOIN dbo.[ref.ClaimField] cf WITH ( NOLOCK ) ON PTFS.ClaimFieldId = cf.ClaimFieldId
                                                                                                             WHERE( CSL.CONTRACTID = @ContractID
                                                                                                                 OR @ContractServiceTypeID = ContractServiceTypeID
                                                                                                                  )
                                                                                                              AND CSL.PaymentTypeID = 14;
                                                                                                             IF @Valuestringdata IS NOT NULL
                                                                                                                 BEGIN
                                                                                                                     INSERT INTO @Temp_ContractFilterTable
                                                                                                                            ( PaymentTypeID,
                                                                                                                              IncludedCodesValueString
                                                                                                                            )
                                                                                                                     VALUES( 14, @Valuestringdata );
                                                                                                                 END;
                                                                                                                     SELECT @Valuestringdata = NULL;  
                                                                                                                     -------------------------------------For Medicare Sequester------------------------------------------------------

                                                                                                                     SELECT @ValueStringData = COALESCE(@ValueStringData + ', ', '') + ' = ' + CONVERT( VARCHAR(MAX), PTMS.Percentage) + '%'
                                                                                                                     FROM dbo.ContractServiceLinePaymentTypes AS CSL WITH ( NOLOCK )
                                                                                                                          JOIN dbo.PaymentTypeMedicareSequester AS PTMS WITH ( NOLOCK ) ON PTMS.PaymentTypeDetailID = CSL.PaymentTypeDetailID
                                                                                                                     WHERE( CSL.ContractID = @ContractID
                                                                                                                         OR @ContractServiceTypeID = ContractServiceTypeID
                                                                                                                          )
                                                                                                                      AND CSL.PaymentTypeID = 15;

                                                                                                                     IF @ValueStringData IS NOT NULL
                                                                                                                         BEGIN
                                                                                                                             INSERT INTO @Temp_ContractFilterTable
                                                                                                                                    ( PaymentTypeID,
                                                                                                                                      IncludedCodesValueString
                                                                                                                                    )
                                                                                                                             VALUES( 15, @ValueStringData );
                                                                                                                         END;
                                                                                                                             SELECT @ValueStringData = NULL;  

	
                                                                                                                             --------------------Combine all payment types and get single string -----------------------------
                                                                                                                             WITH FilterCodes
                                                                                                                                  AS ( SELECT TOP 100 ISNULL(IncludedCodesValueString, '') AS FILTERVALUES,
                                                                                                                                                      ( CASE PT.PaymentTypeID
                                                                                                                                                            WHEN 11
                                                                                                                                                            THEN ''
                                                                                                                                                            ELSE PAYMENTTYPENAME
                                                                                                                                                        END ) AS FILTERNAME
                                                                                                                                       FROM @Temp_ContractFilterTable AS TEMPTABLE
                                                                                                                                            JOIN dbo.[REF.PAYMENTTYPES] AS PT WITH ( NOLOCK ) ON PT.PaymentTypeID = TEMPTABLE.PaymentTypeID
                                                                                                                                            JOIN dbo.CONTRACTSERVICELINEPAYMENTTYPES CSP WITH ( NOLOCK ) ON CSP.PaymentTypeID = TEMPTABLE.PaymentTypeID
                                                                                                                                       WHERE CONTRACTID = @ContractID
                                                                                                                                          OR ContractServiceTypeID = @ContractServiceTypeID
                                                                                                                                       ORDER BY TEMPTABLE.RowNumber ASC )
                                                                                                                                  SELECT @Stlist = COALESCE(@Stlist + ', ', '') + FILTERNAME + ' ' + FilterValues
                                                                                                                                  FROM FilterCodes;

																										SET @Stlist=REPLACE(@Stlist,'Percent of Charges','Percentage of Charges')

                                                                                                                                  RETURN @Stlist;
                                                                                                                             END;