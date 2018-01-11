/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Percentage Discount Details functionalities

/************************************************************************************************************/

using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;
using System.Collections.Generic;

namespace SSI.ContractManagement.BusinessLogic
{
    public class PaymentTypePercentageChargeLogic : PaymentTypeBaseLogic
    {
        /// <summary>
        /// The _payment type percentage discount details repository
        /// </summary>
        private readonly IPaymentTypePercentageChargeRepository _paymentTypePercentageDiscountDetailsRepository;

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        public override PaymentTypeBase PaymentTypeBase { get; set; }

        /// <summary>
        /// Gets the payment type fee schedule.
        /// </summary>
        /// <value>
        /// The payment type fee schedule.
        /// </value>
        private PaymentTypePercentageCharge PaymentTypePercentageCharge { get { return PaymentTypeBase as PaymentTypePercentageCharge; } }


        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePercentageChargeLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypePercentageChargeLogic(string connectionString)
        {
            _paymentTypePercentageDiscountDetailsRepository = Factory.CreateInstance<IPaymentTypePercentageChargeRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePercentageChargeLogic"/> class.
        /// </summary>
        /// <param name="paymentTypePercentageDiscountDetailsRepository">The payment type percentage discount details repository.</param>
        public PaymentTypePercentageChargeLogic(IPaymentTypePercentageChargeRepository paymentTypePercentageDiscountDetailsRepository)
        {
            if (paymentTypePercentageDiscountDetailsRepository != null)
                _paymentTypePercentageDiscountDetailsRepository = paymentTypePercentageDiscountDetailsRepository;
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypePercentageDiscountDetailsRepository.AddEditPaymentTypePercentageDiscountDetails((PaymentTypePercentageCharge)paymentType);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypePercentageDiscountDetailsRepository.GetPaymentTypePercentageDiscountDetails((PaymentTypePercentageCharge)paymentType);
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
            paymentResults = (PaymentTypePercentageCharge.PayAtClaimLevel) ? EvaluateAtClaimLevel(claim, paymentResults, isCarveOut) : EvaluateAtLineLevel(claim, paymentResults, isCarveOut);
            return paymentResults;
        }

        /// <summary>
        /// Evaluates at claim level.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        private List<PaymentResult> EvaluateAtClaimLevel(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut)
        {
            if (paymentResults != null)
            {
                PaymentResult claimPaymentResult = GetClaimLevelPaymentResult(paymentResults, isCarveOut);
                if (claimPaymentResult != null)
                {
                    //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
                    Utilities.UpdatePaymentResult(claimPaymentResult, PaymentTypePercentageCharge.ServiceTypeId,
                        PaymentTypePercentageCharge.PaymentTypeDetailId, PaymentTypePercentageCharge.PaymentTypeId);

                    if (claim.ClaimTotal.HasValue)
                    {
                        claimPaymentResult.AdjudicatedValue = (PaymentTypePercentageCharge.Percentage/100)*
                                                              claim.ClaimTotal.Value;
                        claimPaymentResult.ClaimStatus =
                            (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                    }
                    else
                        claimPaymentResult.ClaimStatus =
                            (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;
                }
            }
            return paymentResults;
        }

        /// <summary>
        /// Evaluates at line level.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        private List<PaymentResult> EvaluateAtLineLevel(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut)
        {
            GetCarveOutPaymentResults(claim, paymentResults, isCarveOut);

            List<PaymentResult> validPaymentResults =
                   paymentResults.Where(
                       currentPaymentResult => currentPaymentResult.Line.HasValue &&
                                               PaymentTypePercentageCharge.ValidLineIds.Contains(
                                                   currentPaymentResult.Line.Value) &&
                                               currentPaymentResult.Line != null).ToList();
            List<ClaimCharge> validCharges = (from charge in claim.ClaimCharges from lineid in PaymentTypePercentageCharge.ValidLineIds where charge.Line == lineid select charge).ToList();

            foreach (var claimCharge in validCharges.TakeWhile(claimchg => isCarveOut || !paymentResults.Any(payment => payment.Line == claimchg.Line && payment.ServiceTypeId == PaymentTypeBase.ServiceTypeId)))
            {
                ////Logic to apply percentage
                PaymentResult paymentResult = validPaymentResults.FirstOrDefault(currentPaymentResult => currentPaymentResult.Line == claimCharge.Line && currentPaymentResult.AdjudicatedValue == null);
                if (paymentResult != null)
                {
                    //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
                    Utilities.UpdatePaymentResult(paymentResult, PaymentTypePercentageCharge.ServiceTypeId,
                        PaymentTypePercentageCharge.PaymentTypeDetailId, PaymentTypePercentageCharge.PaymentTypeId);

                    if (claimCharge.Amount.HasValue)
                    {
                        paymentResult.AdjudicatedValue = (PaymentTypePercentageCharge.Percentage/100)*
                                                         claimCharge.Amount.Value;
                        paymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                    }
                    else
                        paymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;
                }
            }
            return paymentResults;
        }
    }
}
