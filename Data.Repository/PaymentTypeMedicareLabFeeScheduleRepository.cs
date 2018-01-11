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
    public class PaymentTypeMedicareLabFeeScheduleRepository : IPaymentTypeMedicareLabFeeScheduleRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareLabFeeScheduleRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeMedicareLabFeeScheduleRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }
        /// <summary>
        /// Gets the payment type medicare lab fee schedule.
        /// </summary>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dtMedicareLabFeeSchedule">The dt medicare lab fee schedule.</param>
        /// <returns></returns>
        public static PaymentTypeMedicareLabFeeSchedule GetPaymentType(long contractServiceTypeId, DataTable dtMedicareLabFeeSchedule)
        {
            PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedule = null;
            if (dtMedicareLabFeeSchedule != null && dtMedicareLabFeeSchedule.Rows.Count > 0)
            {
                paymentTypeMedicareLabFeeSchedule = (from DataRow row in dtMedicareLabFeeSchedule.Rows
                                                     where
                                                         (row["contractServiceTypeId"] != DBNull.Value &&
                                                          Convert.ToInt64(row["contractServiceTypeId"]) == contractServiceTypeId)
                                                     select new PaymentTypeMedicareLabFeeSchedule
                                                     {
                                                         Percentage = Convert.ToDouble(
                                                             row["Percentage"]),
                                                         ContractId = DBNull.Value == row["ContractId"]
                                                                                              ? (long?)null
                                                             : Convert.ToInt64(
                                                                 row["ContractId"]),
                                                         ServiceTypeId =
                                                             DBNull.Value == row["ContractServiceTypeID"]
                                                                                                  ? (long?)null
                                                                 : Convert.ToInt64(
                                                                     row["ContractServiceTypeID"]),
                                                         PaymentTypeDetailId = Convert.ToInt64(row["PaymentTypeDetailID"]),
                                                         PaymentTypeId = (byte)Enums.PaymentTypeCodes.MedicareLabFeeSchedule
                                                     }).FirstOrDefault();


                # region "For future Use"
                //if (paymentTypeMedicareLabFeeSchedule != null)
                //{
                //    List<ICondition> conditions = new List<ICondition>();
                //    if (!string.IsNullOrEmpty(paymentTypeMedicareLabFeeSchedule.RevCode))
                //    {
                //        ICondition conditionRevCode = new Condition
                //        {
                //            ConditionOperator = (int)Enums.ConditionOperation.EqualTo,
                //            OperandIdentifier = (int)Enums.OperandIdentifier.RevCode,
                //            RightOperand = paymentTypeMedicareLabFeeSchedule.RevCode
                //        };
                //        conditionRevCode = UpdatePropertyAndOperand(conditionRevCode);
                //        conditions.Add(conditionRevCode);
                //    }
                //    if (!string.IsNullOrEmpty(paymentTypeMedicareLabFeeSchedule.HCPCSCode))
                //    {
                //        ICondition conditionHCPCSCode = new Condition
                //        {
                //            ConditionOperator = (int)Enums.ConditionOperation.EqualTo,
                //            OperandIdentifier = (int)Enums.OperandIdentifier.HCPCSCode,
                //            RightOperand = paymentTypeMedicareLabFeeSchedule.HCPCSCode
                //        };
                //        conditionHCPCSCode = UpdatePropertyAndOperand(conditionHCPCSCode);
                //        conditions.Add(conditionHCPCSCode);
                //    }
                //    paymentTypeMedicareLabFeeSchedule.Conditions = conditions;
                //}

                #endregion
            }
            return paymentTypeMedicareLabFeeSchedule;
        }


        /// <summary>
        /// AddEdit PaymentType Medicare IP Payment
        /// </summary>
        /// <param name="paymentTypeMedicareLabFeeSchedulePayment"></param>
        /// <returns>paymentTypeMedicareLabFeeSchedulePaymentID</returns>
        public long AddEditPaymentTypeMedicareLabFeeSchedulePayment(PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedulePayment)
        {
            //Checks if input request is not null
            if (paymentTypeMedicareLabFeeSchedulePayment != null)
            {


                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("AddEditMedicareLabFeeSchedulePayment");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@PaymentTypeDetailID", DbType.Int64, paymentTypeMedicareLabFeeSchedulePayment.PaymentTypeDetailId);
                _db.AddInParameter(_cmd, "@Percentage", DbType.Decimal, paymentTypeMedicareLabFeeSchedulePayment.Percentage);
                _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypeMedicareLabFeeSchedulePayment.PaymentTypeId);
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypeMedicareLabFeeSchedulePayment.ContractId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypeMedicareLabFeeSchedulePayment.ServiceTypeId);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypeMedicareLabFeeSchedulePayment.UserName);
                

                // Retrieve the results of the Stored Procedure in Datatable
                return long.Parse(_db.ExecuteScalar(_cmd).ToString());

               
            }
           
            return 0;
        }

        /// <summary>
        /// Get PaymentType Medicare IP Payment
        /// </summary>
        /// <param name="paymentTypeMedicareLabFeeSchedulePayment"></param>
        /// <returns>paymentTypeMedicareLabFeeSchedulePaymentID</returns>
        public PaymentTypeMedicareLabFeeSchedule GetPaymentTypeMedicareLabFeeSchedulePayment(PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedulePayment)
        {
            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypeMedicareLabFeeSchedulePayment.PaymentTypeId);
            _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypeMedicareLabFeeSchedulePayment.ContractId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypeMedicareLabFeeSchedulePayment.ServiceTypeId);
            _db.AddInParameter(_cmd, "@ServiceLineTypeId", DbType.Int64, 0);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypeMedicareLabFeeSchedulePayment.UserName);

            // Retrieve the results of the Stored Procedure in Datatable
            DataSet paymentTypeMedicareLabFeeDataSet = _db.ExecuteDataSet(_cmd);
            if (paymentTypeMedicareLabFeeDataSet != null && paymentTypeMedicareLabFeeDataSet.Tables.Count > 0)
            {
                //populating ContractBasicInfo data
                if (paymentTypeMedicareLabFeeDataSet.Tables[0].Rows != null && paymentTypeMedicareLabFeeDataSet.Tables[0] != null && paymentTypeMedicareLabFeeDataSet.Tables[0].Rows.Count > 0)
                {
                    PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedule = new PaymentTypeMedicareLabFeeSchedule
                    {
                        Percentage = Convert.ToDouble(paymentTypeMedicareLabFeeDataSet.Tables[0].Rows[0]["Percentage"]),
                        PaymentTypeDetailId = Convert.ToInt64(paymentTypeMedicareLabFeeDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"])
                    };
                    return paymentTypeMedicareLabFeeSchedule;
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
