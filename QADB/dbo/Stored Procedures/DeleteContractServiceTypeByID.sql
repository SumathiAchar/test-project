CREATE PROCEDURE [dbo].[DeleteContractServiceTypeByID]  
(  
    @ContractServiceTypeId BIGINT,
    @UserName VARCHAR(50)  
)  
AS  
/****************************************************************************  
 *   Name         : DeleteContractServiceTypeById 12227  
 *   Author       : Vishesh
 *   Date         : 03/Sep/2013  
 *   Module       : Delete Contract Service Type By ContractServiceTypeId  
 *   Description  : Delete Contract Service Types Information from database  
 *****************************************************************************/  
BEGIN  
  
  	 DECLARE @TransactionName VARCHAR(100) = 'DeleteContractServiceTypeById';
	 BEGIN TRY
	 BEGIN TRAN @TransactionName 
	
		 -- Insert into Audit log

		 EXEC InsertAuditLog
                 @UserName,
                 'Delete',
                 'Service Type',
                 Null,
                 @ContractServiceTypeID,
                 0;      
		IF EXISTS(SELECT 1 FROM ContractServiceLinePaymentTypes WHERE ContractServiceTypeID=@ContractServiceTypeID)
		   --Updating Contract GUID
		   EXEC [UpdateContractGUID]
			       NULL,
				  @ContractServiceTypeID;

		DECLARE @ContractID BIGINT 
		SELECT @ContractID = ContractID FROM 
		ContractServiceTypes WHERE ContractServiceTypeID = @ContractServiceTypeId

		UPDATE ContractServiceTypes SET IsDeleted = 1,IsCarveOut = NULL 
		WHERE ContractServiceTypeID = @ContractServiceTypeId

		IF NOT EXISTS ( SELECT ContractID FROM ContractServiceTypes 
					WHERE ContractID = @ContractID AND IsDeleted = 0)
					BEGIN
						EXEC [dbo].[MoveContractToDeActiveState] @ContractID
					END

	  --Check Any Transation happened than commit transation
			COMMIT TRANSACTION @TransactionName;      
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN @TransactionName;
		EXEC RaiseErrorInformation
	END CATCH
END

GO
