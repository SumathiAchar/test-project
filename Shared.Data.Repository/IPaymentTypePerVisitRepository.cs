using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypePerVisitRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit payment type per visit details.
        /// </summary>
        /// <param name="paymentTypePerVisit">The payment type per visit.</param>
        /// <returns></returns>
        long AddEditPaymentTypePerVisitDetails(PaymentTypePerVisit paymentTypePerVisit);

        /// <summary>
        /// Gets the payment type per visit details.
        /// </summary>
        /// <param name="paymentTypePerVisit">The payment type per visit.</param>
        /// <returns></returns>
        PaymentTypePerVisit GetPaymentTypePerVisitDetails(PaymentTypePerVisit paymentTypePerVisit);

        ///// <summary>
        ///// Edits the payment type per visit details.
        ///// </summary>
        ///// <param name="paymentTypePerVisit">The payment type per visit.</param>
        ///// <returns></returns>
        //long EditPaymentTypePerVisitDetails(PaymentTypePerVisit paymentTypePerVisit);
    }
}
