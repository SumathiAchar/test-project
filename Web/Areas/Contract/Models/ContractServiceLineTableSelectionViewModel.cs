using System.Collections.Generic;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class ContractServiceLineTableSelectionViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the contract service line table selection Id.
        /// </summary>
        /// <value>
        /// The contract service line table selection Id.
        /// </value>
        public long ContractServiceLineTableSelectionId { get; set; }
        /// <summary>
        /// Gets or sets the contract service type id.
        /// </summary>
        /// <value>
        /// The contract service type id.
        /// </value>
        public long? ContractServiceTypeId { get; set; }
        /// <summary>
        /// Gets or sets the contract id.
        /// </summary>
        /// <value>
        /// The contract id.
        /// </value>
        public long? ContractId { get; set; }
        /// <summary>
        /// Gets or sets the name of the contract service line table.
        /// </summary>
        /// <value>
        /// The name of the contract service line table.
        /// </value>
        public string TableName { get; set; }
        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int? Status { get; set; }

        /// <summary>
        /// Gets or sets the ClaimFieldId.
        /// </summary>
        /// <value>
        /// The ClaimFieldId.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public long? ClaimFieldId { get; set; }

        /// <summary>
        /// Gets or sets the ClaimFieldDocId.
        /// </summary>
        /// <value>
        /// The ClaimFieldDocId.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public long? ClaimFieldDocId { get; set; }

        //To create list for grid selection
        public List<ContractServiceLineTableSelectionViewModel> TableselectionList { get; set; }

        /// <summary>
        /// Gets or sets the ClaimFieldText.
        /// </summary>
        /// <value>
        /// The Text.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets the IsEdit value.
        /// </summary>
        /// <value>
        /// The isEdit.
        /// </value>
        public bool IsEdit { get; set; }
        /// <summary>
        /// Gets or sets the ServiceLineTypeId.
        /// </summary>
        /// <value>
        /// The ServiceLineTypeId.
        /// </value>
        public long? ServiceLineTypeId { get; set; }
        /// <summary>
        /// Gets or sets the contract service line identifier.
        /// </summary>
        /// <value>
        /// The contract service line identifier.
        /// </value>
        public long ContractServiceLineId { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public int Operator { get; set; }

        /// <summary>
        /// Gets or sets the type of the operator.
        /// </summary>
        /// <value>
        /// The type of the operator.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public string OperatorType { get; set; }

        /// <summary>
        /// Gets or sets the module identifier.
        /// </summary>
        /// <value>
        /// The module identifier.
        /// </value>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the type of the table.
        /// </summary>
        /// <value>
        /// The type of the table.
        /// </value>
        public int? TableType { get; set; }

        /// <summary>
        /// Gets or sets the user text.
        /// </summary>
        /// <value>
        /// The user text.
        /// </value>
        public string UserText { get; set; }
    }
}