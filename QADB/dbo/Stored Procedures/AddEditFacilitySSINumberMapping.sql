/****************************************************************************
 *   Name         : AddEditFacilitySSINumberMapping
 *   Author       : Manikandan
 *   Date         : 18/Feb/2016
 *   Module       : User management
 *   Description  : Insert/Update Payment Type StopLoss Information into database
 *****************************************************************************/

CREATE PROCEDURE [dbo].[AddEditFacilitySSINumberMapping](@FacilityID BIGINT,
                                                         @SsiNumber  VARCHAR(200))
AS
     BEGIN
         SET NOCOUNT ON;
         DECLARE @Currentdate DATETIME= GETUTCDATE(), @Transactionname VARCHAR(100)= 'AddEditFacilityAndSSINumberMapping';

         --taking distinct ssi numbers 
         DECLARE @Ssinumbertable TABLE(SSINUMBER BIGINT);
	    DECLARE @Ssinumbers TABLE( SSINumber BIGINT );
         INSERT INTO @Ssinumbertable
                SELECT DISTINCT
                       items
                FROM [dbo].[Split](@SsiNumber, ',')
                WHERE items <> '';
         BEGIN TRY
             BEGIN TRAN @Transactionname;
             IF @SsiNumber IS NOT NULL
                 BEGIN
			  INSERT INTO @Ssinumbers
                           SELECT SSINumber
                           FROM DBO.Facility_SSINumber
                           WHERE FacilityId = @FacilityID
                             AND SSINumber NOT IN( 
                                                   SELECT SSINUMBER
                                                   FROM @Ssinumbertable );
                     DELETE FROM DBO.FACILITY_SSINUMBER
                     WHERE FACILITYID = @FacilityID;
                     --Insert Facility SSI Number into Table
                     INSERT INTO DBO.FACILITY_SSINUMBER
                     (FACILITYID,
                      SSINUMBER,
                      INSERTDATE,
                      UPDATEDATE
                     )
                            SELECT @FacilityID,
                                   SSINUMBER,
                                   @Currentdate,
                                   NULL
                            FROM @Ssinumbertable;
                     DECLARE @Payer TABLE
                     (PayerName       VARCHAR(MAX),
                      ContractPayerID INT
                     );
				  DECLARE @SelectedPayers TABLE( PayerName       VARCHAR(MAX),
                                          ContractPayerID INT );	
                     INSERT INTO @Payer
                            SELECT DISTINCT
                                   P.Name AS PayerName,
                                   CP.ContractPayerID
                            FROM DBO.PAYERBYSITES AS P
                                 INNER JOIN DBO.CONTRACTPAYERS CP ON REPLACE(P.Name, CHAR(160), CHAR(32)) = REPLACE(CP.PayerName, CHAR(160), CHAR(32))
                                 INNER JOIN @Ssinumbers SsiTable ON SsiTable.SSINumber = P.SSINumber
						   INNER JOIN Facility_SSINumber FSS on FSS.SSINumber = P.SSINumber
                            WHERE ISNULL(P.Name, '') <> ''
					   AND FSS.FacilityID = @FacilityID;

					   INSERT INTO @SelectedPayers
                           SELECT DISTINCT P.Name AS PayerName,
                                           CP.ContractPayerID
                           FROM DBO.PAYERBYSITES AS P
                                INNER JOIN DBO.CONTRACTPAYERS CP ON REPLACE(P.Name, CHAR(160), CHAR(32)) = REPLACE(CP.PayerName, CHAR(160), CHAR(32))
                                INNER JOIN @Ssinumbertable SsiTable ON SsiTable.SSINumber = P.SSINumber
						  INNER JOIN Facility_SSINumber FSS on FSS.SSINumber = P.SSINumber
                           WHERE ISNULL(P.Name, '') <> ''
					  AND FSS.FACILITYID = @FacilityID ;

                     IF EXISTS
                     (
                         SELECT 1
                         FROM @payer
                     )
                         BEGIN
                             DELETE FROM CONTRACTPAYERS
                            WHERE ContractPayerID IN( SELECT ContractPayerID
                                                      FROM @payer )
										    AND ContractPayerID NOT IN( SELECT ContractPayerID
                                                      FROM @SelectedPayers );
                         END;
                 END;

             --Check Any Transaction happened than commit transaction
             COMMIT TRANSACTION @Transactionname;
         END TRY
         BEGIN CATCH
             ROLLBACK TRAN @TransactionName;
             EXEC RaiseErrorInformation;
         END CATCH;
     END;