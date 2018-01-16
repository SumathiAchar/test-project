using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;

namespace SSI.ContractManagement.Web.Areas.Common.Models
{
    /// <summary>
    /// Encapsulates information about a user.
    /// </summary>
    [DataContract]
    [Serializable]
    public class UserInfo
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the assigned facilities.
        /// </summary>
        /// <value>
        /// The assigned facilities.
        /// </value>
        public List<FacilityViewModel> AssignedFacilities { get; set; }

        /// <summary>
        /// Gets or sets the user type identifier.
        /// </summary>
        /// <value>
        /// The user type identifier.
        /// </value>
        public int UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>
        /// The user key.
        /// </value>
        public string UserKey { get; set; }


        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        /// <value>
        /// The UserId
        /// </value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets LastLoginDate.
        /// </summary>
        /// <value>
        /// The LastLoginDate.
        /// </value>
        public string LastLoginDate { get; set; }

        /// <summary>
        /// Gets or sets PasswordExpirationDays.
        /// </summary>
        /// <value>
        /// The PasswordExpirationDays.
        /// </value>
        public string PasswordExpirationDays { get; set; }

        /// <summary>
        /// Gets or sets the landing page identifier.
        /// </summary>
        /// <value>
        /// The landing page identifier.
        /// </value>
        public byte LandingPageId { get; set; }


        /// <summary>
        /// Gets or sets the IsSingleFacility.
        /// </summary>
        /// <value>
        /// The  Single Facility Flag.
        /// </value>
        public int? SingleFacility { get; set; }

        /// <summary>
        /// Gets or sets the IsFromSecurityPage.
        /// </summary>
        /// <value>
        /// The IsFromSecurityPage.
        /// </value>
        public bool IsFromSecurityPage { get; set; }

    }
}