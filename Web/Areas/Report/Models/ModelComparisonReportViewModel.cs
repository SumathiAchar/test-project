using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.Report.Models
{
    public class ModelComparisonReportViewModel : ContractBasicInfoViewModel
    {
        /// <summary>
        /// Gets or sets the type of the date.
        /// </summary>
        /// <value>
        /// The type of the date.
        /// </value>
        public int DateType { get; set; }

        /// <summary>
        /// Gets or sets the model identifier.
        /// </summary>
        /// <value>
        /// The model identifier.
        /// </value>
        public long ModelId { get; set; }

        /// <summary>
        /// Gets or sets the claim search criteria.
        /// </summary>
        /// <value>
        /// The claim search criteria.
        /// </value>
        public string ClaimSearchCriteria { get; set; }

        /// <summary>
        /// Gets or sets the selected model list.
        /// </summary>
        /// <value>
        /// The selected model list.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public string SelectedModelList { get; set; }

        /// <summary>
        /// Gets or sets the detail select value.
        /// </summary>
        /// <value>
        /// The detail select value.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public int DetailSelectValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked detail level.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is checked detail level; otherwise, <c>false</c>.
        /// </value>
        public bool IsCheckedDetailLevel { get; set; }

        /// <summary>
        /// Gets or sets the model comparison data.
        /// </summary>
        /// <value>
        /// The model comparison data.
        /// </value>
        public List<ModelComparisonReportDetails> ModelComparisonData { get; set; }

        /// <summary>
        /// Gets or sets the RequestedUserID.
        /// </summary>
        /// <value>
        /// The Requested User ID.
        /// </value>
        public string RequestedUserId { get; set; }

        /// <summary>
        /// Gets or sets the module identifier.
        /// </summary>
        /// <value>
        /// The module identifier.
        /// </value>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the time zone information.
        /// </summary>
        /// <value>
        /// The time zone information.
        /// </value>
        public new string CurrentDateTime { get; set; }
    }
}