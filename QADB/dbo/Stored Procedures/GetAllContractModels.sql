CREATE PROCEDURE [dbo].[GetAllContractModels](
       @FacilityID BIGINT)
AS  

/****************************************************************************  
 *   Name         : GetAllContractModels 
 *   Author       : Prasadd  
 *   Date         : 05/Sep/2013  
 *   Module       : Get All Contract Models  
 *   Description  : Get Contract Models Information from database  
 *****************************************************************************/

    BEGIN 
        -- READ UNCOMMITTED option is used to avoid table being blocked during Copy Model and Copy Contract process
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        
            BEGIN
                SELECT NodeID,
                       NodeText,
                       IsPrimaryNode
                FROM [dbo].[ContractHierarchy]
                WHERE IsPrimaryNode IS NOT NULL
                  AND FacilityId = @FacilityID
                  AND IsDeleted = 0
                  AND IsPrimaryNode = 0
                UNION ALL
                SELECT NodeID,
                       NodeText,
                       IsPrimaryNode
                FROM [dbo].[ContractHierarchy]
                WHERE IsPrimaryNode IS NOT NULL
                  AND FacilityId = @FacilityID
                  AND IsDeleted = 0
                  AND IsPrimaryNode = 1
                ORDER BY IsPrimaryNode DESC,
                         NodeText;
            END;
    END;