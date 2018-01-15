using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class MicrodynApcEditInput
    {
        public long ClaimId { get; set; }
        public CRecord CRecord { get; set; }
        public DRecord DRecord { get; set; }
        public List<LRecord> LRecords { get; set; }
        public MedicareOutPatient MedicareOutPatientRecord { get; set; }
        public ERecord ERecord { get; set; }
    }
}