using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class PaymentTypeMedicareSequesterLogic : PaymentTypeBaseLogic
    {
        /// <summary>
        /// Initialize IPaymentTypeMedicareSequesterRepository repository
        /// </summary>
        private readonly IPaymentTypeMedicareSequesterRepository _paymentTypeMedicareSequesterRepository;

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        public override PaymentTypeBase PaymentTypeBase { get; set; }

        /// <summary>
        /// Gets the payment type medicare sequester.
        /// </summary>
        /// <value>
        /// The payment type medicare sequester.
        /// </value>
        private PaymentTypeMedicareSequester PaymentTypeMedicareSequester { get { return PaymentTypeBase as PaymentTypeMedicareSequester; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareSequesterLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeMedicareSequesterLogic(string connectionString)
        {
            _paymentTypeMedicareSequesterRepository = Factory.CreateInstance<IPaymentTypeMedicareSequesterRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareSequesterLogic"/> class.
        /// </summary>
        /// <param name="paymentTypeMedicareSequesterRepository">The payment type medicare sequester details repository.</param>
        public PaymentTypeMedicareSequesterLogic(IPaymentTypeMedicareSequesterRepository paymentTypeMedicareSequesterRepository)
        {
            if (paymentTypeMedicareSequesterRepository != null)
                _paymentTypeMedicareSequesterRepository = paymentTypeMedicareSequesterRepository;
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeMedicareSequesterRepository.AddEditPaymentTypeMedicareSequester((PaymentTypeMedicareSequester)paymentType);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeMedicareSequesterRepository.GetPaymentTypeMedicareSequester((PaymentTypeMedicareSequester)paymentType);
        }

        /// <summary>
        /// Evaluates the type of the payment.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        /// <returns></returns>
        public override List<PaymentResult> EvaluatePaymentType(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut, bool isContractFilter)
        {
            //Get Payment result for contract level
            PaymentResult paymentResult = paymentResults.First();
            if (paymentResult.AdjudicatedValue.HasValue && PaymentTypeMedicareSequester.Percentage.HasValue && claim.PatientResponsibility.HasValue)
            {
                double? allowedAmount = paymentResult.AdjudicatedValue - claim.PatientResponsibility;
                paymentResult.MedicareSequesterAmount = allowedAmount * (PaymentTypeMedicareSequester.Percentage / Constants.MedicareSequesterPercentage);
                paymentResult.AdjudicatedValue = allowedAmount - paymentResult.MedicareSequesterAmount + claim.PatientResponsibility;
            }
            return paymentResults;
        }
    }
}
