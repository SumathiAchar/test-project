namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Class for ConditionCode
    /// </summary>
    public class ConditionCode
    {
        /// <summary>
        /// Gets or sets the claim identifier.
        /// </summary>
        /// <value>
        /// The claim identifier.
        /// </value>
        public long ClaimId { set; get; }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public int Instance { set; get; }

        /// <summary>
        /// Gets or sets the condition code.
        /// </summary>
        /// <value>
        /// The condition code.
        /// </value>
        public string Code { set; get; }
    }
}
