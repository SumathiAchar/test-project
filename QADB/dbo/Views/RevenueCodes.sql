
/****************************************************/
  
--Created By  : Dev  
--Date        : 1-Aug-2013  
--Description : View to retrieve RevenueCodes

/****************************************************/
 
CREATE VIEW RevenueCodes 
AS SELECT
		  CASE
		  WHEN Code LIKE '0%' THEN RIGHT(Code, LEN(Code) - 1)
		  ELSE Code
		  END AS Code, 
		  Description AS Description, 
		  ShortDesc AS ShortDesc
	 FROM [$(CONTRACTMANAGEMENT_QA)].dbo._RevenueCodes; 
GO