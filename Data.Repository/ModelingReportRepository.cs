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
    public class ModelingReportRepository : IModelingReportRepository
    {

        private Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelingReportRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ModelingReportRepository(string connectionString)
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
        /// <param name="modelingReport">The modeling report.</param>
        /// <returns></returns>
        public ModelingReport GetAllModelingDetails(ModelingReport modelingReport)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            if (modelingReport != null)
            {
                var modelingDetails = new ModelingReport();
                var modelingReportlist = new List<ModelingReport>();

                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetCMSModellingReport");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@NodeID", DbType.Int64, modelingReport.NodeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@IsActive", DbType.String, modelingReport.IsActive);
                _databaseObj.AddInParameter(_databaseCommandObj, "@LoggedInUser", DbType.String, modelingReport.LoggedInUser);
                _databaseCommandObj.CommandTimeout = modelingReport.CommandTimeoutForModelingReport;
                // Retrieve the results of the Stored Procedure in Datatable
                DataSet modelingDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

                long? totalRecords = 0;
                if (modelingDataSet.IsTableDataPopulated(1) && modelingDataSet.Tables[1].Rows[0]["TotalRecords"] != DBNull.Value)
                    totalRecords = Convert.ToInt64(modelingDataSet.Tables[1].Rows[0]["TotalRecords"]);
                if (modelingDataSet.IsTableDataPopulated(0))
                {
                    //populating ContractBasicInfo data
                    DataTable dataTable = modelingDataSet.Tables[0];
                    modelingReportlist.AddRange(dataTable.Rows.Cast<object>().Select((t, indexCount) => new ModelingReport
                    {
                        ContractId = Convert.ToInt64(modelingDataSet.Tables[0].Rows[indexCount]["ContractId"]),
                        ContractName = Convert.ToString(modelingDataSet.Tables[0].Rows[indexCount]["Contract"]),
                        PayerName = textInfo.ToTitleCase(Convert.ToString(modelingDataSet.Tables[0].Rows[indexCount]["Payers"]).ToLower()),
                        StartDate = Convert.ToDateTime(modelingDataSet.Tables[0].Rows[indexCount]["BeginEffectiveDate"]),
                        EndDate = Convert.ToDateTime(modelingDataSet.Tables[0].Rows[indexCount]["EndEffectiveDate"]),
                        ServiceType = Convert.ToString(modelingDataSet.Tables[0].Rows[indexCount]["ServType"]),
                        ClaimTools = Convert.ToString(modelingDataSet.Tables[0].Rows[indexCount]["ServiceLine"]).Replace(Environment.NewLine, ""),
                        PaymentTool = Convert.ToString(modelingDataSet.Tables[0].Rows[indexCount]["PaymentType"]).Replace(Environment.NewLine, ""),
                        FacilityName = Convert.ToString(modelingDataSet.Tables[0].Rows[indexCount]["FacilityName"]),
                        ModelName = Convert.ToString(modelingDataSet.Tables[0].Rows[indexCount]["ModelName"]),
                        IsCarveOut = Convert.ToString(modelingDataSet.Tables[0].Rows[indexCount]["IsCarveOut"]).Equals("true", StringComparison.CurrentCultureIgnoreCase),
                        IsContractSpecific = Convert.ToString(modelingDataSet.Tables[0].Rows[indexCount]["IsContractSpecific"]).Equals("true", StringComparison.CurrentCultureIgnoreCase),
                        TotalRecords = totalRecords,
                        PayerCodes = Convert.ToString(modelingDataSet.Tables[0].Rows[indexCount]["PayerCode"])
                    }));

                    modelingDetails.FacilityName = modelingReportlist[0].FacilityName;
                    modelingDetails.ReportDate = DateTime.UtcNow.ToShortDateString();
                    modelingDetails.UserName = modelingReport.UserName;
                    modelingDetails.IsActive = modelingReport.IsActive;
                    modelingDetails.ModelingReports = modelingReportlist;
                }

                return modelingDetails;
            }
            return null;
        }
    }
}
