using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class JobStatusRepository : BaseRepository, IJobStatusRepository
    {
        /// <summary>
        /// The _database obj variable
        /// </summary>
        private Database _databaseObj;

        /// <summary>
        /// The _database command variable
        /// </summary>
        private DbCommand _databaseCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobStatusRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public JobStatusRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets all jobs.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public List<TrackTask> GetAllJobs(TrackTask data)
        {
            List<TrackTask> jobs = new List<TrackTask>();


            //Lots of magic integers are used like 29,130 and no comments are there for those what does it do
            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetAllJobs");
            _databaseObj.AddInParameter(_databaseCommand, "@FacilityID", DbType.Int64, data.FacilityId);
            _databaseObj.AddInParameter(_databaseCommand, "@RunningStatus", DbType.Int32,
                Convert.ToInt32(data.Status.ToTrim()));
            _databaseObj.AddInParameter(_databaseCommand, "@Take", DbType.String, data.PageSetting.Take);
            _databaseObj.AddInParameter(_databaseCommand, "@Skip", DbType.String, data.PageSetting.Skip);

            // Retrieve the results of the Stored Procedure 
            DataSet jobsDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);

            DataRow[] myJobs = jobsDataSet.Tables[0].Select();

            jobs.AddRange(myJobs.Select(ssiJobTrack => new TrackTask
            {
                RequestName =
                    GetStringValue(ssiJobTrack["RequestName"]),
                TaskId = GetStringValue(ssiJobTrack["JobId"]),
                UserName = GetStringValue(ssiJobTrack["UserName"]),
                Status = Enum.GetName(typeof(Enums.JobStatus), ssiJobTrack["STATUS"]),
                ModelId = GetValue<long>(ssiJobTrack["ModelId"], typeof(long)),
                ElapsedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    TimeSpan.FromSeconds(Convert.ToDouble(ssiJobTrack["ElapsedTime"])).Days,
                    TimeSpan.FromSeconds(Convert.ToDouble(ssiJobTrack["ElapsedTime"])).Hours,
                    TimeSpan.FromSeconds(Convert.ToDouble(ssiJobTrack["ElapsedTime"])).Minutes),
                ClaimsSelectionCount =
                    GetStringValue(
                        ssiJobTrack["NoOfClaimsSelected"]),
                AdjudicatedClaimsCount =
                    GetStringValue(
                        ssiJobTrack["NoOfClaimsAdjudicated"]),
                IsVerified = DBNull.Value != ssiJobTrack["IsVerified"] &&
                             GetValue<bool>((ssiJobTrack["IsVerified"]), typeof(bool)),
                ModelName = GetStringValue(ssiJobTrack["ModelName"]),
                Criteria = GetStringValue(ssiJobTrack["Criteria"])
            }));

            var firstOrDefault = jobs.FirstOrDefault();
            if (firstOrDefault != null)
                firstOrDefault.TotalJobs = Convert.ToInt32(jobsDataSet.Tables[1].Rows[0][0]);

            return jobs;
        }

        /// <summary>
        /// Gets all jobs for adjudication.
        /// </summary>
        /// <param name="jobStatusCodesIncluded"></param>
        /// <returns></returns>
        public List<TrackTask> GetAllJobsForAdjudication(string jobStatusCodesIncluded)
        {
            List<TrackTask> jobs = new List<TrackTask>();

            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetAllJobsForAdjudication");
            _databaseObj.AddInParameter(_databaseCommand, "@JobStatusCodesIncluded", DbType.String,
                jobStatusCodesIncluded);

            // Retrieve the results of the Stored Procedure 
            DataSet jobsDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);
            DataRow[] myJobs = jobsDataSet.Tables[0].Select();

            jobs.AddRange(myJobs.Select(ssiJobTrack => new TrackTask
            {
                RequestName = GetStringValue(ssiJobTrack["RequestName"]),
                TaskId = GetStringValue(ssiJobTrack["JobId"]),
                Status = Enum.GetName(typeof(Enums.JobStatus), ssiJobTrack["Status"]),
                FacilityId = GetValue<int>(ssiJobTrack["FacilityID"], typeof(int))
            }));

            return jobs;
        }


        /// <summary>
        /// Updates task status in DB
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public int UpdateJobStatus(TrackTask job)
        {
            if (job != null)
            {


                // Initialize the Stored Procedure
                _databaseCommand = _databaseObj.GetStoredProcCommand("UpdateJobStatus");
                _databaseObj.AddInParameter(_databaseCommand, "@TaskId", DbType.Int64, Convert.ToInt64(job.TaskId));
                _databaseObj.AddInParameter(_databaseCommand, "@RunningStatus", DbType.Int16,
                    Convert.ToInt16(job.Status));
                _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, job.UserName);
                //// Retrieve the results of the Stored Procedure 
                _databaseObj.ExecuteScalar(_databaseCommand);
                return 1;

            }
            return 0;
        }

        /// <summary>
        /// Jobs the count alert.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public int JobCountAlert(TrackTask data)
        {
            if (data != null)
            {
                _databaseCommand = _databaseObj.GetStoredProcCommand("GetUnVerifiedJobsCount");
                _databaseObj.AddInParameter(_databaseCommand, "@FacilityID", DbType.Int64, data.FacilityId);
                _databaseObj.AddOutParameter(_databaseCommand, "@UnVerifiedJobsCount", DbType.Int32, Int32.MaxValue);

                _databaseObj.ExecuteNonQuery(_databaseCommand);
                return Convert.ToInt32(_databaseCommand.Parameters["@UnVerifiedJobsCount"].Value);


            }
            //returns 0 if any exception occurs
            return 0;
        }


        /// <summary>
        /// Updates the job verified status.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public bool UpdateJobVerifiedStatus(TrackTask data)
        {
            if (data != null)
            {
                _databaseCommand = _databaseObj.GetStoredProcCommand("UpdateVerifiedSatusForJobs");
                _databaseObj.AddInParameter(_databaseCommand, "@TaskID", DbType.Int64, data.TaskId);
                _databaseObj.ExecuteNonQuery(_databaseCommand);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseObj = null;
            _databaseCommand.Dispose();
        }

        /// <summary>
        /// Determines whether [is manual adjudication running].
        /// </summary>
        /// <returns></returns>
        public bool IsManualAdjudicationRunning()
        {
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetManualAdjudicationStatus");
            int activeTaskCount = Convert.ToInt32(_databaseObj.ExecuteScalar(_databaseCommand));
            //FIX-RAGINI-FEB - isManualAdjudicationRunning is expecting value 0 or 1 but in SP datatype used in INT change it to Bit
            bool isManualAdjudicationRunning = activeTaskCount > 0;

            return isManualAdjudicationRunning;
        }

        /// <summary>
        /// Cleanups the cancelled tasks.
        /// </summary>
        /// <returns></returns>
        public bool CleanupCancelledTasks()
        {
            _databaseCommand = _databaseObj.GetStoredProcCommand("CleanupCancelledTasks");
            _databaseObj.ExecuteScalar(_databaseCommand);
            return true;
        }

        /// <summary>
        /// Determines whether [is model exist] [the specified job].
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public bool IsModelExist(TrackTask job)
        {
            if (job != null)
            {
                // Initialize the Stored Procedure
                _databaseCommand = _databaseObj.GetStoredProcCommand("IsModelExist");
                _databaseObj.AddInParameter(_databaseCommand, "@ModelId", DbType.Int64, Convert.ToInt64(job.ModelId));
                // Retrieve the results of the Stored Procedure 
                return Convert.ToInt32(_databaseObj.ExecuteScalar(_databaseCommand)) > 0;
            }
            return false;
        }

        /// <summary>
        /// Res the adjudicate.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        public long ReAdjudicate(TrackTask job)
        {
            if (job != null)
            {
                // Initialize the Stored Procedure
                _databaseCommand = _databaseObj.GetStoredProcCommand("ReAdjudicateTask");
                _databaseObj.AddInParameter(_databaseCommand, "@TaskId", DbType.Int64, Convert.ToInt64(job.TaskId));
                _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, job.UserName);
                // Retrieve the results of the Stored Procedure 
                return Convert.ToInt64(_databaseObj.ExecuteScalar(_databaseCommand));
            }
            return 0;
        }


        /// <summary>
        /// Gets Open Claim Columns By UserId
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public ClaimColumnOptions GetOpenClaimColumnOptionByUserId(ClaimColumnOptions data)
        {
            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetOpenClaimColumnOptionByUserId");
            _databaseObj.AddInParameter(_databaseCommand, "@UserId", DbType.Int64, data.UserId);
            // Retrieve the results of the Stored Procedure 
            DataSet claimColumnOptionsDataset = _databaseObj.ExecuteDataSet(_databaseCommand);
            ClaimColumnOptions claimColumnOptionsModelsList = new ClaimColumnOptions();
            if (claimColumnOptionsDataset != null)
            {
                if (claimColumnOptionsDataset.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dataRows = claimColumnOptionsDataset.Tables[0].Select();
                    var availableColumnList = dataRows.Select(claimColumns => new ClaimColumnOptions
                    {
                        ClaimColumnOptionId = Convert.ToInt32(claimColumns["ClaimColumnOptionId"]),
                        ColumnName = Convert.ToString(claimColumns["ColumnName"])

                    }).ToList();
                    claimColumnOptionsModelsList.AvailableColumnList = availableColumnList;
                }
                if (claimColumnOptionsDataset.Tables[1].Rows.Count > 0)
                {
                    DataRow[] dataRows = claimColumnOptionsDataset.Tables[1].Select();
                    var selectedColumnList = dataRows.Select(claimColumns => new ClaimColumnOptions
                    {
                        ClaimColumnOptionId = Convert.ToInt32(claimColumns["ClaimColumnOptionId"]),
                        ColumnName = Convert.ToString(claimColumns["ColumnName"])

                    }).ToList();
                    claimColumnOptionsModelsList.SelectedColumnList = selectedColumnList;
                }

            }
            return claimColumnOptionsModelsList;
        }

        /// <summary>
        /// Save Claim Column Options.
        /// </summary>
        /// <param name="claimColumnsInfo"></param>
        /// <returns></returns>
        public bool SaveClaimColumnOptions(ClaimColumnOptions claimColumnsInfo)
        {
            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("AddEditOpenClaimColumnByUserId");
            _databaseObj.AddInParameter(_databaseCommand, "@SelectedColumn", DbType.String, claimColumnsInfo.ClaimColumnOptionIds);
            _databaseObj.AddInParameter(_databaseCommand, "@UserId", DbType.Int64, claimColumnsInfo.UserId);
            // Retrieve the results of the Stored Procedure 
            return Convert.ToInt32(_databaseObj.ExecuteScalar(_databaseCommand)) > 0;
        }
    }
}

