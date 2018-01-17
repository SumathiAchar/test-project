using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;

namespace SSI.ContractManagement.Web.Areas.Report.Controllers
{
    public class ReportSelectionController : CommonController
    {
        public ActionResult ReportSelection()
        {
            return View();
        }

        /// <summary>
        /// Get the all Claim Fields and Operator Values
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public JsonResult GetClaimFieldsandOperatorValues(int moduleId)
        {
            List<SelectListItem> operatorValues = new List<SelectListItem>();
            List<SelectListItem> contractServiceClaims = new List<SelectListItem>();

            List<ClaimFieldOperator> claimFieldOperators = GetApiResponse<List<ClaimFieldOperator>>(Constants.ReportSelection, Constants.GetAllClaimFieldsOperators);

            if (claimFieldOperators != null && claimFieldOperators.Count > 0)
            {
                operatorValues.AddRange(claimFieldOperators.Select(item => new SelectListItem { Text = item.OperatorType, Value = item.OperatorId.ToString(CultureInfo.InvariantCulture) }));
            }

            ClaimSelector reportClaimSelectorInfo = new ClaimSelector
            {
                ModuleId = moduleId
            };

            // Get Claim field from DB 
            List<ClaimField> claimFields = PostApiResponse<List<ClaimField>>(Constants.ReportSelection, Constants.GetAllClaimFields, reportClaimSelectorInfo);

            if (claimFields != null && claimFields.Count > 0)
            {
                contractServiceClaims.AddRange(claimFields.Select(item => new SelectListItem { Text = item.Text, Value = item.ClaimFieldId.ToString(CultureInfo.InvariantCulture) }));
            }

            List<EnumHelper> reportNames = EnumHelperLibrary.GetFieldInfoFromEnum<Enums.ReportTypeFilter>();
            List<EnumHelper> reportLevels = EnumHelperLibrary.GetFieldInfoFromEnum<Enums.ReportLevelFilter>();
            List<EnumHelper> dateTypes = EnumHelperLibrary.GetFieldInfoFromEnum<Enums.DateTypeFilter>();

            return Json(
                        new
                        {
                            claimFeildOperatorList = operatorValues,
                            claimFields = contractServiceClaims,
                            reportNamesAvailable = reportNames,
                            reportLevelsAvailable = reportLevels,
                            dateTypesAvailable = dateTypes
                        }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets adjudication request names based on the model id and use rename.
        /// </summary>
        /// <returns> List of ClaimSelector</returns>
        [HttpPost]
        public JsonResult GetAdjudicationRequestNames(long? modelId)
        {
            ClaimSelector claimSelector = new ClaimSelector
            {
                ModelId = modelId,
                CompletedJobsDuration = GlobalConfigVariables.NumberOfDaysToDismissCompletedJobs
            };

            List<ClaimSelector> adjudicationRequestNames = PostApiResponse<List<ClaimSelector>>(Constants.ReportSelection, Constants.GetAdjudicationRequestNames, claimSelector);
            return Json(new { adjudicationRequestNames });

        }

        /// <summary>
        /// Add or edit query name.
        /// </summary>
        /// <param name="claimSelector"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditQueryName(ClaimSelector claimSelector)
        {
            claimSelector.UserName = GetCurrentUserName();
            claimSelector.FacilityName = GetCurrentFacilityName();
            int savedQuery = PostApiResponse<int>(Constants.ReportSelection, Constants.AddEditQueryName, claimSelector);
            return Json(savedQuery);
        }

        /// <summary>
        /// Delete query name with criteria.
        /// </summary>
        /// <param name="claimSelector"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteQueryName(ClaimSelector claimSelector)
        {
            claimSelector.UserName = GetCurrentUserName();
            claimSelector.FacilityName = GetCurrentFacilityName();
            bool isSuccess = PostApiResponse<bool>(Constants.ReportSelection, Constants.DeleteQueryName, claimSelector);
            return Json(isSuccess);
        }

        /// <summary>
        /// Get and set queryname.
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryName()
        {
            return View();
        }
        /// <summary>
        /// Get Quries By Id
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetQueriesById(UserViewModel userInfo)
        {
            User user = Mapper.Map<UserViewModel, User>(userInfo);
            List<ClaimSelector> claimSelector = PostApiResponse<List<ClaimSelector>>(Constants.ReportSelection, Constants.GetQueriesById, user);
            return Json(claimSelector, JsonRequestBehavior.AllowGet);
        }

    }
}
