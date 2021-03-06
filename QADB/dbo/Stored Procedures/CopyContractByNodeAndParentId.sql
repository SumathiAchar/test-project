CREATE PROCEDURE [dbo].[CopyContractByNodeAndParentId](
      @NodeID           BIGINT,
      @ParentId         BIGINT,
      @UserName         VARCHAR(100),
      @NodeText         VARCHAR(500),
      @LoggedInUserName VARCHAR(50))
AS

/****************************************************************************
 *   Name         : CopyContractByNodeAndParentId
 *   Author       : Vishesh
 *   Date         : 8/12/2013
 *   Module       : Copy Contract
 *   Description  : Insert duplicate Contract Information
 --Exec CopyContractByNodeAndParentId 127515,69645,null,1, 'S8'
 *****************************************************************************/

    BEGIN
	
        --For getting inserted node id
        DECLARE @InsertedModelNodeID BIGINT;
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SET NOCOUNT ON;
        DECLARE @CurrentDate DATETIME = GETUTCDATE();
        DECLARE @TransactionName VARCHAR(100) = 'CopyContractByNodeAndParentId';
        BEGIN TRY
            BEGIN TRAN @TransactionName; 

            --For getting inserted contractId
            DECLARE @InsertedContractTable TABLE( InsertedContractID BIGINT );

            -- Temp table needed to keep data of models which are newly added and later we are deleting this temp table in last 
            DECLARE @temp_ContractHierarchyDataTable TABLE( NodeId      BIGINT,
                                                            ParentId    BIGINT,
                                                            NodeText    VARCHAR(MAX),
                                                            NewNodeID   BIGINT,
                                                            NewParentID BIGINT,
                                                            UserName    VARCHAR(500));
            SET @NodeID = @NodeID; -- This statement is require to be here else SP will give error. This line will not make difference in SP.

            WITH ReportData
                 AS (
                 SELECT ContractHierarchy.NodeId,
                        ContractHierarchy.NodeText,
                        ContractHierarchy.ParentId,
                        ContractHierarchy.UserName
                 FROM ContractHierarchy
                 WHERE ContractHierarchy.ParentId = @NodeID
                 UNION ALL
                 SELECT ContractHierarchy.NodeId,
                        ContractHierarchy.NodeText,
                        ContractHierarchy.ParentId,
                        ContractHierarchy.UserName
                 FROM ReportData
                      INNER JOIN ContractHierarchy ON ContractHierarchy.ParentId = ReportData.NodeId
                 WHERE ContractHierarchy.IsDeleted = 0 )
                 INSERT INTO @temp_ContractHierarchyDataTable
                        ( NodeId,
                          ParentId,
                          NodeText,
                          UserName
                        )
                        SELECT NodeId,
                               ParentId,
                               NodeText,
                               UserName
                        FROM ReportData;
          
            -- ReportData is having all the modules which are need to be added into database. This contains information about the node and parent nodes.
            -- We need to do recursive call so for that I am taking one by one each node
            DECLARE @LastParentId BIGINT;
            DECLARE @TmpTable_CH TABLE( InsertedID BIGINT );
            INSERT INTO ContractHierarchy
                   ( NodeText,
                     ParentId,
                     FacilityID,
                     UserName,
                     IsPrimaryNode
                   )
            OUTPUT INSERTED.NodeID
                   INTO @TmpTable_CH -- inserting id into @TmpTable_CH
                   SELECT @NodeText,
                          ParentId,
                          FacilityID,
                          @LoggedInUserName,
                          ( CASE
                                WHEN IsPrimaryNode IS NOT NULL
                                THEN 0
                                ELSE NULL
                            END ) AS IsPrimaryNode
                   FROM ContractHierarchy
                   WHERE NodeId = @NodeId;
            DECLARE @FacilityId BIGINT = ( SELECT FacilityID
                                           FROM ContractHierarchy
                                           WHERE NodeId = @NodeId );
            --Insert AuditLog information
		  -- REVIEW-NOV15 Use common code everywhere 
            INSERT INTO DBO.AuditLogs
                   ( LoggedDate,
                     UserName,
                     ACTION,
                     ObjectType,
                     FacilityName,
                     ModelName,
                     ContractName,
                     ServiceTypeName,
                     Description
                   )
            VALUES( @CurrentDate, @LoggedInUserName, 'Add', 'Model', ( SELECT NodeText
                                                                       FROM ContractHierarchy
                                                                       WHERE FacilityID = @FacilityID
                                                                         AND ParentID = 0 ), @NodeText, '', '', '' );
            SELECT @LastParentId = InsertedID
            FROM @TmpTable_CH;
            IF( @InsertedModelNodeID IS NULL
            AND ( SELECT( CASE
                              WHEN IsPrimaryNode IS NOT NULL
                              THEN 0
                              ELSE NULL
                          END )
                  FROM ContractHierarchy
                  WHERE NodeId = @NodeId ) = 0
              )
                BEGIN
                    SELECT @InsertedModelNodeID = @LastParentId;
                END;
            --Set IsDeleted true untill Copy process finished.So user cant able to see it untill process finished
            UPDATE ContractHierarchy
              SET IsDeleted = 1
            WHERE NodeId = @InsertedModelNodeID;
            DELETE FROM @TmpTable_CH;

            -- define the last Node ID which need to be copyed, this is going to be our Node ID which are passing to SP
            DECLARE @LastNodeId BIGINT;
            SET @LastNodeId = @NodeId;


            -- define the Node Id to be copy now
            -- We need to keep update ParentID as well as NodeID while added new node into database based on condition
            DECLARE @NodeIDToHandle BIGINT;
            DECLARE @ParentIDToHandle BIGINT;
            DECLARE @ParentIDToPass BIGINT;
            SET @ParentIDToPass = @LastParentId;

            -- We are selecting the next Node to copy
            -- Top 1 used to run the loop based on NodeId

            SELECT TOP 1 @NodeIDToHandle = NodeId,
                         @ParentIDToHandle = @LastParentId
            FROM @temp_ContractHierarchyDataTable
            WHERE NodeId > @LastNodeId
            ORDER BY NodeId;

            -- As long as we have Nodes we need to keep copy them
            WHILE @NodeIDToHandle IS NOT NULL
                BEGIN
                    SELECT TOP 1 @ParentIDToPass = NewNodeId
                    FROM @temp_ContractHierarchyDataTable
                    WHERE NodeId = @ParentIDToHandle;
	
                    -- TestProcedure was ALTER d during testing phase. We dont need that SP now, we can delete it
                    ------------------------------------------------

                    INSERT INTO ContractHierarchy
                           ( NodeText,
                             ParentId,
                             IsPrimaryNode,
                             FacilityID,
                             UserName,
                             IsDeleted
                           )
                    OUTPUT INSERTED.NodeID
                           INTO @TmpTable_CH -- inserting id into @TmpTable_CH
                           SELECT NodeText,
                                  @ParentIDToPass,
                                  IsPrimaryNode,
                                  FacilityID,
                                  @LoggedInUserName,
                                  IsDeleted
                           FROM ContractHierarchy
                           WHERE NodeId = @NodeIDToHandle;
                    DECLARE @NewNodeID BIGINT;
                    SELECT @NewNodeID = InsertedID
                    FROM @TmpTable_CH;
                    DELETE FROM @TmpTable_CH;
                    UPDATE @temp_ContractHierarchyDataTable
                      SET NewNodeId = @NewNodeID,
                          NewParentId = @ParentIDToPass
                    WHERE NodeId = @NodeIDToHandle;

                    -- WE can call CopyContractById Fucntion from a stored procedure, but data will not get updated into Database. Because Functions doesn't change the state of database table
                    -- Fix implemented below which is working
                    ---------------------------------------------------------------------------------------------------------------
                    ---------------------------------------------------------------------------------------------------------------
                    -- define the last Contract ID handled
                    DECLARE @LastContractId BIGINT;
                    SET @LastContractId = 0;


                    -- define the Contract ID to be handled now
                    DECLARE @ContractIDToHandle BIGINT;

                    -- select the next Contract ID to handle
                    -- Top 1 used to run the loop based on ContractID
                    SELECT TOP 1 @ContractIDToHandle = ContractID
                    FROM Contracts
                    WHERE ContractID > @LastContractId
                      AND NodeId = @NodeIDToHandle
                      AND IsDeleted = 0
                    ORDER BY ContractID;

                    -- as long as we have Nodes......    
                    WHILE @ContractIDToHandle IS NOT NULL
                        BEGIN
	
                            --Copy Contract. Pass IsDeleted as True so user can't see created Contract as well as Contract Alerts. Once copy will comlete at that time we need to make IsDeleted as False
                            INSERT INTO @InsertedContractTable
                                   ( InsertedContractID
                                   )
                            EXEC dbo.CopyContractById
                                 @ContractIDToHandle,
                                 @NewNodeID,
                                 @LoggedInUserName,
                                 1;
					
                            -- set the last contract handled to the one we just handled
                            SET @LastContractId = @ContractIDToHandle;
                            SET @ContractIDToHandle = NULL;

                            -- select the next contract to handle
                            -- Top 1 used to run the loop based on ContractID   
                            SELECT TOP 1 @ContractIDToHandle = ContractID
                            FROM Contracts
                            WHERE ContractID > @LastContractId
                              AND NodeId = @NodeIDToHandle
                              AND IsDeleted = 0
                            ORDER BY ContractID;
                        END;

                    -- set the last Node handled to the one we just copied
                    SET @LastNodeId = @NodeIDToHandle;
                    SET @NodeIDToHandle = NULL;
                    SELECT TOP 1 @NodeIDToHandle = NodeId,
                                 @ParentIDToHandle = ParentID
                    FROM @temp_ContractHierarchyDataTable
                    WHERE NodeId > @LastNodeId
                    ORDER BY NodeId;
                END;

            -- To Delete TEMP Table
            DELETE @temp_ContractHierarchyDataTable;

            --Set IsDeleted false after copy process finished.So user can able to see it.
            UPDATE ContractHierarchy
              SET IsDeleted = 0
            WHERE NodeId = @InsertedModelNodeID;

            --Set IsDeleted false after copy process finished.So user can able to see Created Contract as well as Alerts.
            UPDATE dbo.Contracts
              SET IsDeleted = 0
            WHERE ContractID IN( SELECT *
                                 FROM @InsertedContractTable );

            --Check Any Transaction happened than commit transaction
            COMMIT TRANSACTION @TransactionName;
            SELECT @InsertedModelNodeID;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;