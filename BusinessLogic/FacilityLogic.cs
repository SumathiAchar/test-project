/************************************************************************************************************/
/**  Author         : Manikandan
/**  Created        : 4-FEB-2016
/**  Summary        : Manage Facilities
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class FacilityLogic : IFacilityLogic
    {
        /// <summary>
        /// The _facility repository
        /// </summary>
        private readonly IFacilityRepository _facilityRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacilityLogic"/> class.
        /// </summary>
        public FacilityLogic()
        {
            _facilityRepository = Factory.CreateInstance<IFacilityRepository>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacilityLogic"/> class.
        /// </summary>
        /// <param name="facilityRepository">The facility repository.</param>
        public FacilityLogic(IFacilityRepository facilityRepository)
        {
            if (facilityRepository != null)
            {
                _facilityRepository = facilityRepository;
            }
        }

        /// <summary>
        /// Save / Update Facility
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        public string SaveFacility(Facility facility)
        {
            if (facility != null)
            {
                //removing spaces from SSiNumber
                string ssiNumbers = Regex.Replace(facility.SsiNo, @"\s", string.Empty);
                
                //Triming SSi number to 9 digits
                List<string> ssiNumbersList = ssiNumbers.Split(',').ToList().Select(ssiNumber => (ssiNumber.Length > 9) ? ssiNumber.Substring(0, 9) : ssiNumber).ToList();

                facility.SsiNo = string.Join(",", ssiNumbersList); 

                //Replace new line with space
                string replaceLineBreak = facility.Domains.Replace("\n", string.Empty);

                List<string> domains = replaceLineBreak.Trim().Split(',').ToList();

                //Removing @ Symble from domain names
                List<string> domainNames =
                    domains.Select(domain => domain.Split('@'))
                        .Select(removeAtSymble => removeAtSymble[removeAtSymble.Length - 1])
                        .ToList();

                facility.Domains = string.Join(",", domainNames);
            }
            return _facilityRepository.SaveFacility(facility);
        }

        /// <summary>
        /// Get the facility by identifier
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        public Facility GetFacilityById(Facility facility)
        {
            return _facilityRepository.GetFacilityById(facility);
        }


        /// <summary>
        /// Get All Facilities
        /// </summary>
        /// <param name="facilityInfo"></param>
        /// <returns></returns>
        public FacilityContainer GetAllFacilities(FacilityContainer facilityInfo)
        {
            return _facilityRepository.GetAllFacilities(facilityInfo);
        }

        /// <summary>
        /// Delete Facility
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        public bool DeleteFacility(Facility facility)
        {
            return _facilityRepository.DeleteFacility(facility);
        }

        /// <summary>
        /// GetActiveStates
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<string> GetActiveStates()
        {
            return _facilityRepository.GetActiveStates();
        }

        /// <summary>
        /// Get Bubbles
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<FacilityBubble> GetBubbles()
        {
            return _facilityRepository.GetBubbles();
        }

        /// <summary>
        /// Gets all Facility models.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<Facility> GetAllFacilityList(User data)
        {
            return _facilityRepository.GetAllFacilityList(data);
        }

        /// <summary>
        /// Gets the facility data source.
        /// </summary>
        /// <param name="bubbleConnectionString"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<Facility> GetFacilitiesDataSource(string bubbleConnectionString)
        {
            return _facilityRepository.GetFacilitiesDataSource(bubbleConnectionString);
           
        }

        /// <summary>
        /// Gets the facility medicare details.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        public Facility GetFacilityMedicareDetails(int facilityId)
        {
            return _facilityRepository.GetFacilityMedicareDetails(facilityId);

        }

        /// <summary>
        /// Gets the facility connection.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        public string GetFacilityConnection(int facilityId)
        {
            return _facilityRepository.GetFacilityConnection(facilityId);
        }

        /// <summary>
        /// Gets the facility data source.
        /// </summary>
        /// <returns></returns>
        public string GetBubbleConnectionString(string bubbleName)
        {
            return _facilityRepository.GetBubbleConnectionString(bubbleName);
        }
    }
}
