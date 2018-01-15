/*******************************************************************************************************/
/**  Author         : Prasad Dintakurti
/**  Created        : 05-Aug-2013
/**  Summary        : Handles Select Claims Model
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/
using System;

namespace SSI.ContractManagement.Shared.Models
{
    public class ClaimSelector : BaseModel
    {
        /// <summary>
        /// Gets or sets the Task Id.
        /// </summary>
        /// <value>
        /// Task Id.
        /// </value>
        public long ClaimSelectorId { get; set; }
        /// <summary>
        /// Gets or sets the RequestName.
        /// </summary>
        /// <value>
        /// RequestName.
        /// </value>
        public string RequestName { get; set; }

        /// <summary>
        /// Gets or sets the ClaimFieldList.
        /// </summary>
        /// <value>
        /// ClaimFieldList.
        /// </value>
        public string ClaimFieldList { get; set; }

        /// <summary>
        /// Gets or sets the ModelId.
        /// </summary>
        /// <value>
        /// ModelId.
        /// </value>
        public long? ModelId { get; set; }

        /// <summary>
        /// Gets or sets the DateType.
        /// </summary>
        /// <value>
        /// DateType.
        /// </value>
        public int? DateType { get; set; }

        /// <summary>
        /// Gets or sets the DateFrom.
        /// </summary>
        /// <value>
        /// DateFrom.
        /// </value>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the DateTo.
        /// </summary>
        /// <value>
        /// DateTo.
        /// </value>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Gets or sets IsUserDefined.
        /// </summary>
        /// <value>
        /// IsUserDefined.
        /// </value>
        public bool IsUserDefined { get; set; }

        /// <summary>
        /// Gets or sets RunningStatus.
        /// </summary>
        /// <value>
        /// RunningStatus.
        /// </value>
        public int RunningStatus { get; set; }

        /// <summary>
        /// Gets or sets Priority.
        /// </summary>
        /// <value>
        /// Priority.
        /// </value>
        public int? Priority { get; set; }

        /// <summary>
        /// Gets or sets Select Criteria.
        /// </summary>
        /// <value>
        /// SelectCriteria.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public string SelectCriteria { get; set; }

        /// <summary>
        /// Gets or sets the command timeout for select claim idsfor adjudicate.
        /// </summary>
        /// <value>
        /// The command timeout for select claim idsfor adjudicate.
        /// </value>
        public int CommandTimeoutForSelectClaimIdsforAdjudicate { get; set; }

        /// <summary>
        /// Gets or sets the command timeout for check adjudication request name exist.
        /// </summary>
        /// <value>
        /// The command timeout for check adjudication request name exist.
        /// </value>
        public int CommandTimeoutForCheckAdjudicationRequestNameExist { get; set; }

        /// <summary>
        /// Gets or sets the command timeout for get adjudication request names.
        /// </summary>
        /// <value>
        /// The command timeout for get adjudication request names.
        /// </value>
        public int CommandTimeoutForGetAdjudicationRequestNames { get; set; }

        /// <summary>
        /// Gets or sets the duration of the completed jobs.
        /// </summary>
        /// <value>
        /// The duration of the completed jobs.
        /// </value>
        public int CompletedJobsDuration { get; set; }

        /// <summary>
        /// Gets or sets the module identifier.
        /// </summary>
        /// <value>
        /// The module identifier.
        /// </value>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the Query identifier.
        /// </summary>
        /// <value>
        /// The query identifier.
        /// </value>
        public int QueryId { get; set; }

        /// <summary>
        /// Gets or sets the query name information.
        /// </summary>
        /// <value>
        /// The query name information.
        /// </value>
        public string QueryName { get; set; }

        /// <summary>
        /// Gets or sets the CriteriaDetails
        /// </summary>
        /// <value>
        /// CriteriaDetails
        /// </value>
        public string CriteriaDetails { get; set; }
    }
}
