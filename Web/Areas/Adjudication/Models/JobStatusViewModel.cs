using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Adjudication.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // No need to Instantiated
    public class JobStatusViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the name of the request.
        /// </summary>
        /// <value>
        /// The name of the request.
        /// </value>
        public string RequestName { get; set; }

        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        public string TaskId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the no of claims selected.
        /// </summary>
        /// <value>
        /// The no of claims selected.
        /// </value>
        public string NoOfClaimsSelected { get; set; }

        /// <summary>
        /// Gets or sets the progresss.
        /// </summary>
        /// <value>
        /// The progresss.
        /// </value>
        public string Progresss { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time.
        /// </summary>
        /// <value>
        /// The elapsed time.
        /// </value>
        public string ElapsedTime { get; set; }

        /// <summary>
        /// Gets or sets the completion.
        /// </summary>
        /// <value>
        /// The completion.
        /// </value>
        public string Completion { get; set; }

        /// <summary>
        /// Gets or sets the no of claims adjudicated.
        /// </summary>
        /// <value>
        /// The no of claims adjudicated.
        /// </value>
        public string NoOfClaimsAdjudicated { get; set; }

        /// <summary>
        /// Gets or sets the noof days to dismiss completed jobs.
        /// </summary>
        /// <value>
        /// The noof days to dismiss completed jobs.
        /// </value>
        public int NoofDaysToDismissCompletedJobs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is verified.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is verified; otherwise, <c>false</c>.
        /// </value>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Gets or sets the name of the initiated user.
        /// </summary>
        /// <value>
        /// The name of the initiated user.
        /// </value>
        public string InitiatedUserName { get; set; }

        /// <summary>
        /// Gets or sets the model identifier.
        /// </summary>
        /// <value>
        /// The model identifier.
        /// </value>
        public string ModelId { get; set; }

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