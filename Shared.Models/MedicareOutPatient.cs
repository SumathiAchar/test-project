using System;

namespace SSI.ContractManagement.Shared.Models
{
    public class MedicareOutPatient
    {
        /// <summary>
        /// Gets or sets the ClaimId
        /// </summary>
        /// <value>
        ///  ClaimId.
        /// </value>
        public long ClaimId { get; set; }

        /// <summary>
        /// Gets or sets the NPI
        /// </summary>
        /// <value>
        ///  NPI.
        /// </value>
        public string Npi { get; set; }

        /// <summary>
        /// Gets or sets the ServiceDate
        /// </summary>
        /// <value>
        ///  ServiceDate.
        /// </value>
        public DateTime ServiceDate { get; set; }

        /// <summary>
        /// Gets or sets the BloodDeductible
        /// </summary>
        /// <value>
        ///  BloodDeductible.
        /// </value>
        public double BeneDeductible { get; set; }

        /// <summary>
        /// Gets or sets the BloodDeductiblePints
        /// </summary>
        /// <value>
        ///  BloodDeductiblePints.
        /// </value>
        public int BloodDeductiblePints { get; set; }

        /// <summary>
        /// Gets or sets the AllowTerminatorProvider
        /// </summary>
        /// <value>
        ///  AllowTerminatorProvider.
        /// </value>
        public string AllowTerminatorProvider { get; set; }

        /// <summary>
        /// Gets or sets the AdjustFactor
        /// </summary>
        /// <value>
        ///  AdjustFactor.
        /// </value>
        public double AdjustFactor { get; set; }

        /// <summary>
        /// Gets or sets the AdjustmentOptions
        /// </summary>
        /// <value>
        ///  AdjustmentOptions.
        /// </value>
        public int AdjustmentOptions { get; set; }
    }
}
