using System;

namespace SSI.ContractManagement.Shared.Models
{
    public class UploadTable
    {
        /// <summary>
        /// Gets or sets the insert date.
        /// </summary>
        /// <value>
        /// The insert date.
        /// </value>
        public DateTime? InsertDate { set; get; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>
        /// The update date.
        /// </value>
        public DateTime? UpdateDate { set; get; }

        /// <summary>
        /// Gets or sets the facility identifier.
        /// </summary>
        /// <value>
        /// The facility identifier.
        /// </value>
        public long? FacilityId { set; get; }

        /// <summary>
        /// Gets or sets the claim field document identifier.
        /// </summary>
        /// <value>
        /// The claim field document identifier.
        /// </value>
        /// 
        public long? ClaimFieldDocId { set; get; }
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
    }
}
