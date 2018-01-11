using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypeMedicareLabFeeScheduleRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit payment type medicare lab fee schedule payment.
        /// </summary>
        /// <param name="paymentTypeMedicareLabFeeSchedulePayment">The payment type medicare lab fee schedule payment.</param>
        /// <returns></returns>
        long AddEditPaymentTypeMedicareLabFeeSchedulePayment(PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedulePayment);

        /// <summary>
        /// Gets the payment type medicare lab fee schedule payment.
        /// </summary>
        /// <param name="paymentTypeMedicareLabFeeSchedulePayment">The payment type medicare lab fee schedule payment.</param>
        /// <returns></returns>
        PaymentTypeMedicareLabFeeSchedule GetPaymentTypeMedicareLabFeeSchedulePayment(PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedulePayment);
    }
}
