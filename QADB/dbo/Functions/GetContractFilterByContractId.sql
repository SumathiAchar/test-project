/****************************************************/  
--Method Name : [GetContractFilterByContractId]
--Module      : Reports
--Created By  : Jiten  
--Date        : 28-Mar-2014  
--Modified By : 
--Modify Date : 
--Description : To get contract filter by ContractID
-- SELECT * FROM [[GetContractFilterByContractId]] (1,NULL)
/****************************************************/  
CREATE  FUNCTION [dbo].[GetContractFilterByContractId]
(
	 @ContractID BIGINT
)
RETURNS  @temp_ContractServiceTypeFilterData TABLE (
					ContractServiceTypeID  BIGINT,
					ServiceTypeCode VARCHAR(MAX),
					PaymentTypeCode VARCHAR(MAX)
				)
AS 
	BEGIN

				DECLARE  @temp_ContractFilterTable TABLE  
					(  
						FilterValues VARCHAR(MAX),
						FilterName VARCHAR(100),
						IsServiceTypeFilter BIT,
						ServiceLineTypeId BIGINT,  
						PaymentTypeId BIGINT,
						ContractServiceTypeID BIGINT NULL,
						ServiceLine VARCHAR(MAX) ,
						PaymentType VARCHAR(MAX)	
					)	  

				--holds service line data
					DECLARE @STList varchar(MAX)
					--holds payment types data
					DECLARE @PTList varchar(MAX)
					INSERT INTO  @temp_ContractFilterTable(FilterValues,FilterName,IsServiceTypeFilter,ServiceLineTypeId,PaymentTypeId)  
					 SELECT * FROM GetContractFiltersByID(@ContractId, NULL)
					
					UPDATE @temp_ContractFilterTable SET ServiceLine=FilterName + ' ' + FilterValues WHERE IsServiceTypeFilter=1
					UPDATE @temp_ContractFilterTable SET PaymentType=FilterName + ' ' +  FilterValues WHERE IsServiceTypeFilter=0
					--Update Service lines
					SELECT @STList = COALESCE(@STList + ', ', '') + ServiceLine
					FROM @temp_ContractFilterTable WHERE IsServiceTypeFilter=1		
					SELECT @PTList = COALESCE(@PTList + ', ', '') + PaymentType
					FROM @temp_ContractFilterTable WHERE IsServiceTypeFilter=0		

					DECLARE @ReturnValue VARCHAR(MAX)
			
				INSERT INTO @temp_ContractServiceTypeFilterData
				SELECT @ContractID,@STList,@PTList
		RETURN;
	
	END
GO
