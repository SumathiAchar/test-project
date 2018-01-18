CREATE PROCEDURE [dbo].[AddEditMedicareSequesterPayment](@PaymenTypeDetailId    BIGINT,
                                                         @Percentage            FLOAT,
                                                         @PaymentTypeId         BIGINT,
                                                         @ContractId            BIGINT,
                                                         @ContractServiceTypeId BIGINT,
                                                         @UserName              VARCHAR(50))
AS

/****************************************************************************
 *   Name         : AddEditMedicareSequesterPayment
 *   Author       : Sushmita Dube
 *   Date         : 15/April/2016
 *   Module       : Add/Edit PaymentTypeMedicareSequester
 *   Description  : Insert/Update Payment Type Medicare Sequester Information into database
 *****************************************************************************/

BEGIN

	SET NOCOUNT ON;
	DECLARE @Currentdate DATETIME = GETUTCDATE(),
		    @Action   VARCHAR(10),
		    @PaymentToolDesc NVARCHAR(1000),
		    @Transactionname VARCHAR(100) = 'AddEditMedicareSequesterPayment';
	BEGIN TRY
		BEGIN TRAN @Transactionname;  
		------------------------------Adding Medicare Sequester Payment Type----------------------------------  
		IF ISNULL(@PaymenTypeDetailId, 0) = 0   
			BEGIN
				SET @Action = 'Add';
				--Declare TmpTable for storing inserted PaymenTypeDetailId by using OUTPUT INSERTED
				DECLARE
				   @Tmptable TABLE(
								   InsertedID BIGINT);
				--Insert PaymentTypePerCase informations
				INSERT INTO dbo.PaymentTypeMedicareSequester(
								 InsertDate, 
								 UpdateDate, 
								 Percentage)
				OUTPUT
					   INSERTED.PaymentTypeDetailID
					   INTO @Tmptable -- inserting id into @TmpTable
				VALUES(@Currentdate, 
					   NULL, 
					   @Percentage);
	
				--Get inserted PaymenTypeDetailId and update variable
				SELECT
					   @PaymenTypeDetailId = InsertedID
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
				VALUES(@PaymenTypeDetailId, 
					   @Currentdate, 
					   NULL, 
					   @PaymentTypeId, 
					   NULL, 
					   @ContractId, 
					   NULL, 
					   @ContractServiceTypeId);
				--Return Inserted PaymentTypeDetailID
				SELECT
					   @PaymenTypeDetailId;

				------------------------Updating Contract GUID-----------------------------
				 EXEC [UpdateContractGUID]
						   @ContractID,
						   @ContractServiceTypeID;
			END;
		ELSE
			BEGIN
			 --Checks if the Payment Type Values are changed and then update the contract GUID if not changed
			 IF NOT EXISTS
				(
				    SELECT 1
				    FROM dbo.PaymentTypeMedicareSequester PPC 
				    JOIN ContractServiceLinePaymentTypes CSL ON PPC.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				  WHERE
						ISNULL(CSL.ContractID, 0) = @ContractId
					AND ISNULL(CSL.ContractServiceTypeID, 0) = @ContractServiceTypeId
					AND ISNULL(CSL.PaymentTypeID, 0) = @PaymentTypeId	
					AND Percentage = @Percentage	  					   
				)
				    BEGIN
				    --Updating Contract GUID
					   EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;
				    END;

				------------------------------Updating Medicare Sequester Payment Type----------------------------------  
				SET @Action = 'Modify';
				UPDATE dbo.PaymentTypeMedicareSequester
				SET
					UpdateDate = @Currentdate, 
					Percentage = @Percentage
				  FROM dbo.PaymentTypeMedicareSequester PPC JOIN ContractServiceLinePaymentTypes CSL ON PPC.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				  WHERE
						ISNULL(CSL.ContractID, 0) = @ContractId
					AND ISNULL(CSL.ContractServiceTypeID, 0) = @ContractServiceTypeId
					AND ISNULL(CSL.PaymentTypeID, 0) = @PaymentTypeId;

				SELECT
					   @PaymenTypeDetailId;
			END;

		/********************** Used for Contract Modeling report *************************/

		EXEC AddEditPaymentTypeFilterCodes @ContractServiceTypeId, @ContractId;
		
		/********************** Insert Audit log *************************/

		SELECT @PaymentToolDesc =FilterName + FilterValues FROM dbo.GetContractFiltersByID( @ContractId, @ContractServiceTypeId )
			   WHERE PaymentTypeId = @PaymentTypeId;
			   
            IF( @Action = 'Add' )
                BEGIN
                    SET @PaymentToolDesc = 'Added Payment Tool: ' + @PaymentToolDesc;
                END;
			 
            EXEC InsertAuditLog
                 @UserName,
			  @Action,
                 'Contract - Modelling',
                 @PaymentToolDesc,
                 @ContractId,
                 1;
		--Check Any Transaction happened than commit transaction
		COMMIT TRANSACTION @Transactionname;

	END TRY
	BEGIN CATCH
		ROLLBACK TRAN @TransactionName;
		EXEC RaiseErrorInformation
	END CATCH;
END;