using System.Collections.Generic;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Adjudication.Models
{
    public class ClaimColumnOptionsViewModel:BaseViewModel
    {

        /// <summary>
        /// Gets or sets the ClaimColumnOptionId
        /// </summary>
        public int ClaimColumnOptionId { set; get; }

        /// <summary>
        /// Gets or sets the ColumnName.
        /// </summary>
        public string ColumnName { set; get; }

        /// <summary>
        /// Gets or sets the available Column list.
        /// </summary>
        /// <value>
        /// The available Column list.
        /// </value>
        public List<ClaimColumnOptionsViewModel> AvailableColumnList { get; set; }

        /// <summary>
        /// Gets or sets the selected Column list.
        /// </summary>
        /// <value>
        /// The selected Column list.
        /// </value>
        public List<ClaimColumnOptionsViewModel> SelectedColumnList { get; set; }

        /// <summary>
        /// Gets or sets the claim column option ids.
        /// </summary>
        /// <value>
        /// The claim column option ids.
        /// </value>
        public string ClaimColumnOptionIds { get; set; }

    }
}