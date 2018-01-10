using System;
using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class MedicareIpAcuteOptionController : BaseController
    {
        private readonly MedicareIpAcuteOptionLogic _medicareIpAcuteOptionLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicareIpAcuteOptionController"/> class.
        /// </summary>
        public MedicareIpAcuteOptionController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _medicareIpAcuteOptionLogic = new MedicareIpAcuteOptionLogic(bubbleDataSource);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicareIpAcuteOptionController"/> class.
        /// </summary>
        /// <param name="medicareIpAcuteOptionLogic">The medicare ip acute option logic.</param>
        public MedicareIpAcuteOptionController(MedicareIpAcuteOptionLogic medicareIpAcuteOptionLogic)
        {
            _medicareIpAcuteOptionLogic = medicareIpAcuteOptionLogic;
        }

        /// <summary>
        /// Gets the medicare ip acute options.
        /// </summary>
        /// <returns></returns>
        public List<MedicareIpAcuteOption> GetMedicareIpAcuteOptions()
        {
            return _medicareIpAcuteOptionLogic.GetMedicareIpAcuteOptions();
        }
    }
}