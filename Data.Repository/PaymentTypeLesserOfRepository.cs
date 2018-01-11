/************************************************************************************************************/
/**  Author         : Raj
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Lessor of payment type for any contract
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class PaymentTypeLesserOfRepository : IPaymentTypeLesserOfRepository
    {
        // Variables
        private Database _databaseObj;
        private DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeLesserOfRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeLesserOfRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }


        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="lesserOfPercentage">The lesser of percentage.</param>
        /// <returns></returns>
        public static PaymentTypeLesserOf GetPaymentType(long? contractId, long? contractServiceTypeId, DataTable lesserOfPercentage)
        {
            PaymentTypeLesserOf paymentTypeLesserOf = null;
            if (lesserOfPercentage != null && lesserOfPercentage.Rows.Count > 0)
            {
                paymentTypeLesserOf = (from DataRow row in lesserOfPercentage.Rows
                                       where
                                           ((row["ContractId"] != DBNull.Value &&
                                             Convert.ToInt64(row["ContractId"]) == contractId)
                                            ||
                                            (row["contractServiceTypeId"] != DBNull.Value &&
                                             Convert.ToInt64(row["contractServiceTypeId"]) == contractServiceTypeId))
                                       select new PaymentTypeLesserOf
                                       {
                                           ContractId = DBNull.Value == row["ContractId"]
                                               ? (long?)null
                                               : Convert.ToInt64(
                                                   row["ContractId"]),
                                           Percentage = Convert.ToDouble(
                                               row["Percentage"]),
                                           PaymentTypeDetailId = Convert.ToInt64(row["PaymentTypeDetailID"]),
                                           IsLesserOf = DBNull.Value == row["IsLesserOf"]
                                               ? (bool?)null
                                               : Convert.ToBoolean(
                                                   row["IsLesserOf"]),
                                           PaymentTypeId = (byte)Enums.PaymentTypeCodes.LesserOf,
                                           ServiceTypeId = DBNull.Value == row["contractServiceTypeId"]
                                               ? (long?)null
                                               : Convert.ToInt64(
                                                   row["contractServiceTypeId"])
                                       }).FirstOrDefault();
            }
            return paymentTypeLesserOf;
        }

        /// <summary>
        /// Add/edit payment type lesser of.
        /// </summary>
        /// <param name="paymentTypeLesserOf">The payment type lesser of.</param>
        /// <returns> true/false </returns>
        public long AddEditPaymentTypeLesserOf(PaymentTypeLesserOf paymentTypeLesserOf)
        {
            if (paymentTypeLesserOf != null)
            {
                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("AddEditLesserOfPayment");
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractID", DbType.Int64, paymentTypeLesserOf.ContractId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceTypeID", DbType.Int64, paymentTypeLesserOf.ServiceTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Percentage", DbType.Double, paymentTypeLesserOf.Percentage);
                _databaseObj.AddInParameter(_databaseCommandObj, "@PaymentTypeDetailID", DbType.Int64, paymentTypeLesserOf.PaymentTypeDetailId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@PaymentTypeID ", DbType.Int64, paymentTypeLesserOf.PaymentTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@IsLesserOf", DbType.Int64, paymentTypeLesserOf.IsLesserOf);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, paymentTypeLesserOf.UserName);

                // Retrieve the results of the Stored Procedure in Datatable
                return long.Parse(_databaseObj.ExecuteScalar(_databaseCommandObj).ToString());

                
            }
           
            return 0;
        }

        /// <summary>
        /// Gets the Lesser of percentage value based on contract id.
        /// </summary>
        /// <param name="paymentTypeLesserOf"></param>
        /// <returns>Lesser of percentage value</returns>
        public PaymentTypeLesserOf GetLesserOfPercentage(PaymentTypeLesserOf paymentTypeLesserOf)
        {
            if (paymentTypeLesserOf != null)
            {
                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@PaymentTypeID ", DbType.Int64, paymentTypeLesserOf.PaymentTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractID", DbType.Int64, paymentTypeLesserOf.ContractId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceTypeID", DbType.Int64, paymentTypeLesserOf.ServiceTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ServiceLineTypeId", DbType.Int64, 0);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, paymentTypeLesserOf.UserName);

                // Retrieve the results of the Stored Procedure in Data table
                DataSet lesserOf = _databaseObj.ExecuteDataSet(_databaseCommandObj);
                var lesserOfDetails = lesserOf.Tables[0];
                if (lesserOf.Tables.Count > 0)
                {
                    //populating ContractBasicInfo data
                    if (lesserOfDetails.Rows != null && lesserOfDetails.Rows.Count > 0)
                    {
                        PaymentTypeLesserOf paymentLesserOf = new PaymentTypeLesserOf
                        {
                            Percentage = Convert.ToDouble(lesserOfDetails.Rows[0]["Percentage"]),
                            PaymentTypeDetailId = Convert.ToInt64(lesserOfDetails.Rows[0]["PaymentTypeDetailID"]),
                            IsLesserOf = DBNull.Value == lesserOfDetails.Rows[0]["IsLesserOf"]
                                ? (bool?)null
                                : Convert.ToBoolean(
                                    lesserOf.Tables[0].Rows[0]["IsLesserOf"])
                        };
                        return paymentLesserOf;
                    }
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
            _databaseCommandObj.Dispose();
            _databaseObj = null;
        }
    }
}
