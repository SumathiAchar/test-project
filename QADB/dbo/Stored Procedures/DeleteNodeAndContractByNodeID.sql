--Method Name : DeleteNodeAndContractByNodeId
--Module      : Delete Contract
--ALTER d By  : Vishesh
--Date        : 27-Aug-2013
--Modified By : 
--Modif Date  :
--Description Delete contracts

/****************************************************/

CREATE PROCEDURE [dbo].[DeleteNodeAndContractByNodeID](
       @NodeID   BIGINT,
       @UserName VARCHAR(50))
AS
    BEGIN
        DECLARE @TransactionName VARCHAR(100) = 'DeleteNodeAndContractByNodeID',
                @ModelName       VARCHAR(50),
                @FacilityId      BIGINT,
                @ParentId        BIGINT,
                @IsContract      BIT = 0;
        DECLARE @temp_ContractHierarchyDataTable TABLE( RowCounter INT IDENTITY(1, 1),
                                                        NodeId     BIGINT,
                                                        ParentId   BIGINT );
        BEGIN TRY
            IF(( SELECT IsPrimaryNode
                 FROM ContractHierarchy
                 WHERE NodeID = @NodeId ) IS NULL
              )
                BEGIN
                    SELECT @IsContract = 1;
                END;
            BEGIN TRAN @TransactionName;
            SELECT @ModelName = NodeText,
                   @ParentId = ParentID
            FROM ContractHierarchy
            WHERE NodeID = @NodeId;
            INSERT INTO @temp_ContractHierarchyDataTable
                   SELECT NodeId,
                          ParentId
                   FROM ContractHierarchy
                   WHERE NodeID = @NodeId;
            DECLARE @NodeIDToHandle BIGINT;
            SET @NodeIDToHandle = @NodeId;
            WITH ReportData
                 AS (
                 SELECT ContractHierarchy.NodeId,
                        ContractHierarchy.NodeText,
                        ContractHierarchy.ParentId
                 FROM ContractHierarchy
                 WHERE ContractHierarchy.ParentId = @NodeId
                 UNION ALL
                 SELECT ContractHierarchy.NodeId,
                        ContractHierarchy.NodeText,
                        ContractHierarchy.ParentId
                 FROM ReportData
                      INNER JOIN ContractHierarchy ON ContractHierarchy.ParentId = ReportData.NodeId )
                 INSERT INTO @temp_ContractHierarchyDataTable
                        ( NodeId,
                          ParentId
                        )
                        SELECT NodeId,
                               ParentId
                        FROM ReportData
                        ORDER BY NodeId DESC;
            DECLARE @RowCount INT = 1;
            SELECT @NodeIDToHandle = NodeID
            FROM @temp_ContractHierarchyDataTable
            WHERE RowCounter = @RowCount;
            WHILE @NodeIDToHandle IS NOT NULL
                BEGIN
                    EXEC DeleteContractByID
                         @NodeIDToHandle,
                         @IsContract,
                         @UserName;
                    SET @NodeIDToHandle = NULL;
                    SET @RowCount = @RowCount + 1;

                    -- Top 1 is used to run the loop based on the NodeID
                    SELECT @NodeIDToHandle = NodeId
                    FROM @temp_ContractHierarchyDataTable
                    WHERE RowCounter = @RowCount;

				--Delete data from RetainedClaims details associated to the contract
			     DELETE FROM RetainedClaims WHERE ContractID =  (SELECT ContractID FROM Contracts WHERE NodeID = @NodeID);
                END;
            COMMIT TRANSACTION @TransactionName;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;