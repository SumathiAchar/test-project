/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType DRG Schedules Details Functionalities

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
    public class PaymentTypeDrgController : BaseController
    {
        private readonly PaymentTypeDrgLogic _paymentTypeDrgPaymentLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeDrgController"/> class.
        /// </summary>
        public PaymentTypeDrgController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _paymentTypeDrgPaymentLogic = new PaymentTypeDrgLogic(bubbleDataSource);
        }

        /// <summary>
        /// Add & Edit Payment Type DRG Schedules
        /// </summary>
        /// <param name="paymentTypeDrgPayment">PaymentTypeDRGPayment Object</param>
        /// <returns>Inserted Payment Id</returns>
        public long AddEditPaymentTypeDrgPayment(PaymentTypeDrg paymentTypeDrgPayment)
        {
            return _paymentTypeDrgPaymentLogic.AddEditPaymentType(paymentTypeDrgPayment);
        }

        /// <summary>
        /// Get Available Weight from master Table
        /// </summary>
        /// <returns>
        /// List of RelativeWeightList
        /// </returns>
        public List<RelativeWeight> GetAllRelativeWeightList()
        {
            return _paymentTypeDrgPaymentLogic.GetAllRelativeWeightList();
        }

        /// <summary>
        /// Get Payment Type DRG Schedules
        /// </summary>
        /// <param name="paymentTypeDrgPayment">PaymentTypeDRGPayment Object</param>
        /// <returns>Inserted Payment Id</returns>
        [HttpPost]
        public PaymentTypeDrg GetPaymentTypeDrgPayment(PaymentTypeDrg paymentTypeDrgPayment)
        {
            return (PaymentTypeDrg)_paymentTypeDrgPaymentLogic.GetPaymentType(paymentTypeDrgPayment);
        }
    }
}
