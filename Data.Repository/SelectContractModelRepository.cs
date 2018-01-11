

/************************************************************************************************************/
/**  Author         : Prasad Dintakurti
/**  Created        : 04-Sep-2013
/**  Summary        : Handles Add/Modify Select Contract Model Repository DataAccess Layer

/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class SelectContractModelRepository : ISelectContractModelRepository
    {
        // Variables
        private readonly Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectContractModelRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SelectContractModelRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets all contract models.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public List<ContractModel> GetAllContractModels(ContractModel data)
        {
            List<ContractModel> contractModelsList = null;

            _cmd = _db.GetStoredProcCommand("GetAllContractModels");
            _db.AddInParameter(_cmd, "@FacilityID", DbType.Int64, data.FacilityId);
            DataSet contractModelDataSet = _db.ExecuteDataSet(_cmd);

            if (contractModelDataSet.IsTableDataPopulated(0))
            {
                if (contractModelDataSet.Tables[0].Rows != null && contractModelDataSet.Tables[0].Rows.Count > 0)
                {
                    contractModelsList = new List<ContractModel>();
                    for (int i = 0; i < contractModelDataSet.Tables[0].Rows.Count; i++)
                    {
                        ContractModel contractModelList = new ContractModel
                        {
                            NodeId =
                                long.Parse(
                                    contractModelDataSet.Tables[0].Rows[i][
                                        "NodeID"].ToString()),
                            NodeText =
                                Convert.ToString(
                                    contractModelDataSet.Tables[0].Rows[i]["NodeText"])
                        };
                        contractModelsList.Add(contractModelList);
                    }

                }
            }

            return contractModelsList;
        }

        /// <summary>
        ///  Gets all Contract Facilities.
        /// </summary>
        /// <returns>List of GetAllContractFacilities</returns>
        public List<FacilityDetail> GetAllContractFacilities(ContractHierarchy contractHierarchyData)
        {
            List<FacilityDetail> contractFacilitiesList = null;
            if (contractHierarchyData != null)
            {

                _cmd = _db.GetStoredProcCommand("GetAllFacilities");
                string facilityIds = string.Join(",", contractHierarchyData.FacilityList.Select(a => a.ToString(CultureInfo.InvariantCulture)).ToArray());
                _db.AddInParameter(_cmd, "@FacilityID", DbType.String, facilityIds);
                DataSet contractHierarchyDataSet = _db.ExecuteDataSet(_cmd);

                if (contractHierarchyDataSet.IsTableDataPopulated(0))
                {
                    if (contractHierarchyDataSet.Tables[0].Rows != null && contractHierarchyDataSet.Tables[0].Rows.Count > 0)
                    {
                        contractFacilitiesList = new List<FacilityDetail>();
                        for (int i = 0; i < contractHierarchyDataSet.Tables[0].Rows.Count; i++)
                        {
                            FacilityDetail contractFacilityList = new FacilityDetail
                            {
                                FacilityId =
                                    int.Parse(
                                        contractHierarchyDataSet.Tables[0].Rows[i][
                                            "FacilityId"].ToString()),
                                FacilityName =
                                    Convert.ToString(
                                        contractHierarchyDataSet.Tables[0].Rows[i][
                                            "FacilityName"]),
                                NodeId = long.Parse(contractHierarchyDataSet.Tables[0].Rows[i]["NodeId"].ToString())
                            };
                            contractFacilitiesList.Add(contractFacilityList);
                        }
                    }
                }
            }
            return contractFacilitiesList;
        }

        /// <summary>
        /// Disposes the objects
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {

        }
    }
}
