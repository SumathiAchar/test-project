using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.UserManagement.Models
{
    public abstract class UserTypeViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the UserTypeId.
        /// </summary>
        /// <value>
        /// The UserTypeId.
        /// </value>
        public int UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The  Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the IsActive.
        /// </summary>
        /// <value>
        /// The  IsActive.
        /// </value>
        public bool IsActive { get; set; }
    }
}