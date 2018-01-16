using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class ContractServiceLineViewModel:BaseViewModel
    {
        /// <summary>
        /// Gets or sets the contract service line Id.
        /// </summary>
        /// <value>
        /// The contract service line Id.
        /// </value>
        public long ContractServiceLineId { get; set; }
        /// <summary>
        /// Gets or sets the service line type Id.
        /// </summary>
        /// <value>
        /// The service line type Id.
        /// </value>
        public long? ServiceLineTypeId { get; set; }
        /// <summary>
        /// Gets or sets the contract service type Id.
        /// </summary>
        /// <value>
        /// The contract service type Id.
        /// </value>
        public long? ContractServiceTypeId { get; set; }
        /// <summary>
        /// Gets or sets the contract Id.
        /// </summary>
        /// <value>
        /// The contract Id.
        /// </value>
        public long? ContractId { get; set; }
        /// <summary>
        /// Gets or sets the included code.
        /// </summary>
        /// <value>
        /// The included code.
        /// </value>
        public string IncludedCode { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int? Status { get; set; }
        /// <summary>
        /// Gets or sets the excluded code.
        /// </summary>
        /// <value>
        /// The excluded code.
        /// </value>
        public string ExcludedCode { get; set; }
       
        /// <summary>
        /// Gets or sets a value indicating whether [is edit].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is edit]; otherwise, <c>false</c>.
        /// </value>
        public bool IsEdit { get; set; }

        //todo: need to add one field for is modified(isModified)


        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }
        public long? TotalRecords { get; set; }
    }
}