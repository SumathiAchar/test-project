using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class ContractServiceTypeController : BaseController
    {
        private readonly ContractServiceTypeLogic _serviceTypeDetailsLogic;
        
        ContractServiceTypeController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _serviceTypeDetailsLogic = new ContractServiceTypeLogic(bubbleDataSource);

        }

        /// <summary>
        /// Returns ContractServiceType list
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// list of ContractServiceTypes object
        /// </returns>
        public List<ContractServiceType> GetAllContractServiceType(long id)
        {
            return _serviceTypeDetailsLogic.GetAllContractServiceType(id);
        }

        /// <summary>
        /// Add/Edit ContractServiceType based on passed ContractServiceTypes object
        /// </summary>
        /// <param name="contractServiceTypes">ContractServiceTypes</param>
        /// <returns>Inserted/Updated ContractServiceTypeId</returns>
        [HttpPost]
        public long AddEditContractServiceType(ContractServiceType contractServiceTypes)
        {
            return _serviceTypeDetailsLogic.AddEditContractServiceType(contractServiceTypes);
        }

        /// <summary>
        /// Copies the contract service type by id.
        /// </summary>
        /// <param name="moduleToCopy">The module to copy.</param>
        /// <returns></returns>
        [HttpPost]
        public long CopyContractServiceTypeById(ContractServiceType moduleToCopy)
        {
            return _serviceTypeDetailsLogic.CopyContractServiceType(moduleToCopy);
        }

        /// <summary>
        /// Rename the contract service type by Id.
        /// </summary>
        /// <param name="data">The Id.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [HttpPost]
        public long RenameContractServiceType(ContractServiceType data)
        {
            return data != null ? _serviceTypeDetailsLogic.RenameContractServiceType(data) : 0;
        }

        /// <summary>
        /// Gets the contract service type details.
        /// </summary>
        /// <param name="contractServiceType">Type of the contract service.</param>
        /// <returns></returns>
        [HttpPost]
        public ContractServiceType GetContractServiceTypeDetails(ContractServiceType contractServiceType)
        {
            return _serviceTypeDetailsLogic.GetContractServiceTypeDetails(contractServiceType);
        }


        /// <summary>
        /// Checks the contract service type name is unique.
        /// </summary>
        /// <param name="contractServiceTypes">The contract service types.</param>
        /// <returns></returns>
        [HttpPost]
        public bool IsContractServiceTypeNameExit(ContractServiceType contractServiceTypes)
        {
            return _serviceTypeDetailsLogic.IsContractServiceTypeNameExit(contractServiceTypes);
        }
    }
}
