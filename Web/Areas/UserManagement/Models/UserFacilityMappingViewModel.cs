using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.UserManagement.Models
{
    public abstract class UserFacilityMappingViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the UserId.
        /// </summary>
        /// <value>
        /// The UserId.
        /// </value>
        public new int UserId { get; set; }

        /// <summary>
        /// Gets or sets the FacilityId.
        /// </summary>
        /// <value>
        /// The  FacilityId.
        /// </value>
        public new int FacilityId { get; set; }
    }
}