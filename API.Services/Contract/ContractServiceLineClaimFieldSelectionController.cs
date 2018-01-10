using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class ContractServiceLineClaimFieldSelectionController : BaseController
    {
        private readonly ServiceLineClaimFieldLogic _serviceLineClaimFieldLogic;

        ContractServiceLineClaimFieldSelectionController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _serviceLineClaimFieldLogic = new ServiceLineClaimFieldLogic(bubbleDataSource);
        }

        //TODO: AddNewClaimFieldSelection AND EditClaimFieldSelection can be combined and read as AddEditClaimFieldSelection
        /// <summary>
        /// Adds the new claim field selection.
        /// </summary>
        /// <param name="claimFieldSelection">The claim field selection.</param>
        public long AddNewClaimFieldSelection(List<ContractServiceLineClaimFieldSelection> claimFieldSelection)
        {
            return _serviceLineClaimFieldLogic.AddNewClaimFieldSelection(claimFieldSelection);
        }

        /// <summary>
        /// Edit the new claim field selection.
        /// </summary>
        /// <param name="claimFieldSelection">The claim field selection.</param>
        public long EditClaimFieldSelection(List<ContractServiceLineClaimFieldSelection> claimFieldSelection)
        {
            return _serviceLineClaimFieldLogic.EditClaimFieldSelection(claimFieldSelection);
        }

        /// <summary>
        /// Gets the new claim field selection.
        /// </summary>
        /// <param name="claimFieldSelection">The claim field selection.</param>
        [HttpPost]
        public List<ContractServiceLineClaimFieldSelection> GetClaimFieldSelection(ContractServiceLineClaimFieldSelection claimFieldSelection)
        {
            return _serviceLineClaimFieldLogic.GetClaimFieldSelection(claimFieldSelection);
        }

    }
}
