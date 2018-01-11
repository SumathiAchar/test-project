using System.Data;
using System.Data.Common;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ContractServiceLinePaymentTypeRepository : IContractServiceLinePaymentTypeRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractServiceLinePaymentTypeRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractServiceLinePaymentTypeRepository(string connectionString)
        {
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }


        /// <summary>
        ///  Deletes  Contract ServiceLines and PaymentTypes
        /// </summary>
        /// <param name="contractServiceLinePaymentTypes">ContractServiceLinePaymentTypes</param>
        /// <returns>true/false</returns>
        public bool DeleteContractServiceLinesAndPaymentTypes(ContractServiceLinePaymentType contractServiceLinePaymentTypes)
        {
            //holds the response data
            bool returnvalue = false;

            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("DeleteServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, contractServiceLinePaymentTypes.ContractId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, contractServiceLinePaymentTypes.ContractServiceTypeId);
            _db.AddInParameter(_cmd, "@ServiceLineTypeID", DbType.Int64, contractServiceLinePaymentTypes.ServiceLineTypeId);
            _db.AddInParameter(_cmd, "@PaymentTypeID", DbType.Int64, contractServiceLinePaymentTypes.PaymentTypeId);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, contractServiceLinePaymentTypes.UserName);
            // Retrieve the results of the Stored Procedure in Datatable
            int updatedRow = _db.ExecuteNonQuery(_cmd);
            //returns response to Business layer
            if (updatedRow > 0)
                returnvalue = true;

            //returns false if any exception occurs
            return returnvalue;
        }

        /// <summary>
        /// Disposes the objects
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _cmd.Dispose();
            _db = null;
        }
    }
}
