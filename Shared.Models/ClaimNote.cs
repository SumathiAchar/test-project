namespace SSI.ContractManagement.Shared.Models
{
    public class ClaimNote:BaseModel
    {
        /// <summary>
        /// Set or Get Claim Note ID.
        /// </summary>
        /// <value>
        /// The Claim Note ID.
        /// </value>
        public long ClaimNoteId { get; set; }

        /// <summary>
        /// Set or Get Claim ID.
        /// </summary>
        /// <value>
        /// The Claim ID.
        /// </value>
        public long ClaimId { get; set; }

        /// <summary>
        /// Set or Get ClaimNote Text.
        /// </summary>
        /// <value>
        /// The ClaimNote Text.
        /// </value>
        public string ClaimNoteText { get; set; }

        /// <summary>
        /// Set or Get Operator.
        /// </summary>
        /// <value>
        /// The Operator.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public string Operator { get; set; }

        public string ShortDateTime { get; set; }
    }
}
