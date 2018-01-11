using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ReassignClaimRepository : BaseRepository, IReassignClaimRepository
    {
        // Variables
        private Database _databaseObj;
        private DbCommand _databaseCommandObj;
        private readonly SqlDatabase _databaseSqlObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReassignClaimRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ReassignClaimRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
            _databaseSqlObj = new SqlDatabase(connectionString);
        }
        /// <summary>
        /// Gets the reassign claims.
        /// </summary>
        /// <param name="claimSearchCriteria"></param>
        /// <returns></returns>
        public ReassignClaimContainer GetReassignGridData(ClaimSearchCriteria claimSearchCriteria)
        {

            ReassignClaimContainer reassignClaimResult = new ReassignClaimContainer();

            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetReassignGridData");
            _databaseObj.AddInParameter(_databaseCommandObj, "@SelectCriteria ", DbType.String,
                claimSearchCriteria.SearchCriteria);
            _databaseObj.AddInParameter(_databaseCommandObj, "@DateType ", DbType.Int32, claimSearchCriteria.DateType);
            _databaseObj.AddInParameter(_databaseCommandObj, "@StartDate ", DbType.DateTime,
                claimSearchCriteria.StartDate);
            _databaseObj.AddInParameter(_databaseCommandObj, "@EndDate ", DbType.DateTime, claimSearchCriteria.EndDate);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, claimSearchCriteria.UserName);
            _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityID", DbType.Int64, claimSearchCriteria.FacilityId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@Take", DbType.Int32, claimSearchCriteria.Take);
            _databaseObj.AddInParameter(_databaseCommandObj, "@Skip", DbType.Int32, claimSearchCriteria.Skip);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserID", DbType.String, claimSearchCriteria.RequestedUserId);

            DataSet reassignClaimDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);
            if (reassignClaimDataSet.IsTableDataPopulated())
            {
                if (reassignClaimDataSet.Tables.Count > 1 && reassignClaimDataSet.Tables[1].Rows.Count > 0)
                {
                    reassignClaimResult.TotalRecords = Convert.ToInt32(reassignClaimDataSet.Tables[1].Rows[0][0]);
                }

                DataRowCollection reassignClaimDataRowCollection = reassignClaimDataSet.Tables[0].Rows;

                List<EvaluateableClaim> evaluateableClaims = (from DataRow row in reassignClaimDataRowCollection
                                                              select new EvaluateableClaim
                                                              {
                                                                  ClaimId = GetValue<long>(row["ClaimID"], typeof(long)),
                                                                  PatientAccountNumber = GetStringValue(row["PatientAccountNumber"]),
                                                                  ClaimType = GetStringValue(row["ClaimType"]),
                                                                  BillType = GetStringValue(row["BillType"]),
                                                                  PriPayerName = GetStringValue(row["PriPayerName"]),
                                                                  ClaimTotal = GetValue<double>(row["ClaimTotal"], typeof(double)),
                                                                  StatementFromValue =
                                                                      DBNull.Value == row["StatementFrom"]
                                                                          ? null
                                                                          : Convert.ToString(row["StatementFrom"].ToString() == "1/1/1900 12:00:00 AM"
                                                                              ? null
                                                                              : Convert.ToDateTime(row["StatementFrom"])
                                                                                  .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)),
                                                                  StatementThruValue =
                                                                      DBNull.Value == row["StatementThru"]
                                                                          ? null
                                                                          : Convert.ToString(row["StatementThru"].ToString() == "1/1/1900 12:00:00 AM"
                                                                              ? null
                                                                              : Convert.ToDateTime(row["StatementThru"])
                                                                                  .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)),
                                                                  BillDateValue =
                                                                        DBNull.Value == row["BillDate"]
                                                                            ? null
                                                                            : Convert.ToString(row["BillDate"].ToString() == "1/1/1900 12:00:00 AM"
                                                                                ? null
                                                                                : Convert.ToDateTime(row["BillDate"])
                                                                                    .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)),
                                                                  IsRetained = GetValue<bool>(row["IsRetained"], typeof(bool)),
                                                                  ModelId = GetValue<long>(row["ModelId"], typeof(long)),
                                                                  ContractId = GetValue<long>(row["ContractId"], typeof(long)),
                                                              }).ToList();

                reassignClaimResult.ClaimData = evaluateableClaims;
            }
            return reassignClaimResult;
        }

        /// <summary>
        /// Adds the reassigned claim job.
        /// </summary>
        /// <param name="reassignedClaimJob">The reassigned claim job.</param>
        /// <returns></returns>
        public bool AddReassignedClaimJob(ReassignedClaimJob reassignedClaimJob)
        {
            if (reassignedClaimJob != null)
            {
                DataTable reassignClaimDataTable = GetReassignClaimDataTable(reassignedClaimJob.ReassignClaim);

                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseSqlObj.GetStoredProcCommand("AddReassignedClaimJob");

                _databaseObj.AddInParameter(_databaseCommandObj, "@RequestName", DbType.String, reassignedClaimJob.RequestName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, reassignedClaimJob.UserName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SearchCriteria", DbType.String, reassignedClaimJob.SearchCriteria);
                _databaseObj.AddInParameter(_databaseCommandObj, "@DateType", DbType.Int64, reassignedClaimJob.DateType);
                _databaseObj.AddInParameter(_databaseCommandObj, "@DateFrom", DbType.DateTime, reassignedClaimJob.DateFrom);
                _databaseObj.AddInParameter(_databaseCommandObj, "@DateTo", DbType.DateTime, reassignedClaimJob.DateTo);
                _databaseObj.AddInParameter(_databaseCommandObj, "@HeaderModelId", DbType.Int64, reassignedClaimJob.ModelId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@HeaderContractId", DbType.Int64, reassignedClaimJob.ContractId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SelectAll", DbType.Boolean, reassignedClaimJob.IsSelectAll);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SelectAllHeader", DbType.Boolean, reassignedClaimJob.IsSelectAllHeader);
                _databaseSqlObj.AddInParameter(_databaseCommandObj, "@ReassignClaims",
                SqlDbType.Structured, reassignClaimDataTable);
                return _databaseObj.ExecuteNonQuery(_databaseCommandObj) > 0;
            }
            return false;
        }

        /// <summary>
        /// Gets the contracts by node identifier.
        /// </summary>
        /// <param name="contractHierarchy">The contract hierarchy.</param>
        /// <returns></returns>
        public List<Contract> GetContractsByNodeId(ContractHierarchy contractHierarchy)
        {
            List<Contract> contractlList = new List<Contract>();
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetContractsByNodeId");
            _databaseObj.AddInParameter(_databaseCommandObj, "@NodeID ", DbType.Int64, contractHierarchy.NodeId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserName ", DbType.String, contractHierarchy.UserName);
            DataSet contractListDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);
            if (contractListDataSet.IsTableDataPopulated())
            {
                DataRowCollection contractListDataRowCollection = contractListDataSet.Tables[0].Rows;
                contractlList = (from DataRow row in contractListDataRowCollection
                                 select new Contract
                                 {
                                     ContractId = GetValue<long>(row["ContractID"], typeof(long)),
                                     ContractName = GetStringValue(row["ContractName"])
                                 }).ToList();
            }
            return contractlList;
        }

        /// <summary>
        /// Gets the reassign claim data table.
        /// </summary>
        /// <param name="reassignedClaimContainer">The reassigned claim container.</param>
        /// <returns></returns>
        private DataTable GetReassignClaimDataTable(IEnumerable<ReassignClaim> reassignedClaimContainer)
        {
            DataTable reassignClaimDataTable = new DataTable();
            reassignClaimDataTable.Columns.Add("ClaimID", typeof(long));
            reassignClaimDataTable.Columns.Add("ModelID", typeof(long));
            reassignClaimDataTable.Columns.Add("ContractID", typeof(long));
            reassignClaimDataTable.Columns.Add("IsRetained", typeof(bool));
            reassignClaimDataTable.Columns.Add("IsSelected", typeof(bool));
            foreach (ReassignClaim reassignedClaim in reassignedClaimContainer)
            {
                DataRow reassignClaimDataRow = reassignClaimDataTable.NewRow();
                reassignClaimDataRow[0] = reassignedClaim.ClaimId;
                reassignClaimDataRow[1] = reassignedClaim.ModelId;
                reassignClaimDataRow[2] = reassignedClaim.ContractId ?? (object)DBNull.Value;
                reassignClaimDataRow[3] = reassignedClaim.IsRetained;
                reassignClaimDataRow[4] = reassignedClaim.IsSelected;
                reassignClaimDataTable.Rows.Add(reassignClaimDataRow);
            }
            return reassignClaimDataTable;
        }

        /// <summary>
        /// Gets the claim linked count.
        /// </summary>
        /// <param name="contractHierarchy">The contract hierarchy.</param>
        /// <returns></returns>
        public int GetClaimLinkedCount(ContractHierarchy contractHierarchy)
        {
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetClaimLinkedCount");
            _databaseObj.AddInParameter(_databaseCommandObj, "@NodeID", DbType.Int64, contractHierarchy.NodeId);
            return (int)_databaseObj.ExecuteScalar(_databaseCommandObj);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseObj = null;
            _databaseCommandObj.Dispose();
        }
    }
}
