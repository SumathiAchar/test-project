using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    /// <summary>
    /// Interface for Condition Logic
    /// </summary>
    public interface IConditionLogic
    {
        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        ICondition Condition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is match].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is match]; otherwise, <c>false</c>.
        /// </value>
        bool IsMatch();

        /// <summary>
        /// Gets the property values.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        List<string> GetPropertyValues(object claim, string propertyName);

      }
}
