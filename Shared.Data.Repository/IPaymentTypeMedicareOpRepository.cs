using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypeMedicareOpRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit payment type medicare property payment.
        /// </summary>
        /// <param name="paymentTypeMedicareOpPayment">The payment type medicare property payment.</param>
        /// <returns></returns>
        long AddEditPaymentTypeMedicareOpPayment(PaymentTypeMedicareOp paymentTypeMedicareOpPayment);

        /// <summary>
        /// Gets the payment type medicare property details.
        /// </summary>
        /// <param name="paymentTypeMedicareOpPayment">The payment type medicare property payment.</param>
        /// <returns></returns>
        PaymentTypeMedicareOp GetPaymentTypeMedicareOpDetails(PaymentTypeMedicareOp paymentTypeMedicareOpPayment);

    }
}
