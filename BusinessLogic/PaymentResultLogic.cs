using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// Class for Payment Logic
    /// </summary>
    public class PaymentResultLogic : IPaymentResultLogic
    {
        /// <summary>
        /// The _payment result repository
        /// </summary>
        private readonly IPaymentResultRepository _paymentResultRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentResultLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentResultLogic(string connectionString)
        {
            _paymentResultRepository = Factory.CreateInstance<IPaymentResultRepository>(connectionString, true);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentResultLogic"/> class.
        /// </summary>
        /// <param name="paymentResultRepository">The payment result repository.</param>
        public PaymentResultLogic(IPaymentResultRepository paymentResultRepository)
        {
            if (paymentResultRepository != null)
                _paymentResultRepository = paymentResultRepository;
        }


        /// <summary>
        /// Determines whether the specified claim is match.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsMatch(IEvaluateableClaim claim)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Evaluates the specified claim.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<PaymentResult> Evaluate(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut, bool isContractFilter)
        {
            //Get claim level payment result
            PaymentResult paymentResult = paymentResults.FirstOrDefault(
                payment => payment.Line == null && payment.ServiceTypeId == null);

            //Update Overall Adjudicated Amount
            UpdateAdjudicatedAmount(paymentResult, paymentResults);

            //Update Claim Adjudicated Status
            UpdateClaimStatus(paymentResult, paymentResults);

            return paymentResults;
        }

        /// <summary>
        /// Updates the adjudicated amount.
        /// </summary>
        /// <param name="paymentResult">The payment result.</param>
        /// <param name="paymentResults">The payment result list.</param>
        private static void UpdateAdjudicatedAmount(PaymentResult paymentResult, IEnumerable<PaymentResult> paymentResults)
        {
            if (paymentResult != null)
            {
                List<PaymentResult> adjudicatedItems =
                    paymentResults.Where(payment => payment.AdjudicatedValue != null).ToList();

                if (adjudicatedItems.Any())
                    //Update claim level AdjudicatedValue with total of all AdjudicatedValues of valid payments.
                    paymentResult.AdjudicatedValue =
                        adjudicatedItems.Sum(p => p.AdjudicatedValue);
            }
        }

        /// <summary>
        /// Updates the claim status.
        /// </summary>
        /// <param name="paymentResult">The payment result.</param>
        /// <param name="paymentResults">The payment result list.</param>
        private static void UpdateClaimStatus(PaymentResult paymentResult, List<PaymentResult> paymentResults)
        {
            if (paymentResult != null)
            {
                // update overall adj status 
                if (paymentResults.All(cureentPaymentResult => cureentPaymentResult.ContractId == null))
                    paymentResult.ClaimStatus = (byte)
                        Enums.AdjudicationOrVarianceStatuses.AdjudicationErrorMissingContract;
                else if (paymentResults.All(
                    cureentPaymentResult => cureentPaymentResult.ServiceTypeId == null && cureentPaymentResult.ContractId.HasValue))
                    paymentResult.ClaimStatus = (byte)
                        Enums.AdjudicationOrVarianceStatuses.NoneOfLineItemsFindAMatchingServiceLine;
                else if (paymentResult.AdjudicatedValue == null &&
                         paymentResults.Any(
                             cureentPaymentResult =>
                                 cureentPaymentResult.ContractId.HasValue && cureentPaymentResult.ServiceTypeId.HasValue &&
                                 cureentPaymentResult.PaymentTypeId == null))
                    paymentResult.ClaimStatus =
                        (byte)
                            Enums.AdjudicationOrVarianceStatuses.AdjudicationErrorMissingPaymentType;
                else if (paymentResult.AdjudicatedValue.HasValue)
                    paymentResult.ClaimStatus =
                        (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
            }
        }

        /// <summary>
        /// Gets the initial payment result.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns>initial payment result list.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<PaymentResult> GetPaymentResults(EvaluateableClaim claim)
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>();

            if (claim != null)
            {
                //Initialize claim level payment result.
                paymentResults.Add(new PaymentResult
                {
                    ClaimId = claim.ClaimId,
                    ClaimTotalCharges = claim.ClaimTotal,
                    IsClaimChargeData = false,
                    ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.UnAdjudicated
                });

                //Initialize claim line level payment result.
                paymentResults.AddRange(claim.ClaimCharges.Select(claimCharge => new PaymentResult
                {
                    ClaimId = claim.ClaimId,
                    ClaimTotalCharges = claimCharge.Amount,
                    IsClaimChargeData = true,
                    Line = claimCharge.Line,
                    IsInitialEntry = true,
                    ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.UnAdjudicated,
                    ServiceLineCode = claimCharge.HcpcsCodeWithModifier
                }));
            }
            return paymentResults;
        }


        /// <summary>
        /// Updates the payment results.
        /// </summary>
        /// <param name="paymentResultDictionary">The payment result dictionary.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="adjudicatedClaims">The adjudicated claims.</param>
        /// <param name="earlyExitClaims">The early exit claims.</param>
        /// <returns></returns>
        public bool UpdatePaymentResults(Dictionary<long, List<PaymentResult>> paymentResultDictionary, int noOfRecords, long taskId, List<EvaluateableClaim> adjudicatedClaims, List<EvaluateableClaim> earlyExitClaims)
        {
            List<PaymentResult> paymentResults = null;
            if (paymentResultDictionary != null && paymentResultDictionary.Any())
            {
                paymentResults = new List<PaymentResult>();
                foreach (var claimResult in paymentResultDictionary)
                    paymentResults.AddRange(claimResult.Value.OrderBy(q => q.Line));
            }

            //Update Payment results in DB
            return _paymentResultRepository.UpdatePaymentResults(paymentResults, noOfRecords, taskId, adjudicatedClaims, earlyExitClaims);
        }

    }
}
