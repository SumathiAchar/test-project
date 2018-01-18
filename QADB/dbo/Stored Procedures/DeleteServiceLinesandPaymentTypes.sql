CREATE PROCEDURE [dbo].[DeleteServiceLinesandPaymentTypes](
       @ContractID            BIGINT,
       @ContractServiceTypeID BIGINT,
       @ServiceLineTypeID     BIGINT,
       @PaymentTypeID         BIGINT,
       @UserName              VARCHAR(50))
AS

/****************************************************************************  
 *   Name         : DeleteServiceLinesandPaymentTypes  
 *   Author       : mmachina  
 *   Date         : 28/Aug/2013  
 *   Modified By  : Raj
 *   Modified Date: 28/Aug/2013  
 *   Module       : Deleting ServiceLines and Payment Types  
 *   Description  : Deleting ServiceLines and Payment Types  

 * --EXEC DeleteServiceLinesandPaymentTypes 16037
 *****************************************************************************/

    BEGIN
        SET NOCOUNT ON;
        DECLARE @Transactionname VARCHAR(100) = 'DeleteServiceLinesandPaymentTypes',
                @ClaimToolDesc   NVARCHAR(500);
        BEGIN TRY
            BEGIN TRAN @Transactionname;
            DECLARE @Contractservicelinepaymenttypeid BIGINT;
            DECLARE @Paymenttypedetailid BIGINT;
            DECLARE @Contractservicelineid BIGINT;
            IF( @ServiceLineTypeID = 0
             OR @ServiceLineTypeID IS NULL
              )
                BEGIN
                    SELECT @ClaimToolDesc = FilterName
                    FROM GetContractFiltersByID( @ContractID, @ContractServiceTypeID )
                    WHERE PaymentTypeID = @PaymentTypeID
                      AND ServiceLineTypeID IS NULL;
                END;
            ELSE
                BEGIN
                    SELECT @ClaimToolDesc = FilterName
                    FROM GetContractFiltersByID( @ContractID, @ContractServiceTypeID )
                    WHERE PaymentTypeID IS NULL
                      AND ServiceLineTypeID = @ServiceLineTypeID;
                END;

            --If Payment Type 
            IF @PaymentTypeID <> 0
                BEGIN
                    ------------------deleting ASCFeeSchedules Payment Type details----------------
                    IF @PaymentTypeID = 1
                        BEGIN
                            DELETE PTFS
                            FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PAYMENTTYPEASCFEESCHEDULES AS PTFS ON PTFS.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
                            ------------------deleting DRG Payment Type details----------------
                    ELSE
                    IF @PaymentTypeID = 2
                        BEGIN
                            DELETE PTDRG
                            FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PAYMENTTYPEDRGPAYMENT AS PTDRG ON PTDRG.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
                            ------------------deleting FeeSchedules Payment Type details----------------
                    ELSE
                    IF @PaymentTypeID = 3
                        BEGIN
                            DELETE PTFS
                            FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PAYMENTTYPEFEESCHEDULES AS PTFS ON PTFS.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
                            ------------------deleting MedicareIP Payment Type details----------------
                    ELSE
                    IF @PaymentTypeID = 4
                        BEGIN
                            DELETE PTMIPP
                            FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PAYMENTTYPEMEDICAREIPPAYMENT AS PTMIPP ON PTMIPP.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
                            ------------------deleting MedicareOP Payment Type details----------------
                    ELSE
                    IF @PaymentTypeID = 5
                        BEGIN
                            DELETE PTMOPP
                            FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PAYMENTTYPEMEDICAREOPPAYMENT AS PTMOPP ON PTMOPP.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
                            ------------------deleting PerCase Payment Type details----------------
                    ELSE
                    IF @PaymentTypeID = 6
                        BEGIN
                            DELETE PTPC
                            FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PAYMENTTYPEPERCASE AS PTPC ON PTPC.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
                            ------------------deleting pePerDiem Payment Type details----------------
                    ELSE
                    IF @PaymentTypeID = 7
                        BEGIN
                            DELETE PTPD
                            FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PAYMENTTYPEPERDIEM AS PTPD ON PTPD.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
                            ------------------deleting PercentageDiscount Payment Type details----------------
                    ELSE
                    IF @PaymentTypeID = 8
                        BEGIN
                            DELETE PTPDP
					   FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PAYMENTTYPEPERCENTAGEDISCOUNTPAYMENT AS PTPDP ON PTPDP.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
                            ------------------deleting PerVisit Payment Type details----------------
                    ELSE
                    IF @PaymentTypeID = 9
                        BEGIN
                            DELETE PTPV
                            FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PAYMENTTYPEPERVISIT AS PTPV ON PTPV.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
                            ------------------deleting StopLoss Payment Type details----------------
                    ELSE
                    IF @PaymentTypeID = 10
                        BEGIN
                            DELETE PTSL
                            FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PAYMENTTYPESTOPLOSS AS PTSL ON PTSL.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
                            ------------------deleting IsLesserOf Payment Type details----------------
                    ELSE
                    IF @PaymentTypeID = 11
                        BEGIN
                            DELETE PTLO
                            FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PAYMENTTYPELESSEROF AS PTLO ON PTLO.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
                            ------------------deleting perCap Payment Type details----------------
                    ELSE
                    IF @PaymentTypeID = 12
                        BEGIN
                            DELETE PTC
                            FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PAYMENTTYPECAP AS PTC ON PTC.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
                            ------------------deleting Custom Payment Type details----------------
                    ELSE
                    IF @PaymentTypeID = 14
                        BEGIN
					   DELETE PTC
                            FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                 JOIN PaymentTypeCustomTable AS PTC ON PTC.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                            WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                              AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;
				      ------------------deleting Medicare Sequester Payment Type details----------------
				ELSE 
				IF @PaymentTypeID = 15
                    BEGIN
				    DELETE PTMS
                        FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                JOIN PaymentTypeMedicareSequester AS PTMS ON PTMS.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
                        WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                            AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                            AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                            AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                    END;;;;;;;;;;;;

                    --Delete Entry from ContractServiceLinePaymentTypes Table
                    DELETE FROM DBO.CONTRACTSERVICELINEPAYMENTTYPES
                    WHERE ISNULL(CONTRACTID, 0) = @ContractID
                      AND ISNULL(CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                      AND ISNULL(SERVICELINETYPEID, 0) = @ServiceLineTypeID
                      AND ISNULL(PAYMENTTYPEID, 0) = @PaymentTypeID;

                    /********************** Used for Contract Modelling report *************************/

                    EXEC AddEditPaymentTypeFilterCodes
                         @ContractServiceTypeID,
                         @ContractID;
                END;
                    --If Service Type
            ELSE
                BEGIN
                    IF @ServiceLineTypeID <> 0
                        BEGIN
                            ------------------deleting service line code details----------------
                            IF @ServiceLineTypeID IN( 1, 2, 3, 4, 5, 6 )
                                BEGIN
                                    DELETE CS
                                    FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                         JOIN CONTRACTSERVICELINES AS CS ON CS.CONTRACTSERVICELINEID = CSL.CONTRACTSERVICELINEID
                                    WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                                      AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                                      AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID;
                                END;

                            ------------------deleting service line claim field details----------------
                            IF @ServiceLineTypeID = 7
                                BEGIN
                                    DELETE CS
                                    FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                         JOIN CONTRACTSERVICELINECLAIMFIELDSELECTION AS CS ON CS.CONTRACTSERVICELINEID = CSL.CONTRACTSERVICELINEID
                                    WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                                      AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                                      AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                                      AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                                END;

                            ------------------deleting service line Table Selection details----------------
                            IF @ServiceLineTypeID = 8
BEGIN
                                    DELETE CS
                                    FROM CONTRACTSERVICELINEPAYMENTTYPES AS CSL
                                         JOIN CONTRACTSERVICELINETABLESELECTION AS CS ON CS.CONTRACTSERVICELINEID = CSL.CONTRACTSERVICELINEID
                                    WHERE ISNULL(CSL.CONTRACTID, 0) = @ContractID
                                      AND ISNULL(CSL.CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                                      AND ISNULL(CSL.SERVICELINETYPEID, 0) = @ServiceLineTypeID
                                      AND ISNULL(CSL.PAYMENTTYPEID, 0) = @PaymentTypeID;
                                END;

                            --Delete Entry from ContractServiceLinePaymentTypes Table
                            DELETE FROM DBO.CONTRACTSERVICELINEPAYMENTTYPES
                            WHERE ISNULL(CONTRACTID, 0) = @ContractID
                              AND ISNULL(CONTRACTSERVICETYPEID, 0) = @ContractServiceTypeID
                              AND ISNULL(SERVICELINETYPEID, 0) = @ServiceLineTypeID
                              AND ISNULL(PAYMENTTYPEID, 0) = @PaymentTypeID;
                        END;

                    /********************** Used for Contract Modelling report *************************/

                    EXEC AddEditServiceTypeFilterCodes
                         @ContractServiceTypeID,
                         @ContractID;
				
                    /********************** Used for Contract Modelling report *************************/



                END;

			 ------------------------Updating Contract GUID-----------------------------
			 EXEC [UpdateContractGUID]
				 @ContractID,
				 @ContractServiceTypeID;


            ---Insert Audit Log Information    
            IF @Contractid IS NULL
            OR @Contractid = 0
                BEGIN
                    EXEC InsertAuditLog
                         @UserName,
                         'Delete',
                         'Service Type',
                         @ClaimToolDesc,
                         @ContractServiceTypeId,
                         0;
                END;
            ELSE
                BEGIN
                    EXEC InsertAuditLog
                         @UserName,
                         'Delete',
                         'Contract - Modelling',
                         @ClaimToolDesc,
                         @Contractid,
                         1;
                END;
            COMMIT TRANSACTION @Transactionname;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;


