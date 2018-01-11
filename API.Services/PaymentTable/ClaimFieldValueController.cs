using System;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.PaymentTable
{
    // ReSharper disable once UnusedMember.Global
    public class ClaimFieldValueController : BaseController
    {
        private readonly ClaimFieldValueLogic _claimFieldValuesLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimFieldValueController"/> class.
        /// </summary>
        public ClaimFieldValueController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _claimFieldValuesLogic = new ClaimFieldValueLogic(bubbleDataSource);
        }

        /// <summary>
        /// Adds the claim field values.
        /// </summary>
        /// <param name="claimFieldValues">The claim field values.</param>
        /// <returns></returns>
        public long AddClaimFieldValues(ClaimFieldValue claimFieldValues)
        {
            return _claimFieldValuesLogic.AddClaimFieldValues(claimFieldValues);
        }
    }
}
