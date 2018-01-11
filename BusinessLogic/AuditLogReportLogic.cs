using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class AuditLogReportLogic
    {
        /// <summary>
        /// The _claim adjudication report repository
        /// </summary>
        private readonly IAuditLogReportRepository _auditLogReportRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="AuditLogReportLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public AuditLogReportLogic(string connectionString)
        {
            _auditLogReportRepository = Factory.CreateInstance<IAuditLogReportRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditLogReportLogic"/> class.
        /// </summary>
        /// <param name="auditLogReportRepository">The audit log report repository.</param>
        public AuditLogReportLogic(IAuditLogReportRepository auditLogReportRepository)
        {
            if (auditLogReportRepository != null)
            {
                _auditLogReportRepository = auditLogReportRepository;
            }
        }

        /// <summary>
        /// Gets the audit log report.
        /// </summary>
        /// <param name="auditLogReport">The audit log report.</param>
        /// <returns></returns>
        public AuditLogReport GetAuditLogReport(AuditLogReport auditLogReport)
        {
            return _auditLogReportRepository.GetAuditLogReport(auditLogReport);
        }
    }
}
