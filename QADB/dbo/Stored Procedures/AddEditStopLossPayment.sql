CREATE PROCEDURE dbo.AddEditStopLossPayment(
       @Paymenttypedetailid   BIGINT,
       @Percentage            FLOAT,
       @Threshold             VARCHAR(5000),
       @Paymenttypeid         BIGINT,
       @Contractid            BIGINT,
       @Contractservicetypeid BIGINT,
       @Days                  VARCHAR(200),
       @Revcode               VARCHAR(2000),
       @Cptcode               VARCHAR(2000),
       @Stoplossconditionid   INT,
       @Isexcesscharge        BIT,
       @UserName              VARCHAR(50))
AS

/****************************************************************************
 *   Name         : AddEditStopLossPayment
 *   Author       : mmachina
 *   Date         : 15/Aug/2013
 *   Module       : Add/Edit PaymentType StopLoss
 *   Description  : Insert/Update Payment Type StopLoss Information into database
 *****************************************************************************/

    BEGIN
        SET NOCOUNT ON;
        DECLARE @Currentdate DATETIME = GETUTCDATE(),
			 @Transactionname VARCHAR(100) = 'AddEditStopLossPayment',
                @ClaimToolDesc   NVARCHAR(MAX);
        DECLARE @GetContractFilters TABLE( FilterValues      VARCHAR(MAX),
                                           FilterName        VARCHAR(100),
                                           ServiceLineTypeId BIGINT,
                                           PaymentTypeId     BIGINT );
        BEGIN TRY
            BEGIN TRAN @Transactionname;   

            ------------------------------Adding StopLoss Payment Type----------------------------------  
            IF ISNULL(@Paymenttypedetailid, 0) = 0
                BEGIN
                    --Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED
                    DECLARE @Tmptable TABLE( InsertedID BIGINT );
                    --Insert PaymentTypeStopLoss informations
                    INSERT INTO dbo.PaymentTypeStopLoss
                           ( InsertDate,
                             UpdateDate,
                             Percentage,
                             Threshold,
                             Days,
                             RevCode,
                             CPTCode,
                             StopLossConditionID,
                             IsExcessCharge
                           )
                    OUTPUT INSERTED.PaymentTypeDetailID
                           INTO @Tmptable -- inserting id into @TmpTable
                    VALUES( @Currentdate, NULL, @Percentage, @Threshold, @Days, @Revcode, @Cptcode, @Stoplossconditionid, @Isexcesscharge );
	
                    --Get inserted PaymentTypeDetailID and update variable
                    SELECT @Paymenttypedetailid = InsertedID
                    FROM @Tmptable;

                    --Insert data into [ContractServiceLinePaymentTypes] Tables
                    INSERT INTO dbo.ContractServiceLinePaymentTypes
                           ( PaymentTypeDetailID,
                             InsertDate,
                             UpdateDate,
                             PaymentTypeID,
                             ContractServiceLineID,
                             ContractID,
                             ServiceLineTypeID,
                             ContractServiceTypeID
                           )
                    VALUES( @Paymenttypedetailid, @Currentdate, NULL, @Paymenttypeid, NULL, @Contractid, NULL, @Contractservicetypeid );
		
                    --Return Inserted PaymentTypeDetailID
                    SELECT @Paymenttypedetailid;
                    INSERT INTO @GetContractFilters
                           SELECT FilterValues,
                                  FilterName,
                                  ServiceLineTypeId,
                                  PaymentTypeId
                           FROM GetContractFiltersByID( @ContractId, @ContractServiceTypeId );
                    SELECT @ClaimToolDesc = 'Added Payment Tool : ' + FilterName + '= ' + FilterValues
                    FROM GetContractFiltersByID( @ContractId, @ContractServiceTypeId )
                    WHERE PaymentTypeId = @PaymentTypeID;

				------------------------Updating Contract GUID-----------------------------
				 EXEC [UpdateContractGUID]
						   @ContractID,
						   @ContractServiceTypeID;

                END;
            ELSE
                BEGIN

			 IF NOT EXISTS
				(
				    SELECT 1
				    FROM dbo.PaymentTypeStopLoss PSL
				    JOIN ContractServiceLinePaymentTypes CSL ON PSL.PaymentTypeDetailId = CSL.PaymentTypeDetailId
                    WHERE ISNULL(CSL.ContractID, 0) = @Contractid
                      AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
                      AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid
				  AND Percentage = @Percentage
				  AND Threshold = @Threshold
				  AND ((@Days IS NULL AND [Days] IS NULL)
						OR ([Days] = @Days))
				  AND ((@Revcode IS NULL AND RevCode IS NULL)
						OR (RevCode = @Revcode))
				  AND ((@Cptcode IS NULL AND CPTCode IS NULL)
						OR (CPTCode = @Cptcode))
				  AND StopLossConditionID = @Stoplossconditionid
				  AND IsExcessCharge = @Isexcesscharge
				)
				    BEGIN
					   EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;
				    END;

                    ----------------Updating StopLoss PaymentType--------------------------------
                    UPDATE dbo.PaymentTypeStopLoss
                      SET UpdateDate = @Currentdate,
                          Percentage = @Percentage,
                          Threshold = @Threshold,
                          Days = @Days,
                          RevCode = @Revcode,
                          CPTCode = @Cptcode,
                          StopLossConditionID = @Stoplossconditionid,
                          IsExcessCharge = @Isexcesscharge
                    FROM dbo.PaymentTypeStopLoss PSL
                         JOIN ContractServiceLinePaymentTypes CSL ON PSL.PaymentTypeDetailId = CSL.PaymentTypeDetailId
                    WHERE ISNULL(CSL.ContractID, 0) = @Contractid
                      AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
                      AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid;
                    SELECT @Paymenttypedetailid;
                    SELECT @ClaimToolDesc = FilterName + '=' + FilterValues
                    FROM GetContractFiltersByID( @ContractId, @ContractServiceTypeId )
                    WHERE PaymentTypeId = @PaymentTypeID;
                END;
			
            /********************** Insert Audit Log Information*************************/

            EXEC AddEditPaymentTypeFilterCodes
                 @Contractservicetypeid,
                 @Contractid;

		  IF @Contractid IS NULL OR @Contractid =0
		  BEGIN
		  EXEC InsertAuditLog
                 @UserName,
                 'Modify',
                 'Service Type',
                 @ClaimToolDesc,
                 @ContractServiceTypeId,
                 0

		  END

		  ELSE
		  
		  BEGIN
	      
            EXEC InsertAuditLog
                 @UserName,
                 'Modify',
                 'Contract - Modelling',
                 @ClaimToolDesc,
                 @Contractid,
                 1;
		  END
            --Check Any Transation happened than commit transation
            COMMIT TRANSACTION @Transactionname;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;