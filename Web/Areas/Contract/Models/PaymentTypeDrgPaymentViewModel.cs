namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class PaymentTypeDrgPaymentViewModel : PaymentTypeBaseViewModel
    {
        /// <summary>
        /// Set or Get RelativeWeightId of PaymentTypeDRGPayment Table
        /// </summary>
        public long? RelativeWeightId { set; get; }
        /// <summary>
        /// Set or Get Rate of PaymentTypeDRGPayment Table
        /// </summary>
        public double? BaseRate { set; get; }
        /// <summary>
        /// Gets or sets the Claim FieldDoc ID.
        /// </summary>
        /// <value>The Claim FieldDoc ID.</value>
        public long? ClaimFieldDocId { get; set; }
        /// <summary>
        /// Gets or sets the Claim Field Id.
        /// </summary>
        /// <value>The Claim Field Id.</value>
        // ReSharper disable once UnusedMember.Global
        public long? ClaimFieldId { get; set; }
    }
}