﻿CREATE PROCEDURE dbo.AddEditDRGPayment(
@Paymenttypedetailid BIGINT, 
@Baserate FLOAT, 
@Paymenttypeid BIGINT, 
@Contractid BIGINT, 
@Contractservicetypeid BIGINT, 
@Claimfielddocid BIGINT,
@UserName VARCHAR(50))
AS

/****************************************************************************
 *   Name         : AddEditDRGPayment
 *   Author       : mmachina
 *   Date         : 15/Aug/2013
 *   Module       : Add/Edit PaymentTypeDRGPayment
 *   Description  : Insert and Update Payment Type DRG payment Information into database
 *****************************************************************************/

BEGIN

	SET NOCOUNT ON;
	DECLARE
	   @Currentdate DATETIME = GETUTCDATE(),
	   @Action          VARCHAR(10),
        @PaymentToolDesc NVARCHAR(1000),
	   @Transactionname VARCHAR(100) = 'AddEditDRGPayment';
	BEGIN TRY
		BEGIN TRAN @Transactionname;   

		------------------------------Adding DRG Payment Type----------------------------------  
		IF ISNULL(@Paymenttypedetailid, 0) = 0   
			BEGIN
				SET @Action = 'Add';  
				--Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED
				DECLARE
				   @Tmptable TABLE(
								   InsertedID BIGINT);
				--Insert PaymentTypeDRGPayment informations
				INSERT INTO dbo.PaymentTypeDRGPayment(
								 InsertDate, 
								 UpdateDate, 
								 BaseRate, 
								 ClaimFieldDocID)
				OUTPUT
					   INSERTED.PaymentTypeDetailID
					   INTO @Tmptable -- inserting id into @TmpTable
				VALUES(@Currentdate, 
					   NULL, 
					   @Baserate, 
					   @Claimfielddocid);
	
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
			
				-----------------------Updating DRG PaymentType-------------------------------------------
				SET @Action = 'Modify';

				---Checking Column values are changed or not
				IF NOT EXISTS
				(
				    SELECT 1
				    FROM dbo.PaymentTypeDRGPayment PTD
					    JOIN ContractServiceLinePaymentTypes CSL ON PTD.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				    WHERE ISNULL(CSL.ContractID, 0) = @Contractid
						AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
						AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid
						AND ((@Baserate IS NULL
							 AND BaseRate IS NULL)
							OR (BaseRate = @Baserate))
						AND ((@Claimfielddocid IS NULL
							 AND ClaimFieldDocID IS NULL)
							OR (ClaimFieldDocID = @Claimfielddocid))
				)
				    BEGIN
					   --------------Update Contract GUID--------------------
					   EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;
				    END;

				    UPDATE dbo.PaymentTypeDRGPayment
				    SET
						UpdateDate = @Currentdate,
						BaseRate = @Baserate,
						ClaimFieldDocID = @Claimfielddocid
				    FROM dbo.PaymentTypeDRGPayment PTD
					    JOIN ContractServiceLinePaymentTypes CSL ON PTD.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				    WHERE ISNULL(CSL.ContractID, 0) = @Contractid
						AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
						AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid;

				SELECT
					   @Paymenttypedetailid;
			END;
			
		/********************** Used for Contract Modelling report *************************/

		EXEC AddEditPaymentTypeFilterCodes @Contractservicetypeid, @Contractid;
			
		/********************** Insert Audit log *************************/

		SELECT @PaymentToolDesc =FilterName + FilterValues FROM dbo.GetContractFiltersByID( @Contractid, @Contractservicetypeid )
			   WHERE PaymentTypeId = @PaymentTypeId;
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
