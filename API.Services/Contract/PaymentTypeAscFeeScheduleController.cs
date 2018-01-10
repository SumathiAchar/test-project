/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType ASC FeeSchedule functionalities

/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class PaymentTypeAscFeeScheduleController : BaseController
    {
        private readonly PaymentTypeAscFeeScheduleLogic _paymentTypeAscFeeScheduleLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeAscFeeScheduleController"/> class.
        /// </summary>
        public PaymentTypeAscFeeScheduleController()
        {
            //REVIEV-MAR16 - read "BubbleDataSource" from Constants.BubbleDataSource. Change other places also.
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _paymentTypeAscFeeScheduleLogic = new PaymentTypeAscFeeScheduleLogic(bubbleDataSource);
        }

        /// <summary>
        /// AddNew PaymentType ASC FeeSchedule
        /// </summary>
        /// <param name="paymentTypeAscFeeSchedule"></param>
        /// <returns></returns>
        [HttpPost]
        public long AddEditPaymentTypeAscFeeSchedule(PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule)
        {
            return _paymentTypeAscFeeScheduleLogic.AddEditPaymentType(paymentTypeAscFeeSchedule);
        }

        /// <summary>
        /// Get PaymentType ASC FeeSchedule
        /// </summary>
        /// <param name="paymentTypeAscFeeSchedule"></param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypeAscFeeSchedule GetPaymentTypeAscFeeSchedule(PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule)
        {
            return (PaymentTypeAscFeeSchedule)_paymentTypeAscFeeScheduleLogic.GetPaymentType(paymentTypeAscFeeSchedule);
        }

        /// <summary>
        /// Get TableName Selection from master Table
        /// </summary>
        /// <returns>List of TableNameSelection </returns>
        [HttpPost]
        public List<PaymentTypeTableSelection> GetTableNameSelection(PaymentTypeAscFeeSchedule paymenttype)
        {
            return _paymentTypeAscFeeScheduleLogic.GetTableNameSelection(paymenttype);
        }

        /// <summary>
        /// Gets the asc fee schedule options.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<AscFeeScheduleOption> GetAscFeeScheduleOptions()
        {
            return _paymentTypeAscFeeScheduleLogic.GetAscFeeScheduleOptions();
        }

    }
}
