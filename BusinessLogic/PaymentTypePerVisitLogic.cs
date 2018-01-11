/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType PerVisit Details functionalities

/************************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class PaymentTypePerVisitLogic : PaymentTypeBaseLogic
    {
        /// <summary>
        /// The _payment type per visit details repository
        /// </summary>
        private readonly IPaymentTypePerVisitRepository _paymentTypePerVisitDetailsRepository;

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
        private PaymentTypePerVisit PaymentTypePerVisit { get { return PaymentTypeBase as PaymentTypePerVisit; } }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePerVisitLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypePerVisitLogic(string connectionString)
        {
            _paymentTypePerVisitDetailsRepository = Factory.CreateInstance<IPaymentTypePerVisitRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePerVisitLogic"/> class.
        /// </summary>
        /// <param name="paymentTypePerVisitDetailsRepository">The payment type per visit details repository.</param>
        public PaymentTypePerVisitLogic(IPaymentTypePerVisitRepository paymentTypePerVisitDetailsRepository)
        {
            if (paymentTypePerVisitDetailsRepository != null)
                _paymentTypePerVisitDetailsRepository = paymentTypePerVisitDetailsRepository;
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypePerVisitDetailsRepository.AddEditPaymentTypePerVisitDetails((PaymentTypePerVisit)paymentType);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypePerVisitDetailsRepository.GetPaymentTypePerVisitDetails((PaymentTypePerVisit)paymentType);
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
            paymentResults = (PaymentTypePerVisit.PayAtClaimLevel) ? EvaluateAtClaimLevel(paymentResults, isCarveOut) : EvaluateAtLineLevel(claim, paymentResults, isCarveOut);
            return paymentResults;
        }

        /// <summary>
        /// Evaluates at claim level.
        /// </summary>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        private List<PaymentResult> EvaluateAtClaimLevel(List<PaymentResult> paymentResults, bool isCarveOut)
        {
            if (paymentResults != null )
            {
                PaymentResult claimPaymentResult = GetClaimLevelPaymentResult(paymentResults, isCarveOut);
                if (claimPaymentResult != null)
                {
                    //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
                    Utilities.UpdatePaymentResult(claimPaymentResult, PaymentTypePerVisit.ServiceTypeId,
                        PaymentTypePerVisit.PaymentTypeDetailId, PaymentTypePerVisit.PaymentTypeId);

                    claimPaymentResult.AdjudicatedValue = PaymentTypePerVisit.Rate;
                    claimPaymentResult.ClaimStatus =
                              (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
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
                                               PaymentTypePerVisit.ValidLineIds.Contains(
                                                   currentPaymentResult.Line.Value) &&
                                               currentPaymentResult.Line != null).ToList();
            List<ClaimCharge> validCharges = (from charge in claim.ClaimCharges from lineid in PaymentTypePerVisit.ValidLineIds where charge.Line == lineid select charge).ToList();
            GetGroupedChargesPaymentResult(paymentResults, isCarveOut, validCharges, validPaymentResults);
            return paymentResults;
        }

        /// <summary>
        /// Gets the grouped charges payment result.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="validCharges">The valid charges.</param>
        /// <param name="validPaymentResults">The valid payment results.</param>
        private void GetGroupedChargesPaymentResult(List<PaymentResult> paymentResults, bool isCarveOut, IEnumerable<ClaimCharge> validCharges,
            List<PaymentResult> validPaymentResults)
        {
            var groupedClaimCharges =
                validCharges.GroupBy(claimCharge => new
                {
                    claimCharge.ServiceFromDate,
                    claimCharge.RevCode
                }
                    )
                    .Select(groupedClaimCharge => new {GroupName = groupedClaimCharge.Key, Members = groupedClaimCharge});
            foreach (var claimchgs in groupedClaimCharges)
            {
                int first = 1;
                foreach (
                    var claimCharge in
                        claimchgs.Members.TakeWhile(
                            claimCharge => !isCarveOut || !paymentResults.Any(payment => payment.Line == claimCharge.Line
                                                                                         &&
                                                                                         payment.ServiceTypeId ==
                                                                                         PaymentTypeBase.ServiceTypeId)))
                {
                    ////Logic to apply stop loss
                    if (PaymentTypePerVisit.Rate != null)
                    {
                        first = GetValidPaymentResult(validPaymentResults, claimCharge, first);
                    }
                    else
                    {
                        PaymentResult paymentResult =
                            validPaymentResults.FirstOrDefault(
                                currentPaymentResult =>
                                    currentPaymentResult.Line == claimCharge.Line &&
                                    currentPaymentResult.AdjudicatedValue == null && !currentPaymentResult.IsInitialEntry);
                        if (paymentResult != null)
                            paymentResults.Remove(paymentResult);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the valid payment result.
        /// </summary>
        /// <param name="validPaymentResults">The valid payment results.</param>
        /// <param name="claimCharge">The claim charge.</param>
        /// <param name="first">The first.</param>
        /// <returns></returns>
        private int GetValidPaymentResult(IEnumerable<PaymentResult> validPaymentResults, ClaimCharge claimCharge, int first)
        {
            PaymentResult paymentResult =
                validPaymentResults.FirstOrDefault(
                    currentPaymentResult =>
                        currentPaymentResult.Line == claimCharge.Line && currentPaymentResult.AdjudicatedValue == null);

            if (paymentResult != null)
            {
                paymentResult.AdjudicatedValue = first == 1 ? PaymentTypePerVisit.Rate : 0.0;
                first++;
                paymentResult.ClaimStatus =
                    (byte) Enums.AdjudicationOrVarianceStatuses.Adjudicated;

                paymentResult.PaymentTypeId =
                    (byte) Enums.PaymentTypeCodes.PerVisit;

                paymentResult.ServiceTypeId = PaymentTypePerVisit.ServiceTypeId;
            }
            return first;
        }
    }
}
