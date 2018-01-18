CREATE PROCEDURE dbo.AddEditMedicareOPPayment(
@Paymenttypedetailid BIGINT, 
@Outpatient FLOAT, 
@Paymenttypeid BIGINT, 
@Contractid BIGINT, 
@Contractservicetypeid BIGINT,
@UserName VARCHAR(50))
AS

/****************************************************************************
 *   Name         : AddEditMedicareOPPayment
 *   Author       : mmachina
 *   Date         : 15/Aug/2013
 *	 Modified Date: 23/Aug/2013
 *   Module       : Add/Edit PaymentTypeMedicareOPPayment
 *   Description  : Insertand Update Payment Type MedicareOP Information into database
 *****************************************************************************/

BEGIN

	SET NOCOUNT ON;
	DECLARE
	   @Currentdate DATETIME = GETUTCDATE(),
	   @Action   VARCHAR(10),
	   @PaymentToolDesc NVARCHAR(1000),
	   @Transactionname VARCHAR(100) = 'AddEditMedicareOPPayment';
	BEGIN TRY
		BEGIN TRAN @Transactionname;   
		------------------------------Adding MedicareOP Payment Type----------------------------------  
		IF ISNULL(@Paymenttypedetailid, 0) = 0   
			BEGIN
				SET @Action = 'Add';
				--Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED
				DECLARE
				   @Tmptable TABLE(
								   InsertedID BIGINT);
				--Insert PaymentTypeMedicareOPPayment informations
				INSERT INTO dbo.PaymentTypeMedicareOPPayment(
								 InsertDate, 
								 UpdateDate, 
								 OutPatient)
				OUTPUT
					   INSERTED.PaymentTypeDetailID
					   INTO @Tmptable -- inserting id into @TmpTable
				VALUES(@Currentdate, 
					   NULL, 
					   @Outpatient);
	
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
				------------Updating MedicareOP PaymentType--------------------------------
				SET @Action = 'Modify';

				---Checking Outpatient Psercentage is changed or not
				IF NOT EXISTS
				(
				    SELECT 1
				    FROM dbo.PaymentTypeMedicareOPPayment MOPP
					    JOIN ContractServiceLinePaymentTypes CSL ON MOPP.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				    WHERE ISNULL(CSL.ContractID, 0) = @Contractid
						AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
						AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid
						AND ((@Outpatient IS NULL
							 AND OutPatient IS NULL)
							OR (OutPatient = @Outpatient))
				)
				    BEGIN
					   --------------Update Contract GUID--------------------
					   EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;
				    END;

				UPDATE dbo.PaymentTypeMedicareOPPayment
				SET
					UpdateDate = GETUTCDATE(), 
					OutPatient = @Outpatient
				  FROM dbo.PaymentTypeMedicareOPPayment MOPP JOIN ContractServiceLinePaymentTypes CSL ON MOPP.PaymentTypeDetailId = CSL.PaymentTypeDetailId
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