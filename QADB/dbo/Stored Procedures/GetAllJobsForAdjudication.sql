/****************************************************************************  
 *   Name         : GetAllJobsForAdjudication
 *   Author       : Raj  
 *   Module       : Job Status and Adjudication
 *   Description  : Gets all the jobs for Adjudication 
 *****************************************************************************/

--EXEC [dbo].[GetAllJobsForAdjudication] '100,131, 129, 130'
CREATE PROCEDURE [dbo].[GetAllJobsForAdjudication](
       @JobStatusCodesIncluded VARCHAR(100))
AS
    BEGIN
        -- READ UNCOMMITTED option is used to avoid table being blocked 
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT TaskID AS JobID,
               RequestName,
               RunningStatus AS [Status],
               FacilityID
        FROM( 
              SELECT TT.TaskID,
                     TT.RequestName,
                     TT.RunningStatus,
                     TT.ModelID,
                     TT.FacilityID AS FacilityID,
                     ROW_NUMBER() OVER(PARTITION BY TT.ModelID ORDER BY TT.InsertDate ASC) AS RowNumber
              FROM TrackTasks TT WITH ( NOLOCK )
                   LEFT JOIN dbo.RunningTask AT ON TT.ModelID = AT.ModelID
              WHERE TT.RunningStatus IN(
                       SELECT *
                       FROM dbo.Split( @JobStatusCodesIncluded, ',' ))
                AND TT.IsUserDefined = 1
                AND AT.IsRunning = 0
                AND TT.InsertDate > GETDATE() - 1 ) AS Ordered
        WHERE Ordered.RowNumber = 1
          AND Ordered.ModelID NOT IN( 
                                      SELECT ModelID
                                      FROM TrackTasks
                                      WHERE RunningStatus = 129 );
    END;
GO