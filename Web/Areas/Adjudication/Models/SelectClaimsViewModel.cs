using System;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Adjudication.Models
{
    public class SelectClaimsViewModel:BaseViewModel
    {
        /// <summary>
        /// Gets or sets the name of the request.
        /// </summary>
        /// <value>
        /// The name of the request.
        /// </value>
        public string RequestName { get; set; }
        /// <summary>
        /// Gets or sets the claim field list.
        /// </summary>
        /// <value>
        /// The claim field list.
        /// </value>
        public string ClaimFieldList { get; set; }
        /// <summary>
        /// Gets or sets the model Id.
        /// </summary>
        /// <value>
        /// The model Ids.
        /// </value>
        public long? ModelId { get; set; }
        /// <summary>
        /// Gets or sets the type of the date.
        /// </summary>
        /// <value>
        /// The type of the date.
        /// </value>
        public int? DateType { get; set; }
        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        /// <value>
        /// The date from.
        /// </value>
        public DateTime? DateFrom { get; set; }
        /// <summary>
        /// Gets or sets the date automatic.
        /// </summary>
        /// <value>
        /// The date automatic.
        /// </value>
        public DateTime? DateTo { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is user defined].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is user defined]; otherwise, <c>false</c>.
        /// </value>
        public bool IsUserDefined { get; set; }
        /// <summary>
        /// Gets or sets the running status.
        /// </summary>
        /// <value>
        /// The running status.
        /// </value>
        public int RunningStatus { get; set; }
        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int? Priority { get; set; }

        public int ModuleId { get; set; }
       
    }
}