-- ==================================================================================
-- Author		: Manikandan
-- Created Date	: 05/03/2016
-- Description	: This return Adjudicated ContractName and Request Name by Criteria
-- SELECT [dbo].[GetAdjudicatedContractNameByCriteria] ('57|3|139433,139436')
-- ==================================================================================
CREATE FUNCTION [dbo].[GetAdjudicatedContractNameByCriteria](@SelectCriteria VARCHAR(1000))
RETURNS VARCHAR(MAX)
AS
     BEGIN
         DECLARE @Values VARCHAR(MAX);
         DECLARE @OperatorValue VARCHAR(20);
         DECLARE @NoOfCriterias INT;
         DECLARE @TmpSelectCriteria VARCHAR(MAX);
         DECLARE @Operator INT;
         DECLARE @ClaimField INT;
         DECLARE @Counter INT= 1;
         DECLARE @ClaimFieldValue VARCHAR(100);
         DECLARE @ValueString VARCHAR(MAX);
         DECLARE @ContractName VARCHAR(MAX);
         DECLARE @TblSelectCriteria TABLE
         (RowId          INT IDENTITY(1, 1),
          SelectCriteria VARCHAR(MAX)
         ); 

         --Inserts the selected criteria     
         INSERT INTO @TblSelectCriteria(SelectCriteria)
                SELECT *
                FROM dbo.Split(@SelectCriteria, '~');
         SET @NoOfCriterias =
         (
             SELECT COUNT(*)
             FROM @TblSelectCriteria
         );
         WHILE @NoOfCriterias >= @Counter
             BEGIN
                 SELECT @TmpSelectCriteria = SelectCriteria
                 FROM @TblSelectCriteria
                 WHERE RowId = @Counter;
                 SELECT @ClaimField = dbo.Getparsestring(-1, '|', @TmpSelectCriteria),
                        @Operator = dbo.Getparsestring(-2, '|', @TmpSelectCriteria),
                        @Values = dbo.Getparsestring(-3, '|', @TmpSelectCriteria);
                 SELECT @ClaimFieldValue = [TEXT]
                 FROM [ref.ClaimField]
                 WHERE ClaimFieldID = @ClaimField;
                 IF(@ClaimFieldValue = 'Adjudicated Contract Name')
                     BEGIN
                         SELECT @ContractName = '';
                         WITH ContractName
                              AS(SELECT DISTINCT
                                        (CONVERT( VARCHAR(100), C.ContractName)+' '+CONVERT(VARCHAR(100), C.StartDate, 101)+'-'+CONVERT(VARCHAR(100), C.EndDate, 101)) AS AdjudicatedContractName,
                                        C.ContractID
                                 FROM Contracts C
                                      INNER JOIN [dbo].[ContractAdjudications] CA ON C.ContractID = CA.ContractID
                                 WHERE C.ContractID IN
                                 (
                                     SELECT items
                                     FROM dbo.Split(@Values, ',')
                                 )
                                      AND IsActive = 1
                                      AND IsDeleted = 0)
                              SELECT @ContractName = COALESCE(@ContractName+'; ', '')+AdjudicatedContractName
                              FROM ContractName;
                         SET @Values = SUBSTRING(@ContractName, 2, LEN(@ContractName));
                     END;
                 IF(@ClaimFieldValue = 'Adjudication Request Name')
                     BEGIN
                         SET @Values =
                         (
                             SELECT RequestName
                             FROM TrackTasks
                             WHERE TaskID = @Values
                         );
                     END;
                 IF(@ClaimFieldValue = 'Reviewed')
                     BEGIN
                         SET @Values =
                         (
                             SELECT ReviewedOption
                             FROM [dbo].[ref.ReviewedOption]
                             WHERE ReviewedOptionID = @Values
                         );
                     END;
                 SELECT @OperatorValue = OperatorType
                 FROM [ref.ClaimFieldOperators]
                 WHERE OperatorID = @Operator;
                 SET @ValueString = COALESCE(@ValueString+'~ ', '')+@ClaimFieldValue+'|'+@OperatorValue+'|'+@Values;
                 SET @Counter = @Counter + 1;
             END;
         RETURN @ValueString;
     END;