namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Feature Control
    /// </summary>
    public class FeatureControl : BaseModel
    {
        /// <summary>
        /// Gets or sets the FeatureControlId
        /// </summary>
        /// <value>
        /// The Feature ControlId.
        /// </value>
        public int FeatureControlId { get; set; }

        /// <summary>
        /// Gets or sets the Feature Control Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the IsSelected
        /// </summary>
        /// <value>
        /// The IsSelected.
        /// </value>
        public bool IsSelected { get; set; }
    }
}
