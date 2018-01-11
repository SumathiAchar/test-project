
/************************************************************************************************************/
/**  Author         : Prasad Dintakurti
/**  Created        : 04-Sep-2013
/**  Summary        : Handles Add/Modify Select Contract Model Logic Details functionalities

/************************************************************************************************************/

using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;
using System.Collections.Generic;

namespace SSI.ContractManagement.BusinessLogic
{
    public class SelectContractModelLogic
    {
        private readonly ISelectContractModelRepository _selectContractModelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectContractModelLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SelectContractModelLogic(string connectionString)
        {
            _selectContractModelRepository = Factory.CreateInstance<ISelectContractModelRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectContractModelLogic"/> class.
        /// </summary>
        /// <param name="selectContractModelRepository">The select contract model repository.</param>
        public SelectContractModelLogic(ISelectContractModelRepository selectContractModelRepository)
        {
            if (selectContractModelRepository != null)
            {
                _selectContractModelRepository = selectContractModelRepository;
            }
        }


        /// <summary>
        /// Gets all contract models.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ContractModel> GetAllContractModels(ContractModel data)
        {
            return _selectContractModelRepository.GetAllContractModels(data);
        }

        /// <summary>
        /// Get All Contract Facilities
        /// </summary>
        /// <returns>List of Contract Facilities </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<FacilityDetail> GetAllContractFacilities(ContractHierarchy data)
        {
            return _selectContractModelRepository.GetAllContractFacilities(data);
        }
    }
}
