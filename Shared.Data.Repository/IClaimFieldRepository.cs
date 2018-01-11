using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IClaimFieldRepository
    {
        List<ClaimField> GetClaimFieldsByModule(int moduleId);
    }
}