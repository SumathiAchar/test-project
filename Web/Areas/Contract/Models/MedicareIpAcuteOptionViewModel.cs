using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // No need to Instantiated
    public class MedicareIpAcuteOptionViewModel
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
        public List<MedicareIpAcuteOptionChildViewModel> MedicareIpAcuteOptionChilds { get; set; }
    }
}