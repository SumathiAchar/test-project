using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class PaymentTypeFeeScheduleController : BaseController
    {
        private readonly PaymentTypeFeeScheduleLogic _feeScheduleLogic;

        PaymentTypeFeeScheduleController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _feeScheduleLogic = new PaymentTypeFeeScheduleLogic(bubbleDataSource);
        }
        
        /// <summary>
        /// Adds the new payment type fee schedule.
        /// </summary>
        /// <param name="paymentTypeFeeSchedules">The payment type fee schedules.</param>
        /// <returns></returns>
        public long AddEditPaymentTypeFeeSchedule(PaymentTypeFeeSchedule paymentTypeFeeSchedules)
        {
            return _feeScheduleLogic.AddEditPaymentType(paymentTypeFeeSchedules);
        }

        /// <summary>
        /// Get the new payment type fee schedule.
        /// </summary>
        /// <param name="paymentTypeFeeSchedules">The payment type fee schedules.</param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypeFeeSchedule GetPaymentTypeFeeSchedule(PaymentTypeFeeSchedule paymentTypeFeeSchedules)
        {
            return (PaymentTypeFeeSchedule)_feeScheduleLogic.GetPaymentType(paymentTypeFeeSchedules);
        }

    }
}
