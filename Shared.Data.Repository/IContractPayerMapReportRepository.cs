using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IContractPayerMapReportRepository : IDisposable
    {
        ContractPayerMapReport Get(ContractPayerMapReport contractPayerMapReport);
    }
}
