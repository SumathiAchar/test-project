-- ===================================================================
-- Author		: Manikandan
-- Created Date: 05/03/2016
-- Description	: This return sort query based on given sortfield
-- ===================================================================
CREATE FUNCTION dbo.GetOpenClaimSortingQuery
(@SortField     VARCHAR(100),
 @SortDirection VARCHAR(4)
)
RETURNS NVARCHAR(1000)
AS
     BEGIN
         DECLARE @SortQuery NVARCHAR(1000);
         IF(@SortDirection IS NULL)
             BEGIN
                 SELECT @SortDirection = 'ASC';
             END;
         IF(@SortField IS NOT NULL)
             IF(@SortField = 'AdjudicatedContractName')
                 BEGIN
                     SET @SortQuery = ' Order by ContractName '+@SortDirection;
                 END;
             ELSE
                 BEGIN
                     SET @SortQuery = ' Order by '+@SortField+' '+@SortDirection;
                 END;;
         ELSE
             BEGIN
                 SET @SortQuery = ' Order by PatientAccountNumber';
             END;
         RETURN @SortQuery;
     END;