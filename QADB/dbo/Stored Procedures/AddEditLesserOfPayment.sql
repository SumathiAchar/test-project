CREATE PROCEDURE [dbo].[AddEditLesserOfPayment](
       @PaymentTypeDetailID   BIGINT,
       @Percentage            FLOAT,
       @PaymentTypeID         BIGINT,
       @ContractID            BIGINT,
       @ContractServiceTypeID BIGINT,
       @IsLesserOf            BIT,
       @UserName              VARCHAR(50))
AS  

/****************************************************************************  
 *   Name         : AddEditLesserOfPayment  
 *   Author       : mmachina  
 *   Date         : 15/Aug/2013  
 *   Module       : AddEditLesserOfPayment  
 *   Description  : Insert and update payment type per discount into database  
 *****************************************************************************/

    BEGIN
        SET NOCOUNT ON;
        DECLARE @CurrentDate     DATETIME = GETUTCDATE(),
                @PaymentToolDesc NVARCHAR(500),
                @TransactionName VARCHAR(100) = 'AddEditLesserOfPayment';
        BEGIN TRY
            BEGIN TRAN @TransactionName;  
            ------------------------------Adding LesserOf Payment Type----------------------------------  
            IF ISNULL(@PaymentTypeDetailID, 0) = 0
                BEGIN
                    --Insert PaymentType LesserOf informations  
                    INSERT INTO dbo.PaymentTypeLesserOf
                           ( InsertDate,
                             UpdateDate,
                             Percentage,
                             IsLesserOf
                           )
                    VALUES( @CurrentDate, NULL, @Percentage, @IsLesserOf );  
   
                    --Get inserted PaymentTypeDetailID and update variable  
                    SELECT @PaymentTypeDetailID = SCOPE_IDENTITY();
  
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
                    VALUES( @PaymentTypeDetailID, @CurrentDate, NULL, @PaymentTypeID, NULL, @ContractID, NULL, @ContractServiceTypeID );  
                    --Return Inserted PaymentTypeDetailID  
                    SELECT @PaymentTypeDetailID;
                    SELECT @PaymentToolDesc = 'Added Payment Tool: ' + FilterName + ' ' + FilterValues
                    FROM dbo.GetContractFiltersByID( @Contractid, @Contractservicetypeid )
                    WHERE PaymentTypeId = @PaymentTypeId;

				------------------------Updating Contract GUID-----------------------------
				 EXEC [UpdateContractGUID]
						   @ContractID,
						   @ContractServiceTypeID;
                END;
            ELSE
            ------------------------------Updating PercentageDiscount Payment Type----------------------------------  
                BEGIN
			 --Checking if the Payment Tool value is changed
			 IF NOT EXISTS
				(
				    SELECT 1
				    FROM  dbo.PaymentTypeLesserOf PLO
                         JOIN ContractServiceLinePaymentTypes CSL ON PLO.PaymentTypeDetailID = CSL.PaymentTypeDetailID
                    WHERE ISNULL(CSL.ContractID, 0) = @ContractID
                      AND ISNULL(CSL.ContractServiceTypeID, 0) = @ContractServiceTypeID
                      AND ISNULL(CSL.PaymentTypeID, 0) = @PaymentTypeID
				  AND ((@Percentage IS NULL AND Percentage IS NULL)
						OR (Percentage = @Percentage))
				  AND ((@IsLesserOf IS NULL AND IsLesserOf IS NULL)
						OR (IsLesserOf = @IsLesserOf))				  			  					   
				)
				    BEGIN
					   EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;
				    END;


                    UPDATE dbo.PaymentTypeLesserOf
                      SET UpdateDate = GETUTCDATE(),
                          Percentage = @Percentage,
                          IsLesserOf = @IsLesserOf
                    FROM dbo.PaymentTypeLesserOf PDP
                         JOIN ContractServiceLinePaymentTypes CSL ON PDP.PaymentTypeDetailID = CSL.PaymentTypeDetailID
                    WHERE ISNULL(CSL.ContractID, 0) = @ContractID
                      AND ISNULL(CSL.ContractServiceTypeID, 0) = @ContractServiceTypeID
                      AND ISNULL(CSL.PaymentTypeID, 0) = @PaymentTypeID;
                    SELECT @PaymentTypeDetailID;
                    SELECT @PaymentToolDesc = FilterName + '=' + FilterValues
                    FROM dbo.GetContractFiltersByID( @Contractid, @Contractservicetypeid )
                    WHERE PaymentTypeId = @PaymentTypeId;
                END;
		
            /********************** Used for Contract Modeling report *************************/

            EXEC AddEditPaymentTypeFilterCodes
                 @ContractServiceTypeID,
                 @ContractID;

	    
            --Insert Audit Log Information

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

            --Check Any Transaction happened than commit transaction
            COMMIT TRANSACTION @TransactionName;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;