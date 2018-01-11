using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypeCustomTableRepository
    {
        string GetHeaders(long documentId);
        long AddEdit(PaymentTypeCustomTable paymentTypeCustomTable);
        PaymentTypeCustomTable GetPaymentTypeCustomTableDetails(PaymentTypeCustomTable paymentTypeCustomTable);
    }
}