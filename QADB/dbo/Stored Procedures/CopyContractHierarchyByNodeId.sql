CREATE PROCEDURE [dbo].[CopyContractHierarchyByNodeId]  
(  
   @ContractID BIGINT  
  ,@NodeId BIGINT  
  ,@UserName VARCHAR(100)
  ,@ContractName VARCHAR(max)
)  
AS  
/****************************************************************************  
 *   Name         : CopyContractById  1,1
 *   Author       : Vishesh Bhawsar  
 *   Date         : 8/19/2013  
 *   Module       : Copy Contract  
 *   Description  : Insert duplicate Contract Information  
 *****************************************************************************/  
BEGIN  
    SET NOCOUNT ON;
    DECLARE @CurrentDate DATETIME = GETUTCDATE();

	 DECLARE @TransactionName VARCHAR(100) = 'CopyContractHierarchyByNodeId';
	 BEGIN TRY
	 BEGIN TRAN @TransactionName  

	 --Declare TmpTable for storing inserted NodeID by using OUTPUT INSERTED
	 DECLARE @TmpTable TABLE (InsertedID BIGINT) 
	
	 DECLARE @newNodeId BIGINT

	--Inserting Duplicate Contract Information  
	 INSERT INTO [dbo].[ContractHierarchy]  
	 ( ParentID,NodeText,IsPrimaryNode,FacilityID, UserName)
	 OUTPUT inserted.NodeID into @TmpTable
	 SELECT ParentId,@ContractName,NULL,FacilityID,@UserName
	 FROM ContractHierarchy 
	 WHERE NodeId = @NodeId
	 SELECT @newNodeId = InsertedID FROM @TmpTable

	 -- To remove nodeID from the table
	 DELETE FROM @TmpTable;

	 INSERT INTO @TmpTable
	 EXEC [dbo].[CopyContractById] @ContractID,@newNodeId, @UserName
	 
	 SELECT @newNodeId
	  
	 DECLARE @Duplicatecontractid BIGINT;
	 SELECT @Duplicatecontractid = InsertedID FROM @TmpTable

	 -- Move the contract to inactive status irrespective of the type of model whether it is primary or forecast.
	 EXEC MoveContractToDeActiveState @Duplicatecontractid


    --Check Any Transaction happened than commit transaction
	COMMIT TRANSACTION @TransactionName;            
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN @TransactionName;
		EXEC RaiseErrorInformation
	END CATCH
END