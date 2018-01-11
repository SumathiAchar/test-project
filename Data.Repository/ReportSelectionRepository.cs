using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ReportSelectionRepository : BaseRepository, IReportSelectionRepository
    {

        private Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportSelectionRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ReportSelectionRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }
        /// <summary>
        /// Gets all claim fields.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<ClaimFieldOperator> GetAllClaimFieldsOperators()
        {
            List<ClaimFieldOperator> claimFieldList = new List<ClaimFieldOperator>();

            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetAllClaimFieldOperators");
            DataSet claimFieldOperatorsDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            if (claimFieldOperatorsDataSet.IsTableDataPopulated(0))
            {
                for (int i = 0; i < claimFieldOperatorsDataSet.Tables[0].Rows.Count; i++)
                {
                    ClaimFieldOperator claimField = new ClaimFieldOperator
                    {
                        OperatorId = long.Parse(claimFieldOperatorsDataSet.Tables[0].Rows[i]["OperatorID"].ToString()),
                        OperatorType = Convert.ToString(claimFieldOperatorsDataSet.Tables[0].Rows[i]["OperatorType"])
                    };
                    claimFieldList.Add(claimField);
                }
            }

            return claimFieldList;
        }

        /// <summary>
        /// Gets all claim fields.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<ClaimField> GetAllClaimFields(ClaimSelector reportClaimSelectorInfo)
        {

            List<ClaimField> claimFieldList = new List<ClaimField>();

            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetClaimFieldsByModuleId");
            _databaseObj.AddInParameter(_databaseCommandObj, "@ModuleId", DbType.Int64, reportClaimSelectorInfo.ModuleId);
            DataSet claimFieldDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            if (claimFieldDataSet.IsTableDataPopulated(0))
            {
                for (int i = 0; i < claimFieldDataSet.Tables[0].Rows.Count; i++)
                {
                    ClaimField claimField = new ClaimField
                    {
                        ClaimFieldId = long.Parse(claimFieldDataSet.Tables[0].Rows[i]["ClaimFieldId"].ToString()),
                        Text = Convert.ToString(claimFieldDataSet.Tables[0].Rows[i]["Text"])
                    };
                    claimFieldList.Add(claimField);
                }
            }

            return claimFieldList;
        }

        /// <summary>
        /// Gets the claim reviewed option.
        /// </summary>
        /// <returns></returns>
        //FIXED-NOV15 Change ReviewedOptionId from Long to byte and make the necessary change in table structure.
        public List<ReviewedOptionType> GetClaimReviewedOption()
        {
            List<ReviewedOptionType> reviewdOptions = new List<ReviewedOptionType>();

            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetAllReviewedOption");
            DataSet reviewdOptionDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            if (reviewdOptionDataSet.IsTableDataPopulated(0))
            {
                for (int index = 0; index < reviewdOptionDataSet.Tables[0].Rows.Count; index++)
                {
                    ReviewedOptionType reviewdOption = new ReviewedOptionType
                    {
                        ReviewedOptionId = Convert.ToByte(reviewdOptionDataSet.Tables[0].Rows[index]["ReviewedOptionID"]),
                        ReviewedOption = Convert.ToString(reviewdOptionDataSet.Tables[0].Rows[index]["ReviewedOption"])
                    };
                    reviewdOptions.Add(reviewdOption);
                }
            }

            return reviewdOptions;
        }

        /// <summary>
        /// Gets ajudication request names based on the model id and use rname.
        /// </summary>
        /// <returns> List of ClaimSelector</returns>
        public List<ClaimSelector> GetAdjudicationRequestNames(ClaimSelector claimSelector)
        {
            List<ClaimSelector> adjudicationRequestList = new List<ClaimSelector>();

            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetAdjudicationRequestNames");
            _databaseObj.AddInParameter(_databaseCommandObj, "@ModelId", DbType.Int64, claimSelector.ModelId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@Runningstatus", DbType.Int32, (byte)Enums.JobStatus.Completed);
            _databaseObj.AddInParameter(_databaseCommandObj, "@NoOfDaysToDismissCompletedJobs", DbType.Int32, claimSelector.CompletedJobsDuration);
            _databaseCommandObj.CommandTimeout = claimSelector.CommandTimeoutForGetAdjudicationRequestNames;
            // Retrieve the results of the Stored Procedure in Data table
            DataSet dsAdjudicationRequestNames = _databaseObj.ExecuteDataSet(_databaseCommandObj);
            if (dsAdjudicationRequestNames.IsTableDataPopulated(0))
            {
                for (int i = 0; i < dsAdjudicationRequestNames.Tables[0].Rows.Count; i++)
                {
                    ClaimSelector adjudicationRequest = new ClaimSelector
                    {
                        RequestName = dsAdjudicationRequestNames.Tables[0].Rows[i]["RequestName"].ToString(),
                        ClaimSelectorId = Convert.ToInt64(dsAdjudicationRequestNames.Tables[0].Rows[i]["TaskID"])
                    };
                    adjudicationRequestList.Add(adjudicationRequest);

                }
            }

            return adjudicationRequestList;
        }

        /// <summary>
        /// Add or edit query name.
        /// </summary>
        /// <param name="claimSelector"></param>
        /// <returns></returns>
        public int AddEditQueryName(ClaimSelector claimSelector)
        {
            if (claimSelector != null)
            {
                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("AddEditQueryName");
                // Pass parameters to Stored Procedure
                _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityID", DbType.Int32, claimSelector.FacilityId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserID", DbType.Int32, claimSelector.UserId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@QueryID", DbType.Int32, claimSelector.QueryId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@QueryName", DbType.String, claimSelector.QueryName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, claimSelector.UserName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityName", DbType.String, claimSelector.FacilityName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Criteria", DbType.String, claimSelector.SelectCriteria);
                return Convert.ToInt32(_databaseObj.ExecuteScalar(_databaseCommandObj));
            }
            return -1;
        }

        /// <summary>
        /// Delete query name with criteria.
        /// </summary>
        /// <param name="claimSelector"></param>
        /// <returns></returns>
        public bool DeleteQueryName(ClaimSelector claimSelector)
        {
            if (claimSelector != null)
            {
                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("DeleteQueryNameByID");
                // Pass parameters to Stored Procedure
                _databaseObj.AddInParameter(_databaseCommandObj, "@QueryID", DbType.Int32, claimSelector.QueryId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, claimSelector.UserName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityName", DbType.String, claimSelector.FacilityName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@QueryName", DbType.String, claimSelector.QueryName);
                return (Convert.ToInt32(_databaseObj.ExecuteScalar(_databaseCommandObj)) == 1);
            }
            return false;
        }

        /// <summary>
        /// Get Quries By Id
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public List<ClaimSelector> GetQueriesById(User userInfo)
        {
            List<ClaimSelector> queryList = new List<ClaimSelector>();
            if (userInfo != null)
            {
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetQueriesById");
                _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityID", DbType.Int32, userInfo.FacilityId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserID", DbType.Int32, userInfo.UserId);

                DataSet dsQueryName = _databaseObj.ExecuteDataSet(_databaseCommandObj);
                if (dsQueryName.IsTableDataPopulated(0) && dsQueryName.Tables[0].Rows.Count > 0)
                {
                    DataRow[] queryDataRows = dsQueryName.Tables[0].Select();
                    var queries = queryDataRows.Select(queryRow => new ClaimSelector
                    {
                        QueryId = GetValue<int>(queryRow["ReportQueryID"], typeof(int)),
                        QueryName = GetStringValue(queryRow["QueryName"]),
                        SelectCriteria = GetStringValue(queryRow["Criteria"]),
                        CriteriaDetails = GetStringValue(queryRow["CriteriaDetails"])
                    }).ToList();
                    queryList = queries;
                }
            }
            return queryList;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseCommandObj.Dispose();
            _databaseObj = null;
        }
    }
}
