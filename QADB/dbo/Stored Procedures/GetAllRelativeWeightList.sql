
CREATE PROCEDURE [dbo].[GetAllRelativeWeightList]

AS
/****************************************************************************
 *   Name         : GetAllRelativeWeightList
 *   Author       : Raj
 *   Date         : 24/Aug/2013
 *   M Date       : 
 *   M Author     : 
 *   Module       : Add PaymentType DRG
 *   Description  : Get all available Relative weight from master table

 --EXEC	[dbo].[GetAllRelativeWeightList]
 *****************************************************************************/
BEGIN
	SELECT
        [RelativeWeightID],
        [Description]
        FROM [dbo].[ref.RelativeWeight]
END







GO
