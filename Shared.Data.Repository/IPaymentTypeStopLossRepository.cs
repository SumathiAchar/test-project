using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypeStopLossRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit payment type stop loss.
        /// </summary>
        /// <param name="paymentTypeStopLoss">The payment type stop loss.</param>
        /// <returns></returns>
        long AddEditPaymentTypeStopLoss(PaymentTypeStopLoss paymentTypeStopLoss);

        /// <summary>
        /// Gets the payment type stop loss.
        /// </summary>
        /// <param name="paymentTypeStopLoss">The payment type stop loss.</param>
        /// <returns></returns>
        PaymentTypeStopLoss GetPaymentTypeStopLoss(PaymentTypeStopLoss paymentTypeStopLoss);

        /// <summary>
        /// Gets the payment type stop loss conditions.
        /// </summary>
        /// <returns></returns>
        List<StopLossCondition> GetPaymentTypeStopLossConditions();
    }
}
