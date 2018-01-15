namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Class for Claim Field Values
    /// </summary>
    public class ClaimFieldValue : BaseModel
    {
        /// <summary>
        /// Gets or sets the ClaimFieldValueId.
        /// </summary>
        /// <value>
        /// ClaimFieldValueId.
        /// </value>
        public long ClaimFieldValueId { set; get; }

        /// <summary>
        /// Gets or sets the ContractId.
        /// </summary>
        /// <value>
        /// ContractId.
        /// </value>
        public long? ContractId { set; get; }

        /// <summary>
        /// Gets or sets the ClaimFieldDocId.
        /// </summary>
        /// <value>
        /// ClaimFieldDocId.
        /// </value>
        public long ClaimFieldDocId { set; get; }

        /// <summary>
        /// Gets or sets the Identifier.
        /// </summary>
        /// <value>
        /// Identifier.
        /// </value>
        public string Identifier { set; get; }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        /// <value>
        /// Value.
        /// </value>
        public string Value { set; get; }

        /// <summary>
        /// Gets or sets the ColumnHeaderFirst.
        /// </summary>
        /// <value>
        /// ColumnHeaderFirst.
        /// </value>
        public string ColumnHeaderFirst { set; get; }

        /// <summary>
        /// Gets or sets the ColumnHeaderSecond.
        /// </summary>
        /// <value>
        /// ColumnHeaderSecond.
        /// </value>
        public string ColumnHeaderSecond { set; get; }

        /// <summary>
        /// Gets or sets the line charge value.
        /// </summary>
        /// <value>
        /// The line charge value.
        /// </value>
        public string LineChargeValue { set; get; }

        /// <summary>
        /// Gets or sets the line.
        /// </summary>
        /// <value>
        /// The line.
        /// </value>
        public int Line { get; set; }
    }
}
