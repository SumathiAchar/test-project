using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class ERecord
    {
        /// <summary>
        /// Gets or sets the claim identifier.
        /// </summary>
        /// <value>
        /// The claim identifier.
        /// </value>
        public string ClaimId { get; set; }

        /// <summary>
        /// Gets or sets the admit diagnosis code.
        /// </summary>
        /// <value>
        /// The admit diagnosis code.
        /// </value>
        public string AdmitDiagnosisCode { get; set; }

        /// <summary>
        /// Gets or sets the principal diagnosis code.
        /// </summary>
        /// <value>
        /// The principal diagnosis code.
        /// </value>
        public string PrincipalDiagnosisCode { get; set; }

        /// <summary>
        /// Gets or sets the secondary diagnosis codes.
        /// </summary>
        /// <value>
        /// The secondary diagnosis codes.
        /// </value>
        public List<string> SecondaryDiagnosisCodes { get; set; }
    }
}
