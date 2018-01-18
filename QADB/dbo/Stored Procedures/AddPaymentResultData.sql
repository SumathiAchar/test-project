/****************************************************/

--Method Name : AddPaymentResultData
--Module      : Adjudication  
--Created By  : Raj  
--Date        : 2-Aug-2014
--Modified By : 
--Modify Date : 
--Description : Add Payment Result into Contract Adjudication Data

/****************************************************/

CREATE PROCEDURE [dbo].[AddPaymentResultData](
       @TaskID             BIGINT,
       @ServerDateTime     DATETIME,
       @IsDmEntry          BIT,
       @XmlAdjudicatedData dbo.AdjudicatedClaims READONLY )
AS
     BEGIN
        DECLARE @InsertedValues AS TABLE( ContractAdjudicationId BIGINT,
                                          ClaimServiceLineID     INT,
                                          ContractServiceTypeID  BIGINT,
                                          ClaimID                BIGINT );
        DECLARE @ModelID               BIGINT,
                @AdjudicatedClaimCount BIGINT,
                @ClaimTable AS            [dbo].[ClaimList];
        BEGIN TRY
            BEGIN

                /***START*****Logic for inserting  claim record as well as claim charge record*********************/

                SELECT @ModelID = ModelID
                FROM dbo.TrackTasks WITH ( NOLOCK )
                WHERE TaskID = @TaskID;
                DECLARE @RunningStatus INT;
                SELECT @RunningStatus = RunningStatus
                FROM TrackTasks
                WHERE TaskId = @TaskID;
                IF( @RunningStatus NOT IN( 130 )
                  )
                    BEGIN
                        --Insert Record into ContractAdjudications table
                        INSERT INTO dbo.ContractAdjudications
                        OUTPUT Inserted.ContractAdjudicationId,
                               Inserted.ClaimServiceLineID,
                               Inserted.ContractServiceTypeID,
                               Inserted.ClaimID
                               INTO @InsertedValues
                               SELECT @ServerDateTime AS InsertDate,
                                      NULL AS UpdateDate,
                                      ContractId,
                                      ServiceTypeId,
                                      ClaimId,
                                      Line,
                                      ClaimStatus,
                                      AdjudicatedValue,
                                      CASE
                                          WHEN( IsInitialEntry = 1
                                            AND IsClaimChargeData = 1
                                              )
                                           OR IsClaimChargeData = 0
                                          THEN ClaimTotalCharges
                                          ELSE 0
                                      END,
                                      IsClaimChargeData,
                                      @ModelID ModelID,
                                      IsInitialEntry,
                                      PaymentTypeDetailId,
                                      PaymentTypeId,
                                      MicrodynEditErrorCodes,
                                      MicrodynPricerErrorCodes,
                                      MicrodynEditReturnRemarks,
                                      0,
                                      MedicareSequesterAmount
                               FROM @XmlAdjudicatedData;
		
                        /***END*******Logic for inserting claim record as well as claim charge record*********************/

                        IF(( SELECT COUNT(*)
                             FROM @InsertedValues ) > 0 )
                            BEGIN
                                INSERT INTO SmartBox
                                       ( ContractAdjudicationId,
                                         Expression,
                                         ExpandedExpression,
                                         ExpressionResult,
                                         CustomExpression,
                                         CustomExpandedExpression,
                                         CustomExpressionResult,
                                         CalculatedAlowedAmount,
                                         IsThresholdFormulaError,
								 MultiplierExpressionResult,
								 IsMultiplierFormulaError,
								 Multiplier,
								 MultiplierExpandedExpression,
								 LimitExpressionResult,
								 IsLimitError,
								 LimitExpandedExpression 
                                       )
                                       SELECT adj.ContractAdjudicationId,
                                              Expression,
                                              ExpandedExpression,
                                              ExpressionResult,
                                              CustomExpression,
                                              CustomExpandedExpression,
                                              CustomExpressionResult,
                                              IntermediateAdjudicatedValue,
                                              IsFormulaError,
									 MultiplierExpressionResult,
								      IsMultiplierFormulaError,
								      Multiplier,
								      MultiplierExpandedExpression,
								      LimitExpressionResult,
								      IsLimitError,
								      LimitExpandedExpression 
                                       FROM @InsertedValues adj
                                            JOIN @XmlAdjudicatedData adjclaims ON ISNULL(adj.ClaimServiceLineID, 0) = ISNULL(adjclaims.Line, 0)
                                                                              AND ISNULL(adj.ContractServiceTypeID, 0) = ISNULL(adjclaims.ServiceTypeId, 0)
                                                                              AND adj.ClaimID = adjclaims.ClaimId
                                       WHERE( adjclaims.ExpressionResult IS NOT NULL
                                           OR adjclaims.CustomExpressionResult IS NOT NULL
                                            )
                                        AND adjclaims.IntermediateAdjudicatedValue IS NOT NULL;
	
                                --Mark claims for a task as adjudicated
                                INSERT INTO @ClaimTable
                                       SELECT TaskClaimId
                                       FROM TaskClaims WITH ( NOLOCK )
                                       WHERE TaskId = @TaskID
                                         AND ClaimID IN( 
                                                         SELECT ClaimId
                                                         FROM @XmlAdjudicatedData
                                                         WHERE IsClaimChargeData = 0
                                                           AND ServiceTypeId IS NULL );
                                UPDATE TC
                                  SET TC.IsAdjudicated = 1
                                FROM dbo.TaskClaims TC WITH ( NOLOCK )
                                     INNER JOIN @ClaimTable CT ON CT.ClaimId = TC.TaskClaimId;
                                SELECT @AdjudicatedClaimCount = AdjudicatedClaimCount
                                FROM dbo.TrackTasks WITH ( NOLOCK )
                                WHERE TaskID = @TaskID;
                                UPDATE dbo.TrackTasks
                                  SET AdjudicatedClaimCount = (SELECT Count(*) FROM TaskClaims WITH (NOLOCK) 
						    WHERE TaskId= @TaskID AND IsAdjudicated = 1)
                                WHERE TaskId = @TaskID;
    
                                -- If DM Entry is enabled than insert claim with contract id
                                IF( @IsDmEntry = 1 )
                                    BEGIN			
                                        --Insert contract claim data into AdjudicatedContractClaim table 
                                        INSERT INTO dbo.AdjudicatedContractClaim
                                               SELECT ContractId,
                                                      ClaimId,
                                                      @ServerDateTime AS Insertdate
                                               FROM @XmlAdjudicatedData
                                               WHERE ContractId IS NOT NULL
                                                 AND IsClaimChargeData = 0
                                                 AND ServiceTypeId IS NULL;
                                    END;
                            END;
                    END;
            END;
        END TRY
        BEGIN CATCH
            BEGIN
                -- If any exception is coming deleting the data from smart box and ContractAdjudications
                DELETE FROM dbo.SmartBox
                WHERE ContractAdjudicationID IN( SELECT ContractAdjudicationId
                                                 FROM @InsertedValues );
                DELETE FROM dbo.ContractAdjudications
                WHERE ContractAdjudicationID IN( SELECT ContractAdjudicationId
                                                 FROM @InsertedValues );
                -- If DM Entry is enabled than delete claim with contract id
                IF( @IsDmEntry = 1 )
                    BEGIN
                        DELETE FROM dbo.AdjudicatedContractClaim
                        WHERE ContractId IN( SELECT ContractId
                                             FROM @XmlAdjudicatedData )
                          AND ClaimId IN( SELECT ClaimId
                                          FROM @XmlAdjudicatedData );
                    END;
                EXEC RaiseErrorInformation;
            END;
        END CATCH;
    END;