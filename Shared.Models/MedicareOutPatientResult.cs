using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class MedicareOutPatientResult
    {
        public string ClaimId { get; set; }
        public double TotalPaymentAmount { get; set; }
        public List<WRecordData> LineItemMedicareDetails { get; set; }
        public int ReturnCode { get; set; }
        public string Message { get; set;}
        //public bool IsSuccess { get; set; }
        public bool IsEditSuccess { get; set; }
        public bool IsPricerSuccess { get; set; }
        public string EditErrorCodes { get; set; }
        public int? PricerErrorCodes { get; set; }
        public string MicrodynEditReturnRemarks { get; set; }
    }
}
