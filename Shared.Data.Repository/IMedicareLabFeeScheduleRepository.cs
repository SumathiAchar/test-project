using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IMedicareLabFeeScheduleRepository : IDisposable
    {
        /// <summary>
        /// Gets the name of the m care lab fee schedule table.
        /// </summary>
        /// <returns></returns>
        List<MedicareLabFeeSchedule> GetMedicareLabFeeScheduleTableNames(MedicareLabFeeSchedule mCareFeeSchedule);

        /// <summary>
        /// Gets the m care lab fee schedule table data.
        /// </summary>
        /// <param name="mCareLabFeeSchedule">The m care lab fee schedule table.</param>
        /// <returns></returns>
        MedicareLabFeeScheduleResult GetMedicareLabFeeSchedule(MedicareLabFeeSchedule mCareLabFeeSchedule);
    }
}
