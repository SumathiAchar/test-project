namespace SSI.ContractManagement.Shared.Models
{
    public class WRecordData
    {
        public int LineItemId { get; set; }
        public double LinePaymentAmount { get; set; }
        public int ReturnCode { get; set; }
        public string ErrorMessage { get; set;}
    }
}
