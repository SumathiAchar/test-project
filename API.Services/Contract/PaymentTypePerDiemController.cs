/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Per Diem Details functionalities

/************************************************************************************************************/

using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class PaymentTypePerDiemController : BaseController
    {
        /// <summary>
        /// The _payment type per diem details logic
        /// </summary>
        private readonly PaymentTypePerDiemLogic _paymentTypePerDiemDetailsLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePerDiemController"/> class.
        /// </summary>
        public PaymentTypePerDiemController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _paymentTypePerDiemDetailsLogic = new PaymentTypePerDiemLogic(bubbleDataSource);
        }

        /// <summary>
        /// Adds the new payment type per diem.
        /// </summary>
        /// <param name="paymentTypePerDiemList">The payment type per diem list.</param>
        /// <returns></returns>
        [HttpPost]
        public long AddEditPaymentTypePerDiem(PaymentTypePerDiem paymentTypePerDiemList)
        {
            return _paymentTypePerDiemDetailsLogic.AddEditPaymentType(paymentTypePerDiemList);
        }

        /// <summary>
        /// Get PaymentType Per Diem
        /// </summary>
        /// <param name="paymentTypePerDiem"></param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypePerDiem GetPaymentTypePerDiem(PaymentTypePerDiem paymentTypePerDiem)
        {
            return (PaymentTypePerDiem)_paymentTypePerDiemDetailsLogic.GetPaymentType(paymentTypePerDiem);
        }
    }
}
