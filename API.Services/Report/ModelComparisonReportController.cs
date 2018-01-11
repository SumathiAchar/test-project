using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Report
{
    // ReSharper disable once UnusedMember.Global
    public class ModelComparisonReportController : BaseController
    {

        /// <summary>
        /// The _model comparison report logic
        /// </summary>
        private readonly ModelComparisonReportLogic _modelComparisonReportLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelComparisonReportController"/> class.
        /// </summary>
        public ModelComparisonReportController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _modelComparisonReportLogic = new ModelComparisonReportLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets the available models.
        /// </summary>
        /// <param name="modelComparisonForPost">The model comparison for post.</param>
        /// <returns></returns>
        [HttpPost]
        public List<ModelComparisonReport> GetModels(ModelComparisonReport modelComparisonForPost)
        {
            return _modelComparisonReportLogic.GetModels(modelComparisonForPost);
        }

        /// <summary>
        /// Generates the model comparasion report.
        /// </summary>
        /// <param name="reportview">The reportview.</param>
        /// <returns></returns>
        //FIXED-JITEN-JUNE - web api is not responsible for generating report. It only provides the data needed. Change the name of this action
        [HttpPost]
        public ModelComparisonReport Get(ModelComparisonReport reportview)
        {
            return _modelComparisonReportLogic.Generate(reportview);
        }
        
    }
}