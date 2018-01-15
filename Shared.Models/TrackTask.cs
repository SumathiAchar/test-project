namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Class holds Job related properties
    /// </summary>
    public class TrackTask:BaseModel
    {
        /// <summary>
        /// Get or set Request Name
        /// </summary>
        public string RequestName { get; set; }
        /// <summary>
        /// Get or set TaskId
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// Get or set Status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Get or set Claims Selection Count
        /// </summary>
        public string ClaimsSelectionCount { get; set; }
        /// <summary>
        /// Get or set Progresss
        /// </summary>
        public string Progresss { get; set; }
        /// <summary>
        /// Get or set Elapsed Time
        /// </summary>
        public string ElapsedTime { get; set; }
        /// <summary>
        /// Get or set Completion
        /// </summary>
        public string Completion { get; set; }
        /// <summary>
        /// Get or set Adjudicated Claims Count
        /// </summary>
        public string AdjudicatedClaimsCount { get; set; }
        /// <summary>
        /// Get or set Completed Jobs Duration
        /// </summary>
        public int ThresholdDaysToExpireJobs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is verified].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is verified]; otherwise, <c>false</c>.
        /// </value>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who initiated that job.
        /// </summary>
        /// <value>
        /// The name of the initiated user.
        /// </value>
        public string InitiatedUserName { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the model identifier.
        /// </summary>
        /// <value>
        /// The model identifier.
        /// </value>
        public long ModelId { get; set; }

        /// <summary>
        /// Gets or sets the page setting.
        /// </summary>
        /// <value>
        /// The page setting.
        /// </value>
        public PageSetting PageSetting { get; set; }

        /// <summary>
        /// Gets or sets the total jobs.
        /// </summary>
        /// <value>
        /// The total jobs.
        /// </value>
        public int TotalJobs { get; set; }

        /// <summary>
        /// Gets or sets the name of the model.
        /// </summary>
        /// <value>
        /// The name of the model.
        /// </value>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the Criteria.
        /// </summary>
        /// <value>
        /// The Criteria.
        /// </value>
        public string Criteria { get; set; }
    }
}
