using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypePerCaseRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit payment type per case.
        /// </summary>
        /// <param name="paymentTypePerCase">The payment type per case.</param>
        /// <returns></returns>
        long AddEditPaymentTypePerCase(PaymentTypePerCase paymentTypePerCase);

        /// <summary>
        /// Gets the payment type per case.
        /// </summary>
        /// <param name="paymentTypePerCase">The payment type per case.</param>
        /// <returns></returns>
        PaymentTypePerCase GetPaymentTypePerCase(PaymentTypePerCase paymentTypePerCase);

        ///// <summary>
        ///// Edits the payment type per case.
        ///// </summary>
        ///// <param name="paymentTypePerCase">The payment type per case.</param>
        ///// <returns></returns>
        //long EditPaymentTypePerCase(PaymentTypePerCase paymentTypePerCase);
    }
}
