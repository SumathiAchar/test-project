using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class PaymentTypeStopLossViewModel : PaymentTypeBaseViewModel
    {
        /// <summary>
        /// Gets or sets the StopLoss PaymentType Percentage..
        /// </summary>
        public double? Percentage { set; get; }
        /// <summary>
        /// Gets or sets the StopLoss Threshold..
        /// </summary>
        public double? Threshold { get; set; }
        /// <summary>
        /// Gets or sets the stop loss conditions.
        /// </summary>
        /// <value>
        /// The stop loss conditions.
        /// </value>
        public List<StopLossCondition> StopLossConditions { get; set; }
        /// <summary>
        /// Gets or sets the days.
        /// </summary>
        /// <value>
        /// The days.
        /// </value>
        public string Days { get; set; }
        /// <summary>
        /// Gets or sets the rev code.
        /// </summary>
        /// <value>
        /// The rev code.
        /// </value>
        public string RevCode { get; set; }
        /// <summary>
        /// Gets or sets the CPT code.
        /// </summary>
        /// <value>
        /// The CPT code.
        /// </value>
        public string CptCode { get; set; }
        /// <summary>
        /// Gets or sets the stop loss condition identifier.
        /// </summary>
        /// <value>
        /// The stop loss condition identifier.
        /// </value>
        public int StopLossConditionId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is excess charge].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is excess charge]; otherwise, <c>false</c>.
        /// </value>
        public bool IsExcessCharge { get; set; }

        public string Expression { get; set; }
    }
}