
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class ContractModifiedReasonViewModel:BaseViewModel
    {
        /// <summary>
        /// Set or Get ContractID of ContractModifiedReasonCode Table
        /// </summary>
        public long? ContractId { set; get; }
        /// <summary>
        /// Set or Get ReasonCode
        /// </summary>
        public long? ReasonCode { set; get; }
        /// <summary>
        /// Set or Get Notes
        /// </summary>
        public string Notes { set; get; }

        /// <summary>
        /// Set or Get Contract Id.
        /// </summary>
        /// <value>
        /// The Contract Id.
        /// </value>
        public long? NodeId { set; get; }

       
    }
}