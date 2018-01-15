using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class Facility : BaseModel
    {
        
        /// <summary>
        /// Gets or sets the DisplayName.
        /// </summary>
        /// <value>
        /// The name of the Display.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the FacilityName.
        /// </summary>
        /// <value>
        /// The name of the facility.
        /// </value>
        public string FacilityName { get; set; } 
        
        /// <summary>
        /// Gets or sets the Address.
        /// </summary>
        /// <value>
        /// The Address.
        /// </value>
        public string Address { get; set; }
        
        /// <summary>
        /// Gets or sets the City.
        /// </summary>
        /// <value>
        /// The City.
        /// </value>
        public string City { get; set; }
        
        /// <summary>
        /// Gets or sets the StateId.
        /// </summary>
        /// <value>
        /// The StateId.
        /// </value>
        public string StateId { get; set; } 
        
        /// <summary>
        /// Gets or sets the Zip.
        /// </summary>
        /// <value>
        /// The Zip.
        /// </value>
        public string Zip { get; set; }
        
        /// <summary>
        /// Gets or sets the Domains.
        /// </summary>
        /// <value>
        /// The Domains.
        /// </value>
        public string Domains { get; set; }
        
        /// <summary>
        /// Gets or sets the Phone.
        /// </summary>
        /// <value>
        /// The Phone.
        /// </value>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the Fax.
        /// </summary>
        /// <value>
        /// The Fax.
        /// </value>
        public string Fax { get; set; }
        
        /// <summary>
        /// Gets or sets the IsActive.
        /// </summary>
        /// <value>
        /// The IsActive.
        /// </value>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Gets or sets the CreatedDate.
        /// </summary>
        /// <value>
        /// The CreatedDate.
        /// </value>
        public DateTime? CreatedDate { get; set; }
        
        /// <summary>
        /// Gets or sets the Early Statement Date.
        /// </summary>
        /// <value>
        /// The Early Statement Date.
        /// </value>
        public DateTime? EarlyStatementDate { get; set; }

        /// <summary>
        /// Gets or sets the SSINo.
        /// </summary>
        /// <value>
        /// The SSI No.
        /// </value>
        public string SsiNo { get; set; }

        /// <summary>
        /// Gets or sets the NoofUsers.
        /// </summary>
        /// <value>
        /// The No of Users.
        /// </value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int NoofUsers { get; set; }

        /// <summary>
        /// Gets or sets the FacilityBubbleId.
        /// </summary>
        /// <value>
        /// The No of FacilityBubbleId.
        /// </value>
        public int FacilityBubbleId { get; set; }

        /// <summary>
        /// Gets or sets the Facility Feature Control.
        /// </summary>
        /// <value>
        /// The Facility Feature Control.
        /// </value>
        public List<FeatureControl> FacilityFeatureControl { get; set; }

        /// <summary>
        /// Gets or sets the Password Expiration Days.
        /// </summary>
        /// <value>
        /// Password Expiration Days.
        /// </value>
        public int PasswordExpirationDays { get; set; }

        /// <summary>
        /// Gets or sets the Invalid Password Attempts.
        /// </summary>
        /// <value>
        /// Invalid Password Attempts
        /// </value>
        public int InvalidPasswordAttempts { get; set; }

        /// <summary>
        /// Gets or sets the ssi number.
        /// </summary>
        /// <value>
        /// The ssi number.
        /// </value>
        public new List<int> SsiNumber { get; set; }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        public string DataSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is medicare ip acute.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is medicare ip acute; otherwise, <c>false</c>.
        /// </value>
        public bool IsMedicareIpAcute { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is medicare op apc.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is medicare op apc; otherwise, <c>false</c>.
        /// </value>
        public bool IsMedicareOpApc { get; set; }

    }
}
