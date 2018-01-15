using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class AppealLetter : BaseModel
    {
        /// <summary>
        /// Gets or sets the letter template identifier.
        /// </summary>
        /// <value>
        /// The letter template identifier.
        /// </value>
        public long LetterTemplateId { get; set; }
        
        /// <summary>
        /// Gets or sets the letter templater text.
        /// </summary>
        /// <value>
        /// The letter templater text.
        /// </value>
        public string LetterTemplaterText { get; set; }

        /// <summary>
        /// Gets or sets the node Id.
        /// </summary>
        /// <value>
        /// The node Id.
        /// </value>
        public long? NodeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the date.
        /// </summary>
        /// <value>
        /// The type of the date.
        /// </value>
        public int? DateType { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the claim search criteria.
        /// </summary>
        /// <value>
        /// The claim search criteria.
        /// </value>
        public string ClaimSearchCriteria { get; set; }

        /// <summary>
        /// Gets or sets the maximum no of records.
        /// </summary>
        /// <value>
        /// The maximum no of records.
        /// </value>
        public int MaxNoOfRecords { get; set; }

        /// <summary>
        /// Gets or sets the report threshold.
        /// </summary>
        /// <value>
        /// The report threshold.
        /// </value>
        public int ReportThreshold { get; set; }

        /// <summary>
        /// Gets or sets the requested user identifier.
        /// </summary>
        /// <value>
        /// The requested user identifier.
        /// </value>
        public string RequestedUserId { get; set; }


        /// <summary>
        /// Gets or sets the appeal letter claims.
        /// </summary>
        /// <value>
        /// The appeal letter claims.
        /// </value>
        public List<AppealLetterClaim> AppealLetterClaims { get; set; }

        public bool IsPreview { get; set; }

    }
}
