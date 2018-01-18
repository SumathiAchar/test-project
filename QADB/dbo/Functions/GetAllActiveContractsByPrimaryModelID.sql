/****************************************************************************  
 *   Name         : GetAllActiveContractsByPrimaryModelID
 *   Author       : Sheshagiri  
 *   Date         : 02/03/2015  
 *   Alter Date   : 
 *   Module       : Background adjudication
 *   Description  : Gets All Active Contracts By PrimaryModelID

SELECT * FROM dbo.GetAllActiveContractsByPrimaryModelID(11977)
 *****************************************************************************/
CREATE FUNCTION dbo.GetAllActiveContractsByPrimaryModelID
( @PrimaryModelNodeID INT)
RETURNS 
@Temp_Contracts TABLE(
						RowNumber INT IDENTITY(1,1), 
						ContractID BIGINT
					  )
AS
BEGIN

	INSERT INTO @Temp_Contracts
	SELECT
				   C.ContractID
			  FROM
				   dbo.Contracts C JOIN dbo.GetAllContractsByModelId(@PrimaryModelNodeID) AS	CH ON C.ContractID = CH.ContractID
			  WHERE C.IsActive = 1;
	RETURN;
END;


