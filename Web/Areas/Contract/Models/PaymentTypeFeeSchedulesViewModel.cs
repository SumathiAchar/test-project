namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class PaymentTypeFeeSchedulesViewModel : PaymentTypeBaseViewModel
    {
        /// <summary>
        /// Gets or sets the ContractIDe.
        /// </summary>
        /// <value>The ContractIe.</value>
        public double? FeeSchedule { set; get; }
        /// <summary>
        /// Gets or sets the non fee schedule.
        /// </summary>
        /// <value>The non fee schedule.</value>
        public double? NonFeeSchedule { set; get; }
        /// <summary>
        /// Gets or sets the Claim FieldDoc ID.
        /// </summary>
        /// <value>The Claim FieldDoc ID.</value>
        public long? ClaimFieldDocId { get; set; }

        /// <summary>
        /// Gets or sets the IsObserveUnits 
        /// </summary>
        public bool IsObserveUnits { get; set; }
    }
}