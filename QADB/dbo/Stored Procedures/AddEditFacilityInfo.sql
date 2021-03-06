CREATE PROCEDURE [dbo].[AddEditFacilityInfo] 
  (
	@FacilityID BIGINT, 
	@FacilityText VARCHAR(100)
  )
AS 
BEGIN
 

SET NOCOUNT ON;

	 DECLARE @TransactionName VARCHAR(100) = 'AddEditFacilityInfo';
	 BEGIN TRY
	 BEGIN TRAN @TransactionName 
	
	IF EXISTS ( SELECT FacilityID FROM ContractHierarchy WHERE FacilityID = @FacilityID )
		BEGIN
			-- If facility ID exist in database, then we are only updating the name. We could even put one more check to check the name and then do this update.
			UPDATE ContractHierarchy SET NodeText = @FacilityText WHERE FacilityID = @FacilityID AND ParentId = 0;
		END
		ELSE
		BEGIN
					DECLARE @NewParentID BIGINT
	
					DECLARE @Temp_Table TABLE ( InsertedID BIGINT )

				--  Adding hospital 

					INSERT INTO ContractHierarchy (ParentID,NodeText,IsPrimaryNode,FacilityID,UserName,IsDeleted)
					OUTPUT inserted.NodeID into @Temp_Table 
					SELECT ParentID,@FacilityText,IsPrimaryNode,@FacilityID,NULL,0
					FROM ContractHierarchyMasterData WHERE NodeID = 1

					SELECT @NewParentID = InsertedID FROM @Temp_Table

					DELETE FROM @Temp_Table

				--  Adding Primary Node 
					INSERT INTO ContractHierarchy (ParentID,NodeText,IsPrimaryNode,FacilityID,UserName,IsDeleted)
					OUTPUT inserted.NodeID into @Temp_Table 
					SELECT @NewParentID,NodeText,IsPrimaryNode,@FacilityID,NULL,0
					FROM ContractHierarchyMasterData WHERE NodeID = 2

					SELECT @NewParentID = InsertedID FROM @Temp_Table

					DELETE FROM @Temp_Table

				--  Adding Active Contracts Node
					DECLARE @ActiveNodeID BIGINT
					INSERT INTO ContractHierarchy (ParentID,NodeText,IsPrimaryNode,FacilityID,UserName,IsDeleted)
					OUTPUT inserted.NodeID into @Temp_Table 
					SELECT @NewParentID,NodeText,IsPrimaryNode,@FacilityID,NULL,0
					FROM ContractHierarchyMasterData WHERE NodeID = 3
					
					SELECT @ActiveNodeID = InsertedID FROM @Temp_Table
					
					DELETE FROM @Temp_Table

				--  Adding InActive Contracts Node

					DECLARE @INActiveNodeID BIGINT
					INSERT INTO ContractHierarchy (ParentID,NodeText,IsPrimaryNode,FacilityID,UserName,IsDeleted)
					OUTPUT inserted.NodeID into @Temp_Table 
					SELECT @NewParentID,NodeText,IsPrimaryNode,@FacilityID,NULL,0
					FROM ContractHierarchyMasterData WHERE NodeID = 4
	
					SELECT @INActiveNodeID = InsertedID FROM @Temp_Table

					DELETE FROM @Temp_Table

				--  Adding Current Contracts Node
					INSERT INTO ContractHierarchy (ParentID,NodeText,IsPrimaryNode,FacilityID,UserName,IsDeleted)
					OUTPUT inserted.NodeID into @Temp_Table 
					SELECT @ActiveNodeID,NodeText,IsPrimaryNode,@FacilityID,NULL,0
					FROM ContractHierarchyMasterData WHERE NodeID = 5
					DELETE FROM @Temp_Table


				--  Adding NonCurrent Contracts Node
					INSERT INTO ContractHierarchy (ParentID,NodeText,IsPrimaryNode,FacilityID,UserName,IsDeleted)
					OUTPUT inserted.NodeID into @Temp_Table 
					SELECT @ActiveNodeID,NodeText,IsPrimaryNode,@FacilityID,NULL,0
					FROM ContractHierarchyMasterData WHERE NodeID = 6
	
					SELECT @NewParentID = InsertedID FROM @Temp_Table

					DELETE FROM @Temp_Table

				--  Adding Current Contracts Node
					INSERT INTO ContractHierarchy (ParentID,NodeText,IsPrimaryNode,FacilityID,UserName,IsDeleted)
					OUTPUT inserted.NodeID into @Temp_Table 
					SELECT @INActiveNodeID,NodeText,IsPrimaryNode,@FacilityID,NULL,0
					FROM ContractHierarchyMasterData WHERE NodeID = 7

					DELETE FROM @Temp_Table

				--  Adding NonCurrent Contracts Node
					INSERT INTO ContractHierarchy (ParentID,NodeText,IsPrimaryNode,FacilityID,UserName,IsDeleted)
					OUTPUT inserted.NodeID into @Temp_Table 
					SELECT @INActiveNodeID,NodeText,IsPrimaryNode,@FacilityID,NULL,0
					FROM ContractHierarchyMasterData WHERE NodeID = 8
	
					DELETE FROM @Temp_Table
		END
	
   --Check Any Transation happened than commit transation
		COMMIT TRANSACTION @TransactionName;            

	END TRY
	BEGIN CATCH
		ROLLBACK TRAN @TransactionName;
		EXEC RaiseErrorInformation
	END CATCH
END
