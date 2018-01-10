using System;
using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Common
{
    // ReSharper disable once UnusedMember.Global
    public class ClaimFieldController : BaseController
    {
        private readonly ClaimFieldLogic _claimFieldLogic;

        private ClaimFieldController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId);
            _claimFieldLogic = new ClaimFieldLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets the claim fields by module.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public List<ClaimField> GetClaimFieldsByModule(int id)
        {
            return _claimFieldLogic.GetClaimFieldsByModule(id);
        }
    }
}