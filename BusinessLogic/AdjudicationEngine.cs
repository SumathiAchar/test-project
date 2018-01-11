using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// Class for doing claim Adjudication
    /// </summary>
    public class AdjudicationEngine : IAdjudicationEngine
    {
        /// <summary>
        /// The _contract logic
        /// </summary>
        private readonly IContractLogic _contractLogic;

        /// <summary>
        /// The _contract repository
        /// </summary>
        private readonly IContractRepository _contractRepository;

        /// <summary>
        /// The _evaluateable claim logic
        /// </summary>
        private readonly IEvaluateableClaimLogic _evaluateableClaimLogic;

        /// <summary>
        /// The _evaluateable claim repository
        /// </summary>
        private readonly IEvaluateableClaimRepository _evaluateableClaimRepository;

        /// <summary>
        /// The _payment result logic
        /// </summary>
        private readonly IPaymentResultLogic _paymentResultLogic;

        /// <summary>
        /// The _contract log logic
        /// </summary>
        private readonly IContractLogLogic _contractLogLogic;

        /// <summary>
        /// The _facility logic
        /// </summary>
        private readonly IFacilityLogic _facilityLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjudicationEngine"/> class.
        /// </summary>
        public AdjudicationEngine(string connectionString)
        {
            _contractLogic = Factory.CreateInstance<IContractLogic>(connectionString, true);
            _contractRepository = Factory.CreateInstance<IContractRepository>(connectionString, true);
            _evaluateableClaimLogic = Factory.CreateInstance<IEvaluateableClaimLogic>();
            //FIXED-2016-R2-S3 : initialize _facilityLogic into constructor  
            _facilityLogic = Factory.CreateInstance<IFacilityLogic>();
            _evaluateableClaimRepository = Factory.CreateInstance<IEvaluateableClaimRepository>(connectionString, true);
            _paymentResultLogic = Factory.CreateInstance<IPaymentResultLogic>(connectionString, true);
            _contractLogLogic = Factory.CreateInstance<IContractLogLogic>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjudicationEngine"/> class.
        /// </summary>
        /// <param name="contractLogic">The contract logic.</param>
        /// <param name="contractRepository">The contract repository.</param>
        /// <param name="evaluateableClaimLogic">The evaluateable claim logic.</param>
        /// <param name="evaluateableClaimRepository">The evaluateable claim repository.</param>
        /// <param name="paymentResultLogic">The payment result logic.</param>
        /// <param name="contractLogLogic">The contract log logic.</param>
        /// <param name="facilityLogic">The facility logic.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "evaluateable")]
        public AdjudicationEngine(IContractLogic contractLogic, IContractRepository contractRepository, IEvaluateableClaimLogic evaluateableClaimLogic, IEvaluateableClaimRepository evaluateableClaimRepository, IPaymentResultLogic paymentResultLogic, IContractLogLogic contractLogLogic, IFacilityLogic facilityLogic)
        {
            if (contractLogic != null)
                _contractLogic = contractLogic;

            if (contractRepository != null)
                _contractRepository = contractRepository;

            if (evaluateableClaimRepository != null)
                _evaluateableClaimRepository = evaluateableClaimRepository;

            if (evaluateableClaimLogic != null)
                _evaluateableClaimLogic = evaluateableClaimLogic;

            if (paymentResultLogic != null)
                _paymentResultLogic = paymentResultLogic;

            if (contractLogLogic != null)
                _contractLogLogic = contractLogLogic;

            if (facilityLogic != null)
                _facilityLogic = facilityLogic;
        }

        /// <summary>
        /// Adjudicates the claim.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public Dictionary<long, List<PaymentResult>> AdjudicateClaim(List<EvaluateableClaim> evaluateableClaims, List<Contract> contracts, long taskId)
        {
            //Update Medicare object (L,C,D and Medicare Record)
            evaluateableClaims = _evaluateableClaimLogic.UpdateEvaluateableClaims(evaluateableClaims);

            //Update Contract Condition for each contract
            if (contracts != null)
            {
                for (int contractIndex = 0; contractIndex < contracts.Count; contractIndex++)
                    lock (contracts)
                    {
                        contracts[contractIndex] = _contractLogic.UpdateContractCondition(contracts[contractIndex]);
                    }
            }

            Dictionary<long, List<PaymentResult>> paymentResultDictionary = new Dictionary<long, List<PaymentResult>>();

            // make sure claim IDs are distinct before this loop; could call claims.Distinct() with custom comparer,
            // but should really do this in data access layer
            if (evaluateableClaims != null)
            {
                foreach (EvaluateableClaim claim in evaluateableClaims)
                {
                    paymentResultDictionary.Add(claim.ClaimId, new List<PaymentResult>());
                    //Initialize payment result.
                    List<PaymentResult> paymentResults = _paymentResultLogic.GetPaymentResults(claim);
                    //check Basic ClaimData is valid or not

                    paymentResults = _evaluateableClaimLogic.Evaluate(claim, paymentResults, false, false);
                    //Get claim level payment result
                    PaymentResult overAllClaimPaymentResult = paymentResults.FirstOrDefault(
                        payment => payment.Line == null && payment.ServiceTypeId == null);

                    if (overAllClaimPaymentResult != null && (overAllClaimPaymentResult.ClaimStatus !=
                                                              (byte)
                                                                  Enums.AdjudicationOrVarianceStatuses
                                                                      .AdjudicationErrorMissingServiceLine &&
                                                              overAllClaimPaymentResult.ClaimStatus !=
                                                              (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError))
                    {
                        paymentResults = UpdateConditions(contracts, taskId, claim, paymentResults,
                            overAllClaimPaymentResult);
                    }
                    //If the paymentResults is null, remove claim details from paymentResultDictionary, so that claim gets early exit and  payment results does not overwrite.
                    if (paymentResults != null)
                    {
                        //Update Payment Results for a claim
                        paymentResultDictionary[claim.ClaimId] = paymentResults;
                    }
                    else
                    {
                        paymentResultDictionary.Remove(claim.ClaimId);
                    }
                }
            }
            return paymentResultDictionary;
        }

        /// <summary>
        /// Updates the conditions.
        /// </summary>
        /// <param name="contracts">The contracts.</param>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="overAllClaimPaymentResult">The over all claim payment result.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SSI.ContractManagement.Shared.Helpers.ErrorLog.Log.LogError(System.String,System.String,System.Exception)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ClaimId"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TaskId"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private List<PaymentResult> UpdateConditions(IEnumerable<Contract> contracts, long taskId, EvaluateableClaim claim, List<PaymentResult> paymentResults,
            PaymentResult overAllClaimPaymentResult)
        {
            try
            {
                if (claim != null)
                {
                    //Checks if the claim is associated to any contract or the claim is picked for any contract from background 
                    if (claim.ContractId == null && claim.BackgroundContractId == 0)
                    {
                        // required for multiple threads so disabled 
                        // ReSharper disable once PossibleMultipleEnumeration
                        lock (contracts)
                        {
                            // required for multiple threads so disabled 
                            // ReSharper disable once PossibleMultipleEnumeration
                            // ReSharper disable PossibleMultipleEnumeration
                            var contractList = contracts.ToList();
                            // ReSharper restore PossibleMultipleEnumeration
                            for (int i = 0; i < contractList.Count(); i++) 
                            {
                                var contract = contractList[i];
                                List<ICondition> contractConditions = new List<ICondition>();
                                // for loop is required for multiple threads
                                // ReSharper disable once LoopCanBeConvertedToQuery
                                // ReSharper disable once ForCanBeConvertedToForeach
                                for (int index = 0; index < contract.Conditions.Count; index++)
                                {
                                    var condition = contract.Conditions[index];
                                    var tempCondition = condition.Clone();
                                    ICondition condition1 = tempCondition;
                                    contractConditions.Add(condition1);
                                }
                                _contractLogic.Contract = contract;

                                //Checking the payer condition
                                CheckPayerCondition(contract, claim);
                                //Checking the payer code condition
                                if (!string.IsNullOrEmpty(contract.PayerCode))
                                {
                                    AddPayerCodeCondition(contract);
                                }

                                //Skipping contract if no payers and no payer code
                                if (!string.IsNullOrEmpty(contract.PayerCode) || contract.Payers.Count > 0)
                                {
                                    //Evaluates the specified claim.
                                    paymentResults = _contractLogic.Evaluate(claim, paymentResults, false, false);
                                }

                                //Getting the actual contract conditions(except payer and payer code) after the claim has evaluated based on payer and payer code conditions
                                contract.Conditions = contractConditions;

                                //If claims got adjudicated against some contract than no need to check for other contracts
                                if (overAllClaimPaymentResult.ContractId > 0 || paymentResults == null)
                                    break;
                            }
                        }
                        if (overAllClaimPaymentResult.ContractId == null)
                            overAllClaimPaymentResult.ClaimStatus =
                                (byte) Enums.AdjudicationOrVarianceStatuses.AdjudicationErrorMissingContract;
                    }
                        //If the claim is retained for any claim
                    else if (claim.ContractId != null && claim.ContractId != 0)
                    {
                        _contractLogic.Contract =
                            contracts.FirstOrDefault(
                                a => a.ContractId == Convert.ToInt64(claim.ContractId, CultureInfo.InvariantCulture));
                        if (_contractLogic.Contract != null)
                            //Evaluates the specified claim.
                            // ReSharper disable once InconsistentlySynchronizedField
                            paymentResults = _contractLogic.Evaluate(claim, paymentResults, false, false);
                    }
                        //If the claim is picked for any contract from background 
                    else if (claim.BackgroundContractId != 0)
                    {
                        _contractLogic.Contract =
                            contracts.FirstOrDefault(
                                a =>
                                    a.ContractId ==
                                    Convert.ToInt64(claim.BackgroundContractId, CultureInfo.InvariantCulture));
                        if (_contractLogic.Contract != null)
                            //Evaluates the specified claim.
                            // ReSharper disable once InconsistentlySynchronizedField
                            paymentResults = _contractLogic.Evaluate(claim, paymentResults, false, false);
                    }
                }
            }

            catch (Exception ex)
            {
                overAllClaimPaymentResult.ClaimStatus =
                    (byte)Enums.AdjudicationOrVarianceStatuses.AdjudicationError;
                //Log Exception in Logging table.
                if (claim != null)
                    Log.LogError(
                        string.Format(CultureInfo.InvariantCulture, Constants.AdjudicationExceptionLog, taskId, claim.ClaimId),
                        Constants.BackgroundServiceUser, ex);
            }
            return paymentResults;
        }

        #region Multithreading

        /// <summary>
        /// Adds the claims for a task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        public long AddClaimsForATask(long taskId)
        {
            _evaluateableClaimRepository.UpdateRunningTask(taskId, Constants.One);
            return _evaluateableClaimRepository.AddClaimsForATask(taskId);
        }

        /// <summary>
        /// Adjudicates the task claims thread.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="facilityId">The facility identifier.</param>
        /// <param name="totalClaimCount">The total claim count.</param>
        /// <returns></returns>
        public List<Contract> GetContracts(long taskId, int facilityId, long totalClaimCount)
        {
            List<Contract> contracts = new List<Contract>();
            if (totalClaimCount > 0)
            {
                //Fetching the facility Medicare Details based on the facility id
                Facility facility = _facilityLogic.GetFacilityMedicareDetails(facilityId);

                //Retrieves the contracts from database based on taskId(contract list under selected model from a task)
                contracts = _contractRepository.GetContracts(taskId, facility.IsMedicareIpAcute, facility.IsMedicareOpApc);
            }
            return contracts;
        }

        /// <summary>
        /// Adjudicates the claims data thread.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="contracts">The contracts.</param>
        /// <param name="startRow">The start row.</param>
        /// <param name="endRow">The end row.</param>
        /// <returns></returns>
        public AdjudicatedClaimsResult AdjudicateClaimsDataThread(long taskId, int noOfRecords, List<Contract> contracts, long startRow, long endRow)
        {
            AdjudicatedClaimsResult adjudicatedClaimsResult = new AdjudicatedClaimsResult();
            List<EvaluateableClaim> adjudicateClaims = new List<EvaluateableClaim>();
            List<EvaluateableClaim> earlyExitClaims = new List<EvaluateableClaim>();


            _contractLogic.AdjudicateClaims = new List<EvaluateableClaim>();
            _contractLogic.EarlyExitClaims = new List<EvaluateableClaim>();

            //Retrieves claims from database based on taskId
            List<EvaluateableClaim> evaluateableClaims =
                _evaluateableClaimRepository.GetEvaluateableClaims(taskId, noOfRecords, startRow, endRow);
            Dictionary<long, List<PaymentResult>> paymentResultDictionary = new Dictionary<long, List<PaymentResult>>();
            try
            {
                if (evaluateableClaims != null && evaluateableClaims.Count > 0)
                {
                    //Get Payment Result Dictionary from claim and contract list
                    paymentResultDictionary =
                        AdjudicateClaim(evaluateableClaims,
                            contracts, taskId);

                    adjudicateClaims.AddRange(_contractLogic.AdjudicateClaims);
                    earlyExitClaims.AddRange(_contractLogic.EarlyExitClaims);
                }
                else
                {
                    adjudicatedClaimsResult.IsPaused = true;
                }

                adjudicatedClaimsResult.AdjudicateClaims = adjudicateClaims;
                adjudicatedClaimsResult.EarlyExitClaims = earlyExitClaims;
                adjudicatedClaimsResult.PaymentResult = paymentResultDictionary;
            }
            catch (Exception ex)
            {
                Log.LogError("AdjudicateClaimsDataThread - AdjudicateClaim " + taskId + "No of Records = " + noOfRecords + " " + ex.StackTrace, "AdjudicateClaim");
            }

            return adjudicatedClaimsResult;
        }

        /// <summary>
        /// Updates the payment results thread.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="contracts">The contracts.</param>
        /// <param name="paymentResultDictionary">The payment result dictionary.</param>
        /// <param name="adjudicatedClaims">The adjudicated claims.</param>
        /// <param name="earlyExitClaims">The early exit claims.</param>
        /// <returns></returns>
        public bool UpdatePaymentResultsThread(long taskId, int noOfRecords, List<Contract> contracts, Dictionary<long, List<PaymentResult>> paymentResultDictionary,
            List<EvaluateableClaim> adjudicatedClaims, List<EvaluateableClaim> earlyExitClaims)
        {
            //If Adjudicated Claims and Early Exit Claims are present, update the list and group by based on ContractId
            if (adjudicatedClaims != null)
            {
                if (adjudicatedClaims.Count > 0)
                {
                    adjudicatedClaims = (from claim in adjudicatedClaims
                                             group claim by claim.ContractId
                                                 into contract
                                                 select
                                                     new EvaluateableClaim
                                                     {
                                                         ContractId = contract.Key,
                                                         ClaimIds = string.Join(Constants.Comma, contract.Select(claim => claim.ClaimId))
                                                     }).ToList();
                }

            }

            //Update Payment Result and Early Exit claims in DB
            bool isRunning = _paymentResultLogic.UpdatePaymentResults(paymentResultDictionary, noOfRecords, taskId, adjudicatedClaims, earlyExitClaims);

            _evaluateableClaimRepository.UpdateRunningTask(taskId, Constants.One);
            //update Contract Log into DB
            if (GlobalConfigVariable.IsContractLogInsert)
                _contractLogLogic.AddContractLog(paymentResultDictionary, contracts);
            return isRunning;
        }

        /// <summary>
        /// Updates the running tasks thread.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        public void UpdateRunningTasksThread(long taskId)
        {
            _evaluateableClaimRepository.UpdateRunningTask(taskId, Constants.Zero);
        }
        #endregion Multithreading


        /// <summary>
        /// Checks the payer condition.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="claim">The claim.</param>
        private static void CheckPayerCondition(Contract contract, EvaluateableClaim claim)
        {
            if (contract.Payers.Count != 0 && (!string.IsNullOrEmpty(claim.PriPayerName)))
            {
                ICondition conditionPayer = new Condition();
                conditionPayer.PropertyColumnName = Constants.PropertyPriPayerName;
                conditionPayer.RightOperand = string.Join(";",
                    contract.Payers.Select(payer => payer.PayerName).ToList());
                conditionPayer.OperandIdentifier = (byte)Enums.OperandIdentifier.PayerName;
                conditionPayer.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                conditionPayer.ConditionOperator = (byte)Enums.ConditionOperation.EqualTo;
                contract.Conditions.Add(conditionPayer);
            }
        }

        /// <summary>
        /// Adds the payer code condition.
        /// </summary>
        /// <param name="contract">The contract.</param>
        private static void AddPayerCodeCondition(Contract contract)
        {
            ICondition conditionUdf = new Condition();
            switch (contract.CustomField)
            {
                case (byte)Enums.ClaimFieldTypes.CustomField1:
                    conditionUdf.PropertyColumnName = Constants.PropertyCustomField1;
                    break;
                case (byte)Enums.ClaimFieldTypes.CustomField2:
                    conditionUdf.PropertyColumnName = Constants.PropertyCustomField2;
                    break;
                case (byte)Enums.ClaimFieldTypes.CustomField3:
                    conditionUdf.PropertyColumnName = Constants.PropertyCustomField3;
                    break;
                case (byte)Enums.ClaimFieldTypes.CustomField4:
                    conditionUdf.PropertyColumnName = Constants.PropertyCustomField4;
                    break;
                case (byte)Enums.ClaimFieldTypes.CustomField5:
                    conditionUdf.PropertyColumnName = Constants.PropertyCustomField5;
                    break;
                case (byte)Enums.ClaimFieldTypes.CustomField6:
                    conditionUdf.PropertyColumnName = Constants.PropertyCustomField6;
                    break;
            }
            conditionUdf.RightOperand = contract.PayerCode;
            conditionUdf.OperandIdentifier = contract.CustomField;
            conditionUdf.OperandType = (byte)Enums.OperandType.AlphaNumeric;
            conditionUdf.ConditionOperator = (byte)Enums.ConditionOperation.EqualTo;
            contract.Conditions.Add(conditionUdf);
        }
    }


}
