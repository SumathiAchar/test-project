using Kendo.DynamicLinq;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;

namespace SSI.ContractManagement.Web.Areas.Alert.Controllers
{
    public class ContractAlertController : CommonController
    {
        /// <summary>
        /// Contracts the alerts.
        /// </summary>
        /// <returns></returns>
        public ActionResult ContractAlert()
        {
            return View();
        }

        /// <summary>
        /// Get ContractAlerts Information of User
        /// </summary>
        /// <returns>ContractAlert</returns>
        [HttpPost]
        public ActionResult ContractAlertsInfo(int take, int skip, IEnumerable<Sort> sort, Kendo.DynamicLinq.Filter filter)
        {
            ContractAlert data = new ContractAlert
            {
                NumberOfDaysToDismissAlerts = GlobalConfigVariables.NumberOfDaysToDismissAlerts,
                FacilityId = GetCurrentFacilityId()
            };
           
            List<ContractAlert> contractAlertList = PostApiResponse<List<ContractAlert>>(Constants.ContractAlert, Constants.GetContractAlerts, data);
            return Json(contractAlertList.AsQueryable().ToDataSourceResult(take, skip, sort, filter));
        }

        /// <summary>
        /// Updating ContractAlerts Information of User
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public JsonResult UpdateContractAlertsInformation(long contractId)
        {
            ContractAlert contractAlert = new ContractAlert { ContractId = contractId };
            long result = PostApiResponse<long>(Constants.ContractAlert, Constants.UpdateContractAlerts, contractAlert);
            return Json(result > 0 ? new { sucess = true } : new { sucess = false });
        }

        /// <summary>
        /// Contracts the alert count.
        /// </summary>
        /// <returns>JsonResult.</returns>
        public int ContractAlertCount(int currentFacilityId)
        {
            ContractAlert alertCount = new ContractAlert
            {
                NumberOfDaysToDismissAlerts = GlobalConfigVariables.NumberOfDaysToDismissAlerts,
                FacilityId = currentFacilityId
            };

            int totalAlertCount = PostApiResponse<int>(Constants.ContractAlert, Constants.ContractAlertCount, alertCount);
            return totalAlertCount;
        }

        /// <summary>
        /// Updates the alerts count.
        /// </summary>
        /// <param name="alertUpdateId">The alert update identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateAlertsCount(long alertUpdateId)
        {
            ContractAlert alert = new ContractAlert { ContractAlertId = alertUpdateId };
            bool alertstatus = PostApiResponse<bool>(Constants.ContractAlert, Constants.UpdateAlertVerifiedStatus, alert);
            return Json(new { data = alertstatus });
        }
    }
}
