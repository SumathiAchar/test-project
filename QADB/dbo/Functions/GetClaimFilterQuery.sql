-- =============================================
--Author		: Manikandan
--Created Date	: 05/03/2016
--Description	: This return where clause query based on given filters
-- =============================================
CREATE FUNCTION [dbo].[GetClaimFilterQuery]
(@FilterSearchCriteria XML
)
RETURNS NVARCHAR(MAX)
AS
     BEGIN
         DECLARE @WhereClause NVARCHAR(MAX), @FilterDatetime DATETIME, @FilterCount INT, @Operator INT, @ColumnName VARCHAR(200), @ColumnNameFilter VARCHAR(200);
         DECLARE @FilterValues TABLE
         (RowID       INT IDENTITY(1, 1)
                          PRIMARY KEY,
          FilterName  VARCHAR(200),
          Operator    VARCHAR(50),
          FilterValue VARCHAR(2000)
         );
		     
	   -- Insert Filter values to filtervalues Temp table
	   INSERT INTO @FilterValues
        SELECT V.X.value( './FilterName[1]', 'VARCHAR(200)' ),
                V.X.value( './Operator[1]', 'INT' ),
                V.X.value( './FilterValue[1]', 'VARCHAR(2000)' )
        FROM @FilterSearchCriteria.nodes( '//SearchCriteria' ) AS V( X );

	   SELECT @FilterCount= COUNT(RowID) FROM @FilterValues
	 
	    WHILE @FilterCount > 0
             BEGIN
                 
			  SELECT @ColumnName=FilterName,@Operator=Operator FROM @FilterValues WHERE RowID=@FilterCount

			  IF(@ColumnName='IsRemitLinked')
			  BEGIN
				    DECLARE @FilterText VARCHAR(2000)=(SELECT FilterValue FROM @FilterValues WHERE RowID=@FilterCount)
				    
				    IF ((@FilterText ='YES') OR (('YES' like '%'+@FilterText+'%') AND @Operator=4)) -- 4 means contains
				    BEGIN
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + 'CD.[lastRemitID] IS NOT NULL' 
					   FROM @FilterValues WHERE RowID=@FilterCount;
				    END
				    ELSE
				    IF ((@FilterText ='NO') OR (('NO' like '%'+@FilterText+'%') AND @Operator=4)) -- 4 means contains
				    BEGIN
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + 'CD.[lastRemitID] IS NULL' 
					   FROM @FilterValues WHERE RowID=@FilterCount;
				    END
				    ELSE
				    BEGIN
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + 'CD.[lastRemitID] = 0' 
					   FROM @FilterValues WHERE RowID=@FilterCount;
				    END
			  END
			  ELSE IF(@ColumnName='ActualPayment')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + '( CASE
                            WHEN CD.LastremitID IS NOT NULL
                            THEN R.ProvPay
                            ELSE Hpmt.Amount
					   END)' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END 
			  ELSE IF(@ColumnName='ActualContractualAdjustment')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + '( CASE
                            WHEN CD.LastremitID IS NOT NULL
                            THEN R.Calcont
                            ELSE Hadj.Amount
					   END)' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='ExpectedContractualAdjustment')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + '(  CASE
                            WHEN CA.AdjudicatedValue IS NOT NULL
                            THEN CD.ClaimTotal - CA.AdjudicatedValue
					   END)' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='PatientResponsibility')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + '(  CASE
                            WHEN CD.LastremitID IS NOT NULL
                            THEN R.PatientResponsibility
					   END)' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END 
			  ELSE IF(@ColumnName='ContractualVariance')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + '(CASE
                            WHEN CD.LastremitID IS NOT NULL
                            THEN R.Calcont
                            ELSE Hadj.Amount
					   END) - (CASE
                                   WHEN CA.AdjudicatedValue IS NOT NULL
                                   THEN(CD.ClaimTotal - CA.AdjudicatedValue)
                               END)' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='PaymentVariance')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + ' CA.AdjudicatedValue - (CASE
                                                   WHEN CD.LastremitID IS NOT NULL
                                                   THEN R.ProvPay
                                                   ELSE Hpmt.Amount
                                               END + (CASE
                                                          WHEN CD.LastremitID IS NOT NULL
                                                          THEN R.PatientResponsibility
											   ELSE 0
                                                      END))' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='Age')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + 'dbo.CalculateAge(CAST(PD.DOB AS DATE), CAST(CD.StatementThru AS DATE))' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='BillType')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + 'CD.[BillType]' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='ClaimID')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + 'CD.[ClaimID]' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='ClaimTotal')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + 'CD.[ClaimTotal]' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='DRG')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + 'CD.[DRG]' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END 
			  ELSE IF(@ColumnName='PatAcctNum')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + 'CD.[PatAcctNum]' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='ssinumber')
			  BEGIN
				    
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + 'CD.[ssinumber]' + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + FilterValue + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' + FilterValue + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END 
			  ELSE IF(@ColumnName='StatementFrom')
			  BEGIN
					   IF (@Operator<>4) -- Other than contains filter, need to cast from string to date
					   BEGIN
						  SELECT @FilterDatetime=FilterValue  FROM  @FilterValues WHERE RowID=@FilterCount
						  SELECT @ColumnNameFilter='CD.[StatementFrom]';
					   END
					   ELSE
					   BEGIN
						  SELECT @ColumnNameFilter='CONVERT(VARCHAR(12), CD.[StatementFrom] , 101)';
					   END
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + @ColumnNameFilter + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='StatementThru')
			  BEGIN
					   IF (@Operator<>4) -- Other than contains filter, need to cast from string to date
					   BEGIN
						  SELECT @FilterDatetime=FilterValue  FROM  @FilterValues WHERE RowID=@FilterCount
						  SELECT @ColumnNameFilter='CD.[StatementFrom]';
					   END
					   ELSE
					   BEGIN
						  SELECT @ColumnNameFilter='CONVERT(VARCHAR(12), CD.[StatementThru] , 101)';
					   END
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + @ColumnNameFilter + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='InsertDate')
			  BEGIN
					   SELECT @ColumnNameFilter='CONVERT(VARCHAR(12), CA.[InsertDate] , 101)';
					   IF (@Operator<>4) -- Other than contains filter, need to cast from string to date
					   BEGIN
						  SELECT @FilterDatetime=FilterValue  FROM  @FilterValues WHERE RowID=@FilterCount
					   END
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + @ColumnNameFilter + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='ClaimDate')
			  BEGIN
					   IF (@Operator<>4) -- Other than contains filter, need to cast from string to date
					   BEGIN
						  SELECT @FilterDatetime=FilterValue  FROM  @FilterValues WHERE RowID=@FilterCount
						  SELECT @ColumnNameFilter='CD.[ClaimDate]';
					   END
					   ELSE
					   BEGIN
						  SELECT @ColumnNameFilter='CONVERT(VARCHAR(12), CD.[ClaimDate] , 101)';
					   END
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + @ColumnNameFilter + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='BillDate')
			  BEGIN
					   IF (@Operator<>4) -- Other than contains filter, need to cast from string to date
					   BEGIN
						  SELECT @FilterDatetime=FilterValue  FROM  @FilterValues WHERE RowID=@FilterCount
						  SELECT @ColumnNameFilter='CD.[BillDate]';
					   END
					   ELSE
					   BEGIN
						  SELECT @ColumnNameFilter='CONVERT(VARCHAR(12), CD.[BillDate] , 101)';
					   END
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + @ColumnNameFilter + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='LastFiledDate')
			  BEGIN
					   IF (@Operator<>4) -- Other than contains filter, need to cast from string to date
					   BEGIN
						  SELECT @FilterDatetime=FilterValue  FROM  @FilterValues WHERE RowID=@FilterCount
						  SELECT @ColumnNameFilter='CD.[LastFiledDate]';
					   END
					   ELSE
					   BEGIN
						  SELECT @ColumnNameFilter='CONVERT(VARCHAR(12), CD.[LastFiledDate] , 101)';
					   END
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + @ColumnNameFilter + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='CheckDate')
			  BEGIN
					   IF (@Operator<>4) -- Other than contains filter, need to cast from string to date
					   BEGIN
						  SELECT @FilterDatetime=FilterValue  FROM  @FilterValues WHERE RowID=@FilterCount
						  SELECT @ColumnNameFilter='R.[CheckDate]';
					   END
					   ELSE
					   BEGIN
						  SELECT @ColumnNameFilter='CONVERT(VARCHAR(12), R.[CheckDate] , 101)';
					   END
					   SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + @ColumnNameFilter + CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN ' = ' + '''' + CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
								    WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
								    WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' +  CONVERT(VARCHAR(12), @FilterDatetime, 101) + ''''
                                        END

					   FROM @FilterValues WHERE RowID=@FilterCount;
				    
			  END
			  ELSE IF(@ColumnName='AdjudicatedContractName')
			  BEGIN
				         SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + 'ISNULL(IIF(C.ContractName IS NULL,NULL,CONVERT( VARCHAR(100), C.ContractName)+'' ''+CONVERT(VARCHAR(100), C.StartDate, 101)+''-''+CONVERT(VARCHAR(100), C.EndDate, 101)),'''')'+ CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN  ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
                                            WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + ISNULL(FilterValue,'')  + ''''
                                            WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' +  ISNULL(FilterValue,'') + ''''
                                            END
								    FROM @FilterValues WHERE RowID=@FilterCount;
			  END
			  ELSE IF(@ColumnName='InsuredsGroupNumber')
			  BEGIN
				         SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') +'ISNULL(ID.GroupNumber,'''')'+ CASE
                                            WHEN Operator = 2 -- 2 means equal
                                            THEN  ' = ' + '''' + FilterValue + ''''
                                            WHEN Operator = 4 -- 4 means contains
                                            THEN ' LIKE ' + '''' + '%' + FilterValue + '%' + ''''
                                            WHEN Operator = 5 -- 5 means greater Than
                                            THEN ' > ' + '''' + ISNULL(FilterValue,'')  + ''''
                                            WHEN Operator = 7 -- 7 means less Than
                                            THEN ' < ' + '''' +  ISNULL(FilterValue,'') + ''''
                                            END
								    FROM @FilterValues WHERE RowID=@FilterCount;
			  END
			  ELSE
			  	  BEGIN
				 SELECT @WhereClause = COALESCE(@WhereClause + ' AND ', '') + '[' + CAST(LTRIM(RTRIM(FilterName)) AS VARCHAR(200)) + ']' + CASE
                                                                                                                        WHEN Operator = 2 -- 2 means equal
                                                                                                                        THEN ' = ' + '''' + LTRIM(RTRIM(FilterValue)) + ''''
                                                                                                                        WHEN Operator = 4 -- 4 means contains
                                                                                                                        THEN ' LIKE ' + '''' + '%' + LTRIM(RTRIM(FilterValue)) + '%' + ''''
																								WHEN Operator = 5 -- 5 means greater Than
																								THEN ' > ' + '''' + LTRIM(RTRIM(FilterValue)) + ''''
																								WHEN Operator = 7 -- 7 means less Than
																								THEN ' < ' + '''' + LTRIM(RTRIM(FilterValue)) + ''''
                                                                                                                    END
				    FROM @FilterValues WHERE RowID=@FilterCount;
			  END

                 SET @FilterCount = @FilterCount - 1;
             END;

		  IF @WhereClause IS NULL
            BEGIN
                SET @WhereClause = '';
            END;
		  ELSE
            BEGIN
                SET @WhereClause = ' WHERE ' + @WhereClause;
            END;

         RETURN @WhereClause;
	    
     END;