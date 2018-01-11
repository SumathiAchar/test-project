/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 29-Dec-2013
/**  Summary        : Handles Add/Modify PaymentType Per Diem Details functionalities

/************************************************************************************************************/

using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;
using System.Collections.Generic;

namespace SSI.ContractManagement.BusinessLogic
{
    public class PaymentTypePerDiemLogic : PaymentTypeBaseLogic
    {
        /// <summary>
        /// The _payment type per diem details repository
        /// </summary>
        private readonly IPaymentTypePerDiemRepository _paymentTypePerDiemDetailsRepository;

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
        private PaymentTypePerDiem PaymentTypePerDiem
        {
            get { return PaymentTypeBase as PaymentTypePerDiem; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePerDiemLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypePerDiemLogic(string connectionString)
        {
            _paymentTypePerDiemDetailsRepository = Factory.CreateInstance<IPaymentTypePerDiemRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePerDiemLogic"/> class.
        /// </summary>
        /// <param name="paymentTypePerDiemDetailsRepository">The payment type per diem details repository.</param>
        public PaymentTypePerDiemLogic(IPaymentTypePerDiemRepository paymentTypePerDiemDetailsRepository)
        {
            if (paymentTypePerDiemDetailsRepository != null)
                _paymentTypePerDiemDetailsRepository = paymentTypePerDiemDetailsRepository;
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypePerDiemDetailsRepository.AddEditPaymentTypePerDiemDetails((PaymentTypePerDiem)paymentType);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypePerDiemDetailsRepository.GetPaymentTypePerDiem((PaymentTypePerDiem)paymentType);
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
            if (PaymentTypePerDiem.PayAtClaimLevel)
            {
                PaymentResult claimPaymentResult = GetClaimLevelPaymentResult(paymentResults, isCarveOut);
                if (claimPaymentResult != null && claim != null)
                {
                    GetAdjudicatedValue(true, claimPaymentResult, claim.Los);
                }
            }
            else
            {
                var validPaymentResults = GetValidPaymentResults(claim, paymentResults, isCarveOut);
                if (claim != null)
                {
                    List<ClaimCharge> validCharges = (from charge in claim.ClaimCharges
                        from lineId in PaymentTypePerDiem.ValidLineIds
                        where charge.Line == lineId
                        select charge).ToList();
                    foreach (var charge in validCharges)
                    {
                        PaymentResult paymentresult =
                            validPaymentResults.FirstOrDefault(
                                currentPaymentResult =>
                                    currentPaymentResult.Line == charge.Line &&
                                    currentPaymentResult.AdjudicatedValue == null);
                        if (paymentresult != null)
                            GetAdjudicatedValue((charge.Units.HasValue && charge.Units > 0), paymentresult, charge.Units);
                    }
                }
            }
            return paymentResults;
        }

        /// <summary>
        /// Gets the adjudicated value.
        /// </summary>
        /// <param name="isUnitsAvailable">if set to <c>true</c> [is unit available].</param>
        /// <param name="paymentResult">The payment result.</param>
        /// <param name="units">The units.</param>
        private void GetAdjudicatedValue(bool isUnitsAvailable, PaymentResult paymentResult, int? units)
        {
            if (PaymentTypePerDiem.PerDiemSelections !=null && PaymentTypePerDiem.PerDiemSelections.Count > 0 && isUnitsAvailable)
            {
                Utilities.UpdatePaymentResult(paymentResult, PaymentTypePerDiem.ServiceTypeId,
                    PaymentTypePerDiem.PaymentTypeDetailId, PaymentTypePerDiem.PaymentTypeId);
                paymentResult.AdjudicatedValue = GetValue(units);
                paymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
            }
            else
            {
                paymentResult.ClaimStatus = PaymentTypePerDiem.PerDiemSelections != null && PaymentTypePerDiem.PerDiemSelections.Count <= 0
                    ? (byte)Enums.AdjudicationOrVarianceStatuses.AdjudicationErrorInvalidPaymentData
                    : (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;
            }
        }

        /// <summary>
        /// Get Adjudicated value (using formula) based on given units
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        private double GetValue(int? units)
        {
            double adjValue = 0.0;
            for (int iloop = 1; iloop <= units; iloop++)
                adjValue += (from perDiemSelection in PaymentTypePerDiem.PerDiemSelections
                             let rate = perDiemSelection.Rate
                             where rate != null
                             where
                                 perDiemSelection.DaysFrom != null && perDiemSelection.DaysTo != null && rate != null
                             where
                                 perDiemSelection.DaysTo != null &&
                                 (perDiemSelection.DaysFrom != null &&
                                  (iloop >= perDiemSelection.DaysFrom.Value && iloop <= perDiemSelection.DaysTo.Value))
                             select rate.Value).Sum();
            return adjValue;
        }

        /// <summary>
        /// Get list  of payment results which matches with line id in Claim charge list
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="paymentResults"></param>
        /// <param name="isCarveOut"></param>
        /// <returns></returns>
        private List<PaymentResult> GetValidPaymentResults(IEvaluateableClaim claim, List<PaymentResult> paymentResults,
            bool isCarveOut)
        {
            GetCarveOutPaymentResults(claim, paymentResults, isCarveOut);
            List<PaymentResult> validPaymentResults =
                paymentResults.Where(
                    currentPaymentResult => currentPaymentResult.Line.HasValue &&
                                            PaymentTypePerDiem.ValidLineIds.Contains(
                                                currentPaymentResult.Line.Value) &&
                                            currentPaymentResult.Line != null).ToList();
            return validPaymentResults;
        }
    }
}
