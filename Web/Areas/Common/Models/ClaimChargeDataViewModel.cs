using System;

namespace SSI.ContractManagement.Web.Areas.Common.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // No need to Instantiated
    public class ClaimChargeDataViewModel
    {
        public long ClaimId { set; get; }

        /// <summary>
        /// Gets or sets the Line.
        /// </summary>
        public int Line { set; get; }

        /// <summary>
        /// Gets or sets the RevCode.
        /// </summary>
        public string RevCode { set; get; }

        /// <summary>
        /// Gets or sets the HCPCSCode.
        /// </summary>
        public string HcpcsCode { set; get; }

        /// <summary>
        /// Gets or sets the StopLoss ServiceFromDate.
        /// </summary>
        public DateTime ServiceFromDate { set; get; }

        /// <summary>
        /// Gets or sets the ServiceThruDate.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public DateTime ServiceThruDate { set; get; }

        /// <summary>
        /// Gets or sets the Units.
        /// </summary>
        public int? Units { set; get; }

        // ReSharper disable once UnusedMember.Global
        public double? TotalCharges { get; set; }

        public double? CalculatedAllowed { get; set; }

        public double? ExpectedContractualAdjustment { get; set; }

        public double? ActualPayment { get; set; }

        public double? ActualContractualAdjustment { get; set; }
    }
}