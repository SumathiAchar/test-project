/************************************************************************************************************/
/**  Author         : Mahesh Machina
/**  Created        : 06/Sep/2013
/**  Summary        : Handles Contract Alert List
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/
using System;

namespace SSI.ContractManagement.Shared.Models
{
    public class ContractAlert : BaseModel
    {
        /// <summary>
        /// Gets or sets the ContractName.
        /// </summary>
        /// <value> ContractName.
        /// </value>
        public string ContractName { get; set; }

        /// <summary>
        /// Gets or sets the PayerName.
        /// </summary>
        /// <value> PayerName. </value>
        public string PayerName { get; set; }

        /// <summary>
        /// Gets or sets the ExpiryDate.
        /// </summary>
        /// <value> ExpiryDate. </value>
        public DateTime DateOfExpiry { get; set; }

        /// <summary>
        /// Gets or sets the ContractId.
        /// </summary>
        /// <value> UserId. </value>
        public long ContractId { get; set; }
        
        /// <summary>
        /// Gets or sets the NumberOfDays To Dismiss Alerts.
        /// </summary>
        /// <value>
        /// NumberOfDaysToDismissAlerts.
        /// </value>
        public int NumberOfDaysToDismissAlerts { get; set; }
        /// <summary>
        /// Gets or sets the name of the logged in user.
        /// </summary>
        /// <value>
        /// The name of the logged in user.
        /// </value>
        public string LoggedInUserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is verified].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is verified]; otherwise, <c>false</c>.
        /// </value>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Gets or sets the contract alert identifier.
        /// </summary>
        /// <value>
        /// The contract alert identifier.
        /// </value>
        public long ContractAlertId { get; set; }
    }
}
