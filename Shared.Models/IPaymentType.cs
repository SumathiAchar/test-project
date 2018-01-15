using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Interface IPaymentType
    /// </summary>
    public interface IPaymentType
    {
        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        /// <value>The conditions.</value>
        // ReSharper disable once UnusedMemberInSuper.Global
        List<ICondition> Conditions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [pay at claim level].
        /// </summary>
        /// <value><c>true</c> if [pay at claim level]; otherwise, <c>false</c>.</value>
        // ReSharper disable once UnusedMemberInSuper.Global
        bool PayAtClaimLevel { get; set; }

        /// <summary>
        /// Gets or sets the valid line ids.
        /// </summary>
        /// <value>The valid line ids.</value>
        List<int> ValidLineIds { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>The type of the payment.</value>
        // ReSharper disable once UnusedMemberInSuper.Global
        // ReSharper disable UnusedMemberInSuper.Global
        int PaymentTypeId { get; set; }
        // ReSharper restore UnusedMemberInSuper.Global

    }
}
