CREATE FUNCTION [dbo].[CheckCodesInContractServiceTypes](
                @ServiceLineTypeID     BIGINT,
                @ContractID            BIGINT,
                @Codes                 VARCHAR(8000),
                @ContractServiceTypeID BIGINT )
RETURNS @TempTable TABLE( IsValidCode BIT,
                          IsIDFound   BIT )
AS
     BEGIN
     DECLARE @IsValidCode BIT = 0;
     DECLARE @IncludedCode VARCHAR(MAX);
     DECLARE @ExcludedCode VARCHAR(MAX);
     DECLARE @IsValidIncludedCode BIT = 0;
     DECLARE @IsValidExcludedCode BIT = 0;
     DECLARE @ContractServiceLineID BIGINT = 0;
     DECLARE @IsContractServiceLineIDFound BIT;
     SELECT @ContractServiceLineID = ContractServiceLineID,
            @IncludedCode = IncludedCode,
            @ExcludedCode = ExcludedCode
     FROM [dbo].[ContractServiceLines] CSL
     WHERE EXISTS( 
                   SELECT 1
                   FROM ContractServiceLinePaymentTypes CSLT
                   WHERE ISNULL(CSLT.ContractID, 0) = ISNULL(@ContractID, 0)
                     AND ISNULL(CSLT.ContractServiceTypeID, 0) = ISNULL(@ContractServiceTypeID, 0)
                     AND ISNULL(CSLT.ServiceLineTypeId, 0) = ISNULL(@ServiceLineTypeID, 0)
                     AND CSLT.ContractServiceLineID = CSL.ContractServiceLineID );
     IF @ContractServiceLineID > 0
     SET @IsContractServiceLineIDFound = 1;
     IF( @Codes != '' )
         BEGIN
             IF( @IncludedCode != '' )
             SET @IsValidIncludedCode = dbo.CheckCodeExist( @Codes, @IncludedCode );
             IF( @ExcludedCode != '' )
             SET @IsValidExcludedCode = dbo.CheckCodeExist( @Codes, @ExcludedCode );
             IF( @IsValidIncludedCode = 1
             AND @IsValidExcludedCode = 0
               )
             SET @IsValidCode = 1;
         END;
             INSERT INTO @TempTable
                    SELECT @IsValidCode,
                           @IsContractServiceLineIDFound;
             RETURN;
             END;