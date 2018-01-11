using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class LogOffRepository : ILogOffRepository
    {
        private Database _databaseObj;
        private DbCommand _databaseCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogOffRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public LogOffRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }



        //FIXED-NON15--Rename sp to LogOffAuditLog and SaveIntoAuditLog to InsertAuditLog
        /// <summary>
        /// Inserts the audit log.
        /// </summary>
        /// <param name="logOffInfo">The log off information.</param>
        /// <returns></returns>
        public void InsertAuditLog(LogOff logOffInfo)
        {
            //FIXED-2016-R2-S3 : change parameter type of FacilityID from VARCHAR(MAX) to INT in store procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("LogOffAuditLog");
            _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, logOffInfo.UserName);
            _databaseObj.AddInParameter(_databaseCommand, "@IsSessionTimeOut", DbType.Boolean, logOffInfo.IsSessionTimeOut);
            _databaseObj.AddInParameter(_databaseCommand, "@UserFacilities", DbType.String, logOffInfo.FacilityIds);
            _databaseObj.ExecuteScalar(_databaseCommand);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseCommand.Dispose();
            _databaseObj = null;
        }
    }
}
