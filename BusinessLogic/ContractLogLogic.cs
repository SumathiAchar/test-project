/************************************************************************************************************/
/**  Author         : Manjunath
/**  Created        : 19-Sep-2013
/**  Summary        : Handles insertion of contractlogs information
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// Class for contract Log
    /// </summary>
    public class ContractLogLogic : IContractLogLogic
    {
        /// <summary>
        /// Initializes Repository
        /// </summary>
        private readonly IContractLogRepository _contractLogsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractLogLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractLogLogic(string connectionString)
        {
            _contractLogsRepository = Factory.CreateInstance<IContractLogRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractLogLogic"/> class.
        /// </summary>
        public ContractLogLogic(IContractLogRepository contractLogsRepository)
        {
            _contractLogsRepository = contractLogsRepository;
        }



        /// <summary>
        /// Adds the contract log.
        /// </summary>
        /// <param name="paymentResultDictionary">The payment result dictionary.</param>
        /// <param name="contractList">The contract list.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public bool AddContractLog(Dictionary<long, List<PaymentResult>> paymentResultDictionary, List<Contract> contractList)
        {
            List<ContractLog> contractLogs = GetContractLogList(paymentResultDictionary, contractList);
            return _contractLogsRepository.AddContractLog(contractLogs);
        }

        /// <summary>
        /// Gets the contract log list.
        /// </summary>
        /// <param name="paymentResultDictionary">The payment result dictionary.</param>
        /// <param name="contractList">The contract list.</param>
        /// <returns></returns>
        private static List<ContractLog> GetContractLogList(Dictionary<long, List<PaymentResult>> paymentResultDictionary,
            List<Contract> contractList)
        {
            List<ContractLog> contractLogs = new List<ContractLog>();
            if (paymentResultDictionary != null && paymentResultDictionary.Count > 0)
            {
                foreach (var paymentResultKeyValuePair in paymentResultDictionary.Where(q => q.Value != null))
                {
                    ContractLog contractLog = new ContractLog { ClaimId = paymentResultKeyValuePair.Key };

                    PaymentResult overAllClaimPaymentResult = paymentResultKeyValuePair.Value.FirstOrDefault(
                            payment => payment.Line == null && payment.ServiceTypeId == null);

                    //Get ContractId and Contract Name
                    long? matchedContractId = null;
                    string matchedContractName = string.Empty;
                    Contract matchedContract = null;
                    if (overAllClaimPaymentResult != null)
                    {
                        if (overAllClaimPaymentResult.ContractId.HasValue)
                        {
                            matchedContractId = overAllClaimPaymentResult.ContractId;
                            matchedContract =
                                contractList.FirstOrDefault(
                                    q =>
                                        matchedContractId != null &&
                                        q.ContractId == overAllClaimPaymentResult.ContractId);
                            if (matchedContract != null)
                                matchedContractName = matchedContract.ContractName;
                        }
                        EnumHelper enumHelper = EnumHelperLibrary.GetFieldInfoFromEnum((Enums.AdjudicationOrVarianceStatuses)overAllClaimPaymentResult.ClaimStatus);
                        contractLog.StatusCode = enumHelper.FieldName;
                    }

                    contractLog.ContractId = matchedContractId;
                    contractLog.ContractName = matchedContractName;
                    contractLog.InsertDate = DateTime.UtcNow;
                    contractLogs.Add(contractLog);

                    //Add log for claim level adjudicated data
                    contractLogs.AddRange(GetClaimLevelContractLogList(paymentResultKeyValuePair, contractList,
                        matchedContract));

                    //Add log for line level adjudicated data
                    contractLogs.AddRange(GetLineLevelContractLogList(paymentResultKeyValuePair, matchedContract));

                }
            }
            contractLogs = UpdateLogContent(contractLogs);
            return contractLogs;
        }

        /// <summary>
        /// Gets the claim level contract log list.
        /// </summary>
        /// <param name="paymentResultKeyValuePair">The payment result key value pair.</param>
        /// <param name="contractList">The contract list.</param>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        private static IEnumerable<ContractLog> GetClaimLevelContractLogList(KeyValuePair<long, List<PaymentResult>> paymentResultKeyValuePair,
            List<Contract> contractList, Contract contract)
        {
            List<ContractLog> contractLogs = new List<ContractLog>();

            List<PaymentResult> paymentResults =
                paymentResultKeyValuePair.Value.Where(q => q.Line == null && q.ServiceTypeId.HasValue).ToList();

            if (paymentResults.Any())
            {
                long? contractId = contract != null ? contract.ContractId : (long?)null;
                string contractName = contract != null ? contract.ContractName : string.Empty;

                foreach (PaymentResult paymentResult in paymentResults)
                {
                    ContractLog contractLog = new ContractLog
                    {
                        ClaimId = paymentResult.ClaimId,
                        ContractId = contractId,
                        ContractName = contractName,
                        InsertDate = DateTime.UtcNow
                    };
                    EnumHelper claimLevelEnumHelper = EnumHelperLibrary.GetFieldInfoFromEnum((Enums.AdjudicationOrVarianceStatuses)paymentResult.ClaimStatus);
                    contractLog.StatusCode = claimLevelEnumHelper.FieldName;
                    Contract contractItem = contractList.FirstOrDefault(q => contractId != null && q.ContractId == contractId);
                    if (contractItem != null)
                    {
                        ContractServiceType contractServiceType =
                            contractItem.ContractServiceTypes.FirstOrDefault(
                                a =>
                                    paymentResult.ServiceTypeId != null &&
                                    a.ContractServiceTypeId == paymentResult.ServiceTypeId.Value);
                        if (contractServiceType != null)
                        {
                            contractLog.ServiceTypeName = contractServiceType.ContractServiceTypeName;
                        }
                    }
                    if (paymentResult.AdjudicatedValue.HasValue &&
                        paymentResult.PaymentTypeId.HasValue)
                    {
                        EnumHelper enumHelperPaymentType =
                            EnumHelperLibrary.GetFieldInfoFromEnum((Enums.PaymentTypeCodes)paymentResult.PaymentTypeId.Value);
                        contractLog.PaymentType = enumHelperPaymentType.FieldName;
                    }
                    contractLogs.Add(contractLog);
                }
            }
            return contractLogs;
        }

        /// <summary>
        /// Gets the line level contract log list.
        /// </summary>
        /// <param name="paymentResultKeyValuePair">The payment result key value pair.</param>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        private static IEnumerable<ContractLog> GetLineLevelContractLogList(KeyValuePair<long, List<PaymentResult>> paymentResultKeyValuePair, Contract contract)
        {
            List<ContractLog> contractLogs = new List<ContractLog>();

            List<PaymentResult> paymentResults =
                paymentResultKeyValuePair.Value.Where(q => q.Line != null).ToList();

            if (paymentResults.Any())
            {
                long? contractId = contract != null ? contract.ContractId : (long?)null;
                string contractName = contract != null ? contract.ContractName : string.Empty;

                foreach (PaymentResult paymentResult in paymentResults)
                {
                    ContractLog contractLog = new ContractLog
                    {
                        ClaimId = paymentResult.ClaimId,
                        ContractId = contractId,
                        ContractName = contractName,
                        ServiceLine = paymentResult.Line
                    };

                    EnumHelper enumHelperClaimCharge = EnumHelperLibrary.GetFieldInfoFromEnum((Enums.AdjudicationOrVarianceStatuses)paymentResult.ClaimStatus);
                    contractLog.StatusCode = enumHelperClaimCharge.FieldName;

                    if (contract != null && paymentResult.ServiceTypeId.HasValue)
                    {
                        ContractServiceType contractServiceType =
                            contract.ContractServiceTypes.FirstOrDefault(
                                q => q.ContractServiceTypeId == paymentResult.ServiceTypeId);
                        if (contractServiceType != null)
                        {
                            contractLog.ServiceTypeName = contractServiceType.ContractServiceTypeName;

                            contractLogs.Add(GetLineItemFoundLog(paymentResult, contractId,
                                contractName, contractServiceType.ContractServiceTypeName));

                            if (paymentResult.PaymentTypeId.HasValue)
                            {
                                EnumHelper enumHelperPaymentType = EnumHelperLibrary.GetFieldInfoFromEnum((Enums.PaymentTypeCodes)paymentResult.PaymentTypeId.Value);
                                contractLog.PaymentType = enumHelperPaymentType.FieldName;
                            }
                        }
                    }
                    contractLog.InsertDate = DateTime.UtcNow;
                    contractLogs.Add(contractLog);
                }
            }
            return contractLogs;
        }

        /// <summary>
        /// Gets the line item found log.
        /// </summary>
        /// <param name="lineLevelPaymentResult">The line level payment result.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="contractName">Name of the contract.</param>
        /// <param name="serviceTypeName">Name of the service type.</param>
        /// <returns></returns>
        private static ContractLog GetLineItemFoundLog(PaymentResult lineLevelPaymentResult, long? contractId, string contractName, string serviceTypeName)
        {
            EnumHelper enumHelperClaimChargeLineItemFound = EnumHelperLibrary.GetFieldInfoFromEnum(Enums.AdjudicationOrVarianceStatuses.LineItemMatchesAServiceLine);

            ContractLog contractLog = new ContractLog
            {
                ClaimId = lineLevelPaymentResult.ClaimId,
                ServiceLine = lineLevelPaymentResult.Line,
                StatusCode = enumHelperClaimChargeLineItemFound.FieldName,
                ContractId = contractId,
                ContractName = contractName,
                ServiceTypeName = serviceTypeName,
                InsertDate = DateTime.UtcNow,
            };
            return contractLog;
        }

        /// <summary>
        /// Updates the content of the log.
        /// </summary>
        /// <param name="contractLogs">The contract log list.</param>
        /// <returns></returns>
        private static List<ContractLog> UpdateLogContent(List<ContractLog> contractLogs)
        {
            contractLogs.ForEach(
                contractLog =>
                    contractLog.LogContent =
                        string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{0}{3}{2}{0}{4}{2}{0}{5}{2}{0}{6}{2}{0}{7}{2}{0}{2}{0}{8}{2}", '<',
                            Convert.ToString(contractLog.ClaimId, CultureInfo.InvariantCulture), '>', contractLog.StatusCode, contractLog.ContractName,
                            contractLog.ServiceTypeName, Convert.ToString(contractLog.ServiceLine, CultureInfo.InvariantCulture),
                            contractLog.PaymentType,
                            Convert.ToString(DateTime.UtcNow, CultureInfo.InvariantCulture)));

            return contractLogs;
        }

    }
}
