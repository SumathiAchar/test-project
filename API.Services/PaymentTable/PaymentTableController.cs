using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.PaymentTable
{
    // ReSharper disable once UnusedMember.Global
    public class PaymentTableController : BaseController
    {
        /// <summary>
        /// The _payment table logic
        /// </summary>
        private readonly PaymentTableLogic _paymentTableLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTableController"/> class.
        /// </summary>
        public PaymentTableController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _paymentTableLogic = new PaymentTableLogic(bubbleDataSource);
        }

        /// <summary>
        /// Determines whether [is table name exists] [the specified data].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        public bool IsTableNameExists(ClaimFieldDoc data)
        {
            return _paymentTableLogic.IsTableNameExists(data);
        }

        /// <summary>
        /// Gets the payment table.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTableContainer GetPaymentTable(ClaimFieldDoc claimFieldDoc)
        {
            return _paymentTableLogic.GetPaymentTable(claimFieldDoc);
        }

    }
}