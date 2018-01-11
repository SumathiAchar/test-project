using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface ILogOffRepository : IDisposable
    {
        /// <summary>
        /// Inserts the audit log.
        /// </summary>
        /// <param name="logOffInfo">The log off information.</param>
        /// <returns></returns>
        void InsertAuditLog(LogOff logOffInfo);
    }
}
