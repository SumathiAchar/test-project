using System;

namespace SSI.ContractManagement.Shared.Models
{
    //FIXED-MAY Please add summary with the purpose of this model.
    public class AdjudicationLog : BaseModel
    {
        public long ClaimId { get; set; }

        public string StatusCode { get; set; }

        public long? ContractId { get; set; }

        public string ContractName { get; set; }

        public string ServiceTypeName { get; set; }

        public int? ServiceLine { get; set; }

        public string PaymentType { get; set; }

        public DateTime CurrentDateTime { get; set; }
    }
}
