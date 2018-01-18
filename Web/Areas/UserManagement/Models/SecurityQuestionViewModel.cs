using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.UserManagement.Models
{
    public class SecurityQuestionViewModel
    {
        /// <summary>
        /// Gets or sets the question Id.
        /// </summary>
        /// <value>
        /// The question Id.
        /// </value>
        public int QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the name of th question.
        /// </summary>
        /// <value>
        /// The name of the question.
        /// </value>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the question answer.
        /// </summary>
        /// <value>
        /// The question answer.
        /// </value>
        public string Answer { get; set; }

        /// <summary>
        /// Gets or sets QuestionIds
        /// </summary>
        /// <value>
        /// QuestionIds
        /// </value>
        public List<int> QuestionIds { get; set; }
    }
}