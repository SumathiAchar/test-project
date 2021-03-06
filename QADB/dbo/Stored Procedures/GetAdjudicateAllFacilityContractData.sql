/****************************************************/
--Method Name : AdjudicateAllFacilityContract  
--Module      : SelectClaims    
--Created By  : Manjunath.B    
--Date        : 17-Oct-2013
--Modified    : 5-Nov-2013
--Description: This script gets all records where isPrimaryNode is true      
--EXEC GetAdjudicateAllFacilityContractData 
/****************************************************/
   
CREATE PROCEDURE [dbo].[GetAdjudicateAllFacilityContractData]
AS
BEGIN
		SELECT 	CH2.FacilityId AS FacilityId,CH.NodeID AS NodeId,CH2.FacilityId AS FacilityIdOfNode
		FROM ContractHierarchy AS CH 
		JOIN ContractHierarchy CH2 ON CH2.NodeID = CH.ParentID
		WHERE CH.IsPrimaryNode = 1
END



GO
