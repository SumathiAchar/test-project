/************************************************************************************************************/
/**  Author         : Raj
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Lessor of payment type for any contract
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTypeLesserOfRepository : IDisposable
    {
        /// <summary>
        /// Add/edit payment type lesser of.
        /// </summary>
        /// <param name="paymentTypeLesserOf">The payment type lesser of.</param>
        /// <returns>true/false</returns>
        long AddEditPaymentTypeLesserOf(PaymentTypeLesserOf paymentTypeLesserOf);
        
        /// <summary>
        /// Gets the payment type lesser of percentage.
        /// </summary>
        /// <returns>PaymentTypeLesserOf object</returns>
        PaymentTypeLesserOf GetLesserOfPercentage(PaymentTypeLesserOf paymentTypeLesserOf);

    }
}
