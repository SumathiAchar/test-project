namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class PaymentTypePerCaseViewModel : PaymentTypeBaseViewModel
    {
        /// <summary>
        /// Set or Get Rate of PaymentTypePerCase Table
        /// </summary>
        public double? Rate { set; get; }
        
        /// <summary>
        /// Gets or sets the maximum cases per day.
        /// </summary>
        /// <value>
        /// The maximum cases per day.
        /// </value>
        public int? MaxCasesPerDay { get; set; }
    }
}