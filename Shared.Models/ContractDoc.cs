/************************************************************************************************************/
/**  Author         : Mahesh Achina
/**  Created        : 08/Aug/2013
/**  Summary        : Handles Contract Docs
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;

namespace SSI.ContractManagement.Shared.Models
{
    public class ContractDoc:BaseModel
    {


        /// <summary>
        /// Set or Get ContractDocID.
        /// </summary>
        /// <value>
        /// The ContractDoc ID.
        /// </value>
        public long ContractDocId { get; set; }


        /// <summary>
        /// Set or Get Contract ID.
        /// </summary>
        /// <value>
        /// The Contract ID.
        /// </value>
        public long? ContractId { get; set; }

        /// <summary>
        /// Set or Get Contract Content.
        /// </summary>
        /// <value>
        /// The Contract Content.
        /// </value>
        public byte[] ContractContent { get; set; }
                
        /// <summary>
        /// Set or Get File Name.
        /// </summary>
        /// <value>
        /// The File Name.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Set or Get Operator.
        /// </summary>
        /// <value>
        /// The Operator.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public String Operator { get; set; }

        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        public Guid? DocumentId { get; set; }
    }
}
