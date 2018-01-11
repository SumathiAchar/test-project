using System.Collections.Generic;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// Logic class for RTA - WIP
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rta")]
    public class RtaLogic :IRtaLogic
    {
        /// <summary>
        /// The _rta repository
        /// </summary>
        private readonly IRtaRepository _rtaRepository;

        /// <summary>
        /// The _adjudication engine
        /// </summary>
        private readonly IAdjudicationEngine _adjudicationEngine;


        /// <summary>
        /// Initializes a new instance of the <see cref="RtaLogic"/> class.
        /// </summary>
        public RtaLogic()
        {
            _rtaRepository = Factory.CreateInstance<IRtaRepository>();
            _adjudicationEngine = Factory.CreateInstance<IAdjudicationEngine>();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RtaLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public RtaLogic(string connectionString)
        {
            _rtaRepository = Factory.CreateInstance<IRtaRepository>(connectionString, true);
            _adjudicationEngine = Factory.CreateInstance<IAdjudicationEngine>(connectionString, true);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RtaLogic"/> class.
        /// </summary>
        /// <param name="rtaRepository">The rta repository.</param>
        /// <param name="adjudicationEngine">The adjudication engine.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rta")]
        public RtaLogic(IRtaRepository rtaRepository, IAdjudicationEngine adjudicationEngine)
        {
            if (rtaRepository != null)
                _rtaRepository = rtaRepository;
            if (adjudicationEngine != null)
                _adjudicationEngine = adjudicationEngine;
        }

        /// <summary>
        /// Gets the rta data by claim.
        /// </summary>
        /// <param name="evaluateableClaim">The evaluate able claim.</param>
        /// <returns></returns>
        public RtaData GetRtaDataByClaim(EvaluateableClaim evaluateableClaim)
        {
            return _rtaRepository.GetRtaDataByClaim(evaluateableClaim);
        }


        /// <summary>
        /// Adjudicates the claim data.
        /// </summary>
        /// <param name="evaluateableClaims">The evaluate able claims.</param>
        /// <param name="contracts">The contracts.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public Dictionary<long, List<PaymentResult>> AdjudicateClaimData(List<EvaluateableClaim> evaluateableClaims, List<Contract> contracts)
        {
            return _adjudicationEngine.AdjudicateClaim(evaluateableClaims, contracts, 0);
        }

        /// <summary>
        /// Saves the time log.
        /// </summary>
        /// <param name="rtaEdiTimeLog">The rta edi time log.</param>
        /// <returns></returns>
        public long SaveTimeLog(RtaEdiTimeLog rtaEdiTimeLog)
        {
            return _rtaRepository.SaveTimeLog(rtaEdiTimeLog);
        }

    }
}
