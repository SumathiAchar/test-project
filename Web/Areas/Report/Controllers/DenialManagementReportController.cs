using System;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Adjudication.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;

namespace SSI.ContractManagement.Web.Areas.Report.Controllers
{
    /// <summary>
    /// Is used to access reports from CMS report module
    /// This controller can be accessed from other application with out logging into CMS. 
    /// At this point of time this controller is being called from denial management application.
    /// Authentication is required for this report and it is being taken care by SSO integarted with denial management application and CMS.
    /// </summary>
    public class DenialManagementReportController : CommonController
    {
        public ActionResult Index()
        {
            ClaimSelector claimSelectorInfo = new ClaimSelector
            {
                ModuleId = Convert.ToInt32(EnumHelperLibrary.GetFieldInfoFromEnum(Enums.Modules.Reporting).FieldIdentityNumber)
            };
            SelectClaimsViewModel denialSelectClaimsViewModel = AutoMapper.Mapper.Map<ClaimSelector, SelectClaimsViewModel>(claimSelectorInfo);
            return View("~/Areas/Report/Views/DenialManagementReport/Index.cshtml", denialSelectClaimsViewModel);
        }
    }
}
