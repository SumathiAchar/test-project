using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IModelingReportRepository : IDisposable
    {
        /// <summary>
        /// Gets all modeling details.
        /// </summary>
        /// <param name="modelingReport">The modeling report.</param>
        /// <returns></returns>
        ModelingReport GetAllModelingDetails(ModelingReport modelingReport);
    }
}
