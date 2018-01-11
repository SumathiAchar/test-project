using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IMedicareIpAcuteOptionRepository : IDisposable
    {
        /// <summary>
        /// Gets the payment type medicare ip payment.
        /// </summary>
        /// <returns></returns>
        List<MedicareIpAcuteOption> GetMedicareIpAcuteOptions();
    }
}
