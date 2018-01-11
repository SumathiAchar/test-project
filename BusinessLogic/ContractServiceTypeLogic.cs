using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// Class for contract service type logic
    /// </summary>
    public class ContractServiceTypeLogic : ContractBaseLogic, IContractServiceTypeLogic
    {
        private readonly IContractServiceTypeRepository _contractServiceTypeRepository;
        private readonly string _connectionString ;

        /// <summary>
        /// Gets or sets the type of the contract service.
        /// </summary>
        /// <value>
        /// The type of the contract service.
        /// </value>
        public ContractServiceType ContractServiceType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractServiceTypeLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractServiceTypeLogic(string connectionString)
        {
            _connectionString = connectionString;
            _contractServiceTypeRepository = Factory.CreateInstance<IContractServiceTypeRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes _serviceTypeDetailsRepository by assiging passed object in contructor
        /// </summary>
        /// <param name="serviceTypeDetailsRepository"></param>
        public ContractServiceTypeLogic(IContractServiceTypeRepository serviceTypeDetailsRepository)
        {
            if (serviceTypeDetailsRepository != null)
            {
                _contractServiceTypeRepository = serviceTypeDetailsRepository;
            }
        }

        /// <summary>
        /// Returns all available Contract Service Type
        /// </summary>
        /// <param name="contractId">contractId</param>
        /// <returns>List of ContractServiceTypes object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ContractServiceType> GetAllContractServiceType(long contractId)
        {
            return _contractServiceTypeRepository.GetAllContractServiceType(contractId);
        }

        /// <summary>
        /// Add/Edit ContractServiceType based on passed ContractServiceTypes object
        /// </summary>
        /// <param name="contractServiceTypes">ContractServiceTypes</param>
        /// <returns>Inserted/Updated ContractServiceTypeId</returns>       
        public long AddEditContractServiceType(ContractServiceType contractServiceTypes)
        {
            return _contractServiceTypeRepository.AddEditContractServiceType(contractServiceTypes);
        }

        /// <summary>
        /// Copies the contract serviceType by contract service type unique identifier.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public long CopyContractServiceType(ContractServiceType data)
        {
            return _contractServiceTypeRepository.CopyContractServiceType(data);
        }

        /// <summary>
        /// Rename the contract serviceType by contract service type unique identifier.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public long RenameContractServiceType(ContractServiceType data)
        {
            return _contractServiceTypeRepository.RenameContractServiceType(data);
        }

        /// <summary>
        /// Gets the contract service type details.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public ContractServiceType GetContractServiceTypeDetails(ContractServiceType data)
        {
            return _contractServiceTypeRepository.GetContractServiceTypeDetails(data);
        }

        /// <summary>
        /// Determines whether [is valid service type].
        /// </summary>
        /// <returns></returns>
        public bool IsValidServiceType()
        {
            return (ContractServiceType != null && ContractServiceType.Conditions != null &&
                    ContractServiceType.Conditions.Any() &&
                    (ContractServiceType.PaymentTypes != null &&
                     ContractServiceType.PaymentTypes.Any(
                         paymentType =>
                             paymentType.PaymentTypeId != (byte)Enums.PaymentTypeCodes.Cap &&
                             paymentType.PaymentTypeId != (byte)Enums.PaymentTypeCodes.LesserOf &&
                             paymentType.PaymentTypeId != (byte)Enums.PaymentTypeCodes.None)));
        }


        /// <summary>
        /// Determines whether the specified claim is match.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        public bool IsMatch(IEvaluateableClaim claim)
        {
            return ContractServiceType != null && IsConditionsValid(ContractServiceType.Conditions, claim);
        }


        /// <summary>
        /// Evaluates the specified claim.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<PaymentResult> Evaluate(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut,bool isContractFilter)
        {
            //Match claim codes with contract codes
            if (IsMatch((claim)))
            {
                foreach (PaymentTypeBase paymentType in ContractServiceType.PaymentTypes)
                {
                    //Inject paymentlogics based on Payment type.
                    IPaymentTypeLogic paymentlogic = Factory.CreateInstance<IPaymentTypeLogic>(((Enums.PaymentTypeCodes)paymentType.PaymentTypeId).ToString(), _connectionString);
                    paymentlogic.PaymentTypeBase = paymentType;
                    //Appy payment logic for each payment types. This will consider all charge line codes to match with contract codes
                    paymentResults = paymentlogic.Evaluate(claim, paymentResults, ContractServiceType.IsCarveOut, isContractFilter);
                }
            }
            return paymentResults;
        }

        /// <summary>
        /// Add/Edit ContractServiceType based on passed ContractServiceTypes object
        /// </summary>
        /// <param name="contractServiceTypes">ContractServiceTypes</param>
        /// <returns>Inserted/Updated ContractServiceTypeId</returns>       
        public bool IsContractServiceTypeNameExit(ContractServiceType contractServiceTypes)
        {
            return _contractServiceTypeRepository.IsContractServiceTypeNameExit(contractServiceTypes);
        }
    }
}
