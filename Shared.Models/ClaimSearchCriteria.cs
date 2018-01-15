using System;

namespace SSI.ContractManagement.Shared.Models
{
    public class ClaimSearchCriteria : BaseModel
    {
        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        public string SearchCriteria { get; set; }

        /// <summary>
        /// Gets or sets the type of the date.
        /// </summary>
        /// <value>
        /// The type of the date.
        /// </value>
        public int DateType { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public long? NodeId { get; set; }

        /// <summary>
        /// Gets or sets the take.
        /// </summary>
        /// <value>
        /// The take.
        /// </value>
        public int Take { get; set; }

        /// <summary>
        /// Gets or sets the skip.
        /// </summary>
        /// <value>
        /// The skip.
        /// </value>
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int PageIndex { get; set; }

        
        /// <summary>
        /// Gets or sets the requested user identifier.
        /// </summary>
        /// <value>
        /// The requested user identifier.
        /// </value>
        public string RequestedUserId { get; set; }


        /// <summary>
        /// Gets or sets the select all clicked.
        /// </summary>
        /// <value>
        /// The select all clicked.
        /// </value>
        public bool IsSelectAll { get; set; }

        /// <summary>
        /// Gets or sets the model identifier.
        /// </summary>
        /// <value>
        /// The model identifier.
        /// </value>
        public long ModelId { get; set; }

        /// <summary>
        /// Gets or sets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public long ContractId { get; set; }

        /// <summary>
        /// Gets or sets the index of the previous page.
        /// </summary>
        /// <value>
        /// The index of the previous page.
        /// </value>
        public int LastSelectedPageIndex { get; set; }
    }
}
