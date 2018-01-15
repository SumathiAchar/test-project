using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Class for Medicare Lab Fee Schedule
    /// </summary>
    public class MedicareLabFeeSchedule : BaseModel
    {
        /// <summary>
        /// Gets or sets the HCPCS.
        /// </summary>
        /// <value>
        /// The HCPCS.
        /// </value>
        public string Hcpcs { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the carrier.
        /// </summary>
        /// <value>
        /// The carrier.
        /// </value>
        public string Carrier { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        /// Getting date as INT as ISO format(YYYYMMDD)
        public int Date { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the provider zip.
        /// </summary>
        /// <value>
        /// The provider zip.
        /// </value>
        public string ProviderZip { get; set; }

        /// <summary>
        /// Gets or sets the page setting.
        /// </summary>
        /// <value>
        /// The page setting.
        /// </value>
        public PageSetting PageSetting { get; set; }
        
        /// <summary>
        /// Gets or sets the user facilities.
        /// </summary>
        /// <value>
        /// The user facilities.
        /// </value>
        public List<Facility> UserFacilities { get; set; }

        /// <summary>
        /// Gets or sets the facility ids.
        /// </summary>
        /// <value>
        /// The facility ids.
        /// </value>
        public string FacilityIds { get; set; }


        /// <summary>
        /// Gets or sets the HcpcsCodeWithModifier.
        /// </summary>
        /// <value>
        /// The HcpcsCodeWithModifier.
        /// </value>
        public string HcpcsCodeWithModifier { get; set; }
        
        /// <summary>
        /// Gets or sets the user text.
        /// </summary>
        /// <value>
        /// The user text.
        /// </value>
        public string UserText { get; set; }
    }
}
