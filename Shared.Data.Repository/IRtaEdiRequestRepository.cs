using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    /// <summary>
    /// Interface for Rta Edi Request Repository 
    /// </summary>
    public interface IRtaEdiRequestRepository : IDisposable
    {
        /// <summary>
        /// Saves the specified rta edi request.
        /// </summary>
        /// <param name="rtaEdiRequest">The rta edi request.</param>
        /// <returns></returns>
        long Save(RtaEdiRequest rtaEdiRequest);
    }
}
