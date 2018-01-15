using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Adjudication.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Adjudication.Controllers
{
    public class ClaimSelectionController : CommonController
    {

        public ActionResult SelectClaims()
        {
            ViewBag.UserName = GetCurrentUserName();
            return View();
        }

        /// <summary>
        /// Adds the payment type asc fee.
        /// </summary>
        /// <param name="selectClaims">The asc fee schedule info.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult AddSelectClaims(SelectClaimsViewModel selectClaims)
        {
                selectClaims.FacilityId = GetCurrentFacilityId();
                selectClaims.IsUserDefined = true;
                ClaimSelector selectClaimsList =
                    AutoMapper.Mapper.Map<SelectClaimsViewModel, ClaimSelector>(selectClaims);
                //Get the UserName logged in
                selectClaimsList.UserName = GetCurrentUserName();
                
                selectClaimsList.CommandTimeoutForSelectClaimIdsforAdjudicate = Convert.ToInt32(GlobalConfigVariable.CommandTimeoutForClaimsSelection);

                bool isRequestnameExist = PostApiResponse<bool>(Constants.ClaimSelection, Constants.CheckAdjudicationRequestNameExist, selectClaimsList);
                //Get Request Unique name exist or not 
                if (isRequestnameExist)
                {
                    return Json(new { success = true });
                }

                long claimsId = PostApiResponse<long>(Constants.ClaimSelection, Constants.AddEditSelectClaims, selectClaimsList);
                return claimsId > 0 ? Json(new { sucess = true, Id = claimsId }) : Json(new { sucess = false, Id = claimsId });
         }

        /// <summary>
        /// Adds the payment type asc fee.
        /// </summary>
        /// <param name="selectClaims">The asc fee schedule info.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult GetSelectClaims(SelectClaimsViewModel selectClaims)
        {
               selectClaims.FacilityId = GetCurrentFacilityId();
                ClaimSelector selectClaimsList =
                    AutoMapper.Mapper.Map<SelectClaimsViewModel, ClaimSelector>(selectClaims);
                //Get the UserName logged in
                selectClaimsList.UserName = GetCurrentUserName();
                selectClaimsList.FacilityId = GetCurrentFacilityId();
                selectClaimsList.SsiNumber = GetSsiNumberBasedOnFacilityId(selectClaims.FacilityId);
                selectClaimsList.ModelId = selectClaims.ModelId;
                long claimscount = PostApiResponse<long>(Constants.ClaimSelection, Constants.GetSelectedClaimList, selectClaimsList);
                return claimscount > 0 ? Json(new { sucess = true, Id = claimscount }) : Json(new { sucess = true, Id = 0 });
        }
        
        /// <summary>
        /// Gets Adjudication  request unique names.
        /// </summary>
        /// <returns> isRequestnameExist </returns>
        [HttpPost]
        public JsonResult CheckAdjudicationRequestNames(int facilityId, string requestName)//TODO Janaki
        {
            ClaimSelector claimSelector = new ClaimSelector();
            {
                claimSelector.FacilityId = facilityId;
                claimSelector.UserName = GetCurrentUserName();
                claimSelector.RequestName = requestName;
                claimSelector.CommandTimeoutForCheckAdjudicationRequestNameExist = Convert.ToInt32(GlobalConfigVariable.CommandTimeoutForCheckAdjudicationRequestNameExist);
            }

            bool isRequestnameExist = PostApiResponse<bool>(Constants.ClaimSelection, Constants.CheckAdjudicationRequestNameExist, claimSelector);
            return Json(new { success = isRequestnameExist });
        }

        /// <summary>
        /// Opens the claim grid view.
        /// </summary>
        /// <param name="selectionCriteria">The selection criteria.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <returns></returns>
        public ActionResult OpenClaimGridView(string selectionCriteria, DateTime? dateFrom, DateTime? dateTo, long? modelId, int? dateType)
        {
            //Handle dates date type null

            selectionCriteria = Utilities.GetSelectionCreteria(selectionCriteria);
            if (dateFrom == DateTime.MinValue ||
                     dateTo == DateTime.MinValue || dateFrom == null ||
                                                                  dateTo == null)
            {
                dateTo = DateTime.Now;
                dateFrom =
                    DateTime.Now.AddYears(-GlobalConfigVariable.PullDataForNumberOfYears);
            }

            if (dateType == 0 || dateType == null)
            {
                dateType = (byte)Enums.DateTypeFilter.DateofserviceandAdmissiondate;
            }

            ViewBag.SelectionCriteria = selectionCriteria;
            ViewBag.dateFrom = Convert.ToDateTime(dateFrom);
            ViewBag.dateTo = Convert.ToDateTime(dateTo);
            ViewBag.modelId = modelId;
            ViewBag.dateType = dateType;
            ViewBag.isSelectClaims = true;
            return View();
        }

        /// <summary>
        /// Reviews the claim.
        /// </summary>
        /// <param name="claimsReviewed">The claims reviewed.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReviewClaim(List<ClaimsReviewed> claimsReviewed)
        {
            claimsReviewed[0].UserName = GetCurrentUserName();
            return
                Json(
                    new
                    {
                        // FIXED-NOV15 Use constant for Action and method name
                        success =
                            PostApiResponse<long>(Constants.ClaimSelection, Constants.ReviewClaim,
                              claimsReviewed)
                    });
        }

        /// <summary>
        /// Reviews all claims.
        /// </summary>
        /// <param name="selectionCriteria">The selection criteria.</param>
        /// <returns></returns>
        [HttpPost]
        //FIXED-NOV15 Create separate model "SelectionCriteria" for all properties
        public JsonResult ReviewAllClaims(SelectionCriteria selectionCriteria)
        {
            if (selectionCriteria.StartDate == DateTime.MinValue ||
                selectionCriteria.EndDate == DateTime.MinValue || selectionCriteria.StartDate == null ||
                selectionCriteria.EndDate == null)
            {
                selectionCriteria.EndDate = DateTime.Now;
                selectionCriteria.StartDate =
                    DateTime.Now.AddYears(-GlobalConfigVariable.PullDataForNumberOfYears);
            }
            SelectionCriteria criteria = new SelectionCriteria
            {
                ClaimSearchCriteria = selectionCriteria.ClaimSearchCriteria,
                UserName = GetCurrentUserName(),
                StartDate = Convert.ToDateTime(selectionCriteria.StartDate),
                EndDate = Convert.ToDateTime(selectionCriteria.EndDate),
                ModelId = Convert.ToInt64(selectionCriteria.ModelId),
                DateType = Convert.ToInt32(selectionCriteria.DateType),
            };
            return
                Json(
                    new
                    {
                        success =
                            PostApiResponse<bool>(Constants.ClaimSelection, Constants.ReviewedAllClaims,
                                criteria)
                    });
        }

        /// <summary>
        /// Opens the claim grid view.
        /// </summary>
        /// <param name="claimId">The claim identifier.</param>
        /// <param name="currentDateTime">The current date time.</param>
        /// <returns></returns>
        public ActionResult ClaimNote(long claimId, string currentDateTime)
        {
            ClaimNotesContainer claimNote = new ClaimNotesContainer { ClaimId = claimId, CurrentDateTime = currentDateTime,UserName = GetCurrentUserName(), FacilityName = GetCurrentFacilityName() };
            ClaimNotesContainer responseClaimNote = PostApiResponse<ClaimNotesContainer>(Constants.ClaimSelection, Constants.GetClaimNotes, claimNote);
            if (responseClaimNote != null && responseClaimNote.ClaimNotes.Count > 0)
            {
                responseClaimNote.ClaimId = claimId;
                return View(responseClaimNote);
            }
            return View(new ClaimNotesContainer
                {ClaimId = claimId,ClaimNotes = new List<ClaimNote>()}
            );
        }

        /// <summary>
        /// Saves the claim notes.
        /// </summary>
        /// <param name="claimNote">The claim note.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveClaimNote(ClaimNote claimNote)
        {
           //Get the Name of User logged in
            claimNote.UserName = GetCurrentUserName();
            claimNote.FacilityName = GetCurrentFacilityName();
            ClaimNote responseClaimNote = PostApiResponse<ClaimNote>(Constants.ClaimSelection, Constants.AddClaimNote, claimNote);
            if (responseClaimNote.ClaimNoteId > 0)
            {
                return
                    Json(
                        new
                        {
                            sucess = true,
                            Id = responseClaimNote.ClaimNoteId,
                            userName = GetCurrentUserName(),
                            insertedTime = Convert.ToDateTime(responseClaimNote.InsertDate).ToString(Constants.DateTimeFormatWithSecond)
                        });
            }
            return Json(new { sucess = false });
        }

        /// <summary>
        /// Deletes the claim notes.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteClaimNote(int id)
        {
            ClaimNote claimNote = new ClaimNote { ClaimNoteId = id, UserName = GetCurrentUserName(), FacilityName = GetCurrentFacilityName() };
            bool isSuccess = PostApiResponse<bool>(Constants.ClaimSelection, Constants.DeleteClaimNote, claimNote);
            return Json(new { sucess = isSuccess });
        }
    }
}