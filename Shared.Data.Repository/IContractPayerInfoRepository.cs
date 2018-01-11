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
using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IContractPayerInfoRepository : IDisposable
    {

        /// <summary>
        /// Gets the contract payer information.
        /// </summary>
        /// <param name="contractPayerInfoId">The contract payer information unique identifier.</param>
        /// <returns></returns>
        ContractPayerInfo GetContractPayerInfo(long contractPayerInfoId);

        /// <summary>
        /// Adds the edit contract payer information.
        /// </summary>
        /// <param name="contractPayerInfo">The contract payer information.</param>
        /// <returns></returns>
        long AddEditContractPayerInfo(ContractPayerInfo contractPayerInfo);

        /// <summary>
        /// Deletes the contract payer information by unique identifier.
        /// </summary>
        /// <param name="contractPayerInfo">The contract payer information unique identifier.</param>
        /// <returns></returns>
        /// 
        bool DeleteContractPayerInfo(ContractPayerInfo contractPayerInfo);
    }
}
