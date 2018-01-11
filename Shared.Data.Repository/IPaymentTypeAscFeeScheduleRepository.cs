using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypeAscFeeScheduleRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit payment type asc fee schedule details.
        /// </summary>
        /// <param name="paymentTypeAscFeeSchedule">The payment type asc fee schedule.</param>
        /// <returns></returns>
        long AddEditPaymentTypeAscFeeScheduleDetails(PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule);

        /// <summary>
        /// Gets the payment type asc fee schedule details.
        /// </summary>
        /// <param name="paymentTypeAscFeeSchedule">The payment type asc fee schedule.</param>
        /// <returns></returns>
        PaymentTypeAscFeeSchedule GetPaymentTypeAscFeeScheduleDetails(PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule);

        /// <summary>
        /// Gets the table name selection.
        /// </summary>
        /// <param name="paymenttype">The paymenttype.</param>
        /// <returns></returns>
        List<PaymentTypeTableSelection> GetTableNameSelection(PaymentTypeAscFeeSchedule paymenttype);

        /// <summary>
        /// Gets the asc fee schedule options.
        /// </summary>
        /// <returns></returns>
        List<AscFeeScheduleOption> GetAscFeeScheduleOptions();
    }
}
