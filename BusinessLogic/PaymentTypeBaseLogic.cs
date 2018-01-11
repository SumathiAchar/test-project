using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// Class for common payment type method
    /// </summary>
    public abstract class PaymentTypeBaseLogic : IPaymentTypeLogic
    {
        /// <summary>
        /// The _condition logic
        /// </summary>
        private readonly IConditionLogic _conditionLogic;

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        public abstract PaymentTypeBase PaymentTypeBase { get; set; }

        /// <summary>
        /// Evaluates the type of the payment.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public abstract List<PaymentResult> EvaluatePaymentType(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut, bool isContractFilter);

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeBaseLogic"/> class.
        /// </summary>
        protected PaymentTypeBaseLogic()
        {
            _conditionLogic = Factory.CreateInstance<IConditionLogic>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeBaseLogic"/> class.
        /// </summary>
        /// <param name="conditionLogic">The condition logic.</param>
        protected PaymentTypeBaseLogic(IConditionLogic conditionLogic)
        {
            if (conditionLogic != null)
                _conditionLogic = conditionLogic;
        }

        /// <summary>
        /// Determines whether [is payment type conditions] [the specified payment type].
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        public bool IsPaymentTypeConditions(IPaymentType paymentType, IEvaluateableClaim claim)
        {
            if (paymentType != null)
            {
                paymentType.ValidLineIds = new List<int>();
                //Fill LHS into Contract's Condition objects
                foreach (ICondition condition in paymentType.Conditions)
                {
                    _conditionLogic.Condition = condition;
                    condition.LeftOperands =
                      _conditionLogic.GetPropertyValues(claim, _conditionLogic.Condition.PropertyColumnName);
                }

                if (claim != null)
                {
                    //Match claim charge line codes with Payment Type Conditions
                    foreach (ClaimCharge claimCharge in claim.ClaimCharges)
                    {
                        //If condition is not there than its valid. That's why we are initialization by default as true. 
                        //For Ex - Payment type don't have any rev code or cpt code than all the lines are valid in this case isMatch should be true
                        bool isMatch = true;
                        bool isClaimLevel = true;
                       //Based on condition Operand Identifier fill LHS operand with charge line codes
                        foreach (ICondition condition in paymentType.Conditions)
                        {
                            if (condition.OperandIdentifier != null)
                                switch ((Enums.OperandIdentifier) condition.OperandIdentifier)
                                {
                                    case Enums.OperandIdentifier.RevCode:
                                        condition.LeftOperands = new List<string> {claimCharge.RevCode};
                                        isClaimLevel = false;
                                        break;
                                    case Enums.OperandIdentifier.HcpcsCode:
                                        condition.LeftOperands = new List<string> { claimCharge.HcpcsCodeWithModifier };
                                        isClaimLevel = false;
                                        break;

                                    case Enums.OperandIdentifier.PlaceOfService:
                                        condition.LeftOperands = new List<string> {claimCharge.PlaceOfService};
                                        isClaimLevel = false;
                                        break;
                                }
                            _conditionLogic.Condition = condition;
                            isMatch = _conditionLogic.IsMatch();
                          
                            if (!isMatch)
                                break;
                        }
                        paymentType.PayAtClaimLevel = isClaimLevel;
                        //If Claim charge line codes matches with Payment Type Conditions then build valid charge line list
                        if (isMatch)
                        {
                            paymentType.ValidLineIds.Add(claimCharge.Line);
                        }
                    }
                }

                return paymentType.ValidLineIds.Any();
            }
            return false;
        }

        /// <summary>
        /// Gets the payment result.
        /// </summary>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="lineId">The line identifier.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected static PaymentResult GetPaymentResult(List<PaymentResult> paymentResults, bool isCarveOut, int lineId)
        {
            PaymentResult paymentResult = null;

            PaymentResult claimChargePaymentResult = paymentResults.FirstOrDefault(
                currentPaymentResult => currentPaymentResult.Line == lineId && currentPaymentResult.AdjudicatedValue != null);
            
            //If service type is CarveOut and matching line already adjudicated (AdjudicatedValue != null) then clone(add) charge line and set AdjudicatedValue = null
            if (paymentResults != null && (isCarveOut && !paymentResults.Exists(result => result.Line == lineId && result.AdjudicatedValue == null)))
            {
                if (claimChargePaymentResult != null)
                {
                    //For carve out service type we need to add clone payment result object with adjudicated value, status, service type to null ( using Utilities.ResetPaymentResult())
                    paymentResult = claimChargePaymentResult.Clone();

                    //Reset clone fields Adj amount, payment type id , adj status and service type id
                    Utilities.ResetPaymentResult(paymentResult);

                    //Add updated result into Payment result list
                    paymentResults.Add(paymentResult);
                }
            }
            else if (paymentResults != null)
                paymentResult = paymentResults.FirstOrDefault(
                    currentPaymentResult => currentPaymentResult.Line == lineId && currentPaymentResult.AdjudicatedValue == null);

            return paymentResult;
        }

        /// <summary>
        /// Updates the payment result list.(add new payment result if adjudication amount is null and carve out is there)
        /// </summary>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="lineId">The line identifier.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected void UpdatePaymentResults(List<PaymentResult> paymentResults, bool isCarveOut, int lineId)
        {
            //Check whether adjudication amount is there and carve out is there
            if (PaymentTypeBase.ValidLineIds.Contains(lineId) && paymentResults.Any(
                  currentPaymentResult =>
                      currentPaymentResult.Line == lineId && (currentPaymentResult.AdjudicatedValue == null || isCarveOut)))
            {
                GetPaymentResult(paymentResults, isCarveOut, lineId);
            }           
        }

        /// <summary>
        /// Determines whether the specified claim is match.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        public bool IsMatch(IEvaluateableClaim claim)
        {
            return IsPaymentTypeConditions(PaymentTypeBase, claim);
        }


        /// <summary>
        /// Evaluates the specified claim.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<PaymentResult> Evaluate(IEvaluateableClaim claim, List<PaymentResult> paymentResults,
            bool isCarveOut, bool isContractFilter)
        {
            //Determines whether the specified claim is match and has matching lines id's
            if (isContractFilter || IsMatch((claim)))
            {
                paymentResults = EvaluatePaymentType(claim, paymentResults, isCarveOut, isContractFilter);
            }
            return paymentResults;
        }

        /// <summary>
        /// Gets the carve out payment results.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected void GetCarveOutPaymentResults(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut)
        {
            if (claim != null)
                foreach (var claimCharge in claim.ClaimCharges.Where(claimCharge => PaymentTypeBase.ValidLineIds.Contains(claimCharge.Line) && paymentResults.Any(
                    currentPaymentResult =>
                        currentPaymentResult.Line == claimCharge.Line && (currentPaymentResult.AdjudicatedValue == null || isCarveOut))))
                {
                    GetPaymentResult(paymentResults, isCarveOut, claimCharge.Line);
                }
        }


        /// <summary>
        /// Gets the claim level payment result.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected static PaymentResult GetClaimLevelPaymentResult(List<PaymentResult> paymentResults, bool isCarveOut)
        {
            PaymentResult paymentResult = null;
            if (paymentResults != null)
            {
                if (paymentResults.Count(currentPaymentResult => currentPaymentResult.Line == null) == 1)
                {
                    PaymentResult claimPaymentResult = paymentResults.FirstOrDefault();
                    if (claimPaymentResult != null)
                    {
                        paymentResult = claimPaymentResult.Clone();
                        paymentResult.IsInitialEntry = false;
                        paymentResults.Add(paymentResult);
                    }
                }
                else
                {
                    paymentResult =
                        paymentResults.FirstOrDefault(
                            a => a.ServiceTypeId != null && a.Line == null && a.AdjudicatedValue == null);

                    if (paymentResult == null && isCarveOut)
                    {
                        PaymentResult claimPaymentResult = paymentResults.FirstOrDefault();
                        if (claimPaymentResult != null)
                        {
                            paymentResult = claimPaymentResult.Clone();
                            paymentResult.IsInitialEntry = false;
                            paymentResults.Add(paymentResult);
                        }
                    }    
                }
            }
            return paymentResult;
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public  abstract long AddEditPaymentType(PaymentTypeBase paymentType);

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public abstract PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType);
    }
}
