using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class PaymentTypeAscFeeScheduleViewModel : PaymentTypeBaseViewModel
    {
        /// <summary>
        /// Gets or sets the primary.
        /// </summary>
        /// <value>The primary.</value>
        public double? Primary { get; set; }
        /// <summary>
        /// Gets or sets the secondary.
        /// </summary>
        /// <value>The secondary.</value>
        public double? Secondary { get; set; }
        /// <summary>
        /// Gets or sets the tertiary.
        /// </summary>
        /// <value>The tertiary.</value>
        public double? Tertiary { get; set; }
        /// <summary>
        /// Gets or sets the quaternary.
        /// </summary>
        /// <value>The quaternary.</value>
        public double? Quaternary { get; set; }
        /// <summary>
        /// Gets or sets the others.
        /// </summary>
        /// <value>The others.</value>
        public double? Others { get; set; }
        /// <summary>
        /// Gets or sets the Claim FieldDoc ID.
        /// </summary>
        /// <value>The Claim FieldDoc ID.</value>
        public long? ClaimFieldDocId { get; set; }
        /// <summary>
        /// Gets or sets the Claim Field Id.
        /// </summary>
        /// <value>The Claim Field Id.</value>
        // ReSharper disable once UnusedMember.Global
        public long? ClaimFieldId { get; set; }

        /// <summary>
        /// Gets or sets the non fee schedule.
        /// </summary>
        /// <value>The non fee schedule.</value>
        public double? NonFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets the asc fee schedule option.
        /// </summary>
        /// <value>
        /// The asc fee schedule option.
        /// </value>
        public List<AscFeeScheduleOptionViewModel> AscFeeScheduleOption { get; set; }

        /// <summary>
        /// Gets or sets the option selection.
        /// </summary>
        /// <value>
        /// The option selection.
        /// </value>
        public int OptionSelection { get; set; }
    }
}