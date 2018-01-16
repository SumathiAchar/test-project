using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class ContractServiceTypeViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the contract service type Id.
        /// </summary>
        /// <value>
        /// The contract service type Id.
        /// </value>
        public long ContractServiceTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is carve out].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is carve out]; otherwise, <c>false</c>.
        /// </value>
        public bool IsCarveOut { get; set; }
        /// <summary>
        /// Gets or sets the name of the contract service type.
        /// </summary>
        /// <value>
        /// The name of the contract service type.
        /// </value>
        public string ContractServiceTypeName { get; set; }
        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes { get; set; }
        /// <summary>
        /// Gets or sets the ContractId
        /// </summary>
        public long ContractId { get; set; }

        /// <summary>
        /// Gets or sets the contract node unique identifier.
        /// </summary>
        /// <value>
        /// The contract node unique identifier.
        /// </value>
        public long ContractNodeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isedit.
        /// </summary>
        /// <value>
        ///  true if [is edit]; otherwise, false.
        /// </value>
        public bool IsEdit { get; set; }

        /// <summary>
        /// Gets or sets the select tool.
        /// </summary>
        /// <value>
        /// The select tool.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        // This Field used in reports
        public string ClaimTools { get; set; }
        /// <summary>
        /// Gets or sets the payment tool.
        /// </summary>
        /// <value>
        /// The payment tool.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        // This Field used in reports
        public string PaymentTool { get; set; }
    }
}