using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Report.Models
{
    public class ClaimSelectorViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the Query identifier.
        /// </summary>
        /// <value>
        /// The query identifier.
        /// </value>
        public int QueryId { get; set; }

        /// <summary>
        /// Gets or sets the query name information.
        /// </summary>
        /// <value>
        /// The query name information.
        /// </value>
        public int QueryName { get; set; }

        /// <summary>
        /// Gets or sets Select Criteria.
        /// </summary>
        /// <value>
        /// SelectCriteria.
        /// </value>
        public string SelectCriteria { get; set; }

    }
}