CREATE PROCEDURE dbo.AddEditServiceLinesInfo(
       @Contractservicelineid BIGINT,
       @Includedcode          VARCHAR(MAX),
       @Excludedcode          VARCHAR(MAX),
       @Contractid            BIGINT,
       @Servicelinetypeid     BIGINT,
       @Contractservicetypeid BIGINT,
       @UserName              VARCHAR(50))
AS

/****************************************************************************  
 *   Name         : AddEditServiceLinesInfo  
 *   Author       : mmachina  
 *   Date         : 24/Aug/2013  
 *   Module       : Adding/Editing ServiceLines   
 *   Description  : Adding/Editing ServiceLines information
 *****************************************************************************/

   BEGIN
        SET NOCOUNT ON;
        DECLARE @Currentdate     DATETIME = GETUTCDATE(),
                @Transactionname VARCHAR(100) = 'AddEditServiceLinesInfo',
                @Action          VARCHAR(10),
                @ClaimToolDesc   NVARCHAR(500);
        BEGIN TRY
            BEGIN TRAN @Transactionname;   
            ------------------------------Adding ServiceLines Information---------------------------------- 
            IF ISNULL(@Contractservicelineid, 0) = 0
                BEGIN
                    SET @Action = 'Add';
                    --Declare TmpTable for storing inserted ContractServiceLineId by using OUTPUT INSERTED
                    DECLARE @Tmptable TABLE( InsertedID BIGINT );
                    --Insert data into ContractServiceLines table
                    INSERT INTO dbo.ContractServiceLines
                           ( InsertDate,
                             UpdateDate,
                             IncludedCode,
                             ExcludedCode
                           )
                    OUTPUT INSERTED.ContractServiceLineID
                           INTO @Tmptable -- inserting id into @TmpTable
                    VALUES( @Currentdate, NULL, @Includedcode, @Excludedcode);

                    --Get inserted ContractServiceLineID and update variable
                    SELECT @Contractservicelineid = InsertedID
                    FROM @Tmptable;


                    --Insert data into [ContractServiceLinePaymentTypes] Table
                    INSERT INTO dbo.ContractServiceLinePaymentTypes
                           ( PaymentTypeDetailId,
                             InsertDate,
                             UpdateDate,
                             PaymentTypeId,
                             ContractServiceLineId,
                             ContractId,
                             ServiceLineTypeId,
                             ContractServiceTypeId
                           )
                    VALUES( NULL, @Currentdate, NULL, NULL, @Contractservicelineid, @Contractid, @Servicelinetypeid, @Contractservicetypeid );

				------------------------Updating Contract GUID-----------------------------
				 EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;


                    --Return Inserted @ContractServiceLineId
                    SELECT @Contractservicelineid;
                END;
            ELSE
                BEGIN
                    SET @Action = 'Modify';

				------------------------Updating Contract GUID-----------------------------

				IF NOT EXISTS
				(
				    SELECT 1
				    FROM dbo.ContractServiceLines SL
					    JOIN ContractServiceLinePaymentTypes CSL ON SL.ContractServiceLineId = CSL.ContractServiceLineId
				    WHERE ISNULL(CSL.ContractID, 0) = @Contractid
						AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
						AND ISNULL(CSL.ServiceLineTypeId, 0) = @Servicelinetypeid
						AND ((@Includedcode IS NULL
							AND IncludedCode IS NULL)
						OR (IncludedCode = @Includedcode))
						AND ((@Excludedcode IS NULL
							AND ExcludedCode IS NULL)
						OR (ExcludedCode = @Excludedcode))
				)
				    BEGIN
					   EXEC [UpdateContractGUID]
						   @Contractid,
						   @Contractservicetypeid;
				    END;

                    ------------------------Updating ServiceLines information-----------------------------
                    UPDATE dbo.ContractServiceLines
                      SET UpdateDate = @Currentdate,
                          IncludedCode = @Includedcode,
                          ExcludedCode = @Excludedcode
                    FROM dbo.ContractServiceLines SL
                         JOIN ContractServiceLinePaymentTypes CSL ON SL.ContractServiceLineId = CSL.ContractServiceLineId
                    WHERE ISNULL(CSL.ContractID, 0) = @Contractid
                      AND ISNULL(CSL.ContractServiceTypeID, 0) = @Contractservicetypeid
                      AND ISNULL(CSL.ServiceLineTypeId, 0) = @Servicelinetypeid;
                    SELECT @Contractservicelineid;
                END;

            /********************** Used for Contract Modelling report *************************/

            EXEC AddEditServiceTypeFilterCodes
                 @Contractservicetypeid,
                 @Contractid;
	
            /********************** Used for Contract Modelling report *************************/

            SELECT @ClaimToolDesc = FilterName + FilterValues
            FROM GetContractFiltersByID( @ContractID, @ContractServiceTypeID )
            WHERE ServiceLineTypeID = @ServiceLineTypeID;
          
            ---Insert Audit Log Information

            IF( @Action = 'Add' )
                BEGIN
                    SET @ClaimToolDesc = 'Added claim tool: ' + @ClaimToolDesc;
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

            --Check Any Transation happened than commit transation
            COMMIT TRANSACTION @Transactionname;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;