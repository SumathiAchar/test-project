using System;
using System.Collections.Generic;
/************************************************************************************************************/
/**  Author         : Ragini Bhandari // WORK IN PROGRESS //
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType StopLoss Logic Details functionalities

/************************************************************************************************************/
using System.Globalization;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class PaymentTypeStopLossLogic : PaymentTypeBaseLogic
    {
        /// <summary>
        /// The _payment type stop loss repository
        /// </summary>
        private readonly IPaymentTypeStopLossRepository _paymentTypeStopLossRepository;

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
        private PaymentTypeStopLoss PaymentTypeStopLoss
        {
            get { return PaymentTypeBase as PaymentTypeStopLoss; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeStopLossLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeStopLossLogic(string connectionString)
        {
            _paymentTypeStopLossRepository = Factory.CreateInstance<IPaymentTypeStopLossRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeStopLossLogic"/> class.
        /// </summary>
        /// <param name="paymentTypeStopLossRepository">The payment type stop loss repository.</param>
        public PaymentTypeStopLossLogic(IPaymentTypeStopLossRepository paymentTypeStopLossRepository)
        {
            if (paymentTypeStopLossRepository != null)
                _paymentTypeStopLossRepository = paymentTypeStopLossRepository;
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeStopLossRepository.AddEditPaymentTypeStopLoss((PaymentTypeStopLoss)paymentType);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeStopLossRepository.GetPaymentTypeStopLoss((PaymentTypeStopLoss)paymentType);
        }

        /// <summary>
        /// Get PaymentType StopLoss Conditions
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<StopLossCondition> GetConditions()
        {
            return _paymentTypeStopLossRepository.GetPaymentTypeStopLossConditions();
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
        public override List<PaymentResult> EvaluatePaymentType(IEvaluateableClaim claim,
            List<PaymentResult> paymentResults, bool isCarveOut, bool isContractFilter)
        {
            UpdateThresholdAndCaa(claim, paymentResults, isContractFilter);

            if (isContractFilter)
            {
                ApplyContractFilter(claim, paymentResults);
            }
            else if (PaymentTypeBase.ValidLineIds != null && PaymentTypeBase.ValidLineIds.Count > 0)
            {
                switch ((Enums.StopLossCondition)PaymentTypeStopLoss.StopLossConditionId)
                {
                    case Enums.StopLossCondition.TotalChargeLines:
                        ApplyTotalChargeLines(claim, paymentResults, isCarveOut,
                            string.IsNullOrEmpty(PaymentTypeStopLoss.RevCode) &&
                                                              string.IsNullOrEmpty(PaymentTypeStopLoss.HcpcsCode));
                        break;
                    case Enums.StopLossCondition.PerChargeLine:
                        ApplyPerChargeLine(claim, paymentResults, isCarveOut);
                        break;
                    case Enums.StopLossCondition.PerDayofStay:
                        ApplyPerDay(claim, paymentResults, isCarveOut);
                        break;
                }
            }
            return paymentResults;
        }

        /// <summary>
        /// Updates the threshold and caa.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        private void UpdateThresholdAndCaa(IEvaluateableClaim claim, IEnumerable<PaymentResult> paymentResults, bool isContractFilter)
        {
            if (isContractFilter)
            {
                var overAllPaymentResult = paymentResults.FirstOrDefault();
                if (overAllPaymentResult != null)
                    claim.SmartBox.CAA = overAllPaymentResult.AdjudicatedValue;
            }
            else
                claim.SmartBox.CAA = Utilities.CalculateAllowedAmount(paymentResults);

            PaymentTypeStopLoss.Threshold = Utilities.EvaluateExpression(PaymentTypeStopLoss.Expression != null
                ? PaymentTypeStopLoss.Expression.ToUpper(CultureInfo.InvariantCulture)
                : string.Empty, claim, PaymentTypeStopLoss);

        }

        /// <summary>
        /// Apply Stop loss at Total Charge Lines
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="paymentResults"></param>
        /// <param name="isCarveOut"></param>
        /// <param name="isClaimLevel"></param>
        /// <returns></returns>
        private void ApplyTotalChargeLines(IEvaluateableClaim claim,
            List<PaymentResult> paymentResults, bool isCarveOut, bool isClaimLevel)
        {
            if (isClaimLevel)
            {
                EvaluateTotalChargeAtClaimLevel(claim, paymentResults, isCarveOut);
            }
            else
            {
                EvaluateTotalChargeAtLineLevel(claim, paymentResults, isCarveOut);
            }
        }

        /// <summary>
        /// Evaluate the total charge at claim level.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        private void EvaluateTotalChargeAtClaimLevel(IEvaluateableClaim claim,
            List<PaymentResult> paymentResults, bool isCarveOut)
        {
            PaymentResult claimPaymentResult = paymentResults.Any(
                result => result.ServiceTypeId == PaymentTypeStopLoss.ServiceTypeId && result.Line == null)
                ? paymentResults.FirstOrDefault(
                    result => result.ServiceTypeId == PaymentTypeStopLoss.ServiceTypeId && result.Line == null)
                : GetClaimLevelPaymentResult(paymentResults, isCarveOut);

            if (claimPaymentResult != null)
            {
                if (claim.ClaimTotal.HasValue)
                {
                    claimPaymentResult = GetTotalLinesValue(claimPaymentResult, claim.ClaimTotal, claim);
                    Utilities.UpdateStopLossDetails(claimPaymentResult, claim.SmartBox, PaymentTypeStopLoss, PaymentTypeStopLoss.IsFormulaError);

                    //Remove PaymentResult if its not satisfied thresold condition 
                    if (claimPaymentResult.AdjudicatedValue == null &&
                        claimPaymentResult.ClaimStatus == (byte)Enums.AdjudicationOrVarianceStatuses.UnAdjudicated)
                        paymentResults.Remove(claimPaymentResult);

                }
                claimPaymentResult.ServiceTypeId = PaymentTypeStopLoss.ServiceTypeId;
            }
        }

        /// <summary>
        /// Evaluates the total charge at line level.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        private void EvaluateTotalChargeAtLineLevel(IEvaluateableClaim claim, List<PaymentResult> paymentResults,
            bool isCarveOut)
        {
            if (PaymentTypeBase.ValidLineIds != null && PaymentTypeBase.ValidLineIds.Count > 0 &&
                PaymentTypeStopLoss.Threshold.HasValue &&
                PaymentTypeStopLoss.Percentage.HasValue)
            {
                List<int> adjudicatedLines = new List<int>();
                foreach (var lineId in PaymentTypeStopLoss.ValidLineIds)
                {
                    if (!adjudicatedLines.Contains(lineId))
                    {
                        ClaimCharge claimCharge =
                            claim.ClaimCharges.FirstOrDefault(
                                currentClaimCharge => currentClaimCharge.Line == lineId);
                        if (claimCharge != null &&
                            (paymentResults.Any(
                                currentPaymentResult =>
                                    currentPaymentResult.Line == claimCharge.Line &&
                                    (currentPaymentResult.AdjudicatedValue == null || isCarveOut ||
                                     currentPaymentResult.ServiceTypeId == PaymentTypeStopLoss.ServiceTypeId))))
                        {
                            List<ClaimCharge> claimCharges = claim.ClaimCharges.Where(currentClaimCharge =>
                                (string.IsNullOrEmpty(PaymentTypeStopLoss.RevCode) ||
                                 claimCharge.RevCode == currentClaimCharge.RevCode) &&
                                (string.IsNullOrEmpty(PaymentTypeStopLoss.HcpcsCode) ||
                                 (claimCharge.HcpcsCode.Trim().ToUpper()) ==
                                  (currentClaimCharge.HcpcsCode.Trim().ToUpper()) &&
                                PaymentTypeStopLoss.ValidLineIds.Contains(currentClaimCharge.Line))).ToList();

                            if (claimCharges.Any())
                            {
                                //Get Sum of Unit based on current rev/cpt selection
                                CalculateSum(claim, paymentResults, isCarveOut, claimCharges, lineId, adjudicatedLines);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the sum.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="claimCharges">The claim charges.</param>
        /// <param name="lineId">The line identifier.</param>
        /// <param name="adjudicatedLines">The adjudicated lines.</param>
        private void CalculateSum(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut, List<ClaimCharge> claimCharges, int lineId,
            List<int> adjudicatedLines)
        {
            double? totalCharge = claimCharges.Sum(q => q.Amount);

            if (PaymentTypeStopLoss.Threshold != null && totalCharge > PaymentTypeStopLoss.Threshold.Value)
            {
                foreach (var currentClaimCharge in claimCharges)
                {
                    ApplyTotalChargeAtLineLevel(paymentResults, currentClaimCharge, isCarveOut,
                        lineId, totalCharge.Value, claim);
                    adjudicatedLines.Add(currentClaimCharge.Line);
                }
            }
            else
                adjudicatedLines.AddRange(
                    claimCharges.Select(q => q.Line).ToList());
        }

        /// <summary>
        /// Applies the total charge at line level.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="currentClaimCharge">The current claim charge.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="lineId">The line identifier.</param>
        /// <param name="totalCharge">The total charge.</param>
        /// <param name="claim"></param>
        private void ApplyTotalChargeAtLineLevel(List<PaymentResult> paymentResults, ClaimCharge currentClaimCharge, bool isCarveOut, int lineId, double totalCharge, IEvaluateableClaim claim)
        {
            PaymentResult paymentResult = paymentResults.Any(
                result =>
                    result.ServiceTypeId == PaymentTypeStopLoss.ServiceTypeId &&
                    result.Line == currentClaimCharge.Line)
                ? paymentResults.FirstOrDefault(
                    result =>
                        result.ServiceTypeId == PaymentTypeStopLoss.ServiceTypeId &&
                        result.Line == currentClaimCharge.Line)
                : GetPaymentResult(paymentResults, isCarveOut,
                    currentClaimCharge.Line);

            //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
            Utilities.UpdatePaymentResult(paymentResult,
                PaymentTypeStopLoss.ServiceTypeId,
                PaymentTypeStopLoss.PaymentTypeDetailId,
                PaymentTypeStopLoss.PaymentTypeId);

            if (paymentResult != null)
            {
                if (PaymentTypeStopLoss.Threshold.HasValue && PaymentTypeStopLoss.Percentage.HasValue)
                {
                    paymentResult.AdjudicatedValue = (currentClaimCharge.Line == lineId)
                        ? PaymentTypeStopLoss.IsExcessCharge
                            ? ((totalCharge - PaymentTypeStopLoss.Threshold.Value) / 100) *
                              PaymentTypeStopLoss.Percentage.Value
                            : (PaymentTypeStopLoss
                                .Percentage.Value /
                               100) * totalCharge
                        : 0;

                    paymentResult.ClaimStatus =
                        (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                    Utilities.UpdateStopLossDetails(paymentResult, claim.SmartBox, PaymentTypeStopLoss, PaymentTypeStopLoss.IsFormulaError);
                }
                else
                    paymentResult.ClaimStatus =
                        (byte)Enums.AdjudicationOrVarianceStatuses.AdjudicationErrorInvalidPaymentData;
            }
        }

        /// <summary>
        /// Gets the valid payment results.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        private IEnumerable<PaymentResult> GetValidPaymentResults(IEvaluateableClaim claim, List<PaymentResult> paymentResults,
            bool isCarveOut)
        {
            foreach (var claimCharge in claim.ClaimCharges.Where(claimCharge => PaymentTypeBase.ValidLineIds.Contains(claimCharge.Line) && paymentResults.Any(
                currentPaymentResult =>
                    currentPaymentResult.Line == claimCharge.Line && (currentPaymentResult.AdjudicatedValue == null || isCarveOut))).Where(claimCharge => !paymentResults.Any(
                        result =>
                            result.ServiceTypeId == PaymentTypeStopLoss.ServiceTypeId && result.Line == claimCharge.Line)))
            {
                GetPaymentResult(paymentResults, isCarveOut, claimCharge.Line);
            }
            List<PaymentResult> validPaymentResults =
                paymentResults.Where(
                    currentPaymentResult => currentPaymentResult.Line.HasValue &&
                                            PaymentTypeStopLoss.ValidLineIds.Contains(
                                                currentPaymentResult.Line.Value) &&
                                            currentPaymentResult.Line != null &&
                                            (currentPaymentResult.AdjudicatedValue == null ||
                                             currentPaymentResult.ServiceTypeId == PaymentTypeStopLoss.ServiceTypeId))
                    .ToList();
            return validPaymentResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentResult"></param>
        /// <param name="totalcharge"></param>
        /// <param name="claim"></param>
        private PaymentResult GetTotalLinesValue(PaymentResult paymentResult, double? totalcharge, IEvaluateableClaim claim)
        {
            //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
            Utilities.UpdatePaymentResult(paymentResult, PaymentTypeStopLoss.ServiceTypeId,
                PaymentTypeStopLoss.PaymentTypeDetailId, PaymentTypeStopLoss.PaymentTypeId);

            if (paymentResult != null &&
                (PaymentTypeStopLoss.Threshold != null && totalcharge > PaymentTypeStopLoss.Threshold.Value &&
                 PaymentTypeStopLoss.Percentage != null))
            {
                paymentResult.AdjudicatedValue = PaymentTypeStopLoss.IsExcessCharge
                    ? ((totalcharge - PaymentTypeStopLoss.Threshold.Value) / 100) *
                      PaymentTypeStopLoss.Percentage.Value
                    : (PaymentTypeStopLoss
                        .Percentage.Value /
                       100) * totalcharge;
                paymentResult.ClaimStatus =
                    (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                Utilities.UpdateStopLossDetails(paymentResult, claim.SmartBox, PaymentTypeStopLoss, PaymentTypeStopLoss.IsFormulaError);
            }
            return paymentResult;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="paymentResults"></param>
        /// <param name="isCarveOut"></param>
        /// <returns></returns>
        private void ApplyPerChargeLine(IEvaluateableClaim claim,
            List<PaymentResult> paymentResults, bool isCarveOut)
        {
            if (PaymentTypeBase.ValidLineIds != null && PaymentTypeBase.ValidLineIds.Any())
            {
                List<int> adjudicatedLines = new List<int>();
                foreach (
                    var lineId in PaymentTypeStopLoss.ValidLineIds.Where(lineId => !adjudicatedLines.Contains(lineId)))
                {
                    //Add lineId into Adjudicated line list
                    adjudicatedLines.Add(lineId);

                    ClaimCharge claimCharge =
                        claim.ClaimCharges.FirstOrDefault(currentClaimCharge => currentClaimCharge.Line == lineId);

                    ApplyChargesOnPaymentType(claim, paymentResults, isCarveOut, claimCharge, lineId);
                }
            }
        }

        /// <summary>
        /// Applies the type of the charges on payment.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="claimCharge">The claim charge.</param>
        /// <param name="lineId">The line identifier.</param>
        private void ApplyChargesOnPaymentType(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut,
            ClaimCharge claimCharge, int lineId)
        {
            if (PaymentTypeStopLoss.Threshold != null && (claimCharge != null &&
                                                          (PaymentTypeBase.ValidLineIds.Contains(
                                                              claimCharge.Line) && paymentResults.Any(
                                                                  currentPaymentResult =>
                                                                      currentPaymentResult.Line ==
                                                                      claimCharge.Line &&
                                                                      (currentPaymentResult.AdjudicatedValue ==
                                                                       null || isCarveOut ||
                                                                       currentPaymentResult.ServiceTypeId ==
                                                                       PaymentTypeStopLoss.ServiceTypeId))) &&
                                                          claimCharge.Amount >
                                                          PaymentTypeStopLoss.Threshold.Value))
            {
                PaymentResult paymentResult = paymentResults.Any(
                    result =>
                        result.ServiceTypeId == PaymentTypeStopLoss.ServiceTypeId && result.Line == lineId)
                    ? paymentResults.FirstOrDefault(
                        result =>
                            result.ServiceTypeId == PaymentTypeStopLoss.ServiceTypeId &&
                            result.Line == lineId)
                    : GetPaymentResult(paymentResults, isCarveOut, claimCharge.Line);

                GetTotalLinesValue(paymentResult, claimCharge.Amount, claim);
            }
        }

        /// <summary>
        /// Applies the per day.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        /// 
        private void ApplyPerDay(IEvaluateableClaim claim,
            List<PaymentResult> paymentResults, bool isCarveOut)
        {
            var validPaymentResults = GetValidPaymentResults(claim, paymentResults, isCarveOut);
            List<int> days = GetDays();

            List<ClaimCharge> validCharges = (from charge in claim.ClaimCharges
                                              from lineId in PaymentTypeStopLoss.ValidLineIds
                                              where charge.Line == lineId
                                              select charge).ToList();
            List<DateTime> serviceDates = validCharges.OrderBy(claimCharge => claimCharge.ServiceFromDate).Select(claimCharge => claimCharge.ServiceFromDate).Distinct().OrderBy(b => b).ToList();

            List<DateTime> validServiceDates = GetValidDates(serviceDates, days);

            if (PaymentTypeBase.ValidLineIds != null && PaymentTypeBase.ValidLineIds.Count > 0)
            {
                if (days.Any())
                {
                    GetQualifiedAndNonQualifiedCharges(claim, paymentResults, validCharges, validServiceDates, validPaymentResults);
                }
                else
                {
                    GetGroupedClaimCharges(claim, paymentResults, validCharges, validPaymentResults);
                }
            }
        }

        /// <summary>
        /// Gets the grouped claim charges.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="validCharges">The valid charges.</param>
        /// <param name="validPaymentResults">The valid payment results.</param>
        private void GetGroupedClaimCharges(IEvaluateableClaim claim, List<PaymentResult> paymentResults, IEnumerable<ClaimCharge> validCharges,
            IEnumerable<PaymentResult> validPaymentResults)
        {
            List<int?> lineIds =
                paymentResults.Where(
                    b =>
                        (b.AdjudicatedValue == null || b.ServiceTypeId == PaymentTypeBase.ServiceTypeId) &&
                        b.Line != null).Select(q => q.Line).ToList();

            var groupedClaimCharges =
                validCharges.Where(a => lineIds.Contains(a.Line)).GroupBy(claimCharge => new
                {
                    claimCharge.ServiceFromDate,
                    RevCode = !string.IsNullOrEmpty(PaymentTypeStopLoss.RevCode) ? claimCharge.RevCode : null,
                    CPTCode = !string.IsNullOrEmpty(PaymentTypeStopLoss.HcpcsCode) ? claimCharge.HcpcsCode : null,
                }
                    )
                    .Select(claimCharge => new { GroupName = claimCharge.Key, Members = claimCharge });

            foreach (var claimCharges in groupedClaimCharges)
            {
                double? totalAmount = claimCharges.Members.Sum(claimCharge => claimCharge.Amount);
                int result = 1;
                foreach (ClaimCharge claimchg in claimCharges.Members)
                {
                    // ReSharper disable once LoopCanBeConvertedToQuery It's hard to debug
                    foreach (PaymentResult payment in paymentResults)
                    {
                        if (payment.Line == claimchg.Line && (payment.ServiceTypeId == null || payment.ServiceTypeId == PaymentTypeBase.ServiceTypeId))
                        {
                            result = CalculatePerDay(paymentResults, totalAmount, validPaymentResults.ToList(), claimchg, result, claim);
                            break;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Gets the qualified and non qualified charges.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="validCharges">The valid charges.</param>
        /// <param name="validServiceDates">The valid service dates.</param>
        /// <param name="validPaymentResults">The valid payment results.</param>
        private void GetQualifiedAndNonQualifiedCharges(IEvaluateableClaim claim, List<PaymentResult> paymentResults, List<ClaimCharge> validCharges,
            List<DateTime> validServiceDates, IEnumerable<PaymentResult> validPaymentResults)
        {
            var qualifiedCharges = GetCharges(claim, paymentResults, validCharges, validServiceDates);
            List<int?> lineIds =
                paymentResults.Where(
                    b =>
                        (b.AdjudicatedValue == null || b.ServiceTypeId == PaymentTypeBase.ServiceTypeId) &&
                        b.Line != null).Select(q => q.Line).ToList();

            var groupedClaimCharges =
                qualifiedCharges.Where(a => lineIds.Contains(a.Line)).GroupBy(claimCharge => new
                {
                    claimCharge.ServiceFromDate,
                    RevCode = !string.IsNullOrEmpty(PaymentTypeStopLoss.RevCode) ? claimCharge.RevCode : null,
                    CPTCode = !string.IsNullOrEmpty(PaymentTypeStopLoss.HcpcsCode) ? claimCharge.HcpcsCode : null,
                }
                    )
                    .Select(claimCharge => new { GroupName = claimCharge.Key, Members = claimCharge });

            foreach (var claimCharges in groupedClaimCharges)
            {
                double? groupCharge = claimCharges.Members.Sum(x => x.Amount);

                int result = 1;
                foreach (ClaimCharge claimchg in claimCharges.Members)
                {
                    bool any = false;
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (PaymentResult payment in paymentResults)
                    {
                        if (payment.Line == claimchg.Line && (payment.ServiceTypeId == PaymentTypeBase.ServiceTypeId || payment.AdjudicatedValue == null))
                        {
                            any = true;
                            break;
                        }
                    }
                    if (!any) break;
                    // ReSharper disable once PossibleMultipleEnumeration
                    result = CalculatePerDay(paymentResults, groupCharge, validPaymentResults, claimchg, result, claim);
                }
            }
        }

        /// <summary>
        /// Gets the charges.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="validCharges">The valid charges.</param>
        /// <param name="validServiceDates">The valid service dates.</param>
        /// <returns></returns>
        private IEnumerable<ClaimCharge> GetCharges(IEvaluateableClaim claim, IEnumerable<PaymentResult> paymentResults, List<ClaimCharge> validCharges, List<DateTime> validServiceDates)
        {
            var qualifiedCharges =
                validCharges.Where(i => validServiceDates.Contains(i.ServiceFromDate))
                    .OrderBy(claimCharge => claimCharge.ServiceFromDate)
                    .ToList();

            var nonQualifiedChargeList = validCharges.Except(qualifiedCharges).ToList();

            IList<PaymentResult> enumerable = paymentResults as IList<PaymentResult> ?? paymentResults.ToList();


            var groupedClaimCharges =
                nonQualifiedChargeList.GroupBy(claimCharge => new
                {
                    claimCharge.ServiceFromDate,
                    RevCode = !string.IsNullOrEmpty(PaymentTypeStopLoss.RevCode) ? claimCharge.RevCode : null,
                    CPTCode = !string.IsNullOrEmpty(PaymentTypeStopLoss.HcpcsCode) ? claimCharge.HcpcsCode : null,
                }
                    )
                    .Select(claimCharge => new { GroupName = claimCharge.Key, Members = claimCharge });

            foreach (var claimCharges in groupedClaimCharges)
            {
                double? groupCharge = claimCharges.Members.Sum(x => x.Amount);

                if (groupCharge > PaymentTypeStopLoss.Threshold)
                {
                    foreach (var paymentResult in from claimCharge in claimCharges.Members
                                                  from paymentResult in enumerable
                                                  where
                                                      claimCharge.Line == paymentResult.Line &&
                                                      (paymentResult.AdjudicatedValue == null || paymentResult.ServiceTypeId == PaymentTypeStopLoss.ServiceTypeId)
                                                  select paymentResult)
                    {
                        paymentResult.AdjudicatedValue = 0;
                        paymentResult.ClaimStatus =
                            (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                        paymentResult.PaymentTypeId =
                            (byte)Enums.PaymentTypeCodes.StopLoss;
                        paymentResult.ServiceTypeId = PaymentTypeStopLoss.ServiceTypeId;
                        Utilities.UpdateStopLossDetails(paymentResult, claim.SmartBox, PaymentTypeStopLoss,
                            PaymentTypeStopLoss.IsFormulaError);
                    }
                }
            }


            return qualifiedCharges;
        }


        /// <summary>
        /// Gets the days from stop loss days property of per day .
        /// </summary>
        /// <returns></returns>
        private List<int> GetDays()
        {
            List<int> days = new List<int>();

            if (!string.IsNullOrEmpty(PaymentTypeStopLoss.Days))
            {
                List<string> splitedDays = PaymentTypeStopLoss.Days.Split(',').ToList();
                foreach (var day in splitedDays)
                {
                    if (day.Contains(':'))
                    {
                        List<string> ranges = new List<string>((day.Split(':')));
                        if (ranges.Count == 2)
                            for (int i = Convert.ToInt16(string.IsNullOrEmpty(ranges[0]) ? "0" : ranges[0], CultureInfo.InvariantCulture);
                                i <= Convert.ToInt16(string.IsNullOrEmpty(ranges[1]) ? "0" : ranges[1], CultureInfo.InvariantCulture);
                                i++)
                                days.Add(i);
                    }
                    else
                        days.Add(Convert.ToInt16(day, CultureInfo.InvariantCulture));
                }
            }
            return days;
        }

        /// <summary>
        /// Gets the valid dates.
        /// </summary>
        /// <param name="serviceDates">The service dates.</param>
        /// <param name="days">The days.</param>
        /// <returns></returns>
        private static List<DateTime> GetValidDates(List<DateTime> serviceDates, List<int> days)
        {
            List<DateTime> validDates = new List<DateTime>();

            DateTime minDate = serviceDates.FirstOrDefault();
            DateTime maxDate = serviceDates.LastOrDefault();

            int dayDiff = (maxDate - minDate).Days + 1;

            for (int j = 1; j <= dayDiff; j++)
            {
                DateTime currentDate = minDate.AddDays(j - 1);
                if (serviceDates.Contains(currentDate) && days.Contains(j))
                    validDates.Add(currentDate);
            }
            return validDates;
        }

        /// <summary>
        /// Calculates the per day.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="groupCharge">The group charge.</param>
        /// <param name="validPaymentResults">The valid payment results.</param>
        /// <param name="claimCharge">The claimCharge.</param>
        /// <param name="itemIndex">The itemIndex.</param>
        /// <param name="claim"></param>
        /// <returns></returns>
        private int CalculatePerDay(ICollection<PaymentResult> paymentResults, double? groupCharge, IEnumerable<PaymentResult> validPaymentResults, ClaimCharge claimCharge,
            int itemIndex, IEvaluateableClaim claim)
        {
            ////Logic to apply stop loss
            if (groupCharge > 0 &&
                (PaymentTypeStopLoss.Threshold != null &&
                 groupCharge > PaymentTypeStopLoss.Threshold.Value &&
                 PaymentTypeStopLoss.Percentage != null))
            {
                PaymentResult paymentResult =
                    validPaymentResults.FirstOrDefault(
                        currentPaymentResult => currentPaymentResult.Line == claimCharge.Line && (currentPaymentResult.AdjudicatedValue == null || currentPaymentResult.ServiceTypeId == PaymentTypeStopLoss.ServiceTypeId));

                if (paymentResult != null)
                {
                    itemIndex = ApplyPerDayFormula(groupCharge, itemIndex, claim, paymentResult);
                }
            }
            else
            {
                PaymentResult paymentResult =
                    validPaymentResults.FirstOrDefault(
                        x => x.Line == claimCharge.Line && x.AdjudicatedValue == null && !x.IsInitialEntry);
                if (paymentResult != null)
                    paymentResults.Remove(paymentResult);
            }
            return itemIndex;
        }

        /// <summary>
        /// Applies the per day formula.
        /// </summary>
        /// <param name="groupCharge">The group charge.</param>
        /// <param name="itemIndex">Index of the item.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResult">The payment result.</param>
        /// <returns></returns>
        private int ApplyPerDayFormula(double? groupCharge, int itemIndex, IEvaluateableClaim claim, PaymentResult paymentResult)
        {
            if (itemIndex == 1)
            {
                if (PaymentTypeStopLoss.Threshold != null)
                    if (PaymentTypeStopLoss.Percentage != null)
                        paymentResult.AdjudicatedValue =
                            PaymentTypeStopLoss.IsExcessCharge
                                ? ((groupCharge - PaymentTypeStopLoss.Threshold.Value) / 100) *
                                  PaymentTypeStopLoss.Percentage.Value
                                : (PaymentTypeStopLoss
                                    .Percentage.Value /
                                   100) * groupCharge;

                itemIndex++;
            }
            else
                paymentResult.AdjudicatedValue = 0.0;

            paymentResult.ClaimStatus =
                (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
            paymentResult.PaymentTypeId = (byte)Enums.PaymentTypeCodes.StopLoss;
            paymentResult.ServiceTypeId = PaymentTypeStopLoss.ServiceTypeId;
            Utilities.UpdateStopLossDetails(paymentResult, claim.SmartBox, PaymentTypeStopLoss,
                PaymentTypeStopLoss.IsFormulaError);
            return itemIndex;
        }

        /// <summary>
        /// Applies the contract filter.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <returns></returns>
        private void ApplyContractFilter(IEvaluateableClaim claim,
            IEnumerable<PaymentResult> paymentResults)
        {
            //Check Stop loss conditions
            if (PaymentTypeStopLoss.StopLossConditionId == (byte)Enums.StopLossCondition.TotalChargeLines &&
                (PaymentTypeStopLoss.Threshold.HasValue && PaymentTypeStopLoss.Percentage.HasValue &&
                 claim.ClaimTotal != null) && claim.ClaimTotal > PaymentTypeStopLoss.Threshold.Value)
            {
                //Get OverAll Payment result
                PaymentResult paymentResult = paymentResults.First();

                //Update Adjudicated Value
                paymentResult.AdjudicatedValue = PaymentTypeStopLoss.IsExcessCharge
                    ? ((claim.ClaimTotal.Value - PaymentTypeStopLoss.Threshold.Value) / 100) *
                      PaymentTypeStopLoss.Percentage.Value
                    : (claim.ClaimTotal.Value / 100) * PaymentTypeStopLoss.Percentage.Value;
                Utilities.UpdateStopLossDetails(paymentResult, claim.SmartBox, PaymentTypeStopLoss, PaymentTypeStopLoss.IsFormulaError);
            }
        }
    }
}
