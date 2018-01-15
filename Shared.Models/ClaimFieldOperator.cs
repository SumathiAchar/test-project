namespace SSI.ContractManagement.Shared.Models
{
    public class ClaimFieldOperator:BaseModel
    {
        /// <summary>
        /// Gets or sets the Operator ID.
        /// </summary>
        /// <value>
        /// The Operator OperatorID.
        /// </value>
        public long OperatorId { get; set; }
        /// <summary>
        /// Gets or sets the OperatorType.
        /// </summary>
        /// <value>
        /// The OperatorType.
        /// </value>
        public string OperatorType { get; set; }
    }
}
