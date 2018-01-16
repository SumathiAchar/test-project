using System.Collections.Generic;
using System.Web.Mvc;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class ContractServiceLineClaimFieldSelectionViewModel:BaseViewModel
    {

        /// <summary>
        /// Gets or sets the contract service line claim field Id.
        /// </summary>
        /// <value>
        /// The contract service line claim field Id.
        /// </value>
        public long ContractServiceLineClaimFieldId { get; set; }
        /// <summary>
        /// Gets or sets the claim field Id.
        /// </summary>
        /// <value>
        /// The claim field Id.
        /// </value>
        public long? ClaimFieldId { get; set; }
        /// <summary>
        /// Gets or sets the contract service type ID.
        /// </summary>
        /// <value>
        /// The contract service type ID.
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
        /// Gets or sets the claim field.
        /// </summary>
        /// <value>
        /// The claim field.
        /// </value>
        public string ClaimField { get; set; }
        /// <summary>
        /// Gets or sets the OperatorType.
        /// </summary>
        /// <value>
        /// The OperatorType.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public string OperatorType { get; set; }
        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public int Operator { get; set; }
        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public string Values { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int? Status { get; set; }
        /// <summary>
        /// Gets or sets the ServiceLineType Id.
        /// </summary>
        /// <value>
        /// The ServiceLineType Id.
        /// </value>
        public long? ServiceLineTypeId { get; set; }

        // ReSharper disable once UnusedMember.Global
        // This field used in report.
        public List<SelectListItem> ClaimTypes { get; set; }

        public List<ContractServiceLineClaimFieldSelectionViewModel> ContractServiceLineClaimFieldSelectionList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is edit].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is edit]; otherwise, <c>false</c>.
        /// </value>
        public bool IsEdit { get; set; }

        public int ModuleId { get; set; }

    }
}