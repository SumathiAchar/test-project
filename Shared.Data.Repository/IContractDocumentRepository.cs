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

using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IContractDocumentRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit contract docs.
        /// </summary>
        /// <param name="contractDocs">The contract docs.</param>
        /// <returns></returns>
        ContractDoc AddEditContractDocs(ContractDoc contractDocs);

        /// <summary>
        /// Deletes the contract document by unique identifier.
        /// </summary>
        /// <param name="contractDocs">The contract document unique identifier.</param>
        /// <returns></returns>
        bool DeleteContractDoc(ContractDoc contractDocs);

        /// <summary>
        /// Gets the contract document by unique identifier.
        /// </summary>
        /// <param name="contractDocId">The contract document unique identifier.</param>
        /// <returns></returns>
        ContractDoc GetContractDocById(long contractDocId);
    }
}
