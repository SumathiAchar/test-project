using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPayerRepository: IDisposable
    {
        List<Payer> GetPayers(ContractServiceLineClaimFieldSelection selectedFacility);
    }
}
