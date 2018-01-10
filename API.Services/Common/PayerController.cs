using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Common
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class PayerController : BaseController
    {
        /// <summary>
        /// The _payer logic
        /// </summary>
        private readonly PayerLogic _payerLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PayerController"/> class.
        /// </summary>
        public PayerController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId);
            _payerLogic = new PayerLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets the payers.
        /// </summary>
        /// <param name="selectedFacility">The selected facility.</param>
        /// <returns></returns>
        [HttpPost]
        public List<Payer> GetPayers(ContractServiceLineClaimFieldSelection selectedFacility)
        {

            return _payerLogic.GetPayers(selectedFacility);
            
        }
    }
}