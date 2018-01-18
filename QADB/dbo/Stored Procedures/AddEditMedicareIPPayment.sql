CREATE PROCEDURE dbo.AddEditMedicareIPPayment(
@PaymentTypeDetailId BIGINT, 
@InPatient FLOAT,
@Formula VARCHAR(500),
@PaymentTypeId BIGINT, 
@ContractId BIGINT, 
@ContractServiceTypeId BIGINT,
@UserName VARCHAR(50))
AS

/****************************************************************************
 *   Name         : AddEditMedicareIPPayment
 *   Author       : mmachina
 *   Date         : 15/Aug/2013
 *   Module       : Add/Edit PaymentTypeMedicareIPPayment
 *   Description  : Insert and Update Payment Type MedicareIP Information into database
 *****************************************************************************/

BEGIN
	SET NOCOUNT ON;
	DECLARE @CurrentDate DATETIME = GETUTCDATE(),
		   @Action      VARCHAR(10),
		   @PaymentToolDesc NVARCHAR(1000),
		   @TransactionName VARCHAR(100) = 'AddEditMedicareIPPayment';
	BEGIN TRY
		BEGIN TRAN @TransactionName;   
		------------------------------Adding MedicareIP Payment Type----------------------------------  
		IF ISNULL(@PaymentTypeDetailId, 0) = 0   
			BEGIN
				SET @Action = 'Add';  
				--Insert PaymentTypeMedicareIPPayment informations
				INSERT INTO dbo.PaymentTypeMedicareIPPayment(
								 InsertDate, 
								 UpdateDate, 
								 InPatient,
								 Formula)
				VALUES(@CurrentDate, 
					   NULL, 
					   @InPatient,
					   @Formula);
	
				--Get inserted PaymentTypeDetailID and update variable
				SELECT
					   @PaymentTypeDetailId = SCOPE_IDENTITY()

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
				VALUES(@PaymentTypeDetailId, 
					   @CurrentDate, 
					   NULL, 
					   @PaymentTypeId, 
					   NULL, 
					   @ContractId, 
					   NULL, 
					   @ContractServiceTypeId);

			 --------------Update Contract GUID--------------------
				EXEC [UpdateContractGUID]
					   @Contractid,
					   @Contractservicetypeid;
			END;
		ELSE
			BEGIN
				------------------Updating MedicareIP PaymentType-------------------------
				SET @Action = 'Modify';


				IF NOT EXISTS
				(
				    SELECT 1
				    FROM dbo.PaymentTypeMedicareIPPayment MIPP
					    JOIN ContractServiceLinePaymentTypes CSL ON MIPP.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				    WHERE ISNULL(CSL.ContractID, 0) = @ContractId
						AND ISNULL(CSL.ContractServiceTypeID, 0) = @ContractServiceTypeId
						AND ISNULL(CSL.PaymentTypeID, 0) = @PaymentTypeId
						AND ((@InPatient IS NULL
							 AND InPatient IS NULL)
							OR (InPatient = @InPatient))
						AND ((@Formula IS NULL
							 AND Formula IS NULL)
							OR (Formula = @Formula))
				)
				    BEGIN
					   --------------Update Contract GUID--------------------
					   EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;
				    END;


				UPDATE dbo.PaymentTypeMedicareIPPayment
				SET
					UpdateDate = @CurrentDate, 
					InPatient = @InPatient,
					Formula = @Formula
				  FROM dbo.PaymentTypeMedicareIPPayment MIPP JOIN ContractServiceLinePaymentTypes CSL ON MIPP.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				  WHERE
						ISNULL(CSL.ContractID, 0) = @ContractId
					AND ISNULL(CSL.ContractServiceTypeID, 0) = @ContractServiceTypeId
					AND ISNULL(CSL.PaymentTypeID, 0) = @PaymentTypeId;
			END;

			--Return Inserted PaymentTypeDetailID
		    SELECT
				   @PaymentTypeDetailId;
		
		/********************** Used for Contract Modelling report *************************/

		EXEC AddEditPaymentTypeFilterCodes @ContractServiceTypeId, @ContractId;
		
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
		COMMIT TRANSACTION @TransactionName;

	END TRY
	BEGIN CATCH
		ROLLBACK TRAN @TransactionName;
		EXEC RaiseErrorInformation
	END CATCH;
END;