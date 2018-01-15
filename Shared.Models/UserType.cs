namespace SSI.ContractManagement.Shared.Models
{
    public abstract class UserType : BaseModel
    {
        /// <summary>
        /// Gets or sets the UserTypeId.
        /// </summary>
        /// <value>
        /// The UserTypeId.
        /// </value>
        public int UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The  Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the IsActive.
        /// </summary>
        /// <value>
        /// The  IsActive.
        /// </value>
        public bool IsActive { get; set; }
    }
}
