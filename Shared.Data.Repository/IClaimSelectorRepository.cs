using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IClaimSelectorRepository : IDisposable
    {
        /// <summary>
        /// Adds the edit select claims.
        /// </summary>
        /// <param name="selectClaims">The select claims.</param>
        /// <returns></returns>
        long AddEditSelectClaims(ClaimSelector selectClaims);


        /// <summary>
        /// Selects the claim id's for adjudicate.
        /// </summary>
        /// <param name="taskId">The task unique identifier.</param>
        /// <returns></returns>
        long SelectClaimIdsToAdjudicate(long? taskId);


        /// <summary>
        /// Update the last Adjudicate Processed flag to true
        /// </summary>  
        /// <param name="taskId">TaskId</param>
        /// <returns></returns>
        void UpdateIsLastAdjudicateProcessed(long taskId);


        /// <summary>
        /// Adjudicates all facility contract.
        /// </summary>
        /// <returns></returns>
        List<ClaimSelector> AdjudicateAllFacilityContract();


        /// <summary>
        /// Gets the ssi number for background ajudication.
        /// </summary>
        /// <returns></returns>
        List<int> GetSsiNumberForBackgroundAjudication();

        /// <summary>
        /// Updates task status in DB
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        void UpdateJobStatus(TrackTask job);

        /// <summary>
        /// Get Claims Count
        /// </summary>
        /// <returns></returns>
        long GetClaimsCountForAdjudication(ClaimSelector selectClaim);

        /// <summary>
        /// Adds the edit select claims.
        /// </summary>
        /// <param name="selectClaims">The select claims.</param>
        /// <returns></returns>
        bool CheckAdjudicationRequestNameExist(ClaimSelector selectClaims);

        /// <summary>
        /// Selects the claimfor background adjudication.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <param name="batchSize">The no of records for background adjudication.</param>
        /// <param name="timeout">command timeout</param>
        /// <returns></returns>
        long GetBackgroundAdjudicationTask(long? facilityId, int batchSize, int timeout);

        /// <summary>
        /// Gets the adjudicated facilities.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TrackTask> GetAdjudicatedTasks(string facilityIds);

        /// <summary>
        /// Reviews the claim.
        /// </summary>
        /// <param name="claimsReviewedlist">The claims reviewedlist.</param>
        /// <returns></returns>
        bool ReviewClaim(IEnumerable<ClaimsReviewed> claimsReviewedlist);

        /// <summary>
        /// Reviewed all claims.
        /// </summary>
        /// <param name="selectionCriteria">The selection criteria.</param>
        /// <returns></returns>
        bool ReviewedAllClaims(SelectionCriteria selectionCriteria);

        /// <summary>
        /// Adds the claim note.
        /// </summary>
        /// <param name="claimNote">The claim note.</param>
        /// <returns></returns>
        ClaimNote AddClaimNote(ClaimNote claimNote);

        /// <summary>
        /// Deletes the claim note.
        /// </summary>
        /// <param name="claimNote">The claim note.</param>
        /// <returns></returns>
        bool DeleteClaimNote(ClaimNote claimNote);

        /// <summary>
        /// Gets the claim notes.
        /// </summary>
        /// <param name="claimNotesContainer">The claim notes container.</param>
        /// <returns></returns>
        ClaimNotesContainer GetClaimNotes(ClaimNotesContainer claimNotesContainer);
    }
}
