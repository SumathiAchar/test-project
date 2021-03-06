/****************************************************/

--Method Name : GetContractServiceTypeFilterByID
--Module      : Reports
--ALTER d By  : Vishesh  
--Date        : 01-Oct-2013  
--Modified By : 
--Modify Date : 
--Description : To get contract service type filter by ContractServiceTypeID
-- SELECT * FROM [GetModelLevelCountFromContractHierarchy] (1,NULL)

/****************************************************/


CREATE FUNCTION [dbo].[GetContractServiceTypeFilterByID](
                @Contractservicetypeid BIGINT )
RETURNS @Temp_Contractservicetypefilterdata TABLE( ContractServiceTypeID BIGINT,
                                                   ServiceTypeCode       VARCHAR(MAX),
                                                   PaymentTypeCode       VARCHAR(MAX))
AS
     BEGIN
     DECLARE @Temp_Contractfiltertable TABLE( FilterValues          VARCHAR(MAX),
                                              FilterName            VARCHAR(100),
                                              IsServiceTypeFilter   BIT,
                                              ServiceLineTypeId     BIGINT,
                                              PaymentTypeId         BIGINT,
                                              ContractServiceTypeID BIGINT NULL,
                                              ServiceLine           VARCHAR(MAX),
                                              PaymentType           VARCHAR(MAX));	  

     --holds service line data
     DECLARE @Stlist VARCHAR(MAX);
     --holds payment types data
     DECLARE @Ptlist VARCHAR(MAX);
     INSERT INTO @Temp_Contractfiltertable
            ( FilterValues,
              FilterName,
              IsServiceTypeFilter,
              ServiceLineTypeId,
              PaymentTypeId,
              ServiceLine,
              PaymentType
            )
            SELECT *,
                   CASE
                       WHEN ISSERVICETYPEFILTER = 1
                       THEN FilterName + ' ' + FilterValues
                       ELSE NULL
                   END,
                   CASE
                       WHEN ISSERVICETYPEFILTER = 0
                       THEN FilterName + ' ' + FilterValues
                       ELSE NULL
                   END
            FROM dbo.GetContractFiltersByID( NULL, @Contractservicetypeid );
	  	
     --Update Service lines	
     SELECT @Stlist = COALESCE(@Stlist + ', ', '') + ServiceLine
     FROM @Temp_ContractFilterTable
     WHERE IsServiceTypeFilter = 1;
     SELECT @Ptlist = COALESCE(@Ptlist + ', ', '') + PaymentType
     FROM @Temp_ContractFilterTable
     WHERE IsServiceTypeFilter = 0;
     DECLARE @ReturnValue VARCHAR(MAX);
     INSERT INTO @Temp_Contractservicetypefilterdata
            SELECT @ContractServiceTypeID,
                   @StList,
                   @PtList;
     RETURN;
     END;
GO