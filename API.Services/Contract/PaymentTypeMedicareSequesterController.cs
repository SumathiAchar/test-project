using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    public class PaymentTypeMedicareSequesterController : BaseController
    {
        private readonly PaymentTypeMedicareSequesterLogic _paymentTypeMedicareSequesterLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareSequesterController"/> class.
        /// </summary>
        public PaymentTypeMedicareSequesterController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId);
            _paymentTypeMedicareSequesterLogic = new PaymentTypeMedicareSequesterLogic(bubbleDataSource);
        }

        /// <summary>
        /// AddNew Payment Medicare Sequester
        /// </summary>
        /// <param name="paymentMedicareSequester"></param>
        /// <returns></returns>
        [HttpPost]
        public long AddEditPaymentMedicareSequester(PaymentTypeMedicareSequester paymentMedicareSequester)
        {
            return _paymentTypeMedicareSequesterLogic.AddEditPaymentType(paymentMedicareSequester);
        }

        /// <summary>
        /// Get Payment Medicare Sequester
        /// </summary>
        /// <param name="paymentMedicareSequester"></param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypeMedicareSequester GetPaymentMedicareSequester(PaymentTypeMedicareSequester paymentMedicareSequester)
        {
            return (PaymentTypeMedicareSequester)_paymentTypeMedicareSequesterLogic.GetPaymentType(paymentMedicareSequester);
        }
    }
}
