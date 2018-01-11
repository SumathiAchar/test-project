using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class RtaEdiResponseRepository : IRtaEdiResponseRepository
    {
        private Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="RtaEdiResponseRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public RtaEdiResponseRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Saves the specified rta edi response.
        /// </summary>
        /// <param name="rtaEdiResponse">The rta edi response.</param>
        /// <returns></returns>
        public long Save(RtaEdiResponse rtaEdiResponse)
        {
            //Checks if input response is not null
            if (rtaEdiResponse != null)
            {
                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("RTA_AddResponse");
                _databaseCommandObj.CommandTimeout = 5400;

                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@EdiRequestID", DbType.Int64,
                    rtaEdiResponse.EdiRequestId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@EdiResponseOutput", DbType.String,
                    rtaEdiResponse.ResponseOutput);
                _databaseObj.AddInParameter(_databaseCommandObj, "@RTACodeID", DbType.Int32,
                    rtaEdiResponse.RtaCodeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ResponseType", DbType.String,
                                       rtaEdiResponse.ResponseType);
                _databaseObj.AddInParameter(_databaseCommandObj, "@CalcAllowedAmt", DbType.Double, rtaEdiResponse.CalcAllowedAmt);

                _databaseObj.AddInParameter(_databaseCommandObj, "@PaymentResult", DbType.String, rtaEdiResponse.PaymentResult);

                // Retrieve the results of the Stored Procedure
                long ediResponseId;
                long.TryParse(_databaseObj.ExecuteScalar(_databaseCommandObj).ToString(), out ediResponseId);
                return ediResponseId;

            }
            //returns response either rtaEdiResponse is null 
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
