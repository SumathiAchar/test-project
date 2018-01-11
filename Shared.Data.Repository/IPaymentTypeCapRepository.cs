using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypeCapRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit payment type cap details.
        /// </summary>
        /// <param name="paymentTypeCap">The payment type cap.</param>
        /// <returns></returns>
        long AddEditPaymentTypeCapDetails(PaymentTypeCap paymentTypeCap);

        /// <summary>
        /// Gets the payment type cap details.
        /// </summary>
        /// <param name="paymentTypeCap">The payment type cap.</param>
        /// <returns></returns>
        PaymentTypeCap GetPaymentTypeCapDetails(PaymentTypeCap paymentTypeCap);

        ///// <summary>
        ///// Edits the payment type cap details.
        ///// </summary>
        ///// <param name="paymentTypeCap">The payment type cap.</param>
        ///// <returns></returns>
        //long EditPaymentTypeCapDetails(PaymentTypeCap paymentTypeCap);
    }
}
