/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Medicare OP Details functionalities

/************************************************************************************************************/

using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class PaymentTypeMedicareOpController : BaseController
    {

        private readonly PaymentTypeMedicareOpLogic _medicareOpDetailsLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareOpController"/> class.
        /// </summary>
        public PaymentTypeMedicareOpController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _medicareOpDetailsLogic = new PaymentTypeMedicareOpLogic(bubbleDataSource);
        }

        /// <summary>
        /// AddNew PaymentType Medicare OP Details
        /// </summary>
        /// <param name="paymentTypeMedicareOpPayment"></param>
        /// <returns></returns>
        [HttpPost]
        public long AddEditPaymentTypeMedicareOpDetails(PaymentTypeMedicareOp paymentTypeMedicareOpPayment)
        {
            return _medicareOpDetailsLogic.AddEditPaymentType(paymentTypeMedicareOpPayment);
        }

        /// <summary>
        /// Get PaymentType Medicare OP Details
        /// </summary>
        /// <param name="paymentTypeMedicareOpPayment"></param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypeMedicareOp GetPaymentTypeMedicareOpDetails(PaymentTypeMedicareOp paymentTypeMedicareOpPayment)
        {
            return (PaymentTypeMedicareOp)_medicareOpDetailsLogic.GetPaymentType(paymentTypeMedicareOpPayment);
        }
    }
}
