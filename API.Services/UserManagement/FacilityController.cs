/************************************************************************************************************/
/**  Author         : Manikandan
/**  Created        : 04-Feb-2016
/**  Summary        : Manage Facility in User Management
/**  User Story Id  : Add Facility
/************************************************************************************************************/
using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.UserManagement
{
    public class FacilityController : ApiController
    {
        private readonly FacilityLogic _facilityLogic;

        /// <summary>
        /// Default Constructor
        /// </summary>
        FacilityController()
        {
            _facilityLogic = new FacilityLogic();
        }

        /// <summary>
        /// Save / Update Facility
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveFacility(Facility facility)
        {
            return _facilityLogic.SaveFacility(facility);
        }

        /// <summary>
        /// Get the facility by identifier
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        [HttpPost]
        public Facility GetFacility(Facility facility)
        {
            return _facilityLogic.GetFacilityById(facility);
        }

        /// <summary>
        /// Get Facility ById
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        [HttpPost]
        public Facility GetFacilityById(Facility facility)
        {
            return _facilityLogic.GetFacilityById(facility);
        }

        /// <summary>
        /// Get All Facilities
        /// </summary>
        /// <param name="facilityInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public FacilityContainer GetAllFacilities(FacilityContainer facilityInfo)
        {
            return _facilityLogic.GetAllFacilities(facilityInfo);
        }

        /// <summary>
        /// Delete Facility
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteFacility(Facility facility)
        {
            return _facilityLogic.DeleteFacility(facility);
        }

        /// <summary>
        /// GetActiveStates
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<string> GetActiveStates()
        {
            return _facilityLogic.GetActiveStates();
        }

        /// <summary>
        /// Get Bubbles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<FacilityBubble> GetBubbles()
        {
            return _facilityLogic.GetBubbles();
        }

        /// <summary>
        /// Get the all All Facilities
        /// </summary>
        [HttpPost]
        public List<Facility> GetAllFacilityList(User data)
        {
            return _facilityLogic.GetAllFacilityList(data);
        }

        /// <summary>
        /// Gets the facility medicare details.
        /// </summary>
        /// <param name="facility">The facility.</param>
        /// <returns></returns>
        [HttpPost]
        public Facility GetFacilityMedicareDetails(Facility facility)
        {
            return _facilityLogic.GetFacilityMedicareDetails(facility.FacilityId);
        }
    }
}
