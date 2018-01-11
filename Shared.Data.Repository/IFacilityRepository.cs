using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IFacilityRepository : IDisposable
    {

        /// <summary>
        /// Save Facility Info
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        string SaveFacility(Facility facility);

        /// <summary>
        /// Get Facility ById
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        Facility GetFacilityById(Facility facility);

        /// <summary>
        /// Get All Facilities
        /// </summary>
        /// <param name="facilityInfo"></param>
        /// <returns></returns>
        FacilityContainer GetAllFacilities(FacilityContainer facilityInfo);

        /// <summary>
        /// Delete Facility
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        bool DeleteFacility(Facility facility);

        /// <summary>
        /// Get Active States
        /// </summary>
        /// <returns></returns>
        List<string> GetActiveStates();

        /// <summary>
        /// Get Bubbles
        /// </summary>
        /// <returns></returns>
        List<FacilityBubble> GetBubbles();

        /// <summary>
        /// Gets all Facility models.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        List<Facility> GetAllFacilityList(User data);

        /// <summary>
        /// Gets the facilities data source.
        /// </summary>
        /// <param name="bubbleConnectionString">The bubble connection string.</param>
        /// <returns></returns>
        List<Facility> GetFacilitiesDataSource(string bubbleConnectionString);

        /// <summary>
        /// Gets the facility medicare details.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        Facility GetFacilityMedicareDetails(int facilityId);

        /// <summary>
        /// Gets the facility connection.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        string GetFacilityConnection(int facilityId);

        /// <summary>
        /// Gets the bubble connection string.
        /// </summary>
        /// <param name="bubbleName">Name of the bubble.</param>
        /// <returns></returns>
        string GetBubbleConnectionString(string bubbleName);
    }
}
