/****************************************************************************
 *   Name         : UpdateContractGUID
 *   Author       : Manikandan
 *   Date         : 22-June-2016
 *   Module       : Early Exit
 *   Description  : Update Contract GUID Based on ContractId / ContractServiceTypeID
 *****************************************************************************/

CREATE PROCEDURE [dbo].[UpdateContractGUID](
       @ContractID            BIGINT = NULL,
       @ContractServiceTypeID BIGINT = NULL )
AS
     SET NOCOUNT ON;
    BEGIN
        IF( @ContractID IS NOT NULL
        AND @ContractID <> 0
          )
            BEGIN
                UPDATE AdjudicatedClaimsContractID
                  SET IsClaimAdjudicated = 0
                WHERE LastAdjudicatedContractID = @ContractID;
            END;
        ELSE
        IF( @ContractServiceTypeID IS NOT NULL
        AND @ContractServiceTypeID <> 0
          )
            BEGIN
                SELECT @ContractID = ContractID
                FROM ContractServiceTypes
                WHERE ContractServiceTypeID = @ContractServiceTypeID;
                UPDATE AdjudicatedClaimsContractID
                  SET IsClaimAdjudicated = 0
                WHERE LastAdjudicatedContractID = @ContractID;
            END;
    END;