/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Per Case Details functionalities

/************************************************************************************************************/

using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class PaymentTypePerCaseController : BaseController
    {

        private readonly PaymentTypePerCaseLogic _paymentTypePerCaseDetailsLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePerCaseController"/> class.
        /// </summary>
        public PaymentTypePerCaseController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _paymentTypePerCaseDetailsLogic = new PaymentTypePerCaseLogic(bubbleDataSource);
        }

        /// <summary>
        /// AddNew Payment Per Case Details
        /// </summary>
        /// <param name="paymentTypePerCase"></param>
        /// <returns></returns>
        [HttpPost]
        public long AddEditPaymentPerCaseDetails(PaymentTypePerCase paymentTypePerCase)
        {
            return _paymentTypePerCaseDetailsLogic.AddEditPaymentType(paymentTypePerCase);
        }

        /// <summary>
        /// Get Payment Per Case Details
        /// </summary>
        /// <param name="paymentTypePerCase"></param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypePerCase GetPaymentPerCaseDetails(PaymentTypePerCase paymentTypePerCase)
        {
            return (PaymentTypePerCase)_paymentTypePerCaseDetailsLogic.GetPaymentType(paymentTypePerCase);
        }
    }
}
