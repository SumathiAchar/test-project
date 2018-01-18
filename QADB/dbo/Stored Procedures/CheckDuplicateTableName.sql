CREATE PROCEDURE [dbo].[CheckDuplicateTableName](@TableName  VARCHAR(50),
                                                 @FacilityId BIGINT)
AS
     BEGIN
         DECLARE @IsExists BIT= 0;
         IF EXISTS
         (
             SELECT *
             FROM dbo.ClaimFieldDocs
             WHERE TableName = @TableName
                   AND FacilityId = @FacilityId
         )
             BEGIN
                 SET @IsExists = 1;
             END;
         SELECT @IsExists;
     END;