/************************************************************************************************************/
/**  Author         : Raj Zalavadia
/**  Created        : 09-Aug-2013
/**  Summary        : Handles Payers
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/
using System;

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Class Payer.
    /// </summary>
    public class Payer : BaseModel
    {
        /// <summary>
        /// Gets or sets the  Payer Id.
        /// </summary>
        ///  <value>
        /// The  Payer Id.
        /// </value>
        public Int64 PayerId { get; set; }

        /// <summary>
        /// Gets or sets the  Payer Name.
        /// </summary>
        ///  <value>
        /// The  Payer Name.
        /// </value>
        public string PayerName { get; set; }

        /// <summary>
        /// Gets or sets the  Is Selected.
        /// </summary>
        ///  <value>
        /// The  Is Selected.
        /// </value>
        public bool IsSelected { get; set; }
      
        /// <summary>
        /// Gets or sets the Contract Id.
        /// </summary>
        ///  <value>
        /// ContractId.
        /// </value>
        public long? ContractId { get; set; }
       
    }
}
