CREATE PROCEDURE dbo.AddEditPerCasePayment(
       @Paymenttypedetailid   BIGINT,
       @Rate                  FLOAT,
       @Maxcasesperday        INT,
       @Paymenttypeid         BIGINT,
       @Contractid            BIGINT,
       @Contractservicetypeid BIGINT,
       @UserName              VARCHAR(50))
AS

/****************************************************************************
 *   Name         : AddEditPerCasePayment
 *   Author       : mmachina
 *   Date         : 15/Aug/2013
 *   Module       : Add/Edit PaymentTypePerCase
 *   Description  : Insert/Update Payment Type Per Case Information into database
 *****************************************************************************/

   BEGIN

	SET NOCOUNT ON;
	DECLARE @Currentdate DATETIME = GETUTCDATE(),
		   @Action   VARCHAR(10),
		   @PaymentToolDesc NVARCHAR(1000),
		   @Transactionname VARCHAR(100) = 'AddEditPerCasePayment';
	BEGIN TRY
		BEGIN TRAN @Transactionname;  
		------------------------------Adding PerCase Payment Type----------------------------------  
		IF ISNULL(@Paymenttypedetailid, 0) = 0   
			BEGIN
				SET @Action = 'Add';
				--Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED
				DECLARE
				   @Tmptable TABLE(
								   InsertedID BIGINT);
				--Insert PaymentTypePerCase informations
				INSERT INTO dbo.PaymentTypePerCase(
								 InsertDate, 
								 UpdateDate, 
								 Rate, 
								 MaxCasesPerDay)
				OUTPUT
					   INSERTED.PaymentTypeDetailID
					   INTO @Tmptable -- inserting id into @TmpTable
				VALUES(@Currentdate, 
					   NULL, 
					   @Rate, 
					   @Maxcasesperday);
	
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
				------------------------------Updating PerCase Payment Type----------------------------------  
				SET @Action = 'Modify';

				------------------------ Check column values are changed or not -----------------------------

				IF NOT EXISTS
				(
				    SELECT 1
				    FROM dbo.PaymentTypePerCase PPC
					    JOIN ContractServiceLinePaymentTypes CSL ON PPC.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				    WHERE ISNULL(CSL.ContractID, 0) = @Contractid
						AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
						AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid
						AND ((@Rate IS NULL
							 AND Rate IS NULL)
							OR (Rate = @Rate))
						AND ((@Maxcasesperday IS NULL
							 AND MaxCasesPerDay IS NULL)
							OR (MaxCasesPerDay = @Maxcasesperday))
				)
				    BEGIN
					   --------------Update Contract GUID--------------------
					   EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;
				    END;

				UPDATE dbo.PaymentTypePerCase
				SET
					UpdateDate = @Currentdate, 
					Rate = @Rate, 
					MaxCasesPerDay = @Maxcasesperday
				  FROM dbo.PaymentTypePerCase PPC JOIN ContractServiceLinePaymentTypes CSL ON PPC.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				  WHERE
						ISNULL(CSL.ContractID, 0) = @Contractid
					AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
					AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid;

				SELECT
					   @Paymenttypedetailid;
			END;

		/********************** Used for Contract Modeling report *************************/

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

		--Check Any Transaction happened than commit transaction
		COMMIT TRANSACTION @Transactionname;

	END TRY
	BEGIN CATCH
		ROLLBACK TRAN @TransactionName;
		EXEC RaiseErrorInformation
	END CATCH;
END;