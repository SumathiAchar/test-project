using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class PaymentTableContainer
    {
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public int Total { get; set; }
        
        /// <summary>
        /// Gets or sets the claim field values.
        /// </summary>
        /// <value>
        /// The claim field values.
        /// </value>
        public List<ClaimFieldValue> ClaimFieldValues { get; set; }
    }
}
