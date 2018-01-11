using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rta")]
    public class RtaEdiRequestLogic : IRtaEdiRequestLogic
    {
        /// <summary>
        /// The _rta edi request repository
        /// </summary>
        private readonly IRtaEdiRequestRepository _rtaEdiRequestRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RtaEdiRequestLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public RtaEdiRequestLogic(string connectionString)
        {
            _rtaEdiRequestRepository = Factory.CreateInstance<IRtaEdiRequestRepository>(connectionString, true);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RtaEdiRequestLogic"/> class.
        /// </summary>
        /// <param name="rtaEdiRequestRepository">The rta edi request repository.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rta")]
        public RtaEdiRequestLogic(IRtaEdiRequestRepository rtaEdiRequestRepository)
        {
            if (rtaEdiRequestRepository != null)
                _rtaEdiRequestRepository = rtaEdiRequestRepository;
        }

        /// <summary>
        /// Saves the specified rta edi request.
        /// </summary>
        /// <param name="rtaEdiRequest">The rta edi request.</param>
        /// <returns></returns>
        public long Save(RtaEdiRequest rtaEdiRequest)
        {
            return _rtaEdiRequestRepository.Save(rtaEdiRequest);            
        }

    }
}
