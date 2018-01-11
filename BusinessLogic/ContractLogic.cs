/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Add/Modify/Rename and Copy Contract functionalities
/**  User Story Id  : 5.User Story Add a new contract 
 *                    6.User Story: Copy a contract.
 *                    7.User Story: View or Modify a contract
 *                    User Story: Rename a contract
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;
using System.Globalization;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;
using System;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// Implements Add/Edit Contracts
    /// </summary>
    public class ContractLogic : ContractBaseLogic, IContractLogic
    {
        /// <summary>
        /// Initializes repository
        /// </summary>
        private readonly IContractRepository _contractRepository;

        /// <summary>
        /// The _service type logic
        /// </summary>
        private readonly IContractServiceTypeLogic _serviceTypeLogic;

        /// <summary>
        /// The _payment result logic
        /// </summary>
        private readonly IPaymentResultLogic _paymentResultLogic;

        /// <summary>
        /// Gets or sets the contract.
        /// </summary>
        /// <value>
        /// The contract.
        /// </value>
        public Contract Contract { get; set; }

        /// <summary>
        /// Gets or sets the adjudicate claims.
        /// </summary>
        /// <value>
        /// The adjudicate claims.
        /// </value>
        public List<EvaluateableClaim> AdjudicateClaims { get; set; }

        /// <summary>
        /// Gets or sets the early exit claims.
        /// </summary>
        /// <value>
        /// The early exit claims.
        /// </value>
        public List<EvaluateableClaim> EarlyExitClaims { get; set; }


        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        // This Field used in reports
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<IPaymentType> PaymentTypes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractLogic(string connectionString)
        {
            _contractRepository = Factory.CreateInstance<IContractRepository>(connectionString, true);
            _serviceTypeLogic = Factory.CreateInstance<IContractServiceTypeLogic>(connectionString, true);
            _paymentResultLogic = Factory.CreateInstance<IPaymentResultLogic>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractLogic"/> class.
        /// </summary>
        /// <param name="contractRepository">The contract repository.</param>
        /// <param name="serviceTypeLogic">The service type logic.</param>
        /// <param name="paymentResultLogic">The payment result logic.</param>
        public ContractLogic(IContractRepository contractRepository, IContractServiceTypeLogic serviceTypeLogic, IPaymentResultLogic paymentResultLogic)
        {
            if (contractRepository != null)
                _contractRepository = contractRepository;

            if (serviceTypeLogic != null)
                _serviceTypeLogic = serviceTypeLogic;

            if (paymentResultLogic != null)
                _paymentResultLogic = paymentResultLogic;

        }

        /// <summary>
        /// Gets the contract full information by unique identifier.
        /// </summary>
        /// <param name="contractInfo">The contract information.</param>
        /// <returns></returns>
        public ContractFullInfo GetContractFullInfo(Contract contractInfo)
        {
            ContractFullInfo contractFullInfo = _contractRepository.GetContractFullInfo(contractInfo);
            return contractFullInfo;
        }


        /// <summary>
        /// Copies the contract by unique identifier.
        /// </summary>
        /// <param name="contracts">The original contract unique identifier.</param>
        /// <returns></returns>
        public long CopyContract(Contract contracts)
        {
            return _contractRepository.CopyContract(contracts);
        }



        /// <summary>
        /// Adds the contract modified reason.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <returns></returns>
        public int AddContractModifiedReason(ContractModifiedReason reason)
        {
            return _contractRepository.AddContractModifiedReason(reason);
        }

        /// <summary>
        /// Renames the contract.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="nodeText">The node text.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public ContractHierarchy RenameContract(long nodeId, string nodeText, string userName)
        {
            return _contractRepository.RenameContract(nodeId, nodeText, userName);
        }

        /// <summary>
        /// Adds the edit contract basic information.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        public Contract AddEditContractBasicInfo(Contract contract)
        {
            return _contractRepository.AddEditContractBasicInfo(contract);
        }

        /// <summary>
        /// Gets the contract first level details.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Contract GetContractFirstLevelDetails(long? id)
        {
            if (id != null)
            {
                return _contractRepository.GetContractFirstLevelDetails(id);
            }
            return null;
        }

        /// <summary>
        /// Determines whether the specified claim is match.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        public bool IsMatch(IEvaluateableClaim claim)
        {
            return Contract != null && IsConditionsValid(Contract.Conditions, claim);
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
            if (claim != null)
            {
                bool isEarlyExit = claim.LastAdjudicatedContractId == Contract.ContractId &&
                                   claim.IsClaimAdjudicated;
                //claim codes with contract codes
                // ReSharper disable once PossibleNullReferenceException
                if (claim.ContractId != null || (IsMatch(claim)))
                {
                    //If the particular claim is adjudicated with the contract and contract is not modified after that, then the claim will go to the early exit skipping the adjudication
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    if (!isEarlyExit && claim != null && paymentResults != null)
                    {
                        //Update Contract Id into payment results
                        paymentResults.ForEach(paymentResult => paymentResult.ContractId = Contract.ContractId);

                        //Evaluate Service Type logic 
                        paymentResults = EvaluateServiceType(claim, paymentResults, isCarveOut);

                        //update payment result(Over All adjudication amount/Status)
                        paymentResults = _paymentResultLogic.Evaluate(claim, paymentResults, false, false);

                        //Evaluate Contract level payment filter
                        paymentResults = EvaluatePaymentFilter(claim, paymentResults);

                        //Updates the Adjudicated Claim details
                        AdjudicateClaims.Add(new EvaluateableClaim
                        {
                            ClaimId = claim.ClaimId,
                            ContractId = Contract.ContractId
                        });

                    }
                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    else 
                    {
                        //Updates the Claim details for early exit claims
                        EarlyExitClaims.Add(new EvaluateableClaim
                        {
                            ClaimIds = Convert.ToString(claim.ClaimId),
                            ContractId = Contract.ContractId
                        });
                        // The claim is already adjucated then pass Payment result as null so it will not update in DB.
                        return null;
                    }
                }
            }

            return paymentResults;
        }

        /// <summary>
        /// Evaluates the type of the service.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        private List<PaymentResult> EvaluateServiceType(IEvaluateableClaim claim, List<PaymentResult> paymentResults,
            bool isCarveOut)
        {
            if (Contract.ContractServiceTypes != null)
            {
                foreach (ContractServiceType contractServiceType in Contract.ContractServiceTypes)
                {
                    _serviceTypeLogic.ContractServiceType = contractServiceType;

                    if (_serviceTypeLogic.IsValidServiceType())
                        paymentResults = _serviceTypeLogic.Evaluate(claim, paymentResults, isCarveOut, false);
                }
            }
            return paymentResults;
        }

        /// <summary>
        /// Evaluates the contract payment filter.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <returns></returns>
        private List<PaymentResult> EvaluatePaymentFilter(IEvaluateableClaim claim,
            List<PaymentResult> paymentResults)
        {
            foreach (PaymentTypeBase paymentType in Contract.PaymentTypes)
            {
                //Inject payment logics based on Payment type.
                IPaymentTypeLogic paymentlogic = Factory.CreateInstance<IPaymentTypeLogic>(((Enums.PaymentTypeCodes)paymentType.PaymentTypeId).ToString());
                paymentlogic.PaymentTypeBase = paymentType;
                //Apply payment logic for each payment types. This will consider all charge line codes to match with contract codes
                paymentResults = paymentlogic.Evaluate(claim, paymentResults, false, true);
            }
            return paymentResults;
        }

        /// <summary>
        /// Updates the contract conditions.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        public Contract UpdateContractCondition(Contract contract)
        {
            if (contract != null)
            {
                List<ICondition> conditions = new List<ICondition>();
                conditions.AddRange(contract.Conditions);

                //Build ClaimState condition
                ICondition conditionClaimState = new Condition();
                conditionClaimState.PropertyColumnName = Constants.PropertyClaimState;
                conditionClaimState.OperandIdentifier = (byte)Enums.OperandIdentifier.ClaimState;
                conditionClaimState.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                conditionClaimState.ConditionOperator = (byte)Enums.ConditionOperation.NotEqualTo;
                conditionClaimState.RightOperand = Constants.InValidClaimStatusForAdjudication;
                conditions.Add(conditionClaimState);

                //Build ClaimType condition
                if ((contract.IsInstitutional || contract.IsProfessional) && !(contract.IsInstitutional && contract.IsProfessional))
                {
                    ICondition conditionClaimType = new Condition();
                    conditionClaimType.PropertyColumnName = Constants.PropertyClaimType;
                    conditionClaimType.OperandIdentifier = (byte)Enums.OperandIdentifier.ClaimType;
                    conditionClaimType.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                    conditionClaimType.ConditionOperator = (byte)Enums.ConditionOperation.Contains;
                    conditionClaimType.RightOperand = contract.IsInstitutional
                        ? Constants.ClaimTypeInstitutionalContract
                        : Constants.ClaimTypeProfessionalContract;
                    conditions.Add(conditionClaimType);
                }



                //Build Start/through date
                switch (contract.IsClaimStartDate)
                {
                    case true:
                        ICondition conditionFromStartDate = new Condition();
                        conditionFromStartDate.PropertyColumnName = Constants.PropertyStatementFrom;
                        conditionFromStartDate.RightOperand = Convert.ToString(contract.StartDate, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                        conditionFromStartDate.OperandIdentifier = (byte)Enums.OperandIdentifier.ClaimStartDate;
                        conditionFromStartDate.OperandType = (byte)Enums.OperandType.Date;
                        conditionFromStartDate.ConditionOperator = (byte)Enums.ConditionOperation.GreaterThanEqualTo;
                        conditions.Add(conditionFromStartDate);

                        ICondition conditionFromEndDate = new Condition();
                        conditionFromEndDate.PropertyColumnName = Constants.PropertyStatementFrom;
                        conditionFromEndDate.RightOperand = Convert.ToString(contract.EndDate, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                        conditionFromEndDate.OperandIdentifier = (byte)Enums.OperandIdentifier.ClaimStartDate;
                        conditionFromEndDate.OperandType = (byte)Enums.OperandType.Date;
                        conditionFromEndDate.ConditionOperator = (byte)Enums.ConditionOperation.LessThanEqualTo;
                        conditions.Add(conditionFromEndDate);

                        break;

                    case false:
                        ICondition conditionThruStartDate = new Condition();
                        conditionThruStartDate.PropertyColumnName = Constants.PropertyStatementThru;
                        conditionThruStartDate.RightOperand = Convert.ToString(contract.StartDate, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                        conditionThruStartDate.OperandIdentifier = (byte)Enums.OperandIdentifier.ClaimEndDate;
                        conditionThruStartDate.OperandType = (byte)Enums.OperandType.Date;
                        conditionThruStartDate.ConditionOperator = (byte)Enums.ConditionOperation.GreaterThanEqualTo;
                        conditions.Add(conditionThruStartDate);


                        ICondition conditionThruEndDate = new Condition();
                        conditionThruEndDate.PropertyColumnName = Constants.PropertyStatementThru;
                        conditionThruEndDate.RightOperand = Convert.ToString(contract.EndDate, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                        conditionThruEndDate.OperandIdentifier = (byte)Enums.OperandIdentifier.ClaimEndDate;
                        conditionThruEndDate.OperandType = (byte)Enums.OperandType.Date;
                        conditionThruEndDate.ConditionOperator = (byte)Enums.ConditionOperation.LessThanEqualTo;
                        conditions.Add(conditionThruEndDate);

                        break;
                }

                contract.Conditions = conditions;
            }

            return contract;
        }

        /// <summary>
        /// Checks the contract name is unique.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        public bool IsContractNameExist(Contract contract)
        {
            return _contractRepository.IsContractNameExist(contract);
        }

        /// <summary>
        /// Gets the adjudicated contract names.
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<Contract> GetAdjudicatedContracts(Contract contract)
        {
            return _contractRepository.GetAdjudicatedContracts(contract);
        }
    }
}
