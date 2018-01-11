using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IAuditLogReportRepository : IDisposable
    {
        /// <summary>
        /// Gets the audit log report.
        /// </summary>
        /// <param name="auditLogReport">The audit log report.</param>
        /// <returns></returns>
        AuditLogReport GetAuditLogReport(AuditLogReport auditLogReport);
    }
}
