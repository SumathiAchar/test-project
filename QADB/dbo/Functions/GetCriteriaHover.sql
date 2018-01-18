CREATE FUNCTION [dbo].[GetCriteriaHover]
(@SelectCriteria VARCHAR(MAX),
                                        @Startdate      DATETIME,
                                        @Enddate        DATETIME,
                                        @Datetype       INT)
RETURNS VARCHAR(MAX)
AS
     BEGIN
         DECLARE @Values VARCHAR(MAX);
         DECLARE @OperatorValue VARCHAR(20);
         DECLARE @NoOfCriterias INT;
         DECLARE @TmpSelectCriteria VARCHAR(MAX);
         DECLARE @Operator INT;
         DECLARE @CriteriaOrder INT= 1;
         DECLARE @ClaimField INT;
         DECLARE @ClaimFieldValue VARCHAR(100);
         DECLARE @ValueString VARCHAR(MAX);
         DECLARE @TblSelectCriteria TABLE
         (RowId          INT IDENTITY(1, 1),
          SelectCriteria VARCHAR(MAX)
         );
         SET @SelectCriteria = [dbo].[GetXmlParsedString](@SelectCriteria);
         --Inserts the selected criteria     
         INSERT INTO @TblSelectCriteria(SelectCriteria)
                SELECT *
                FROM dbo.Split(@SelectCriteria, '~');
         SET @NoOfCriterias =
         (
             SELECT COUNT(*)
             FROM @TblSelectCriteria
         );
         IF @NoOfCriterias = 0
             BEGIN
                 SET @ValueString = 'Data Type: '+
                 (
                     SELECT CASE
                                WHEN @Datetype = 1
                                THEN 'Service From'
                                WHEN @Datetype = 4
                                THEN 'Service Thru'
                                WHEN @Datetype = 2
                                THEN 'Billing'
                                WHEN @Datetype = 3
                                THEN 'Submission'
                            END
                 )+'@'+REPLACE(CONVERT(VARCHAR(19), @Startdate, 101), ' ', '/')+' - '+REPLACE(CONVERT(VARCHAR(19), @Enddate, 101), ' ', '/');--CONVERT(VARCHAR(19), @Enddate, 110);
             END;
         ELSE
             BEGIN
                 WHILE @NoOfCriterias <> 0
                     BEGIN
                         SELECT @TmpSelectCriteria = SelectCriteria
                         FROM @TblSelectCriteria
                         WHERE RowId = @CriteriaOrder;
                         SELECT @ClaimField = dbo.Getparsestring(-1, '|', @TmpSelectCriteria),
                                @Operator = dbo.Getparsestring(-2, '|', @TmpSelectCriteria),
                                @Values = dbo.Getparsestring(-3, '|', @TmpSelectCriteria);
                         SELECT @ClaimFieldValue = [TEXT]
                         FROM [ref.ClaimField]
                         WHERE ClaimFieldID = @ClaimField;
                         IF(@ClaimFieldValue = 'Adjudication Request Name')
                             BEGIN
                                 SET @Values =
                                 (
                                     SELECT RequestName
                                     FROM TrackTasks
                                     WHERE TaskID = @Values
                                 );
                                 SET @Operator = 3;
                             END;
                         IF(@ClaimFieldValue = 'Reviewed')
                             BEGIN
                                 SET @Values =
                                 (
                                     SELECT ReviewedOption
                                     FROM [dbo].[ref.ReviewedOption]
                                     WHERE ReviewedOptionID = @Values
                                 );
                                 SET @Operator = 3;
                             END;
				     IF(@ClaimField = 57)
                             BEGIN
                                 SET @Values =
                                 (
                                     SELECT ContractName
                                     FROM Contracts
                                     WHERE ContractId = @Values
                                 );
						   SET @Operator = 3;
                             END;
                         SELECT @OperatorValue = OperatorType
                         FROM [ref.ClaimFieldOperators]
                         WHERE OperatorID = @Operator;
                         SET @ValueString = COALESCE(@ValueString+'; ', '')+@ClaimFieldValue+' '+@OperatorValue+' '+@Values;
                         SET @CriteriaOrder = @CriteriaOrder + 1;
                         SET @NoOfCriterias = @NoOfCriterias - 1;
                     END;
                 IF @Datetype <> -1
                     BEGIN
                         SET @ValueString = 'Data Type: '+
                         (
                             SELECT CASE
                                        WHEN @Datetype = 1
                                        THEN 'Service From'
                                        WHEN @Datetype = 4
                                        THEN 'Service Thru'
                                        WHEN @Datetype = 2
                                        THEN 'Billing'
                                        WHEN @Datetype = 3
                                        THEN 'Submission'
                                    END
                         )+'@ '+REPLACE(CONVERT(VARCHAR(19), @Startdate, 101), ' ', '/')+' - '+REPLACE(CONVERT(VARCHAR(19), @Enddate, 101), ' ', '/')+'@ '+@ValueString;
                     END;
                 ELSE
                     BEGIN
                         SET @ValueString = @ValueString;
                     END;
             END;
         RETURN @ValueString;
     END;
