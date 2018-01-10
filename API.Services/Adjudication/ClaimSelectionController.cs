using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Adjudication
{
    // ReSharper disable once UnusedMember.Global
    public class ClaimSelectionController : BaseController
    {
        private readonly ClaimSelectorLogic _claimSelectorLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimSelectionController"/> class.
        /// </summary>
        public ClaimSelectionController()
        {

            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId);
            _claimSelectorLogic = new ClaimSelectorLogic(bubbleDataSource);
        }

        /// <summary>
        /// Add Select Claims
        /// </summary>
        /// <param name="selectClaims"></param>
        /// <returns>Getting TaskId</returns>
        [HttpPost]
        public long AddEditSelectClaims(ClaimSelector selectClaims)
        {
            return _claimSelectorLogic.AddEditSelectClaims(selectClaims);
        }

        /// <summary>
        /// Gets the selected claims count
        /// </summary>
        /// <param name="selectClaim"></param>
        /// <returns></returns>
        [HttpPost]
        public long GetSelectedClaimList(ClaimSelector selectClaim)
        {
            return _claimSelectorLogic.GetSelectedClaimList(selectClaim);
        }

        /// <summary>
        /// Check request Name
        /// </summary>
        /// <param name="selectClaims"></param>
        [HttpPost]
        public bool CheckAdjudicationRequestNameExist(ClaimSelector selectClaims)
        {
            return _claimSelectorLogic.CheckAdjudicationRequestNameExist(selectClaims);
        }

        /// <summary>
        /// Reviews the claim.
        /// </summary>
        /// <param name="claimsReviewed">The claims reviewed.</param>
        /// <returns></returns>
        [HttpPost]
        public bool ReviewClaim(IEnumerable<ClaimsReviewed> claimsReviewed)
        {
            return _claimSelectorLogic.ReviewClaim(claimsReviewed);
        }

        /// <summary>
        /// Reviewed all claims.
        /// </summary>
        /// <param name="selectionCriteria">The selection criteria.</param>
        /// <returns></returns>
        [HttpPost]
        public bool ReviewedAllClaims(SelectionCriteria selectionCriteria)
        {
            return _claimSelectorLogic.ReviewedAllClaims(selectionCriteria);
        }

        /// <summary>
        /// Adds the claim note.
        /// </summary>
        /// <param name="claimNote">The claim note.</param>
        /// <returns></returns>
        [HttpPost]
        public ClaimNote AddClaimNote(ClaimNote claimNote)
        {
            return _claimSelectorLogic.AddClaimNote(claimNote);
        }

        /// <summary>
        /// Deletes the claim note.
        /// </summary>
        /// <param name="claimNote">The claim note.</param>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteClaimNote(ClaimNote claimNote)
        {
            return _claimSelectorLogic.DeleteClaimNote(claimNote);
        }

        /// <summary>
        /// Gets the claim notes.
        /// </summary>
        /// <param name="claimNotesContainer">The claim notes container.</param>
        /// <returns></returns>
        [HttpPost]
        public ClaimNotesContainer GetClaimNotes(ClaimNotesContainer claimNotesContainer)
        {
            return _claimSelectorLogic.GetClaimNotes(claimNotesContainer);
        }
    }
}
