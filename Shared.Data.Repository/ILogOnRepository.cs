using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface ILogOnRepository : IDisposable
    {
        /// <summary>
        /// Inserts the audit log.
        /// </summary>
        /// <param name="logOnInfo">The log in information.</param>
        /// <returns></returns>
        void InsertAuditLog(LogOn logOnInfo);
    }
}
