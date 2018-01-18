CREATE PROCEDURE [dbo].[AddEditFeeSchedulePayment](
@Paymenttypedetailid BIGINT, 
@Feeschedule FLOAT, 
@Nonfeeschedule FLOAT, 
@Paymenttypeid BIGINT, 
@Contractid BIGINT, 
@Contractservicetypeid BIGINT, 
@Claimfielddocid BIGINT,
@UserName VARCHAR(50),
@IsObserveUnits BIT)
AS

/****************************************************************************
 *   Name         : AddEditFeeSchedulePayment
 *   Author       : mmachina
 *   Date         : 07/Sep/2013
 *   Module       : Add/Edit PaymentTypeFeeSchedules
 *   Description  : Insert and Update Payment Type FeeSchedule Information into database
 *****************************************************************************/

BEGIN
	SET NOCOUNT ON;
	DECLARE
	   @Currentdate DATETIME = GETUTCDATE(),
	   @Action          VARCHAR(10),
        @PaymentToolDesc NVARCHAR(1000),
	   @Transactionname VARCHAR(100) = 'AddEditFeeSchedulePayment';

	BEGIN TRY
		BEGIN TRAN @Transactionname;   
		------------------------------Adding FeeSchedule Payment Type----------------------------------  
		IF ISNULL(@Paymenttypedetailid, 0) = 0   
			BEGIN
				SET @Action = 'Add';	
				--Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED
				DECLARE
				   @Tmptable TABLE(
								   InsertedID BIGINT);
				--Insert PaymentTypeFeeSchedules informations
				INSERT INTO dbo.PaymentTypeFeeSchedules(
								 InsertDate, 
								 UpdateDate, 
								 FeeSchedule, 
								 NonFeeSchedule, 
								 ClaimFieldDocID,
								 IsObserveUnits)
				OUTPUT
					   INSERTED.PaymentTypeDetailID
					   INTO @Tmptable -- inserting id into @TmpTable
				VALUES(@Currentdate, 
					   NULL, 
					   @Feeschedule, 
					   @Nonfeeschedule, 
					   @Claimfielddocid,
					   @IsObserveUnits);
	
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
				---------------Updating FeeSchedule PaymentType-----------------------------------
				SET @Action = 'Modify';  

				---Checking Column values are changed or not
				IF NOT EXISTS
				(
				    SELECT 1
				    FROM dbo.PaymentTypeFeeSchedules PTFS
					    JOIN ContractServiceLinePaymentTypes CSL ON PTFS.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				    WHERE ISNULL(CSL.ContractID, 0) = @Contractid
						AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
						AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid
						AND ((@Feeschedule IS NULL
							 AND FeeSchedule IS NULL)
							OR (FeeSchedule = @Feeschedule))
						AND ((@Nonfeeschedule IS NULL
							 AND NonFeeSchedule IS NULL)
							OR (NonFeeSchedule = @Nonfeeschedule))
						AND ((@Claimfielddocid IS NULL
							 AND ClaimFieldDocID IS NULL)
							OR (ClaimFieldDocID = @Claimfielddocid))
						AND ((@IsObserveUnits IS NULL
							 AND IsObserveUnits IS NULL)
							OR (IsObserveUnits = @IsObserveUnits))
				)
				    BEGIN
					   --------------Update Contract GUID--------------------
					   EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;
				    END;

				UPDATE dbo.PaymentTypeFeeSchedules
				SET
					UpdateDate = @Currentdate, 
					FeeSchedule = @Feeschedule, 
					NonFeeSchedule = @Nonfeeschedule, 
					ClaimFieldDocID = @Claimfielddocid,
					IsObserveUnits=@IsObserveUnits
				  FROM dbo.PaymentTypeFeeSchedules PTFS JOIN ContractServiceLinePaymentTypes CSL ON PTFS.PaymentTypeDetailId = CSL.PaymentTypeDetailId
				  WHERE
						ISNULL(CSL.ContractID, 0) = @Contractid
					AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
					AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid;

				SELECT
					   @Paymenttypedetailid;
			END;
			
		/********************** Used for Contract Modelling report *************************/

		EXEC AddEditPaymentTypeFilterCodes @Contractservicetypeid, @Contractid;
			
		/**********************Insert Audit log *************************/

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
