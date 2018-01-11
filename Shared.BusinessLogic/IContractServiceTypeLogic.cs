using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    /// <summary>
    /// Interface for the ServiceType Logic class
    /// </summary>
    public interface IContractServiceTypeLogic : IAdjudicationBaseLogic
    {
        /// <summary>
        /// Gets or sets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        ContractServiceType ContractServiceType { get; set; }

        /// <summary>
        /// Determines whether [is valid service type].
        /// </summary>
        /// <returns></returns>
        bool IsValidServiceType();
    }
}
