using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Report
{
    // ReSharper disable once UnusedMember.Global
    public class ContractPayerMapReportController : BaseController
    {

        private readonly ContractPayerMapReportLogic _payerMappingReportLogic;


        /// <summary>
        /// Initializes a new instance of the <see cref="ModelingReportController"/> class.
        /// </summary>
        public ContractPayerMapReportController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _payerMappingReportLogic = new ContractPayerMapReportLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets all modeling details.
        /// </summary> 
        /// <param name="contractPayerMapReport">The modeling report.</param>
        /// <returns></returns>
        [HttpPost]
        public ContractPayerMapReport Get(ContractPayerMapReport contractPayerMapReport)
        {
            return _payerMappingReportLogic.Get(contractPayerMapReport);

        }
    }
}
