/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Contract payers info , it add/modify multiple instances of payer contact information
/**  User Story Id  : 5.User Story Add a new contract Figure 11
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ContractPayerInfoLogic
    {
        /// <summary>
        /// Initializes Repository
        /// </summary>
        private readonly IContractPayerInfoRepository _contractInfoRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractPayerInfoLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractPayerInfoLogic(string connectionString)
        {
            _contractInfoRepository = Factory.CreateInstance<IContractPayerInfoRepository>(connectionString, true);
        }

        /// <summary>
        /// parametrized constructor
        /// </summary>
        /// <param name="contractRepository"></param>
        public ContractPayerInfoLogic(IContractPayerInfoRepository contractRepository)
        {
            if (contractRepository != null)
            {
                _contractInfoRepository = contractRepository;
            }
        }

        /// <summary>
        /// Get Contract Payer Info data
        /// </summary>
        /// <param name="contractPayerInfoId"></param>
        /// <returns></returns>
        public ContractPayerInfo GetContractPayerInfo(long contractPayerInfoId)
        {
            ContractPayerInfo contractPayerInfo = _contractInfoRepository.GetContractPayerInfo(contractPayerInfoId);
            return contractPayerInfo;
        }

        /// <summary>
        /// Add & Edit Contract Payer Info data
        /// </summary>
        /// <param name="contractPayerInfo"></param>
        /// <returns></returns>
        public long AddContractPayerInfo(ContractPayerInfo contractPayerInfo)
        {
            return _contractInfoRepository.AddEditContractPayerInfo(contractPayerInfo);
        }

        /// <summary>
        /// Delete Contract Payer Info By ID
        /// </summary>
        /// <param name="contractPayerInfo"></param>
        /// <returns></returns>
        public bool DeleteContractPayerInfo(ContractPayerInfo contractPayerInfo)
        {
            return _contractInfoRepository.DeleteContractPayerInfo(contractPayerInfo);
        }
    }
}
