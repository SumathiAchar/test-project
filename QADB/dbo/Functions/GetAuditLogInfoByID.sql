CREATE FUNCTION [dbo].[GetAuditLogInfoByID](
                @ID         BIGINT,
                @EntityType INT )
RETURNS @AuditLogInfo TABLE( ContractID      BIGINT,
                             ContractName    VARCHAR(150),
                             FacilityName    VARCHAR(50),
                             ModelName       VARCHAR(50),
                             ServiceTypeName VARCHAR(150))
AS
     BEGIN
     DECLARE @FacilityID              BIGINT,
             @ContractID              BIGINT,
             @ContractServiceTypeName VARCHAR(150) = NULL,
             @ContractName            VARCHAR(150) = NULL,
             @ModelName               VARCHAR(50) = NULL,
             @FacilityName            VARCHAR(50) = NULL;
      
     -- For Service level
     IF @EntityType = 0
         BEGIN
             
             --Select Contract & Facility information based on ContractServiceTypeID           
             SELECT @ContractID = C.ContractID,
                    @ContractName = C.ContractName,
                    @FacilityID = C.FacilityID,
                    @ModelName = dbo.GetModelNameByContractID( C.ContractID ),
                    @ContractServiceTypeName = CS.ContractServiceTypeName,
                    @FacilityName = CH.NodeText
             FROM Contracts C
                  INNER JOIN ContractServiceTypes CS ON C.ContractID = CS.ContractID
                  INNER JOIN ContractHierarchy CH ON CH.FacilityID = C.FacilityID
                                                 AND ParentID = 0
             WHERE CS.ContractServiceTypeID = @ID;
         END;
          
             -- For Contract level
     ELSE
     IF @EntityType = 1
         BEGIN
             --Select Contract & Facility information based on ContractID  
             SELECT @ContractID = @ID,
                    @ContractName = C.ContractName,
                    @FacilityID = C.FacilityID,
                    @ModelName = dbo.GetModelNameByContractID( C.ContractID ),
                    @FacilityName = CH.NodeText
             FROM Contracts C
                  INNER JOIN ContractHierarchy CH ON CH.FacilityID = C.FacilityID
                                                 AND ParentID = 0
             WHERE C.ContractID = @ID;
         END;
 
             --For Appeal Letter Templates
     ELSE
     IF @EntityType = 2
         BEGIN
             SELECT @FacilityName = CH.NodeText
             FROM LetterTemplates LT
                  INNER JOIN ContractHierarchy CH ON CH.FacilityID = LT.FacilityID
                                                 AND ParentID = 0
             WHERE LT.LetterTemplateId = @ID;
         END;
 
             --For Job Status
     ELSE
     IF @EntityType = 4
         BEGIN
             SELECT @ModelName = CHModel.NodeText,
                    @FacilityName = CHFacility.NodeText
             FROM TrackTasks TT
                  INNER JOIN ContractHierarchy CHFacility ON CHFacility.FacilityID = TT.FacilityId
                                                         AND CHFacility.ParentID = 0
                  INNER JOIN ContractHierarchy CHModel ON CHModel.FacilityID = TT.FacilityId
                                                      AND CHModel.NodeId = TT.ModelID
             WHERE TaskId = @ID;
         END;
 
             --For Letter Templates
     ELSE
     IF @EntityType = 5
         BEGIN
             SELECT @FacilityName = CH.NodeText
             FROM ClaimFieldDocs CFD
                  INNER JOIN ContractHierarchy CH ON CH.FacilityID = CFD.FacilityID
                                                 AND CH.ParentID = 0
             WHERE ClaimFieldDocId = @ID;
         END;
 
             --For Open Claims
     ELSE
     IF @EntityType = 6
         BEGIN
             SELECT @ModelName = CH.NodeText,
                    @FacilityName = CHFacility.NodeText
             FROM ContractHierarchy CH
                  INNER JOIN ContractHierarchy CHFacility ON CHFacility.NodeId = CH.ParentID
             WHERE CH.NodeId = @ID;
         END;

	    -- for Reassigned Claims
	ELSE
     IF @EntityType = 7
         BEGIN
             SELECT @FacilityName = NodeText
             FROM ContractHierarchy
             WHERE FacilityID = @ID AND ParentID=0
         END;
     --Insert data into @AuditLogInfo
     INSERT INTO @AuditLogInfo
            ( ContractID,
              ContractName,
              ModelName,
              ServiceTypeName,
              FacilityName
            )
     VALUES( @ContractID, @ContractName, @ModelName, @ContractServiceTypeName, @FacilityName );
     RETURN;
     END;