namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Class for letter template
    /// </summary>
    public class LetterTemplate : BaseModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long LetterTemplateId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string TemplateText { get; set; }
       
        
    }
}
