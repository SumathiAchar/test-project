/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles PaymentType StopLoss , it add PaymentType StopLoss information
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    /// <summary>
    /// Class PaymentTypeStopLossController.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class PaymentTypeStopLossController : BaseController
    {
        private readonly PaymentTypeStopLossLogic _paymentTypeStopLossLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeStopLossController"/> class.
        /// </summary>
        public PaymentTypeStopLossController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _paymentTypeStopLossLogic = new PaymentTypeStopLossLogic(bubbleDataSource);
        }

        /// <summary>
        /// Adds the edit payment type stop loss.
        /// </summary>
        /// <param name="paymentTypeStopLoss">The payment type stop loss.</param>
        /// <returns></returns>
        [HttpPost]
        public long AddEditPaymentTypeStopLoss(PaymentTypeStopLoss paymentTypeStopLoss)
        {
            return _paymentTypeStopLossLogic.AddEditPaymentType(paymentTypeStopLoss);
        }

        /// <summary>
        /// Gets the payment type stop loss.
        /// </summary>
        /// <param name="paymentTypeStopLoss">The payment type stop loss.</param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypeStopLoss GetPaymentTypeStopLoss(PaymentTypeStopLoss paymentTypeStopLoss)
        {
            return (PaymentTypeStopLoss)_paymentTypeStopLossLogic.GetPaymentType(paymentTypeStopLoss);
        }

        /// <summary>
        /// Gets the payment type stop loss conditions.
        /// </summary>
        /// <returns></returns>
        public List<StopLossCondition> GetPaymentTypeStopLossConditions()
        {
            return _paymentTypeStopLossLogic.GetConditions();
        }
    }
}
