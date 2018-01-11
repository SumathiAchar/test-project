/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Cap Logic Details functionalities

/************************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// Class PaymentTypeCapLogic.
    /// </summary>

    public class PaymentTypeCapLogic : PaymentTypeBaseLogic
    {
        /// <summary>
        /// The _payment type cap details repository
        /// </summary>
        private readonly IPaymentTypeCapRepository _paymentTypeCapDetailsRepository;

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        public override PaymentTypeBase PaymentTypeBase { get; set; }

        /// <summary>
        /// Gets the payment type cap.
        /// </summary>
        /// <value>
        /// The payment type cap.
        /// </value>
        private PaymentTypeCap PaymentTypeCap { get { return PaymentTypeBase as PaymentTypeCap; } }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeCapLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeCapLogic(string connectionString)
        {
            _paymentTypeCapDetailsRepository = Factory.CreateInstance<IPaymentTypeCapRepository>(connectionString, true);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeCapLogic"/> class.
        /// </summary>
        /// <param name="paymentTypeCapDetailsRepository">The payment type cap details repository.</param>
        public PaymentTypeCapLogic(IPaymentTypeCapRepository paymentTypeCapDetailsRepository)
        {
            if (paymentTypeCapDetailsRepository != null)
            {
                _paymentTypeCapDetailsRepository = paymentTypeCapDetailsRepository;
            }
        }


        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeCapDetailsRepository.AddEditPaymentTypeCapDetails((PaymentTypeCap)paymentType);
        }
        
        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeCapDetailsRepository.GetPaymentTypeCapDetails((PaymentTypeCap)paymentType);
        }

        /// <summary>
        /// Evaluates the type of the payment.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public override List<PaymentResult> EvaluatePaymentType(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut, bool isContractFilter)
        {
            paymentResults = isContractFilter
                ? ApplyContractFilter(paymentResults)
                : ApplyCap(paymentResults);

            return paymentResults;
        }

        /// <summary>
        /// Applies the cap.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        /// <returns></returns>
        private List<PaymentResult> ApplyCap(List<PaymentResult> paymentResults)
        {
            //Get All Payment result with current service type id
            List<PaymentResult> validPaymentResults =
                paymentResults.FindAll(currentPaymentResult => currentPaymentResult.ServiceTypeId == PaymentTypeCap.ServiceTypeId);

            double? adjudicatedValue = validPaymentResults.Sum(q => q.AdjudicatedValue);

            //Check Threshold condition
            if (adjudicatedValue != null && (PaymentTypeCap.Threshold.HasValue 
                                             && adjudicatedValue.Value > PaymentTypeCap.Threshold.Value))
            {
                //Set adjudicated value = 0  to each valid payment result 
                foreach (var paymentResult in validPaymentResults)
                    paymentResult.AdjudicatedValue = 0;

                //Update adjudicated value of First Payment result with Threshold
                validPaymentResults.First().AdjudicatedValue = PaymentTypeCap.Threshold.Value;
            }
            return paymentResults;
        }

        /// <summary>
        /// Applies the contract filter.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        /// <returns></returns>
        private List<PaymentResult> ApplyContractFilter(List<PaymentResult> paymentResults)
        {
            //Get OverAll Payment result
            PaymentResult paymentResult = paymentResults.First();

            //Check Threshold condition
            if (PaymentTypeCap.Threshold.HasValue && paymentResult.AdjudicatedValue.HasValue 
                    && paymentResult.AdjudicatedValue.Value > PaymentTypeCap.Threshold.Value)
            {
                paymentResult.AdjudicatedValue = PaymentTypeCap.Threshold.Value;
            }
            return paymentResults;
        }
    }
}
