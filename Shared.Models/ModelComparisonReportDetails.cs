namespace SSI.ContractManagement.Shared.Models
{
    public class ModelComparisonReportDetails 
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public string Count { get; set; }

        /// <summary>
        /// Gets or sets the claim data.
        /// </summary>
        /// <value>
        /// The claim data.
        /// </value>
        public EvaluateableClaim  ClaimData { get; set; }
    }
}
