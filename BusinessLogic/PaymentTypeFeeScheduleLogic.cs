using System;
using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class PaymentTypeFeeScheduleLogic : PaymentTypeBaseLogic
    {
        /// <summary>
        /// Initializes Repository
        /// </summary>
        private readonly IPaymentTypeFeeScheduleRepository _paymentTypeFeeScheduleRepository;

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
        private PaymentTypeFeeSchedule PaymentTypeFeeSchedule { get { return PaymentTypeBase as PaymentTypeFeeSchedule; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeFeeScheduleLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeFeeScheduleLogic(string connectionString)
        {
            _paymentTypeFeeScheduleRepository = Factory.CreateInstance<IPaymentTypeFeeScheduleRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeFeeScheduleLogic"/> class.
        /// </summary>
        /// <param name="paymentTypeFeeScheduleRepository">The payment type fee schedule repository.</param>
        public PaymentTypeFeeScheduleLogic(IPaymentTypeFeeScheduleRepository paymentTypeFeeScheduleRepository)
        {
            if (paymentTypeFeeScheduleRepository != null)
                _paymentTypeFeeScheduleRepository = paymentTypeFeeScheduleRepository;
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeFeeScheduleRepository.AddEditPaymentTypeFeeSchedule((PaymentTypeFeeSchedule)paymentType);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeFeeScheduleRepository.GetPaymentTypeFeeSchedule((PaymentTypeFeeSchedule)paymentType);
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
            if (claim != null)
                foreach (ClaimCharge claimCharge in claim.ClaimCharges.Where(code => (!string.IsNullOrWhiteSpace(code.HcpcsCodeWithModifier.Trim()))))
                {
                    if (isCarveOut && paymentResults.Any(currentPaymentResult => currentPaymentResult.Line == claimCharge.Line && currentPaymentResult.ServiceTypeId == PaymentTypeBase.ServiceTypeId))
                        break;

                    if (PaymentTypeBase.ValidLineIds.Contains(claimCharge.Line) && paymentResults.Any(
                        currentPaymentResult => currentPaymentResult.Line == claimCharge.Line && (currentPaymentResult.AdjudicatedValue == null || isCarveOut)))
                    {
                        PaymentResult paymentResult = GetPaymentResult(paymentResults, isCarveOut, claimCharge.Line);

                        if (paymentResult != null)
                            EvaluateLine(paymentResult, claimCharge);
                    }
                }
            return paymentResults;
        }

        /// <summary>
        /// Evaluates the type of the payment.
        /// </summary>
        /// <param name="paymentResult">The payment result.</param>
        /// <param name="claimCharge">The claim charge.</param>
        /// <returns></returns>
        private void EvaluateLine(PaymentResult paymentResult, ClaimCharge claimCharge)
        {
            //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
            Utilities.UpdatePaymentResult(paymentResult, PaymentTypeFeeSchedule.ServiceTypeId,
                PaymentTypeFeeSchedule.PaymentTypeDetailId, PaymentTypeFeeSchedule.PaymentTypeId);

            if (PaymentTypeFeeSchedule.ClaimFieldDoc != null &&
                PaymentTypeFeeSchedule.ClaimFieldDoc.ClaimFieldValues != null &&
                PaymentTypeFeeSchedule.ClaimFieldDoc.ClaimFieldValues.Count > 0)
            {
                string hcpcsCode = claimCharge.HcpcsCodeWithModifier.ToUpper();
               ClaimFieldValue claimFieldValue =
                    PaymentTypeFeeSchedule.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(
                        currentClaimFieldValue =>
                            (currentClaimFieldValue.Identifier.ToUpper() == hcpcsCode));
                   if (claimFieldValue == null && hcpcsCode.Trim().Length!=5)
                       claimFieldValue = PaymentTypeFeeSchedule.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(
                        currentClaimFieldValue =>
                            (currentClaimFieldValue.Identifier.ToUpper() == hcpcsCode.Substring(0,5)));
                if (claimFieldValue != null)
                {
                    ApplyFeeSchedule(paymentResult, claimFieldValue, claimCharge);
                }
                else
                {
                    ApplyNonFeeSchedule(paymentResult, claimCharge);
                }

            }

        }


        /// <summary>
        /// Applies the fee schedule.
        /// </summary>
        /// <param name="paymentResult">The payment result.</param>
        /// <param name="claimFieldValue">The claim field value.</param>
        /// <param name="claimCharge">The claim charge.</param>
        private void ApplyFeeSchedule(PaymentResult paymentResult, ClaimFieldValue claimFieldValue, ClaimCharge claimCharge)
        {
            double amount;
            if (!string.IsNullOrEmpty(claimFieldValue.Value) && Double.TryParse(claimFieldValue.Value, out amount))
            {
                if (PaymentTypeFeeSchedule.FeeSchedule.HasValue)
                {
                    double percentageAmount = (PaymentTypeFeeSchedule.FeeSchedule.Value /
                                                100) * amount;
                    paymentResult.AdjudicatedValue = (PaymentTypeFeeSchedule.IsObserveUnits) ?
                        percentageAmount * claimCharge.Units : percentageAmount;
                    paymentResult.ClaimStatus =
                        (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                }
                else
                    paymentResult.ClaimStatus =
                        (byte)Enums.AdjudicationOrVarianceStatuses
                            .AdjudicationErrorInvalidPaymentData;
            }
            else
                paymentResult.ClaimStatus =
                    (byte)Enums.AdjudicationOrVarianceStatuses
                            .AdjudicationErrorInvalidPaymentData;


        }


        /// <summary>
        /// Applies the non fee schedule.
        /// </summary>
        /// <param name="paymentResult">The payment result.</param>
        /// <param name="claimCharge">The claim charge.</param>
        /// <returns></returns>
        private void ApplyNonFeeSchedule(PaymentResult paymentResult, ClaimCharge claimCharge)
        {
            if (PaymentTypeFeeSchedule.NonFeeSchedule.HasValue)
            {
                if (claimCharge.Amount.HasValue)
                {
                    paymentResult.AdjudicatedValue =
                        (PaymentTypeFeeSchedule.NonFeeSchedule.Value / 100) *
                        claimCharge.Amount.Value;
                    paymentResult.ClaimStatus =
                        (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                }
                else
                    paymentResult.ClaimStatus =
                        (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;
            }
            else
                paymentResult.ClaimStatus =
                    (byte)Enums.AdjudicationOrVarianceStatuses
                            .AdjudicationErrorInvalidPaymentData;

        }
    }
}
