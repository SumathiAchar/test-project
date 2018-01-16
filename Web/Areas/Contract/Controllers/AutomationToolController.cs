using System.Collections.Generic;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class AutomationToolController : CommonController
    {
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Fetching ServiceLines and PaymentTypes Information from Enums 
            List<EnumHelper> serviceLineCodes = EnumHelperLibrary.GetFieldInfoFromEnum<Enums.ServiceLineCodes>();
            List<EnumHelper> paymentTypeCodes = EnumHelperLibrary.GetFieldInfoFromEnum<Enums.PaymentTypeCodes>();
            AutomationTool automationToolData = new AutomationTool
            {
                ServiceLineTypes = serviceLineCodes,
                PaymentTypes = paymentTypeCodes
            };
            return View(automationToolData);
        }
    }
}
