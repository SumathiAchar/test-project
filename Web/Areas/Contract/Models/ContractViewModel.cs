using System;
using System.Collections.Generic;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class ContractViewModel:BaseViewModel
    {
        /// <summary>
        /// Gets or sets the contract basic information.
        /// </summary>
        /// <value>
        /// The contract basic information.
        /// </value>
        public ContractBasicInfoViewModel ContractBasicInfo { get; set; }
        /// <summary>
        /// Gets or sets the contract contact ids.
        /// </summary>
        /// <value>
        /// The contract contact ids.
        /// </value>
        public List<Int64> ContractContactIds { get; set; }
        /// <summary>
        /// Gets or sets the contract notes.
        /// </summary>
        /// <value>
        /// The contract notes.
        /// </value>
        public List<ContractNotesViewModel> ContractNotes { get; set; }
        /// <summary>
        /// Gets or sets the contract upload files.
        /// </summary>
        /// <value>
        /// The contract upload files.
        /// </value>
        public List<ContractUploadFiles> ContractUploadFiles { get; set; }
        /// <summary>
        /// Gets or sets the contract Id.
        /// </summary>
        /// <value>
        /// The contract Id.
        /// </value>
        public long ContractId { get; set; }

        /// <summary>
        /// Gets or sets the node Id.
        /// </summary>
        /// <value>
        /// The node Id.
        /// </value>
        public long? NodeId { get; set; }

        /// <summary>
        /// Gets or sets the payer code.
        /// </summary>
        /// <value>
        /// The payer code.
        /// </value>
        public string PayerCode { get; set; }

        /// <summary>
        /// Gets or sets the user defined field.
        /// </summary>
        /// <value>
        /// The user defined field.
        /// </value>
        public int? CustomField { get; set; }

        /// <summary>
        /// Gets or sets the is parent node.
        /// </summary>
        /// <value>
        /// The is parent node.
        /// </value>
        public bool? IsParentNode { get; set; }

        /// <summary>
        /// Gets or sets the model parent identifier.
        /// </summary>
        /// <value>
        /// The model parent identifier.
        /// </value>
        public long? ModelParentId { get; set; }

        /// <summary>
        /// Gets or sets the model identifier.
        /// </summary>
        /// <value>
        /// The model identifier.
        /// </value>
        public long ModelId { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public string Values { get; set; }

    }
}