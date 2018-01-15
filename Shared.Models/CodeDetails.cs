using System;

namespace SSI.ContractManagement.Shared.Models
{
    public class CodeDetails
    {
        /// <summary>
        /// Gets or sets the occurence.
        /// </summary>
        /// <value>
        /// The occurence.
        /// </value>
        public int Occurence { get; set; }

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        /// <value>
        /// The day.
        /// </value>
        public DateTime Day { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the per day of stay limit.
        /// </summary>
        /// <value>
        /// The per day of stay limit.
        /// </value>
        public int Limit { get; set; }

    }
}
