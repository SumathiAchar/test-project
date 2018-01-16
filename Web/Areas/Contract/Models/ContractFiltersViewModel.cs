using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
   
    public class ContractFiltersViewModel:BaseViewModel
    {
        /// <summary>
        /// Gets or sets the name of the filter.
        /// </summary>
        /// <value>
        /// The name of the filter.
        /// </value>
        public string FilterName { get; set; }
        /// <summary>
        /// Gets or sets the filter values.
        /// </summary>
        /// <value>
        /// The filter values.
        /// </value>
        public string FilterValues { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is service type filter.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is service type filter; otherwise, <c>false</c>.
        /// </value>
        public bool? IsServiceTypeFilter { get; set; }

        /// <summary>
        /// Gets or sets the Service Line Type Id.
        /// </summary>
        /// <value>
        /// TService Line Type Id.
        /// </value>
        public long? ServiceLineTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Payment Type Id.
        /// </summary>
        /// <value>
        /// Payment Type Id.
        /// </value>
        public long? PaymentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the fullstring(filtername+filtervalues) to show in tooltip.
        /// </summary>
        /// <value>
        /// FullString
        /// </value>
        public string FullString { get; set; }

        /// <summary>
        /// Gets or sets the shortstring(filtername+filtervalues) to display.
        /// </summary>
        /// <value>
        /// ShortString 
        /// </value>
        public string ShortString { get; set; }

        /// <summary>
        /// Tooltip content
        /// </summary>
        /// <value>
        /// ShortString 
        /// </value>
        public string Title { get; set; }

    }
       
    }
