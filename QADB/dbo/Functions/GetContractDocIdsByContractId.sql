-- =============================================
-- Author:		Manjunath Cycle
-- Create date: 10/25/2013
-- Description:	Gettting contract documentids based on the contractid
-- =============================================
CREATE FUNCTION [dbo].[GetContractDocIdsByContractId](
                @Contractidlist VARCHAR(MAX))
RETURNS @Claimfielddocid TABLE( ClaimFieldDocID BIGINT,
                                ContractId      BIGINT )
AS
     BEGIN
     DECLARE @Count INT;
     DECLARE @PaymentType INT;
     DECLARE @ServiceLineType INT;
     DECLARE @ContractID BIGINT;
     DECLARE @ContractCount INT;
     DECLARE @PaymentContractId BIGINT;
     DECLARE @ServiceLineContractId BIGINT;
     DECLARE @ContractIDs TABLE( RowID      INT IDENTITY(1, 1)
                                                PRIMARY KEY,
                                 ContractID BIGINT );
     DECLARE @Temp TABLE( RowID                 INT IDENTITY(1, 1)
                                                    PRIMARY KEY,
                          PaymentTypeDetailID   BIGINT,
                          PaymentTypeID         INT,
                          ContractServiceLineID BIGINT,
                          ServiceLineTypeID     INT,
                          ContractId            BIGINT );
     --Getting splitted contractIds
     INSERT INTO @Contractids
            ( ContractID
            )
            SELECT ITEMS
            FROM dbo.Split( @Contractidlist, ',' );
     SET @ContractCount = @@ROWCOUNT;
								
     --Looping through each contractid
     WHILE @ContractCount > 0
         BEGIN
             SET @ContractID = ( SELECT ContractID
                                 FROM @ContractIDs
                                 WHERE RowID = @ContractCount );
		  --For service level 
             INSERT INTO @Temp
                    ( PaymentTypeDetailID,
                      PaymentTypeID,
                      ContractId
                    )
                    SELECT PaymentTypeDetailID,
                           PaymentTypeID,
                           @ContractID
                    FROM dbo.ContractServiceLinePaymentTypes WITH ( NOLOCK )
                    WHERE PaymentTypeID IN( 1, 2, 3, 14 )
                      AND ContractServiceTypeID IN( --exists can be used here
                          SELECT ContractServiceTypeID
                          FROM dbo.ContractServiceTypes WITH ( NOLOCK )
                          WHERE ContractID = @ContractID
                            AND IsDeleted = 0 );
             INSERT INTO @Temp
                    ( ContractServiceLineID,
                      ServiceLineTypeID,
                      ContractId
                    )
                    SELECT ContractServiceLineID,
                           ServiceLineTypeID,
                           @ContractID
                    FROM dbo.ContractServiceLinePaymentTypes WITH ( NOLOCK )
                    WHERE ServiceLineTypeID = 8
                      AND ContractServiceTypeID IN( --exists can be used here
                          SELECT ContractServiceTypeID
                          FROM dbo.ContractServiceTypes WITH ( NOLOCK )
                          WHERE ContractID = @ContractID
                            AND IsDeleted = 0 );
		  --For Contract Level
		  INSERT INTO @Temp
                    ( ContractServiceLineID,
                      ServiceLineTypeID,
                      ContractId
                    )
                    SELECT ContractServiceLineID,
                           ServiceLineTypeID,
                           @ContractID
                    FROM dbo.ContractServiceLinePaymentTypes WITH ( NOLOCK )
                    WHERE ServiceLineTypeID = 8
                                    AND ContractID = @ContractID
             SET @ContractCount = @ContractCount - 1;
         END;
             SELECT @Count = COUNT(*)
             FROM @Temp;
             WHILE @Count > 0
                 BEGIN
                     SELECT @PaymentType = PaymentTypeID,
                            @PaymentContractId = ContractId
                     FROM @Temp
                     WHERE RowID = @Count;
                     SELECT @ServiceLineType = ServiceLineTypeID,
                            @ServiceLineContractId = ContractId
                     FROM @Temp
                     WHERE RowID = @Count;
                     IF @Paymenttype IS NOT NULL
                         BEGIN
                             IF @PaymentType = 1
                                 BEGIN
                                     INSERT INTO @ClaimFieldDocID
                                            ( ClaimFieldDocID,
                                              ContractId
                                            )
                                            SELECT ClaimFieldDocID,
                                                   @PaymentContractId
                                            FROM dbo.PaymentTypeASCFeeSchedules WITH ( NOLOCK )
                                            WHERE PaymentTypeDetailID = ( 
                                                                          SELECT PaymentTypeDetailID
                                                                          FROM @Temp
                                                                          WHERE RowID = @Count );
                                 END;
                             ELSE
                             IF @PaymentType = 2
                                 BEGIN
                                     INSERT INTO @ClaimFieldDocID
                                            ( ClaimFieldDocID,
                                              ContractId
                                            )
                                            SELECT ClaimFieldDocID,
                                                   @PaymentContractId
                                            FROM DBO.PaymentTypeDRGPayment WITH ( NOLOCK )
                                            WHERE PaymentTypeDetailID = ( 
                                                                          SELECT PaymentTypeDetailID
                                                                          FROM @Temp
                                                                          WHERE RowID = @Count );
                                 END;
                             ELSE
                             IF @PaymentType = 3
                                 BEGIN
                                     INSERT INTO @ClaimFieldDocID
                                            ( ClaimFieldDocID,
                                              ContractId
                                            )
                                            SELECT ClaimFieldDocID,
                                                   @PaymentContractId
                                            FROM dbo.PaymentTypeFeeSchedules WITH ( NOLOCK )
                                            WHERE PAYMENTTYPEDETAILID = ( 
                                                                          SELECT PaymentTypeDetailID
                                                                          FROM @Temp
                                                                          WHERE RowID = @Count );
                                 END;
                             ELSE
                             IF @PaymentType = 14
                                 BEGIN
                                     INSERT INTO @ClaimFieldDocID
                                            ( ClaimFieldDocID,
                                              ContractId
                                            )
                                            SELECT ClaimFieldDocID,
                                                   @PaymentContractId
                                            FROM dbo.PaymentTypeCustomTable WITH ( NOLOCK )
                                            WHERE PAYMENTTYPEDETAILID = ( 
                                                                          SELECT PaymentTypeDetailID
                                                                          FROM @Temp
                                                                          WHERE RowID = @Count );
                                 END;;;
                         END;
                     ELSE
                         BEGIN
                             IF @ServiceLineType = 8
                                 BEGIN
                                     INSERT INTO @ClaimFieldDocID
                                            ( ClaimFieldDocID,
                                              ContractId
                                            )
                                            SELECT ClaimFieldDocId,
                                                   @ServiceLineContractId
                                            FROM dbo.ContractServiceLineTableSelection WITH ( NOLOCK )
                                            WHERE ContractServiceLineID = ( 
                                                                            SELECT ContractServiceLineID
                                                                            FROM @Temp
                                                                            WHERE RowID = @Count );
                                 END;
                         END;
                     SET @Count = @Count - 1;
                 END;
                     RETURN;
                     END;
GO