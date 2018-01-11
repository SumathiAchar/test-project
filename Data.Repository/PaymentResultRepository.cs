using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{

    /// <summary>
    /// Repository for Payment result
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class PaymentResultRepository : IPaymentResultRepository
    {
        // Variables
        private SqlDatabase _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="LetterTemplateRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentResultRepository(string connectionString)
        {
            _databaseObj = new SqlDatabase(connectionString);
        }

        /// <summary>
        /// Updates the payment results.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="noOfRecords">The no of records.</param>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="adjudicatedClaims">The adjudicated claims.</param>
        /// <param name="earlyExitClaims">The early exit claims.</param>
        /// <returns></returns>
        public bool UpdatePaymentResults(List<PaymentResult> paymentResults, int noOfRecords, long taskId, List<EvaluateableClaim> adjudicatedClaims, List<EvaluateableClaim> earlyExitClaims)
        {
            int startIndex = 0;
            int range = noOfRecords;
            bool isRunning = true;
            if (paymentResults != null && paymentResults.Count > 0)
            {
                //Logic for deleting existing records based on claimid and ModelId
                var claimIdList = paymentResults.Select(q => q.ClaimId).Distinct();
                string claimIds = string.Join(",", claimIdList.ToArray());
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("DeletePaymentResultData");
                _databaseObj.AddInParameter(_databaseCommandObj, "@TaskID", DbType.Int64, taskId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ClaimIDs", DbType.String, claimIds.ToTrim());
                _databaseCommandObj.CommandTimeout = 3600;
                int runningStatus = Convert.ToInt16(_databaseObj.ExecuteScalar(_databaseCommandObj));
                if (runningStatus != Convert.ToInt16(Enums.JobStatus.Paused, CultureInfo.InvariantCulture))
                {
                    //Logic for Inserting new records in DB
                    int remainingRecords = paymentResults.Count;
                    while (remainingRecords > 0)
                    {
                        if (remainingRecords >= noOfRecords)
                        {
                            remainingRecords -= noOfRecords;
                        }
                        else
                        {
                            range = remainingRecords;
                            remainingRecords = 0;
                        }
                        List<PaymentResult> paymentResultRanges = paymentResults.GetRange(startIndex, range);

                        //end delete
                        DataTable convertToDataTable = Utilities.ToDataTable(paymentResultRanges);
                        convertToDataTable.Columns.Remove("ServiceLineCode");
                        convertToDataTable.Columns.Remove("ServiceLineDate");
                        _databaseCommandObj = _databaseObj.GetStoredProcCommand("AddPaymentResultData");
                        _databaseCommandObj.CommandTimeout = 3600;
                        _databaseObj.AddInParameter(_databaseCommandObj, "@TaskID", SqlDbType.BigInt, taskId);
                        _databaseObj.AddInParameter(_databaseCommandObj, "@ServerDateTime", SqlDbType.DateTime,
                            DateTime.UtcNow);
                        _databaseObj.AddInParameter(_databaseCommandObj, "@IsDmEntry", SqlDbType.Bit,
                            GlobalConfigVariable.IsDmEntry);
                        _databaseObj.AddInParameter(_databaseCommandObj, "@XmlAdjudicatedData", SqlDbType.Structured,
                            convertToDataTable);

                        _databaseObj.ExecuteNonQuery(_databaseCommandObj);


                        startIndex += noOfRecords;
                    }
                }
                else
                {
                    isRunning = false;
                }
            }
            if (isRunning)
            {
                isRunning = UpdateEarlyExitPaymentResults(adjudicatedClaims, earlyExitClaims, taskId);
            }
            return isRunning;
        }

        /// <summary>
        /// Updates the early exit payment results.
        /// </summary>
        /// <param name="adjudicatedClaims">The adjudicated claims.</param>
        /// <param name="earlyExitClaims">The early exit claims.</param>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        private bool UpdateEarlyExitPaymentResults(IEnumerable<EvaluateableClaim> adjudicatedClaims, IEnumerable<EvaluateableClaim> earlyExitClaims, long taskId)
        {

            _databaseCommandObj = _databaseObj.GetStoredProcCommand("SaveAdjudicatedClaimsContractID");
            _databaseCommandObj.CommandTimeout = 3600;
            _databaseObj.AddInParameter(_databaseCommandObj, "@TaskID", SqlDbType.BigInt, taskId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@AdjudicatedClaims", SqlDbType.Structured, GetAdjudicatedClaimDataTable(adjudicatedClaims));
            _databaseObj.AddInParameter(_databaseCommandObj, "@EarlyExitClaims", SqlDbType.Structured, GetAdjudicatedClaimDataTable(earlyExitClaims));

            int runningStatus = Convert.ToInt16(_databaseObj.ExecuteScalar(_databaseCommandObj));
            return (runningStatus != Convert.ToInt16(Enums.JobStatus.Paused, CultureInfo.InvariantCulture));
        }


        /// <summary>
        /// Gets the adjudicated claim data table.
        /// </summary>
        /// <param name="adjudicatedClaims">The adjudicated claims.</param>
        /// <returns></returns>
        private DataTable GetAdjudicatedClaimDataTable(IEnumerable<EvaluateableClaim> adjudicatedClaims)
        {
            DataTable adjudicatedClaimDataTable = new DataTable();
            adjudicatedClaimDataTable.Columns.Add("ContractID", typeof(long));
            adjudicatedClaimDataTable.Columns.Add("ClaimID", typeof(string));

            foreach (EvaluateableClaim adjudicatedClaim in adjudicatedClaims)
            {
                DataRow reassignClaimDataRow = adjudicatedClaimDataTable.NewRow();
                reassignClaimDataRow[0] = adjudicatedClaim.ContractId;
                reassignClaimDataRow[1] = Convert.ToString(adjudicatedClaim.ClaimIds);
                
                adjudicatedClaimDataTable.Rows.Add(reassignClaimDataRow);
            }
            return adjudicatedClaimDataTable;
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
