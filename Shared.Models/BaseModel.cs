/************************************************************************************************************/
/**  Author         : Anantasairam
/**  Created        : 17/Aug/2013
/**  Summary        : Handles BaseModel
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/
using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    [Serializable]
    public class BaseModel
    {
        /// <summary>
        /// Gets or sets the  User Id.
        /// </summary>
        /// <value>
        /// The User Id.
        /// </value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the  User Name.
        /// </summary>
          /// <value>
        /// The User Name.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the  Facility Id.
        /// </summary>
        /// <value>
        /// The Facility Id.
        /// </value>
        public int FacilityId { get; set; }  
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

        /// <summary>
        /// Gets or sets the facility name information.
        /// </summary>
        /// <value>
        /// The facility name information.
        /// </value>
        public string FacilityName { get; set; }

    }
}
