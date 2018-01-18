CREATE PROCEDURE dbo.CopyContractServiceTypeByID(
       @Contractservicetypeid   BIGINT,
       @Contractservicetypename VARCHAR(500),
       @UserName                VARCHAR(50))
AS  

/****************************************************************************  
 *   Name         : CopyContractServiceTypeByID
 *   Author       : Vishesh Bhawsar  
 *   Date         : 23/Jan/2014  
 *   Module       : Copy Contract Service Type
 *   Description  : Insert duplicate Contract Service Type Information
 *	 Modified By  : Sheshagiri on 07/23/2014 to include Non Fee Schedule parameter
 *	 EXEC CopyContractServiceTypeByID  1,'Copy of primary model'
 *****************************************************************************/

    BEGIN
        SET NOCOUNT ON;
        DECLARE @Currentdate                   DATETIME = GETUTCDATE(),
                @Description                   NVARCHAR(MAX) = 'Notes: ',
                @Transactionname               VARCHAR(100) = 'CopyContractServiceTypeByID',
                @Notes                         VARCHAR(MAX),
                @IsCarveOut                    BIT,
                @InsertedContractServiceTypeID BIGINT;
        BEGIN TRY
            BEGIN TRAN @Transactionname;
            DECLARE @Tempcontractservicetypes TABLE( INSERTEDID BIGINT ); 

            --Inserting Duplicate data of ContractServiceTypes
            INSERT INTO dbo.CONTRACTSERVICETYPES
                   ( INSERTDATE,
                     UPDATEDATE,
                     CONTRACTID,
                     CONTRACTSERVICETYPENAME,
                     NOTES,
                     ISCARVEOUT
                   )
            OUTPUT INSERTED.CONTRACTSERVICETYPEID
                   INTO @Tempcontractservicetypes
                   SELECT @Currentdate,
                          NULL,
                          CONTRACTID,
                          @Contractservicetypename,
                          NOTES,
                          ISCARVEOUT
                   FROM CONTRACTSERVICETYPES
                   WHERE CONTRACTSERVICETYPEID = @Contractservicetypeid;
            SET @InsertedContractServiceTypeID = SCOPE_IDENTITY();
            ----Insert Audit Log---

            SELECT @Notes = NOTES,
                   @IsCarveOut = ISCARVEOUT
            FROM CONTRACTSERVICETYPES
            WHERE CONTRACTSERVICETYPEID = @Contractservicetypeid;
            IF @Notes IS NOT NULL
                BEGIN
                    SET @Description = @Description + @Notes;
                END;
            IF @IsCarveOut = 1
                BEGIN
                    SET @Description = @Description + ' , CarveOut';
                END;
            BEGIN
                EXEC InsertAuditLog
                     @UserName,
                     'Add',
                     'Service Type',
                     @Description,
                     @InsertedContractServiceTypeID,
                     0;
            END;

            ----Insert Audit Log---

            DECLARE @Duplicatecontractservicetypeid BIGINT;
            SELECT @Duplicatecontractservicetypeid = INSERTEDID
            FROM @Tempcontractservicetypes;

            -- define the last Node ID which need to be copyed, this is going to be our Node ID which are passing to SP
            DECLARE @Lastcontractservicelinepaymenttypeid BIGINT;
            SET @Lastcontractservicelinepaymenttypeid = 0;
            -- define the Node Id to be copy now
            -- We need to keep update ParentID as well as NodeID while added new node into database based on condition
            DECLARE @Contractservicelinepaymenttypeidtohandle BIGINT;

            -- We are selecting the next Node to copy
            SELECT TOP 1 @Contractservicelinepaymenttypeidtohandle = CONTRACTSERVICELINEPAYMENTTYPEID
            FROM CONTRACTSERVICELINEPAYMENTTYPES
            WHERE CONTRACTSERVICELINEPAYMENTTYPEID > @Lastcontractservicelinepaymenttypeid
              AND CONTRACTSERVICETYPEID = @Contractservicetypeid
            ORDER BY CONTRACTSERVICELINEPAYMENTTYPEID;
            DECLARE @Servicelinetypeid BIGINT;
            DECLARE @Contractservicelineid BIGINT;
            DECLARE @Paymenttypedetailid BIGINT;
            DECLARE @Paymenttypeid BIGINT;
            DECLARE @Temppaymenttypedetail TABLE( INSERTEDID BIGINT );
            DECLARE @Newpaymenttypedetailid BIGINT;
            -- As long as we have data we need to keep copy them
            WHILE @Contractservicelinepaymenttypeidtohandle IS NOT NULL
                BEGIN

                    -- Copy Items one by one here and ALTER  new entries
                    SELECT @Servicelinetypeid = SERVICELINETYPEID,
                           @Contractservicelineid = CONTRACTSERVICELINEID,
                           @Paymenttypedetailid = PAYMENTTYPEDETAILID,
                           @Paymenttypeid = PAYMENTTYPEID
                    FROM CONTRACTSERVICELINEPAYMENTTYPES
                    WHERE CONTRACTSERVICELINEPAYMENTTYPEID = @Contractservicelinepaymenttypeidtohandle;
                    IF @Servicelinetypeid IS NOT NULL
                        BEGIN
                            -- Copy Service Line
                            DECLARE @Tempcontractserviceline TABLE( INSERTEDID BIGINT );
                            IF @Servicelinetypeid = 8
                                BEGIN
                                    INSERT INTO dbo.CONTRACTSERVICELINETABLESELECTION
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             CLAIMFIELDID,
                                             CLAIMFIELDDOCID,
                                             Operator
                                           )
                                    OUTPUT INSERTED.CONTRACTSERVICELINEID
                                           INTO @Tempcontractserviceline
                                           SELECT @Currentdate,
                                                  NULL,
                                                  CLAIMFIELDID,
                                                  CLAIMFIELDDOCID,
                                                  Operator
                                           FROM CONTRACTSERVICELINETABLESELECTION
                                           WHERE CONTRACTSERVICELINEID = @Contractservicelineid;

                                    --Insert Audit Log for each Claim Tool---
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE ServiceLineTypeID = @ServiceLineTypeID;
                                    SET @Description = 'Added claim tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                    --Insert Audit Log for each Claim Tool----

                                END;
                            ELSE
                                BEGIN
                                    IF @Servicelinetypeid = 7
                                        BEGIN
                                            INSERT INTO dbo.CONTRACTSERVICELINECLAIMFIELDSELECTION
                                                   ( INSERTDATE,
                                                     UPDATEDATE,
                                                     CLAIMFIELDID,
                                                     OPERATORID,
                                                     [VALUES]
                                                   )
                                            OUTPUT INSERTED.CONTRACTSERVICELINEID
                                                   INTO @Tempcontractserviceline
                                                   SELECT @Currentdate,
                                                          NULL,
                                                          CLAIMFIELDID,
                                                          OPERATORID,
                                                          [VALUES]
                                                   FROM CONTRACTSERVICELINECLAIMFIELDSELECTION
                                                   WHERE CONTRACTSERVICELINEID = @Contractservicelineid;


                                            --Insert Audit Log for each Claim Tool---
                                            SELECT @Description = FilterName + FilterValues
                                            FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                            WHERE ServiceLineTypeID = @ServiceLineTypeID;
                                            SET @Description = 'Added claim tool : ' + @Description;
                                            EXEC InsertAuditLog
                                                 @UserName,
                                                 'Modify',
                                                 'Service Type',
                                                 @Description,
                                                 @InsertedContractServiceTypeID,
                                                 0;
                                            SET @Description = '';
                                            --Insert Audit Log for each Claim Tool----
                                        END;
                                    ELSE
                                        BEGIN
                                            INSERT INTO dbo.CONTRACTSERVICELINES
                                                   ( INSERTDATE,
                                                     UPDATEDATE,
                                                     INCLUDEDCODE,
                                                     EXCLUDEDCODE
                                                   )
                                            OUTPUT INSERTED.CONTRACTSERVICELINEID
                                                   INTO @Tempcontractserviceline
                                                   SELECT @Currentdate,
                                                          NULL,
                                                          INCLUDEDCODE,
                                                          EXCLUDEDCODE
                                                   FROM CONTRACTSERVICELINES
                                                   WHERE CONTRACTSERVICELINEID = @Contractservicelineid;

                                            --Insert Audit Log for each Claim Tool---
                                            SELECT @Description = FilterName + FilterValues
                                            FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                            WHERE ServiceLineTypeID = @ServiceLineTypeID;
                                            SET @Description = 'Added claim tool : ' + @Description;
                                            EXEC InsertAuditLog
                                                 @UserName,
                                                 'Modify',
                                                 'Service Type',
                                                 @Description,
                                                 @InsertedContractServiceTypeID,
                                                 0;
                                            SET @Description = '';
                                            --Insert Audit Log for each Claim Tool----
                                        END;
                                END;
                            SELECT TOP 1 @Contractservicelineid = INSERTEDID
                            FROM @Tempcontractserviceline;
                        END;
                    IF @Paymenttypeid IS NOT NULL
                        BEGIN 
                            -- Insert into PaymentTypeASCFeeSchedules When @PaymentTypeID 1
                            IF @Paymenttypeid = 1
                                BEGIN
                                    INSERT INTO dbo.PAYMENTTYPEASCFEESCHEDULES
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             [PRIMARY],
                                             SECONDARY,
                                             TERTIARY,
                                             QUATERNARY,
                                             OTHERS,
                                             NONFEESCHEDULE,
                                             CLAIMFIELDDOCID,
                                             SELECTEDOPTION
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  [PRIMARY],
                                                  SECONDARY,
                                                  TERTIARY,
                                                  QUATERNARY,
                                                  OTHERS,
                                                  NONFEESCHEDULE,
                                                  CLAIMFIELDDOCID,
                                                  SELECTEDOPTION
                                           FROM PAYMENTTYPEASCFEESCHEDULES
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END;

                            -- Insert into [PaymentTypeDRGPayment] When @PaymentTypeID = 2
                            IF @Paymenttypeid = 2
                                BEGIN
                                    INSERT INTO dbo.PAYMENTTYPEDRGPAYMENT
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             BASERATE,
                                             CLAIMFIELDDOCID
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  BASERATE,
                                                  CLAIMFIELDDOCID
                                           FROM PAYMENTTYPEDRGPAYMENT
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END;
                            -- Insert into [[PaymentTypeFeeSchedules]] When @PaymentTypeID = 3
                            IF @Paymenttypeid = 3
                                BEGIN
                                    INSERT INTO dbo.PAYMENTTYPEFEESCHEDULES
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             FEESCHEDULE,
                                             NONFEESCHEDULE,
                                             CLAIMFIELDDOCID,
									IsObserveUnits
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  FEESCHEDULE,
                                                  NONFEESCHEDULE,
                                                  CLAIMFIELDDOCID,
										IsObserveUnits
                                           FROM PAYMENTTYPEFEESCHEDULES
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END;
                            -- Insert into [PaymentTypeMedicareIPPayment] When @PaymentTypeID = 4
                            IF @Paymenttypeid = 4
                                BEGIN
                                    INSERT INTO dbo.PAYMENTTYPEMEDICAREIPPAYMENT
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             INPATIENT,
                                             FORMULA
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  INPATIENT,
                                                  FORMULA
                                           FROM PAYMENTTYPEMEDICAREIPPAYMENT
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END;
                    
                            -- Insert into [PaymentTypeMedicareOPPayment] When @PaymentTypeID = 5
                            IF @Paymenttypeid = 5
                                BEGIN
                                    INSERT INTO dbo.PAYMENTTYPEMEDICAREOPPAYMENT
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             OUTPATIENT
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  OUTPATIENT
                                           FROM PAYMENTTYPEMEDICAREOPPAYMENT
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END;
                    
                            -- Insert into [PaymentTypeMedicareLabFeeSchedule] When @PaymentTypeID = 13
                            IF @Paymenttypeid = 13
                                BEGIN
                                    INSERT INTO dbo.PAYMENTTYPEMEDICARELABFEESCHEDULE
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             PERCENTAGE
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  PERCENTAGE
                                           FROM PAYMENTTYPEMEDICARELABFEESCHEDULE
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END;
                    
                            -- Insert into [PaymentTypePerCase] When @PaymentTypeID = 6
                            IF @Paymenttypeid = 6
                                BEGIN
                                    INSERT INTO dbo.PAYMENTTYPEPERCASE
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             RATE,
                                             MAXCASESPERDAY
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  RATE,
                                                  MAXCASESPERDAY
                                           FROM PAYMENTTYPEPERCASE
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END;
                            -- Insert into [PaymentTypePerDiem] When @PaymentTypeID = 7
                            IF @Paymenttypeid = 7
                                BEGIN
                                    INSERT INTO dbo.PAYMENTTYPEPERDIEM
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             RATE,
                                             DAYSFROM,
                                             DAYSTO
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  RATE,
                                                  DAYSFROM,
                                                  DAYSTO
                                           FROM PAYMENTTYPEPERDIEM
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END; 
                            -- Insert into [PaymentTypePercentageDiscountPayment] When @PaymentTypeID = 8
                            IF @Paymenttypeid = 8
                                BEGIN
                                    INSERT INTO dbo.PAYMENTTYPEPERCENTAGEDISCOUNTPAYMENT
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             PERCENTAGE
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  PERCENTAGE
                                           FROM PAYMENTTYPEPERCENTAGEDISCOUNTPAYMENT
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END; 
                    
               
                            -- Insert into [PaymentTypePerVisit] When @PaymentTypeID = 9
                            IF @Paymenttypeid = 9
                                BEGIN
                                    INSERT INTO dbo.PAYMENTTYPEPERVISIT
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             RATE
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  RATE
                                           FROM PAYMENTTYPEPERVISIT
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END;

                            -- Insert into [PaymentTypeCustomTable] When @PaymentTypeID = 14
                            IF @Paymenttypeid = 14
                                BEGIN
                                    INSERT INTO dbo.PaymentTypeCustomTable
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             CLAIMFIELDDOCID,
                                             CLAIMFIELDID,
                                             FORMULA,
									MultiplierFirst,
									MultiplierSecond,
									MultiplierThird,
									MultiplierFour,
									MultiplierOther,
									IsObserveServiceUnit,
									ObserveServiceUnitLimit,
									IsPerDayOfStay,
									IsPerCode
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  CLAIMFIELDDOCID,
                                                  CLAIMFIELDID,
                                                  FORMULA,
										MultiplierFirst,
									     MultiplierSecond,
									     MultiplierThird,
									     MultiplierFour,
									     MultiplierOther,
									     IsObserveServiceUnit,
									     ObserveServiceUnitLimit,
									     IsPerDayOfStay,
									     IsPerCode
                                           FROM PaymentTypeCustomTable
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END;


                            -- Insert into [PaymentTypeStopLoss] When @PaymentTypeID = 10
                            IF @Paymenttypeid = 10
                                BEGIN
                                    INSERT INTO dbo.PAYMENTTYPESTOPLOSS
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             PERCENTAGE,
                                             THRESHOLD,
                                             DAYS,
                                             REVCODE,
                                             CPTCODE,
                                             STOPLOSSCONDITIONID,
                                             ISEXCESSCHARGE
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  PERCENTAGE,
                                                  THRESHOLD,
                                                  DAYS,
                                                  REVCODE,
                                                  CPTCODE,
                                                  STOPLOSSCONDITIONID,
                                                  ISEXCESSCHARGE
                                           FROM PAYMENTTYPESTOPLOSS
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END;

                                   
                            -- Insert into PaymentTypeCap When @PaymentTypeID = 12
                            IF @Paymenttypeid = 12
                                BEGIN
                                    INSERT INTO dbo.PAYMENTTYPECAP
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             THRESHOLD,
                                             PERCENTAGE
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  THRESHOLD,
                                                  PERCENTAGE
                                           FROM dbo.PAYMENTTYPECAP
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;

                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END;

                            -- Insert into PaymentTypeLesserOf/GreaterOf When @PaymentTypeID = 11
                            IF @Paymenttypeid = 11
                                BEGIN
                                    INSERT INTO dbo.PaymentTypeLesserOf
                                           ( INSERTDATE,
                                             UPDATEDATE,
                                             Percentage,
                                             IsLesserOf
                                           )
                                    OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                           INTO @Temppaymenttypedetail
                                           SELECT @Currentdate,
                                                  NULL,
                                                  Percentage,
                                                  IsLesserOf
                                           FROM dbo.PaymentTypeLesserOf
                                           WHERE PAYMENTTYPEDETAILID = @Paymenttypedetailid;
                                   
                                    --Insert Audit Log for each Payment Tool			    
                                    SELECT @Description = FilterName + FilterValues
                                    FROM GetContractFiltersByID( NULL, @ContractServiceTypeID )
                                    WHERE PaymentTypeID = @Paymenttypeid;
                                    SET @Description = 'Added Payment tool : ' + @Description;
                                    EXEC InsertAuditLog
                                         @UserName,
                                         'Modify',
                                         'Service Type',
                                         @Description,
                                         @InsertedContractServiceTypeID,
                                         0;
                                    SET @Description = '';
                                END;
                        END;
                    SELECT @Newpaymenttypedetailid = INSERTEDID
                    FROM @Temppaymenttypedetail;
                    -- Insert into [ContractServiceLinePaymentTypes]
                    INSERT INTO dbo.CONTRACTSERVICELINEPAYMENTTYPES
                           ( PAYMENTTYPEDETAILID,
                             INSERTDATE,
                             UPDATEDATE,
                             PAYMENTTYPEID,
                             CONTRACTSERVICELINEID,
                             CONTRACTID,
                             SERVICELINETYPEID,
                             CONTRACTSERVICETYPEID
                           )
                           SELECT @Newpaymenttypedetailid,
                                  @Currentdate,
                                  NULL,
                                  PAYMENTTYPEID,
                                  @Contractservicelineid,
                                  CONTRACTID,
                                  SERVICELINETYPEID,
                                  @Duplicatecontractservicetypeid
                           FROM CONTRACTSERVICELINEPAYMENTTYPES
                           WHERE CONTRACTSERVICELINEPAYMENTTYPEID = @Contractservicelinepaymenttypeidtohandle;
                    SET @Newpaymenttypedetailid = NULL;
                    SET @Servicelinetypeid = NULL;
                    SET @Contractservicelineid = NULL;
                    SET @Paymenttypedetailid = NULL;
                    SET @Paymenttypeid = NULL;
                    DELETE FROM @Tempcontractservicetypes;
                    DELETE FROM @Temppaymenttypedetail;
                    DELETE FROM @Tempcontractserviceline;
                    -- set the last Node handled to the one we just copyed
                    SET @Lastcontractservicelinepaymenttypeid = @Contractservicelinepaymenttypeidtohandle;
                    SET @Contractservicelinepaymenttypeidtohandle = NULL;

                    -- select the next Node to copy
                    SELECT TOP 1 @Contractservicelinepaymenttypeidtohandle = CONTRACTSERVICELINEPAYMENTTYPEID
                    FROM CONTRACTSERVICELINEPAYMENTTYPES
                    WHERE CONTRACTSERVICELINEPAYMENTTYPEID > @Lastcontractservicelinepaymenttypeid
                      AND CONTRACTSERVICETYPEID = @Contractservicetypeid
                    ORDER BY CONTRACTSERVICELINEPAYMENTTYPEID;
                END;

            -- Copy Sevice type and payment type codes
            INSERT INTO ServiceTypePaymentTypeFilterCodes
                   SELECT @Duplicatecontractservicetypeid,
                          @Currentdate,
                          NULL,
                          NULL,
                          FC.serviceTypecodes,
                          FC.Paymenttypecodes
                   FROM ServiceTypePaymentTypeFilterCodes FC
                   WHERE FC.CONTRACTSERVICETYPEID = @Contractservicetypeid;
            SELECT @Duplicatecontractservicetypeid;  --returning ContractId  

		  		  
		  IF EXISTS( SELECT 1
				   FROM ContractServiceLinePaymentTypes
				   WHERE ContractServiceTypeID = @Duplicatecontractservicetypeid )
			 BEGIN
				--Updating Contract GUID
				EXEC [UpdateContractGUID]
					NULL,
					@Duplicatecontractservicetypeid;
			 END;	

            ----Check Any Transation happened than commit transation
            COMMIT TRANSACTION @Transactionname;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;