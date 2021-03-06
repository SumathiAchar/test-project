/****************************************************************************      
 *   Name         : AddEditContracts  1,1,'11','01/01/2010','01/01/2010',1,1,1,1,1,null,''    
 *   Author       : Vishesh      
 *   Date         : 29/Aug/2013      
 *   Module       : Add/Edit Contract      
 *   Description  : Insert/Update Contract Information      
 *****************************************************************************/

CREATE PROCEDURE [dbo].[AddEditContracts](
       @Contractid           BIGINT,
       @Contractname         VARCHAR(100),
       @Startdate            DATETIME,
       @Enddate              DATETIME,
       @Facilityid           BIGINT,
       @Status               BIT,
       @Nodeid               BIGINT,
       @Parentid             BIGINT,
       @Ismodified           INT,
       @Isclaimstartdate     INT,
       @Isprofessionalclaim  INT,
       @Isinstitutionalclaim INT,
       @Username             VARCHAR(100),
       @Xmlpayerdata         XML,
       @Alertthreshold       INT,
       @PayerCode            VARCHAR(500),
       @CustomField          INT )
AS
    BEGIN
        SET NOCOUNT ON;
        DECLARE @Currentdate   DATETIME = GETUTCDATE(),
                @ClaimToolDesc NVARCHAR(500);
        DECLARE @Action NCHAR(10);
        DECLARE @TempPayersChanges TABLE( ContractID BIGINT,
                                          PayerName  VARCHAR(100));
        DECLARE @TempPayers TABLE( ContractID BIGINT,
                                   PayerName  VARCHAR(100));
        DECLARE @Transactionname VARCHAR(100) = 'AddEditContracts';
        BEGIN TRY
            BEGIN TRAN @Transactionname;
            INSERT INTO @TempPayers
                   SELECT @Contractid,
                          V.X.value( './PayerName[1]', 'VARCHAR(100)' ) AS PayerName
                   FROM @Xmlpayerdata.nodes( '//Payer' ) AS V( X );
            IF ISNULL(@Contractid, 0) = 0
                BEGIN
                    DECLARE @Tmptable TABLE( INSERTEDID BIGINT );
                    SET @Parentid = dbo.GetNodeIDBasedOnStartAndEndDate( @Startdate, @Enddate, @Nodeid );
                    SET @Action = 'Add';
                    --<End>Logic to find the nodeid to add contract in specified hierarchy***************************
                    INSERT INTO CONTRACTHIERARCHY
                           ( PARENTID,
                             NODETEXT,
                             FACILITYID,
                             USERNAME
                           )
                    OUTPUT INSERTED.NodeID
                           INTO @Tmptable
                    VALUES( @Parentid, @Contractname, @Facilityid, @Username );      
		      
                    -- Once UI is done below line need to uncomment 
                    SELECT @Nodeid = INSERTEDID
                    FROM @Tmptable;
                    DELETE FROM @Tmptable;     
		     
                    --Insert Contract information      
                    INSERT INTO DBO.Contracts
                           ( InsertDate,
                             UpdateDate,
                             ContractName,
                             StartDate,
                             EndDate,
                             FacilityID,
                             IsActive,
                             IsModified,
                             NodeID,
                             IsClaimStartDate,
                             IsInstitutionalClaim,
                             IsProfessionalClaim,
                             UserName,
                             ThresholdDaysToExpireAlters,
                             PayerCode,
                             CustomField
                           )
                    OUTPUT INSERTED.CONTRACTID
                           INTO @Tmptable -- inserting id into @TmpTable    
                    VALUES( @Currentdate, NULL, @Contractname, @Startdate, @Enddate, @Facilityid, @Status, @Ismodified, @Nodeid, @Isclaimstartdate, @Isinstitutionalclaim, @Isprofessionalclaim, @Username, @Alertthreshold, @PayerCode, @CustomField);
                    SELECT @Contractid = INSERTEDID
                    FROM @Tmptable;    
                    --Insert data into Payer Table      
                    IF @Xmlpayerdata IS NOT NULL
                        BEGIN
                            INSERT INTO DBO.ContractPayers
                                   ( InsertDate,
                                     UpdateDate,
                                     ContractID,
                                     PayerName
                                   )
                                   SELECT @Currentdate,
                                          NULL,
                                          @Contractid,
                                          PayerName
                                   FROM @TempPayers;
                        END;
                    SELECT @Contractid AS CONTRACTID,
                           @Nodeid AS NODEID;
                END;
            ELSE
                BEGIN
                    SET @Action = 'Modify';
                    DECLARE @Currentstatus BIT;
                    -- TO move contract to active or deactive state
                    SELECT @Currentstatus = ISACTIVE
                    FROM Contracts
                    WHERE ContractID = @Contractid;
                    --Compares data if the data is modified
                    INSERT INTO @TempPayersChanges
                           SELECT ContractID,
                                  PayerName
                           FROM @TempPayers
                           EXCEPT
                           SELECT ContractID,
                                  PayerName
                           FROM dbo.ContractPayers
                           WHERE ContractID = @ContractID;
                    INSERT INTO @TempPayersChanges
                           SELECT ContractID,
                                  PayerName
                           FROM dbo.ContractPayers
                           WHERE ContractID = @ContractID
                           EXCEPT
                           SELECT ContractID,
                                  PayerName
                           FROM @TempPayers;
                    --If contract is modified
                    IF NOT EXISTS( SELECT 1
                                   FROM dbo.Contracts
                                   WHERE ContractID = @ContractID
                                     AND StartDate = @StartDate
                                     AND EndDate = @EndDate
                                     AND IsClaimStartDate = @IsClaimStartDate
                                     AND (( @IsProfessionalClaim IS NULL
                                        AND IsProfessionalClaim IS NULL
                                          )
                                       OR ( IsProfessionalClaim = @IsProfessionalClaim )
                                         )
                                     AND (( @IsInstitutionalClaim IS NULL
                                        AND IsInstitutionalClaim IS NULL
                                          )
                                       OR ( IsInstitutionalClaim = @IsInstitutionalClaim )
                                         )
                                     AND (( @IsModified IS NULL
                                        AND IsModified IS NULL
                                          )
                                       OR ( IsModified = @IsModified )
                                         )
                                     AND (( @PayerCode IS NULL
                                        AND PayerCode IS NULL
                                          )
                                       OR ( PayerCode = @PayerCode )
                                         )
                                     AND (( @CustomField IS NULL
                                        AND CustomField IS NULL
                                          )
                                       OR ( CustomField = @CustomField )
                                         )
                                     AND (( @Alertthreshold IS NULL
                                        AND ThresholdDaysToExpireAlters IS NULL
                                          )
                                       OR ( ThresholdDaysToExpireAlters = @Alertthreshold )
                                         ))
                        BEGIN
                            --Updating Contract GUID
                            EXEC [UpdateContractGUID]
                                 @ContractID,
                                 NULL;
                        END;
                    ELSE
                    IF(( SELECT COUNT(*)
                         FROM @TempPayersChanges ) > 0 )
                        BEGIN
                            EXEC [UpdateContractGUID]
                                 @ContractID,
                                 NULL;
                        END;
                    IF @Currentstatus <> @Status
                        BEGIN
                            --Updating Contract GUID
                            EXEC [UpdateContractGUID]
                                 @Contractid,
                                 NULL;
                            IF @Status = 1
                                BEGIN
                                    EXEC MOVECONTRACTTOACTIVESTATE
                                         @Contractid;
                                END;
                            ELSE
                                BEGIN
                                    EXEC MOVECONTRACTTODEACTIVESTATE
                                         @Contractid;
                                END;
                        END;
                    --Checks if the Contract Values are changed and then update the contract GUID if not changed
                    UPDATE DBO.Contracts
                      SET UpdateDate = @Currentdate,
                          ContractName = @Contractname,
                          StartDate = @Startdate,
                          EndDate = @Enddate,
                          IsModified = @Ismodified,
                          IsClaimStartDate = @Isclaimstartdate,
                          IsInstitutionalClaim = @Isinstitutionalclaim,
                          IsProfessionalClaim = @Isprofessionalclaim,
                          ThresholdDaysToExpireAlters = @Alertthreshold,
                          PayerCode = @PayerCode,
                          CustomField = @CustomField
                    WHERE ContractID = @Contractid;      
 
                    -- To update the contract name inside ContractHierarchy table
                    UPDATE dbo.ContractHierarchy
                      SET NodeText = @Contractname
                    FROM ContractHierarchy AS CH
                         JOIN Contracts AS C ON C.NodeID = CH.NodeID
                    WHERE C.ContractID = @Contractid;


                    --Delete Inserted Payer from Payer table      
                    DELETE dbo.ContractPayers
                    WHERE ContractID = @ContractID;       
      
                    --Insert data into Payer Table      
                    IF @Xmlpayerdata IS NOT NULL
                        BEGIN
                            INSERT INTO dbo.ContractPayers
                                   ( InsertDate,
                                     UpdateDate,
                                     ContractID,
                                     PayerName
                                   )
                                   SELECT @Currentdate,
                                          NULL,
                                          ContractID,
                                          PayerName
                                   FROM @TempPayers;
                        END;
                    SELECT @Contractid AS CONTRACTID,
                           @Nodeid AS NODEID;
                END;
            DECLARE @modelName      NVARCHAR(50),
                    @ModifiedReason NVARCHAR(100) = '';
            SELECT @modelName = dbo.GetModelNameByContractID( @Contractid );
            IF @modelName = 'Primary Model'
                BEGIN
                    SET @ModifiedReason = ( SELECT TOP 1 Text
                                            FROM [dbo].[ref.ContractModifiedReasonCodes] a
                                                 INNER JOIN ContractModifiedReasons b ON b.ContractModifiedReasonCodeID = a.ContractModifiedReasonCodeID
                                            WHERE ContractID = @Contractid
                                            ORDER BY ContractModifiedReasonID DESC );
                    SET @ModifiedReason = CASE
                                              WHEN @ModifiedReason != ''
                                              THEN 'Reason: ' + @ModifiedReason
                                              ELSE ''
                                          END;
                END;
            ELSE
                BEGIN
                    SET @ModifiedReason = ( SELECT 'Contract modified:  ' + ContractName
                                            FROM dbo.GetAuditLogInfoByID( @Contractid, 1 ));
                END;

            --Insert AuditLog information 
            SELECT @ClaimToolDesc = CASE
                                        WHEN @Action = 'Add'
                                        THEN NULL
                                        ELSE @ModifiedReason
                                    END;
            EXEC InsertAuditLog
                 @UserName,
                 @Action,
                 'Contract',
                 @ClaimToolDesc,
                 @Contractid,
                 1;
            --Check Any Transaction happened than commit transaction
            COMMIT TRANSACTION @Transactionname;
        END TRY
        BEGIN CATCH
            ROLLBACK TRAN @TransactionName;
            EXEC RaiseErrorInformation;
        END CATCH;
    END;