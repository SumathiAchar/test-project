/*******************************************************************************************************/
/**  Author         : Prasad 
/**  Created        : 30-Sep-2013
/**  Summary        : Handles Insured Data
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Class InsuredData
    /// </summary>
    public class InsuredData
    {
        /// <summary>
        /// Gets or sets the claim identifier.
        /// </summary>
        /// <value>
        /// The claim identifier.
        /// </value>
        public long ClaimId { set; get; }

        /// <summary>
        /// Gets or sets the name of the payer.
        /// </summary>
        /// <value>
        /// The name of the payer.
        /// </value>
        public string PayerName { set; get; }

        /// <summary>
        /// Gets or sets the first name of the insured.
        /// </summary>
        /// <value>
        /// The first name of the insured.
        /// </value>
        public string InsuredFirstName { set; get; }

        /// <summary>
        /// Gets or sets the last name of the insured.
        /// </summary>
        /// <value>
        /// The last name of the insured.
        /// </value>
        public string InsuredLastName { set; get; }

        /// <summary>
        /// Gets or sets the name of the insured middle.
        /// </summary>
        /// <value>
        /// The name of the insured middle.
        /// </value>
        public string InsuredMiddleName { set; get; }

        /// <summary>
        /// Gets or sets the certification number.
        /// </summary>
        /// <value>
        /// The certification number.
        /// </value>
        public string CertificationNumber { set; get; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        public string GroupName { set; get; }

        /// <summary>
        /// Gets or sets the group number.
        /// </summary>
        /// <value>
        /// The group number.
        /// </value>
        public string GroupNumber { set; get; }

        /// <summary>
        /// Gets or sets the treatment authorization.
        /// </summary>
        /// <value>
        /// The treatment authorization.
        /// </value>
        public string TreatmentAuthorization { set; get; }
    }
}
