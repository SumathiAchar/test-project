CREATE PROCEDURE dbo.AddEditPercentageDiscountPayment(
       @Paymenttypedetailid   BIGINT,
       @Percentage            FLOAT,
       @Paymenttypeid         BIGINT,
       @Contractid            BIGINT,
       @Contractservicetypeid BIGINT,
       @UserName              VARCHAR(50))
AS  

/****************************************************************************  
 *   Name         : AddEditPercentageDiscountPayment  
 *   Author       : mmachina  
 *   Date         : 15/Aug/2013  
 *   Module       : AddEditPercentageDiscountPayment  
 *   Description  : Insert and update payment type per discount into database  
 *****************************************************************************/

    BEGIN
	SET NOCOUNT ON;
	DECLARE
	   @Currentdate DATETIME = GETUTCDATE(),
	   @Action      VARCHAR(10),
	   @PaymentToolDesc NVARCHAR(1000),
	   @Transactionname VARCHAR(100) = 'AddEditPercentageDiscountPayment';
	BEGIN TRY
		BEGIN TRAN @Transactionname;  
		------------------------------Adding PercentageDiscount Payment Type----------------------------------  
		IF ISNULL(@Paymenttypedetailid, 0) = 0   
			BEGIN
			    SET @Action = 'Add';
				--Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED  
				DECLARE
				   @Tmptable TABLE(
								   InsertedID BIGINT);  
				--Insert PaymentTypePercDiscountPayment informations  
				INSERT INTO dbo.PaymentTypePercentageDiscountPayment(
								 InsertDate, 
								 UpdateDate, 
								 Percentage)
				OUTPUT
					   INSERTED.PaymentTypeDetailID
					   INTO @Tmptable -- inserting id into @TmpTable  
				VALUES(@Currentdate, 
					   NULL, 
					   @Percentage);  
   
				--Get inserted PaymentTypeDetailID and update variable  
				SELECT
					   @Paymenttypedetailid = InsertedID
				  FROM @Tmptable;  
  
				--Insert data into [ContractServiceLinePaymentTypes] Tables  
				INSERT INTO dbo.ContractServiceLinePaymentTypes(
								 PaymentTypeDetailID, 
								 InsertDate, 
								 UpdateDate, 
								 PaymentTypeID, 
								 ContractServiceLineID, 
								 ContractID, 
								 ServiceLineTypeID, 
								 ContractServiceTypeID)
				VALUES(@Paymenttypedetailid, 
					   @Currentdate, 
					   NULL, 
					   @Paymenttypeid, 
					   NULL, 
					   @Contractid, 
					   NULL, 
					   @Contractservicetypeid);  
       
				--------------Update Contract GUID---------------------
				EXEC UpdateContractGUID
					@Contractid,
					@ContractServiceTypeId;


				--Return Inserted PaymentTypeDetailID  
				SELECT
					   @Paymenttypedetailid;
			END;
		ELSE
			------------------------------Updating PercentageDiscount Payment Type----------------------------------  
			BEGIN
				SET @Action = 'Modify';


				-----Checking Rate is changed or not-------------------------
				IF NOT EXISTS
				(
				    SELECT 1
				    FROM dbo.PaymentTypePercentageDiscountPayment PDP
					    JOIN ContractServiceLinePaymentTypes CSL ON PDP.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				    WHERE ISNULL(CSL.ContractID, 0) = @Contractid
						AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
						AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid
						AND Percentage = @Percentage
				)
				    BEGIN
					   --------------Update Contract GUID---------------------
					   EXEC UpdateContractGUID
						   @Contractid,
						   @ContractServiceTypeId;
				    END;


				UPDATE dbo.PaymentTypePercentageDiscountPayment
				SET
					UpdateDate = @Currentdate, 
					Percentage = @Percentage
				  FROM dbo.PaymentTypePercentageDiscountPayment PDP JOIN ContractServiceLinePaymentTypes CSL ON PDP.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				  WHERE
						ISNULL(CSL.ContractID, 0) = @Contractid
					AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
					AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid;

				SELECT
					   @Paymenttypedetailid;
			END;
		
		/********************** Used for Contract Modelling report *************************/

		EXEC AddEditPaymentTypeFilterCodes @Contractservicetypeid, @Contractid;
		
		/********************** Insert Audit log *************************/
		SELECT @PaymentToolDesc =FilterName + FilterValues FROM dbo.GetContractFiltersByID( @Contractid, @Contractservicetypeid )
			   WHERE PaymentTypeId = @Paymenttypeid;
            IF( @Action = 'Add' )
                BEGIN
                    SET @PaymentToolDesc = 'Added Payment tool:' + @PaymentToolDesc;
                END;

            EXEC InsertAuditLog
                 @UserName,
                 'Modify',
                 'Service Type',
                 @PaymentToolDesc,
                 @ContractServiceTypeID,
                 0;

		--Check Any Transation happened than commit transation
		COMMIT TRANSACTION @Transactionname;

	END TRY
	BEGIN CATCH
		ROLLBACK TRAN @TransactionName;
		EXEC RaiseErrorInformation
	END CATCH;
END;