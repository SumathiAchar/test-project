using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class ContractServiceLinePaymentTypesController : BaseController
    {
        /// <summary>
        /// The contract service line payment types logic
        /// </summary>
        private readonly ContractServiceLinePaymentTypeLogic _contractServiceLinePaymentTypesLogic;


        /// <summary>
        /// Prevents a default instance of the <see cref="ContractServiceLinePaymentTypesController"/> class from being created.
        /// </summary>
        ContractServiceLinePaymentTypesController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _contractServiceLinePaymentTypesLogic = new ContractServiceLinePaymentTypeLogic(bubbleDataSource);
        }


        /// <summary>
        /// Deletes the contract service calculate inesand payment types by unique identifier.
        /// </summary>
        /// <param name="contractServiceLinePaymentTypes">The contract service line payment types.</param>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteContractServiceLInesandPaymentTypesById(ContractServiceLinePaymentType contractServiceLinePaymentTypes)
        {
            return _contractServiceLinePaymentTypesLogic.DeleteContractServiceLinesAndPaymentTypes(contractServiceLinePaymentTypes);
        }
    }
}
