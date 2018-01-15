namespace SSI.ContractManagement.Shared.Models
{
    public class FacilityBubble : BaseModel
    {
        /// <summary>
        /// Gets or sets the FacilityBubbleId
        /// </summary>
        /// <value>
        /// The FacilityBubbleId.
        /// </value>
        public int FacilityBubbleId { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// The Description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ConnectionString.
        /// </summary>
        /// <value>
        /// The ConnectionString.
        /// </value>
        public string ConnectionString { get; set; }
    }
}
