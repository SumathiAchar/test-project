CREATE PROCEDURE [dbo].[GetASCTableNames](
       @FacilityId   BIGINT,
       @ClaimFieldID BIGINT,
       @UserText     VARCHAR(200))
AS  

/****************************************************************************  
 *   Name         : GetASCTableNames  
 *   Author       : Prasad  
 *   Date         : 07/Sep/2013  
 *   Module       : Get ASCTableNames  
 *   Description  : Get ASCTableNames Information from database  
 -- EXEC [GetASCTableNames] 14279,21

 *****************************************************************************/

    BEGIN
        IF @Claimfieldid = 0
            BEGIN
                SELECT CLAIMFIELDDOCID,
                       TABLENAME
                FROM CLAIMFIELDDOCS docs
                WHERE FacilityId = @FacilityId
                  AND ClaimFieldId IN( 21, 22, 23, 35 )
                  AND docs.TABLENAME LIKE '%' + @UserText + '%';   --ClaimFieldID = 21(ASC Fee Schedule), 22(Fee Schedule), 23(DRG Schedule), 35(Custom Table), 
            END;
        ELSE
            BEGIN
                SELECT CLAIMFIELDDOCID,
                       TABLENAME
                FROM CLAIMFIELDDOCS docs
                WHERE CLAIMFIELDID = @Claimfieldid
                  AND FacilityId = @FacilityId
                  AND docs.TABLENAME LIKE '%' + @UserText + '%';
            END;
    END;