using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{

    // ReSharper disable once UnusedMember.Global
    public class ContractModifiedReasonController : BaseController
    {
        /// <summary>
        /// The _contract logic
        /// </summary>
        private readonly ContractLogic _contractLogic;
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractModifiedReasonController"/> class.
        /// </summary>
        public ContractModifiedReasonController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _contractLogic = new ContractLogic(bubbleDataSource);
        }
        /// <summary>
        /// Adds the modified reason.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <returns></returns>
        [HttpPost]
        public int AddModifiedReason(ContractModifiedReason reason)
        {
            return _contractLogic.AddContractModifiedReason(reason);
        }
    }
}
