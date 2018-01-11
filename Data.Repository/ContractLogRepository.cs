/************************************************************************************************************/
/**  Author         : Manjunath
/**  Created        : 19-Sep-2013
/**  Summary        : Handles insertion of contractLogs infromation
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/
using System;
using System.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SSI.ContractManagement.Data.Repository
{
    /// <summary>
    /// Repository for the COntract Log
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ContractLogRepository : IContractLogRepository
    {
        /// <summary>
        /// The _data sourse connection string
        /// </summary>
        private readonly string _dataSourseConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractLogRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractLogRepository(string connectionString)
        {
            _dataSourseConnectionString = connectionString;
        }

        /// <summary>
        /// Adds the contract log.
        /// </summary>
        /// <param name="contractLogsList">The contract logs list.</param>
        /// <returns></returns>
        public bool AddContractLog(List<ContractLog> contractLogsList)
        {
            bool isInserted = false;

            if (contractLogsList != null && contractLogsList.Count > 0)
            {
                DataTable contractLogsDataTable = new DataTable();
                contractLogsDataTable.Columns.Add("InsertDate", typeof(DateTime));
                contractLogsDataTable.Columns.Add("UpdateDate", typeof(DateTime));
                contractLogsDataTable.Columns.Add("ContractID", typeof(long));
                contractLogsDataTable.Columns.Add("ClaimID", typeof(long));
                contractLogsDataTable.Columns.Add("LogContent", typeof(string));
                foreach (ContractLog contractLogs in contractLogsList)
                {
                    contractLogsDataTable.Rows.Add(DateTime.UtcNow, null,contractLogs.ContractId,
                        contractLogs.ClaimId, contractLogs.LogContent.ToTrim());
                }
                using (SqlBulkCopy sqlBulkInsert = new SqlBulkCopy(_dataSourseConnectionString, SqlBulkCopyOptions.KeepNulls & SqlBulkCopyOptions.KeepIdentity))
                {
                    sqlBulkInsert.DestinationTableName = "ContractLogs";
                    sqlBulkInsert.BatchSize = contractLogsList.Count;
                    sqlBulkInsert.ColumnMappings.Clear();
                    sqlBulkInsert.ColumnMappings.Add("InsertDate", "InsertDate");
                    sqlBulkInsert.ColumnMappings.Add("UpdateDate", "UpdateDate");
                    sqlBulkInsert.ColumnMappings.Add("ContractID", "ContractID");
                    sqlBulkInsert.ColumnMappings.Add("ClaimID", "ClaimID");
                    sqlBulkInsert.ColumnMappings.Add("LogContent", "LogContent");
                    sqlBulkInsert.WriteToServer(contractLogsDataTable);
                }
                isInserted = true;
            }

            return isInserted;
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
        }
    }
}
