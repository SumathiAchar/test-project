using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    public interface IRtaEdiRequestLogic
    {
        // ReSharper disable once UnusedMemberInSuper.Global
        long Save(RtaEdiRequest rtaEdiRequest);
    }
}
