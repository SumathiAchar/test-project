-- =============================================
--Name        : CalculateAge
--Author	  : Dev
--Created By  : Subramani M
--Created Date: 11-Apr-2016
--Description : Getting difference between patient DOB and claim data StatementThru in No of years
--SELECT dbo.CalculateAge(CAST('1956-08-30 00:0:00' AS DATE), CAST('2014-08-16 00:0:00' AS DATE)) AS Age
-- =============================================
CREATE FUNCTION [dbo].[CalculateAge] (@DiffFrom DATE, @DiffTo DATE) RETURNS INT
AS
BEGIN
    DECLARE @NumOfYears INT
    SET @NumOfYears = (SELECT 
                         DATEDIFF(YEAR, @DiffFrom, @DiffTo) + 
                         CASE 
                           WHEN MONTH(@DiffTo) < MONTH(@DiffFrom) THEN -1 
                           WHEN MONTH(@DiffTo) > MONTH(@DiffFrom) THEN 0 
                           ELSE 
                             CASE WHEN DAY(@DiffTo) < DAY(@DiffFrom) THEN -1 ELSE 0 END 
                         END)
    IF @NumOfYears < 0
    BEGIN
        SET @NumOfYears = 0;
    END

    RETURN @NumOfYears;
END
