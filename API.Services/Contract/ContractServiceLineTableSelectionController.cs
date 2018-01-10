using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class ContractServiceLineTableSelectionController : BaseController
    {
        private readonly ServiceLineTableSelectionLogic _serviceLineTableSelectionDetailsLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeDrgController"/> class.
        /// </summary>
        public ContractServiceLineTableSelectionController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _serviceLineTableSelectionDetailsLogic = new ServiceLineTableSelectionLogic(bubbleDataSource);
        }

        /// <summary>
        /// Adds the service line ClaimField and Tables details.
        /// </summary>
        /// <param name="contract">The contractId and ContractServiceTypeId.</param>
        [HttpPost]
        public List<ClaimField> GetClaimFieldAndTables(ContractServiceLineTableSelection contract)
        {
            return _serviceLineTableSelectionDetailsLogic.GetClaimFieldAndTables(contract);
        }

        /// <summary>
        /// Add Edit ServiceLine ClaimField and Table
        /// </summary>
        /// <param name="serviceLineClaimandTableList">serviceLineClaimandTableList</param>
        /// <returns></returns>
        [HttpPost]
        public long AddEditServiceLineClaimAndTables(List<ContractServiceLineTableSelection> serviceLineClaimandTableList)
        {
            return _serviceLineTableSelectionDetailsLogic.AddEditServiceLineClaimAndTables(serviceLineClaimandTableList);
        }

        //To get all ClaimFields and associated Tables for editing
        [HttpPost]
        public List<ContractServiceLineTableSelection> GetServiceLineTableSelection(ContractServiceLineTableSelection contractServiceLineTableSelection)
        {
            return _serviceLineTableSelectionDetailsLogic.GetServiceLineTableSelection(contractServiceLineTableSelection);

        }

        /// <summary>
        /// Gets the table selection claim field operators.
        /// </summary>
        /// <returns></returns>
       public List<ClaimFieldOperator> GetTableSelectionClaimFieldOperators()
        {
            return _serviceLineTableSelectionDetailsLogic.GetTableSelectionClaimFieldOperators();
        }

    }
}
