using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Helpers.ConditionValidator
{
    /// <summary>
    /// Interface IConditionValidator
    /// </summary>
    public interface IConditionValidator
    {
        /// <summary>
        /// Determines whether the specified condition is valid.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns><c>true</c> if the specified condition is valid; otherwise, <c>false</c>.</returns>
        bool IsValid(ICondition condition);

    }
}
