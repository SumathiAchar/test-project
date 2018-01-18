CREATE PROCEDURE dbo.AddEditCAPPayment(
       @Paymenttypedetailid   BIGINT,
       @Percentage            FLOAT,
       @Threshold             FLOAT,
       @Paymenttypeid         BIGINT,
       @Contractid            BIGINT,
       @Contractservicetypeid BIGINT,
       @UserName              VARCHAR(50))
AS

/****************************************************************************
 *   Name         : AddEditCAPPayment
 *   Author       : mmachina
 *   Date         : 23/Aug/2013
 *   Module       : Add/Edit PaymentType Cap
 *   Description  : Insert and update Payment Type Cap Information into database
 *****************************************************************************/

    BEGIN
        SET NOCOUNT ON;
        DECLARE @Currentdate     DATETIME = GETUTCDATE(),
                @PaymentToolDesc NVARCHAR(500),
                @Transactionname VARCHAR(100) = 'AddEditCAPPayment';
        BEGIN TRY
            BEGIN TRAN @Transactionname;   
            ------------------------------Adding CAP Payment Type----------------------------------  
            IF ISNULL(@PaymentTypeDetailId, 0) = 0
                BEGIN
                    --Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED
                    DECLARE @Tmptable TABLE( InsertedID BIGINT );
                    --Insert PaymentTypeStopLoss informations
                    INSERT INTO dbo.PaymentTypeCap
                           ( InsertDate,
                             UpdateDate,
                             Percentage,
                             Threshold
                           )
                    OUTPUT INSERTED.PaymentTypeDetailID
                           INTO @Tmptable -- inserting id into @TmpTable
                    VALUES( @Currentdate, NULL, @Percentage, @Threshold );
	
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
                    SELECT @PaymentToolDesc = 'Added Payment Tool: ' + FilterName+ ' ' + FilterValues
                    FROM dbo.GetContractFiltersByID( @Contractid, @Contractservicetypeid )
                    WHERE PaymentTypeId = @PaymentTypeId;

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
				    FROM dbo.PaymentTypeCap PTC
                         JOIN ContractServiceLinePaymentTypes CSL ON PTC.PaymentTypeDetailId = CSL.PaymentTypeDetailId
                    WHERE ISNULL(CSL.ContractID, 0) = @Contractid
                      AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
                      AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid	
				  AND ((@Percentage IS NULL AND Percentage IS NULL)
						OR (Percentage = @Percentage))
				  AND ((@Threshold IS NULL AND Threshold IS NULL)
						OR (Threshold = @Threshold))				  		  					   
				)
				    BEGIN
				    --Updating Contract GUID
					   EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;
				    END;

                    -------------Updating Cap PaymentType-----------------------------------
                    UPDATE dbo.PaymentTypeCap
                      SET UpdateDate = @Currentdate,
                          Percentage = @Percentage,
                          Threshold = @Threshold
                    FROM dbo.PaymentTypeCap PTC
                         JOIN ContractServiceLinePaymentTypes CSL ON PTC.PaymentTypeDetailId = CSL.PaymentTypeDetailId
                    WHERE ISNULL(CSL.ContractID, 0) = @Contractid
                      AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
                      AND ISNULL(CSL.PaymentTypeID, 0) = @Paymenttypeid;
                    SELECT @Paymenttypedetailid;
                    SELECT @PaymentToolDesc = FilterName + ' ' + FilterValues
                    FROM dbo.GetContractFiltersByID( @Contractid, @Contractservicetypeid )
                    WHERE PaymentTypeId = @PaymentTypeId;
                END;
		
            /********************** Used for Contract Modelling report *************************/

            EXEC AddEditPaymentTypeFilterCodes
                 @Contractservicetypeid,
                 @Contractid;
		
            ---Insert Audit Log Information


            IF @Contractid IS NULL
            OR @Contractid = 0
                BEGIN
                    EXEC InsertAuditLog
                         @UserName,
                         'Modify',
                         'Service Type',
                         @PaymentToolDesc,
                         @ContractServiceTypeId,
                         0;
                END;
            ELSE
                BEGIN
                    EXEC InsertAuditLog
                         @UserName,
                         'Modify',
                         'Contract - Modelling',
                         @PaymentToolDesc,
                         @Contractid,
                         1;
                END;
            --Check Any Transation happened than commit transation
            COMMIT TRANSACTION @Transactionname;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @Transactionname;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;