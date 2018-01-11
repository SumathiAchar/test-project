using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ModelComparisonReportRepository : IModelComparisonReportRepository
    {
        private Database _databaseObj;
        private DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelComparisonReportRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ModelComparisonReportRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }
        /// <summary>
        /// Gets the available models.
        /// </summary>
        /// <param name="modelComparisonForPost">The model comparison for post.</param>
        /// <returns></returns>
        public List<ModelComparisonReport> GetModels(ModelComparisonReport modelComparisonForPost)
        {
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetModelsByFacilityId");
            _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityID ", DbType.Int64, modelComparisonForPost.FacilityId);

            // Retrieve the results of the Stored Procedure in Datatable
            DataSet modelComparisonDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            if (modelComparisonDataSet.IsTableDataPopulated(0) && modelComparisonDataSet.Tables[0].Rows != null && modelComparisonDataSet.Tables[0].Rows.Count > 0)
            {
               
                List<ModelComparisonReport> availableModels = (from DataRow row in modelComparisonDataSet.Tables[0].Rows
                                   select new ModelComparisonReport
                                        {
                                            NodeId = long.Parse(row["NodeID"].ToString()),
                                            ModelName = Convert.ToString(row["NodeText"])
                                        }).ToList();

                return availableModels;
            }

            return null;
        }


        /// <summary>
        /// Generates the model compassion report.
        /// </summary>
        /// <param name="modelComparisonForPost">The model comparison for post.</param>
        /// <returns></returns>
        public ModelComparisonReport Generate(ModelComparisonReport modelComparisonForPost)
        {
            ModelComparisonReport modelComparisonReportData = new ModelComparisonReport();

            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetModelComparisonReport");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommandObj, "@SelectedModels ", DbType.String, modelComparisonForPost.SelectedModelList);
            _databaseObj.AddInParameter(_databaseCommandObj, "@IsDetailedView ", DbType.Int32, modelComparisonForPost.IsCheckedDetailLevel);
            _databaseObj.AddInParameter(_databaseCommandObj, "@DetailedViewAt ", DbType.Int32, modelComparisonForPost.DetailSelectValue);
            _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityId ", DbType.Int32, modelComparisonForPost.FacilityId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@DateType", DbType.Int32, modelComparisonForPost.DateType);
            _databaseObj.AddInParameter(_databaseCommandObj, "@DateFrom", DbType.DateTime, modelComparisonForPost.StartDate);
            _databaseObj.AddInParameter(_databaseCommandObj, "@DateTo", DbType.DateTime, modelComparisonForPost.EndDate);
            _databaseObj.AddInParameter(_databaseCommandObj, "@SelectCriteria", DbType.String, modelComparisonForPost.ClaimSearchCriteria);
            _databaseObj.AddInParameter(_databaseCommandObj, "@RequestedUserID", DbType.String, modelComparisonForPost.RequestedUserId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@RequestedUserName", DbType.String, modelComparisonForPost.RequestedUserName);

            // Retrieve the results of the Stored Procedure in Data table
            _databaseCommandObj.CommandTimeout = 1000;
            DataSet modelComparisonDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);
            if (modelComparisonDataSet.IsTableDataPopulated())
            {
                List<ModelComparisonReportDetails> modelComparisonReportList = new List<ModelComparisonReportDetails>();

                DataRowCollection modelComparisonRowCollection = modelComparisonDataSet.Tables[0].Rows;
                for (int indexCount = 0; indexCount < modelComparisonRowCollection.Count; indexCount++)
                {
                    ModelComparisonReportDetails modelComparisonReportDetails = new ModelComparisonReportDetails();
                    DataRow modelComparisonRow = modelComparisonRowCollection[indexCount];
                    EvaluateableClaim claimData = new EvaluateableClaim
                    {
                        NodeText = DBNull.Value == modelComparisonRow["ModelName"] ? string.Empty : modelComparisonRow["ModelName"].ToString(),
                        ModelId = DBNull.Value == modelComparisonRow["ModelId"] ? (int?)null : Convert.ToInt32(modelComparisonRow["ModelId"]),
                        ClaimTotal = DBNull.Value == modelComparisonRow["ClaimTotalCharges"] ? (double?)null : Convert.ToDouble(modelComparisonRow["ClaimTotalCharges"]),
                        AdjudicatedValue = DBNull.Value == modelComparisonRow["AdjudicatedValue"] ? (double?)null : Convert.ToDouble(modelComparisonRow["AdjudicatedValue"]),
                        CalculatedAdjustment = DBNull.Value == modelComparisonRow["CalculatedAdjudtment"] ? (double?)null : Convert.ToDouble(modelComparisonRow["CalculatedAdjudtment"]),
                        ActualPayment = DBNull.Value == modelComparisonRow["ActualPayment"] ? (double?)null : Convert.ToDouble(modelComparisonRow["ActualPayment"]),
                        ActualAdjustment = DBNull.Value == modelComparisonRow["ActualAdjustment"] ? (double?)null : Convert.ToDouble(modelComparisonRow["ActualAdjustment"]),
                        PatientResponsibility = DBNull.Value == modelComparisonRow["PatientResponsibility"] ? (double?)null : Convert.ToDouble(modelComparisonRow["PatientResponsibility"]),
                        RemitAllowedAmt = DBNull.Value == modelComparisonRow["RemitAllowed"] ? (double?)null : Convert.ToDouble(modelComparisonRow["RemitAllowed"]),
                        RemitNonCovered = DBNull.Value == modelComparisonRow["RemitNonCovered"] ? (double?)null : Convert.ToDouble(modelComparisonRow["RemitNonCovered"]),
                        RemitContrAdj = DBNull.Value == modelComparisonRow["RemitNonCovered"] ? (double?)null : Convert.ToDouble(modelComparisonRow["RemitNonCovered"]),
                        ContractualVariance = DBNull.Value == modelComparisonRow["ContractualVariance"] ? (double?)null : Convert.ToDouble(modelComparisonRow["ContractualVariance"]),
                        PaymentVariance = DBNull.Value == modelComparisonRow["PaymentVariance"] ? (double?)null : Convert.ToDouble(modelComparisonRow["PaymentVariance"]),
                        DetailedSelection = DBNull.Value == modelComparisonRow["DetailedSelection"] ? string.Empty : modelComparisonRow["DetailedSelection"].ToString()
                    };
                    modelComparisonReportDetails.ClaimData = claimData;
                    modelComparisonReportDetails.Count = DBNull.Value == modelComparisonRow["ClaimCount"]
                        ? string.Empty
                        : modelComparisonRow["ClaimCount"].ToString();
                    modelComparisonReportList.Add(modelComparisonReportDetails);
                }
                modelComparisonReportData.ModelComparisonData = modelComparisonReportList;
            }

            return modelComparisonReportData;
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

