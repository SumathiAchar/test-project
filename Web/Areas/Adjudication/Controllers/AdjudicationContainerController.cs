using System;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Adjudication.Models;

namespace SSI.ContractManagement.Web.Areas.Adjudication.Controllers
{
    public class AdjudicationContainerController : Controller
    {
        //
        // GET: /AdjudicationContainer/

        public ActionResult Index()
        {
            ClaimSelector claimSelectorInfo = new ClaimSelector
            {
                ModuleId = Convert.ToByte(EnumHelperLibrary.GetFieldInfoFromEnum(Enums.Modules.Adjudication).FieldIdentityNumber)
            };
            SelectClaimsViewModel selectAdjudicationClaimViewModel = AutoMapper.Mapper.Map<ClaimSelector, SelectClaimsViewModel>(claimSelectorInfo);
            return View(selectAdjudicationClaimViewModel);
        }
    }
}
