using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Common
{
    // ReSharper disable once UnusedMember.Global
    public class RtaController : BaseController
    {
        /// <summary>
        /// The _rta edi request logic
        /// </summary>
        private readonly RtaLogic _rtaLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="RtaController"/> class.
        /// </summary>
        public RtaController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId);
            _rtaLogic = new RtaLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets the date rta data.
        /// </summary>
        /// <param name="evaluateableClaim">The evaluateable claim.</param>
        /// <returns></returns>
        [HttpPost]
        public RtaData GetRtaData(EvaluateableClaim evaluateableClaim)
        {
            return _rtaLogic.GetRtaDataByClaim(evaluateableClaim);
        }

        /// <summary>
        /// Adjudicates the specified rta data.
        /// </summary>
        /// <param name="rtaData">The rta data.</param>
        /// <returns></returns>
        [HttpPost]
        public Dictionary<long, List<PaymentResult>> Adjudicate(RtaData rtaData)
        {
            return _rtaLogic.AdjudicateClaimData(new List<EvaluateableClaim> { rtaData.EvaluateableClaim}, rtaData.Contracts);
        }

        /// <summary>
        /// Saves the time log.
        /// </summary>
        /// <param name="rtaEdiTimeLog">The rta edi time log.</param>
        /// <returns></returns>
        [HttpPost]
        public long SaveTimeLog(RtaEdiTimeLog rtaEdiTimeLog)
        {
            return _rtaLogic.SaveTimeLog(rtaEdiTimeLog);
        }
    }
}
