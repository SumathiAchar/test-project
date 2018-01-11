/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 23-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType CapDetails DataAccess Layer

/************************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class PaymentTypeCapRepository : IPaymentTypeCapRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeCapRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeCapRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the payment type cap.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dtCapTable">The dt cap table.</param>
        /// <returns></returns>
        public static PaymentTypeCap GetPaymentType(long? contractId, long? contractServiceTypeId, DataTable dtCapTable)
        {
            PaymentTypeCap paymentTypeCap = null;

            if (dtCapTable != null && dtCapTable.Rows.Count > 0)
            {
                paymentTypeCap = (from DataRow row in dtCapTable.Rows
                                  where
                                      ((row["ContractId"] != DBNull.Value &&
                                        Convert.ToInt64(row["ContractId"]) == contractId)
                                       ||
                                       (row["contractServiceTypeId"] != DBNull.Value &&
                                        Convert.ToInt64(row["contractServiceTypeId"]) == contractServiceTypeId))
                                  select new PaymentTypeCap
                                  {
                                      ContractId = DBNull.Value == row["ContractId"]
                                          ? (long?)null
                                          : Convert.ToInt64(
                                              row["ContractId"]),
                                      ServiceTypeId =
                                          DBNull.Value == row["ContractServiceTypeID"]
                                              ? (long?)null
                                              : Convert.ToInt64(
                                                  row["ContractServiceTypeID"]),
                                      Threshold = DBNull.Value == row["Threshold"]
                                          ? (double?)null
                                          : Convert.ToDouble(
                                              row["Threshold"]),
                                      PaymentTypeDetailId = Convert.ToInt64(row["PaymentTypeDetailID"]),
                                      PaymentTypeId = (byte)Enums.PaymentTypeCodes.Cap
                                  }).FirstOrDefault();
            }
            return paymentTypeCap;
        }

        /// <summary>
        /// AddEdit PaymentType Cap Details
        /// </summary>
        /// <param name="paymentTypeCap"></param>
        /// <returns></returns>
        public long AddEditPaymentTypeCapDetails(PaymentTypeCap paymentTypeCap)
        {
            //Checks if input request is not null
            if (paymentTypeCap != null)
            {
                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("AddEditCAPPayment");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@PaymentTypeDetailID", DbType.Int64, paymentTypeCap.PaymentTypeDetailId);
                _db.AddInParameter(_cmd, "@Threshold ", DbType.Decimal, paymentTypeCap.Threshold);
                _db.AddInParameter(_cmd, "@Percentage ", DbType.Int64, paymentTypeCap.Percentage);
                _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypeCap.PaymentTypeId);
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypeCap.ContractId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypeCap.ServiceTypeId);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypeCap.UserName);

                // Retrieve the results of the Stored Procedure in Datatable
                return long.Parse(_db.ExecuteScalar(_cmd).ToString());

               
            }
           
            return 0;
        }

        /// <summary>
        /// Get PaymentType Cap Details
        /// </summary>
        /// <param name="paymentTypeCap"></param>
        /// <returns></returns>
        public PaymentTypeCap GetPaymentTypeCapDetails(PaymentTypeCap paymentTypeCap)
        {

            if (paymentTypeCap != null)
            {
                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypeCap.PaymentTypeId);
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypeCap.ContractId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypeCap.ServiceTypeId);
                _db.AddInParameter(_cmd, "@ServiceLineTypeId", DbType.Int64, 0);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypeCap.UserName);

                // Retrieve the results of the Stored Procedure in Datatable
                DataSet paymentTypeCapDataSet = _db.ExecuteDataSet(_cmd);
                if (paymentTypeCapDataSet != null && paymentTypeCapDataSet.Tables.Count > 0)
                {
                    //populating paymentTypeCap data
                    if (paymentTypeCapDataSet.Tables[0].Rows != null && paymentTypeCapDataSet.Tables[0] != null && paymentTypeCapDataSet.Tables[0].Rows.Count > 0)
                    {
                        PaymentTypeCap paymentCap = new PaymentTypeCap
                        {
                            Threshold =
                                DBNull.Value == paymentTypeCapDataSet.Tables[0].Rows[0]["Threshold"]
                                    ? (double?)null
                                    : Convert.ToDouble(paymentTypeCapDataSet.Tables[0].Rows[0]["Threshold"]),
                            Percentage =
                                DBNull.Value == paymentTypeCapDataSet.Tables[0].Rows[0]["Percentage"]
                                    ? (double?)null
                                    : Convert.ToDouble(paymentTypeCapDataSet.Tables[0].Rows[0]["Percentage"]),
                            PaymentTypeDetailId = Convert.ToInt64(paymentTypeCapDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"])
                        };
                        return paymentCap;
                    }
                }
            }
            return null;
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
