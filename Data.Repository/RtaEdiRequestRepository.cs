using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class RtaEdiRequestRepository : IRtaEdiRequestRepository
    {
        private Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="RtaEdiRequestRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public RtaEdiRequestRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Saves the specified rta edi request.
        /// </summary>
        /// <param name="rtaEdiRequest">The rta edi request.</param>
        /// <returns></returns>
        public long Save(RtaEdiRequest rtaEdiRequest)
        {
            //Checks if input request is not null
            if (rtaEdiRequest != null)
            {

                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("RTA_AddRequest");
                _databaseCommandObj.CommandTimeout = 5400;
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@RequestInput", DbType.String, rtaEdiRequest.RequestInput);
                _databaseObj.AddInParameter(_databaseCommandObj, "@StatementFrom", DbType.DateTime, rtaEdiRequest.StatementFrom);
                _databaseObj.AddInParameter(_databaseCommandObj, "@StatementTo", DbType.DateTime, rtaEdiRequest.StatementTo);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SsiNumber", DbType.Int32, rtaEdiRequest.SsiNumber);
                _databaseObj.AddInParameter(_databaseCommandObj, "@PayerName", DbType.String, rtaEdiRequest.PayerName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@PayerCode", DbType.String, rtaEdiRequest.PayerCode);
                _databaseObj.AddInParameter(_databaseCommandObj, "@TotalCharges", DbType.Double, rtaEdiRequest.TotalCharges);

                // Retrieve the results of the Stored Procedure 
                long ediRequestId;
                long.TryParse(_databaseObj.ExecuteScalar(_databaseCommandObj).ToString(), out ediRequestId);
                return ediRequestId;

            }
            //returns response either rtaEdiRequest is null or any exception occurred 
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


