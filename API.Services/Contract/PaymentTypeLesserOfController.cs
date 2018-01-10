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
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class PaymentTypeLesserOfController : BaseController
    {
        private readonly PaymentTypeLesserOfLogic _paymentTypeLesserOfLogic;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PaymentTypeLesserOfController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _paymentTypeLesserOfLogic = new PaymentTypeLesserOfLogic(bubbleDataSource);
        }


        /// <summary>
        /// Add/Edit Lessor Of payment type for a contract
        /// </summary>
        /// <param name="paymentTypeLesserOf">The payment type lesser of.</param>
        /// <returns>true/false</returns>
        [HttpPost]
        public long AddEditPaymentTypeLesserOf(PaymentTypeLesserOf paymentTypeLesserOf)
        {
            return _paymentTypeLesserOfLogic.AddEditPaymentType(paymentTypeLesserOf);
        }

        /// <summary>
        /// Gets the lesser of percentage.
        /// </summary>
        /// <param name="paymentTypeLesserOf">The payment type lesser of.</param>
        /// <returns>lesser of percentage value</returns>
        [HttpPost]
        public PaymentTypeLesserOf GetLesserOfPercentage(PaymentTypeLesserOf paymentTypeLesserOf)
        {
            return (PaymentTypeLesserOf)_paymentTypeLesserOfLogic.GetPaymentType(paymentTypeLesserOf);
        }
    }
}
