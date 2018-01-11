using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    /// <summary>
    /// Interface for Rta Repository 
    /// </summary>
    public interface IRtaRepository : IDisposable
    {
        /// <summary>
        /// Gets the rta data by claim.
        /// </summary>
        /// <param name="evaluateableClaim">The evaluate able claim.</param>
        /// <returns></returns>
        RtaData GetRtaDataByClaim(EvaluateableClaim evaluateableClaim);

        /// <summary>
        /// Saves the time log.
        /// </summary>
        /// <param name="rtaEdiTimeLog">The rta edi time log.</param>
        /// <returns></returns>
        long SaveTimeLog(RtaEdiTimeLog rtaEdiTimeLog);
    }
}
