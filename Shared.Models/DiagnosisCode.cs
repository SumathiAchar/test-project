/*******************************************************************************************************/
/**  Author         : Prasad & Manjunath
/**  Created        : 12-Sep-2013
/**  Summary        : Handles Contract Adjudication
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Class for DiagnosisCode
    /// </summary>
    public class DiagnosisCode
    {
        /// <summary>
        /// Gets or sets the claim identifier.
        /// </summary>
        /// <value>
        /// The claim identifier.
        /// </value>
        public long ClaimId { set; get; }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public string Instance { set; get; }

        /// <summary>
        /// Gets or sets the icdd code.
        /// </summary>
        /// <value>
        /// The icdd code.
        /// </value>
        public string IcddCode { set; get; }
    }
}
