using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.PaymentTable
{
    // ReSharper disable once UnusedMember.Global
    public class ClaimFieldDocController : BaseController
    {
        private readonly ClaimFieldDocLogic _claimFieldDocsLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimFieldDocController"/> class.
        /// </summary>
        public ClaimFieldDocController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _claimFieldDocsLogic = new ClaimFieldDocLogic(bubbleDataSource);
        }

        /// <summary>
        /// Adds the claim field docs.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        [HttpPost]
        public long AddClaimFieldDocs(ClaimFieldDoc claimFieldDoc)
        {
            return _claimFieldDocsLogic.AddClaimFieldDocs(claimFieldDoc);
        }


        /// <summary>
        /// Gets the claim field docs.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        [HttpPost]
        public List<ClaimFieldDoc> GetClaimFieldDocs(ClaimFieldDoc claimFieldDoc)
        {
            return _claimFieldDocsLogic.GetClaimFieldDocs(claimFieldDoc);
        }

        /// <summary>
        /// Gets all claim fields.
        /// </summary>
        /// <returns></returns>
        public List<ClaimField> GetAllClaimFields()
        {
            return _claimFieldDocsLogic.GetAllClaimFields();
        }

        /// <summary>
        /// Deletes the specified claim field document.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        [HttpPost]
        public bool Delete(ClaimFieldDoc claimFieldDoc)
        {
            return _claimFieldDocsLogic.Delete(claimFieldDoc);
        }

        /// <summary>
        /// Determines whether [is document in use] [the specified claim field document].
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        [HttpPost]
        public List<ContractLog> IsDocumentInUse(ClaimFieldDoc claimFieldDoc)
        {
            return _claimFieldDocsLogic.IsDocumentInUse(claimFieldDoc);
        }

        /// <summary>
        /// Rename Payment Table.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        [HttpPost]
        public ClaimFieldDoc RenamePaymentTable(ClaimFieldDoc claimFieldDoc)
        {
            return _claimFieldDocsLogic.RenamePaymentTable(claimFieldDoc);
        }
    }
}

