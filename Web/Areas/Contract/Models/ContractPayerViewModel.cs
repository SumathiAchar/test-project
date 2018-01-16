using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class ContractPayerViewModel:BaseViewModel
    {
        /// <summary>
        /// Gets or sets the payer Id
        /// </summary>
        /// <value>
        /// The payer Id
        /// </value>
        // ReSharper disable once UnusedMember.Global
        // This property used in report
        public long PayerId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}