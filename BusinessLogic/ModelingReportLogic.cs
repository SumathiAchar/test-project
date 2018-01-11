using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ModelingReportLogic
    {
        private readonly IModelingReportRepository _modelingReportRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelingReportLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ModelingReportLogic(string connectionString)
        {
            _modelingReportRepository = Factory.CreateInstance<IModelingReportRepository>(connectionString, true);
        }

        public ModelingReportLogic(IModelingReportRepository modelingReportRepository)
        {
            if (modelingReportRepository != null)
            {
                _modelingReportRepository = modelingReportRepository;
            }
        }

        /// <summary>
        /// Gets all modeling details.
        /// </summary>
        /// <param name="modelingReport">The modeling report.</param>
        /// <returns></returns>
        public ModelingReport GetAllModelingDetails(ModelingReport modelingReport)
        {
            return _modelingReportRepository.GetAllModelingDetails(modelingReport);
        }
    }
}
