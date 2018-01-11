using System;
using SSI.ContractManagement.Shared.Models;
using System.Collections.Generic;


namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypeDrgPaymentRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit payment type DRG payment.
        /// </summary>
        /// <param name="paymentTypeDrgPayment">The payment type DRG payment.</param>
        /// <returns></returns>
        long AddEditPaymentTypeDrgPayment(PaymentTypeDrg paymentTypeDrgPayment);

        /// <summary>
        /// Gets all relative weight list.
        /// </summary>
        /// <returns></returns>
        List<RelativeWeight> GetAllRelativeWeightList();

        /// <summary>
        /// Gets the payment type DRG payment.
        /// </summary>
        /// <param name="paymentTypeDrgPayment">The payment type DRG payment.</param>
        /// <returns></returns>
        PaymentTypeDrg GetPaymentTypeDrgPayment(PaymentTypeDrg paymentTypeDrgPayment);

    }
}
