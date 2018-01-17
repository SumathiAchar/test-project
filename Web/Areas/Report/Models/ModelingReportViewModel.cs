/************************************************************************************************************/
/**  Author         :Girija
/**  Created        :14-Sept-2013
/**  Summary        :Handles Claim Adjudication Report
/**  User Story Id  :Figure 46
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.Report.Models
{
    public class ModelingReportViewModel : ContractBasicInfoViewModel
    {

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the ContractPhone.
        /// </summary>
        /// <value>
        /// The ContractPhone.
        /// </value>
        public string ContractPhone { get; set; }
        /// <summary>
        /// Gets or sets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        public string ServiceType { get; set; }
        /// <summary>
        /// Gets or sets the payment tool.
        /// </summary>
        /// <value>
        /// The payment tool.
        /// </value>
        public string PaymentTool { get; set; }
        /// <summary>
        /// Gets or sets the payer Id
        /// </summary>
        /// <value>
        /// The payer Id
        /// </value>
        public long PayerId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string ContactInfoName { get; set; }
        /// <summary>
        /// Gets or sets the name of the payer.
        /// </summary>
        /// <value>
        /// The name of the payer.
        /// </value>
        public string PayerName { get; set; }


        /// <summary>
        /// Gets or sets the name of the facility.
        /// </summary>
        /// <value>
        /// The name of the facility.
        /// </value>
        public string FacilityName { get; set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int? PageSize { get; set; }
        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int? PageIndex { get; set; }
        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        /// <value>
        /// The total records.
        /// </value>
        public long? TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the report date.
        /// </summary>
        /// <value>The report date.</value>
        // ReSharper disable once UnusedMember.Global
        public string ReportDate { get; set; }
        /// <summary>
        /// Gets or sets the contracts.
        /// </summary>
        /// <value>The contracts.</value>
        // ReSharper disable once UnusedMember.Global
        public List<ContractBasicInfoViewModel> Contracts { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is carve out.
        /// </summary>
        /// <value><c>true</c> if this instance is carve out; otherwise, <c>false</c>.</value>
        // ReSharper disable once UnusedMember.Global
        public bool IsCarveOut { get; set; }
        /// <summary>
        /// Gets or sets the logged in user.
        /// </summary>
        /// <value>The logged in user.</value>
        public string LoggedInUser { get; set; }

        public List<ModelingReportViewModel> ModelingReports { get; set; }

        public new string CurrentDateTime { get; set; }

        public string PayerCodes { get; set; }
    }
}