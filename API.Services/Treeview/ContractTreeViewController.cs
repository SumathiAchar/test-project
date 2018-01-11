/************************************************************************************************************/
/**  Author         : 
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Contract Hierarchical structure
/**  User Story Id  : 	Contract tree structure Figure 9
/** Modification History ************************************************************************************
 *  Date Modified   : 21 Aug 2013
 *  Author          : Vishesh
 *  Summary         : CopyContractByNodeAndParentId,DeleteNodeAndContractByNodeId & GetContractHierarchy Method Updated/Created
/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Treeview
{
    // ReSharper disable once UnusedMember.Global
    public class ContractTreeViewController : BaseController
    {
        private readonly ContractHierarchyLogic _contractLogic;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ContractTreeViewController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _contractLogic = new ContractHierarchyLogic(bubbleDataSource);
        }


        /// <summary>
        /// Gets the contract hierarchy.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        public List<ContractHierarchy> GetContractHierarchy(ContractHierarchy data)
        {
            return _contractLogic.GetContractHierarchy(data);
        }


        /// <summary>
        /// Copies the contract by node and parent identifier.
        /// </summary>
        /// <param name="moduleToCopy">The module to copy.</param>
        /// <returns></returns>
        [HttpPost]
        public long CopyContractByNodeAndParentId(ContractHierarchy moduleToCopy)
        {
            return _contractLogic.CopyNode(moduleToCopy);
        }


        /// <summary>
        /// Deletes the node and contract by node id.
        /// </summary>
        /// <param name="data">The node id.</param>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteNodeAndContractByNodeId(ContractHierarchy data)
        {
            return _contractLogic.DeleteNode(data);
        }

        /// <summary>
        /// Copies the contract by contract id.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        public long CopyContractByContractId(ContractHierarchy data)
        {
            return _contractLogic.CopyContract(data);
        }

        /// <summary>
        /// Deletes the contract service type by Id.
        /// </summary>
        /// <param name="data">The Id.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [HttpPost]
        public bool DeleteContractServiceTypeById(ContractHierarchy data)
        {
            return _contractLogic.DeleteContractServiceType(data);
        }


        /// <summary>
        /// Checks the model name is unique.
        /// </summary>
        /// <param name="contractHierarchy">The contract hierarchy.</param>
        /// <returns></returns>
        [HttpPost]
        public bool IsModelNameExit(ContractHierarchy contractHierarchy)
        {
            return _contractLogic.IsModelNameExit(contractHierarchy);
        }
    }
}
