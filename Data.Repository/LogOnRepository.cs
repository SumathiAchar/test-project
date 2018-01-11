using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class LogOnRepository : ILogOnRepository
    {
        private Database _databaseObj;
        private DbCommand _databaseCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogOnRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public LogOnRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Inserts the audit log.
        /// </summary>
        /// <param name="logOnInfo">The log in information.</param>
        /// <returns></returns>
        public void InsertAuditLog(LogOn logOnInfo)
        {
            //FIXED-2016-R2-S3 : change parameter type of FacilityID from VARCHAR(MAX) to INT in store procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("LogOnAuditLog");
            _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, logOnInfo.UserName);
            _databaseObj.AddInParameter(_databaseCommand, "@UserFacilities", DbType.String, logOnInfo.FacilityIds);
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
