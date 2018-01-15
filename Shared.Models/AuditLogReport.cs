using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class AuditLogReport : BaseModel
    {
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the facility.
        /// </summary>
        /// <value>
        /// The name of the facility.
        /// </value>
        public string FacilityName { get; set; }

        /// <summary>
        /// Gets or sets the audit log identifier.
        /// </summary>
        /// <value>
        /// The audit log identifier.
        /// </value>
        public long AuditLogId { get; set; }

        /// <summary>
        /// Gets or sets the logged date.
        /// </summary>
        /// <value>
        /// The logged date.
        /// </value>
        public DateTime LoggedDate { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the type of the object.
        /// </summary>
        /// <value>
        /// The type of the object.
        /// </value>
        public string ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the name of the model.
        /// </summary>
        /// <value>
        /// The name of the model.
        /// </value>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the name of the contract.
        /// </summary>
        /// <value>
        /// The name of the contract.
        /// </value>
        public string ContractName { get; set; }

        /// <summary>
        /// Gets or sets the name of the service type.
        /// </summary>
        /// <value>
        /// The name of the service type.
        /// </value>
        public string ServiceTypeName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the command timeout for audit log.
        /// </summary>
        /// <value>
        /// The command timeout for audit log.
        /// </value>
        public int CommandTimeoutForAuditLog { get; set; }

        /// <summary>
        /// Gets or sets the audit log report list.
        /// </summary>
        /// <value>
        /// The audit log report list.
        /// </value>
        public List<AuditLogReport> AuditLogReportList { get; set; }

        /// <summary>
        /// Gets or sets the maximum lines for Csv report.
        /// </summary>
        /// <value>The maximum lines for Csv report.</value>
        public int MaxLinesForCsvReport { get; set; }

        /// <summary>
        /// Gets or sets the time zone information.
        /// </summary>
        /// <value>
        /// The time zone information.
        /// </value>
        public new string CurrentDateTime { get; set; }
    }
}
