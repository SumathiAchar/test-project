using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class ServiceLineCodeController : BaseController
    {
        private readonly ServiceLineCodeLogic _serviceLineCodeDetailsLogic;

        public ServiceLineCodeController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _serviceLineCodeDetailsLogic = new ServiceLineCodeLogic(bubbleDataSource);
        }

        /// <summary>
        /// Adds the service line code details.
        /// </summary>
        /// <param name="contractServiceLine">The contract service line list.</param>
        public long AddEditServiceLineCodeDetails(ContractServiceLine contractServiceLine)
        {
            return _serviceLineCodeDetailsLogic.AddEditServiceLineCodeDetails(contractServiceLine);
        }

        /// <summary>
        /// Adds the service line code details.
        /// </summary>
        /// <param name="contractServiceLine">The contract service line list.</param>
        [HttpPost]
        public ContractServiceLine GetServiceLineCodeDetails(ContractServiceLine contractServiceLine)
        {
            return _serviceLineCodeDetailsLogic.GetServiceLineCodeDetails(contractServiceLine);
        }

        /// <summary>
        /// Gets all service line code details by contract id.
        /// </summary>
        /// <param name="contractId">The contract id.</param>
        /// <returns></returns>
        public List<ContractServiceLine> GetAllServiceLineCodeDetailsByContractId(long contractId)
        {
            return _serviceLineCodeDetailsLogic.GetAllServiceLineCodeDetailsByContractId(contractId);
        }

        /// <summary>
        /// Gets all ServiceLine Codes.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<ServiceLineCode> GetAllServiceLineCodes(ContractServiceLine input)
        {
            //TODO: From the controller we are sending an Nullable type, then till business & data layer same type should be send.
            // From controller we are sending long? and in business & data layer in signature is GetAllServiceLineCodes(long id )
            if (input != null)
                if (input.ServiceLineId != null)
                    return _serviceLineCodeDetailsLogic.GetAllServiceLineCodes(input.ServiceLineId.Value, input.PageSize, input.PageIndex);
            return null;
        }
    }
}
