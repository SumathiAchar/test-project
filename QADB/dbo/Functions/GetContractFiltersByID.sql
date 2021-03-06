CREATE FUNCTION [dbo].[GetContractFiltersById]
(@Contractid            BIGINT,
 @Contractservicetypeid BIGINT
)
RETURNS @Temptable TABLE
(FILTERVALUES        VARCHAR(MAX),
 FILTERNAME          VARCHAR(MAX),
 ISSERVICETYPEFILTER BIT,
 SERVICELINETYPEID   BIGINT,
 PAYMENTTYPEID       BIGINT
)
AS

/****************************************************************************  
 *   Name         : GetContractFiltersById
 *   Author       : Vishesh Bhawsar  
 *   Date         : 01/30/2014  
 *   Module       : PaymentType Container
 *   Description  : Gets contract filter criteria to be displayed on paymentType Container
 *	 Modified By  : Sheshagiri on 07/23/2014 to include Non Fee Schedule parameter

 *  SELECT GetContractFiltersById (1,'350,450',2)
 *****************************************************************************/

     BEGIN
         DECLARE @Temp_Contractfiltertable TABLE
         (SERVICELINETYPEID        BIGINT,
          INCLUDEDCODESVALUESTRING VARCHAR(MAX),
          PAYMENTTYPEID            BIGINT
         );
         DECLARE @Valuestringdata VARCHAR(MAX);
         DECLARE @Valueexcludedstringdata VARCHAR(MAX);
         DECLARE @Fullservicelinecodedata VARCHAR(MAX); 
  
         --------------------- For BillType ServiceLine------------------------  

         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+CS.INCLUDEDCODE,
                @Valueexcludedstringdata = COALESCE(@Valueexcludedstringdata+', ', '')+CS.EXCLUDEDCODE
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN CONTRACTSERVICELINES AS CS ON CS.CONTRACTSERVICELINEID = CSL.CONTRACTSERVICELINEID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.SERVICELINETYPEID = 1;
         IF @Valueexcludedstringdata IS NOT NULL
             BEGIN
                 SET @Valueexcludedstringdata = '<> '+@Valueexcludedstringdata;
             END;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 SET @Valuestringdata = '= '+@Valuestringdata;
             END;
         SET @Fullservicelinecodedata = CONCAT(COALESCE(@Valuestringdata, ''), CHAR(13) + CHAR(10), COALESCE(@Valueexcludedstringdata, ''));
         INSERT INTO @Temp_Contractfiltertable
         (SERVICELINETYPEID,
          INCLUDEDCODESVALUESTRING
         )
         VALUES
         (1,
          @Fullservicelinecodedata
         );
         SELECT @Valuestringdata = NULL;
         SELECT @Valueexcludedstringdata = NULL;
         SELECT @Fullservicelinecodedata = NULL;
         --------------------- For RevenueCode ServiceLine ------------------------  

         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+CS.INCLUDEDCODE,
                @Valueexcludedstringdata = COALESCE(@Valueexcludedstringdata+', ', '')+CS.EXCLUDEDCODE
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.CONTRACTSERVICELINES AS CS ON CS.CONTRACTSERVICELINEID = CSL.CONTRACTSERVICELINEID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.SERVICELINETYPEID = 2;
         IF @Valueexcludedstringdata IS NOT NULL
             BEGIN
                 SET @Valueexcludedstringdata = '<> '+@Valueexcludedstringdata;
             END;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 SET @Valuestringdata = '= '+@Valuestringdata;
             END;
         SET @Fullservicelinecodedata = CONCAT(COALESCE(@Valuestringdata, ''), CHAR(13) + CHAR(10), COALESCE(@Valueexcludedstringdata, ''));
         INSERT INTO @Temp_Contractfiltertable
         (SERVICELINETYPEID,
          INCLUDEDCODESVALUESTRING
         )
         VALUES
         (2,
          @Fullservicelinecodedata
         );
         SELECT @Valuestringdata = NULL;
         SELECT @Valueexcludedstringdata = NULL;
         SELECT @Fullservicelinecodedata = NULL;
         --------------------- For CPT ServiceLine------------------------  

         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+CS.INCLUDEDCODE,
                @Valueexcludedstringdata = COALESCE(@Valueexcludedstringdata+', ', '')+CS.EXCLUDEDCODE
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.CONTRACTSERVICELINES AS CS ON CS.CONTRACTSERVICELINEID = CSL.CONTRACTSERVICELINEID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.SERVICELINETYPEID = 3;
         IF @Valueexcludedstringdata IS NOT NULL
             BEGIN
                 SET @Valueexcludedstringdata = '<> '+@Valueexcludedstringdata;
             END;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 SET @Valuestringdata = '= '+@Valuestringdata;
             END;
         SET @Fullservicelinecodedata = CONCAT(COALESCE(@Valuestringdata, ''), CHAR(13) + CHAR(10), COALESCE(@Valueexcludedstringdata, ''));
         INSERT INTO @Temp_Contractfiltertable
         (SERVICELINETYPEID,
          INCLUDEDCODESVALUESTRING
         )
         VALUES
         (3,
          @Fullservicelinecodedata
         );
         SELECT @Valuestringdata = NULL;
         SELECT @Valueexcludedstringdata = NULL;
         SELECT @Fullservicelinecodedata = NULL;
         --------------------- For DRG ServiceLine------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+CS.INCLUDEDCODE,
                @Valueexcludedstringdata = COALESCE(@Valueexcludedstringdata+', ', '')+CS.EXCLUDEDCODE
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.CONTRACTSERVICELINES AS CS ON CS.CONTRACTSERVICELINEID = CSL.CONTRACTSERVICELINEID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.SERVICELINETYPEID = 4;
         IF @Valueexcludedstringdata IS NOT NULL
             BEGIN
                 SET @Valueexcludedstringdata = '<> '+@Valueexcludedstringdata;
             END;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 SET @Valuestringdata = '= '+@Valuestringdata;
             END;
         SET @Fullservicelinecodedata = CONCAT(COALESCE(@Valuestringdata, ''), CHAR(13) + CHAR(10), COALESCE(@Valueexcludedstringdata, ''));
         INSERT INTO @Temp_Contractfiltertable
         (SERVICELINETYPEID,
          INCLUDEDCODESVALUESTRING
         )
         VALUES
         (4,
          @Fullservicelinecodedata
         );
         SELECT @Valuestringdata = NULL;
         SELECT @Valueexcludedstringdata = NULL;
         SELECT @Fullservicelinecodedata = NULL;
         --------------------- For DiagnosisCode ServiceLine------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+CS.INCLUDEDCODE,
                @Valueexcludedstringdata = COALESCE(@Valueexcludedstringdata+', ', '')+CS.EXCLUDEDCODE
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.CONTRACTSERVICELINES AS CS ON CS.CONTRACTSERVICELINEID = CSL.CONTRACTSERVICELINEID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.SERVICELINETYPEID = 5;
         IF @Valueexcludedstringdata IS NOT NULL
             BEGIN
                 SET @Valueexcludedstringdata = '<> '+@Valueexcludedstringdata;
             END;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 SET @Valuestringdata = '= '+@Valuestringdata;
             END;
         SET @Fullservicelinecodedata = CONCAT(COALESCE(@Valuestringdata, ''), CHAR(13) + CHAR(10), COALESCE(@Valueexcludedstringdata, ''));
         INSERT INTO @Temp_Contractfiltertable
         (SERVICELINETYPEID,
          INCLUDEDCODESVALUESTRING
         )
         VALUES
         (5,
          @Fullservicelinecodedata
         );
         SELECT @Valuestringdata = NULL;
         SELECT @Valueexcludedstringdata = NULL;
         SELECT @Fullservicelinecodedata = NULL;
         --------------------- For Procedure Code ServiceLine------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+CS.INCLUDEDCODE,
                @Valueexcludedstringdata = COALESCE(@Valueexcludedstringdata+', ', '')+CS.EXCLUDEDCODE
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.CONTRACTSERVICELINES AS CS ON CS.CONTRACTSERVICELINEID = CSL.CONTRACTSERVICELINEID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.SERVICELINETYPEID = 6;
         IF @Valueexcludedstringdata IS NOT NULL
             BEGIN
                 SET @Valueexcludedstringdata = '<> '+@Valueexcludedstringdata;
             END;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 SET @Valuestringdata = '= '+@Valuestringdata;
             END;
         SET @Fullservicelinecodedata = CONCAT(COALESCE(@Valuestringdata, ''), CHAR(13) + CHAR(10), COALESCE(@Valueexcludedstringdata, ''));
         INSERT INTO @Temp_Contractfiltertable
         (SERVICELINETYPEID,
          INCLUDEDCODESVALUESTRING
         )
         VALUES
         (6,
          @Fullservicelinecodedata
         );
         SELECT @Valuestringdata = NULL;
         SELECT @Valueexcludedstringdata = NULL;
         SELECT @Fullservicelinecodedata = NULL;
         --------------------- For Claim Fields ServiceLine ------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+CF.[Text]+' '+CFO.OPERATORTYPE+' '+CS.[VALUES]
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.CONTRACTSERVICELINECLAIMFIELDSELECTION AS CS ON CS.CONTRACTSERVICELINEID = CSL.CONTRACTSERVICELINEID
              JOIN [REF.CLAIMFIELD] AS CF ON CF.CLAIMFIELDID = CS.CLAIMFIELDID
              JOIN [REF.CLAIMFIELDOPERATORS] AS CFO ON CFO.OPERATORID = CS.OPERATORID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.SERVICELINETYPEID = 7;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (SERVICELINETYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (7,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  



         --------------------- For Table Selection ServiceLine------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', ' ')+CF.[Text]+' '+CFO.OPERATORTYPE+' '+CFD.TABLENAME
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.CONTRACTSERVICELINETABLESELECTION AS CS ON CS.CONTRACTSERVICELINEID = CSL.CONTRACTSERVICELINEID
              JOIN CLAIMFIELDDOCS AS CFD ON CFD.CLAIMFIELDDOCID = CS.CLAIMFIELDDOCID
              JOIN [Ref.ClaimField] AS CF WITH (NOLOCK) ON CF.ClaimFieldID = CS.ClaimFieldId
              JOIN [REF.CLAIMFIELDOPERATORS] AS CFO ON CFO.OPERATORID = CS.OPERATOR
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.SERVICELINETYPEID = 8;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (SERVICELINETYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (8,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  

 
    
         --------------------- For ASC Fee Schedules Payment Type------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+' Percentage  = '+ISNULL(CONVERT( VARCHAR(MAX), PTFS.[PRIMARY]), '')+'%, '+ISNULL(CONVERT(VARCHAR(MAX), PTFS.SECONDARY), '')+'%, '+ISNULL(CONVERT(VARCHAR(MAX), PTFS.TERTIARY), '')+'%, '+ISNULL(CONVERT(VARCHAR(MAX), PTFS.QUATERNARY), '')+'%, '+ISNULL(CONVERT(VARCHAR(MAX), PTFS.OTHERS), '')+'%, NFS= '+ISNULL(CONVERT(VARCHAR(MAX), PTFS.NONFEESCHEDULE), '')+'%, '+' Table Name = '+CFD.TABLENAME+', '+ASCOP.ASCFeeScheduleOptionName
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPEASCFEESCHEDULES AS PTFS ON PTFS.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
              JOIN CLAIMFIELDDOCS AS CFD ON CFD.CLAIMFIELDDOCID = PTFS.CLAIMFIELDDOCID
              JOIN ASCFEESCHEDULEOPTIONS AS ASCOP ON PTFS.SelectedOption = ASCOP.ASCFeeScheduleOptionId
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 1;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (1,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  
   
         --------------------- For DRG Payment Payment Type------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+' Base Rate = $'+CONVERT( VARCHAR(MAX), REPLACE(RTRIM(REPLACE(REPLACE(RTRIM(LTRIM(STR(PTDRG.BASERATE, 15, 2))), ' ', '0'), '.', ' ')), ' ', '.'))+', '+'Table Name = '+CFD.TABLENAME+''
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPEDRGPAYMENT AS PTDRG ON PTDRG.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
              JOIN CLAIMFIELDDOCS AS CFD ON CFD.CLAIMFIELDDOCID = PTDRG.CLAIMFIELDDOCID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 2;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (2,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  
         --------------------- For Fee Schedules Payment Type------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+' FS = '+ISNULL(CONVERT( VARCHAR(MAX), PTFS.FEESCHEDULE), '')+'%, '+'NFS= '+ISNULL(CONVERT(VARCHAR(MAX), PTFS.NONFEESCHEDULE), '')+'%'+', '+' Table Name = '+CFD.TABLENAME + CASE PTFS.IsObserveUnits
                            WHEN 1
                            THEN ', Observe Service Units'
                            ELSE ''
                            END
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPEFEESCHEDULES AS PTFS ON PTFS.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
              JOIN CLAIMFIELDDOCS AS CFD ON CFD.CLAIMFIELDDOCID = PTFS.CLAIMFIELDDOCID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 3;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (3,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  
         --------------------- For Medicare IP Payment Payment Type ------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+' Percentage ='+CONVERT( VARCHAR(MAX), PTMIPP.INPATIENT)+'%, Formula ='+PTMIPP.Formula
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPEMEDICAREIPPAYMENT AS PTMIPP ON PTMIPP.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 4;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (4,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  
         --------------------- For Medicare OP Payment Payment Type  ------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+' Percentage ='+CONVERT( VARCHAR(MAX), PTMOPP.OUTPATIENT)+'%'
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPEMEDICAREOPPAYMENT AS PTMOPP ON PTMOPP.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 5;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (5,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  

         --------------------- For Medicare Lab Fee Schedule Payment Type ------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+' Percentage ='+CONVERT( VARCHAR(MAX), PTMLFSP.PERCENTAGE)+'%'
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPEMEDICARELABFEESCHEDULE AS PTMLFSP ON PTMLFSP.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 13;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (13,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  

         --------------------- For Per Case Payment Type ----------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+' Rate = $'+CONVERT( VARCHAR(MAX), REPLACE(RTRIM(REPLACE(REPLACE(RTRIM(LTRIM(STR(PTPC.RATE, 15, 2))), ' ', '0'), '.', ' ')), ' ', '.'))+CASE
                                                                                                                                                                                                                   WHEN PTPC.MAXCASESPERDAY IS NOT NULL
                                                                                                                                                                                                                   THEN ', Max Cases Per Day = '+CONVERT(VARCHAR(MAX), PTPC.MAXCASESPERDAY)
                                                                                                                                                                                                                   ELSE ''
                                                                                                                                                                                                               END
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPEPERCASE AS PTPC ON PTPC.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 6;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (6,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  
  
         --------------------- For Per Diem Payment Type ------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+' Days ='+CONCAT(PTPD.DAYSFROM, '-', PTPD.DAYSTO)+', '+'Rate = $'+CONVERT( VARCHAR(MAX), REPLACE(RTRIM(REPLACE(REPLACE(RTRIM(LTRIM(STR(PTPD.RATE, 15, 2))), ' ', '0'), '.', ' ')), ' ', '.'))
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPEPERDIEM AS PTPD ON PTPD.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 7;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (7,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  
  
         --------------------- For Percentage Discount Payment Payment Type ------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+' Percentage ='+CONVERT( VARCHAR(MAX), PTPDP.PERCENTAGE)+'%'
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPEPERCENTAGEDISCOUNTPAYMENT AS PTPDP ON PTPDP.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 8;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (8,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  
    
         --------------------- For Per Visit Payment Type ------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+' Rate = $'+CONVERT( VARCHAR(MAX), REPLACE(RTRIM(REPLACE(REPLACE(RTRIM(LTRIM(STR(PTPV.RATE, 15, 2))), ' ', '0'), '.', ' ')), ' ', '.'))
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPEPERVISIT AS PTPV ON PTPV.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 9;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (9,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  


         -------------------------------------For Custom Payment------------------------------------------------------

         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+' Table Name = '+CFD.TABLENAME+', '+'ClaimField ='+cf.[Text]+', '+'Formula = '+PTFS.Formula
	    +ISNULL([dbo].[GetCustomMultipliers](PTFS.MultiplierFirst,PTFS.MultiplierSecond,PTFS.MultiplierThird,PTFS.MultiplierFour,PTFS.MultiplierOther),'')
	    +CASE
		  WHEN PTFS.IsObserveServiceUnit = 1
		  THEN ', Observe Service Units Count'
		  ELSE ''
		  END
	    +CASE 
		  WHEN PTFS.ObserveServiceUnitLimit IS NOT NULL
		  THEN ', Limit: '+PTFS.ObserveServiceUnitLimit
		  ELSE ''
		  END
	    +CASE 
		  WHEN PTFS.IsPerDayOfStay  = 1
		  THEN ', Scope: Per Day of Stay'
		  ELSE ''
		  END
		+CASE 
		  WHEN PTFS.IsPerCode  = 1
		  THEN ', Scope: Per Code'
		  ELSE ''
		  END
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL WITH (NOLOCK)
              JOIN dbo.PaymentTypeCustomTable AS PTFS WITH (NOLOCK) ON PTFS.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
              JOIN CLAIMFIELDDOCS AS CFD WITH (NOLOCK) ON CFD.CLAIMFIELDDOCID = PTFS.CLAIMFIELDDOCID
              JOIN dbo.[ref.ClaimField] cf ON PTFS.ClaimFieldId = cf.ClaimFieldId
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 14;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (14,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;    

   
         --------------------- For Stop Loss Payment Type ------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+'Threshold = '+PTSL.THRESHOLD+' , '+'Percentage = '+CONVERT( VARCHAR(MAX), PTSL.PERCENTAGE)+'%'++CASE
                                                                                                                                                                            WHEN PTSL.DAYS IS NOT NULL
                                                                                                                                                                            THEN ' , Days = '+PTSL.DAYS
                                                                                                                                                                            ELSE ''
                                                                                                                                                                        END+CASE
                                                                                                                                                                                WHEN PTSL.REVCODE IS NOT NULL
                                                                                                                                                                                THEN ' , Rev Codes = '+PTSL.REVCODE
                                                                                                                                                                                ELSE ''
                                                                                                                                                                            END+CASE
                                                                                                                                                                                    WHEN PTSL.CPTCODE IS NOT NULL
                                                                                                                                                                                    THEN ' , CPT/HCPCS Codes = '+PTSL.CPTCODE
                                                                                                                                                                                    ELSE ''
                                                                                                                                                                                END+CASE
                                                                                                                                                                                        WHEN PTSL.ISEXCESSCHARGE = 1
                                                                                                                                                                                        THEN+' , Condition = '+CASE
                                                                                                                                                                                                                   WHEN PTSL.STOPLOSSCONDITIONID = 1
                                                                                                                                                                                                                   THEN 'Total Charge Lines, Excess Charges'
                                                                                                                                                                                                                   WHEN PTSL.STOPLOSSCONDITIONID = 2
                                                                                                                                                                                                                   THEN 'Per Charge Line, Excess Charges'
                                                                                                                                                                                                                   WHEN PTSL.STOPLOSSCONDITIONID = 3
                                                                                                                                                                                                                   THEN 'Per Day of Stay, Excess Charges'
                                                                                                                                                                                                                   ELSE ''
                                                                                                                                                                                                               END
                                                                                                                                                                                        ELSE+' , Condition = '+CASE
                                                                                                                                                                                                                   WHEN PTSL.STOPLOSSCONDITIONID = 1
                                                                                                                                                                                                                   THEN 'Total Charge Lines'
                                                                                                                                                                                                                   WHEN PTSL.STOPLOSSCONDITIONID = 2
                                                                                                                                                                                                                   THEN 'Per Charge Line'
                                                                                                                                                                                                                   WHEN PTSL.STOPLOSSCONDITIONID = 3
                                                                                                                                                                                                                   THEN 'Per Day of Stay'
                                                                                                                                                                                                                   ELSE ''
                                                                                                                                                                                                               END
                                                                                                                                                                                    END
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPESTOPLOSS AS PTSL ON PTSL.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 10;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (10,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;  

         --------------------- For Lesser Of Payment Type ------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+(CASE PTLO.ISLESSEROF
                                                                            WHEN 1
                                                                            THEN 'Lesser Of = '
                                                                            ELSE 'Greater Of = '
                                                                        END)+CONVERT( VARCHAR(MAX), PTLO.PERCENTAGE)+'%'
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPELESSEROF AS PTLO ON PTLO.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 11;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (11,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL; 
  
         --------------------- For CAP Payment Type  ------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+'Threshold =$'+CONVERT( VARCHAR(MAX), REPLACE(RTRIM(REPLACE(REPLACE(RTRIM(LTRIM(STR(PTC.THRESHOLD, 15, 2))), ' ', '0'), '.', ' ')), ' ', '.'))
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PAYMENTTYPECAP AS PTC ON PTC.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
         WHERE(CSL.CONTRACTID = @Contractid
               OR @Contractservicetypeid = CONTRACTSERVICETYPEID)
              AND CSL.PAYMENTTYPEID = 12;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (12,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;

         --------------------- For Medicare Sequester Payment Type ----------------------------  
         SELECT @Valuestringdata = COALESCE(@Valuestringdata+', ', '')+' Percentage = '+CONVERT( VARCHAR(MAX), PTMS.PERCENTAGE)+'%'
         FROM dbo.CONTRACTSERVICELINEPAYMENTTYPES AS CSL
              JOIN dbo.PaymentTypeMedicareSequester AS PTMS ON PTMS.PAYMENTTYPEDETAILID = CSL.PAYMENTTYPEDETAILID
         WHERE(CSL.CONTRACTID = @Contractid)
              AND CSL.PAYMENTTYPEID = 15;
         IF @Valuestringdata IS NOT NULL
             BEGIN
                 INSERT INTO @Temp_Contractfiltertable
                 (PAYMENTTYPEID,
                  INCLUDEDCODESVALUESTRING
                 )
                 VALUES
                 (15,
                  @Valuestringdata
                 );
             END;
         SELECT @Valuestringdata = NULL;
         INSERT INTO @Temptable
                SELECT ISNULL(INCLUDEDCODESVALUESTRING, '') AS FILTERVALUES,
                       SERVICELINENAME AS FILTERNAME,
                       CONVERT( BIT, 1) AS ISSERVICETYPEFILTER,
                       SLT.SERVICELINETYPEID AS SERVICELINETYPEID,
                       NULL AS PAYMENTTYPEID
                FROM @Temp_Contractfiltertable AS TEMPTABLE
                     JOIN dbo.[REF.SERVICELINETYPES] AS SLT ON SLT.SERVICELINETYPEID = TEMPTABLE.SERVICELINETYPEID
                     JOIN dbo.CONTRACTSERVICELINEPAYMENTTYPES CSP ON CSP.SERVICELINETYPEID = TEMPTABLE.SERVICELINETYPEID
                WHERE CONTRACTID = @Contractid
                      OR CONTRACTSERVICETYPEID = @Contractservicetypeid
                UNION
                SELECT ISNULL(INCLUDEDCODESVALUESTRING, '') AS FILTERVALUES,
                       (CASE PT.PAYMENTTYPEID
                            WHEN 11
                            THEN 'Lesser/Greater Of'
                            ELSE PAYMENTTYPENAME
                        END) AS FILTERNAME,
                       CONVERT( BIT, 0) AS ISSERVICETYPEFILTER,
                       NULL AS SERVICELINETYPEID,
                       PT.PAYMENTTYPEID AS PAYMENTTYPEID
                FROM @Temp_Contractfiltertable AS TEMPTABLE
                     JOIN dbo.[REF.PAYMENTTYPES] AS PT ON PT.PAYMENTTYPEID = TEMPTABLE.PAYMENTTYPEID
                     JOIN dbo.CONTRACTSERVICELINEPAYMENTTYPES CSP ON CSP.PAYMENTTYPEID = TEMPTABLE.PAYMENTTYPEID
                WHERE CONTRACTID = @Contractid
                      OR CONTRACTSERVICETYPEID = @Contractservicetypeid;
         RETURN;
     END;