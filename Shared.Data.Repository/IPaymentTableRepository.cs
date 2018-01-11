using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IPaymentTableRepository : IDisposable
    {
        /// <summary>
        /// Determines whether [is table name exists] [the specified claim field docs].
        /// </summary>
        /// <param name="claimFieldDocs">The claim field docs.</param>
        /// <returns></returns>
        bool IsTableNameExists(ClaimFieldDoc claimFieldDocs);

        
        /// <summary>
        /// Gets the payment table.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        PaymentTableContainer GetPaymentTable(ClaimFieldDoc claimFieldDoc);

        /// <summary>
        /// Gets the custom payment table.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        PaymentTableContainer GetCustomPaymentTable(ClaimFieldDoc claimFieldDoc);


    }
}