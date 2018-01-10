/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Cap Controller functionalities

/************************************************************************************************************/

using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class PaymentTypeCapController : BaseController
    {
        /// <summary>
        /// The _payment type cap logic
        /// </summary>
        private readonly PaymentTypeCapLogic _paymentTypeCapLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeCapController"/> class.
        /// </summary>
        public PaymentTypeCapController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _paymentTypeCapLogic = new PaymentTypeCapLogic(bubbleDataSource);
        }

        /// <summary>
        /// AddNew PaymentType Cap
        /// </summary>
        /// <param name="paymentTypeCap"></param>
        /// <returns></returns>
        public long AddEditPaymentTypeCap(PaymentTypeCap paymentTypeCap)
        {
            return _paymentTypeCapLogic.AddEditPaymentType(paymentTypeCap);
        }

        /// <summary>
        /// Get PaymentType Cap
        /// </summary>
        /// <param name="paymentTypeCap"></param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypeBase GetPaymentTypeCap(PaymentTypeCap paymentTypeCap)
        {
            return _paymentTypeCapLogic.GetPaymentType(paymentTypeCap);
        }
    }
}
