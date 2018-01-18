CREATE PROCEDURE [dbo].[AddEditTableSelectionDetails](
       @Contractid                  BIGINT,
       @Contractservicetypeid       BIGINT,
       @ContractServiceLineTableId  BIGINT,
       @Xmlservicelineclaimandtable XML,
       @UserName                    VARCHAR(50))
AS  

/****************************************************************************    
 *   Name         : AddEditTableSelectionDetails  
 *   Author       : Raj  
 *   Date         : 10/Sep/2013  
 *   Module       : Update table selection Details 
 *   Description  : Update table Selection Details 
 
 *****************************************************************************/

  BEGIN
        DECLARE @TransactionName VARCHAR(100) = 'AddEditTableSelectionDetails',
                @Action          VARCHAR(10),
                @ClaimToolDesc   NVARCHAR(1000);
        DECLARE @Currentdate DATETIME = GETUTCDATE();
        BEGIN TRY
            BEGIN TRAN @TransactionName;
            SET NOCOUNT ON;
            DECLARE @Contractservicelineid BIGINT;
            DECLARE @Numberrecords INT;
            DECLARE @Rowcount INT;

		  DECLARE @TempTableSelectionChanges TABLE( RowID           INT IDENTITY(1, 1),
                                               ClaimFieldId    BIGINT,
                                               ClaimFieldDocId BIGINT,
                                               Operator        INT);
		  DECLARE @TempTableSelection TABLE( RowID           INT IDENTITY(1, 1),
                                               ClaimFieldId    BIGINT,
                                               ClaimFieldDocId BIGINT,
                                               Operator        INT);
            INSERT INTO @TempTableSelection
                   ( ClaimFieldId,
                     ClaimFieldDocId,
                     Operator
                   )
                   SELECT V.X.value( './ClaimFieldId[1]', 'BIGINT' ) AS ClaimFieldId,
                          V.X.value( './ClaimFieldDocId[1]', 'BIGINT' ) AS ClaimFieldDocId,
                          V.X.value( './Operator[1]', 'BIGINT' ) AS Operator
                   FROM @Xmlservicelineclaimandtable.nodes( '//ContractServiceLineTableSelection' ) AS V( X );
            SET @Numberrecords = @@Rowcount;

            IF( @ContractServiceLineTableId > 0 )
                BEGIN

				INSERT INTO @TempTableSelectionChanges
				SELECT 
				    ClaimFieldId,
				    ClaimFieldDocId,
				    Operator
				FROM @TempTableSelection
				EXCEPT
				SELECT 
				    ClaimFieldId,
				    ClaimFieldDocId,
				    Operator
				FROM dbo.ContractServiceLineTableSelection CSTS
					   JOIN ContractServiceLinePaymentTypes CSL ON CSL.ContractServiceLineID = CSTS.ContractServiceLineID
				WHERE ISNULL(CSL.ContractID, 0) = ISNULL(@Contractid, 0)
				    AND ISNULL(CSL.ContractServiceTypeID, 0) = ISNULL(@Contractservicetypeid, 0)
				    AND ISNULL(CSL.ServiceLineTypeID, 0) = 8; 

				INSERT INTO @TempTableSelectionChanges
				SELECT 
				    ClaimFieldId,
				    ClaimFieldDocId,
				    Operator
				FROM dbo.ContractServiceLineTableSelection CSTS
					   JOIN ContractServiceLinePaymentTypes CSL ON CSL.ContractServiceLineID = CSTS.ContractServiceLineID
				WHERE ISNULL(CSL.ContractID, 0) = ISNULL(@Contractid, 0)
				    AND ISNULL(CSL.ContractServiceTypeID, 0) = ISNULL(@Contractservicetypeid, 0)
				    AND ISNULL(CSL.ServiceLineTypeID, 0) = 8
				EXCEPT
				SELECT 
				    ClaimFieldId,
				    ClaimFieldDocId,
				    Operator
				FROM @TempTableSelection
				
				IF((SELECT COUNT(*) FROM @TempTableSelectionChanges)>0)
				BEGIN
					   --------------Update Contract GUID---------------------
				EXEC UpdateContractGUID
					   @Contractid,
					   @ContractServiceTypeId;
				END

                    --------Deleting record from ContractServiceLineTableSelection table---------------------------------------------- 
                    SET @Action = 'Modify';
                    DELETE ContractServiceLineTableSelection
                    FROM dbo.ContractServiceLineTableSelection CSTS
                         JOIN ContractServiceLinePaymentTypes CSL ON CSL.ContractServiceLineID = CSTS.ContractServiceLineID
                    WHERE ISNULL(CSL.ContractID, 0) = ISNULL(@Contractid, 0)
                      AND ISNULL(CSL.ContractServiceTypeID, 0) = ISNULL(@Contractservicetypeid, 0)
                      AND ISNULL(CSL.ServiceLineTypeID, 0) = 8;  

                    ------------------Deleting record from ContractServiceLinePaymentTypes table----------------------------------------------  
                    DELETE ContractServiceLinePaymentTypes
                    FROM dbo.ContractServiceLinePaymentTypes CSL
                    WHERE ISNULL(CSL.ContractID, 0) = ISNULL(@Contractid, 0)
                      AND ISNULL(CSL.ContractServiceTypeID, 0) = ISNULL(@Contractservicetypeid, 0)
                      AND ISNULL(CSL.ServiceLineTypeID, 0) = 8;
                END;
            ELSE
                BEGIN
			 --------------------Update Contract GUID-----------------------
				EXEC UpdateContractGUID
				@Contractid,
				@ContractServiceTypeId;

                    SET @Action = 'Add';
                END;
            ---------------------Inserting data into ContractServiceLineTableSelection,ContractServiceLinePaymentTypes tables------------------------------  
          
            SET @Rowcount = 1;
            WHILE @Rowcount <= @Numberrecords
                BEGIN

                    --Declare TmpTable for storing inserted ContractServiceLineID by using OUTPUT INSERTED
                    DECLARE @Tmptable TABLE( InsertedID BIGINT );

                    --Insert ContractServiceLineTableSelection informations
                    INSERT INTO dbo.ContractServiceLineTableSelection
                           ( InsertDate,
                             UpdateDate,
                             ClaimFieldId,
                             ClaimFieldDocId,
                             Operator
                           )
                    OUTPUT INSERTED.ContractServiceLineID
                           INTO @Tmptable
                           SELECT @Currentdate,
                                  NULL,
                                  ClaimFieldId,
                                  ClaimFieldDocId,
                                  Operator
                           FROM @TempTableSelection
                           WHERE RowID = @Rowcount;
                    SELECT @Contractservicelineid = InsertedID
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
                    VALUES( NULL, @Currentdate, NULL, NULL, @Contractservicelineid, @Contractid, 8, @Contractservicetypeid );
                    SET @Rowcount = @Rowcount + 1;
                END;

            /********************** Used for Contract Modelling report *************************/

            EXEC AddEditServiceTypeFilterCodes
                 @Contractservicetypeid,
                 @Contractid;
	
		
		
            ---Insert Audit Log Information

            SELECT @ClaimToolDesc = dbo.GetLatestServiceTypeCodesById( @Contractid, @Contractservicetypeid );
            IF( @Action = 'Add' )
                BEGIN
                    SET @ClaimToolDesc = 'Added claim tool : ' + @ClaimToolDesc;
                END;
           IF @Contractid IS NULL
            OR @Contractid = 0
                BEGIN
                    EXEC InsertAuditLog
                         @UserName,
                         'Modify',
                         'Service Type',
                         @ClaimToolDesc,
                         @ContractServiceTypeID,
                         0;
                END;
            ELSE
                BEGIN
                    EXEC InsertAuditLog
                         @UserName,
                         'Modify',
                         'Contract - Modelling',
                         @ClaimToolDesc,
                         @Contractid,
                         1;
                END;
            SELECT @@Trancount;
            --Check Any Transation happened than commit transation
            COMMIT TRANSACTION @TransactionName;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;