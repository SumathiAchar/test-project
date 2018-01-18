CREATE PROCEDURE dbo.AddServiceLineClaimFields(
       @Xmlservicelineclaimfieldselection XML,
       @Servicelinetypeid                 BIGINT,
       @Contractid                        BIGINT,
       @Contractservicetypeid             BIGINT,
       @UserName                          VARCHAR(50))
AS  

/****************************************************************************  
*   Name         : AddServiceLineClaimFields  
*   Author       : Prasad   
*   Date         : 06/Sep/2013  
*   Module       : AddServiceLineClaimFields  
*   Description  : Insert list of ServiceLine ClaimFields Information into database  
*****************************************************************************/


   BEGIN
        DECLARE @Transactionname VARCHAR(100) = 'AddServiceLineClaimFields',
                @Action          VARCHAR(10),
                @ClaimToolDesc   NVARCHAR(1000);
        --DECLARE @UserName nvarchar(50)
        BEGIN TRY
            BEGIN TRAN @Transactionname;
            SET NOCOUNT ON;
            DECLARE @Contractservicelineid BIGINT;
            DECLARE @Numberrecords INT;
            DECLARE @Rowcount INT;
            DECLARE @Currentdate DATETIME = GETUTCDATE();
            DECLARE @Tempclaimsfield TABLE( RowID        INT IDENTITY(1, 1),
                                            ClaimFieldID BIGINT,
                                            OperatorID   INT,
                                            [Values]     VARCHAR(5000));
            INSERT INTO @Tempclaimsfield
                   ( ClaimFieldID,
                     OperatorID,
                     [Values]
                   )
                   SELECT V.X.value( './ClaimFieldId[1]', 'BIGINT' ) AS ClaimFieldID,
                          V.X.value( './Operator[1]', 'INT' ) AS OperatorID,
                          V.X.value( './Values[1]', 'VARCHAR(5000)' ) AS [Values]
                   FROM @Xmlservicelineclaimfieldselection.nodes( '//ContractServiceLineClaimFieldSelection' ) AS V( X );    
            -- Get the number of records in the temporary table  
            SET @Numberrecords = @@Rowcount;
            SET @Rowcount = 1;
            SELECT @UserName = UserName
            FROM Contracts
            WHERE ContractID = @Contractid;
            -- loop through all records in the temporary table  
            -- using the WHILE loop construct  
            WHILE @Rowcount <= @Numberrecords
                BEGIN  
                    --Declare TmpTable for storing inserted ContractServiceLineID by using OUTPUT INSERTED  
                    DECLARE @Tmptable TABLE( InsertedID BIGINT );  
                    --Insert ContractServiceLineClaimFieldSelection informations  
                    INSERT INTO dbo.ContractServiceLineClaimFieldSelection
                           ( InsertDate,
                             UpdateDate,
                             ClaimFieldID,
                             OperatorID,
                             [Values]
                           )
                    OUTPUT INSERTED.ContractServiceLineID
                           INTO @Tmptable -- inserting id into @TmpTable  
                           SELECT @Currentdate,
                                  NULL,
                                  ClaimFieldID,
                                  OperatorID,
                                  [Values]
                           FROM @Tempclaimsfield
                           WHERE RowID = @Rowcount;  
                    --Get inserted PaymentTypeDetailID and update variable  
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
                    VALUES( NULL, @Currentdate, NULL, NULL, @Contractservicelineid, @Contractid, @Servicelinetypeid, @Contractservicetypeid );  

                    --Increment RowCount variable  
                    SET @Rowcount = @Rowcount + 1;
                END;
            DELETE @Tempclaimsfield;
            SELECT @@Trancount;  

		  --------------Update Contract GUID---------------------
		  EXEC UpdateContractGUID
			  @Contractid,
			  @ContractServiceTypeId;

            /********************** Used for Contract Modelling report *************************/

            EXEC AddEditServiceTypeFilterCodes
                 @Contractservicetypeid,
                 @Contractid;
	
            /********************** Used for Contract Modelling report *************************/

            ---Insert Audit Log Information
           SELECT @ClaimToolDesc = FilterName + FilterValues
            FROM GetContractFiltersByID( @ContractID, @ContractServiceTypeID )
            WHERE ServiceLineTypeID = @ServiceLineTypeID;
          
            SET @ClaimToolDesc = 'Added claim tool : ' + @ClaimToolDesc;

		  IF @Contractid IS NULL OR @Contractid=0
		  BEGIN
		   EXEC InsertAuditLog
                 @UserName,
                 'Modify',
			  'Service Type',
                 @ClaimToolDesc,
                 @ContractServiceTypeID,
                 0;
		  END
		  ELSE
		  BEGIN
            EXEC InsertAuditLog
                 @UserName,
                 'Modify',
                 'Contract - Modelling',
                 @ClaimToolDesc,
                 @Contractid,
                 1;
		  END
            --Check Any Transation happened than commit transation  

            COMMIT TRANSACTION @Transactionname;
        END TRY
        BEGIN CATCH  
            --SELECT ERROR_NUMBER() AS ErrorNumber ,ERROR_MESSAGE() AS ErrorMessage;   
            ROLLBACK TRAN @Transactionname;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;