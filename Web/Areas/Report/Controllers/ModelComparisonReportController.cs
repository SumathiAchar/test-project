using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using SSI.ContractManagement.Web.WebUtil;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Report.Controllers
{
    public class ModelComparisonReportController : CommonController
    {
        /// <summary>
        /// Models the comparison report.
        /// </summary>
        /// <param name="dateType">Type of the date.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ModelComparisonReport(int dateType, DateTime? startDate, DateTime? endDate, string criteria)
        {
            ModelComparisonReportViewModel reportview = new ModelComparisonReportViewModel
            {
                DateType = dateType,
                FacilityId = GetCurrentFacilityId(),
                StartDate = Convert.ToDateTime(startDate),
                EndDate = Convert.ToDateTime(endDate),
                ClaimSearchCriteria = criteria,
                ModuleId = Convert.ToInt32(EnumHelperLibrary.GetFieldInfoFromEnum(Enums.Modules.ModelComparisonReport).FieldIdentityNumber),
            };
            return View(reportview);
        }

        /// <summary>
        /// Gets the available models.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetModels(int facilityId)
        {
            ModelComparisonReport modelComparisonForPost = new ModelComparisonReport
            {
                FacilityId = facilityId
            };

            List<ModelComparisonReport> availableModelsList = PostApiResponse<List<ModelComparisonReport>>(Constants.ModelComparisonReport,
                                                           Constants.GetModels,
                                                           modelComparisonForPost);
            List<ModelComparisonReportViewModel> modelsList =
                AutoMapper.Mapper.Map<List<ModelComparisonReport>, List<ModelComparisonReportViewModel>>(availableModelsList);
            return Json(new { modelsList });
        }

        /// <summary>
        /// Pushes the contract basic information data.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerateModelComparasionReport(ModelComparisonReportViewModel reportview)
        {
            reportview.RequestedUserId = GetUserKey();
            reportview.RequestedUserName = GetCurrentUserName();

            ModelComparisonReport modelComparisoneReportInfo =
                PostApiResponse<ModelComparisonReport>(Constants.ModelComparisonReport, Constants.Get,
                    reportview);
            ModelComparisonReportViewModel modelComparisonReportlist =
                    AutoMapper.Mapper.Map<ModelComparisonReport, ModelComparisonReportViewModel>(modelComparisoneReportInfo);
            modelComparisonReportlist.IsCheckedDetailLevel = reportview.IsCheckedDetailLevel;

            // Gets the current CST time.
            modelComparisonReportlist.CurrentDateTime = Utilities.GetLocalTimeString(reportview.CurrentDateTime);

            return Json(new ModelComparisonReportUtil().GetExportedFileName(modelComparisonReportlist, GlobalConfigVariable.ReportsFilePath));
        }
    }
}
