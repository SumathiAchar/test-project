
CREATE PROCEDURE dbo.AddEditMedicareLabFeeSchedulePayment(
@Paymenttypedetailid BIGINT, 
@Percentage FLOAT, 
@Paymenttypeid BIGINT, 
@Contractid BIGINT, 
@Contractservicetypeid BIGINT,
@UserName VARCHAR(50))
AS

/****************************************************************************
 *   Name         : AddEditMedicareLabFeeSchedulePayment
 *   Author       : Raj
 *   Date         : 17/Sep/2014
 *   Module       : Add/Edit PaymentTypeMedicareLabFeeSchedulePayment
 *   Description  : Insert and Update Payment Type MedicareLabFeeSchedule Information into database
 *****************************************************************************/

BEGIN
	SET NOCOUNT ON;

	DECLARE
	   @Currentdate DATETIME = GETUTCDATE(),
	   @Action   VARCHAR(10),
	   @PaymentToolDesc NVARCHAR(1000);

	DECLARE
	   @Transactionname VARCHAR(100) = 'AddEditMedicareLabFeeSchedulePayment';
	BEGIN TRY
		BEGIN TRAN @Transactionname;   
		------------------------------Adding MedicareLabFeeSchedule Payment Type----------------------------------  
		IF ISNULL(@Paymenttypedetailid, 0) = 0   
			BEGIN
				SET @Action = 'Add';
				--Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED
				DECLARE
				   @Tmptable TABLE(
								   InsertedID BIGINT);
				--Insert PaymentTypeMedicareLabFeeSchedulePayment informations
				INSERT INTO dbo.PaymentTypeMedicareLabFeeSchedule(
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

				 --------------Update Contract GUID--------------------
				EXEC [UpdateContractGUID]
					   @Contractid,
					   @Contractservicetypeid;

				--Return Inserted PaymentTypeDetailID
				SELECT
					   @Paymenttypedetailid;
			END;
		ELSE
			BEGIN
				SET @Action = 'Modify';

				-------Check parcentage, Include modifier column values changed or not---------

				IF NOT EXISTS
				(
				    SELECT 1
				    FROM dbo.PaymentTypeMedicareLabFeeSchedule MLFSP JOIN ContractServiceLinePaymentTypes CSL 
				    ON MLFSP.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				  WHERE
						ISNULL(CSL.ContractID, 0) = @Contractid
					AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
					AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid
						AND ((@Percentage IS NULL
							 AND Percentage IS NULL)
							OR (Percentage = @Percentage))
				)
				    BEGIN
					   --------------Update Contract GUID--------------------
					   EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;
				    END;



				------------------Updating MedicareLabFeeSchedule PaymentType-------------------------
				UPDATE dbo.PaymentTypeMedicareLabFeeSchedule
				SET
					UpdateDate = @Currentdate, 
					Percentage = @Percentage
				  FROM dbo.PaymentTypeMedicareLabFeeSchedule MLFSP JOIN ContractServiceLinePaymentTypes CSL ON MLFSP.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				  WHERE
						ISNULL(CSL.ContractID, 0) = @Contractid
					AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
					AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid;

				SELECT
					   @Paymenttypedetailid;
			END;
		
		/********************** Used for Contract Modelling report *************************/

		EXEC AddEditPaymentTypeFilterCodes @Contractservicetypeid, @Contractid;
		
		/********************** Used for Contract Modelling report *************************/

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