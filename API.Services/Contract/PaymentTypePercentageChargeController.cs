/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Discount Details Details functionalities

/************************************************************************************************************/

using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class PaymentTypePercentageChargeController : BaseController
    {
        private readonly PaymentTypePercentageChargeLogic _paymentTypePercentageDiscountDetailsLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePercentageChargeController"/> class.
        /// </summary>
        public PaymentTypePercentageChargeController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _paymentTypePercentageDiscountDetailsLogic = new PaymentTypePercentageChargeLogic(bubbleDataSource);

        }
        /// <summary>
        /// AddNew PaymentType Percentage Discount
        /// </summary>
        /// <param name="paymentTypePercentageDiscount"></param>
        /// <returns></returns>
        [HttpPost]
        public long AddEditPaymentTypePercentageDiscount(PaymentTypePercentageCharge paymentTypePercentageDiscount)
        {
            return _paymentTypePercentageDiscountDetailsLogic.AddEditPaymentType(paymentTypePercentageDiscount);
        }
        /// <summary>
        /// Get PaymentType Percentage Discount
        /// </summary>
        /// <param name="paymentTypePercentageDiscount"></param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypePercentageCharge GetPaymentTypePercentageDiscount(PaymentTypePercentageCharge paymentTypePercentageDiscount)
        {
            return (PaymentTypePercentageCharge)_paymentTypePercentageDiscountDetailsLogic.GetPaymentType(paymentTypePercentageDiscount);
        }
    }
}
