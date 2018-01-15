/************************************************************************************************************/
/**  Author         : R Bhandari
/**  Created        : 05/Aug/2013
/**  Summary        : Handles Contrac tHierarchy
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{

     [Serializable]
    public class ContractHierarchy : BaseModel
    {

        /// <summary>
        /// Set or Get Node Id
        /// </summary>
        /// <value>
        /// The Contract Docs.
        /// </value>
        public long NodeId { get; set; }

        /// <summary>
        /// Set or Get Parent Id.
        /// </summary>
        /// <value>
        /// The Parent Id.
        /// </value>
        public long? ParentId { get; set; }

        /// <summary>
        /// Set or Get Node Text.
        /// </summary>
        /// <value>
        /// The Node Text.
        /// </value>
        public string NodeText { get; set; }

        /// <summary>
        /// Set or Get Append String.
        /// </summary>
        /// <value>
        /// The Append String.
        /// </value>
        public string AppendString { get; set; }

        /// <summary>
        /// Set or Get IsContract.
        /// </summary>
        /// <value>
        /// The IsContract.
        /// </value>
        public bool? IsContract { get; set; }

        /// <summary>
        /// Set or Get Contract Id.
        /// </summary>
        /// <value>
        /// The Contrac tId.
        /// </value>
        public long ContractId { get; set; }

        /// <summary>
        /// Set or Get Contract Service Type Id.
        /// </summary>
        /// <value>
        /// The Contract Service Type Id.
        /// </value>
        public long ContractServiceTypeId { get; set; }

        /// <summary>
        /// Set or Get Is PrimaryNode.
        /// </summary>
        /// <value>
        /// The IsPrimary Node.
        /// </value>
        public bool IsPrimaryNode { get; set; }

        /// <summary>
        /// Set or Get nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public List<ContractHierarchy> Nodes { get; set; }
        /// <summary>
        /// Set or Get CarveOut.
        /// </summary>
        /// <value>
        /// The CarveOut.
        /// </value>
        public bool IsCarveOut { get; set; }

        public int CommandTimeoutForContractHierarchy { get; set; }
        public string LoggedInUserName { get; set; }
        public string ContractName { get; set; }

    }
}
