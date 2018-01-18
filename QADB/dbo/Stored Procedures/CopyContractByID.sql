CREATE PROCEDURE [dbo].[CopyContractByID](@ContractID BIGINT,
                                          @NodeID     BIGINT,
                                          @UserName   VARCHAR(100),
                                          @IsDeleted  BIT          = 0)
AS  

/****************************************************************************  
 *   Name         : CopyContractById  1,1
 *   Author       : Vishesh Bhawsar  
 *   Date         : 8/19/2013  
 *   Module       : Copy Contract  
 *   Description  : Insert duplicate Contract Information  
 *	 Modified By  : Sheshagiri on 07/23/2014 to include Non Fee Schedule parameter
 *	 EXEC CopyContractById  1,5,'jay'
 *****************************************************************************/

     BEGIN
         SET NOCOUNT ON;
         DECLARE @CurrentDate DATETIME;
         DECLARE @ContractName VARCHAR(500);
         DECLARE @ClaimToolDesc VARCHAR(MAX);
         DECLARE @LatestInsertedContractServiceTypeID BIGINT;
         SELECT @ContractName = NODETEXT
         FROM CONTRACTHIERARCHY
         WHERE NODEID = @NodeID;
         SET @CurrentDate =
         (
             SELECT GETUTCDATE()
         );
         DECLARE @DuplicateContractID BIGINT;  
 
         --Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED
         DECLARE @Tmptable TABLE(INSERTEDID BIGINT); 

         --Inserting Duplicate Contract Information  
         INSERT INTO dbo.CONTRACTS
         (INSERTDATE,
          UPDATEDATE,
          CONTRACTNAME,
          STARTDATE,
          ENDDATE,
          FACILITYID,
          ISACTIVE,
          ISCLAIMSTARTDATE,
          ISPROFESSIONALCLAIM,
          ISINSTITUTIONALCLAIM,
          ISMODIFIED,
          NODEID,
          USERNAME,
          ISEXPIRED,
          ISDELETED,
          ThresholdDaysToExpireAlters,
          PayerCode,
          CustomField
         )
         OUTPUT INSERTED.CONTRACTID
                INTO @Tmptable
                SELECT @CurrentDate,
                       NULL,
                       @ContractName,
                       STARTDATE,
                       ENDDATE,
                       FACILITYID,
                       ISACTIVE,
                       ISCLAIMSTARTDATE,
                       ISPROFESSIONALCLAIM,
                       ISINSTITUTIONALCLAIM,
                       ISMODIFIED,
                       @NodeID,
                       @UserName,
                       ISEXPIRED,
                       @IsDeleted,
                       ThresholdDaysToExpireAlters,
                       PayerCode,
                       CustomField
                FROM CONTRACTS
                WHERE CONTRACTID = @ContractID;
         SET @DuplicateContractID =
         (
             SELECT TOP 1 INSERTEDID
             FROM @Tmptable
         ); --Assigning ContractIdvalue to the duplicate Contract  
         DELETE @Tmptable;
         --Inserting Duplicate data of ContractPayers Information  
         INSERT INTO dbo.CONTRACTPAYERS
         (INSERTDATE,
          UPDATEDATE,
          CONTRACTID,
          PAYERNAME
         )
                SELECT @CurrentDate,
                       NULL,
                       @DuplicateContractID,
                       PAYERNAME
                FROM CONTRACTPAYERS
                WHERE CONTRACTID = @ContractID;
         -- Inserting the Audit log  
         EXEC InsertAuditLog
              @UserName,
              'Add',
              'Contract',
              '',
              @DuplicateContractID,
              1;
         --Inserting Duplicate data of ContractPayerInfo  
         INSERT INTO dbo.CONTRACTINFO
         (INSERTDATE,
          UPDATEDATE,
          CONTRACTID,
          CONTRACTINFONAME,
          MAILADDRESS1,
          MAILADDRESS2,
          CITY,
          STATE,
          ZIP,
          PHONE1,
          PHONE2,
          FAX,
          EMAIL,
          WEBSITE,
          TAXID,
          NPI,
          MEMO,
          PROVDERID,
          PLANID
         )
                SELECT @CurrentDate,
                       NULL,
                       @DuplicateContractID,
                       CONTRACTINFONAME,
                       MAILADDRESS1,
                       MAILADDRESS2,
                       CITY,
                       STATE,
                       ZIP,
                       PHONE1,
                       PHONE2,
                       FAX,
                       EMAIL,
                       WEBSITE,
                       TAXID,
                       NPI,
                       MEMO,
                       PROVDERID,
                       PLANID
                FROM CONTRACTINFO
                WHERE CONTRACTID = @ContractID;
         DECLARE @ContractInfoCount INT=
         (
             SELECT COUNT(*)
             FROM CONTRACTINFO
             WHERE CONTRACTID = @ContractID
         );
         WHILE(@ContractInfoCount > 0)
             BEGIN
                 --Insert AuditLog information 
                 SET @ClaimToolDesc = 'Added Contact : '+
                 (
                     SELECT TOP 1 CONTRACTINFONAME
                     FROM
                     (
                         SELECT TOP (@ContractInfoCount) CONTRACTINFONAME
                         FROM CONTRACTINFO
                         WHERE CONTRACTID = @ContractID
                         ORDER BY 1 DESC
                     ) CONTRACTName
                     ORDER BY 1
                 );
                 EXEC InsertAuditLog
                      @UserName,
                      'Modify',
                      'Contract',
                      @ClaimToolDesc,
                      @DuplicateContractID,
                      1;
                 SET @ContractInfoCount = @ContractInfoCount - 1;
             END;
         --Inserting Duplicate data of ContractNotes  
         INSERT INTO dbo.CONTRACTNOTES
         (INSERTDATE,
          UPDATEDATE,
          CONTRACTID,
          NOTETEXT,
          USERNAME
         )
                SELECT @CurrentDate,
                       NULL,
                       @DuplicateContractID,
                       NOTETEXT,
                       @UserName
                FROM CONTRACTNOTES
                WHERE CONTRACTID = @ContractID;
         DECLARE @ContractNotesCount INT=
         (
             SELECT COUNT(*)
             FROM CONTRACTNOTES
             WHERE CONTRACTID = @ContractID
         );
         WHILE(@ContractNotesCount > 0)
             BEGIN 
                 --Insert AuditLog information 
                 EXEC InsertAuditLog
                      @UserName,
                      'Modify',
                      'Contract',
                      'Added note',
                      @DuplicateContractID,
                      1;
                 SET @ContractNotesCount = @ContractNotesCount - 1;
             END;
         --Inserting Duplicate data of ContractDocs  
         INSERT INTO dbo.CONTRACTDOCS
         (INSERTDATE,
          UPDATEDATE,
          CONTRACTID,
          CONTRACTCONTENT,
          FILENAME,
          DOCUMENTID
         )
                SELECT @CurrentDate,
                       NULL,
                       @DuplicateContractID,
                       CONTRACTCONTENT,
                       FILENAME,
                       DOCUMENTID
                FROM CONTRACTDOCS
                WHERE CONTRACTID = @ContractID;
         DECLARE @ContractDocsCount INT=
         (
             SELECT COUNT(*)
             FROM CONTRACTDOCS
             WHERE CONTRACTID = @ContractID
         );
         WHILE(@ContractDocsCount > 0)
             BEGIN 
                 --Insert AuditLog information 
                 SELECT @ClaimToolDesc = 'Added attachment: '+
                 (
                     SELECT TOP 1 [FileName]
                     FROM
                     (
                         SELECT TOP (@ContractDocsCount) [FileName]
                         FROM CONTRACTDOCS
                         WHERE CONTRACTID = @DuplicateContractID
                         ORDER BY 1 DESC
                     ) CONTRACTDOCID
                     ORDER BY 1
                 );
                 SET @ContractDocsCount = @ContractDocsCount - 1;
                 EXEC InsertAuditLog
                      @UserName,
                      'Modify',
                      'Contract',
                      @ClaimToolDesc,
                      @DuplicateContractID,
                      1;
             END;

         --Inserting Duplicate data of ContractModifiedReasons
         INSERT INTO dbo.CONTRACTMODIFIEDREASONS
         (CONTRACTMODIFIEDREASONCODEID,
          CONTRACTID,
          REASONNOTES
         )
                SELECT CONTRACTMODIFIEDREASONCODEID,
                       @DuplicateContractID,
                       REASONNOTES
                FROM CONTRACTMODIFIEDREASONS
                WHERE CONTRACTID = @ContractID;

         --Inserting Duplicate data of ContractAlerts
         INSERT INTO dbo.CONTRACTALERTS
         (INSERTDATE,
          UPDATEDATE,
          CONTRACTID,
          ISDISMISSED,
          ENDDATE
         )
                SELECT TOP (1) INSERTDATE,
                               UPDATEDATE,
                               @DuplicateContractID,
                               NULL,
                               ENDDATE
                FROM CONTRACTALERTS
                WHERE CONTRACTID = @ContractID;


         -- define the last ContractServiceTypeID which need to be copyed, this is going to be our ContractServiceTypeID which are passing to SP
         DECLARE @LastContractServiceTypeID BIGINT= 0;
         DECLARE @Temp_ContractServiceType TABLE
         (OLDCONTRACTSERVICETYPEID BIGINT,
          NEWCONTRACTSERVICETYPEID BIGINT
         );

         -- define the ContractServiceTypeID to be copy now
         -- We need to keep update ContractServiceTypeID while added new node into database based on condition
         DECLARE @ContractServiceTypeIDToHandle BIGINT;
         DECLARE @TempContractServiceTypes TABLE(INSERTEDID BIGINT); 
         -- We are selecting the next ContractServiceTypeID to copy
         SELECT TOP 1 @ContractServiceTypeIDToHandle = CONTRACTSERVICETYPEID
         FROM CONTRACTSERVICETYPES
         WHERE CONTRACTSERVICETYPEID > @LastContractServiceTypeID
               AND CONTRACTID = @ContractID
               AND ISDELETED = 0
         ORDER BY CONTRACTSERVICETYPEID;
         -- As long as we have ContractServiceTypeID we need to keep copy them
         WHILE @ContractServiceTypeIDToHandle IS NOT NULL
             BEGIN
                 --Inserting Duplicate data of ContractServiceTypes
                 INSERT INTO dbo.CONTRACTSERVICETYPES
                 (INSERTDATE,
                  UPDATEDATE,
                  CONTRACTID,
                  CONTRACTSERVICETYPENAME,
                  NOTES,
                  ISCARVEOUT
                 )
                 OUTPUT INSERTED.CONTRACTSERVICETYPEID
                        INTO @TempContractServiceTypes
                        SELECT @CurrentDate,
                               NULL,
                               @DuplicateContractID,
                               CONTRACTSERVICETYPENAME,
                               NOTES,
                               ISCARVEOUT
                        FROM CONTRACTSERVICETYPES
                        WHERE CONTRACTID = @ContractID
                              AND CONTRACTSERVICETYPEID = @ContractServiceTypeIDToHandle;
		   
                 --Insert AuditLog information 
                 DECLARE @Description NVARCHAR(500)= 'Notes: ';
                 DECLARE @IsCarveOut BIT;
                 DECLARE @Notes VARCHAR(MAX);
                 SELECT @Notes = Notes,
                        @IsCarveOut = IsCarveOut
                 FROM CONTRACTSERVICETYPES CS
                      INNER JOIN @TempContractServiceTypes tempServiceType ON CS.ContractServiceTypeID = tempServiceType.INSERTEDID
                 WHERE CONTRACTID = @DuplicateContractID;
                 IF @Notes IS NOT NULL
                     BEGIN
                         SET @Description = @Description + @Notes;
                     END;
                 IF @IsCarveOut = 1
                     BEGIN
                         SET @Description = @Description+', Carve Out ';
                     END;
                 DECLARE @AuditServiceTypeID BIGINT=
                 (
                     SELECT INSERTEDID
                     FROM @TempContractServiceTypes
                 );
                 EXEC InsertAuditLog
                      @UserName,
                      'Add',
                      'Service Type',
                      @Description,
                      @AuditServiceTypeID,
                      0;
                 INSERT INTO @Temp_ContractServiceType
                        SELECT @ContractServiceTypeIDToHandle,
                               INSERTEDID
                        FROM @TempContractServiceTypes;
                 DELETE FROM @TempContractServiceTypes;
                 -- set the last Node handled to the one we just copyed
                 SET @LastContractServiceTypeID = @ContractServiceTypeIDToHandle;
                 SET @ContractServiceTypeIDToHandle = NULL;
			
                 -- select the next Node to copy
                 SELECT TOP 1 @ContractServiceTypeIDToHandle = CONTRACTSERVICETYPEID
                 FROM CONTRACTSERVICETYPES
                 WHERE CONTRACTSERVICETYPEID > @LastContractServiceTypeID
                       AND CONTRACTID = @ContractID
                       AND ISDELETED = 0
                 ORDER BY CONTRACTSERVICETYPEID;
             END;


         -- define the last Node ID which need to be copyed, this is going to be our Node ID which are passing to SP
         DECLARE @LastContractServiceLinePaymentTypeID BIGINT;
         SET @LastContractServiceLinePaymentTypeID = 0;
         -- define the Node Id to be copy now
         -- We need to keep update ParentID as well as NodeID while added new node into database based on condition
         DECLARE @ContractServiceLinePaymentTypeIDToHandle BIGINT;

         -- We are selecting the next Node to copy
         SELECT TOP 1 @ContractServiceLinePaymentTypeIDToHandle = CONTRACTSERVICELINEPAYMENTTYPEID
         FROM CONTRACTSERVICELINEPAYMENTTYPES
         WHERE CONTRACTSERVICELINEPAYMENTTYPEID > @LastContractServiceLinePaymentTypeID
               AND (CONTRACTID = @ContractID
                    OR CONTRACTSERVICETYPEID IN
                   (
                       SELECT CONTRACTSERVICETYPEID
                       FROM CONTRACTSERVICETYPES
                       WHERE CONTRACTID = @ContractID
                             AND ISDELETED = 0
                   ))
         ORDER BY CONTRACTSERVICELINEPAYMENTTYPEID;
         DECLARE @Loopcount INT;
         DECLARE @ServiceLineTypeID BIGINT;
         DECLARE @ContractServiceLineID BIGINT;
         DECLARE @ContractServiceTypeID BIGINT;
         DECLARE @PaymentTypeDetailID BIGINT;
         DECLARE @PaymentTypeID BIGINT;
         DECLARE @TempPaymentTypeDetail TABLE(INSERTEDID BIGINT);
         DECLARE @NewPaymentTypeDetailID BIGINT;
         -- As long as we have data we need to keep copy them
         WHILE @ContractServiceLinePaymentTypeIDToHandle IS NOT NULL
             BEGIN
                 -- Copy Items one by one here and ALTER  new entries
                 SELECT @ServiceLineTypeID = SERVICELINETYPEID,
                        @ContractServiceLineID = CONTRACTSERVICELINEID,
                        @ContractServiceTypeID = CONTRACTSERVICETYPEID,
                        @PaymentTypeDetailID = PAYMENTTYPEDETAILID,
                        @PaymentTypeID = PAYMENTTYPEID
                 FROM CONTRACTSERVICELINEPAYMENTTYPES
                 WHERE CONTRACTSERVICELINEPAYMENTTYPEID = @ContractServiceLinePaymentTypeIDToHandle;
                 IF @ServiceLineTypeID IS NOT NULL
                     BEGIN
                         SET @ClaimToolDesc = '';
                         IF @ContractServiceTypeID IS NULL
                            OR @ContractServiceTypeID = 0
                             BEGIN
                                 SELECT @ClaimToolDesc = 'Added claim tool : '+FilterName+FilterValues
                                 FROM GetContractFiltersByID(@ContractID, NULL)
                                 WHERE ServiceLineTypeID = @ServiceLineTypeID;
                             END;
                         ELSE
                             BEGIN
                                 SELECT @ClaimToolDesc = 'Added claim tool : '+FilterName+FilterValues
                                 FROM GetContractFiltersByID(NULL, @ContractServiceTypeID)
                                 WHERE ServiceLineTypeID = @ServiceLineTypeID;
                             END;

                         -- Copy Service Line
                         DECLARE @Tempcontractserviceline TABLE(INSERTEDID BIGINT);
                         IF @ServiceLineTypeID = 8
                             BEGIN
                                 INSERT INTO dbo.ContractServiceLineTableSelection
                                 (INSERTDATE,
                                  UPDATEDATE,
                                  CLAIMFIELDID,
                                  CLAIMFIELDDOCID,
                                  Operator
                                 )
                                 OUTPUT INSERTED.CONTRACTSERVICELINEID
                                        INTO @Tempcontractserviceline
                                        SELECT @CurrentDate,
                                               NULL,
                                               CLAIMFIELDID,
                                               CLAIMFIELDDOCID,
                                               Operator
                                        FROM CONTRACTSERVICELINETABLESELECTION
                                        WHERE CONTRACTSERVICELINEID = @ContractServiceLineID;
                             END;
                         ELSE
                             BEGIN
                                 IF @ServiceLineTypeID = 7
                                     BEGIN
                                         INSERT INTO dbo.ContractServiceLineClaimFieldSelection
                                         (INSERTDATE,
                                          UPDATEDATE,
                                          CLAIMFIELDID,
                                          OPERATORID,
                                          [VALUES]
                                         )
                                         OUTPUT INSERTED.CONTRACTSERVICELINEID
                                                INTO @Tempcontractserviceline
                                                SELECT @CurrentDate,
                                                       NULL,
                                                       CLAIMFIELDID,
                                                       OPERATORID,
                                                       [VALUES]
                                                FROM CONTRACTSERVICELINECLAIMFIELDSELECTION
                                                WHERE CONTRACTSERVICELINEID = @ContractServiceLineID;
                                         -- FIXED-NOV15 Select @ClaimToolDesc and insert audit log should be in one if condition and Move fetching of service type id code inside while loop 

                                     END;
                                 ELSE
                                     BEGIN
                                         INSERT INTO dbo.CONTRACTSERVICELINES
                                         (INSERTDATE,
                                          UPDATEDATE,
                                          INCLUDEDCODE,
                                          EXCLUDEDCODE
                                         )
                                         OUTPUT INSERTED.CONTRACTSERVICELINEID
                                                INTO @Tempcontractserviceline
                                                SELECT @CurrentDate,
                                                       NULL,
                                                       INCLUDEDCODE,
                                                       EXCLUDEDCODE
                                                FROM CONTRACTSERVICELINES
                                                WHERE CONTRACTSERVICELINEID = @ContractServiceLineID;
                                     END;
                             END;
                         SELECT TOP 1 @ContractServiceLineID = INSERTEDID
                         FROM @Tempcontractserviceline;
                     END;
                 IF @PaymentTypeID IS NOT NULL
                     BEGIN
                         SET @ClaimToolDesc = '';
                         IF @ContractServiceTypeID IS NULL
                            OR @ContractServiceTypeID = 0
                             BEGIN
                                 SELECT @ClaimToolDesc = 'Added Payment tool : '+FilterName+FilterValues
                                 FROM GetContractFiltersByID(@ContractID, NULL)
                                 WHERE PaymentTypeId = @PaymentTypeID;
                             END;
                         ELSE
                             BEGIN
                                 SELECT @ClaimToolDesc = 'Added Payment tool : '+FilterName+FilterValues
                                 FROM GetContractFiltersByID(NULL, @ContractServiceTypeID)
                                 WHERE PaymentTypeId = @PaymentTypeID;
                             END;
                         -- Insert into PaymentTypeASCFeeSchedules When @PaymentTypeID 1
                         IF @PaymentTypeID = 1
                             BEGIN
                                 INSERT INTO dbo.PaymentTypeASCFeeSchedules
                                 (INSERTDATE,
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
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
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
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;


                         -- Insert into [PaymentTypeDRGPayment] When @PaymentTypeID = 2
                         IF @PaymentTypeID = 2
                             BEGIN
                                 INSERT INTO dbo.PaymentTypeDRGPayment
                                 (INSERTDATE,
                                  UPDATEDATE,
                                  BASERATE,
                                  CLAIMFIELDDOCID
                                 )
                                 OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               BASERATE,
                                               CLAIMFIELDDOCID
                                        FROM PAYMENTTYPEDRGPAYMENT
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;
                         -- Insert into [[PaymentTypeFeeSchedules]] When @PaymentTypeID = 3
                         IF @PaymentTypeID = 3
                             BEGIN
                                 INSERT INTO dbo.PaymentTypeFeeSchedules
                                 (INSERTDATE,
                                  UPDATEDATE,
                                  FEESCHEDULE,
                                  NONFEESCHEDULE,
                                  CLAIMFIELDDOCID,
                                  IsObserveUnits
                                 )
                                 OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               FEESCHEDULE,
                                               NONFEESCHEDULE,
                                               CLAIMFIELDDOCID,
                                               IsObserveUnits
                                        FROM PAYMENTTYPEFEESCHEDULES
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;
                         -- Insert into [PaymentTypeMedicareIPPayment] When @PaymentTypeID = 4
                         IF @PaymentTypeID = 4
                             BEGIN
                                 INSERT INTO dbo.PAYMENTTYPEMEDICAREIPPAYMENT
                                 (INSERTDATE,
                                  UPDATEDATE,
                                  INPATIENT,
                                  FORMULA
                                 )
                                 OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               INPATIENT,
                                               FORMULA
                                        FROM PAYMENTTYPEMEDICAREIPPAYMENT
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;
                    
                         -- Insert into [PaymentTypeMedicareOPPayment] When @PaymentTypeID = 5
                         IF @PaymentTypeID = 5
                             BEGIN
                                 INSERT INTO dbo.PAYMENTTYPEMEDICAREOPPAYMENT
                                 (INSERTDATE,
                                  UPDATEDATE,
                                  OUTPATIENT
                                 )
                                 OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               OUTPATIENT
                                        FROM PAYMENTTYPEMEDICAREOPPAYMENT
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;
                    
                         -- Insert into [PaymentTypeMedicareLabFeeSchedule] When @PaymentTypeID = 13
                         IF @PaymentTypeID = 13
                             BEGIN
                                 INSERT INTO dbo.PAYMENTTYPEMEDICARELABFEESCHEDULE
                                 (INSERTDATE,
                                  UPDATEDATE,
                                  PERCENTAGE
                                 )
                                 OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               PERCENTAGE
                                        FROM PAYMENTTYPEMEDICARELABFEESCHEDULE
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;
                    
                         -- Insert into [PaymentTypePerCase] When @PaymentTypeID = 6
                         IF @PaymentTypeID = 6
                             BEGIN
                                 INSERT INTO dbo.PAYMENTTYPEPERCASE
                                 (INSERTDATE,
                                  UPDATEDATE,
                                  RATE,
                                  MAXCASESPERDAY
                                 )
                                 OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               RATE,
                                               MAXCASESPERDAY
                                        FROM PAYMENTTYPEPERCASE
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;
                         -- Insert into [PaymentTypePerDiem] When @PaymentTypeID = 7
                         IF @PaymentTypeID = 7
                             BEGIN
                                 INSERT INTO dbo.PAYMENTTYPEPERDIEM
                                 (INSERTDATE,
                                  UPDATEDATE,
                                  RATE,
                                  DAYSFROM,
                                  DAYSTO
                                 )
                                 OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               RATE,
                                               DAYSFROM,
                                               DAYSTO
                                        FROM PAYMENTTYPEPERDIEM
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END; 
                         -- Insert into [PaymentTypePercentageDiscountPayment] When @PaymentTypeID = 8
                         IF @PaymentTypeID = 8
                             BEGIN
                                 INSERT INTO dbo.PaymentTypePercentageDiscountPayment
                                 (INSERTDATE,
                                  UPDATEDATE,
                                  PERCENTAGE
                                 )
                                 OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               PERCENTAGE
                                        FROM PAYMENTTYPEPERCENTAGEDISCOUNTPAYMENT
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END; 
                    
               
                         -- Insert into [PaymentTypePerVisit] When @PaymentTypeID = 9
                         IF @PaymentTypeID = 9
                             BEGIN
                                 INSERT INTO dbo.PAYMENTTYPEPERVISIT
                                 (INSERTDATE,
                                  UPDATEDATE,
                                  RATE
                                 )
                                 OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               RATE
                                        FROM PAYMENTTYPEPERVISIT
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;


                         -- Insert into [PaymentTypeCustomtable] When @PaymentTypeID = 14
                         IF @PaymentTypeID = 14
                             BEGIN
                                 INSERT INTO dbo.PAYMENTTYPECUSTOMTABLE
                                 (INSERTDATE,
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
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
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
                                        FROM PAYMENTTYPECUSTOMTABLE
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;

                         -- Insert into [PaymentTypeStopLoss] When @PaymentTypeID = 10
                         IF @PaymentTypeID = 10
                             BEGIN
                                 INSERT INTO dbo.PAYMENTTYPESTOPLOSS
                                 (INSERTDATE,
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
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               PERCENTAGE,
                                               THRESHOLD,
                                               DAYS,
                                               REVCODE,
                                               CPTCODE,
                                               STOPLOSSCONDITIONID,
                                               ISEXCESSCHARGE
                                        FROM PAYMENTTYPESTOPLOSS
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;
                       
                         -- Insert into PaymentTypeLesserOf When @PaymentTypeID = 11
                         IF @PaymentTypeID = 11
                             BEGIN
                                 INSERT INTO dbo.PaymentTypeLesserOf
                                 (InsertDate,
                                  UpdateDate,
                                  Percentage,
                                  IsLesserOf
                                 )
                                 OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               Percentage,
                                               IsLesserOf
                                        FROM dbo.PaymentTypeLesserOf
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;
					               
                         -- Insert into PaymentTypeCap When @PaymentTypeID = 12
                         IF @PaymentTypeID = 12
                             BEGIN
                                 INSERT INTO dbo.PAYMENTTYPECAP
                                 (INSERTDATE,
                                  UPDATEDATE,
                                  THRESHOLD,
                                  PERCENTAGE
                                 )
                                 OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               THRESHOLD,
                                               PERCENTAGE
                                        FROM dbo.PAYMENTTYPECAP
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;

                         -- Insert into [PaymentTypeMedicareSequester] When @PaymentTypeID = 15
                         IF @PaymentTypeID = 15
                             BEGIN
                                 INSERT INTO dbo.PaymentTypeMedicareSequester
                                 (INSERTDATE,
                                  UPDATEDATE,
                                  PERCENTAGE
                                 )
                                 OUTPUT INSERTED.PAYMENTTYPEDETAILID
                                        INTO @TempPaymentTypeDetail
                                        SELECT @CurrentDate,
                                               NULL,
                                               PERCENTAGE
                                        FROM dbo.PaymentTypeMedicareSequester
                                        WHERE PAYMENTTYPEDETAILID = @PaymentTypeDetailID;
                             END;
                     END;
                 SELECT @NewPaymentTypeDetailID = INSERTEDID
                 FROM @TempPaymentTypeDetail;


                 ---Insert Audit Log Information

                 IF @ContractServiceTypeID IS NULL
                    OR @ContractServiceTypeID = 0
                     BEGIN
                         EXEC InsertAuditLog
                              @UserName,
                              'Modify',
                              'Contract - Modelling',
                              @ClaimToolDesc,
                              @DuplicateContractID,
                              1;
                     END;
                 ELSE
                     BEGIN
                         SELECT TOP 1 @LatestInsertedContractServiceTypeID = NEWCONTRACTSERVICETYPEID
                         FROM @Temp_ContractServiceType
                         WHERE OLDCONTRACTSERVICETYPEID = @ContractServiceTypeID;
                         EXEC InsertAuditLog
                              @UserName,
                              'Modify',
                              'Service Type',
                              @ClaimToolDesc,
                              @LatestInsertedContractServiceTypeID,
                              0;
                     END;






                 -- Insert into [ContractServiceLinePaymentTypes]
                 INSERT INTO dbo.CONTRACTSERVICELINEPAYMENTTYPES
                 (PAYMENTTYPEDETAILID,
                  INSERTDATE,
                  UPDATEDATE,
                  PAYMENTTYPEID,
                  CONTRACTSERVICELINEID,
                  CONTRACTID,
                  SERVICELINETYPEID,
                  CONTRACTSERVICETYPEID
                 )
                        SELECT @NewPaymentTypeDetailID,
                               @CurrentDate,
                               NULL,
                               PAYMENTTYPEID,
                               @ContractServiceLineID,
                               CASE
                                   WHEN CONTRACTID IS NULL
                                   THEN NULL
                                   ELSE @DuplicateContractID
                               END,
                               SERVICELINETYPEID,
                        (
                            SELECT TOP 1 NEWCONTRACTSERVICETYPEID
                            FROM @Temp_ContractServiceType
                            WHERE OLDCONTRACTSERVICETYPEID = @ContractServiceTypeID
                        )
                        FROM CONTRACTSERVICELINEPAYMENTTYPES
                        WHERE CONTRACTSERVICELINEPAYMENTTYPEID = @ContractServiceLinePaymentTypeIDToHandle;
                 SET @NewPaymentTypeDetailID = NULL;
                 SET @ServiceLineTypeID = NULL;
                 SET @ContractServiceLineID = NULL;
                 SET @ContractServiceTypeID = NULL;
                 SET @PaymentTypeDetailID = NULL;
                 SET @PaymentTypeID = NULL;
                 DELETE FROM @TempContractServiceTypes;
                 DELETE FROM @TempPaymentTypeDetail;
                 DELETE FROM @Tempcontractserviceline;
                 -- set the last Node handled to the one we just copyed
                 SET @LastContractServiceLinePaymentTypeID = @ContractServiceLinePaymentTypeIDToHandle;
                 SET @ContractServiceLinePaymentTypeIDToHandle = NULL;

                 -- select the next Node to copy
                 SELECT TOP 1 @ContractServiceLinePaymentTypeIDToHandle = CONTRACTSERVICELINEPAYMENTTYPEID
                 FROM CONTRACTSERVICELINEPAYMENTTYPES
                 WHERE CONTRACTSERVICELINEPAYMENTTYPEID > @LastContractServiceLinePaymentTypeID
                       AND (CONTRACTID = @ContractID
                            OR CONTRACTSERVICETYPEID IN
                           (
                               SELECT CONTRACTSERVICETYPEID
                               FROM CONTRACTSERVICETYPES
                               WHERE CONTRACTID = @ContractID
                                     AND ISDELETED = 0
                           ))
                 ORDER BY CONTRACTSERVICELINEPAYMENTTYPEID;
             END;

         -- Copy ServiceType and PaymentType Filter Codes
         INSERT INTO ServiceTypePaymentTypeFilterCodes
                SELECT NST.NEWCONTRACTSERVICETYPEID,
                       @CurrentDate,
                       NULL,
                       NULL,
                       FC.serviceTypecodes,
                       FC.Paymenttypecodes
                FROM ServiceTypePaymentTypeFilterCodes FC
                     JOIN @Temp_ContractServiceType NST ON NST.OLDCONTRACTSERVICETYPEID = FC.CONTRACTSERVICETYPEID
                UNION
                SELECT NULL,
                       @CurrentDate,
                       NULL,
                       @DuplicateContractID,
                       FC.serviceTypecodes,
                       FC.Paymenttypecodes
                FROM ServiceTypePaymentTypeFilterCodes FC
                WHERE contractid = @ContractID;
         SELECT @DuplicateContractID;  --returning ContractId  
     END;