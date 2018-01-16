namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class PaymentTypeCapViewModel : PaymentTypeBaseViewModel
    {
        /// <summary>
        /// Gets or sets the threshold.
        /// </summary>
        /// <value>The threshold.</value>
        public double? Threshold { get; set; }
        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>The percentage.</value>
        public double? Percentage { get; set; }
    }
}