CREATE FUNCTION [dbo].[CheckCodesClaimFieldSelections](
                @ContractID            BIGINT,
                @ContractServiceTypeID BIGINT,
                @ClaimFieldID          BIGINT,
                @Codes                 VARCHAR(100))
RETURNS @TempTable TABLE( ISVALIDCODE BIT,
                          ISIDFOUND   BIT )
AS
     BEGIN
     DECLARE @IsValidCode BIT = 0;
     DECLARE @IsContractServiceLineIdFound BIT = 0;
     DECLARE @IncludedCode VARCHAR(8000);
     DECLARE @ContractServiceLineID BIGINT = 0;
     DECLARE @OperatorID BIGINT;
     SELECT @ContractServiceLineID = CS.ContractServiceLineID,
            @IncludedCode = [Values],
            @OperatorID = OperatorID
     FROM --ContractServiceLinePaymentTypes AS CSL 	JOIN 
     ContractServiceLineClaimFieldSelection AS CS --ON CS.ContractServiceLineID = CSL.ContractServiceLineID
     JOIN [ref.ClaimField] AS CF ON CF.ClaimFieldID = CS.ClaimFieldID
                                AND CS.ClaimFieldID = @ClaimFieldID
     WHERE CS.CONTRACTSERVICELINEID = ( 
                                        SELECT TOP 1 ContractServiceLineID
                                        FROM ContractServiceLinePaymentTypes CSL
                                        WHERE ISNULL(CSL.ContractID, 0) = @Contractid
                                          AND ISNULL(CSL.ContractServiceTypeID, 0) = @ContractServiceTypeID
                                          AND ISNULL(CSL.ServiceLineTypeID, 0) = 7 );
     IF @ContractServiceLineID > 0
         BEGIN
             SET @IsContractServiceLineIdFound = 1;
         END;
             IF @IncludedCode != ''
                 BEGIN
                     IF ISNUMERIC(@IncludedCode) = ISNUMERIC(@Codes)
                         BEGIN
                             --IF it is not a number that means we need to do only two operations
                             IF ISNUMERIC(@IncludedCode) = 0
                                 BEGIN
                                     -----------------------1 Not Equal------------------------------
                                     ----------------------- 3 = ------------------------------------
                                     IF(( @OperatorID = 1
                                      AND @Codes <> @IncludedCode
                                        )
                                     OR ( @Operatorid = 3
                                      AND @Codes = @IncludedCode
                                        )
                                       )
                                     SET @IsValidCode = 1;
                                 END;
                         END;
                     ELSE --IF it is a number that means we need to do all four operations
                         BEGIN
                             ----------------------- 1 Not Equal ----------------------------- OR
                             ----------------------- 2 > ------------------------------------- OR
                             ----------------------- 3 = ------------------------------------- OR
                             ----------------------- 4 < -------------------------------------
                             IF(( @OperatorID = 1
                              AND CONVERT(INT, @Codes) <> CONVERT(INT, @IncludedCode)
                                )
                             OR ( @OperatorID = 2
                              AND CONVERT(INT, @Codes) > CONVERT(INT, @IncludedCode)
                                )
                             OR ( @OperatorID = 3
                              AND CONVERT(INT, @Codes) = CONVERT(INT, @IncludedCode)
                                )
                             OR ( @OperatorID = 4
                              AND CONVERT(INT, @Codes) < CONVERT(INT, @IncludedCode)
                                )
                               )
                             SET @IsValidCode = 1;
                         END;
                 END;
                         INSERT INTO @TempTable
                                SELECT @IsValidCode,
                                       @IsContractServiceLineIdFound;
                         RETURN;
                         END;
GO