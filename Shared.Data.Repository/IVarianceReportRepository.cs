using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IVarianceReportRepository : IDisposable
    {
        /// <summary>
        /// Gets all variance report.
        /// </summary>
        /// <param name="varianceReport">The variance report.</param>
        /// <returns></returns>
        VarianceReport GetAllVarianceReport(VarianceReport varianceReport);


        /// <summary>
        /// Gets the excel variance report.
        /// </summary>
        /// <param name="varianceReport">The variance report.</param>
        /// <param name="maxRecordLimit">The maximum record limit.</param>
        /// <returns></returns>
        VarianceReport GetExcelVarianceReport(VarianceReport varianceReport, int maxRecordLimit);
        
    }
}
