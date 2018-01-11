/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 23-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType PerVisit DataAccess Layer

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
    public class PaymentTypePerVisitRepository : IPaymentTypePerVisitRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePerVisitRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypePerVisitRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the payment type per visit.
        /// </summary>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dtPerVisitTable">The dt per visit table.</param>
        /// <returns></returns>
        public static PaymentTypePerVisit GetPaymentType(long contractServiceTypeId, DataTable dtPerVisitTable)
        {
            PaymentTypePerVisit paymentTypePerVisit = null;
            if (dtPerVisitTable != null && dtPerVisitTable.Rows.Count > 0)
            {
                paymentTypePerVisit = (from DataRow row in dtPerVisitTable.Rows
                                       where
                                           (row["contractServiceTypeId"] != DBNull.Value &&
                                            Convert.ToInt64(row["contractServiceTypeId"]) == contractServiceTypeId)
                                       select new PaymentTypePerVisit
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
                                           Rate = DBNull.Value == row["Rate"]
                                              ? (double?)null
                                              : Convert.ToDouble(
                                                  row["Rate"]),
                                           PaymentTypeDetailId = Convert.ToInt64(row["PaymentTypeDetailID"]),
                                           PaymentTypeId = (byte)Enums.PaymentTypeCodes.PerVisit
                                       }).FirstOrDefault();
                # region "For future Use"
                //if (paymentTypePerVisit != null)
                //{
                //    List<ICondition> conditions = new List<ICondition>();
                //    if (!string.IsNullOrEmpty(paymentTypePerVisit.RevCode))
                //    {
                //        ICondition conditionRevCode = new Condition
                //        {
                //            ConditionOperator = (int)Enums.ConditionOperation.EqualTo,
                //            OperandIdentifier = (int)Enums.OperandIdentifier.RevCode,
                //            RightOperand = paymentTypePerVisit.RevCode
                //        };
                //        conditionRevCode = UpdatePropertyAndOperand(conditionRevCode);
                //        conditions.Add(conditionRevCode);
                //    }
                //    if (!string.IsNullOrEmpty(paymentTypePerVisit.HCPCSCode))
                //    {
                //        ICondition conditionHCPCSCode = new Condition
                //        {
                //            ConditionOperator = (int)Enums.ConditionOperation.EqualTo,
                //            OperandIdentifier = (int)Enums.OperandIdentifier.HCPCSCode,
                //            RightOperand = paymentTypePerVisit.HCPCSCode
                //        };
                //        conditionHCPCSCode = UpdatePropertyAndOperand(conditionHCPCSCode);
                //        conditions.Add(conditionHCPCSCode);
                //    }
                //    paymentTypePerVisit.Conditions = conditions;
                //}
                #endregion
            }
            return paymentTypePerVisit;
        }

        /// <summary>
        /// Add Edit PaymentType PerVisit Details
        /// </summary>
        /// <param name="paymentTypePerVisit"></param>
        /// <returns></returns>
        public long AddEditPaymentTypePerVisitDetails(PaymentTypePerVisit paymentTypePerVisit)
        {
            //Checks if input request is not null
            if (paymentTypePerVisit != null)
            {
                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("AddEditPerVisitPaymentType");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@PaymentTypeDetailID", DbType.Int64, paymentTypePerVisit.PaymentTypeDetailId);
                _db.AddInParameter(_cmd, "@Rate", DbType.Decimal, paymentTypePerVisit.Rate);
                _db.AddInParameter(_cmd, "@PaymentTypeID", DbType.Int64, paymentTypePerVisit.PaymentTypeId);
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypePerVisit.ContractId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypePerVisit.ServiceTypeId);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypePerVisit.UserName);

                // Retrieve the results of the Stored Procedure in Datatable
                return long.Parse(_db.ExecuteScalar(_cmd).ToString());


                
            }
           
            return 0;
        }

        /// <summary>
        /// Get PaymentType PerVisit Details
        /// </summary>
        /// <param name="paymentTypePerVisit"></param>
        /// <returns></returns>
        public PaymentTypePerVisit GetPaymentTypePerVisitDetails(PaymentTypePerVisit paymentTypePerVisit)
        {

            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypePerVisit.PaymentTypeId);
            _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypePerVisit.ContractId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypePerVisit.ServiceTypeId);
            _db.AddInParameter(_cmd, "@ServiceLineTypeId", DbType.Int64, 0);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypePerVisit.UserName);

            // Retrieve the results of the Stored Procedure in Data set
            DataSet paymentTypePerVisitDataSet = _db.ExecuteDataSet(_cmd);
            if (paymentTypePerVisitDataSet != null && paymentTypePerVisitDataSet.Tables.Count > 0)
            {
                //populating PerVisit data
                if (paymentTypePerVisitDataSet.Tables[0].Rows != null && paymentTypePerVisitDataSet.Tables[0] != null && paymentTypePerVisitDataSet.Tables[0].Rows.Count > 0)
                {
                    PaymentTypePerVisit paymentPerVisit = new PaymentTypePerVisit
                    {
                        Rate = DBNull.Value == paymentTypePerVisitDataSet.Tables[0].Rows[0]["Rate"] ? (double?)null : Convert.ToDouble(paymentTypePerVisitDataSet.Tables[0].Rows[0]["Rate"]),
                        PaymentTypeDetailId = Convert.ToInt64(paymentTypePerVisitDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"])
                    };
                    return paymentPerVisit;
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
