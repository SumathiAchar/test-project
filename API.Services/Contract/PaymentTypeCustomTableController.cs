using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class PaymentTypeCustomTableController : BaseController
    {
        private readonly PaymentTypeCustomTableLogic _paymentTypeCustomTableLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeCustomTableController"/> class.
        /// </summary>
        public PaymentTypeCustomTableController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _paymentTypeCustomTableLogic = new PaymentTypeCustomTableLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetHeaders(long id)
        {
            return _paymentTypeCustomTableLogic.GetHeaders(id);
        }

        /// <summary>
        /// Adds the edit.
        /// </summary>
        /// <param name="paymentTypeCustomTable">The payment type custom table.</param>
        /// <returns></returns>
        [HttpPost]
        public long AddEdit(PaymentTypeCustomTable paymentTypeCustomTable)
        {
            return _paymentTypeCustomTableLogic.AddEditPaymentType(paymentTypeCustomTable);
        }

        /// <summary>
        /// Gets the payment type custom table.
        /// </summary>
        /// <param name="paymentTypeCustomTable">The payment type custom table.</param>
        /// <returns></returns>
        [HttpPost]
        public PaymentTypeCustomTable GetPaymentTypeCustomTable(PaymentTypeCustomTable paymentTypeCustomTable)
        {
            return (PaymentTypeCustomTable)_paymentTypeCustomTableLogic.GetPaymentType(paymentTypeCustomTable);
        }
    }
}
