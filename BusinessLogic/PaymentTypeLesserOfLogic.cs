using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class PaymentTypeLesserOfLogic : PaymentTypeBaseLogic
    {
        /// <summary>
        /// Initializes Repository
        /// </summary>
        private readonly IPaymentTypeLesserOfRepository _paymentTypeLesserOfRepository;

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
        private PaymentTypeLesserOf PaymentTypeLesserOf { get { return PaymentTypeBase as PaymentTypeLesserOf; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeLesserOfLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeLesserOfLogic(string connectionString)
        {
            _paymentTypeLesserOfRepository = Factory.CreateInstance<IPaymentTypeLesserOfRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeLesserOfLogic"/> class.
        /// </summary>
        /// <param name="paymentTypeLesserOfRepository">The payment type lesser of repository.</param>
        public PaymentTypeLesserOfLogic(IPaymentTypeLesserOfRepository paymentTypeLesserOfRepository)
        {
            if (paymentTypeLesserOfRepository != null)
                _paymentTypeLesserOfRepository = paymentTypeLesserOfRepository;
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeLesserOfRepository.AddEditPaymentTypeLesserOf((PaymentTypeLesserOf)paymentType);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeLesserOfRepository.GetLesserOfPercentage((PaymentTypeLesserOf)paymentType);
        }

        /// <summary>
        /// Evaluates the type of the payment.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public override List<PaymentResult> EvaluatePaymentType(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut, bool isContractFilter)
        {
            paymentResults = isContractFilter ? ApplyContractFilter(claim, paymentResults) : ApplyLesserOf(paymentResults);
            return paymentResults;
        }

        /// <summary>
        /// Evaluates at claim level.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <returns></returns>//contractfilter
        private List<PaymentResult> ApplyContractFilter(IEvaluateableClaim claim, List<PaymentResult> paymentResults)
        {
            //Get OverAll Payment result
            PaymentResult paymentResult = paymentResults.First();

            if (claim.ClaimTotal.HasValue && paymentResult.AdjudicatedValue.HasValue && PaymentTypeLesserOf.Percentage.HasValue)
            {
                paymentResult.AdjudicatedValue = Convert.ToBoolean(PaymentTypeLesserOf.IsLesserOf, CultureInfo.InvariantCulture)
                    ? Math.Min((claim.ClaimTotal.Value*PaymentTypeLesserOf.Percentage.Value)/100,
                        paymentResult.AdjudicatedValue.Value)
                    : Math.Max((claim.ClaimTotal.Value*PaymentTypeLesserOf.Percentage.Value)/100,
                        paymentResult.AdjudicatedValue.Value);
            }

            return paymentResults;
        }

        /// <summary>
        /// Evaluates at line level.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        /// <returns></returns>
        private List<PaymentResult> ApplyLesserOf(List<PaymentResult> paymentResults)
        {
            //Get valid line result based on ServiceTYpeId
            List<PaymentResult> validPaymentResults =
            paymentResults.FindAll(currentPaymentResult => currentPaymentResult.ServiceTypeId == PaymentTypeLesserOf.ServiceTypeId && currentPaymentResult.AdjudicatedValue.HasValue);

            //Total Adjudicated amount
            double? adjudicatedValue = validPaymentResults.Sum(currentPaymentResult => currentPaymentResult.AdjudicatedValue);

            //Get overall claim level payment result to identify Whether adjudication happen at claim level or line level
            PaymentResult paymentClaimResult = validPaymentResults.FirstOrDefault(
                paymentResult =>
                    paymentResult.Line == null &&
                    paymentResult.ServiceTypeId == PaymentTypeLesserOf.ServiceTypeId);

            //Get claim total charge based on adjudicated items(Whether claim level or line level)  
            double? claimTotalCharge = paymentClaimResult != null ? paymentClaimResult.ClaimTotalCharges : validPaymentResults.Sum(currentPaymentResult => currentPaymentResult.ClaimTotalCharges);
            
            //Get Min value from calculated lesser of and adjudicated value
            if (PaymentTypeLesserOf.Percentage != null && claimTotalCharge.HasValue && adjudicatedValue != null && validPaymentResults.Any())
            {     
                //Set adjudicated value = 0  to each valid payment result 
                foreach (PaymentResult paymentResult in validPaymentResults)
                    paymentResult.AdjudicatedValue = 0;

                //Set Adjudicated amount to first payment result
                validPaymentResults.First().AdjudicatedValue = Convert.ToBoolean(PaymentTypeLesserOf.IsLesserOf, CultureInfo.InvariantCulture)
                    ? Math.Min((claimTotalCharge.Value*PaymentTypeLesserOf.Percentage.Value)/100,
                        adjudicatedValue.Value)
                    : Math.Max((claimTotalCharge.Value*PaymentTypeLesserOf.Percentage.Value)/100,
                        adjudicatedValue.Value);

                //Change claim status as Adjudicated
                validPaymentResults.First().ClaimStatus = (int)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
            }
            return paymentResults;
        }

    }
}
