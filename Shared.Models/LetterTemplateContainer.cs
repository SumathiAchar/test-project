using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class LetterTemplateContainer : BaseModel
    {
        /// <summary>
        /// Gets or sets the letter template list.
        /// </summary>
        /// <value>
        /// The letter template list.
        /// </value>
        public List<LetterTemplate> LetterTemplates { get; set; }

        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        /// <value>
        /// The total records.
        /// </value>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the page setting.
        /// </summary>
        /// <value>
        /// The page setting.
        /// </value>
        public PageSetting PageSetting { get; set; }
    }
}
