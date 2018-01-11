using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rta")]
    public class RtaEdiResponseLogic :IRtaEdiResponseLogic
    {
        /// <summary>
        /// The _rta edi response repository
        /// </summary>
        private readonly IRtaEdiResponseRepository _rtaEdiResponseRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RtaEdiResponseLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public RtaEdiResponseLogic(string connectionString)
        {
            _rtaEdiResponseRepository = Factory.CreateInstance<IRtaEdiResponseRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RtaEdiResponseLogic"/> class.
        /// </summary>
        /// <param name="rtaEdiResponseRepository">The rta edi response repository.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rta")]
        public RtaEdiResponseLogic(IRtaEdiResponseRepository rtaEdiResponseRepository)
        {
            if (rtaEdiResponseRepository != null)
                _rtaEdiResponseRepository = rtaEdiResponseRepository;
        }

        /// <summary>
        /// Saves the specified rta edi response.
        /// </summary>
        /// <param name="rtaEdiResponse">The rta edi response.</param>
        /// <returns></returns>
        public long Save(RtaEdiResponse rtaEdiResponse)
        {
            return _rtaEdiResponseRepository.Save(rtaEdiResponse);
        }
    }
}
