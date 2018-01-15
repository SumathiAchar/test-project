using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class ClaimNotesContainer:BaseModel
    {
        /// <summary>
        /// Set or Get Claim ID.
        /// </summary>
        /// <value>
        /// The Claim ID.
        /// </value>
        public long ClaimId { get; set; }

        /// <summary>
        /// Gets or sets the claim notes.
        /// </summary>
        /// <value>
        /// The claim notes.
        /// </value>
        public List<ClaimNote> ClaimNotes { get; set; }
    }
}
