/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Per Case Details functionalities

/************************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class PaymentTypePerCaseLogic : PaymentTypeBaseLogic
    {
        /// <summary>
        /// Initialize IPaymentTypePerCaseRepository repository
        /// </summary>
        private readonly IPaymentTypePerCaseRepository _paymentTypePerCaseDetailsRepository;

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
        private PaymentTypePerCase PaymentTypePerCase { get { return PaymentTypeBase as PaymentTypePerCase; } }


        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePerCaseLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypePerCaseLogic(string connectionString)
        {
            _paymentTypePerCaseDetailsRepository = Factory.CreateInstance<IPaymentTypePerCaseRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePerCaseLogic"/> class.
        /// </summary>
        /// <param name="paymentTypePerCaseDetailsRepository">The payment type per case details repository.</param>
        public PaymentTypePerCaseLogic(IPaymentTypePerCaseRepository paymentTypePerCaseDetailsRepository)
        {
            if (paymentTypePerCaseDetailsRepository != null)
                _paymentTypePerCaseDetailsRepository = paymentTypePerCaseDetailsRepository;
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypePerCaseDetailsRepository.AddEditPaymentTypePerCase((PaymentTypePerCase)paymentType);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypePerCaseDetailsRepository.GetPaymentTypePerCase((PaymentTypePerCase)paymentType);
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
            paymentResults = (PaymentTypePerCase.PayAtClaimLevel) ? EvaluateAtClaimLevel(paymentResults, isCarveOut) : EvaluateAtLineLevel(claim, paymentResults, isCarveOut);
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
            if (paymentResults != null)
            {
                PaymentResult claimPaymentResult = GetClaimLevelPaymentResult(paymentResults, isCarveOut);
                if (claimPaymentResult != null)
                {
                    //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
                    Utilities.UpdatePaymentResult(claimPaymentResult, PaymentTypePerCase.ServiceTypeId,
                        PaymentTypePerCase.PaymentTypeDetailId, PaymentTypePerCase.PaymentTypeId);

                    claimPaymentResult.AdjudicatedValue = PaymentTypePerCase.Rate;
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
            if (PaymentTypePerCase.MaxCasesPerDay == null ||
                PaymentTypePerCase.MaxCasesPerDay.Value == 0)
                paymentResults = ApplyPerCaseWithoutMaxCases(claim, paymentResults, isCarveOut);
            else
                paymentResults = ApplyPerCaseWithMaxCases(claim, paymentResults, isCarveOut);
            return paymentResults;
        }

        /// <summary>
        /// Applies the per case with out maximum cases.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        private List<PaymentResult> ApplyPerCaseWithoutMaxCases(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut)
        {
            if (PaymentTypeBase.ValidLineIds != null && PaymentTypeBase.ValidLineIds.Count > 0)
            {
                foreach (ClaimCharge claimCharge in claim.ClaimCharges)
                {
                    //If carve out is there and line is already adjudicated with service type than break it
                    if (isCarveOut && paymentResults.Any(currentPaymentResult => currentPaymentResult.Line == claimCharge.Line && currentPaymentResult.ServiceTypeId == PaymentTypeBase.ServiceTypeId))
                        break;

                    if (PaymentTypeBase.ValidLineIds.Contains(claimCharge.Line) && paymentResults.Any(
                        currentPaymentResult => currentPaymentResult.Line == claimCharge.Line && (currentPaymentResult.AdjudicatedValue == null || isCarveOut)))
                    {
                        PaymentResult paymentResult = GetPaymentResult(paymentResults, isCarveOut, claimCharge.Line);

                        //Evaluate Payment result without Max Cases
                        EvaluateWithoutMaxCases(paymentResult, claimCharge.Units);
                    }
                }
            }
            return paymentResults;
        }


        /// <summary>
        /// Evaluates the without maximum cases.
        /// </summary>
        /// <param name="paymentResult">The payment result.</param>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        private void EvaluateWithoutMaxCases(PaymentResult paymentResult, int? unit)
        {
            if (paymentResult != null)
            {
                //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
                Utilities.UpdatePaymentResult(paymentResult, PaymentTypePerCase.ServiceTypeId,
                    PaymentTypePerCase.PaymentTypeDetailId, PaymentTypePerCase.PaymentTypeId);

                if (unit.HasValue)
                {
                    paymentResult.AdjudicatedValue = unit.Value * PaymentTypePerCase.Rate;
                    paymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                }
                else
                    paymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;
            }
        }

        /// <summary>
        /// Evaluates the with maximum cases.
        /// </summary>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="lineIds">The line ids.</param>
        /// <param name="unitSum">The unit sum.</param>
        /// <returns></returns>
        private List<PaymentResult> EvaluateWithMaxCases(List<PaymentResult> paymentResults, IEvaluateableClaim claim, List<int> lineIds, int? unitSum)
        {
            if (unitSum.HasValue && PaymentTypePerCase.MaxCasesPerDay.HasValue)
            {
                bool isMaxCases = PaymentTypePerCase.MaxCasesPerDay.Value < unitSum;
                bool isFirstLine = true;
                foreach (PaymentResult paymentResult in paymentResults.Where(
                    currentPaymentResult => currentPaymentResult.Line.HasValue &&
                                            lineIds.Contains(
                                                currentPaymentResult.Line.Value) &&
                                            currentPaymentResult.AdjudicatedValue == null).ToList())
                {
                    //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
                    Utilities.UpdatePaymentResult(paymentResult, PaymentTypePerCase.ServiceTypeId,
                        PaymentTypePerCase.PaymentTypeDetailId, PaymentTypePerCase.PaymentTypeId);

                    if (paymentResult.Line.HasValue)
                    {
                        if (isMaxCases)
                        {
                            if (isFirstLine)
                            {
                                isFirstLine = false;
                                paymentResult.AdjudicatedValue =
                                    PaymentTypePerCase.MaxCasesPerDay.Value *
                                    PaymentTypePerCase.Rate;
                            }
                            else
                                paymentResult.AdjudicatedValue = 0;
                            paymentResult.ClaimStatus =
                           (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                        }
                        else
                        {
                            int? unit =
                                // ReSharper disable once PossibleNullReferenceException
                                claim.ClaimCharges.FirstOrDefault(
                                    currentClaimCharge => currentClaimCharge.Line == paymentResult.Line)
                                    .Units;

                            //EvaluateWithoutMaxCases(paymentResult, unit);
                            if (unit.HasValue)
                            {
                                paymentResult.AdjudicatedValue = unit.Value * PaymentTypePerCase.Rate;
                                paymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                            }
                            else
                                paymentResult.ClaimStatus =
                                    (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;
                        }
                    }
                }
            }
            return paymentResults;
        }


        /// <summary>
        /// Applies the per case with maximum cases.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        private List<PaymentResult> ApplyPerCaseWithMaxCases(IEvaluateableClaim claim,
            List<PaymentResult> paymentResults, bool isCarveOut)
        {
            //Check whether payment type contains rev codee as selection
            bool isRevCodeAvailable = !string.IsNullOrEmpty(PaymentTypePerCase.RevCode);

            //Check whether payment type contains cpt code as selection
            bool isCptCodeAvailable = !string.IsNullOrEmpty(PaymentTypePerCase.HcpcsCode);

            //update payment result list
            paymentResults = UpdatePaymentResult(paymentResults, isCarveOut);

            //Select payment results with valid line selection and adjudication amount is null
            List<PaymentResult> validPaymentResults =
                paymentResults.Where(
                    currentPaymentResult => currentPaymentResult.Line.HasValue &&
                                            PaymentTypePerCase.ValidLineIds.Contains(currentPaymentResult.Line.Value) &&
                                            currentPaymentResult.AdjudicatedValue == null).ToList();

            foreach (int line in
                    validPaymentResults.Select(currentPaymentResult => (int) currentPaymentResult.Line).ToList())
            {
                ClaimCharge chargeData =
                        claim.ClaimCharges.FirstOrDefault(currentClaimCharge => currentClaimCharge.Line == line);

                if (paymentResults.Any(
                        currentPaymentResult =>
                            currentPaymentResult.Line.HasValue && currentPaymentResult.Line.Value == line &&
                            currentPaymentResult.AdjudicatedValue == null) && chargeData != null)
                {
                    //get current rev/cpt/thrudate selection
                    paymentResults = GetCurrentSelection(claim, paymentResults, isRevCodeAvailable, chargeData, isCptCodeAvailable, validPaymentResults);
                }
            }
            return paymentResults;
        }

        /// <summary>
        /// Gets the current sellection.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isRevCodeAvailable">if set to <c>true</c> [is rev code available].</param>
        /// <param name="chargeData">The charge data.</param>
        /// <param name="isCptCodeAvailable">if set to <c>true</c> [is CPT code available].</param>
        /// <param name="validPaymentResults">The valid payment results.</param>
        /// <returns></returns>
        private List<PaymentResult> GetCurrentSelection(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isRevCodeAvailable,
            ClaimCharge chargeData, bool isCptCodeAvailable, IEnumerable<PaymentResult> validPaymentResults)
        {
            List<ClaimCharge> claimCharges = claim.ClaimCharges.Where(
                currentClaimCharge =>
                    (!isRevCodeAvailable || chargeData.RevCode == currentClaimCharge.RevCode) &&
                    (!isCptCodeAvailable || chargeData.HcpcsCode == currentClaimCharge.HcpcsCode) &&
                    chargeData.ServiceFromDate == currentClaimCharge.ServiceFromDate &&
                    validPaymentResults.Select(currentPaymentType => currentPaymentType.Line)
                        .ToList()
                        .Contains(chargeData.Line)).ToList();

            //Get valid line ids based on current rev/cpt/thrudate selection
            List<int> lineItemWithCurrentSelection = claimCharges.Select(currentClaimCharge => currentClaimCharge.Line).ToList();

            //Get Sum of Unit based on current rev/cpt/thrudate selection
            int? unitSum = claimCharges.Sum(currentClaimCharge => currentClaimCharge.Units);

            if (lineItemWithCurrentSelection.Any())
            {
                paymentResults = EvaluateWithMaxCases(paymentResults, claim,
                    lineItemWithCurrentSelection, unitSum);
            }
            return paymentResults;
        }

        /// <summary>
        /// Updates the payment result.
        /// </summary>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        private List<PaymentResult> UpdatePaymentResult(List<PaymentResult> paymentResults, bool isCarveOut)
        {
            foreach (int currentLine in PaymentTypePerCase.ValidLineIds.TakeWhile(currentLine => !isCarveOut || !paymentResults.Any(
                currentPaymentResult => currentPaymentResult.Line == currentLine && currentPaymentResult.ServiceTypeId == PaymentTypeBase.ServiceTypeId)))
            {
                //Update Payment Result List (add new payment result if adjudication amount is null and carveout is there)
                UpdatePaymentResults(paymentResults, isCarveOut, currentLine);
            }
            return paymentResults;
        }
    }
}
