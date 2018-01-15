namespace SSI.ContractManagement.Shared.Models
{
    public class ClaimsReviewed : BaseModel
    {
        /// <summary>
        /// Gets or sets the claim identifier.
        /// </summary>
        /// <value>
        /// The claim identifier.
        /// </value>
        public long ClaimId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is reviewed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is reviewed; otherwise, <c>false</c>.
        /// </value>
        public bool IsReviewed { get; set; }

        /// <summary>
        /// Gets or sets the model identifier.
        /// </summary>
        /// <value>
        /// The model identifier.
        /// </value>
        public long ModelId { get; set; }
    }
}
