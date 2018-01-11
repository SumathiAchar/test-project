/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Contract Hierarchical structure
/**  User Story Id  : Contract tree structure Figure 9
/** Modification History ************************************************************************************
 *  Date Modified   : 21 Aug 2013
 *  Author          :Vishesh
 *  Summary         :CopyContractByNodeAndParentId,DeleteNodeAndContractByNodeId & GetContractHierarchy Method Updated/Created
/************************************************************************************************************/
using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IContractHierarchyRepository : IDisposable
    {
        /// <summary>
        /// Gets the contract hierarchy.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        List<ContractHierarchy> GetContractHierarchy(ContractHierarchy data);
        /// <summary>
        /// Copies the contract by node and parent id.
        /// </summary>
        /// <param name="moduleToCopy">The module to copy.</param>
        /// <returns></returns>
        long CopyNode(ContractHierarchy moduleToCopy);

        /// <summary>
        /// Copies the contract by contract id.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        long CopyContract(ContractHierarchy data);

        /// <summary>
        /// Deletes the node and contract by node id.
        /// </summary>
        /// <param name="data">The node id.</param>
        /// <returns></returns>
        bool DeleteNode(ContractHierarchy data);

        /// <summary>
        /// Deletes the contract service type by Id.
        /// </summary>
        /// <param name="data">The Id.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteContractServiceType(ContractHierarchy data);

        /// <summary>
        /// Checks the model name is unique.
        /// </summary>
        /// <param name="contractHierarchy">The contractHierarchy.</param>
        /// <returns></returns>
        bool IsModelNameExit(ContractHierarchy contractHierarchy);
    }
}