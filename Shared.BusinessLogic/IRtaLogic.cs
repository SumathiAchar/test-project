using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    public interface IRtaLogic
    {
        // ReSharper disable once UnusedMemberInSuper.Global
        RtaData GetRtaDataByClaim(EvaluateableClaim evaluateableClaim);

        // ReSharper disable once UnusedMemberInSuper.Global
        Dictionary<long, List<PaymentResult>> AdjudicateClaimData(List<EvaluateableClaim> evaluateableClaims,
            List<Contract> contracts);

        // ReSharper disable once UnusedMemberInSuper.Global
        long SaveTimeLog(RtaEdiTimeLog rtaEdiTimeLog);
    }
}
