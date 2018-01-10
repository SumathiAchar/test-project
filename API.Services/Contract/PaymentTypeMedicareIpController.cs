/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Medicare IP Details functionalities

/************************************************************************************************************/

using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class PaymentTypeMedicareIpController : BaseController
    {
        private readonly PaymentTypeMedicareIpLogic _medicareIpDetailsLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareIpController"/> class.
        /// </summary>
        public PaymentTypeMedicareIpController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _medicareIpDetailsLogic = new PaymentTypeMedicareIpLogic(bubbleDataSource);
        }

        /// <summary>
        /// AddNew PaymentType Medicare IP Details
        /// </summary>
        /// <param name="paymentTypeMedicareIpPayment"></param>
        /// <returns></returns>
        [HttpPost]
        public long AddEditPaymentTypeMedicareIpDetails(PaymentTypeMedicareIp paymentTypeMedicareIpPayment)
        {
            return _medicareIpDetailsLogic.AddEditPaymentType(paymentTypeMedicareIpPayment);
        }

        /// <summary>
        /// Get PaymentType Medicare IP Details
        /// </summary>
        /// <param name="paymentTypeMedicareIpPayment"></param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypeMedicareIp GetPaymentTypeMedicareIpDetails(PaymentTypeMedicareIp paymentTypeMedicareIpPayment)
        {
            return (PaymentTypeMedicareIp)_medicareIpDetailsLogic.GetPaymentType(paymentTypeMedicareIpPayment);
        }
    }
}
