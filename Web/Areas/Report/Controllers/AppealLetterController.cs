using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.WebUtil;

namespace SSI.ContractManagement.Web.Areas.Report.Controllers
{
    public class AppealLetterController : CommonController
    {
        /// <summary>
        /// Generates the appeal letter.
        /// </summary>
        /// <param name="leterTemplateId">The leter template identifier.</param>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public ActionResult GenerateAppealLetter(long leterTemplateId, long modelId, int? dateType, DateTime? startDate, DateTime? endDate, string criteria)
        {
            AppealLetter appealLetter = new AppealLetter
                {
                    LetterTemplateId = leterTemplateId,
                    NodeId = modelId,
                    DateType = Convert.ToInt32(dateType),
                    StartDate = Convert.ToDateTime(startDate),
                    EndDate = Convert.ToDateTime(endDate),
                    ClaimSearchCriteria = criteria,
                    //Added RequestedUserID and RequestedUserName with reference to HIPAA logging feature
                    RequestedUserId =
                        string.IsNullOrEmpty(GetLoggedInUserName()) ? Guid.Empty.ToString() : GetUserKey(),
                    RequestedUserName = GetLoggedInUserName(),
                    MaxNoOfRecords = GlobalConfigVariable.MaxNoOfDataForLetters
                };
            appealLetter = PostApiResponse<AppealLetter>(Constants.AppealLetter,
                Constants.GetAppealLetter, appealLetter);
            string[] fileName = LetterReportUtil.GetExportedFileName(appealLetter,
                GlobalConfigVariable.ReportsFilePath, Constants.AppealLetterFileBaseName);
            return Json(fileName);
        }

        /// <summary>
        /// Gets the appeal templates.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAppealTemplates()
        {
            LetterTemplate appealLetterInfo = new LetterTemplate
            {
                FacilityId = GetCurrentFacilityId()
            };
            List<LetterTemplate> letterTemplates = PostApiResponse<List<LetterTemplate>>(Constants.AppealLetter, Constants.GetAppealTemplates, appealLetterInfo);
            return Json(new { letterTemplates }, JsonRequestBehavior.AllowGet);
        }
    }
}
