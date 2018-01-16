using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.Common.Models
{
    [Serializable]
    public class BaseViewModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the facility Id
        /// </summary>
        /// <value>
        /// The facility Id
        /// </value>
        public int FacilityId { get; set; }
       
        /// <summary>
        /// Gets or sets the user Id.
        /// </summary>
        /// <value>
        /// The user Id.
        /// </value>
        public int UserId { get;  set; }
        /// <summary>
        /// Gets or sets the  Insert Date.
        /// </summary>
        /// <value>
        /// The Insert Date.
        /// </value>
        public DateTime? InsertDate { get; set; }

        /// <summary>
        /// Gets or sets the  Update Date.
        /// </summary>
        /// <value>
        /// The Update Date.
        /// </value>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the  FacilityList.
        /// </summary>
        /// <value>
        /// The FacilityList.
        /// </value>
        public List<int> FacilityList { get; set; }

        /// <summary>
        /// Gets or sets the ssi number.
        /// </summary>
        /// <value>
        /// The ssi number.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public List<int> SsiNumber { get; set; }

        /// <summary>
        /// Gets or sets the RequestedUserName.
        /// </summary>
        /// <value>
        /// The Requested User Name.
        /// </value>
        public string RequestedUserName { get; set; }

        /// <summary>
        /// Gets or sets the time zone information.
        /// </summary>
        /// <value>
        /// The time zone information.
        /// </value>
        public string CurrentDateTime { get; set; }

    }
}
