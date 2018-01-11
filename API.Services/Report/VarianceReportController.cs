using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Report
{
    // ReSharper disable once UnusedMember.Global
    public class VarianceReportController : BaseController
    {
        private readonly VarianceReportLogic _varianceReportLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="VarianceReportController"/> class.
        /// </summary>
        public VarianceReportController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _varianceReportLogic = new VarianceReportLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets all variance report.
        /// </summary>
        /// <param name="varianceReport">The variance report.</param>
        /// <returns></returns>
        [HttpPost]
        public VarianceReport GetVarianceReport(VarianceReport varianceReport)
        {
            if (varianceReport != null)
            {
                return _varianceReportLogic.GetVarianceReport(varianceReport);
            }
            return null;
        }
    }
}
