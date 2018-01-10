using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Common
{
    // ReSharper disable once UnusedMember.Global
    public class RtaEdiRequestController : BaseController
    {
        /// <summary>
        /// The _rta edi request logic
        /// </summary>
        private readonly RtaEdiRequestLogic _rtaEdiRequestLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="RtaEdiRequestController"/> class.
        /// </summary>
        public RtaEdiRequestController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId);
            _rtaEdiRequestLogic = new RtaEdiRequestLogic(bubbleDataSource);
        }

        /// <summary>
        /// Saves the specified rta edi request.
        /// </summary>
        /// <param name="rtaEdiRequest">The rta edi request.</param>
        /// <returns></returns>
        [HttpPost]
        public long Save(RtaEdiRequest rtaEdiRequest)
        {
            return _rtaEdiRequestLogic.Save(rtaEdiRequest);
        }
    }
}
