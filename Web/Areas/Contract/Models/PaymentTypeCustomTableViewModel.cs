using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class PaymentTypeCustomTableViewModel : PaymentTypeBaseViewModel
    {
        public string Expression { get; set; }
        /// <summary>
        /// Gets or sets the Claim FieldDoc ID.
        /// </summary>
        /// <value>The uploaded document ID.</value>
        public long? DocumentId { get; set; }
        /// <summary>
        /// Gets or sets the Claim Field Id.
        /// </summary>
        /// <value>The Claim Field Id.</value>
        public long? ClaimFieldId { get; set; }

        /// <summary>
        /// CMS module ID. Enums.Modules
        /// </summary>
        public int ModuleId { get; set; }

        public Dictionary<string, string> TableHeaders { get; set; }

        /// <summary>
        /// Gets or sets the multiplier first.
        /// </summary>
        public string MultiplierFirst { get; set; }

        /// <summary>
        /// Gets or sets the multiplier second.
        /// </summary>
        public string MultiplierSecond { get; set; }

        /// <summary>
        /// Gets or sets the multiplier third.
        /// </summary>
        public string MultiplierThird { get; set; }

        /// <summary>
        /// Gets or sets the multiplier fourth.
        /// </summary>
        public string MultiplierFourth { get; set; }

        /// <summary>
        /// Gets or sets the multiplier other.
        /// </summary>
        public string MultiplierOther { get; set; }

        /// <summary>
        /// Gets or sets the observe unit check box.
        /// </summary>
        public bool IsObserveServiceUnit { get; set; }

        /// <summary>
        /// Gets or sets the observe service unit limit.
        /// </summary>
        public string ObserveServiceUnitLimit { get; set; }

        /// <summary>
        /// Gets or sets the per day of stay.
        /// </summary>
        public bool IsPerDayOfStay { get; set; }

        /// <summary>
        /// Gets or sets the per code.
        /// </summary>
        public bool IsPerCode { get; set; }

    }
}