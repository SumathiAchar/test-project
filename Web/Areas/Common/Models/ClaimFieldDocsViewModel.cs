namespace SSI.ContractManagement.Web.Areas.Common.Models
{
    public class ClaimFieldDocsViewModel:BaseViewModel
    {
        /// <summary>
        /// Gets or sets the claim field document Id.
        /// </summary>
        /// <value>
        /// The claim field document Id.
        /// </value>
        public long ClaimFieldDocId { set; get; }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { set; get; }       
        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { set; get; }
        /// <summary>
        /// Gets or sets the column header first.
        /// </summary>
        /// <value>
        /// The column header first.
        /// </value>
        public string ColumnHeaderFirst { set; get; }
        /// <summary>
        /// Gets or sets the column header second.
        /// </summary>
        /// <value>
        /// The column header second.
        /// </value>
        public string ColumnHeaderSecond { set; get; }
        /// <summary>
        /// Gets or sets the claim field Id.
        /// </summary>
        /// <value>
        /// The claim field Id.
        /// </value>
        public long? ClaimFieldId { set; get; }
        /// <summary>
        /// Gets or sets the contract Id.
        /// </summary>
        /// <value>
        /// The contract Id.
        /// </value>
        public long? ContractId { set; get; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public long? NodeId { get; set; }
    }
}