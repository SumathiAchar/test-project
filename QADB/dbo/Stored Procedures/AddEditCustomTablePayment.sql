CREATE PROCEDURE [dbo].[AddEditCustomTablePayment](@Paymenttypedetailid     BIGINT,
                                                  @Expression              VARCHAR(5000),
                                                  @DocumentId              BIGINT,
                                                  @ClaimFieldId            BIGINT,
                                                  @Paymenttypeid           BIGINT,
                                                  @Contractid              BIGINT,
                                                  @Contractservicetypeid   BIGINT,
                                                  @UserName                VARCHAR(50),
                                                  @MultiplierFirst         VARCHAR(5000),
                                                  @MultiplierSecond        VARCHAR(5000),
                                                  @MultiplierThird         VARCHAR(5000),
                                                  @MultiplierFour          VARCHAR(5000),
                                                  @MultiplierOther         VARCHAR(5000),
                                                  @IsObserveServiceUnit    BIT,
                                                  @ObserveServiceUnitLimit VARCHAR(5000),
                                                  @IsPerDayOfStay          BIT,
                                                  @IsPerCode               BIT)
AS
     BEGIN
         SET NOCOUNT ON;
         DECLARE @Currentdate DATETIME= GETUTCDATE(), @Action VARCHAR(10), @PaymentToolDesc NVARCHAR(MAX), @Transactionname VARCHAR(100)= 'AddEditCustomTablePayment';
         BEGIN TRY
             BEGIN TRAN @Transactionname;   
             ------------------------------Adding custom table Payment Type----------------------------------
             IF ISNULL(@Paymenttypedetailid, 0) = 0
                 BEGIN
                     SET @Action = 'Add';  
                     --Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED
                     DECLARE @Tmptable TABLE(INSERTEDID BIGINT);
                     --Insert PaymentTypeCustomTable informations
                     INSERT INTO dbo.PaymentTypeCustomTable
                     (InsertDate,
                      UpdateDate,
                      Formula,
                      ClaimFieldId,
                      ClaimFieldDocId,
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
                     VALUES
                     (@Currentdate,
                      NULL,
                      @Expression,
                      @ClaimFieldId,
                      @DocumentId,
                      @MultiplierFirst,
                      @MultiplierSecond,
                      @MultiplierThird,
                      @MultiplierFour,
                      @MultiplierOther,
                      @IsObserveServiceUnit,
                      @ObserveServiceUnitLimit,
                      @IsPerDayOfStay,
                      @IsPerCode
                     );
     
                     --Get inserted PaymentTypeDetailID and update variable
                     SELECT @Paymenttypedetailid = SCOPE_IDENTITY();

                     --Insert data into [ContractServiceLinePaymentTypes] Tables
                     INSERT INTO dbo.ContractServiceLinePaymentTypes
                     (PAYMENTTYPEDETAILID,
                      INSERTDATE,
                      UPDATEDATE,
                      PAYMENTTYPEID,
                      CONTRACTSERVICELINEID,
                      CONTRACTID,
                      SERVICELINETYPEID,
                      CONTRACTSERVICETYPEID
                     )
                     VALUES
                     (@Paymenttypedetailid,
                      @Currentdate,
                      NULL,
                      @Paymenttypeid,
                      NULL,
                      @Contractid,
                      NULL,
                      @Contractservicetypeid
                     );
                     --Return Inserted PaymentTypeDetailID
                     SELECT @Paymenttypedetailid;

                     ------------------------Updating Contract GUID-----------------------------
                     EXEC [UpdateContractGUID]
                          @ContractID,
                          @ContractServiceTypeID;
                 END;
             ELSE
                 BEGIN
                     ---------------------Updating CustomTable PaymentType-----------------------------
                     SET @Action = 'Modify';
                     --Checks if the Payment Type Values are changed and then update the contract GUID if not changed
                     IF NOT EXISTS
                     (
                         SELECT 1
                         FROM dbo.PaymentTypeCustomTable PTAF
                              JOIN CONTRACTSERVICELINEPAYMENTTYPES CSL ON PTAF.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                         WHERE ISNULL(CSL.CONTRACTID, 0) = @Contractid
                               AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @Contractservicetypeid
                               AND ISNULL(CSL.PAYMENTTYPEID, 0) = @Paymenttypeid
                               AND Formula = @Expression
                               AND ClaimFieldDocID = @DocumentID
                               AND ClaimFieldID = @ClaimFieldID
                               AND ((@MultiplierFirst IS NULL
                                     AND MultiplierFirst IS NULL)
                                    OR (MultiplierFirst = @MultiplierFirst))
                               AND ((@MultiplierSecond IS NULL
                                     AND MultiplierSecond IS NULL)
                                    OR (MultiplierSecond = @MultiplierSecond))
                               AND ((@MultiplierThird IS NULL
                                     AND MultiplierThird IS NULL)
                                    OR (MultiplierThird = @MultiplierThird))
                               AND ((@MultiplierFour IS NULL
                                     AND MultiplierFour IS NULL)
                                    OR (MultiplierFour = @MultiplierFour))
                               AND ((@MultiplierOther IS NULL
                                     AND MultiplierOther IS NULL)
                                    OR (MultiplierOther = @MultiplierOther))
                               AND ((@IsObserveServiceUnit IS NULL
                                     AND IsObserveServiceUnit IS NULL)
                                    OR (IsObserveServiceUnit = @IsObserveServiceUnit))
                               AND ((@ObserveServiceUnitLimit IS NULL
                                     AND ObserveServiceUnitLimit IS NULL)
                                    OR (ObserveServiceUnitLimit = @ObserveServiceUnitLimit))
                               AND ((@IsPerDayOfStay IS NULL
                                     AND IsPerDayOfStay IS NULL)
                                    OR (IsPerDayOfStay = @IsPerDayOfStay))
                               AND ((@IsPerCode IS NULL
                                     AND IsPerCode IS NULL)
                                    OR (IsPerCode = @IsPerCode))
                     )
                         BEGIN
                             --Updating Contract GUID
                             EXEC [UpdateContractGUID]
                                  @ContractID,
                                  @ContractServiceTypeID;
                         END;
                     UPDATE dbo.PaymentTypeCustomTable
                       SET
                           UPDATEDATE = @Currentdate,
                           Formula = @Expression,
                           CLAIMFIELDDOCID = @DocumentId,
                           ClaimFieldId = @ClaimFieldId,
                           MultiplierFirst = @MultiplierFirst,
                           MultiplierSecond = @MultiplierSecond,
                           MultiplierThird = @MultiplierThird,
                           MultiplierFour = @MultiplierFour,
                           MultiplierOther = @MultiplierOther,
                           IsObserveServiceUnit = @IsObserveServiceUnit,
                           ObserveServiceUnitLimit = @ObserveServiceUnitLimit,
                           IsPerDayOfStay = @IsPerDayOfStay,
                           IsPerCode = @IsPerCode
                     FROM dbo.PaymentTypeCustomTable PTAF
                          JOIN CONTRACTSERVICELINEPAYMENTTYPES CSL ON PTAF.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                     WHERE ISNULL(CSL.CONTRACTID, 0) = @Contractid
                           AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @Contractservicetypeid
                           AND ISNULL(CSL.PAYMENTTYPEID, 0) = @Paymenttypeid;
                     SELECT @Paymenttypedetailid;
                 END;
			
             /********************** Used for Contract Modelling report *************************/

             EXEC AddEditPaymentTypeFilterCodes
                  @Contractservicetypeid,
                  @Contractid;
			
		
             /********************** Insert Audit log *************************/

             SELECT @PaymentToolDesc = FilterName + FilterValues
             FROM dbo.GetContractFiltersByID(@Contractid, @Contractservicetypeid)
             WHERE PaymentTypeId = @PaymentTypeId;
             IF(@Action = 'Add')
                 BEGIN
                     SET @PaymentToolDesc = 'Added Payment tool : '+@PaymentToolDesc;
                 END;
             EXEC InsertAuditLog
                  @UserName,
                  'Modify',
                  'Service Type',
                  @PaymentToolDesc,
                  @ContractServiceTypeID,
                  0;

             --Check Any Transaction happened than commit transaction
             COMMIT TRANSACTION @Transactionname;
         END TRY
         BEGIN CATCH
             ROLLBACK TRAN @TransactionName;
             EXEC RaiseErrorInformation;
         END CATCH;
     END;