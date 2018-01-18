/****************************************************************************  
 *   Name         : DeleteClaimFieldDocByID  
 *   Author       : Sumathi  
 *   Date         : 07/Sep/2015
 *   Module       : Maintain/View in Payment Tables
 *   Description  : Deletes the Payment Table based on the ClaimFieldDocID
 *****************************************************************************/

CREATE PROCEDURE [dbo].[DeleteClaimFieldDocByID](
       @ClaimFieldDocID BIGINT,
       @UserName        VARCHAR(50))
AS
    BEGIN
        DECLARE @TransactionName VARCHAR(100) = 'DeleteClaimFieldDocByID',
                @ClaimToolDesc   VARCHAR(1000);
			 
			 
	   
	   CREATE TABLE #Temp_PaymentTypeDetail(
								 PaymentTypeDetailID    BIGINT NULL,
                                         PaymentTypeID TINYINT NULL,
								 ContractServiceLineID BIGINT NULL,
								 ServiceLineTypeID TINYINT NULL
								   );
        BEGIN TRY
            BEGIN TRAN @TransactionName;

            --Audit Logging
            SELECT @ClaimToolDesc = 'File Name: ' + C.[FileName] + ', Table Type:' + CASE
                                                                                         WHEN C.ClaimFieldID = 21
                                                                                         THEN 'ASC Fee Schedule'
                                                                                         WHEN C.ClaimFieldID = 22
                                                                                         THEN 'Fee Schedule'
                                                                                         WHEN C.ClaimFieldID = 23
                                                                                         THEN 'DRG Schedule'
                                                                                         WHEN C.ClaimFieldID = 35
                                                                                         THEN 'Custom Table'
                                                                                     END + ', Table Name:' + TableName
            FROM ClaimFieldDocs C
            WHERE C.ClaimFieldDocId = @ClaimFieldDocID;
            EXEC InsertAuditLog
                 @UserName,
                 'Delete',
                 'Payment Tables',
                 @ClaimToolDesc,
                 @ClaimFieldDocID,
                 5;
			  
		  --Deletes the table information from  Contract Service Line Table Selection
		  INSERT INTO #Temp_PaymentTypeDetail(PaymentTypeDetailID,PaymentTypeID,ContractServiceLineID,ServiceLineTypeID)
		  SELECT NULL,NULL,ContractServiceLineID,8 FROM ContractServiceLineTableSelection
		  WHERE ClaimFieldDocId=@ClaimFieldDocID

		  DELETE CSTS FROM ContractServiceLineTableSelection CSTS INNER JOIN #Temp_PaymentTypeDetail TPTD
		  ON CSTS.ContractServiceLineID=TPTD.ContractServiceLineID WHERE TPTD.ServiceLineTypeID=8
		
		  --Deletes the table information from  Custom Table Payment Type
		   INSERT INTO #Temp_PaymentTypeDetail(PaymentTypeDetailID,PaymentTypeID,ContractServiceLineID,ServiceLineTypeID)
		  SELECT PaymentTypeDetailID,14,NULL,NULL FROM PaymentTypeCustomTable
		  WHERE ClaimFieldDocId=@ClaimFieldDocID

            DELETE PTCT FROM [dbo].[PaymentTypeCustomTable] PTCT INNER JOIN #Temp_PaymentTypeDetail TPTD
		  ON PTCT.PaymentTypeDetailID=TPTD.PaymentTypeDetailID WHERE TPTD.PaymentTypeID=14

            --Deletes the table information from ASC Fee Schedules Payment Type
		  INSERT INTO #Temp_PaymentTypeDetail(PaymentTypeDetailID,PaymentTypeID,ContractServiceLineID,ServiceLineTypeID)
		  SELECT PaymentTypeDetailID,1,NULL,NULL FROM PaymentTypeASCFeeSchedules
		  WHERE ClaimFieldDocId=@ClaimFieldDocID

		  DELETE PTAS FROM [dbo].[PaymentTypeASCFeeSchedules] PTAS INNER JOIN #Temp_PaymentTypeDetail TPTD
		  ON PTAS.PaymentTypeDetailID=TPTD.PaymentTypeDetailID WHERE TPTD.PaymentTypeID=1
          
		  --Deletes the table information from Fee Schedules Payment Type
		   INSERT INTO #Temp_PaymentTypeDetail(PaymentTypeDetailID,PaymentTypeID,ContractServiceLineID,ServiceLineTypeID)
		  SELECT PaymentTypeDetailID,3,NULL,NULL FROM PaymentTypeFeeSchedules
		  WHERE ClaimFieldDocId=@ClaimFieldDocID 

		  DELETE PTFS FROM [dbo].[PaymentTypeFeeSchedules] PTFS INNER JOIN #Temp_PaymentTypeDetail TPTD
		  ON PTFS.PaymentTypeDetailID=TPTD.PaymentTypeDetailID WHERE TPTD.PaymentTypeID=3
            
		  --Deletes the table information from DRG Payment Type
		  INSERT INTO #Temp_PaymentTypeDetail(PaymentTypeDetailID,PaymentTypeID,ContractServiceLineID,ServiceLineTypeID)
		  SELECT PaymentTypeDetailID,2,NULL,NULL FROM PaymentTypeDRGPayment
		  WHERE ClaimFieldDocId=@ClaimFieldDocID

		  DELETE PTDP FROM [dbo].[PaymentTypeDRGPayment] PTDP INNER JOIN #Temp_PaymentTypeDetail TPTD
		  ON PTDP.PaymentTypeDetailID=TPTD.PaymentTypeDetailID WHERE TPTD.PaymentTypeID=2
            
		  --Deleting from ContractServiceLinePaymentTypes
		  DELETE CSPT FROM [dbo].ContractServiceLinePaymentTypes CSPT INNER JOIN #Temp_PaymentTypeDetail TPTD 
		  ON CSPT.PaymentTypeDetailID=TPTD.PaymentTypeDetailID AND CSPT.PaymentTypeID=TPTD.PaymentTypeID
		  AND CSPT.ContractServiceLineID=TPTD.ContractServiceLineID AND CSPT.ServiceLineTypeID=TPTD.ServiceLineTypeID
		  		  
            --Deletes the Claim Field Values
            DELETE FROM [dbo].[ClaimFieldValues]
            WHERE ClaimFieldDocID = @ClaimFieldDocId;
   			
            --Deletes the Claim Field Document
            DELETE FROM [dbo].[ClaimFieldDocs]
            WHERE ClaimFieldDocID = @ClaimFieldDocId;

		  DROP TABLE #Temp_PaymentTypeDetail
            COMMIT TRANSACTION @TransactionName;

        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;
