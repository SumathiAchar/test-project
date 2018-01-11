using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;

namespace SSI.ContractManagement.Data.Repository
{
    // ReSharper disable once InconsistentNaming
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class PaymentTypeMedicareIpRepository : IPaymentTypeMedicareIpRepository
    {
        // Variables
        private Database _database;
        DbCommand _command;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareIpRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeMedicareIpRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _database = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dtPaymentTypeMedicareIp">The dt payment type medicare ip.</param>
        /// <returns></returns>
        public static PaymentTypeMedicareIp GetPaymentType(long contractServiceTypeId, DataTable dtPaymentTypeMedicareIp)
        {
            PaymentTypeMedicareIp paymentTypeMedicareIp = null;
            if (dtPaymentTypeMedicareIp != null && dtPaymentTypeMedicareIp.Rows.Count > 0)
            {
                paymentTypeMedicareIp = (from DataRow medicareIpRow in dtPaymentTypeMedicareIp.Rows
                                         where
                                             (medicareIpRow["contractServiceTypeId"] != DBNull.Value &&
                                                   Convert.ToInt64(medicareIpRow["contractServiceTypeId"]) == contractServiceTypeId)
                                         select new PaymentTypeMedicareIp
                                  {
                                      ContractId = DBNull.Value == medicareIpRow["ContractId"]
                                          ? (long?)null
                                          : Convert.ToInt64(
                                              medicareIpRow["ContractId"]),
                                      ServiceTypeId =
                                    DBNull.Value == medicareIpRow["ContractServiceTypeID"]
                                        ? (long?)null
                                        : Convert.ToInt64(
                                            medicareIpRow["ContractServiceTypeID"]),
                                      InPatient = DBNull.Value == medicareIpRow["InPatient"]
                                          ? (double?)null
                                          : Convert.ToDouble(
                                              medicareIpRow["InPatient"]),
                                      PaymentTypeDetailId = Convert.ToInt64(medicareIpRow["PaymentTypeDetailID"]),
                                      PaymentTypeId = (byte)Enums.PaymentTypeCodes.MedicareIp,
                                      Formula = DBNull.Value == medicareIpRow["Formula"]
                                          ? string.Empty
                                          : Convert.ToString(medicareIpRow["Formula"])
                                  }).FirstOrDefault();
            }
            return paymentTypeMedicareIp;
        }

        /// <summary>
        /// AddEdit PaymentType Medicare IP Payment
        /// </summary>
        /// <param name="paymentTypeMedicareIpPayment"></param>
        /// <returns>paymentTypeMedicareIPPaymentID</returns>
        public long AddEditPaymentTypeMedicareIpPayment(PaymentTypeMedicareIp paymentTypeMedicareIpPayment)
        {
            long paymentTypeMedicareIpPaymentId = 0;
            //Checks if input request is not null
            if (paymentTypeMedicareIpPayment != null)
            {

                // Initialize the Stored Procedure
                _command = _database.GetStoredProcCommand("AddEditMedicareIPPayment");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _database.AddInParameter(_command, "@PaymentTypeDetailID", DbType.Int64, paymentTypeMedicareIpPayment.PaymentTypeDetailId);
                _database.AddInParameter(_command, "@InPatient ", DbType.Decimal, paymentTypeMedicareIpPayment.InPatient);
                _database.AddInParameter(_command, "@Formula", DbType.String, paymentTypeMedicareIpPayment.Formula);
                _database.AddInParameter(_command, "@PaymentTypeID ", DbType.Int64, paymentTypeMedicareIpPayment.PaymentTypeId);
                _database.AddInParameter(_command, "@ContractID", DbType.Int64, paymentTypeMedicareIpPayment.ContractId);
                _database.AddInParameter(_command, "@ContractServiceTypeID", DbType.Int64, paymentTypeMedicareIpPayment.ServiceTypeId);
                _database.AddInParameter(_command, "@UserName", DbType.String, paymentTypeMedicareIpPayment.UserName);

                // Retrieve the results of the Stored Procedure in Datatable
                paymentTypeMedicareIpPaymentId = long.Parse(_database.ExecuteScalar(_command).ToString());

            }
            //returns response to Business layer
            return paymentTypeMedicareIpPaymentId;
        }

        /// <summary>
        /// Get PaymentType Medicare IP Payment
        /// </summary>
        /// <param name="paymentTypeMedicareIpPayment"></param>
        /// <returns>paymentTypeMedicareIPPaymentID</returns>
        public PaymentTypeMedicareIp GetPaymentTypeMedicareIpPayment(PaymentTypeMedicareIp paymentTypeMedicareIpPayment)
        {
            try
            {
                // Initialize the Stored Procedure
                _command = _database.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _database.AddInParameter(_command, "@PaymentTypeID ", DbType.Int64, paymentTypeMedicareIpPayment.PaymentTypeId);
                _database.AddInParameter(_command, "@ContractID", DbType.Int64, paymentTypeMedicareIpPayment.ContractId);
                _database.AddInParameter(_command, "@ContractServiceTypeID", DbType.Int64, paymentTypeMedicareIpPayment.ServiceTypeId);
                _database.AddInParameter(_command, "@ServiceLineTypeId", DbType.Int64, 0);
                _database.AddInParameter(_command, "@UserName", DbType.String, paymentTypeMedicareIpPayment.UserName);

                // Retrieve the results of the Stored Procedure in Data set
                DataSet paymentTypeMedicareIpDataSet = _database.ExecuteDataSet(_command);
                if (paymentTypeMedicareIpDataSet.IsTableDataPopulated(0))
                {
                    //populating MedicareIp data
                    if (paymentTypeMedicareIpDataSet.Tables[0].Rows != null && paymentTypeMedicareIpDataSet.Tables[0] != null && paymentTypeMedicareIpDataSet.Tables[0].Rows.Count > 0)
                    {
                        PaymentTypeMedicareIp paymentTypeIpPayment = new PaymentTypeMedicareIp
                        {
                            InPatient =
                                DBNull.Value == paymentTypeMedicareIpDataSet.Tables[0].Rows[0]["InPatient"]
                                    ? (double?)null
                                    : Convert.ToDouble(paymentTypeMedicareIpDataSet.Tables[0].Rows[0]["InPatient"]),
                            PaymentTypeDetailId =
                                Convert.ToInt64(paymentTypeMedicareIpDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"]),
                            Formula =
                                DBNull.Value == paymentTypeMedicareIpDataSet.Tables[0].Rows[0]["Formula"]
                                    ? string.Empty
                                    : Convert.ToString(paymentTypeMedicareIpDataSet.Tables[0].Rows[0]["Formula"])
                        };
                        return paymentTypeIpPayment;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError(string.Empty, string.Empty, ex);
            }
            //returns response to Business layer
            return null;
        }

        /// <summary>
        /// Disposes the objects
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _command.Dispose();
            _database = null;
        }
    }
}
