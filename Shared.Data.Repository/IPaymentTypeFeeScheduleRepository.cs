/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Fee Schedule docs uploaded
/**  User Story Id  : 4.User Story Model a contract Figure 6
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypeFeeScheduleRepository : IDisposable
    {
        /// <summary>
        /// Adds the payment type fee schedule details.
        /// </summary>
        /// <param name="paymentTypeFeeSchedules">The payment type asc fee schedule.</param>
        /// <returns>ResponseObject</returns>
        long AddEditPaymentTypeFeeSchedule(PaymentTypeFeeSchedule paymentTypeFeeSchedules);

        /// <summary>
        /// Get the payment type fee schedule details.
        /// </summary>
        /// <param name="paymentTypeFeeSchedules">The payment type asc fee schedule.</param>
        /// <returns>ResponseObject</returns>
        PaymentTypeFeeSchedule GetPaymentTypeFeeSchedule(PaymentTypeFeeSchedule paymentTypeFeeSchedules);
    }
}
