using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using Kendo.DynamicLinq;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;

namespace SSI.ContractManagement.Web.Areas.UserManagement.Controllers
{
    public class FacilityController : CommonController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add Edit Facility
        /// </summary>
        /// <returns></returns>
        public ActionResult AddEditFacility(int facilityId)
        {
            FacilityViewModel facilityViewModel = new FacilityViewModel();
            Facility facility = new Facility { FacilityId = facilityId };

            Facility facilityDetails = PostApiResponse<Facility>(Constants.Facility, Constants.GetFacilityById, facility, true);
            if (facilityDetails != null)
            {
                facilityViewModel = Mapper.Map<Facility, FacilityViewModel>(facilityDetails);
            }

            return View(facilityViewModel);
        }

        /// <summary>
        /// Save / Update Facility
        /// </summary>
        /// <param name="facilityViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveFacility(FacilityViewModel facilityViewModel)
        {
            facilityViewModel.RequestedUserName = GetLoggedInUserName();
            Facility facility = Mapper.Map<FacilityViewModel, Facility>(facilityViewModel);
            string message = PostApiResponse<string>(Constants.Facility, Constants.SaveFacility, facility, true);
            return Json(message);
        }

        /// <summary>
        /// Delete Facility
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteFacility(int facilityId)
        {
            Facility facility = new Facility { FacilityId = facilityId, RequestedUserName = GetLoggedInUserName() };
            bool isSuccess = PostApiResponse<bool>(Constants.Facility, Constants.DeleteFacility, facility, true);
            return Json(new { success = isSuccess });
        }

        /// <summary>
        /// Get Active State List
        /// </summary>
        /// <returns></returns>
        public JsonResult GetActiveStates()
        {
            List<string> stateList = GetApiResponse<List<string>>(Constants.Facility, Constants.GetActiveStates, true);
            return Json(new { states = stateList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Bubbles
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBubbles()
        {
            List<FacilityBubble> facilityBubbles = GetApiResponse<List<FacilityBubble>>(Constants.Facility, Constants.GetBubbles, true);
            return Json(new { bubbleList = facilityBubbles }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get All Facilities
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="isActive"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ContentResult GetAllFacilities(int take, int skip, bool isActive, int userId)
        {
            FacilityContainer facilityInfo = new FacilityContainer
            {
                PageSetting = new PageSetting { Skip = skip, Take = take },
                IsActive = isActive,
                UserId = userId
            };

            FacilityContainer facilityContainer = PostApiResponse<FacilityContainer>(Constants.Facility, Constants.GetAllFacilities, facilityInfo, true);
            DataSourceResult facilitiesData = new DataSourceResult();
            if (facilityContainer != null && facilityContainer.Facilities != null && facilityContainer.Facilities.Any())
            {
                facilitiesData.Data = Mapper.Map<List<Facility>, List<FacilityViewModel>>(facilityContainer.Facilities);
                facilitiesData.Total = facilityContainer.TotalRecords;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            return new ContentResult
            {
                Content = serializer.Serialize(facilitiesData),
                ContentType = Constants.ContentTypeApplication
            };
        }

        /// <summary>
        /// Get the all All Facilities
        /// </summary>
        [HttpPost]
        public ContentResult GetAllFacilityList(UserViewModel userInfo)
        {
            var facilityList = PostApiResponse<List<Facility>>(Constants.Facility, Constants.GetAllFacilityList, userInfo, true);
            var userInfoResult = new DataSourceResult();
            if (facilityList != null)
            {
                userInfoResult.Data = Mapper.Map<List<Facility>, List<FacilityViewModel>>(facilityList);
            }
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            return new ContentResult
            {
                Content = serializer.Serialize(userInfoResult.Data),
                ContentType = Constants.ContentTypeApplication //FIXED-RAGINI-FEB2016 - Create constant for this
            };
        }

        /// <summary>
        /// Gets the facility medicare details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetFacilityMedicareDetails()
        {
            Facility facility = new Facility
            {
                FacilityId = GetCurrentFacilityId()
            };
            facility = PostApiResponse<Facility>(Constants.Facility, Constants.GetFacilityMedicareDetails, facility);
            return Json(new { facility }, JsonRequestBehavior.AllowGet);
        }
    }
}
