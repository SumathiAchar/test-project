using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class FacilityContainer : BaseModel
    {
        /// <summary>
        /// Gets or sets the facility list.
        /// </summary>
        /// <value>
        /// The facility list.
        /// </value>
        public List<Facility> Facilities { get; set; }

        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        /// <value>
        /// The total records.
        /// </value>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets IsActive.
        /// </summary>
        /// <value>
        /// The IsActive.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the page setting.
        /// </summary>
        /// <value>
        /// The page setting.
        /// </value>
        public PageSetting PageSetting { get; set; }
    }
}
