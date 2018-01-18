using System;
using System.Collections.Generic;
using SSI.ContractManagement.Web.Areas.Common.Models;

//FIXED-FEB16 - Remove directives which are not required

namespace SSI.ContractManagement.Web.Areas.UserManagement.Models
{
    public class UserViewModel : BaseViewModel
    {
        
        /// <summary>
        /// Gets or sets the LoggedInUserId.
        /// </summary>
        /// <value>
        /// The LoggedInUserId.
        /// </value>
        public int LoggedInUserId { get; set; }

        /// <summary>
        /// Gets or sets the User GUID.
        /// </summary>
        /// <value>
        /// The User GUID.
        /// </value>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Gets or sets the PasswordHash.
        /// </summary>
        /// <value>
        /// The PasswordHash.
        /// </value>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the PasswordSalt.
        /// </summary>
        /// <value>
        /// The PasswordSalt.
        /// </value>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the FirstName.
        /// </summary>
        /// <value>
        /// The FirstName .
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName.
        /// </summary>
        /// <value>
        /// The LastName.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the MiddleName.
        /// </summary>
        /// <value>
        /// The MiddleName.
        /// </value>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the IsLocked.
        /// </summary>
        /// <value>
        /// The IsLocked.
        /// </value>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets the selected UserTypeId.
        /// </summary>
        /// <value>
        /// The  UserTypeId.
        /// </value>
        public int UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate.
        /// </summary>
        /// <value>
        /// The  CreatedDate.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the IsAdmin.
        /// </summary>
        /// <value>
        /// The  IsAdmin.
        /// </value>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets the available Facility list.
        /// </summary>
        /// <value>
        /// The available Facility list.
        /// </value>
        public List<FacilityViewModel> AvailableFacilityList { get; set; }

        /// <summary>
        /// Gets or sets the selected Facility list.
        /// </summary>
        /// <value>
        /// The selected Facility list.
        /// </value>
        public List<FacilityViewModel> SelectedFacilityList { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public new string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public string SelectedFacility { get; set; }

        /// <summary>
        /// Gets or sets the LastLockoutDate.
        /// </summary>
        /// <value>
        /// The LastLockoutDate.
        /// </value>
        public DateTime LastLockoutDate { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        /// <value>
        /// The Email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the available security question list and answer.
        /// </summary>
        /// <value>
        /// The available security question list and answer.
        /// </value>
        //FIXED-FEB16 - Rename userSecurityQuestion to UserSecurityQuestion
        public List<SecurityQuestionViewModel> UserSecurityQuestion { get; set; }

        /// <summary>
        /// Gets or sets the PasswordExpirationDays.
        /// </summary>
        /// <value>
        /// The PasswordExpirationDays.
        /// </value>
        public int PasswordExpirationDays { get; set; }

        /// <summary>
        /// Email Type Enum
        /// </summary>
        public int EmailType { get; set; }

        /// <summary>
        /// Gets or sets the IsPasswordResetLock.
        /// </summary>
        /// <value>
        /// The IsPasswordResetLock.
        /// </value>
        public bool IsPasswordResetLock { get; set; }

        /// <summary>
        /// Gets or sets the landing page identifier.
        /// </summary>
        /// <value>
        /// The landing page identifier.
        /// </value>
        public byte LandingPageId { get; set; }
    }
}