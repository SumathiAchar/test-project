using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Report
{
    // ReSharper disable once UnusedMember.Global
    public class AppealLetterController : BaseController
    {
        /// <summary>
        /// The _appeal letter logic
        /// </summary>
        private readonly AppealLetterLogic _appealLetterLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppealLetterController"/> class.
        /// </summary>
        public AppealLetterController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _appealLetterLogic = new AppealLetterLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets the appeal letter.
        /// </summary>
        /// <param name="appealLetter">The appeal letter container.</param>
        /// <returns></returns>
        [HttpPost]
        public AppealLetter GetAppealLetter(AppealLetter appealLetter)
        {
            return _appealLetterLogic.GetAppealLetter(appealLetter);
        }
        
        /// <summary>
        /// Gets the appeal templates.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<LetterTemplate> GetAppealTemplates(LetterTemplate appealLetterInfo)
        {
            return _appealLetterLogic.GetAppealTemplates(appealLetterInfo);
        }
    }
}
