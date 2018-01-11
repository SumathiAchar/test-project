using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IModelComparisonReportRepository : IDisposable
    {
        /// <summary>
        /// Gets the available models.
        /// </summary>
        /// <param name="modelComparisonForPost">The model comparison for post.</param>
        /// <returns></returns>
        List<ModelComparisonReport> GetModels(ModelComparisonReport modelComparisonForPost);

        /// <summary>
        /// Generates the model comparasion report.
        /// </summary>
        /// <param name="reportview">The reportview.</param>
        /// <returns></returns>
        ModelComparisonReport Generate(ModelComparisonReport reportview);
    }
}
