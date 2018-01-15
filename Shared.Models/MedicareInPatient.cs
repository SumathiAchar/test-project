using System;

namespace SSI.ContractManagement.Shared.Models
{
    public class MedicareInPatient
    {
        /// <summary>
        /// Gets or sets the ClaimId
        /// </summary>
        /// <value>
        ///  ClaimId.
        /// </value>
        public long ClaimId { get; set; }

        /// <summary>
        /// Gets or sets the LengthOfStay
        /// </summary>
        /// <value>
        ///  LengthOfStay.
        /// </value>
        public int LengthOfStay { get; set; }

        /// <summary>
        /// Gets or sets the Claim charges
        /// </summary>
        /// <value>
        ///  Charges.
        /// </value>
        public double Charges { get; set; }

        /// <summary>
        /// Gets or sets the NPI
        /// </summary>
        /// <value>
        ///  NPI.
        /// </value>
        public string Npi { get; set; }

        
        /// <summary>
        /// Gets or sets the Discharge Status
        /// </summary>
        /// <value>
        ///  DischargeStatus.
        /// </value>
        public string DischargeStatus { get; set; }

        /// <summary>
        /// Gets or sets the Discharge Date
        /// </summary>
        /// <value>
        ///  DischargeDate.
        /// </value>
        public DateTime DischargeDate { get; set; }

        /// <summary>
        /// Gets or sets the DRG
        /// </summary>
        /// <value>
        ///  DRG.
        /// </value>
        public string Drg { get; set; }

    }
}
