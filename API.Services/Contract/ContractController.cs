/************************************************************************************************************/
/**  Author         : 
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
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class ContractController : BaseController
    {
        readonly ContractLogic _contractLogic;

        public ContractController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId);
            _contractLogic = new ContractLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets the contract full information.
        /// </summary>
        /// <param name="contractInfo">The contract information.</param>
        /// <returns></returns>
        [HttpPost]
        public ContractFullInfo GetContractFullInfo(Shared.Models.Contract contractInfo)
        {
            return _contractLogic.GetContractFullInfo(contractInfo);
        }


        /// <summary>
        /// This method will create once copy contract by using original contract and return newly created contract id
        /// </summary>
        /// <param name="contracts">originalContractId</param>
        /// <returns>newly created contract id</returns>
        [HttpPost]
        public long CopyContractById(Shared.Models.Contract contracts)
        {
            return _contractLogic.CopyContract(contracts);
        }

        /// <summary>
        /// Renames the Contract
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [HttpPost]
        public ContractHierarchy RenameContract(ContractHierarchy node)
        {
            if (node != null)
                return _contractLogic.RenameContract(node.NodeId, node.NodeText, node.UserName);

            return null;
        }

        /// <summary>
        /// This method will add/edit contract info in database and return inserted/updated id
        /// </summary>
        /// <param name="contract">Contracts object</param>
        /// <returns>inserted/updated ContractId</returns>
        [HttpPost]
        public Shared.Models.Contract AddEditContractBasicInfo(Shared.Models.Contract contract)
        {
            return _contractLogic.AddEditContractBasicInfo(contract);
        }

        /// <summary>
        /// Get Contract FirstLevel Details
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Shared.Models.Contract GetContractFirstLevelDetails(long? id)
        {
            return _contractLogic.GetContractFirstLevelDetails(id);
        }


        /// <summary>
        /// Checks the contract name is unique.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        [HttpPost]
        public bool IsContractNameExist(Shared.Models.Contract contract)
        {
            return _contractLogic.IsContractNameExist(contract);
        }

        /// <summary>
        /// Sample method to test ApiController Get
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            return "TestSucceded";
        }

        /// <summary>
        /// Gets the adjudicated contract names.
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        [HttpPost]
        public List<Shared.Models.Contract> GetAdjudicatedContracts(Shared.Models.Contract contract)
        {
            return _contractLogic.GetAdjudicatedContracts(contract);
        }

    }
}
