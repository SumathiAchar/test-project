/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType medicare lab fee schedule Details functionalities

/************************************************************************************************************/

using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class PaymentTypeMedicareLabFeeScheduleController : BaseController
    {
        private readonly PaymentTypeMedicareLabFeeScheduleLogic _medicareLabFeeScheduleDetailsLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareLabFeeScheduleController"/> class.
        /// </summary>
        public PaymentTypeMedicareLabFeeScheduleController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _medicareLabFeeScheduleDetailsLogic = new PaymentTypeMedicareLabFeeScheduleLogic(bubbleDataSource);

        }
        /// <summary>
        /// AddNew PaymentType medicare lab fee schedule Details
        /// </summary>
        /// <param name="paymentTypeMedicareLabFeeSchedulePayment"></param>
        /// <returns></returns>
        [HttpPost]
        public long AddEditPaymentTypeMedicareLabFeeScheduleDetails(PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedulePayment)
        {
            return _medicareLabFeeScheduleDetailsLogic.AddEditPaymentType(paymentTypeMedicareLabFeeSchedulePayment);
        }
        /// <summary>
        /// Get PaymentType medicare lab fee schedule Details
        /// </summary>
        /// <param name="paymentTypeMedicareLabFeeSchedulePayment"></param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypeMedicareLabFeeSchedule GetPaymentTypeMedicareLabFeeScheduleDetails(PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedulePayment)
        {
            return (PaymentTypeMedicareLabFeeSchedule)_medicareLabFeeScheduleDetailsLogic.GetPaymentType(paymentTypeMedicareLabFeeSchedulePayment);
        }
    }
}
