/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Fee Schedule docs uploaded
/**  User Story Id  : 4.User Story Model a contract Figure 6
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
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Helpers;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class PaymentTypeFeeScheduleRepository : BaseRepository, IPaymentTypeFeeScheduleRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeFeeScheduleRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeFeeScheduleRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the payment type fee schedules.
        /// </summary>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dtFeeScheduleTable">The dt fee schedule table.</param>
        /// <param name="dtDoc">The dt document.</param>
        /// <param name="dtDocValues">The dt document values.</param>
        /// <returns></returns>
        public static PaymentTypeFeeSchedule GetPaymentType(long contractServiceTypeId, DataTable dtFeeScheduleTable,
            DataTable dtDoc, DataTable dtDocValues)
        {
            PaymentTypeFeeSchedule paymentTypeFeeSchedule = null;
            if (dtFeeScheduleTable != null && dtFeeScheduleTable.Rows.Count > 0)
            {
                paymentTypeFeeSchedule = (from DataRow row in dtFeeScheduleTable.Rows
                    where
                        (row["contractServiceTypeId"] != DBNull.Value &&
                         Convert.ToInt64(row["contractServiceTypeId"]) == contractServiceTypeId)
                    select new PaymentTypeFeeSchedule
                    {
                        FeeSchedule = DBNull.Value == row["FeeSchedule"]
                            ? (double?) null
                            : Convert.ToDouble(
                                row["FeeSchedule"]),
                        NonFeeSchedule =
                            DBNull.Value == row["NonFeeSchedule"]
                                ? (double?) null
                                : Convert.ToDouble(row["NonFeeSchedule"]),
                        ClaimFieldDocId =
                            Convert.ToInt64(row["ClaimFieldDocID"]),
                        ContractId = DBNull.Value == row["ContractId"]
                            ? (long?) null
                            : Convert.ToInt64(
                                row["ContractId"]),
                        ServiceTypeId =
                            DBNull.Value == row["ContractServiceTypeID"]
                                ? (long?) null
                                : Convert.ToInt64(
                                    row["ContractServiceTypeID"]),
                        ClaimFieldDoc = GetClaimFieldDoc(
                            Convert.ToInt64(row["ClaimFieldDocID"]), dtDoc, dtDocValues),
                        PaymentTypeDetailId = Convert.ToInt64(row["PaymentTypeDetailID"]),
                        PaymentTypeId = (byte) Enums.PaymentTypeCodes.FeeSchedule,
                        IsObserveUnits = Convert.ToBoolean(row["IsObserveUnits"])
                    }).FirstOrDefault();
            }

            return paymentTypeFeeSchedule;
        }

        /// <summary>
        /// Add Edit the payment type fee schedule details.
        /// </summary>
        /// <param name="paymentTypeFeeSchedules">The payment type fee schedules.</param>
        /// <returns>paymentTypeASCFeeScheduleId</returns>
        public long AddEditPaymentTypeFeeSchedule(PaymentTypeFeeSchedule paymentTypeFeeSchedules)
        {
            //Checks if input request is not null
            if (paymentTypeFeeSchedules != null)
            {
                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("AddEditFeeSchedulePayment");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@PaymentTypeDetailID", DbType.Int64, paymentTypeFeeSchedules.PaymentTypeDetailId);
                _db.AddInParameter(_cmd, "@FeeSchedule ", DbType.Decimal, paymentTypeFeeSchedules.FeeSchedule);
                _db.AddInParameter(_cmd, "@NonFeeSchedule ", DbType.Decimal, paymentTypeFeeSchedules.NonFeeSchedule);
                _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypeFeeSchedules.PaymentTypeId);
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypeFeeSchedules.ContractId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypeFeeSchedules.ServiceTypeId);
                _db.AddInParameter(_cmd, "@ClaimFieldDocID", DbType.Int64, paymentTypeFeeSchedules.ClaimFieldDocId);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypeFeeSchedules.UserName);
                _db.AddInParameter(_cmd, "@IsObserveUnits", DbType.Boolean, paymentTypeFeeSchedules.IsObserveUnits);
                // Retrieve the results of the Stored Procedure in Datatable
                return long.Parse(_db.ExecuteScalar(_cmd).ToString());
               
             
            }
           
            return 0;
        }

        /// <summary>
        /// Get the payment type fee schedule details.
        /// </summary>
        /// <param name="paymentTypeFeeSchedules">The payment type fee schedules.</param>
        /// <returns>paymentTypeASCFeeScheduleId</returns>
        public PaymentTypeFeeSchedule GetPaymentTypeFeeSchedule(PaymentTypeFeeSchedule paymentTypeFeeSchedules)
        {

            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypeFeeSchedules.PaymentTypeId);
            _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypeFeeSchedules.ContractId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypeFeeSchedules.ServiceTypeId);
            _db.AddInParameter(_cmd, "@ServiceLineTypeId", DbType.Int64, 0);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypeFeeSchedules.UserName);

            // Retrieve the results of the Stored Procedure in Datatable
            DataSet paymentTypeFeeDataSet = _db.ExecuteDataSet(_cmd);
            if (paymentTypeFeeDataSet != null && paymentTypeFeeDataSet.Tables.Count > 0)
            {
                //populating ContractBasicInfo data
                if (paymentTypeFeeDataSet.Tables[0].Rows != null && paymentTypeFeeDataSet.Tables[0] != null && paymentTypeFeeDataSet.Tables[0].Rows.Count > 0)
                {
                    PaymentTypeFeeSchedule paymentTypeSchedules = new PaymentTypeFeeSchedule
                    {
                        FeeSchedule = DBNull.Value == paymentTypeFeeDataSet.Tables[0].Rows[0]["FeeSchedule"] ? (double?)null : Convert.ToDouble(paymentTypeFeeDataSet.Tables[0].Rows[0]["FeeSchedule"]),
                        NonFeeSchedule = DBNull.Value == paymentTypeFeeDataSet.Tables[0].Rows[0]["NonFeeSchedule"] ? (double?)null : Convert.ToDouble(paymentTypeFeeDataSet.Tables[0].Rows[0]["NonFeeSchedule"]),
                        ClaimFieldDocId = DBNull.Value == paymentTypeFeeDataSet.Tables[0].Rows[0]["ClaimFieldDocID"] ? (long?)null : Convert.ToInt64(paymentTypeFeeDataSet.Tables[0].Rows[0]["ClaimFieldDocID"]),
                        PaymentTypeDetailId = Convert.ToInt64(paymentTypeFeeDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"]),
                        IsObserveUnits = Convert.ToBoolean(paymentTypeFeeDataSet.Tables[0].Rows[0]["IsObserveUnits"])
                    };
                    return paymentTypeSchedules;
                }
            }

            //returns response to Business layer
            return null;
        }

        /// <summary>
        /// 
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
