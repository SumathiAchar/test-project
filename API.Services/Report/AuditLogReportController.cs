using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Report
{
    public class AuditLogReportController : BaseController
    {
        /// <summary>
        /// The _audit log report logic
        /// </summary>
        private readonly AuditLogReportLogic _auditLogReportLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditLogReportController"/> class.
        /// </summary>
        public AuditLogReportController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _auditLogReportLogic = new AuditLogReportLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets the audit log report.
        /// </summary>
        /// <param name="auditLogReport">The audit log report.</param>
        /// <returns></returns>
        [HttpPost]
        public AuditLogReport GetAuditLogReport(AuditLogReport auditLogReport)
        {
            return _auditLogReportLogic.GetAuditLogReport(auditLogReport);
        }
    }
}
