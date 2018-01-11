using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    /// <summary>
    /// Interface IPaymentLogic
    /// </summary>
    public interface IPaymentTypeLogic:IAdjudicationBaseLogic
    {
        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>The type of the payment.</value>
        PaymentTypeBase PaymentTypeBase { get; set; }
        
        /// <summary>
        /// Evaluates the type of the payment.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        /// <returns></returns>
        // ReSharper disable once UnusedMemberInSuper.Global
        List<PaymentResult> EvaluatePaymentType(IEvaluateableClaim claim, List<PaymentResult> paymentResults,
            bool isCarveOut, bool isContractFilter);


    }
}
