/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 12-Aug-2013
/**  Summary        : Handles documents added , delete or view related to Contract
/**  User Story Id  : 5.User Story Add a new contract Figure 13
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
    public class ContractDocumentLogic
    {
        /// <summary>
        /// Initializes Repository
        /// </summary>
        private readonly IContractDocumentRepository _contractDocumentsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractDocumentLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractDocumentLogic(string connectionString)
        {
            _contractDocumentsRepository = Factory.CreateInstance<IContractDocumentRepository>(connectionString, true);
        }

        /// <summary>
        /// parametrized constructor
        /// </summary>
        /// <param name="contractRepository"></param>
        public ContractDocumentLogic(IContractDocumentRepository contractRepository)
        {
            if (contractRepository != null)
            {
                _contractDocumentsRepository = contractRepository;
            }
        }


        /// <summary>
        /// Adds the edit contract docs.
        /// </summary>
        /// <param name="contractDocs">The contract docs.</param>
        /// <returns></returns>
        public ContractDoc AddEditContractDocs(ContractDoc contractDocs)
        {
            return _contractDocumentsRepository.AddEditContractDocs(contractDocs);
        }

        /// <summary>
        /// Delete the ContractDocument
        /// </summary>
        /// <param name="contractDocs"></param>
        /// <returns></returns>
        public bool DeleteContractDoc(ContractDoc contractDocs)
        {
            return _contractDocumentsRepository.DeleteContractDoc(contractDocs);
        }

        /// <summary>
        /// Get Contract full details
        /// </summary>
        /// <param name="contractDocId"></param>
        /// <returns></returns>
        public ContractDoc GetContractDocById(long contractDocId)
        {
            ContractDoc contractDoc = _contractDocumentsRepository.GetContractDocById(contractDocId);
            return contractDoc;
        }
    }
}
