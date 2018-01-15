namespace SSI.ContractManagement.Shared.Models
{
    public class MedicareInPatientResult
    {
        public long ClaimId { get; set; }
        public double TotalPaymentAmount { get; set; }
        public int ReturnCode { get; set; }
        public string Message { get; set; }
        public bool IsSucess { get; set; }
    }
}
