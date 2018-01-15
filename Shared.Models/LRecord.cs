using System;

namespace SSI.ContractManagement.Shared.Models
{
    public class LRecord
    {
        /// <summary>
        /// Gets or sets the ClaimId
        /// </summary>
        /// <value>
        ///  ClaimId.
        /// </value>
        public string ClaimId { get; set; }

        /// <summary>
        /// Gets or sets the LineItemId
        /// </summary>
        /// <value>
        ///  LineItemId.
        /// </value>
        public int LineItemId { get; set; }

        /// <summary>
        /// Gets or sets the HCPCSProcedureCode
        /// </summary>
        /// <value>
        ///  HCPCSProcedureCode.
        /// </value>
        public string HcpcsProcedureCode { get; set; }

        /// <summary>
        /// Gets or sets the HCPCSModifiers
        /// </summary>
        /// <value>
        ///  HCPCSModifiers.
        /// </value>
        public string HcpcsModifiers { get; set; }
        
        /// <summary>
        /// Gets or sets the ServiceDate
        /// </summary>
        /// <value>
        ///  ServiceDate.
        /// </value>
        public DateTime ServiceDate { get; set; }

        /// <summary>
        /// Gets or sets the RevenueCode
        /// </summary>
        /// <value>
        ///  RevenueCode.
        /// </value>
        public string RevenueCode { get; set; }

        /// <summary>
        /// Gets or sets the UnitsofService
        /// </summary>
        /// <value>
        ///  UnitsofService.
        /// </value>
        public int? UnitsofService { get; set; }

        /// <summary>
        /// Gets or sets the lineItemCharge
        /// </summary>
        /// <value>
        ///  lineItemCharge.
        /// </value>
        public double? LineItemCharge { get; set; }

        /// <summary>
        /// Gets or sets the lineItemFlag
        /// </summary>
        /// <value>
        ///  lineItemFlag.
        /// </value>
        public int LineItemFlag { get; set; }

    }
}
