using System;
using SSI.ContractManagement.Shared.Models;


namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypePerDiemRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit payment type per diem details.
        /// </summary>
        /// <param name="paymentTypePerDiemList">The payment type per diem list.</param>
        /// <returns></returns>
        long AddEditPaymentTypePerDiemDetails(PaymentTypePerDiem paymentTypePerDiemList);

        /// <summary>
        /// Gets the payment type per diem.
        /// </summary>
        /// <param name="paymentTypePerDiemList">The payment type per diem list.</param>
        /// <returns></returns>
        PaymentTypePerDiem GetPaymentTypePerDiem(PaymentTypePerDiem paymentTypePerDiemList);

    }
}
