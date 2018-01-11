using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypeMedicareSequesterRepository : IDisposable
    {
        /// <summary>
        /// Adds the edit payment type MedicareSequester.
        /// </summary>
        /// <param name="paymentTypeMedicareSequester">The payment type Medicare Sequester.</param>
        /// <returns></returns>
        long AddEditPaymentTypeMedicareSequester(PaymentTypeMedicareSequester paymentTypeMedicareSequester);

        /// <summary>
        /// Gets the payment type Medicare Sequester.
        /// </summary>
        /// <param name="paymentTypeMedicareSequester">The payment type Medicare Sequester.</param>
        /// <returns></returns>
        PaymentTypeMedicareSequester GetPaymentTypeMedicareSequester(PaymentTypeMedicareSequester paymentTypeMedicareSequester);
    }
}
