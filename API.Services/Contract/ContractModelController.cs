using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class ContractModelController : BaseController
    {
        private readonly SelectContractModelLogic _selectContractModelLogic;
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractModelController"/> class.
        /// </summary>
        public ContractModelController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _selectContractModelLogic = new SelectContractModelLogic(bubbleDataSource);
        }


        /// <summary>
        /// Gets all contract models.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        public List<ContractModel> GetAllContractModels(ContractModel data)
        {
            return _selectContractModelLogic.GetAllContractModels(data);
        }

        /// <summary>
        /// Get All Contract Facilities
        /// </summary>
        /// <returns>List of Contract Facilities </returns>
        [HttpPost]
        public List<FacilityDetail> GetAllContractFacilities(ContractHierarchy contractHierarchy)
        {
            return _selectContractModelLogic.GetAllContractFacilities(contractHierarchy);
        }
    }
}
