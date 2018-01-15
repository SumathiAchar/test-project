using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class PageSetting
    {
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
        /// Gets or sets the sort field.
        /// </summary>
        /// <value>
        /// The sort field.
        /// </value>
        public string SortField { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public string SortDirection { get; set; }

        /// <summary>
        /// Gets or sets the default sort field.
        /// </summary>
        /// <value>
        /// The default sort field.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        // This field is used for default sort. 
        public string DefaultSortField { get; set; }

        /// <summary>
        /// Gets or sets the filter parameter collection.
        /// </summary>
        /// <value>
        /// The filter parameter collection.
        /// </value>
        public List<SearchCriteria> SearchCriteriaList { get; set; }
    }
}
