CREATE FUNCTION [dbo].[GetPhysicianDataFilterQuery](
                @ClaimFieldID BIGINT,
                @OperatorType VARCHAR(10),
                @Values       VARCHAR(MAX))
RETURNS VARCHAR(MAX)
AS
     BEGIN
     DECLARE @Query VARCHAR(MAX) = '';
     SELECT @Query = ' CPD.PhysicianType =  ' + CASE
                                                    WHEN @ClaimFieldID = 10
                                                    THEN+'''Referring'''
                                                    WHEN @ClaimFieldID = 11
                                                    THEN+'''Rendering'''
                                                    WHEN @ClaimFieldID = 14
                                                    THEN+'''Attending'''
                                                END + ' AND' + [dbo].[GetFilterCodeQuery]( @Values, ' REPLACE (CPD.FirstName + CPD.MiddleName + CPD.LastName, '' '', '''' )', @OperatorType, @ClaimFieldID );
     RETURN @Query;
     END;