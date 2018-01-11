using System;
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.PaymentTable
{
    // ReSharper disable once UnusedMember.Global
    public class MedicareLabFeeScheduleController : BaseController
    {
        /// <summary>
        /// The _payment table logic
        /// </summary>
        private readonly MedicareLabFeeScheduleLogic _medicareLabFeeScheduleLogic;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTableController"/> class.
        /// </summary>
        public MedicareLabFeeScheduleController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _medicareLabFeeScheduleLogic = new MedicareLabFeeScheduleLogic(bubbleDataSource);
        }

        /// <summary>
        /// Gets the name of the Medicare Lab Fee Schedule table.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<MedicareLabFeeSchedule> GetMedicareLabFeeScheduleTableNames(MedicareLabFeeSchedule mCareFeeSchedule)
        {
            return _medicareLabFeeScheduleLogic.GetMedicareLabFeeScheduleTableNames(mCareFeeSchedule);
        }

        /// <summary>
        /// Gets the name of the m care lab fee schedule table.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public MedicareLabFeeScheduleResult GetMedicareLabFeeSchedule(MedicareLabFeeSchedule mCareLabFeeSchedule)
        {
            return _medicareLabFeeScheduleLogic.GetMedicareLabFeeSchedule(mCareLabFeeSchedule);
        }
    }
}
