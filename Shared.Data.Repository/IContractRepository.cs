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
using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IContractRepository : IDisposable
    {


        /// <summary>
        /// Gets the contract full information.
        /// </summary>
        /// <param name="contractInfo">The contract information.</param>
        /// <returns></returns>
        ContractFullInfo GetContractFullInfo(Contract contractInfo);

        /// <summary>
        /// Adds the edit contract basic information.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        Contract AddEditContractBasicInfo(Contract contract);

        /// <summary>
        /// Copies the contract by unique identifier.
        /// </summary>
        /// <param name="contracts">The original contract unique identifier.</param>
        /// <returns></returns>
        long CopyContract(Contract contracts);

        /// <summary>
        /// Renames the contract.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="nodeText">The node text.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        ContractHierarchy RenameContract(long nodeId, string nodeText, string userName);

        /// <summary>
        /// Adds the contract modified reason.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <returns></returns>
        int AddContractModifiedReason(ContractModifiedReason reason);

        /// <summary>
        /// Gets the contract first level details.
        /// </summary>
        /// <param name="contractid">The contractid.</param>
        /// <returns></returns>
        Contract GetContractFirstLevelDetails(long? contractid);

        /// <summary>
        /// Gets the contracts.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="isMedicareIpAcuteEnabled">if set to <c>true</c> [is medicare ip acute enabled].</param>
        /// <param name="isMedicareOpApcEnabled">if set to <c>true</c> [is medicare op apc enabled].</param>
        /// <returns></returns>
        List<Contract> GetContracts(long taskId, bool isMedicareIpAcuteEnabled, bool isMedicareOpApcEnabled);

        /// <summary>
        /// Checks the contract name is unique.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        bool IsContractNameExist(Contract contract);

        /// <summary>
        /// Gets the adjudicated contract names.
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        List<Contract> GetAdjudicatedContracts(Contract contract);
    }
}
