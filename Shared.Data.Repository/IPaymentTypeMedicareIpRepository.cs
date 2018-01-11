using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypeMedicareIpRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit payment type medicare ip payment.
        /// </summary>
        /// <param name="paymentTypeMedicareIpPayment">The payment type medicare ip payment.</param>
        /// <returns></returns>
        long AddEditPaymentTypeMedicareIpPayment(PaymentTypeMedicareIp paymentTypeMedicareIpPayment);

        /// <summary>
        /// Gets the payment type medicare ip payment.
        /// </summary>
        /// <param name="paymentTypeMedicareIpPayment">The payment type medicare ip payment.</param>
        /// <returns></returns>
        PaymentTypeMedicareIp GetPaymentTypeMedicareIpPayment(PaymentTypeMedicareIp paymentTypeMedicareIpPayment);

    }
}
