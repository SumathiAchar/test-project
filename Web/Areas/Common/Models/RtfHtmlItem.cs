using System.Collections.Generic;
using System.Drawing;

namespace SSI.ContractManagement.Web.Areas.Common.Models
{
    /// <summary>
    /// Its a item object which holds text/image information with formats
    /// </summary>
    public class RtfHtmlItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RtfHtmlItem"/> class.
        /// </summary>
        public RtfHtmlItem()
        {
            RtfFormattedText = new List<string>();
        }

        /// <summary>
        /// Gets or sets the HTML text.
        /// </summary>
        /// <value>
        /// The HTML text.
        /// </value>
        public string HtmlText { get; set; }

        /// <summary>
        /// Gets or sets the HTML image.
        /// </summary>
        /// <value>
        /// The HTML image.
        /// </value>
        public Bitmap HtmlImage { get; set; }

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        /// <value>
        /// The height of the image.
        /// </value>
        public int ImageHeight { get; set; }

        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        /// <value>
        /// The width of the image.
        /// </value>
        public int ImageWidth { get; set; }

        /// <summary>
        /// Gets or sets the RTF formatted text.
        /// </summary>
        /// <value>
        /// The RTF formatted text.
        /// </value>
        public List<string> RtfFormattedText { get; private set; }
    }
}