using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ModelComparisonReportLogic
    {
        private readonly IModelComparisonReportRepository _modelComparisonReportRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelComparisonReportLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ModelComparisonReportLogic(string connectionString)
        {
            _modelComparisonReportRepository = Factory.CreateInstance<IModelComparisonReportRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelComparisonReportLogic"/> class.
        /// </summary>
        /// <param name="modelComparisonReportRepository">The model comparison report repository.</param>
        public ModelComparisonReportLogic(IModelComparisonReportRepository modelComparisonReportRepository)
        {
            if (modelComparisonReportRepository != null)
                _modelComparisonReportRepository = modelComparisonReportRepository;
        }


        /// <summary>
        /// Gets the available models.
        /// </summary>
        /// <param name="modelComparisonForPost">The model comparison for post.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ModelComparisonReport> GetModels(ModelComparisonReport modelComparisonForPost)
        {
            return _modelComparisonReportRepository.GetModels(modelComparisonForPost);
        }

        /// <summary>
        /// Generates the model comparison report.
        /// </summary>
        /// <param name="modelComparisonReport">The model comparison report.</param>
        /// <returns></returns>
        public ModelComparisonReport Generate(ModelComparisonReport modelComparisonReport)
        {
            if (modelComparisonReport != null)
            {
                if (modelComparisonReport.StartDate == DateTime.MinValue &&
                    modelComparisonReport.EndDate == DateTime.MinValue)
                {
                    // If search criteria contains "-99|" means search criteria contains "adjudication request name" and ignoring the date type filter in sp level
                    if (!string.IsNullOrEmpty(modelComparisonReport.ClaimSearchCriteria) &&
                        modelComparisonReport.ClaimSearchCriteria.Contains(Constants.AdjudicationRequestCriteria))
                        modelComparisonReport.DateType = Constants.DefaultDateType;

                    modelComparisonReport.EndDate = DateTime.Now;
                    modelComparisonReport.StartDate =
                        DateTime.Now.AddYears(-GlobalConfigVariable.PullDataForNumberOfYears);
                }
                return _modelComparisonReportRepository.Generate(modelComparisonReport);
            }
            return null;
        }
    }
}
