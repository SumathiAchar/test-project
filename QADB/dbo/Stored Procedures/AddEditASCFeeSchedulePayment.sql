CREATE PROCEDURE dbo.AddEditASCFeeSchedulePayment(
       @PaymentTypeDetailId   BIGINT,
       @Primary               FLOAT,
       @Secondary             FLOAT,
       @Tertiary              FLOAT,
       @Quaternary            FLOAT,
       @Others                FLOAT,
       @NonFeeSchedule        FLOAT,
       @PaymentTypeId         BIGINT,
       @ContractId            BIGINT,
       @ContractServiceTypeId BIGINT,
       @ClaimFieldDocId       BIGINT,
       @SelectedOption        INT,
       @UserName              VARCHAR(50))
AS

/****************************************************************************
 *   Name         : AddEditASCFeeSchedulePayment
 *   Author       : mmachina
 *   Date         : 07/Sep/2013
 *   Module       : Add/Edit PaymentType ASC Fee Schedules
 *   Description  : Insert and Update Payment Type ASCFeeSchedule Information into database
 *	 Modified By  : Sheshagiri on 07/23/2014 to include Non Fee Schedule parameter
 *	 EXEC AddEditASCFeeSchedulePayment  2,5,25,30,35,40,52,10,1,2,5,6
 *****************************************************************************/

    BEGIN
        SET NOCOUNT ON;
        DECLARE @CurrentDate     DATETIME = GETUTCDATE(),
                @Action          VARCHAR(10),
                @PaymentToolDesc NVARCHAR(1000),
                @TransactionName VARCHAR(100) = 'AddEditASCFeeSchedulePayment';
        BEGIN TRY
            BEGIN TRAN @TransactionName;   
            ------------------------------Adding ASCFeeSchedule Payment Type----------------------------------  
            IF ISNULL(@PaymentTypeDetailId, 0) = 0
                BEGIN
                    SET @Action = 'Add';
                    --Insert PaymentTypeASCFeeSchedule informations
                    INSERT INTO dbo.PaymentTypeASCFeeSchedules
                           ( InsertDate,
                             UpdateDate,
                             [Primary],
                             [Secondary],
                             Tertiary,
                             Quaternary,
                             Others,
                             NonFeeSchedule,
                             ClaimFieldDocID,
                             SelectedOption
                           )
                    VALUES( @CurrentDate, NULL, @Primary, @Secondary, @Tertiary, @Quaternary, @Others, @NonFeeSchedule, @ClaimFieldDocId, @SelectedOption);
     
                    --Get inserted PaymentTypeDetailID and update variable
                    SELECT @PaymentTypeDetailId = SCOPE_IDENTITY();

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
                    VALUES( @PaymentTypeDetailId, @CurrentDate, NULL, @PaymentTypeId, NULL, @ContractId, NULL, @ContractServiceTypeId );
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
				    FROM dbo.PaymentTypeASCFeeSchedules PTAF
                         JOIN ContractServiceLinePaymentTypes CSL ON PTAF.PaymentTypeDetailID = CSL.PaymentTypeDetailID
                    WHERE ISNULL(CSL.ContractID, 0) = @ContractId
                      AND ISNULL(CSL.ContractServiceTypeID, 0) = @ContractServiceTypeId
                      AND ISNULL(CSL.PaymentTypeID, 0) = @PaymentTypeId
				  AND ((@Primary IS NULL AND [Primary] IS NULL)
						OR ([Primary] = @Primary))
				  AND ((@Secondary IS NULL AND [Secondary] IS NULL)
						OR ([Secondary] = @Secondary))
				  AND ((@Tertiary IS NULL AND Tertiary IS NULL)
						OR (Tertiary = @Tertiary))
				  AND ((@Quaternary IS NULL AND Quaternary IS NULL)
						OR (Quaternary = @Quaternary))
				  AND ((@Others IS NULL AND Others IS NULL)
						OR (Others = @Others))	
				  AND ((@NonFeeSchedule IS NULL AND NonFeeSchedule IS NULL)
						OR (NonFeeSchedule = @NonFeeSchedule))
				  AND ClaimFieldDocID = @ClaimFieldDocId
                      AND SelectedOption = @SelectedOption
				)
				    BEGIN
				    --Updating Contract GUID
					   EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;
				    END;

                    ---------------------Updating ASCFeeSchedules PaymentType-----------------------------
                    SET @Action = 'Modify';
                    UPDATE dbo.PaymentTypeASCFeeSchedules
                      SET UpdateDate = @CurrentDate,
                          [Primary] = @Primary,
                          [Secondary] = @Secondary,
                          Tertiary = @Tertiary,
                          Quaternary = @Quaternary,
                          Others = @Others,
                          NonFeeSchedule = @NonFeeSchedule,
                          ClaimFieldDocID = @ClaimFieldDocId,
                          SelectedOption = @SelectedOption
                    FROM dbo.PaymentTypeASCFeeSchedules PTAF
                         JOIN ContractServiceLinePaymentTypes CSL ON PTAF.PaymentTypeDetailID = CSL.PaymentTypeDetailID
                    WHERE ISNULL(CSL.ContractID, 0) = @ContractId
                      AND ISNULL(CSL.ContractServiceTypeID, 0) = @ContractServiceTypeId
                      AND ISNULL(CSL.PaymentTypeID, 0) = @PaymentTypeId;
                END;

            --Return Inserted PaymentTypeDetailID
            SELECT @PaymentTypeDetailId;
		
            /********************** Used for Contract Modelling report *************************/

            EXEC AddEditPaymentTypeFilterCodes
                 @ContractServiceTypeId,
                 @ContractId;
			
            /********************** Used for Contract Modelling report *************************/

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

            --Check Any Transaction happened than commit transaction
            COMMIT TRANSACTION @TransactionName;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @Transactionname;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;