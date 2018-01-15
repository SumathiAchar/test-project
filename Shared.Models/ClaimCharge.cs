using System;

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Class for ClaimCharge
    /// </summary>
    public class ClaimCharge
    {
        /// <summary>
        /// Gets or sets the StopClaimID
        /// </summary>
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
        /// Gets or sets the HcpcsMods.
        /// </summary>
        public string HcpcsModifiers { set; get; }

        /// <summary>
        /// Gets or sets the StopLoss ServiceFromDate.
        /// </summary>
        public DateTime ServiceFromDate { set; get; }

        /// <summary>
        /// Gets or sets the ServiceThruDate.
        /// </summary>
        public DateTime ServiceThruDate { set; get; }

        /// <summary>
        /// Gets or sets the Units.
        /// </summary>
        public int? Units { set; get; }

        /// <summary>
        /// Gets or sets the Amount.
        /// </summary>
        public double? Amount { set; get; }

        /// <summary>
        /// Gets or sets the NonCoveredCharge.
        /// </summary>
        public double? NonCoveredCharge { set; get; }

        /// <summary>
        /// Gets or sets the CoveredCharge.
        /// </summary>
        public double? CoveredCharge { set; get; }

        /// <summary>
        /// Gets or sets the calculated allowed.
        /// </summary>
        /// <value>
        /// The calculated allowed.
        /// </value>
        public double? CalculatedAllowed { get; set; }

        /// <summary>
        /// Gets or sets the expected contractual adjustment.
        /// </summary>
        /// <value>
        /// The expected contractual adjustment.
        /// </value>
        public double? ExpectedContractualAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the actual payment.
        /// </summary>
        /// <value>
        /// The actual payment.
        /// </value>
        public double? ActualPayment { get; set; }

        /// <summary>
        /// Gets or sets the actual contractual adjustment.
        /// </summary>
        /// <value>
        /// The actual contractual adjustment.
        /// </value>
        public double? ActualContractualAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the place of service.
        /// </summary>
        /// <value>
        /// The place of service.
        /// </value>
        public string PlaceOfService { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is include modifiers selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is include modifiers selected; otherwise, <c>false</c>.
        /// </value>
        public string HcpcsCodeWithModifier { get; set; }
    }
}
