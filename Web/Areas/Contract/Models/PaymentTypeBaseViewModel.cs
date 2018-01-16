using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class PaymentTypeBaseViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the payment type detail identifier.
        /// </summary>
        /// <value>
        /// The payment type detail identifier.
        /// </value>
        public long PaymentTypeDetailId { set; get; }

        /// <summary>
        /// Gets or sets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public long? ContractId { set; get; }

        /// <summary>
        /// Gets or sets the payment type identifier.
        /// </summary>
        /// <value>
        /// The payment type identifier.
        /// </value>
        public int PaymentTypeId { set; get; }

        /// <summary>
        /// Gets or sets the service type identifier.
        /// </summary>
        /// <value>
        /// The service type identifier.
        /// </value>
        public long? ServiceTypeId { set; get; }

        /// <summary>
        /// Gets or sets the IsEdit.
        /// </summary>
        /// <value>The IsEdit.</value>
        public bool IsEdit { get; set; }
    }
}