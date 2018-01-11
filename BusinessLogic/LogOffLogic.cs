using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class LogOffLogic
    {
        private readonly ILogOffRepository _logOffRepository;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "conection"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string")]
        public LogOffLogic(string conectionString)
        {
            _logOffRepository = Factory.CreateInstance<ILogOffRepository>(conectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogOffLogic"/> class.
        /// </summary>
        /// <param name="logOffRepository">The log out repository.</param>
        public LogOffLogic(ILogOffRepository logOffRepository)
        {
            if (logOffRepository != null)
                _logOffRepository = logOffRepository;
        }

        /// <summary>
        /// Inserts the audit log.
        /// </summary>
        /// <param name="logOffInfo">The log off information.</param>
        /// <returns></returns>
        public void InsertAuditLog(LogOff logOffInfo)
        {
            _logOffRepository.InsertAuditLog(logOffInfo);
        }
    }
}
