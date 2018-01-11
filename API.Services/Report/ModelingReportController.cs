using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Report
{
    // ReSharper disable once UnusedMember.Global
    public class ModelingReportController : BaseController
    {

        private readonly ModelingReportLogic _modelingReportLogic;


        /// <summary>
        /// Initializes a new instance of the <see cref="ModelingReportController"/> class.
        /// </summary>
        public ModelingReportController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _modelingReportLogic = new ModelingReportLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets all modeling details.
        /// </summary> 
        /// <param name="modelingReport">The modeling report.</param>
        /// <returns></returns>
        [HttpPost]
        public ModelingReport GetAllModelingDetails(ModelingReport modelingReport)
        {
            return _modelingReportLogic.GetAllModelingDetails(modelingReport);

        }
    }
}
