using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Common
{
    // ReSharper disable once UnusedMember.Global
    public class RtaEdiResponseController : BaseController
    {
        /// <summary>
        /// The _rta edi response logic
        /// </summary>
        private readonly RtaEdiResponseLogic _rtaEdiResponseLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="RtaEdiResponseController"/> class.
        /// </summary>
        public RtaEdiResponseController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId);
            _rtaEdiResponseLogic = new RtaEdiResponseLogic(bubbleDataSource);
        }

        /// <summary>
        /// Saves the specified rta edi response.
        /// </summary>
        /// <param name="rtaEdiResponse">The rta edi response.</param>
        /// <returns></returns>
        [HttpPost]
        public long Save(RtaEdiResponse rtaEdiResponse)
        {
            return _rtaEdiResponseLogic.Save(rtaEdiResponse);
        }
    }
}
