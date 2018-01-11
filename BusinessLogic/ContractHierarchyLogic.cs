/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Contract Hierarchical structure
/**  User Story Id  : 	Contract tree structure Figure 9
/** Modification History ************************************************************************************
 *  Date Modified   : 21 Aug 2013
 *  Author          : Vishesh
 *  Summary         : CopyContractByNodeAndParentId,DeleteNodeAndContractByNodeId & GetContractHierarchy Method Updated/Created
/************************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ContractHierarchyLogic
    {

        /// <summary>
        /// The _contract TreeView repository
        /// </summary>
        private readonly IContractHierarchyRepository _contractTreeViewRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractHierarchyLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractHierarchyLogic(string connectionString)
        {
            _contractTreeViewRepository = Factory.CreateInstance<IContractHierarchyRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractHierarchyLogic"/> class.
        /// </summary>
        /// <param name="contractRepository">The contract repository.</param>
        public ContractHierarchyLogic(IContractHierarchyRepository contractRepository)
        {
            if (contractRepository != null)
            {
                _contractTreeViewRepository = contractRepository;
            }
        }


        /// <summary>
        /// Gets the contract hierarchy.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ContractHierarchy> GetContractHierarchy(ContractHierarchy data)
        {
            if (data != null)
            {
                List<ContractHierarchy> contractHierarchies = _contractTreeViewRepository.GetContractHierarchy(data);
                // below logic is only for contracts
                if (contractHierarchies != null && contractHierarchies.Any(a => a.IsContract.HasValue && a.IsContract.Value))
                {
                    contractHierarchies.Where(a => a.IsContract.HasValue && a.IsContract.Value).ToList().ForEach(a => a.Nodes = GetChildContractServiceTypes(contractHierarchies, a.NodeId));
                    // Removeing all the items where ParentId null, that means it has already binded into child nodes.
                    contractHierarchies.RemoveAll(a => !a.ParentId.HasValue);
                }
                return contractHierarchies;
            }
            return null;
        }


        /// <summary>
        /// Gets the child contract service types.
        /// </summary>
        /// <param name="contractHierarchies">The contract hierarchies.</param>
        /// <param name="nodeId">The node unique identifier.</param>
        /// <returns></returns>
        private static List<ContractHierarchy> GetChildContractServiceTypes(List<ContractHierarchy> contractHierarchies, long nodeId)
        {
            if (contractHierarchies.Any(a => a.NodeId == nodeId && !a.IsContract.HasValue))
            {
                return contractHierarchies.Where(a => a.NodeId == nodeId && !a.IsContract.HasValue).OrderBy(a => a.ContractServiceTypeId).ToList();
            }
            return null;
        }


        /// <summary>
        /// Copies the contract by node and parent unique identifier.
        /// </summary>
        /// <param name="moduleToCopy">The module automatic copy.</param>
        /// <returns></returns>
        public long CopyNode(ContractHierarchy moduleToCopy)
        {
            return _contractTreeViewRepository.CopyNode(moduleToCopy);
        }


        /// <summary>
        /// Deletes the node and contract by node unique identifier.
        /// </summary>
        /// <param name="data">The unique identifier.</param>
        /// <returns></returns>
        public bool DeleteNode(ContractHierarchy data)
        {
            if (data != null)
            {
                return _contractTreeViewRepository.DeleteNode(data);
            }
            return false;
        }


        /// <summary>
        /// Copies the contract by contract unique identifier.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public long CopyContract(ContractHierarchy data)
        {
            return _contractTreeViewRepository.CopyContract(data);
        }


        /// <summary>
        /// Deletes the contract service type by unique identifier.
        /// </summary>
        /// <param name="data">The unique identifier.</param>
        /// <returns></returns>
        public bool DeleteContractServiceType(ContractHierarchy data)
        {
            if (data != null)
            {
                return _contractTreeViewRepository.DeleteContractServiceType(data);
            }
            return false;
        }


        /// <summary>
        /// Checks the model name is unique.
        /// </summary>
        /// <param name="contractHierarchy">The contract hierarchy.</param>
        /// <returns></returns>
        public bool IsModelNameExit(ContractHierarchy contractHierarchy)
        {
            return _contractTreeViewRepository.IsModelNameExit(contractHierarchy);
        }
    }
}
