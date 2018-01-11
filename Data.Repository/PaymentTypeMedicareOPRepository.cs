/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Medicare OP Details DataAccess Layer

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
    // ReSharper disable once InconsistentNaming
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class PaymentTypeMedicareOPRepository : IPaymentTypeMedicareOpRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareOPRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeMedicareOPRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dtPaymentTypeMedicareOp">The dt payment type medicare op.</param>
        /// <returns></returns>
        public static PaymentTypeMedicareOp GetPaymentType(long contractServiceTypeId, DataTable dtPaymentTypeMedicareOp)
        {
            PaymentTypeMedicareOp paymentTypeMedicareOp = null;
            if (dtPaymentTypeMedicareOp != null && dtPaymentTypeMedicareOp.Rows.Count > 0)
            {
                paymentTypeMedicareOp = (from DataRow row in dtPaymentTypeMedicareOp.Rows
                                         where
                                             (row["contractServiceTypeId"] != DBNull.Value &&
                                                   Convert.ToInt64(row["contractServiceTypeId"]) == contractServiceTypeId)
                                         select new PaymentTypeMedicareOp
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
                                             OutPatient = DBNull.Value == row["OutPatient"]
                                                 ? (double?)null
                                                 : Convert.ToDouble(
                                                     row["OutPatient"]),
                                             PaymentTypeDetailId = Convert.ToInt64(row["PaymentTypeDetailID"]),
                                             PaymentTypeId = (byte)Enums.PaymentTypeCodes.MedicareOp
                                         }).FirstOrDefault();
            }
            return paymentTypeMedicareOp;
        }


        /// <summary>
        /// AddEdit PaymentType Medicare OP Payment
        /// </summary>
        /// <param name="paymentTypeMedicareOpPayment"></param>
        /// <returns>paymentTypeMedicareOPPaymentId</returns>
        public long AddEditPaymentTypeMedicareOpPayment(PaymentTypeMedicareOp paymentTypeMedicareOpPayment)
        {
            //Checks if input request is not null
            if (paymentTypeMedicareOpPayment != null)
            {
                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("AddEditMedicareOPPayment");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@PaymentTypeDetailID", DbType.Int64, paymentTypeMedicareOpPayment.PaymentTypeDetailId);
                _db.AddInParameter(_cmd, "@OutPatient", DbType.Decimal, paymentTypeMedicareOpPayment.OutPatient);
                _db.AddInParameter(_cmd, "@PaymentTypeID", DbType.Int64, paymentTypeMedicareOpPayment.PaymentTypeId);
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypeMedicareOpPayment.ContractId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypeMedicareOpPayment.ServiceTypeId);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypeMedicareOpPayment.UserName);

                // Retrieve the results of the Stored Procedure in Datatable
                return long.Parse(_db.ExecuteScalar(_cmd).ToString());

                
            }
            return 0;
        }

        /// <summary>
        /// Get PaymentType Medicare OP Payment
        /// </summary>
        /// <param name="paymentTypeMedicareOpPayment"></param>
        /// <returns>paymentTypeMedicareOPPaymentId</returns>
        public PaymentTypeMedicareOp GetPaymentTypeMedicareOpDetails(PaymentTypeMedicareOp paymentTypeMedicareOpPayment)
        {

            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypeMedicareOpPayment.PaymentTypeId);
            _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypeMedicareOpPayment.ContractId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypeMedicareOpPayment.ServiceTypeId);
            _db.AddInParameter(_cmd, "@ServiceLineTypeId", DbType.Int64, 0);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypeMedicareOpPayment.UserName);

            // Retrieve the results of the Stored Procedure in Datatable
            DataSet paymentTypeMedicareOpDataSet = _db.ExecuteDataSet(_cmd);
            if (paymentTypeMedicareOpDataSet != null && paymentTypeMedicareOpDataSet.Tables.Count > 0)
            {
                //populating ContractBasicInfo data
                if (paymentTypeMedicareOpDataSet.Tables[0].Rows != null && paymentTypeMedicareOpDataSet.Tables[0] != null && paymentTypeMedicareOpDataSet.Tables[0].Rows.Count > 0)
                {
                    PaymentTypeMedicareOp paymentTypeOpPayment = new PaymentTypeMedicareOp
                    {
                        OutPatient = DBNull.Value == paymentTypeMedicareOpDataSet.Tables[0].Rows[0]["OutPatient"] ? (double?)null : Convert.ToDouble(paymentTypeMedicareOpDataSet.Tables[0].Rows[0]["OutPatient"]),
                        PaymentTypeDetailId = Convert.ToInt64(paymentTypeMedicareOpDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"])
                    };
                    return paymentTypeOpPayment;
                }
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
            _cmd.Dispose();
            _db = null;
        }
    }
}
