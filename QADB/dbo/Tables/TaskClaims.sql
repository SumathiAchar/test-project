CREATE TABLE [dbo].[TaskClaims] (
    [ClaimID]       BIGINT NOT NULL,
    [TaskID]        BIGINT NOT NULL,
    [IsAdjudicated] BIT    NOT NULL,
    [IsPicked]      INT    NOT NULL,
    [TaskClaimId]   BIGINT IDENTITY (1, 1) NOT NULL,
    [RowID] INT NULL,
    [AdjudicateContractID] BIGINT NULL, 
    PRIMARY KEY CLUSTERED ([TaskClaimId] ASC)    
);
GO
