using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class DRecord
    {
        /// <summary>
        /// Gets or sets the ClaimId
        /// </summary>
        /// <value>
        ///  ClaimId.
        /// </value>
        public string ClaimId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AdmitDiagnosisCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PrincipalDiagnosisCode { get; set; }

        /// <summary>
        /// Gets or sets the Diagnosis data
        /// </summary>
        /// <value>
        ///  List of Diagnosis.
        /// </value>
        public List<string> SecondaryDiagnosisCodes { get; set; }
        
    }
}
