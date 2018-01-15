using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class MedicareIpAcuteOption
    {
        /// <summary>
        /// 
        /// </summary>
        public int MedicareIpAcuteOptionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MedicareIpAcuteOptionCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MedicareIpAcuteOptionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<MedicareIpAcuteOptionChild> MedicareIpAcuteOptionChilds { get; set; }
    }
}
