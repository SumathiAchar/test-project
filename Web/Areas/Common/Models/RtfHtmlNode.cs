using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.Common.Models
{
    /// <summary>
    /// Its a node object (paragraph) which contains different text/image and formatting
    /// </summary>
    public class RtfHtmlNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RtfHtmlNode"/> class.
        /// </summary>
        public RtfHtmlNode()
        {
            RtfHtmlItem = new List<RtfHtmlItem>();
        }

        /// <summary>
        /// Gets or sets the HTML text.
        /// </summary>
        /// <value>
        /// The HTML text.
        /// </value>
        public List<RtfHtmlItem> RtfHtmlItem { get; private set; }

        /// <summary>
        /// Gets or sets the RTF paragraph format.
        /// </summary>
        /// <value>
        /// The RTF paragraph format.
        /// </value>
        public string RtfParagraphFormat { get; set; }
    }
}