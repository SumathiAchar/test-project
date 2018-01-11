using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class LogOnLogic
    {
        private readonly ILogOnRepository _logOnRepository;

        public LogOnLogic(string connectionString)
        {
            _logOnRepository = Factory.CreateInstance<ILogOnRepository>(connectionString, true); 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogOnLogic"/> class.
        /// </summary>
        /// <param name="logOnRepository">The log in repository.</param>
        public LogOnLogic(ILogOnRepository logOnRepository)
        {
            if (logOnRepository != null)
                _logOnRepository = logOnRepository;
        }

        /// <summary>
        /// Inserts the audit log.
        /// </summary>
        /// <param name="logOnInfo">The log on information.</param>
        public void InsertAuditLog(LogOn logOnInfo)
        {
            _logOnRepository.InsertAuditLog(logOnInfo);
        }
    }
}
