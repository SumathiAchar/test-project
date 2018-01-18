/****************************************************************************  
 *   Name         : GetSubQueryForClaimFiltersForBackground
 *   Author       : Sumathi  
 *   Date         : 01/09/2016
 *   Alter Date   : 
 *   Module       : Background adjudication
 *   Description  : Builds filter code query based on Contract level claim tools and contract conditions

 SELECT dbo.GetSubQueryForClaimFiltersForBackground(-1,NULL,NULL,'2|3|*',2,170286,1,11977)
 *****************************************************************************/

CREATE FUNCTION [dbo].[GetSubQueryForClaimFiltersForBackground](
               @DateType       INT,
               @DateFrom       DATETIME,
               @DateTo         DATETIME,
               @SelectCriteria VARCHAR(MAX),
               @FacilityID     BIGINT,
               @ContractID     BIGINT,
               @IsReadjudicate BIT,
			@PrimaryNodeID INT )
RETURNS VARCHAR(MAX)
     BEGIN
     DECLARE @ClaimSubQry VARCHAR(MAX);  
     --Variables used to hold intermediate operation results and sql query   
     DECLARE @TmpSelectCriteria VARCHAR(MAX);
     DECLARE @ValueStringData VARCHAR(MAX);
     DECLARE @ClaimChargesSubQry VARCHAR(MAX);
     DECLARE @Count INT;
     DECLARE @ClaimFieldValue VARCHAR(MAX);
     DECLARE @OperatorValue VARCHAR(MAX);
     DECLARE @SubQueryFlag INT = 0;
     DECLARE @DateFlag INT = 0;
     DECLARE @Flag INT = 0;
     DECLARE @ClaimField INT;
     DECLARE @Operator INT;
     DECLARE @Values VARCHAR(MAX);
     DECLARE @NoOfCriterias INT;
     --Table used to hold all the selected criteria row wise split   
     DECLARE @TblSelectCriteria TABLE( rowid          INT IDENTITY(1, 1),
                                       selectcriteria VARCHAR(MAX),
                                       claimfield     INT,
                                       operator       INT,
                                       [values]       VARCHAR(MAX));  
  
     --holds claimchargedata query to filter claims based on Hcspcscode and Rev code   
     SET @ClaimChargesSubQry = '';  
  
     --Inserts the selected criteria     
     INSERT INTO @TblSelectCriteria
            ( selectcriteria
            )
            SELECT *
            FROM dbo.Split( @SelectCriteria, '~' );  
  
     --Counts the total criteria selected by user to filter the claims   
     SELECT @Count = COUNT(*)
     FROM @TblSelectCriteria;
     SET @NoOfCriterias = ( SELECT COUNT(*)
                            FROM @TblSelectCriteria );
     --Main query used to filter the claims
     -- Do not change the below line. We are updating this line in other SP based on requirement.   
     --SET @ClaimSubQry = 'SELECT DISTINCT CONVERT(VARCHAR,CD.claimid) C from ClaimData AS CD  ';
     SET @ClaimSubQry = 'SELECT DISTINCT CD.claimid C,' + CONVERT(VARCHAR(100), @ContractID) + ' as ContractID from ClaimData AS CD  ';
 
     ----Appending join queries to @ClaimSubQry based on selectCriteria
     WHILE @NoOfCriterias <> 0
         BEGIN
             SELECT @TmpSelectCriteria = selectcriteria
             FROM @TblSelectCriteria
             WHERE rowid = @NoOfCriterias;
             SELECT @ClaimField = dbo.Getparsestring( -1, '|', @TmpSelectCriteria );
             SET @ClaimSubQry = CASE
                                    WHEN( @ClaimField = 7
                                       OR @ClaimField = 20
                                        )
                                    THEN CASE
                                             WHEN( @ClaimSubQry NOT LIKE '%JOIN InsuredData%'
                                                 )
                                             THEN @ClaimSubQry + ' JOIN InsuredData AS ID ON ID.ClaimID = CD.ClaimID'
                                             ELSE @ClaimSubQry
                                         END
                                    WHEN( @ClaimField = 3
                                       OR @ClaimField = 4
                                       OR @ClaimField = 9
                                        )
                                    THEN CASE
                                             WHEN( @ClaimSubQry NOT LIKE '%JOIN ClaimChargeData%'
                                                 )
                                             THEN @ClaimSubQry + ' JOIN ClaimChargeData AS CCD ON CCD.ClaimID = CD.ClaimID'
                                             ELSE @ClaimSubQry
                                         END
                                    WHEN( @ClaimField = 12 )
                                    THEN CASE
                                             WHEN( @ClaimSubQry NOT LIKE '%JOIN ICDDData%'
                                                 )
                                             THEN @ClaimSubQry + ' JOIN ICDDData AS ICD ON ICD.ClaimID = CD.ClaimID'
                                             ELSE @ClaimSubQry
                                         END
                                    WHEN( @ClaimField = 13 )
                                    THEN CASE
                                             WHEN( @ClaimSubQry NOT LIKE '%JOIN ICDPData%'
                                                 )
                                             THEN @ClaimSubQry + ' JOIN ICDPData AS ICP ON ICP.ClaimID = CD.ClaimID'
                                             ELSE @ClaimSubQry
                                         END
                                    WHEN( @ClaimField = 17 )
                                    THEN CASE
                                             WHEN( @ClaimSubQry NOT LIKE '%JOIN ValueCodeData%'
                                                 )
                                             THEN @ClaimSubQry + ' JOIN ValueCodeData AS VCD ON VCD.ClaimID = CD.ClaimID'
                                             ELSE @ClaimSubQry
                                         END
                                    WHEN( @ClaimField = 18 )
                                    THEN CASE
                                             WHEN( @ClaimSubQry NOT LIKE '%JOIN OccurrenceCodeData%'
                                                 )
                                             THEN @ClaimSubQry + ' JOIN OccurrenceCodeData AS OCD ON OCD.ClaimID = CD.ClaimID'
                                             ELSE @ClaimSubQry
                                         END
                                    WHEN( @ClaimField = 19 )
                                    THEN CASE
                                             WHEN( @ClaimSubQry NOT LIKE '%JOIN ConditionCodeData%'
                                                 )
                                             THEN @ClaimSubQry + ' JOIN ConditionCodeData AS CC ON CC.ClaimID = CD.ClaimID'
                                             ELSE @ClaimSubQry
                                         END
                                    WHEN( @ClaimField = 10
                                       OR @ClaimField = 11
                                       OR @ClaimField = 14
                                        )
                                    THEN CASE
                                             WHEN( @ClaimSubQry NOT LIKE '%JOIN ClaimPhysicianData%'
                                                 )
                                             THEN @ClaimSubQry + ' JOIN ClaimPhysicianData AS CPD ON CPD.ClaimID = CD.ClaimID'
                                             ELSE @ClaimSubQry
                                         END
                                    WHEN( @ClaimField IN( 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 36, 37, 38, 50, 55, 56, 57 )
                                        )
                                    THEN CASE
                                             WHEN( @ClaimSubQry NOT LIKE '%JOIN dbo.ClaimPayVarData%'
                                                 )
                                             THEN @ClaimSubQry + ' JOIN dbo.ClaimPayVarData AS CPVA ON CPVA.ClaimID = CD.ClaimID'
                                             ELSE @ClaimSubQry
                                         END
                                    WHEN( @ClaimField = 51
                                       OR @ClaimField = 54
                                        )
                                    THEN CASE
                                             WHEN( @ClaimSubQry NOT LIKE '%JOIN dbo.PatientData%'
                                                 )
                                             THEN @ClaimSubQry + ' JOIN dbo.PatientData AS PD ON PD.ClaimID = CD.ClaimID'
                                             ELSE @ClaimSubQry
                                         END
                                    WHEN( @ClaimField = 52 )
                                    THEN CASE
                                             WHEN( @ClaimSubQry NOT LIKE '%LEFT JOIN dbo.ClaimReviewed%'
                                                 )
                                             THEN @ClaimSubQry + ' LEFT JOIN dbo.ClaimReviewed AS CR ON CR.ClaimID = CD.ClaimID'
                                             ELSE @ClaimSubQry
                                         END
                                    ELSE @ClaimSubQry
                                END;
             SET @NoOfCriterias = @NoOfCriterias - 1;
         END;
             IF @FacilityID IS NOT NULL
            AND @FacilityID > 0
                 BEGIN
                     --SET @ClaimSubQry = @ClaimSubQry + ' JOIN Facility_SSINumber AS FS ON FS.SSINumber = CD.SSINumber ' //SSH
                     SET @ClaimSubQry = @ClaimSubQry + ' JOIN Facility_SSINumber AS FS ON CONVERT(INT,FS.SSINumber) = CONVERT(INT,CD.SSINumber) ';
                 END;

                     --clear data in @TmpSelectCriteria,@ClaimField fields
                     SET @ClaimField = NULL;
                     SET @TmpSelectCriteria = NULL;
   
                     --Applying date type filter   
                     IF @DateType = 1 -- StatementFrom date   
                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE CD.StatementFrom BETWEEN ''' + CONVERT(VARCHAR(12), @DateFrom, 101) + ''' AND ''' + CONVERT(VARCHAR(12), @DateTo, 101) + '''  ';
                     IF @DateType = 2 -- BillDate date   
                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE CD.BillDate BETWEEN ''' + CONVERT(VARCHAR(12), @DateFrom, 101) + ''' AND ''' + CONVERT(VARCHAR(12), @DateTo, 101) + '''  ';
                     IF @DateType = 3 -- ClaimDate date   
                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE CD.ClaimDate BETWEEN ''' + CONVERT(VARCHAR(12), @DateFrom, 101) + ''' AND ''' + CONVERT(VARCHAR(12), @DateTo, 101) + '''  ';
                     IF @DateType = 4 -- StatementThru date
                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE CD.StatementThru BETWEEN ''' + CONVERT(VARCHAR(12), @DateFrom, 101) + ''' AND ''' + CONVERT(VARCHAR(12), @DateTo, 101) + '''  ';
                     IF @FacilityID IS NOT NULL
                    AND @FacilityID > 0
                         BEGIN
                             SET @ClaimSubQry = @ClaimSubQry + ' AND FS.FacilityID = ' + CONVERT(VARCHAR(100), @FacilityID);
                         END;
                             --Flag set to form the query properly by adding AND operator for another set of conditions   
                             IF @DateType > 0
                                 BEGIN
                                     SET @DateFlag = 1;
                                     IF @Count > 0
                                     SET @ClaimSubQry = @ClaimSubQry + ' AND ';
                                 END;
                                     IF( @IsReadjudicate = 1 )
                                         BEGIN
                                             SET @ClaimSubQry = @ClaimSubQry + ' INNER JOIN AdjudicatedClaimsContractID ACC on ACC.ClaimID = CD.ClaimID AND ACC.ModelID = ' + CONVERT(VARCHAR(100), @PrimaryNodeID);
                                         END;
  
                                             --Loops through all criteria selected by user and form the SQL   
                                             WHILE( @Count > 0 )
                                                 BEGIN
                                                     SELECT @TmpSelectCriteria = selectcriteria
                                                     FROM @TblSelectCriteria
                                                     WHERE rowid = @Count;
                                                     SELECT @ClaimField = dbo.Getparsestring( -1, '|', @TmpSelectCriteria ),
                                                            @Operator = dbo.Getparsestring( -2, '|', @TmpSelectCriteria ),
                                                            @Values = dbo.Getparsestring( -3, '|', @TmpSelectCriteria );
                                                     SELECT @OperatorValue = operatortype
                                                     FROM [ref.claimfieldoperators]
                                                     WHERE operatorid = @Operator;
                                                     SET @Flag = 0;
                                                     SET @Values = REPLACE(@Values, '''', '''''');  --SSH

                                                     IF( @ClaimField = 1 ) --Patient  Account Number   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' CD.PatAcctNum ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 2 ) --Type of Bill (I)   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' CD.BillType ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 3 ) -- Revenue Code(I)   
                                                         BEGIN
                                                             DECLARE @pad_characters            VARCHAR(2) = '0000',
                                                                     @Pad                       VARCHAR(2) = '0%',
                                                                     @ClaimChargesRevCodeSubQry VARCHAR(MAX) = '',
                                                                     @RevCodeBlankCondition     VARCHAR(100) = ' AND ISNULL(CCD.RevCode, '''') <> ''''';
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimChargesRevCodeSubQry = [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' (case 
																when CCD.RevCode like ''' + @Pad + '''
																 then right(CCD.RevCode, len(CCD.RevCode) - 1)
																else CCD.RevCode end) ', @OperatorValue, @ClaimField );
                                                             IF @OperatorValue = '<>'
                                                             SET @ClaimChargesRevCodeSubQry = dbo.GetSubQueryForNotEqualOrNotInOperator( @ClaimChargesRevCodeSubQry, 'ClaimChargeData' );
                                                             IF @SubQueryFlag >= 1
                                                             SET @ClaimChargesSubQry = @ClaimChargesSubQry + ' AND ';
                                                             SET @ClaimChargesSubQry = @ClaimChargesSubQry + @ClaimChargesRevCodeSubQry;
                                                             IF( CHARINDEX(@RevCodeBlankCondition, @ClaimChargesSubQry) = 0 )
                                                             SET @ClaimChargesSubQry = @ClaimChargesSubQry + @RevCodeBlankCondition;
                                                             SET @SubQueryFlag = @SubQueryFlag + 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 4 ) -- HCPCS/RATE/HIPPS   
                                                         BEGIN
                                                             DECLARE @ClaimChargesHCPCSCodeSubQry VARCHAR(MAX) = '',
                                                                     @HCPCSCodeBlankCondition     VARCHAR(100) = ' AND ISNULL(CCD.HCPCSCode, '''') <> ''''';
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimChargesHCPCSCodeSubQry = [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' CCD.HCPCSCode ', @OperatorValue, @ClaimField );
                                                             IF @OperatorValue = '<>'
                                                             SET @ClaimChargesHCPCSCodeSubQry = dbo.GetSubQueryForNotEqualOrNotInOperator( @ClaimChargesHCPCSCodeSubQry, 'ClaimChargeData' );
                                                             IF @SubQueryFlag >= 1
                                                             SET @ClaimChargesSubQry = @ClaimChargesSubQry + ' AND ';
                                                             SET @ClaimChargesSubQry = @ClaimChargesSubQry + @ClaimChargesHCPCSCodeSubQry;
                                                             IF( CHARINDEX(@HCPCSCodeBlankCondition, @ClaimChargesSubQry) = 0 )
                                                             SET @ClaimChargesSubQry = @ClaimChargesSubQry + @HCPCSCodeBlankCondition;
                                                             SET @SubQueryFlag = @SubQueryFlag + 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 6 ) -- Payer Name   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' CD.PriPayerName ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 7
                                                      OR @ClaimField = 20
                                                       ) 
                                                     -- 7	Insured’s ID
                                                     -- 20	Insured’s group
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetInsuredGroupFilterQuery]( @ClaimField, @OperatorValue, CONVERT(VARCHAR(MAX), @Values));
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 8 ) -- DRG(I)   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' CD.DRG ', @OperatorValue, @ClaimField );
                                                             SET @ClaimSubQry = @ClaimSubQry + ' AND ISNULL(CD.DRG, '''') <> ''''';
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 9 ) -- Place Of Service
                                                         BEGIN
                                                             DECLARE @ClaimChargesPOSSubQry VARCHAR(MAX) = '',
                                                                     @POSBlankCondition     VARCHAR(100) = ' AND ISNULL(CCD.PlaceOfService, '''') <> ''''';
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimChargesPOSSubQry = [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' CCD.PlaceOfService ', @OperatorValue, @ClaimField );
                                                             IF @OperatorValue = '<>'
                                                             SET @ClaimChargesPOSSubQry = dbo.GetSubQueryForNotEqualOrNotInOperator( @ClaimChargesPOSSubQry, 'ClaimChargeData' );
                                                             IF @SubQueryFlag >= 1
                                                             SET @ClaimChargesSubQry = @ClaimChargesSubQry + ' AND ';
                                                             SET @ClaimChargesSubQry = @ClaimChargesSubQry + @ClaimChargesPOSSubQry;
                                                             IF( CHARINDEX(@POSBlankCondition, @ClaimChargesSubQry) = 0 )
                                                             SET @ClaimChargesSubQry = @ClaimChargesSubQry + @POSBlankCondition;
                                                             SET @SubQueryFlag = @SubQueryFlag + 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 10
                                                      OR @ClaimField = 11
                                                      OR @ClaimField = 14
                                                       ) 
                                                     -- 10	Referring Physician(P)
                                                     -- 11	Rendering Physician(P)
                                                     -- 14   Attending Physician(I)
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetPhysicianDataFilterQuery]( @ClaimField, @OperatorValue, CONVERT(VARCHAR(MAX), @Values));
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 12
                                                      OR @ClaimField = 13
                                                       ) 
                                                     -- 12	ICD-9 Diagnosis
                                                     -- 13	ICD-9 Procedure(I)
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetICDProcedureAndDiagnosisCodeFilterQuery]( @ClaimField, @OperatorValue, CONVERT(VARCHAR(MAX), @Values));
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 15 ) -- Total Charges   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' CD.ClaimTotal ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 16 )   -- Statement covers period(I)- Dates of service(P)   
                                                         BEGIN
                                                             DECLARE @DateValue NVARCHAR(10);
                                                             SET @DateValue = CONVERT(VARCHAR(10), @Values);
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             IF( @OperatorValue = '=' )
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' CD.StatementFrom <= ' + ' ''' + @DateValue + '''' + ' AND CD.StatementThru >= ' + ' ''' + @DateValue + '''';
                                                                 END;
                                                             ELSE
                                                             IF( @OperatorValue = '>' )
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' CD.StatementFrom > ' + ' ''' + @DateValue + '''';
                                                                 END;
                                                             ELSE
                                                             IF( @OperatorValue = '<' )
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' CD.StatementThru < ' + ' ''' + @DateValue + '''';
                                                                 END;
                                                             ELSE
                                                             IF( @OperatorValue = '<>' )
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' CD.StatementFrom > ' + ' ''' + @DateValue + '''' + ' AND CD.StatementThru < ' + ' ''' + @DateValue + '''';
                                                                 END;;;
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 17 ) -- 17	Value Codes(I)
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetValueCodeDataFilterQuery]( @ClaimField, @OperatorValue, CONVERT(VARCHAR(MAX), @Values));
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 18
                                                      OR @ClaimField = 19
                                                       ) 
                                                     -- 18	Occurrence Code(I)
                                                     -- 19	Condition Codes(I)
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetOccurrenceAndConditionCodeFilterQuery]( @ClaimField, @OperatorValue, CONVERT(VARCHAR(MAX), @Values));
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 24 ) -- ClaimID   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' CD.ClaimID ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = -99 )  -- Job RequestName  
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;  
                                                             --SET @ClaimSubQry = @ClaimSubQry + ' CD.ClaimID IN  ( ' + [dbo].[GetRequestNameFilterQuery]( @OperatorValue, CONVERT(VARCHAR(MAX), @Values) ) + ' )';
                                                             SET @ClaimSubQry = @ClaimSubQry + ' EXISTS  ( ' + [dbo].[GetRequestNameFilterQuery]( @OperatorValue, @Values,0 )
                                                                                --CONVERT(VARCHAR(MAX), @Values))
                                                                                + ' ) ';
                                                             SET @Flag = 1;
                                                         END;
                                                     ELSE
                                                     IF( @ClaimField = 28 ) -- Calculated Adj   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' ISNULL(CPVA.ExpectedContractualAdjustment,0) ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;;;;;;;;;;;;;;;;
                                                     IF( @ClaimField = 27 ) -- Actual Adj   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' ISNULL(CPVA.ActualContractualAdjustment,0) ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 26 ) -- Contractual Variance  
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' ISNULL(CPVA.ContractualVariance,0) ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 25 ) -- Payment Variance 
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' ISNULL(CPVA.PaymentVariance,0) ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 29 ) -- CustomField1   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' ISNULL(CPVA.CustomField1,'''') ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 30 ) -- CustomField2   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' ISNULL(CPVA.CustomField2,'''') ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 31 ) -- CustomField3   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' ISNULL(CPVA.CustomField3,'''') ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 32 ) -- CustomField4   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' ISNULL(CPVA.CustomField4,'''') ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 33 ) -- CustomField5   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' ISNULL(CPVA.CustomField5,'''') ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 34 ) -- CustomField6   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' ISNULL(CPVA.CustomField6,'''') ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 36 ) -- NPI   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' CD.NPI ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 37 ) -- Claim State   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' CD.ClaimState', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 38 ) -- Discharge Status   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' CD.DischargeStatus', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 50 ) -- ICN   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' CPVA.ICN', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 51 ) -- MRN   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' PD.MedRecNo', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 52 ) -- Reviewed   
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             IF( @Values = 1 )
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + 'CR.ClaimID IS NOT NULL';
                                                                 END;
                                                             ELSE
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + 'CR.ClaimID IS NULL';
                                                                 END;
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 53 ) -- Los
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' (CD.Los)  ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 54 ) -- Age
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), ' dbo.CalculateAge(CAST(PD.DOB AS DATE), CAST(CD.StatementThru AS DATE)) ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 55 ) -- Check Date
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), '(CPVA.Checkdate) ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 56 ) -- Check Number
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), '(CPVA.CheckNumber) ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @ClaimField = 57 ) -- Adjudicated Contract Name
                                                         BEGIN
                                                             IF @DateFlag = 0
                                                                 BEGIN
                                                                     SET @ClaimSubQry = @ClaimSubQry + ' WHERE ';
                                                                     SET @DateFlag = 1;
                                                                 END;
                                                             SET @ClaimSubQry = @ClaimSubQry + [dbo].[GetFilterCodeQuery]( CONVERT(VARCHAR(MAX), @Values), '(CPVA.ContractID) ', @OperatorValue, @ClaimField );
                                                             SET @Flag = 1;
                                                         END;
                                                     IF( @Count > 1
                                                     AND @FLAG = 1
                                                       )
                                                     SET @ClaimSubQry = @ClaimSubQry + ' AND ';
                                                     SET @Count = @Count - 1;
                                                 END;
                                                     IF @SubQueryFlag > 0
                                                    AND @FLAG = 1
                                                     SET @ClaimSubQry = @ClaimSubQry + ' AND ' + @ClaimChargesSubQry;
                                                     ELSE
                                                     IF @SubQueryFlag > 0
                                                     SET @ClaimSubQry = @ClaimSubQry + @ClaimChargesSubQry;
                                                     IF @IsReadjudicate = 1
                                                         BEGIN
                                                             SET @ClaimSubQry = @ClaimSubQry + ' AND  ACC.IsClaimAdjudicated = 0 ';
                                                         END;
                                                             RETURN( @ClaimSubQry );
                                                         END;