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
    public class ContractPayerMapReportRepository : IContractPayerMapReportRepository
    {

        private Database _databaseObj;
        DbCommand _databaseCommandObj;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractPayerMapReportRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractPayerMapReportRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
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

        /// <summary>
        /// Gets all modeling details.
        /// </summary>
        /// <param name="contractPayerMapReport">The modeling report.</param>
        /// <returns></returns>
        public ContractPayerMapReport Get(ContractPayerMapReport contractPayerMapReport)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            if (contractPayerMapReport != null)
            {
                var payerMapReport = new ContractPayerMapReport();
                var contractPayerMapReports = new List<ContractPayerMapReport>();

                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetPayerMappingReport");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@NodeID", DbType.Int64, contractPayerMapReport.NodeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Reporttype", DbType.String, contractPayerMapReport.ReportType);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Requestedusername", DbType.String, contractPayerMapReport.LoggedInUser);
                _databaseCommandObj.CommandTimeout = contractPayerMapReport.CommandTimeoutForModelingReport;
                // Retrieve the results of the Stored Procedure in Datatable
                DataSet payerMapDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

                if (payerMapDataSet.IsTableDataPopulated(0))
                {
                    //populating ContractBasicInfo data
                    DataTable dataTable = payerMapDataSet.Tables[0];
                    contractPayerMapReports.AddRange(dataTable.Rows.Cast<object>().Select((t, indexCount) => new ContractPayerMapReport
                    {
                        ContractId = Convert.ToInt64(payerMapDataSet.Tables[0].Rows[indexCount]["ContractId"]),
                        ContractName = Convert.ToString(payerMapDataSet.Tables[0].Rows[indexCount]["ContractName"]),
                        PayerName = textInfo.ToTitleCase(Convert.ToString(payerMapDataSet.Tables[0].Rows[indexCount]["PayerName"]).ToLower()),
                        ClaimCount = Convert.ToInt64(payerMapDataSet.Tables[0].Rows[indexCount]["ClaimCount"]),
                        TotalClaimCharges = Convert.ToDouble(payerMapDataSet.Tables[0].Rows[indexCount]["ClaimTotal"]),
                        BilledDate = DBNull.Value == payerMapDataSet.Tables[0].Rows[indexCount]["FirstBilledDate"] ? string.Empty : String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(payerMapDataSet.Tables[0].Rows[indexCount]["FirstBilledDate"])),
                        StatementThrough = DBNull.Value == payerMapDataSet.Tables[0].Rows[indexCount]["FirstStatementThrough"] ? string.Empty : String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(payerMapDataSet.Tables[0].Rows[indexCount]["FirstStatementThrough"])),
                        FacilityName = Convert.ToString(payerMapDataSet.Tables[0].Rows[indexCount]["FacilityName"]),
                        ModelName = Convert.ToString(payerMapDataSet.Tables[0].Rows[indexCount]["ModelName"]),
                        IsActive = DBNull.Value != payerMapDataSet.Tables[0].Rows[indexCount]["IsActive"] && Convert.ToBoolean(payerMapDataSet.Tables[0].Rows[indexCount]["IsActive"]),
                        Priority = Convert.ToInt16(payerMapDataSet.Tables[0].Rows[indexCount]["Priority"]),
                    }));

                    payerMapReport.FacilityName = contractPayerMapReports[0].FacilityName;
                    payerMapReport.ReportDate = DateTime.UtcNow.ToShortDateString();
                    payerMapReport.UserName = contractPayerMapReport.UserName;
                    payerMapReport.ContractPayerMapReports = contractPayerMapReports;
                    payerMapReport.ReportType = contractPayerMapReport.ReportType;
                    payerMapReport.ModelName = contractPayerMapReports[0].ModelName;
                }

                return payerMapReport;
            }
            return null;
        }
    }
}
