using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Report
{
    // ReSharper disable once UnusedMember.Global
    public class ReportSelectionController : BaseController
    {
        private readonly ReportSelectionLogic _reportSelectionLogic;

        public ReportSelectionController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _reportSelectionLogic = new ReportSelectionLogic(bubbleDataSource);
        } 
        /// <summary>
        /// Gets all claim fields.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<ClaimField> GetAllClaimFields(ClaimSelector reportClaimSelectorInfo)
        {
            return _reportSelectionLogic.GetAllClaimFields(reportClaimSelectorInfo);
        }
        /// <summary>
        /// Gets all ClaimField Operators.
        /// </summary>
        /// <returns></returns>
        public List<ClaimFieldOperator> GetAllClaimFieldsOperators()
        {
            return _reportSelectionLogic.GetAllClaimFieldsOperators();
        }

        /// <summary>
        /// Gets the claim reviewed option.
        /// </summary>
        /// <returns></returns>
        public List<ReviewedOptionType> GetClaimReviewedOption()
        {
            return _reportSelectionLogic.GetClaimReviewedOption();
        }

        /// <summary>
        /// Gets ajudication request names based on the model id and use rname.
        /// </summary>
        /// <param name="claimSelectione">The claim selectione.</param>
        /// <returns>
        /// List of ClaimSelector
        /// </returns>
        [HttpPost]
        public List<ClaimSelector> GetAdjudicationRequestNames(ClaimSelector claimSelectione)
        {
            return _reportSelectionLogic.GetAdjudicationRequestNames(claimSelectione);
        }

        /// <summary>
        /// Add or edit query name.
        /// </summary>
        /// <param name="claimSelection"></param>
        /// <returns></returns>
        [HttpPost]
        public int AddEditQueryName(ClaimSelector claimSelection)
        {
            return _reportSelectionLogic.AddEditQueryName(claimSelection);
        }

        /// <summary>
        /// Delete query name with criteria.
        /// </summary>
        /// <param name="claimSelector"></param>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteQueryName(ClaimSelector claimSelector)
        {
            return _reportSelectionLogic.DeleteQueryName(claimSelector);
        }


        /// <summary>
        /// Get Quries By Id
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public List<ClaimSelector> GetQueriesById(User userInfo)
        {
            return _reportSelectionLogic.GetQueriesById(userInfo);
        }
    }
}
