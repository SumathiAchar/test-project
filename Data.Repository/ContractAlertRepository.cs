
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ContractAlertRepository : BaseRepository, IContractAlertRepository
    {
        // Variables
        private Database _databaseObj;
        DbCommand _databaseCommandObj;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractAlertRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractAlertRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the contract alerts.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public List<ContractAlert> GetContractAlerts(ContractAlert data)
        {
            List<ContractAlert> contractAlertList = new List<ContractAlert>();
            if (data != null)
            {

                _databaseCommandObj = _databaseObj.GetStoredProcCommand("DisplayContractAlerts");
                _databaseObj.AddInParameter(_databaseCommandObj, "@DaysToDismissAlerts", DbType.Int32,
                                            data.NumberOfDaysToDismissAlerts);
                _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityID", DbType.Int64, data.FacilityId);
                DataSet contractAlertDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

                if (contractAlertDataSet.IsTableDataPopulated(0))
                {
                    for (int i = 0; i < contractAlertDataSet.Tables[0].Rows.Count; i++)
                    {
                        ContractAlert alert = new ContractAlert
                        {
                            PayerName = GetStringValue(contractAlertDataSet.Tables[0].Rows[i]["PayerName"]),
                            ContractName = GetStringValue(contractAlertDataSet.Tables[0].Rows[i]["ContractName"]),
                            DateOfExpiry = GetValue<DateTime>(contractAlertDataSet.Tables[0].Rows[i]["EndDate"], typeof(DateTime)),
                            ContractId = GetValue<long>(contractAlertDataSet.Tables[0].Rows[i]["ContractId"], typeof(long)),
                            ContractAlertId = GetValue<long>(contractAlertDataSet.Tables[0].Rows[i]["ContractAlertId"], typeof(long)),
                            IsVerified = DBNull.Value != contractAlertDataSet.Tables[0].Rows[i]["IsVerified"] && 
                                        GetValue<bool>(contractAlertDataSet.Tables[0].Rows[i]["IsVerified"], typeof(bool))
                        };
                        contractAlertList.Add(alert);
                    }
                }
            }
            //returns response to Business layer
            return contractAlertList;
        }

        /// <summary>
        /// Updating Contract Alerts Information
        /// </summary>
        /// <param name="data">ContractId and UserName</param>
        /// <returns>contractId</returns>
        public bool UpdateContractAlerts(ContractAlert data)
        {
            bool isUpdated = false;
            if (data != null)
            {

                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("UpdateContractAlerts");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractId ", DbType.Int64, data.ContractId);

                // Retrieve the results of the Stored Procedure in Data table
                int returnValue = _databaseObj.ExecuteNonQuery(_databaseCommandObj);
                if (returnValue > 0)
                    isUpdated = true;

            }
            //returns response to Business layer
            return isUpdated;
        }

        /// <summary>
        /// Gets Contract alert count.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public int ContractAlertCount(ContractAlert data)
        {
            if (data != null)
            {
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetUnVerifiedContractAlertsCount");
                _databaseObj.AddInParameter(_databaseCommandObj, "@DaysToDismissAlerts", DbType.Int32,
                                            data.NumberOfDaysToDismissAlerts);
                _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityID", DbType.Int64, data.FacilityId);
                _databaseObj.AddOutParameter(_databaseCommandObj, "@UnVerifiedAlertsCount", DbType.Int32, Int32.MaxValue);
                _databaseObj.ExecuteNonQuery(_databaseCommandObj);
                int alertCount = Convert.ToInt32(_databaseCommandObj.Parameters["@UnVerifiedAlertsCount"].Value);
                return alertCount;
            }
            //returns 0 if any exception occurs
            return 0;
        }

        /// <summary>
        /// Updates the alert verified status.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public bool UpdateAlertVerifiedStatus(ContractAlert data)
        {
            if (data != null)
            {
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("UpdateVerifiedSatusForContractAlter");
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractAlertID", DbType.Int64, data.ContractAlertId);
                _databaseObj.ExecuteNonQuery(_databaseCommandObj);
                return true;
            }
            //returns 0 if any exception occurs
            return false;
        }

        /// <summary>
        /// Disposes the objects
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseCommandObj.Dispose();
            _databaseObj = null;
        }
    }
}
