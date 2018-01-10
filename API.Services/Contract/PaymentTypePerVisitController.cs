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
    public class PaymentTypePerVisitController : BaseController
    {
        /// <summary>
        /// The _payment type per visit details logic
        /// </summary>
        private readonly PaymentTypePerVisitLogic _paymentTypePerVisitDetailsLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePerVisitController"/> class.
        /// </summary>
        public PaymentTypePerVisitController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _paymentTypePerVisitDetailsLogic = new PaymentTypePerVisitLogic(bubbleDataSource);
        }

        /// <summary>
        /// AddNew PaymentType Per VisitDetails
        /// </summary>
        /// <param name="paymentTypePerVisit"></param>
        /// <returns></returns>
        [HttpPost]
        public long AddEditPaymentTypePerVisitDetails(PaymentTypePerVisit paymentTypePerVisit)
        {
            return _paymentTypePerVisitDetailsLogic.AddEditPaymentType(paymentTypePerVisit);
        }

        /// <summary>
        /// Get PaymentType Per VisitDetails
        /// </summary>
        /// <param name="paymentTypePerVisit"></param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypePerVisit GetPaymentTypePerVisitDetails(PaymentTypePerVisit paymentTypePerVisit)
        {
            return (PaymentTypePerVisit)_paymentTypePerVisitDetailsLogic.GetPaymentType(paymentTypePerVisit);
        }
    }
}
