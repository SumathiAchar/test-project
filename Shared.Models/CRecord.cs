using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class CRecord
    {
        /// <summary>
        /// Gets or sets the ClaimId.
        /// </summary>
        /// <value>
        ///  ClaimId.
        /// </value>
        public string ClaimId { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        ///  date of birth.
        /// </value>
        public DateTime Dob { get; set; }

        /// <summary>
        /// Gets or sets the Sex.
        /// </summary>
        /// <value> Valid values are 0(Unknown),1(M) and 2(F)
        ///  Sex.
        /// </value>
        public int Sex { get; set; }

        /// <summary>
        /// Gets or sets the FromDate.
        /// </summary>
        /// <value>
        ///  FromDate.
        /// </value>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Gets or sets the Through Date.
        /// </summary>
        /// <value>
        ///  ThruDate.
        /// </value>
        public DateTime ThruDate { get; set; }

        
        /// <summary>
        /// Gets or sets the ConditionCodes.
        /// </summary>
        /// <value>
        ///  ConditionCodes.
        /// </value>
        public List<string> ConditionCodes { get; set; }

        /// <summary>
        /// Gets or sets the BillType.
        /// </summary>
        /// <value>
        ///  BillType.
        /// </value>
        public string BillType { get; set; }

        /// <summary>
        /// Gets or sets the NPI.
        /// </summary>
        /// <value>
        ///  NPI.
        /// </value>
        public string Npi { get; set; }

        /// <summary>
        /// Gets or sets the OSCAR.
        /// </summary>
        /// <value>
        ///  OSCAR.
        /// </value>
        public string Oscar { get; set; }

        /// <summary>
        /// Gets or sets the PatientStatus.
        /// </summary>
        /// <value>
        ///  PatientStatus.
        /// </value>
        public string PatientStatus { get; set; }

        /// <summary>
        /// Gets or sets the OPPSFlag.
        /// </summary>
        /// <value>
        ///  OPPSFlag.
        /// </value>
        public int OppsFlag { get; set; }

        
        /// <summary>
        /// Gets or sets the OccurrenceCodes.
        /// </summary>
        /// <value>
        ///  OccurrenceCodes.
        /// </value>
        public List<string> OccurrenceCodes { get; set; }

        /// <summary>
        /// Gets or sets the PatientFirstName.
        /// </summary>
        /// <value>
        ///  PatientFirstName.
        /// </value>
        public string PatientFirstName { get; set; }

        /// <summary>
        /// Gets or sets the PatientLastName.
        /// </summary>
        /// <value>
        ///  PatientLastName.
        /// </value>
        public string PatientLastName { get; set; }

        /// <summary>
        /// Gets or sets the PatientMiddleInitial.
        /// </summary>
        /// <value>
        ///  PatientMiddleInitial.
        /// </value>
        public string PatientMiddleInitial { get; set; }

        /// <summary>
        /// Gets or sets the Benefit Amount.
        /// </summary>
        /// <value>
        ///  BeneAmount.
        /// </value>
        public double BeneAmount { get; set; }

        /// <summary>
        /// Gets or sets the BloodPint.
        /// </summary>
        /// <value>
        ///  BloodPint.
        /// </value>
        public int BloodPint { get; set; }

        /// <summary>
        /// Gets or sets the patient data.
        /// </summary>
        /// <value>
        /// The patient data.
        /// </value>
        public PatientData PatientData { get; set; }

    }
}
