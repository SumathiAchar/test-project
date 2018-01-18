/****************************************************************************
 *   Name         : GetAdjudicatedTasks
 *   Author       : Sumathi
 *   Date         : 26/Feb/2016
 *   Module       : Background Adjudication
 *   Description  : Gets facility details of Adjudicated Tasks
 *****************************************************************************/

CREATE PROCEDURE [dbo].[GetAdjudicatedTasks](
       @FacilityIds VARCHAR(MAX))
AS
    BEGIN
        DECLARE @TblFacilityIds TABLE( FacilityId VARCHAR(MAX));
        INSERT INTO @TblFacilityIds
               SELECT *
               FROM dbo.Split( @FacilityIds, ',' );
        SELECT FacilityId,
               InsertDate,
               RunningStatus
        FROM( 
              SELECT TT.FacilityId AS FacilityId,
                     InsertDate AS InsertDate,
                     RunningStatus AS RunningStatus,
                     ROW_NUMBER() OVER(PARTITION BY TT.FacilityId ORDER BY TT.InsertDate DESC) RowNumber
              FROM TrackTasks TT WITH ( NOLOCK )
                   INNER JOIN @TblFacilityIds Temp ON Temp.FacilityId = TT.FacilityId
              WHERE IsUserDefined = 0
                AND RunningStatus IN( 100, 129, 133 )) AS Ordered                 --Running Status: 100 = Completed, 129 = Running, 133 = Failed 
        WHERE Ordered.RowNumber = 1;
    END;