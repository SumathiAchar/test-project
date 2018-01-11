using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;
using System;
using System.Data;
using System.Data.Common;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class AuditLogReportRepository : BaseRepository, IAuditLogReportRepository
    {
        // Variables
        private Database _databaseObj;
        DbCommand _databaseCommandObj;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditLogReportRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public AuditLogReportRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the claim adjudication report.
        /// </summary>
        /// <param name="auditLogReport">The audit log report.</param>
        /// <returns></returns>
        public AuditLogReport GetAuditLogReport(AuditLogReport auditLogReport)
        {
            AuditLogReport auditReport = new AuditLogReport();
              // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetAuditLogReport");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityName ", DbType.String,
                    auditLogReport.FacilityName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@StartDate ", DbType.DateTime,
                    auditLogReport.StartDate);
                _databaseObj.AddInParameter(_databaseCommandObj, "@EndDate ", DbType.DateTime,
                    auditLogReport.EndDate);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String,
                    auditLogReport.UserName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@MaxLinesForCsvReport", DbType.Int32,
                    auditLogReport.MaxLinesForCsvReport);
                _databaseCommandObj.CommandTimeout = auditLogReport.CommandTimeoutForAuditLog;
                DataSet auditLogReportDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);
                if (auditLogReportDataSet.IsTableDataPopulated(0))
                {
                    if (auditLogReportDataSet.Tables[0].Columns.Count == 1)
                    {
                        auditReport.MaxLinesForCsvReport = Constants.ReportThreshold;
                        return auditReport;
                    }
                    //FIXED-NOV15 - Use common GetValue method to get value from DataColumn
                    auditReport.AuditLogReportList =
                        (from DataRow auditDataRow in auditLogReportDataSet.Tables[0].Rows
                            select new AuditLogReport
                            {
                                AuditLogId = GetValue<long>(auditDataRow["AuditLogId"], typeof (long)),
                                LoggedDate =
                                    GetValue<DateTime>(
                                        Utilities.GetLocalTimeString(auditLogReport.CurrentDateTime,
                                            Convert.ToDateTime(auditDataRow["LoggedDate"])), typeof (DateTime)),
                                UserName = GetStringValue(auditDataRow["UserName"]),
                                Action = GetStringValue(auditDataRow["Action"]),
                                ObjectType = GetStringValue(auditDataRow["ObjectType"]),
                                FacilityName = GetStringValue(auditDataRow["FacilityName"]),
                                ModelName = GetStringValue(auditDataRow["ModelName"]),
                                ContractName = GetStringValue(auditDataRow["ContractName"]),
                                ServiceTypeName =
                                    GetStringValue(auditDataRow["ServiceTypeName"]),
                                Description = GetStringValue(auditDataRow["Description"]),
                            }).ToList();
                }
            
            return auditReport;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseCommandObj.Dispose();
            _databaseObj = null;
        }
    }
}
