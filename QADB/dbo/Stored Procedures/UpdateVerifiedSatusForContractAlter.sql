CREATE PROCEDURE [dbo].[UpdateVerifiedSatusForContractAlter]
(
@ContractAlertID INT
)
AS
BEGIN
UPDATE ContractAlerts 
SET IsVerified = 1
WHERE ContractAlertID = @ContractAlertID
--SELECT IsVerified FROM ContractAlerts WHERE ContractAlertID = @ContractAlertID
END

GO
