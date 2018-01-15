namespace SSI.ContractManagement.Shared.Models
{
    public abstract class UserFacilityMapping : BaseModel
    {
        /// <summary>
        /// Gets or sets the UserId.
        /// </summary>
        /// <value>
        /// The UserId.
        /// </value>
        public new int UserId { get; set; }

        /// <summary>
        /// Gets or sets the FacilityId.
        /// </summary>
        /// <value>
        /// The  FacilityId.
        /// </value>
        public new int FacilityId { get; set; }
    }
}
