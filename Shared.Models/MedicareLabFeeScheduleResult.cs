using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class MedicareLabFeeScheduleResult
    {
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the medicare lab fee schedule list.
        /// </summary>
        /// <value>
        /// The medicare lab fee schedule list.
        /// </value>
        public List<MedicareLabFeeSchedule> MedicareLabFeeScheduleList { get; set; }
    }
}
