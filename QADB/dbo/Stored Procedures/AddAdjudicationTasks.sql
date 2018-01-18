CREATE PROCEDURE dbo.AddAdjudicationTasks(@UserName       VARCHAR(100),
                                          @RequestName    VARCHAR(100),
                                          @IsUserDefined  BIT,
                                          @RunningStatus  INT,
                                          @Priority       INT,
                                          @SelectCriteria VARCHAR(MAX),
                                          @FacilityID     BIGINT,
                                          @ModelID        BIGINT,
                                          @DateType       VARCHAR(100),
                                          @DateFrom       DATETIME,
                                          @DateTo         DATETIME,
                                          @IdJob          INT,
                                          @StartTime      DATETIME,
                                          @EndTime        DATETIME,
                                          @InsertedID     BIGINT OUTPUT)
AS    

/****************************************************************************      
*   Name         : AddAdjudicationTasks    
*   Author       : mmachina      
*   Date         : 20/Sep/2013     
*   Modified By  : Prasad  
*   Module       : Adjudication  Testing
*   Description  : Adding adjudication tasks    
 *****************************************************************************/

     BEGIN
         SET NOCOUNT ON;
         DECLARE @CurrentDate DATETIME= GETUTCDATE(), @ClaimToolDesc VARCHAR(1000), @JobID VARCHAR(100), @TotalClaimCountQuery NVARCHAR(MAX), @ClaimCount INT, @ClaimCountQuery NVARCHAR(MAX)= ' ', @ParamDefinition NVARCHAR(100);
         DECLARE @TmpTable TABLE(InsertedID BIGINT);
        
	    SET @ParamDefinition = N'@CountOUT INT OUTPUT';
         SET @ClaimCountQuery = ([dbo].[GetSubQueryForClaimFilters](@DateType, @DateFrom, @DateTo, @SelectCriteria, @FacilityID,@ModelID));
         SET @ClaimCountQuery = STUFF(@ClaimCountQuery, CHARINDEX('SELECT DISTINCT CD.claimid C FROM ClaimData AS CD ', @ClaimCountQuery), LEN('SELECT DISTINCT CD.claimid C FROM ClaimData AS CD'), '');
         SET @ClaimCountQuery = 'SELECT @CountOUT = COUNT(DISTINCT CD.ClaimID) FROM ClaimData AS CD  WITH (NOLOCK)'+@ClaimCountQuery;
         EXECUTE sp_executesql
                 @ClaimCountQuery,
                 @ParamDefinition,
                 @CountOUT = @ClaimCount OUTPUT;
	 
         -- Claim Count is 0 then we should not make any entry in TrackTasks table
         IF(@ClaimCount = 0)
             BEGIN
                 SET @InsertedID = -1;
                 RETURN;
             END;

          INSERT INTO dbo.TrackTasks
         ([InsertDate],
          [UpdateDate],
          [UserName],
          [RequestName],
          [IsUserDefined],
          [RunningStatus],
          [Priority],
          [SelectCriteria],
          [FacilityID],
          [ModelID],
          [DateType],
          [DateFrom],
          [DateTo],
          [IdJob],
          [StartTime],
          [EndTime],
          [LastUpdatedDateForElapsedTime],
          [IsVerified]
         )
         OUTPUT INSERTED.TaskID
                INTO @TmpTable
         VALUES
         (@CurrentDate,
          NULL,
          @UserName,
          @RequestName,
          @IsUserDefined,
          @RunningStatus,
          @Priority,
          @SelectCriteria,
          @FacilityID,
          @ModelID,
          @DateType,
          @DateFrom,
          @DateTo,
          @IdJob,
          @StartTime,
          @EndTime,
          @CurrentDate,
          0
         );
         SELECT @InsertedID = InsertedID
         FROM @TmpTable; 
         -- Add model id into running task table
         EXEC [dbo].[AddRunningTask]
              @ModelID;  	
	
         --Audit Logging
         SELECT @JobID = InsertedID
         FROM @Tmptable;
         SELECT @ClaimToolDesc = 'Job Name: '+RequestName
         FROM TrackTasks
         WHERE TaskId = @JobID;
         EXEC InsertAuditLog
              @UserName,
              'Add',
              'Jobs',
              @ClaimToolDesc,
              @JobID,
              4;
     END;
