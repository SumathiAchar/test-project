/****************************************************************************
 *   Name         : AddEditOpenClaimColumnByUserId
 *   Author       : Janakiraman
 *   Date         : 21-Apr-2016
 *   Module       : Adjudication 
 *   Description  : Add/Edit Open Claim Column Columns By UserId
 *****************************************************************************/

CREATE PROCEDURE [dbo].[AddEditOpenClaimColumnByUserId](@SelectedColumn VARCHAR(MAX),@UserId INT)
AS
     BEGIN
       DECLARE @temptable TABLE
			 ( 
			  items VARCHAR(MAX), 
			  RowID  INT IDENTITY(1, 1)
                        PRIMARY KEY
			  )
		  INSERT INTO @temptable
            SELECT Delimited.Data.value( '.', 'VARCHAR(MAX)' ) AS 'Strng'
            FROM( 
                    SELECT CAST('<Data>' + REPLACE(@SelectedColumn, ',', '</Data><Data>') + '</Data>' AS XML)) AS Data( Splited )
                CROSS APPLY Data.Splited.nodes( 'Data' ) AS Delimited( Data );

		  DELETE FROM [dbo].[ClaimAvailableColumns]
		  WHERE UserId=@UserId;
           
		  INSERT INTO [dbo].[ClaimAvailableColumns]
            (UserId,
            ClaimColumnOptionId,
		  [Order]
            )
            SELECT 
			@UserId,
			items,
			RowID
            FROM @temptable           

     END;