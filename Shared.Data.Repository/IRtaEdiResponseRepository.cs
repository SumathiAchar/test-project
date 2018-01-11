using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    /// <summary>
    /// Interface for Rta Edi Response Repository 
    /// </summary>
    public interface IRtaEdiResponseRepository : IDisposable
    {
        /// <summary>
        /// Saves the specified rta edi response.
        /// </summary>
        /// <param name="rtaEdiResponse">The rta edi response.</param>
        /// <returns></returns>
        long Save(RtaEdiResponse rtaEdiResponse);
    }
}
