-- =============================================
--Author      : Dev
--Literals and Variables
--	@TimeZone can be set as LOCAL or UTC or can be left as empty. Empty is considered as LOCAL
--	@ISODateStyle is the datetime format(121,101,131) -- Ex: 121: YYYY-MM-dd hh:mm:ss.mmm
--	@PHIAccessLogBatchsize: inserts log into logging table in batches
-- =============================================

CREATE PROCEDURE [dbo].[AddPHIAccessAuditTrails](
       @ClaimIDList           CLAIMLIST READONLY,
       @RequestedUserID       UNIQUEIDENTIFIER,
       @RequestedUserName     VARCHAR(100),
       @PHIAccessLogBatchsize INT = 3000,
       @TimeZone              VARCHAR(5) = 'LOCAL',
       @ISODateStyle          INT = 121 )
AS
    BEGIN
        DECLARE @DateViewd       DATETIME,
                @TotalClaimCount INT,
                @LoopCounter     INT = 1;
        SET @Dateviewd = dbo.GetSSIDateTime( @TimeZone, @ISODateStyle );--121 :YYYY-MM-dd hh:mm:ss.mmm	
        --HIPAA logging--
        SELECT @TotalClaimCount = COUNT(1)
        FROM @ClaimIDList;
        WHILE @LoopCounter <= @TotalClaimCount
            BEGIN
                INSERT INTO PHIAccessAuditTrails
                       SELECT @RequestedUserID,
                              @RequestedUserName,
                              STUFF(( 
                                      SELECT ', ' + CONVERT( VARCHAR(MAX), ClaimID)
                                      FROM @ClaimIDList
                                      WHERE RowNumber BETWEEN @LoopCounter AND @LoopCounter + @PHIAccessLogBatchsize
                                      FOR XML PATH( '' ), TYPE ).value( '.', 'NVARCHAR(MAX)' ), 1, 2, '') Claims,
                              @DateViewd;
                --FROM @ClaimIDList
                --WHERE RowNumber BETWEEN @LoopCounter AND @LoopCounter + @PHIAccessLogBatchsize			
                SET @LoopCounter = @LoopCounter + @PHIAccessLogBatchsize + 1;
            END;
    END;