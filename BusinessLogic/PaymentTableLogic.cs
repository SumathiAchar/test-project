using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class PaymentTableLogic
    {
        /// <summary>
        /// The _payment table repository
        /// </summary>
        private readonly IPaymentTableRepository _paymentTableRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTableLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTableLogic(string connectionString)
        {
            _paymentTableRepository = Factory.CreateInstance<IPaymentTableRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTableLogic"/> class.
        /// </summary>
        /// <param name="paymentTableRepository">The payment table repository.</param>
        public PaymentTableLogic(IPaymentTableRepository paymentTableRepository)
        {
            if (paymentTableRepository != null)
            {
                _paymentTableRepository = paymentTableRepository;
            }
        }

        /// <summary>
        /// Determines whether [is table name exists] [the specified claim field docs].
        /// </summary>
        /// <param name="claimFieldDocs">The claim field docs.</param>
        /// <returns></returns>
        public bool IsTableNameExists(ClaimFieldDoc claimFieldDocs)
        {
            return _paymentTableRepository.IsTableNameExists(claimFieldDocs);
        }

        

        /// <summary>
        /// Gets the payment table.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        public PaymentTableContainer GetPaymentTable(ClaimFieldDoc claimFieldDoc)
        {
            if (claimFieldDoc != null && claimFieldDoc.ClaimFieldId == (byte) Enums.ClaimFieldTypes.CustomPaymentType)
            {
                return _paymentTableRepository.GetCustomPaymentTable(claimFieldDoc);
            }

            return _paymentTableRepository.GetPaymentTable(claimFieldDoc);
           
        }
    }
}