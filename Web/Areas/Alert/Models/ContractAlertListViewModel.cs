using System;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Alert.Models
{
    public class ContractAlertViewModel : BaseViewModel
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