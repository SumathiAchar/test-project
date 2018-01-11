/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 24-Dec-2014
/**  Summary        : Handles Add/Modify PaymentType ASC FeeSchedule Details functionalities

/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asc")]
    public class PaymentTypeAscFeeScheduleLogic : PaymentTypeBaseLogic
    {

        private const string DoubleConversionError = "Double Conversion Failed. Doc Id:";
        private const string BackgroundServiceUser = "BackgroundServiceUser";
        private const string Value = "Value:";
        private const string LineChargeValue = "LineChargeValue:";
        /// <summary>
        /// The _payment type asc fee schedule details repository
        /// </summary>
        private readonly IPaymentTypeAscFeeScheduleRepository _paymentTypeAscFeeScheduleDetailsRepository;

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
        private PaymentTypeAscFeeSchedule PaymentTypeAscFeeSchedule
        {
            get { return PaymentTypeBase as PaymentTypeAscFeeSchedule; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeAscFeeScheduleLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeAscFeeScheduleLogic(string connectionString)
        {
            _paymentTypeAscFeeScheduleDetailsRepository = Factory.CreateInstance<IPaymentTypeAscFeeScheduleRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeAscFeeScheduleLogic"/> class.
        /// </summary>
        /// <param name="paymentTypeAscFeeScheduleDetailsRepository">The payment type asc fee schedule details repository.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asc")]
        public PaymentTypeAscFeeScheduleLogic(IPaymentTypeAscFeeScheduleRepository paymentTypeAscFeeScheduleDetailsRepository)
        {
            if (paymentTypeAscFeeScheduleDetailsRepository != null)
                _paymentTypeAscFeeScheduleDetailsRepository = paymentTypeAscFeeScheduleDetailsRepository;
        }

        /// <summary>
        /// Gets the valid payment results.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        private List<PaymentResult> GetValidPaymentResults(IEvaluateableClaim claim, List<PaymentResult> paymentResults,
            bool isCarveOut)
        {
            GetCarveOutPaymentResults(claim, paymentResults, isCarveOut);
            List<PaymentResult> validPaymentResults =
                paymentResults.Where(
                    currentPaymentResult => currentPaymentResult.Line.HasValue &&
                                            PaymentTypeAscFeeSchedule.ValidLineIds.Contains(
                                                currentPaymentResult.Line.Value) &&
                                            currentPaymentResult.Line != null).ToList();
            return validPaymentResults;
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
        public override List<PaymentResult> EvaluatePaymentType(IEvaluateableClaim claim,
            List<PaymentResult> paymentResults, bool isCarveOut, bool isContractFilter)
        {
            var validPaymentResults = GetValidPaymentResults(claim, paymentResults, isCarveOut);
            List<ClaimCharge> validCharges = new List<ClaimCharge>();
            if (claim != null)
                validCharges = (from charge in claim.ClaimCharges
                    from lineid in PaymentTypeAscFeeSchedule.ValidLineIds
                    where charge.Line == lineid
                    select charge).ToList();

            List<ClaimFieldValue> validCodeAmounts = GetValidCodeAmounts(validCharges);
            int order = 0;

            foreach (var validCode in validCodeAmounts)
            {
                GetValidChargeAdjudicatedValue(validCode, validCharges, validPaymentResults, ref order);
            }
            ApplyingNfs(validCharges, validCodeAmounts, validPaymentResults);
            List<PaymentResult> tempPaymentResults = paymentResults.Where(result => result.Line == null || !(!result.IsInitialEntry && result.ClaimStatus == (byte) Enums.AdjudicationOrVarianceStatuses.UnAdjudicated && result.ServiceTypeId == null && result.AdjudicatedValue == null)).ToList();
            paymentResults = tempPaymentResults;
            return paymentResults;
        }

        /// <summary>
        /// Applyings the NFS.
        /// </summary>
        /// <param name="validCharges">The valid charges.</param>
        /// <param name="validCodeAmounts">The valid code amounts.</param>
        /// <param name="validPaymentResults">The valid payment results.</param>
        private void ApplyingNfs(IEnumerable<ClaimCharge> validCharges, List<ClaimFieldValue> validCodeAmounts, List<PaymentResult> validPaymentResults)
        {
            foreach (var validCharge in validCharges.Where(x => (!validCodeAmounts.Exists(a => ((a.Identifier == x.HcpcsCodeWithModifier.Trim()))) || !validCodeAmounts.Exists(a => ((a.Identifier.Trim() == x.HcpcsCodeWithModifier.Trim().Substring(0, 5) )))) && !string.IsNullOrWhiteSpace((x.HcpcsCodeWithModifier.Trim())) ))
            {
                PaymentResult paymentResult =
                    validPaymentResults.FirstOrDefault(
                        currentPaymentResult =>
                            currentPaymentResult.Line == validCharge.Line && currentPaymentResult.AdjudicatedValue == null);

                if (paymentResult != null)
                {
                    //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
                    Utilities.UpdatePaymentResult(paymentResult, PaymentTypeAscFeeSchedule.ServiceTypeId,
                        PaymentTypeAscFeeSchedule.PaymentTypeDetailId, PaymentTypeAscFeeSchedule.PaymentTypeId);

                    if (PaymentTypeAscFeeSchedule.NonFeeSchedule != null && validCharge.Amount != null)
                    {
                        paymentResult = GetAdjudicatedValue(PaymentTypeAscFeeSchedule.NonFeeSchedule.Value,
                            validCharge.Amount.Value.ToString(CultureInfo.InvariantCulture), paymentResult);
                        paymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                    }
                    else
                    {
                        if (PaymentTypeAscFeeSchedule.NonFeeSchedule == null)
                            paymentResult.ClaimStatus =
                                (byte)Enums.AdjudicationOrVarianceStatuses
                                    .AdjudicationErrorInvalidPaymentData;
                        else if (validCharge.Amount == null)
                            paymentResult.ClaimStatus =
                                (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;
                    }
                }
            }
        }


        /// <summary>
        /// Gets the valid code amounts.
        /// </summary>
        /// <param name="validCharges">The valid charges.</param>
        /// <returns></returns>
        private List<ClaimFieldValue> GetValidCodeAmounts(IEnumerable<ClaimCharge> validCharges)
        {
            List<ClaimFieldValue> codeAmounts = new List<ClaimFieldValue>();

            switch (PaymentTypeAscFeeSchedule.OptionSelection)
            {
                case 1:
                    foreach (ClaimCharge charge in validCharges)
                    {
                        bool isMatched=false;
                        foreach (var claimfieldvalue in PaymentTypeAscFeeSchedule.ClaimFieldDoc.ClaimFieldValues)
                        {
                            string hcpcsCode = charge.HcpcsCodeWithModifier.ToUpper();
                                
                            if (hcpcsCode == claimfieldvalue.Identifier.ToUpper())
                            {
                                codeAmounts.Add(new ClaimFieldValue
                                {
                                    Identifier = claimfieldvalue.Identifier,
                                    Value = claimfieldvalue.Value,
                                    ClaimFieldDocId = claimfieldvalue.ClaimFieldDocId,
                                    LineChargeValue = claimfieldvalue.Value,
                                    Line = charge.Line
                                });
                                isMatched = true;
                               break;
                            }
                        }
                        if (!isMatched && charge.HcpcsCodeWithModifier.Trim().Length!=5)
                        foreach (var claimfieldvalue in PaymentTypeAscFeeSchedule.ClaimFieldDoc.ClaimFieldValues)
                        {
                            string hcpcsCode = charge.HcpcsCode.Trim();

                            if (hcpcsCode == claimfieldvalue.Identifier.ToUpper())
                            {
                                codeAmounts.Add(new ClaimFieldValue
                                {
                                    Identifier = claimfieldvalue.Identifier,
                                    Value = claimfieldvalue.Value,
                                    ClaimFieldDocId = claimfieldvalue.ClaimFieldDocId,
                                    LineChargeValue = claimfieldvalue.Value,
                                    Line = charge.Line
                                });
                               break;
                            }
                        }
                    }
                    break;
                case 2:


                    codeAmounts.AddRange(from charge in validCharges
                                         from claimfieldvalue in PaymentTypeAscFeeSchedule.ClaimFieldDoc.ClaimFieldValues
                                         where (charge.HcpcsCodeWithModifier.ToUpper() == claimfieldvalue.Identifier.ToUpper()) || (!string.IsNullOrWhiteSpace(charge.HcpcsCodeWithModifier) && charge.HcpcsCodeWithModifier.Trim().Length != 5 && charge.HcpcsCode.ToUpper() == claimfieldvalue.Identifier.ToUpper())
                                         select new ClaimFieldValue
                                         {
                                             Identifier = claimfieldvalue.Identifier,
                                             Value = claimfieldvalue.Value,
                                             ClaimFieldDocId = claimfieldvalue.ClaimFieldDocId,
                                             LineChargeValue = Convert.ToString(charge.Amount, CultureInfo.InvariantCulture),
                                             Line = charge.Line
                                         });
                    break;
                default:
                    foreach (ClaimCharge charge in validCharges)
                    {
                        bool isMatched = false;
                        foreach (var claimfieldvalue in PaymentTypeAscFeeSchedule.ClaimFieldDoc.ClaimFieldValues)
                        {
                            string hcpcsCode = charge.HcpcsCode.ToUpper();
                            if (hcpcsCode == claimfieldvalue.Identifier)
                            {
                                codeAmounts.Add(new ClaimFieldValue
                                {
                                    Identifier = claimfieldvalue.Identifier,
                                    Value = claimfieldvalue.Value,
                                    ClaimFieldDocId = claimfieldvalue.ClaimFieldDocId,
                                    LineChargeValue = claimfieldvalue.Value,
                                    Line = charge.Line
                                });
                                isMatched = true;
                                break;
                            }
                        }
                        if (!isMatched)
                        foreach (var claimfieldvalue in PaymentTypeAscFeeSchedule.ClaimFieldDoc.ClaimFieldValues)
                        {
                            string hcpcsCode = charge.HcpcsCode.ToUpper();
                            if (hcpcsCode == claimfieldvalue.Identifier.ToUpper())
                            {
                                codeAmounts.Add(new ClaimFieldValue
                                {
                                    Identifier = claimfieldvalue.Identifier,
                                    Value = claimfieldvalue.Value,
                                    ClaimFieldDocId = claimfieldvalue.ClaimFieldDocId,
                                    LineChargeValue = claimfieldvalue.Value,
                                    Line = charge.Line
                                });
                                break;
                            }
                        }
                    }
                    break;
            }
            //log error Amount
            LogErrorAmount(codeAmounts);

            List<ClaimFieldValue> validCodeAmounts =
                codeAmounts.Where(codeAmount => !string.IsNullOrWhiteSpace(codeAmount.LineChargeValue))
                    .OrderByDescending(codeAmount => Convert.ToDouble(codeAmount.LineChargeValue, CultureInfo.InvariantCulture))
                    .ToList();
            validCodeAmounts.AddRange(
                codeAmounts.Where(codeAmount => string.IsNullOrWhiteSpace(codeAmount.LineChargeValue)));
            return validCodeAmounts;
        }

        /// <summary>
        /// Gets the adjudicated value.
        /// </summary>
        /// <param name="percentage">The percentage.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="paymentResult">The payment result.</param>
        /// <returns></returns>
        private static PaymentResult GetAdjudicatedValue(double? percentage, string amount, PaymentResult paymentResult)
        {
            if (percentage.HasValue && !string.IsNullOrEmpty(amount))
            {
                paymentResult.AdjudicatedValue = (percentage / 100) * Convert.ToDouble(amount, CultureInfo.InvariantCulture);
                paymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
            }
            else
                paymentResult.ClaimStatus =
                    (byte)Enums.AdjudicationOrVarianceStatuses.AdjudicationErrorInvalidPaymentData;
            return paymentResult;
        }


        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        /// FIXED-JAN-AA The payment type's CRUD operations also can be fitted into the PaymentTypeBase interface for uniformity. This will ensure that the interfacing is complete.
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeAscFeeScheduleDetailsRepository.AddEditPaymentTypeAscFeeScheduleDetails((PaymentTypeAscFeeSchedule)paymentType);
        }


        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        /// FIXED-JAN-AA The payment type's CRUD operations also can be fitted into the PaymentTypeBase interface for uniformity. This will ensure that the interfacing is complete.
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeAscFeeScheduleDetailsRepository.GetPaymentTypeAscFeeScheduleDetails((PaymentTypeAscFeeSchedule)paymentType);
        }

        /// <summary>
        /// Gets the table name selection.
        /// </summary>
        /// <param name="paymentTypeAscFeeSchedule">The payment type asc fee schedule.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asc")]
        public List<PaymentTypeTableSelection> GetTableNameSelection(PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule)
        {
            return _paymentTypeAscFeeScheduleDetailsRepository.GetTableNameSelection(paymentTypeAscFeeSchedule);
        }

        /// <summary>
        /// Gets the asc fee schedule options.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asc")]
        public List<AscFeeScheduleOption> GetAscFeeScheduleOptions()
        {
            return _paymentTypeAscFeeScheduleDetailsRepository.GetAscFeeScheduleOptions();
        }

        private void GetValidChargeAdjudicatedValue(ClaimFieldValue validCode, IEnumerable<ClaimCharge> validCharges,List<PaymentResult> validPaymentResults,ref int order)
        {
            foreach (var charge in validCharges)
            {
                PaymentResult paymentResult =
                    validPaymentResults.FirstOrDefault(
                        currentPaymentResult =>
                            currentPaymentResult.Line == charge.Line && validCode.Line == charge.Line && currentPaymentResult.AdjudicatedValue == null);
                if (paymentResult != null)
                {
                    //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
                    Utilities.UpdatePaymentResult(paymentResult, PaymentTypeAscFeeSchedule.ServiceTypeId,
                       PaymentTypeAscFeeSchedule.PaymentTypeDetailId, PaymentTypeAscFeeSchedule.PaymentTypeId);

                    string hcpcsCode = charge.HcpcsCode.ToUpper();

                    if ((hcpcsCode == validCode.Identifier || hcpcsCode.Substring(0, 5) == validCode.Identifier.Substring(0,5)) && validCode.Line == charge.Line)
                    {
                        string amount = validCode.Value;
                        switch (order)
                        {
                            case 0:
                                GetAdjudicatedValue(PaymentTypeAscFeeSchedule.Primary, amount, paymentResult);
                                break;
                            case 1:
                                GetAdjudicatedValue(PaymentTypeAscFeeSchedule.Secondary, amount, paymentResult);
                                break;
                            case 2:
                                GetAdjudicatedValue(PaymentTypeAscFeeSchedule.Tertiary, amount, paymentResult);
                                break;
                            case 3:
                                GetAdjudicatedValue(PaymentTypeAscFeeSchedule.Quaternary, amount, paymentResult);
                                break;
                            default:
                                GetAdjudicatedValue(PaymentTypeAscFeeSchedule.Others, amount, paymentResult);
                                break;
                        }
                        order++;
                        break;
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "LineChargeValue"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SSI.ContractManagement.Shared.Helpers.ErrorLog.Log.LogError(System.String,System.String,System.Exception)")]
        private  void LogErrorAmount(IEnumerable<ClaimFieldValue> codeAmounts)
        {
            foreach (var codeAmount in codeAmounts)
            {
                double amount;
                if (!string.IsNullOrWhiteSpace(codeAmount.Value) && !Double.TryParse(codeAmount.Value, out amount))
                {
                    //If value is not a double than give codeAmount as 0. And log the error for the same.
                    Log.LogError(
                        string.Format(CultureInfo.InvariantCulture, DoubleConversionError + Constants.FirstParameter + Value + Constants.SecondParameter,
                            PaymentTypeAscFeeSchedule.ClaimFieldDoc.ClaimFieldDocId, codeAmount.Value),
                        BackgroundServiceUser);
                    codeAmount.Value = Convert.ToString(0, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrWhiteSpace(codeAmount.LineChargeValue) &&
                    !Double.TryParse(codeAmount.LineChargeValue, out amount))
                {
                    //If line charge value is not a double than give codeAmount as 0. And log the error for the same.
                    Log.LogError(
                        string.Format(CultureInfo.InvariantCulture, DoubleConversionError + Constants.FirstParameter + LineChargeValue + Constants.SecondParameter,
                            PaymentTypeAscFeeSchedule.ClaimFieldDoc.ClaimFieldDocId, codeAmount.LineChargeValue),
                        BackgroundServiceUser);
                    codeAmount.Value = Convert.ToString(0, CultureInfo.InvariantCulture);
                }
            }
        }
       
    }
}
