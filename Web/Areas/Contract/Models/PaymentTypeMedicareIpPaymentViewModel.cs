using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    /// <summary>
    /// Class PaymentTypeMedicareIPPaymentViewModel.
    /// </summary>
    public class PaymentTypeMedicareIpPaymentViewModel : PaymentTypeBaseViewModel
    {
        /// <summary>
        /// Gets or sets the information patient.
        /// </summary>
        /// <value>The information patient.</value>
        public double? InPatient { set; get; }

        /// <summary>
        /// Gets or sets the formula information.
        /// </summary>
        /// <value>The formula information</value>
        public string Formula { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public List<MedicareIpAcuteOptionViewModel> MedicareIpAcuteOptions { get; set; }
    }
}