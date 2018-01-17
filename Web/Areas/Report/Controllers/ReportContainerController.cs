using System;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Adjudication.Models;

namespace SSI.ContractManagement.Web.Areas.Report.Controllers
{
    public class ReportContainerController : Controller
    {
        //
        // GET: /ReportContainer/

        public ActionResult Index()
        {
            ClaimSelector claimSelectorInfo = new ClaimSelector
            {
                ModuleId = Convert.ToInt32(EnumHelperLibrary.GetFieldInfoFromEnum(Enums.Modules.Reporting).FieldIdentityNumber)
            };
            SelectClaimsViewModel selectClaimViewModel = AutoMapper.Mapper.Map<ClaimSelector, SelectClaimsViewModel>(claimSelectorInfo);
            return View(selectClaimViewModel);
        }
    }
}
