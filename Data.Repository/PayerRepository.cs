using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class PayerRepository : IPayerRepository
    {
        private Database _db;
        private DbCommand _cmd;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PayerRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PayerRepository(string connectionString)
        {
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the payers.
        /// </summary>
        /// <param name="selectedFacility">The selected facility.</param>
        /// <returns></returns>
        public List<Payer> GetPayers(ContractServiceLineClaimFieldSelection selectedFacility)
        {
            List<Payer> availablePayers = new List<Payer>();

            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetPayerNames");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@NodeID", DbType.Int64, selectedFacility.FacilityId);
            _db.AddInParameter(_cmd, "@Contractid", DbType.Int64, selectedFacility.ContractId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, selectedFacility.ContractServiceTypeId);
            _db.AddInParameter(_cmd, "@UserText", DbType.String, selectedFacility.Values);
            string ssiNumbers = "";
            if (selectedFacility.SsiNumber != null && selectedFacility.SsiNumber.Count > 0)
            {
                ssiNumbers = string.Join(",",
                    selectedFacility.SsiNumber.Select(a => a.ToString(CultureInfo.InvariantCulture))
                        .ToArray());
            }

            _db.AddInParameter(_cmd, "@SSINumbers", DbType.String, ssiNumbers);

            DataSet payerDataSet = _db.ExecuteDataSet(_cmd);
            if (payerDataSet != null && payerDataSet.Tables.Count > 0)
            {
                for (int i = 0; i < payerDataSet.Tables[0].Rows.Count; i++)
                {
                    availablePayers.Add(new Payer
                    {
                        PayerName = Convert.ToString(payerDataSet.Tables[0].Rows[i]["PayerName"])
                    });
                }
            }
            return availablePayers;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _cmd.Dispose();
            _db = null;
        }
    }
}
