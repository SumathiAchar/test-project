/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 29-Dec-2014
/**  Summary        : Handles Add/Modify PaymentType DRG Schedules Logic functionalities

/************************************************************************************************************/

using System;
using System.Globalization;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;
using System.Collections.Generic;


namespace SSI.ContractManagement.BusinessLogic
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Drg")]
    public class PaymentTypeDrgLogic : PaymentTypeBaseLogic
    {

        /// <summary>
        /// The _payment type DRG payment repository
        /// </summary>
        private readonly IPaymentTypeDrgPaymentRepository _paymentTypeDrgPaymentRepository;
        
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
        private PaymentTypeDrg PaymentTypeDrg
        {
            get { return PaymentTypeBase as PaymentTypeDrg; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeDrgLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeDrgLogic(string connectionString)
        {
            _paymentTypeDrgPaymentRepository = Factory.CreateInstance<IPaymentTypeDrgPaymentRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeDrgLogic"/> class.
        /// </summary>
        /// <param name="paymentTypeDrgPaymentRepository">The payment type DRG payment repository.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Drg")]
        public PaymentTypeDrgLogic(IPaymentTypeDrgPaymentRepository paymentTypeDrgPaymentRepository)
        {
            if (paymentTypeDrgPaymentRepository != null)
                _paymentTypeDrgPaymentRepository = paymentTypeDrgPaymentRepository;
        }


        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeDrgPaymentRepository.AddEditPaymentTypeDrgPayment((PaymentTypeDrg)paymentType);
        }
        
        /// <summary>
        /// Get Available Weight from master Table
        /// </summary>
        /// <returns>List of RelativeWeightList </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public List<RelativeWeight> GetAllRelativeWeightList()
        {
            return _paymentTypeDrgPaymentRepository.GetAllRelativeWeightList();
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeDrgPaymentRepository.GetPaymentTypeDrgPayment((PaymentTypeDrg)paymentType);
        }

        /// <summary>
        /// Evaluates the type of the payment.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SSI.ContractManagement.Shared.Helpers.ErrorLog.Log.LogError(System.String,System.String,System.Exception)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public override List<PaymentResult> EvaluatePaymentType(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut, bool isContractFilter)
        {
            PaymentResult claimPaymentResult = GetClaimLevelPaymentResult(paymentResults, isCarveOut);

            if (claimPaymentResult != null)
            {
                Utilities.UpdatePaymentResult(claimPaymentResult, PaymentTypeDrg.ServiceTypeId,
                    PaymentTypeDrg.PaymentTypeDetailId, PaymentTypeDrg.PaymentTypeId);
                ClaimFieldValue drgRate = PaymentTypeDrg.ClaimFieldDoc.ClaimFieldValues.Find(claimField => claimField.Identifier == claim.Drg);
                if (drgRate != null)
                {
                    double amount;
                    if (Double.TryParse(drgRate.Value, out amount))
                        claimPaymentResult.AdjudicatedValue = PaymentTypeDrg.BaseRate * amount;
                    else
                    {
                        //If value is not a double than give codeAmount as 0. And log the error for the same.
                        claimPaymentResult.AdjudicatedValue = 0;
                        Log.LogError(
                            string.Format(CultureInfo.InvariantCulture, Constants.ConversionFailedError, PaymentTypeDrg.ClaimFieldDoc.ClaimFieldDocId, drgRate.Value), Constants.BackgroundServiceUser);
                    }
                }
                else
                    claimPaymentResult.AdjudicatedValue = 0;
                claimPaymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
            }
            return paymentResults;
        }

        
    }
}
