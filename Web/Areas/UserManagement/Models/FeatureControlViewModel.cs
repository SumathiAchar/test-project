using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.UserManagement.Models
{
    public class FeatureControlViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the FeatureControlId
        /// </summary>
        /// <value>
        /// The Feature ControlId.
        /// </value>
        public int FeatureControlId { get; set; }

        /// <summary>
        /// Gets or sets the Feature Control Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the IsSelected
        /// </summary>
        /// <value>
        /// The IsSelected.
        /// </value>
        public bool IsSelected { get; set; }
    }
}