using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;
using System.Data;
using System.Linq;
using SSI.ContractManagement.Shared.Helpers;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class RtaRepository : BaseRepository, IRtaRepository
    {
        private Database _databaseObj;
        DbCommand _databaseCommandObj;

        private const int MedicareTablePosition = 1;
        private const int FacilityTablePosition = 0;
        private const int ContractBasicInfoTablePosition = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RtaRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public RtaRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the rta data by claim.
        /// </summary>
        /// <param name="evaluateableClaim">The evaluate able claim.</param>
        /// <returns></returns>
        public RtaData
            GetRtaDataByClaim(EvaluateableClaim evaluateableClaim)
        {
            RtaData rtaData = new RtaData { EvaluateableClaim = evaluateableClaim, Contracts = new List<Contract>() };

            if (evaluateableClaim != null)
            {

                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("RTA_GetDataByClaim");

                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@SsiNumber", DbType.Int32, evaluateableClaim.Ssinumber);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Hcpcs", DbType.String, string.Join(",", evaluateableClaim.ClaimCharges.Select(q => q.HcpcsCode)));
                _databaseObj.AddInParameter(_databaseCommandObj, "@ZipCode", DbType.String, evaluateableClaim.ProviderZip);
                _databaseObj.AddInParameter(_databaseCommandObj, "@StatementFrom", DbType.DateTime, evaluateableClaim.StatementFrom);
                _databaseObj.AddInParameter(_databaseCommandObj, "@StatementThru", DbType.DateTime, evaluateableClaim.StatementThru);
                _databaseObj.AddInParameter(_databaseCommandObj, "@PayerName", DbType.String, evaluateableClaim.PriPayerName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@IsInstitutional", DbType.Boolean, evaluateableClaim.ClaimType.ToLower().Contains(Constants.ClaimTypeInstitutionalContract));

                // Retrieve the results of the Stored Procedure in Data table
                DataSet dataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

                if (dataSet.Tables.Count > 0)
                {
                    rtaData.FacilityId = dataSet.Tables[0].Rows[0]["FacilityID"] == DBNull.Value
                        ? (long?)null
                        : Convert.ToInt64(dataSet.Tables[0].Rows[0]["FacilityID"]);
                    if (rtaData.FacilityId != null)
                    {
                        rtaData.EvaluateableClaim.MedicareLabFeeSchedules = GetMedicareLabFeeSchedules(dataSet);
                        //Remove RTA related Tables from dataSet and than pass dataSet to CMS logic to build List<Contract>
                        dataSet.Tables.RemoveAt(MedicareTablePosition); //Remove Medicare Lab Fee Schedule Table
                        dataSet.Tables.RemoveAt(FacilityTablePosition); //Remove Facility Table
                        DataTable contractBasicInfo = dataSet.Tables[0];
                        dataSet.Tables.RemoveAt(ContractBasicInfoTablePosition);//Remove Contract Basic Info Table.
                        rtaData.Contracts = ContractRepository.GetContracts(dataSet);
                        rtaData.Contracts.AddRange(ContractRepository.GetContracts(contractBasicInfo));
                    }
                }
            }
            return rtaData;
        }

        /// <summary>
        /// Gets the Medicare lab fee schedules.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        private List<MedicareLabFeeSchedule> GetMedicareLabFeeSchedules(DataSet dataSet)
        {
            return (from DataRow row in dataSet.Tables[1].Rows
                    select new MedicareLabFeeSchedule
                    {
                        Hcpcs = Convert.ToString(row["HCPCS"]),
                        Amount = Convert.ToDouble(row["Amount"]),
                        ProviderZip = Convert.ToString(row["ZipCode"])
                    }).ToList();
        }


        /// <summary>
        /// Saves the time log.
        /// </summary>
        /// <param name="rtaEdiTimeLog">The rta edi time log.</param>
        /// <returns></returns>
        public long SaveTimeLog(RtaEdiTimeLog rtaEdiTimeLog)
        {
            //Checks if input Timelog is not null
            if (rtaEdiTimeLog != null)
            {
                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("RTA_AddTimeLog");
                _databaseCommandObj.CommandTimeout = 5400;
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@TimeTaken", DbType.String, rtaEdiTimeLog.TimeTaken);
                _databaseObj.AddInParameter(_databaseCommandObj, "@RequestType", DbType.String, rtaEdiTimeLog.RequestType);
                _databaseObj.AddInParameter(_databaseCommandObj, "@EdiResponseID", DbType.Int64, rtaEdiTimeLog.EdiResponseId);

                // Retrieve the results of the Stored Procedure
                long logId;
                long.TryParse(_databaseObj.ExecuteScalar(_databaseCommandObj).ToString(), out logId);
                return logId;

            }
            return 0;
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
