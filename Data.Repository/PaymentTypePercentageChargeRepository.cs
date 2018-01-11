/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Percentage DiscountDetails DataAccess Layer

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
    public class PaymentTypePercentageChargeRepository : IPaymentTypePercentageChargeRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePercentageChargeRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypePercentageChargeRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }


        /// <summary>
        /// Gets the payment type percentage.
        /// </summary>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dtPercentageTable">The percentage table.</param>
        /// <returns></returns>
        public static PaymentTypePercentageCharge GetPaymentType(long contractServiceTypeId, DataTable dtPercentageTable)
        {
            PaymentTypePercentageCharge paymentTypePercentageCharge = null;
            if (dtPercentageTable != null && dtPercentageTable.Rows.Count > 0)
            {
                paymentTypePercentageCharge = (from DataRow row in dtPercentageTable.Rows
                                               where
                                                   (row["contractServiceTypeId"] != DBNull.Value &&
                                                    Convert.ToInt64(row["contractServiceTypeId"]) == contractServiceTypeId)
                                               select new PaymentTypePercentageCharge
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
                                                   Percentage = Convert.ToDouble(
                                                          row["Percentage"]),
                                                   PaymentTypeDetailId = Convert.ToInt64(row["PaymentTypeDetailID"]),
                                                   PaymentTypeId = (byte)Enums.PaymentTypeCodes.PercentageDiscountPayment
                                               }).FirstOrDefault();

            }
            return paymentTypePercentageCharge;
        }

        /// <summary>
        /// Add & Edit Payment Type Percentage Discount Details
        /// </summary>
        /// <param name="paymentTypePercentageDiscount"></param>
        /// <returns></returns>
        public long AddEditPaymentTypePercentageDiscountDetails(PaymentTypePercentageCharge paymentTypePercentageDiscount)
        {
            //Checks if input request is not null
            if (paymentTypePercentageDiscount != null)
            {
                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("AddEditPercentageDiscountPayment");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@PaymentTypeDetailID", DbType.Int64, paymentTypePercentageDiscount.PaymentTypeDetailId);
                _db.AddInParameter(_cmd, "@Percentage ", DbType.Decimal, paymentTypePercentageDiscount.Percentage);
                _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypePercentageDiscount.PaymentTypeId);
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypePercentageDiscount.ContractId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypePercentageDiscount.ServiceTypeId);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypePercentageDiscount.UserName);

                // Retrieve the results of the Stored Procedure in Datatable
                return long.Parse(_db.ExecuteScalar(_cmd).ToString());


               
            }
          
            return 0;
        }

        /// <summary>
        /// Get Payment Type Percentage Discount Details
        /// </summary>
        /// <param name="paymentTypePercentageDiscount"></param>
        /// <returns></returns>
        public PaymentTypePercentageCharge GetPaymentTypePercentageDiscountDetails(PaymentTypePercentageCharge paymentTypePercentageDiscount)
        {

            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypePercentageDiscount.PaymentTypeId);
            _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypePercentageDiscount.ContractId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypePercentageDiscount.ServiceTypeId);
            _db.AddInParameter(_cmd, "@ServiceLineTypeId", DbType.Int64, 0);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypePercentageDiscount.UserName);

            // Retrieve the results of the Stored Procedure in Data set
            DataSet paymentTypePercentageDiscountDataSet = _db.ExecuteDataSet(_cmd);
            if (paymentTypePercentageDiscountDataSet != null && paymentTypePercentageDiscountDataSet.Tables.Count > 0)
            {
                //populating PercentageDiscount data
                if (paymentTypePercentageDiscountDataSet.Tables[0].Rows != null && paymentTypePercentageDiscountDataSet.Tables[0] != null && paymentTypePercentageDiscountDataSet.Tables[0].Rows.Count > 0)
                {
                    PaymentTypePercentageCharge paymentPercentageDiscount = new PaymentTypePercentageCharge
                    {
                        Percentage = Convert.ToDouble(paymentTypePercentageDiscountDataSet.Tables[0].Rows[0]["Percentage"]),
                        PaymentTypeDetailId = Convert.ToInt64(paymentTypePercentageDiscountDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"]),
                    };
                    return paymentPercentageDiscount;
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
