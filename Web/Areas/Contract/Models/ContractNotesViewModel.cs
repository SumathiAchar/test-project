using System;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class ContractNotesViewModel:BaseViewModel
    {
        /// <summary>
        /// Gets or sets the contract note Id.
        /// </summary>
        /// <value>
        /// The contract note Id.
        /// </value>
        public Int64 ContractNoteId { get; set; }
        /// <summary>
        /// Gets or sets the contract Id.
        /// </summary>
        /// <value>
        /// The contract Id.
        /// </value>
        public Int64? ContractId { get; set; }
        /// <summary>
        /// Gets or sets the note text.
        /// </summary>
        /// <value>
        /// The note text.
        /// </value>
        public string NoteText { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int? Status { get; set; }
        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public string Operator { get; set; }

        // ReSharper disable once UnusedMember.Global
        public string ShortDateTime { get; set; }
    }
}