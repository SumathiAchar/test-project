/************************************************************************************************************/
/**  Author         :Raj Zalavadia
/**  Created        :20/Aug/2013
/**  Summary        : Handles Contract Service Types
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class ContractServiceType:BaseModel
    {
         /// <summary>
        /// Gets or sets the Contract Service Type Id.
        /// </summary>
        public long ContractServiceTypeId { get; set; }
        /// <summary>
        /// Gets or sets the CarveOut.
        /// </summary>
        public bool IsCarveOut { get; set; }
        /// <summary>
        /// Gets or sets the Contract Service Type Name
        /// </summary>
        public string ContractServiceTypeName { get; set; }
        /// <summary>
        ///  Gets or sets the Notes
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Gets or sets the Node Id
        /// </summary>
        public long ContractId { get; set; }

        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        /// <value>The conditions.</value>
        public List<ICondition> Conditions { get; set; }

        /// <summary>
        /// Gets or sets the payment types.
        /// </summary>
        /// <value>The payment types.</value>
        public List<PaymentTypeBase> PaymentTypes { get; set; }
        
        /// <summary>
        /// Gets or sets the select tool.
        /// </summary>
        /// <value>
        /// The select tool.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        // This Field used in reports
        public string ClaimTools { get; set; }
        /// <summary>
        /// Gets or sets the payment tool.
        /// </summary>
        /// <value>
        /// The payment tool.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        // This Field used in reports
        public string PaymentTool { get; set; }

        public int CommandTimeoutForContractHierarchyCopyContractServiceTypeById { get; set; }
    }
}
