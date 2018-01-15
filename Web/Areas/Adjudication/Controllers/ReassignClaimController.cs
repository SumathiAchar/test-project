using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;

namespace SSI.ContractManagement.Web.Areas.Adjudication.Controllers
{
    public class ReassignClaimController : SessionStoreController
    {
        // FIXED-DEC15 Change function name to Index and pass model only to view in-place so we can avoid ViewBag . 
        /// <summary>
        /// Indexes the specified claim search criteria.
        /// </summary>
        /// <param name="claimSearchCriteria">The claim search criteria.</param>
        /// <returns></returns>
        public ActionResult Index(ClaimSearchCriteria claimSearchCriteria)
        {
            claimSearchCriteria.FacilityId = GetCurrentFacilityId();
            // Making Session null while opening pop up
            SetReassignedClaims(null);
            if (claimSearchCriteria.StartDate == DateTime.MinValue ||
                     claimSearchCriteria.EndDate == DateTime.MinValue)
            {
                claimSearchCriteria.EndDate = DateTime.Now;
                claimSearchCriteria.StartDate =
                    DateTime.Now.AddYears(-GlobalConfigVariable.PullDataForNumberOfYears);
            }
            if (claimSearchCriteria.DateType == 0)
            {
                claimSearchCriteria.DateType = (byte)Enums.DateTypeFilter.DateofserviceandAdmissiondate;
            }
            return View(claimSearchCriteria);
        }

        /// <summary>
        /// Gets the reassign grid data.
        /// </summary>
        /// <param name="claimSearchCriteria">The claim search criteria.</param>
        /// <param name="reassignClaims">The reassign claims.</param>
        /// <param name="isSelectAllPage">if set to <c>true</c> [is select all page].</param>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="isSelectAllHeader">if set to <c>true</c> [is select all header].</param>
        /// <param name="isPrimaryModel">if set to <c>true</c> [is primary model].</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <returns></returns>
        /// FIXED-DEC15 Pass ClaimSearchCriteria as model in parameter in-place of separate properties
        /// Fixed-DEC15 use ClaimSearchCriteria in the model and change name to GetReassignGridData
        [HttpPost]
        public ActionResult GetReassignGridData(ClaimSearchCriteria claimSearchCriteria,
            List<ReassignClaim> reassignClaims, bool isSelectAllPage, long modelId,long contractId, bool isSelectAllHeader=false,bool isPrimaryModel=false)
        {
            string currentUserName = GetCurrentUserName();
            string userGuid = GetUserKey();//user key replaced

            claimSearchCriteria.FacilityId = GetCurrentFacilityId();
            claimSearchCriteria.UserName = currentUserName;
            claimSearchCriteria.RequestedUserId = string.IsNullOrEmpty(currentUserName) ? Guid.Empty.ToString() : userGuid;

            // Getting the current page data
            ReassignClaimContainer reassignClaim = PostApiResponse<ReassignClaimContainer>(Constants.ReassignClaim,
                     Constants.GetReassignGridData, claimSearchCriteria);
            // Getting session data
            List<RetainedClaim> retainedClaims = GetReassignedClaims();
            UpdateRetainClaims(retainedClaims, isSelectAllPage, modelId, isSelectAllHeader, isPrimaryModel, contractId);
            // Setting the 50 retained claims data 
            List<ReassignClaim> pagedReassignClaims=new List<ReassignClaim>();
            if (retainedClaims.Any(a => a.Order == claimSearchCriteria.PageIndex))
            {
                pagedReassignClaims = retainedClaims.Find(a => a.Order == claimSearchCriteria.PageIndex).ReassignClaims;
            }
            else if (reassignClaim.ClaimData!=null)
            {
                pagedReassignClaims = reassignClaim.ClaimData.Select(claim => new ReassignClaim
                {
                    ClaimId = claim.ClaimId,
                    IsRetained = claim.IsRetained,
                    ContractId = (isPrimaryModel && claimSearchCriteria.ContractId != 0) ? claimSearchCriteria.ContractId : (claimSearchCriteria.ContractId!=0)?claimSearchCriteria.ContractId:claim.ContractId,
                    ModelId =
                        claimSearchCriteria.ModelId == 0 || claim.IsRetained
                            ? claim.ModelId
                            : claimSearchCriteria.ModelId,
                    IsSelected = (claimSearchCriteria.IsSelectAll && claim.IsRetained && !isSelectAllHeader)
                        ? (isPrimaryModel && claim.ContractId!=contractId) || Convert.ToBoolean(claim.IsSelected)
                        : claimSearchCriteria.IsSelectAll
                }).ToList();
            }

            // Saving the data into session
            if (retainedClaims.Any(a => a.Order == claimSearchCriteria.PageIndex))
            {
                List<ReassignClaim> claims = retainedClaims.Find(a => a.Order == claimSearchCriteria.PageIndex).ReassignClaims;
                // Selected claims retained claims data
                foreach (ReassignClaim reassign in pagedReassignClaims)
                {
                    // Getting the claim data from reassign claim
                    ReassignClaim claimData = claims.Where(claim => claim.ClaimId == reassign.ClaimId).DefaultIfEmpty().FirstOrDefault();
                    if (claimData != null)
                    {
                        // Setting the properties
                        reassign.IsSelected = (claimSearchCriteria.IsSelectAll && claimData.IsSelected) ? (claimSearchCriteria.IsSelectAll && claimData.IsSelected) : claimData.IsSelected;
                        reassign.ModelId = claimData.ModelId;
                           reassign.ContractId = claimSearchCriteria.ContractId == 0 || claimData.IsRetained
                            ? claimData.ContractId
                            : claimSearchCriteria.ContractId;
                        reassign.IsRetained = claimData.IsRetained;
                        reassign.ContractId = (isPrimaryModel && claimSearchCriteria.ContractId != 0)
                            ? claimSearchCriteria.ContractId
                            : reassign.ContractId;
                    }
                }
            }
            // Setting session data for the previous page
            if (retainedClaims.Any(q => q.Order == claimSearchCriteria.LastSelectedPageIndex))
            {
                retainedClaims.Find(a => a.Order == claimSearchCriteria.LastSelectedPageIndex).ReassignClaims =
                    reassignClaims;
            }
            // If the data is not available in the session
            else if (reassignClaims != null && reassignClaims.Any())
            {
                retainedClaims.Add(new RetainedClaim
                {
                    Order = claimSearchCriteria.LastSelectedPageIndex,
                    ReassignClaims = reassignClaims
                });
            }
            // Setting session
            SetReassignedClaims(retainedClaims);
            return
                Json(
                    new
                    {
                        data = reassignClaim.ClaimData,
                        retainedClaims = pagedReassignClaims,
                        total = reassignClaim.TotalRecords,
                        lastSelectedPageIndex = claimSearchCriteria.PageIndex
                    });
        }

        /// <summary>
        /// Gets the contracts by node identifier.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetContractsByNodeId(long nodeId)
        {
            ContractHierarchy contractHierarchy = new ContractHierarchy { NodeId = nodeId, UserName = GetLoggedInUserName() };
            List<Shared.Models.Contract> contractlist =
                PostApiResponse<List<Shared.Models.Contract>>(Constants.ReassignClaim, Constants.GetContractsByNodeId,
                    contractHierarchy);
            return Json(new { data = contractlist });
        }

        //FIXED-DEC15 Change method name to AddReassignedClaimJob. 
        /// <summary>
        /// Adds the reassigned claim job.
        /// </summary>
        /// <param name="reassignedClaimJob">The reassigned claim job.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddReassignedClaimJob(ReassignedClaimJob reassignedClaimJob)
        {
            List<RetainedClaim> retainedClaims = GetReassignedClaims();
            if (reassignedClaimJob.ModelId != null && reassignedClaimJob.ContractId != null)
               UpdateRetainClaims(retainedClaims, reassignedClaimJob.IsSelectAllPage, reassignedClaimJob.ModelId.Value, reassignedClaimJob.IsSelectAllHeader, reassignedClaimJob.IsPrimaryModel, reassignedClaimJob.ContractId.Value);
            if (retainedClaims.Any(a => a.Order == reassignedClaimJob.PageIndex))
            {
                retainedClaims.Find(a => a.Order == reassignedClaimJob.PageIndex).ReassignClaims =
                    reassignedClaimJob.ReassignClaim;
            }
            else
            {
                retainedClaims.Add(new RetainedClaim
                {
                    Order = reassignedClaimJob.PageIndex,
                    ReassignClaims = reassignedClaimJob.ReassignClaim
                });
            }
            //UpdateReassignClaim(reassignedClaimJob);
            if(retainedClaims.Any(claim=>claim.ReassignClaims!=null))
            {
                reassignedClaimJob.ReassignClaim = retainedClaims.SelectMany(q => q.ReassignClaims).ToList();
            }
            reassignedClaimJob.UserName = GetLoggedInUserName();
            reassignedClaimJob.FacilityId = GetCurrentFacilityId();
           string errorMessage = ValidateReassignClaim(reassignedClaimJob);
            if (string.IsNullOrEmpty(errorMessage))
            {
                bool result = PostApiResponse<bool>(Constants.ReassignClaim, Constants.AddReassignedClaimJob,
                    reassignedClaimJob);
                // Making Session null while closing pop up
                SetReassignedClaims(null);
                return Json(new { data = result });
            }
            return Json(new { data = false, errorMessage });
        }

        //FIXED-DEC15 Change method name to GetClaimLinkedCount and use contractHierachy model 
        /// <summary>
        /// Gets the claim linked count.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetClaimLinkedCount(long nodeId)
        {
            ContractHierarchy contractHierarchy = new ContractHierarchy { NodeId = nodeId, UserName = GetLoggedInUserName() };
            int claimCount = PostApiResponse<int>(Constants.ReassignClaim, Constants.GetClaimLinkedCount, contractHierarchy);
            return Json(new { claimCount });
        }

        private static void UpdateRetainClaims(List<RetainedClaim> retainedClaims, bool isSelectAllPage, long modelId, bool isSelectAllHeader,bool isPrimaryModel,long contractId)
        {
            if (retainedClaims != null && retainedClaims.Count>0 && retainedClaims[0].ReassignClaims!=null && isSelectAllPage)
            {
                foreach (RetainedClaim retainedClaim in retainedClaims)
                {
                    foreach (ReassignClaim claim in retainedClaim.ReassignClaims)
                    {
                        if (!claim.IsRetained)
                        {
                            claim.IsSelected = true;
                            claim.ModelId = modelId;
                            if(contractId!=0)
                            claim.ContractId = contractId;
                        }
                        if (isSelectAllHeader)
                        {
                            claim.IsSelected = true;
                        }
                        if (isPrimaryModel && contractId != 0 && (contractId != claim.ContractId))
                        {
                            claim.IsSelected = true;
                            claim.ContractId = contractId;
                            claim.ModelId = modelId;
                            
                        }
                    }

                }
            }

        }

        private string ValidateReassignClaim(ReassignedClaimJob reassignedClaimJob)
        {
            if (reassignedClaimJob.ReassignClaim != null)
            {
                if (reassignedClaimJob.IsSelectAll != null && (!reassignedClaimJob.ReassignClaim.Any((p => p.IsSelected)) && (bool) !reassignedClaimJob.IsSelectAll) )
                    return Constants.ClaimSelectErrorMsg;
                if (
                    reassignedClaimJob.ReassignClaim.Any(
                        p => p.IsRetained && (p.ContractId == 0 || p.ContractId == null)))
                    return Constants.ContractSelectErrorMsg;
            }
            else
            {
                return Constants.ReassignClaimEmptyErrorMsg;
            }
            return string.Empty;
        }
    }
}
