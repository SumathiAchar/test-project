CREATE PROCEDURE [dbo].[AddEditContractServiceTypes](
       @ContractServiceTypeId   BIGINT,
       @ContractId              BIGINT,
       @ContractServiceTypeName VARCHAR(100),
       @Notes                   VARCHAR(MAX),
       @IsCarveOut              BIT,
       @UserName                VARCHAR(50))
AS  

/****************************************************************************  
 *   Name         : AddEditContractServiceTypes  
 *   Author       : Vishesh  
 *   Date         : 29/Aug/2013  
 *   Module       : Add/Edit Contract Service types  
 *   Description  : Insert/Update Contract Service Types Information into database  
 *****************************************************************************/

    BEGIN
        SET NOCOUNT ON;
        DECLARE @CurrentDate     DATETIME = GETUTCDATE(),
                @Description     NVARCHAR(500) = 'Notes: ',
                @Action          VARCHAR(10),
                @TransactionName VARCHAR(100) = 'AddEditContractServiceTypes';
        BEGIN TRY
            BEGIN TRAN @TransactionName;
            IF ISNULL(@ContractServiceTypeId, 0) = 0
                BEGIN
                    SET @Action = 'Add';
                    --Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED
                    DECLARE @TmpTable TABLE( InsertedID BIGINT );

                    --Insert ContractServiceTypes informations  
                    INSERT INTO [dbo].[ContractServiceTypes]
                           ( [InsertDate],
                             [UpdateDate],
                             [ContractId],
                             [ContractServiceTypeName],
                             [Notes],
                             [IsCarveOut]
                           )
                    OUTPUT INSERTED.ContractServiceTypeID
                           INTO @TmpTable -- inserting id into @TmpTable
                    VALUES( @CurrentDate, NULL, @ContractId, @ContractServiceTypeName, @Notes, @IsCarveOut );
                    SELECT @ContractServiceTypeID = InsertedID
                    FROM @TmpTable;
                END;
            ELSE
                BEGIN
                    --Declare TmpTable for storing inserted PaymentTypeDetailID by using OUTPUT INSERTED
                    SET @Action = 'Modify';
                    -- If ContractServiceTypes is not having any row for the current contract then we need to move it active state.
                    IF NOT EXISTS( SELECT ContractServiceTypeID
                                   FROM ContractServiceTypes
                                   WHERE ContractID = @ContractId
                                     AND IsDeleted = 0 )
                        BEGIN
                            EXEC MoveContractToActiveState
                                 @ContractId;
                        END;
				    
				      --------------Update Contract GUID If IsCarveOut Changed---------------------
				    IF(
					 (
						SELECT [IsCarveOut]
						FROM [ContractServiceTypes]
						WHERE ContractId = @ContractId
							 AND ContractServiceTypeID = @ContractServiceTypeID
					 ) <> @IsCarveOut)
					   BEGIN
						 IF EXISTS(SELECT 1 FROM ContractServiceLinePaymentTypes WHERE ContractServiceTypeID=@ContractServiceTypeId)
						 BEGIN
						  --------------Update Contract GUID---------------------
						  EXEC UpdateContractGUID
							  @Contractid,
							  @ContractServiceTypeId;
						  END
					   END;
                    --UPDATE ContractServiceTypes informations  
                    UPDATE [dbo].[ContractServiceTypes]
                      SET [UpdateDate] = @CurrentDate,
                          [ContractServiceTypeName] = @ContractServiceTypeName,
                          [Notes] = @Notes,
                          [IsCarveOut] = @IsCarveOut
                    WHERE ContractId = @ContractId
                      AND ContractServiceTypeID = @ContractServiceTypeID;
                   
                END;
		
		 SELECT @ContractServiceTypeID;
            --Insert Audit Log

            IF @Notes IS NOT NULL
                BEGIN
                    SET @Description = @Description + @Notes;
                END;
            IF @IsCarveOut = 1
                BEGIN
                    SET @Description = @Description + ' , Carve Out ';
                END;
          
		 EXEC InsertAuditLog
                         @UserName,
                         @Action,
                         'Service Type',
                         @Description,
                         @ContractServiceTypeID,
                         0;
              
       --Check Any Transaction happened than commit transaction
	    COMMIT TRANSACTION @TransactionName;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;
GO