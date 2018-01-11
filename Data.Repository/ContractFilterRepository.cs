/************************************************************************************************************/
/**  Author         : Vishesh Bhawsar
/**  Created        : 23-Aug-2013
/**  Summary        : Handles Contract Filters info
/**  User Story Id  : 5.User Story Add a new contract Figure 15
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    public class ContractFilterRepository : IContractFilterRepository
    {
        /// <summary>
        /// The _database obj variable
        /// </summary>
        private Database _databaseObj;
        /// <summary>
        /// The _database command variable
        /// </summary>
        DbCommand _databaseCommand;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractFilterRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractFilterRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }


        /// <summary>
        /// Gets the contract filters data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public List<ContractFilter> GetContractFilters(ContractServiceType data)
        {
            //holds the response data
            List<ContractFilter> contractFilterData = new List<ContractFilter>();

            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetContractFiltersDataBasedOnContractId");
            _databaseObj.AddInParameter(_databaseCommand, "@ContractId", DbType.Int64, data.ContractId);
            _databaseObj.AddInParameter(_databaseCommand, "@ContractServiceTypeID", DbType.Int64, data.ContractServiceTypeId);
            _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, data.UserName);
            // Retrieve the results of the Stored Procedure
            DataSet dataSetObj = _databaseObj.ExecuteDataSet(_databaseCommand);
            //Map datatable to business objects
            // sending 0 to insure that table at index is having valid data.
            if (dataSetObj.IsTableDataPopulated(0))
            {
                DataTable dataTable = dataSetObj.Tables[0];
                for (int indexCount = 0; indexCount < dataTable.Rows.Count; indexCount++)
                {
                    ContractFilter tempData = new ContractFilter
                    {
                        FilterName = Convert.ToString(dataTable.Rows[indexCount]["FilterName"]),
                        FilterValues = Convert.ToString(dataTable.Rows[indexCount]["FilterValues"]),
                        IsServiceTypeFilter = DBNull.Value == dataTable.Rows[indexCount]["IsServiceTypeFilter"]
                                                                   ? (bool?)null
                                                                   : Convert.ToBoolean(dataTable.Rows[indexCount]["IsServiceTypeFilter"].ToString()),

                        PaymentTypeId = DBNull.Value == dataTable.Rows[indexCount]["PaymentTypeID"]
                                                                   ? (long?)null
                                                                   : long.Parse(dataTable.Rows[indexCount]["PaymentTypeID"].ToString()),
                        ServiceLineTypeId = DBNull.Value == dataTable.Rows[indexCount]["ServiceLineTypeId"]
                                                                   ? (long?)null
                                                                   : long.Parse(dataTable.Rows[indexCount]["ServiceLineTypeId"].ToString())

                    };
                    contractFilterData.Add(tempData);
                }
                //returns the response to Business layer
                return contractFilterData;
            }
            return null;
        }

        /// <summary>
        /// Disposes the objects
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseCommand.Dispose();
            _databaseObj = null;
        }
    }
}
