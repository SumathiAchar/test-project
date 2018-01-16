using System.Collections.Generic;
using SSI.ContractManagement.Shared.Helpers;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class AutomationTool
    {
        /// <summary>
        /// Gets or sets the ServiceLines information.
        /// </summary>
        /// <value>
        /// ServiceLines information.
        /// </value>
        public List<EnumHelper> ServiceLineTypes { get; set; }

        /// <summary>
        /// Gets or sets the PaymentTypes information.
        /// </summary>
        /// <value>
        /// PaymentTypes information.
        /// </value>
        public List<EnumHelper> PaymentTypes { get; set; }
    }
}