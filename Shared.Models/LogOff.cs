using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class LogOff
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is session time out.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is session time out; otherwise, <c>false</c>.
        /// </value>
        public bool IsSessionTimeOut { get; set; }

        /// <summary>
        /// Gets or sets the user facilities.
        /// </summary>
        /// <value>
        /// The user facilities.
        /// </value>
        public List<Facility> UserFacilities { get; set; }

        /// <summary>
        /// Gets or sets the facility ids.
        /// </summary>
        /// <value>
        /// The facility ids.
        /// </value>
        public string FacilityIds { get; set; }
    }
}
