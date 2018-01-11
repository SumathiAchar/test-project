using System;
using SSI.ContractManagement.Shared.Models;


namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypePercentageChargeRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit payment type percentage discount details.
        /// </summary>
        /// <param name="paymentTypePercentageDiscount">The payment type percentage discount.</param>
        /// <returns></returns>
        long AddEditPaymentTypePercentageDiscountDetails(PaymentTypePercentageCharge paymentTypePercentageDiscount);

        /// <summary>
        /// Gets the payment type percentage discount details.
        /// </summary>
        /// <param name="paymentTypePercentageDiscount">The payment type percentage discount.</param>
        /// <returns></returns>
        PaymentTypePercentageCharge GetPaymentTypePercentageDiscountDetails(PaymentTypePercentageCharge paymentTypePercentageDiscount);

    }
}
