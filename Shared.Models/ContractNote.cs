/************************************************************************************************************/
/**  Author         : Mahesh Achina
/**  Created        : 08/Aug/2013
/**  Summary        : Handles Contract Notes
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    public class ContractNote:BaseModel
    {
        /// <summary>
        /// Set or Get Contract Note ID.
        /// </summary>
        /// <value>
        /// The Contract Note ID.
        /// </value>
        public long ContractNoteId { get; set; }
        
        /// <summary>
        /// Set or Get Contract ID.
        /// </summary>
        /// <value>
        /// The Contract ID.
        /// </value>
        public long? ContractId { get; set; }

        /// <summary>
        /// Set or Get Note Text.
        /// </summary>
        /// <value>
        /// The Note Text.
        /// </value>
        public string NoteText { get; set; }

        /// <summary>
        /// Set or Get Status.
        /// </summary>
        /// <value>
        /// The Status.
        /// </value>
        public int? Status { get; set; }

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
