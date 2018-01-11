using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class PaymentTypeMedicareLabFeeScheduleLogic : PaymentTypeBaseLogic
    {
        /// <summary>
        /// The _medicare lab fee schedule details repository
        /// </summary>
        private readonly IPaymentTypeMedicareLabFeeScheduleRepository _medicareLabFeeScheduleDetailsRepository;

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
        private PaymentTypeMedicareLabFeeSchedule PaymentTypeMedicareLabFeeSchedule { get { return PaymentTypeBase as PaymentTypeMedicareLabFeeSchedule; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareLabFeeScheduleLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeMedicareLabFeeScheduleLogic(string connectionString)
        {
            _medicareLabFeeScheduleDetailsRepository = Factory.CreateInstance<IPaymentTypeMedicareLabFeeScheduleRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareLabFeeScheduleLogic"/> class.
        /// </summary>
        /// <param name="paymentTypeMedicareLabFeeScheduleDetailsRepository">The payment type medicare lab fee schedule details repository.</param>
        public PaymentTypeMedicareLabFeeScheduleLogic(IPaymentTypeMedicareLabFeeScheduleRepository paymentTypeMedicareLabFeeScheduleDetailsRepository)
        {
            if (paymentTypeMedicareLabFeeScheduleDetailsRepository != null)
                _medicareLabFeeScheduleDetailsRepository = paymentTypeMedicareLabFeeScheduleDetailsRepository;
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _medicareLabFeeScheduleDetailsRepository.GetPaymentTypeMedicareLabFeeSchedulePayment((PaymentTypeMedicareLabFeeSchedule)paymentType);
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _medicareLabFeeScheduleDetailsRepository.AddEditPaymentTypeMedicareLabFeeSchedulePayment((PaymentTypeMedicareLabFeeSchedule)paymentType);
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
            if (claim != null)
                foreach (ClaimCharge claimCharge in claim.ClaimCharges)
                {
                    if (isCarveOut && paymentResults.Any(currentPaymentResult => currentPaymentResult.Line == claimCharge.Line && currentPaymentResult.ServiceTypeId == PaymentTypeBase.ServiceTypeId))
                        break;

                    if (PaymentTypeBase.ValidLineIds.Contains(claimCharge.Line) && paymentResults.Any(
                        paymentResult => paymentResult.Line == claimCharge.Line && (paymentResult.AdjudicatedValue == null || isCarveOut)))
                    {
                        PaymentResult paymentResult = GetPaymentResult(paymentResults, isCarveOut, claimCharge.Line);

                        if (paymentResult != null)
                            EvaluateLine(paymentResult, claimCharge, claim.MedicareLabFeeSchedules);
                    }
                }
            return paymentResults;
        }

        /// <summary>
        /// Evaluates the line.
        /// </summary>
        /// <param name="paymentResult">The payment result.</param>
        /// <param name="claimCharge">The claim charge.</param>
        /// <param name="medicareLabFeeSchedules">The medicare lab fee schedules.</param>
        /// <returns></returns>
        private void EvaluateLine(PaymentResult paymentResult, ClaimCharge claimCharge, List<MedicareLabFeeSchedule> medicareLabFeeSchedules)
        {
            //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
            Utilities.UpdatePaymentResult(paymentResult, PaymentTypeMedicareLabFeeSchedule.ServiceTypeId,
                PaymentTypeMedicareLabFeeSchedule.PaymentTypeDetailId, PaymentTypeMedicareLabFeeSchedule.PaymentTypeId);

            string hcpcsCode = claimCharge.HcpcsCodeWithModifier;

            //Get Medicare Lab Fee Schedule Amount based on HCPCS code
            double? medicareLabFeeScheduleAmount = GetAmount(medicareLabFeeSchedules, hcpcsCode);

            if (medicareLabFeeScheduleAmount.HasValue)
            {
                paymentResult.AdjudicatedValue= (PaymentTypeMedicareLabFeeSchedule.Percentage / 100) * medicareLabFeeScheduleAmount.Value;
                paymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
            }
            else{
                paymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.AdjudicationErrorInvalidPaymentData;
            }
        }


        /// <summary>
        /// Gets the medicare lab fee schedule amount.
        /// </summary>
        /// <param name="medicareLabFeeSchedules">The medicare lab fee schedule list.</param>
        /// <param name="hcpcsCode">The HCPCS code.</param>
        /// <returns></returns>
        private static double? GetAmount(List<MedicareLabFeeSchedule> medicareLabFeeSchedules, string hcpcsCode)
        {
            double? amount = null;
            if (medicareLabFeeSchedules != null && medicareLabFeeSchedules.Any())
            {
                //Find valid medicare lab fee scheduled based on passed HCPCS code
                MedicareLabFeeSchedule medicareLabFeeSchedule =
                    medicareLabFeeSchedules.FirstOrDefault(
                        medicareLabFeeScheduleItem =>
                            (medicareLabFeeScheduleItem.HcpcsCodeWithModifier.Trim()
                               == hcpcsCode));
                if (medicareLabFeeSchedule == null && hcpcsCode.Trim().Length>5)
                    medicareLabFeeSchedule =
                       medicareLabFeeSchedules.FirstOrDefault(
                           medicareLabFeeScheduleItem =>
                               (medicareLabFeeScheduleItem.HcpcsCodeWithModifier//if exact hcpcs code does not match than take substring n match exact identifier
                                  == hcpcsCode.Trim().Substring(0,5)));
                if (medicareLabFeeSchedule != null)
                    amount = medicareLabFeeSchedule.Amount;
            }
            return amount;
        }
    }
}
