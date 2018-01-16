
namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class PaymentTypeLesserOfViewModel : PaymentTypeBaseViewModel
    {
        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>The percentage.</value>
        public double? Percentage { get; set; }

        /// <summary>
        /// Gets or sets the is lesser of.
        /// </summary>
        /// <value>
        /// The is lesser of.
        /// </value>
        public bool? IsLesserOf { get; set; }
    }
}