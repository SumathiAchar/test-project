CREATE PROCEDURE [dbo].[AddEditPaymentTypePerDiem]
(
   @XmlPaymentTypePerDiemData XML,
   @PaymentTypeID BIGINT,
   @ContractID BIGINT,
   @ContractServiceTypeID BIGINT,
   @PaymentTypeDetailsId BIGINT,
   @UserName VARCHAR(50)
)

AS

/****************************************************************************

 *   Name         : AddPaymentTypePerDiem

 *   Author       : Raj

 *   Date         : 23/Aug/2013

 *   Module       : AddPaymentTypePerDiem

 *   Description  : Insert list of Payment Type Per Diem Information into database

 *****************************************************************************/
 BEGIN
         SET NOCOUNT ON;
         DECLARE @TransactionName VARCHAR(100)= 'AddEditPaymentTypePerDiem', @PaymentToolDesc NVARCHAR(1000), @PaymentTypeDetailID BIGINT, @NumberRecords INT, @RowCount INT, @CurrentDate DATETIME= GETUTCDATE();
         DECLARE @TempPerDiemChanges TABLE
         (RowID    INT IDENTITY(1, 1),
          Rate     FLOAT,
          DaysFrom INT,
          DaysTo   INT
         );
         DECLARE @TempPerDiem TABLE
         (RowID    INT IDENTITY(1, 1),
          Rate     FLOAT,
          DaysFrom INT,
          DaysTo   INT
         );
         BEGIN TRY
             BEGIN TRAN @TransactionName;
             INSERT INTO @TempPerDiem
             (Rate,
              DaysFrom,
              DaysTo
             )
                    SELECT V.X.value('./Rate[1]', 'FLOAT') AS Rate,
                           V.X.value('./DaysFrom[1]', 'INT') AS DaysFrom,
                           V.X.value('./DaysTo[1]', 'INT') AS DaysTo
                    FROM @XmlPaymentTypePerDiemData.nodes('//PaymentTypePerDiem/PerDiemSelections/PerDiemSelection') AS V(X);  

             -- Get the number of records in the temporary table
             SET @NumberRecords = @@ROWCOUNT;
             IF @PaymentTypeDetailsId > 0
                 BEGIN

                     -----Checking table values is changed or not-------------
                     INSERT INTO @TempPerDiemChanges
                            SELECT Rate,
                                   DaysFrom,
                                   DaysTo
                            FROM @TempPerDiem
                            EXCEPT
                            SELECT Rate,
                                   DaysFrom,
                                   DaysTo
                            FROM [dbo].[PaymentTypePerDiem] PTPD
                                 JOIN ContractServiceLinePaymentTypes CSL ON CSL.PaymentTypeDetailId = PTPD.PaymentTypeDetailId
                            WHERE ISNULL(CSL.ContractID, 0) = ISNULL(@ContractID, 0)
                                  AND ISNULL(CSL.ContractServiceTypeID, 0) = ISNULL(@ContractServiceTypeID, 0)
                                  AND ISNULL(CSL.PaymentTypeID, 0) = ISNULL(@PaymentTypeID, 0);

                     -----Checking table values is changed or not-------------
                     INSERT INTO @TempPerDiemChanges
                            SELECT Rate,
                                   DaysFrom,
                                   DaysTo
                            FROM [dbo].[PaymentTypePerDiem] PTPD
                                 JOIN ContractServiceLinePaymentTypes CSL ON CSL.PaymentTypeDetailId = PTPD.PaymentTypeDetailId
                            WHERE ISNULL(CSL.ContractID, 0) = ISNULL(@ContractID, 0)
                                  AND ISNULL(CSL.ContractServiceTypeID, 0) = ISNULL(@ContractServiceTypeID, 0)
                                  AND ISNULL(CSL.PaymentTypeID, 0) = ISNULL(@PaymentTypeID, 0)
                            EXCEPT
                            SELECT Rate,
                                   DaysFrom,
                                   DaysTo
                            FROM @TempPerDiem;
                     IF(
                       (
                           SELECT COUNT(*)
                           FROM @TempPerDiemChanges
                       ) > 0)
                         BEGIN
                             --------------Update Contract GUID---------------------
                             EXEC UpdateContractGUID
                                  @Contractid,
                                  @ContractServiceTypeId;
                         END;


                     --------------------------Deleting record from PaymentTypePerDiem table---------------------------------------------- 
                     DELETE PaymentTypePerDiem
                     FROM [dbo].[PaymentTypePerDiem] PTPD
                          JOIN ContractServiceLinePaymentTypes CSL ON CSL.PaymentTypeDetailId = PTPD.PaymentTypeDetailId
                     WHERE ISNULL(CSL.ContractID, 0) = ISNULL(@ContractID, 0)
                           AND ISNULL(CSL.ContractServiceTypeID, 0) = ISNULL(@ContractServiceTypeID, 0)
                           AND ISNULL(CSL.PaymentTypeID, 0) = ISNULL(@PaymentTypeID, 0);  

                     ------------------Deleting record from ContractServiceLinePaymentTypes table----------------------------------------------  
                     DELETE ContractServiceLinePaymentTypes
                     FROM [dbo].[ContractServiceLinePaymentTypes] CSL
                     WHERE ISNULL(CSL.ContractID, 0) = ISNULL(@ContractID, 0)
                           AND ISNULL(CSL.ContractServiceTypeID, 0) = ISNULL(@ContractServiceTypeID, 0)
                           AND ISNULL(CSL.PaymentTypeID, 0) = ISNULL(@PaymentTypeID, 0);
                 END;
             ELSE
                 BEGIN
                     --------------------Update Contract GUID-----------------------
                     EXEC UpdateContractGUID
                          @Contractid,
                          @ContractServiceTypeId;
                 END;


             ---------------------Inserting data into PerDiem,ContractServiceLinePaymentTypes tables------------------------------  

             SET @RowCount = 1;
             WHILE @RowCount <= @NumberRecords
                 BEGIN
                     --Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED
                     DECLARE @TmpTable TABLE(InsertedID BIGINT);
                     --Insert PaymentTypePerCase informations
                     INSERT INTO [dbo].[PaymentTypePerDiem]
                     ([InsertDate],
                      [UpdateDate],
                      [Rate],
                      [DaysFrom],
                      [DaysTo]
                     )
                     OUTPUT INSERTED.PaymentTypeDetailID
                            INTO @TmpTable -- inserting id into @TmpTable
                            SELECT @CurrentDate,
                                   NULL,
                                   Rate,
                                   DaysFrom,
                                   DaysTo
                            FROM @TempPerDiem
                            WHERE RowID = @RowCount;
                     --Get inserted PaymentTypeDetailID and update variable
                     SELECT @PaymentTypeDetailID = InsertedID
                     FROM @TmpTable;
                     --Insert data into [ContractServiceLinePaymentTypes] Tables
                     INSERT INTO [dbo].[ContractServiceLinePaymentTypes]
                     ([PaymentTypeDetailID],
                      [InsertDate],
                      [UpdateDate],
                      [PaymentTypeID],
                      [ContractServiceLineID],
                      [ContractID],
                      [ServiceLineTypeID],
                      [ContractServiceTypeID]
                     )
                     VALUES
                     (@PaymentTypeDetailID,
                      @CurrentDate,
                      NULL,
                      @PaymentTypeID,
                      NULL,
                      @ContractID,
                      NULL,
                      @ContractServiceTypeID
                     );
                     SET @RowCount = @RowCount + 1;
                 END;
             DELETE @TempPerDiem;	
		
             /********************** Used for Contract Modelling report *************************/

             EXEC AddEditPaymentTypeFilterCodes
                  @ContractServiceTypeID,
                  @ContractID;
		
             /********************** Used for Contract Modelling report *************************/

             SELECT @@TRANCOUNT;
             --Check Any Transation happened than commit transation
             COMMIT TRANSACTION @TransactionName;
             SELECT @PaymentToolDesc = FilterName + FilterValues
             FROM GetContractFiltersByID(@ContractID, @ContractServiceTypeID)
             WHERE PaymentTypeID = @PaymentTypeID;
             IF @PaymentTypeDetailsId = 0
                 BEGIN
                     SET @PaymentToolDesc = 'Added Payment tool:'+@PaymentToolDesc;
                 END;
             EXEC InsertAuditLog
                  @UserName,
                  'Modify',
                  'Service Type',
                  @PaymentToolDesc,
                  @ContractServiceTypeID,
                  0;
         END TRY
         BEGIN CATCH
             ROLLBACK TRAN @TransactionName;
             EXEC RaiseErrorInformation;
         END CATCH;
     END;
