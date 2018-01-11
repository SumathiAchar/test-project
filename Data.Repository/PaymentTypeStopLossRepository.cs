/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 23-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType StopLoss DataAccess Layer

/************************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class PaymentTypeStopLossRepository : BaseRepository, IPaymentTypeStopLossRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeStopLossRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeStopLossRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dtData">The dt data.</param>
        /// <returns></returns>
        public static PaymentTypeStopLoss GetPaymentType(long? contractId, long? contractServiceTypeId, DataTable dtData)
        {
            PaymentTypeStopLoss paymentTypeStopLoss = null;
            if (dtData != null && dtData.Rows.Count > 0)
            {
                paymentTypeStopLoss = (from DataRow row in dtData.Rows
                    where
                        ((row["ContractId"] != DBNull.Value &&
                          Convert.ToInt64(row["ContractId"]) == contractId)
                         ||
                         (row["contractServiceTypeId"] != DBNull.Value &&
                          Convert.ToInt64(row["contractServiceTypeId"]) == contractServiceTypeId))
                    select new PaymentTypeStopLoss
                    {
                        Percentage = Convert.ToDouble(
                            row["Percentage"]),
                        ContractId = DBNull.Value == row["ContractId"]
                            ? (long?) null
                            : Convert.ToInt64(
                                row["ContractId"]),
                        ServiceTypeId =
                            DBNull.Value == row["ContractServiceTypeID"]
                                ? (long?) null
                                : Convert.ToInt64(
                                    row["ContractServiceTypeID"]),
                        RevCode = Convert.ToString(row["RevCode"]).Replace(" ",""),
                        HcpcsCode = Convert.ToString(row["CPTCODE"]).Replace(" ", ""),
                        Expression = Convert.ToString(row["Threshold"]),
                        PaymentTypeDetailId = Convert.ToInt64(row["PaymentTypeDetailID"]),
                        StopLossConditionId = Convert.ToInt32(row["StopLossConditionID"]),
                        IsExcessCharge = Convert.ToBoolean(row["IsExcessCharge"]),
                        Days = Convert.ToString(row["Days"]),
                        PaymentTypeId = (byte) Enums.PaymentTypeCodes.StopLoss
                    }).FirstOrDefault();

                if (paymentTypeStopLoss != null)
                {
                    List<ICondition> conditions = new List<ICondition>();
                    if (!string.IsNullOrEmpty(paymentTypeStopLoss.RevCode))
                    {
                        ICondition conditionRevCode = new Condition
                        {
                            ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                            OperandIdentifier = (byte) Enums.OperandIdentifier.RevCode,
                            RightOperand = paymentTypeStopLoss.RevCode
                        };
                        conditionRevCode = UpdatePropertyAndOperand(conditionRevCode);
                        conditions.Add(conditionRevCode);
                    }
                    if (!string.IsNullOrEmpty(paymentTypeStopLoss.HcpcsCode))
                    {
                        ICondition conditionHcpcsCode = new Condition
                        {
                            ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                            OperandIdentifier = (byte) Enums.OperandIdentifier.HcpcsCode,
                            RightOperand = paymentTypeStopLoss.HcpcsCode
                        };
                        conditionHcpcsCode = UpdatePropertyAndOperand(conditionHcpcsCode);
                        conditions.Add(conditionHcpcsCode);
                    }
                    paymentTypeStopLoss.Conditions = conditions;
                }

            }
            return paymentTypeStopLoss;
        }

        /// <summary>
        /// Add Edit Payment Type StopLoss
        /// </summary>
        /// <param name="paymentTypeStopLoss"></param>
        /// <returns></returns>
        public long AddEditPaymentTypeStopLoss(PaymentTypeStopLoss paymentTypeStopLoss)
        {
            //Checks if input request is not null
            if (paymentTypeStopLoss != null)
            {
                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("AddEditStopLossPayment");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@PaymentTypeDetailID", DbType.Int64, paymentTypeStopLoss.PaymentTypeDetailId);
                _db.AddInParameter(_cmd, "@Threshold ", DbType.String, paymentTypeStopLoss.Expression.ToUpper());
                _db.AddInParameter(_cmd, "@Percentage ", DbType.Double, paymentTypeStopLoss.Percentage);
                _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypeStopLoss.PaymentTypeId);
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypeStopLoss.ContractId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypeStopLoss.ServiceTypeId);
                _db.AddInParameter(_cmd, "@Days", DbType.String, paymentTypeStopLoss.Days);
                _db.AddInParameter(_cmd, "@RevCode", DbType.String, paymentTypeStopLoss.RevCode);
                _db.AddInParameter(_cmd, "@CPTCode", DbType.String, paymentTypeStopLoss.HcpcsCode);
                _db.AddInParameter(_cmd, "@StopLossConditionID", DbType.Int64, paymentTypeStopLoss.StopLossConditionId);
                _db.AddInParameter(_cmd, "@IsExcessCharge", DbType.Boolean, paymentTypeStopLoss.IsExcessCharge);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypeStopLoss.UserName);

                // Retrieve the results of the Stored Procedure in Data table
                return long.Parse(_db.ExecuteScalar(_cmd).ToString());


              
            }
           
            return 0;
        }

        /// <summary>
        /// Get Payment Type StopLoss
        /// </summary>
        /// <param name="paymentTypeStopLoss"></param>
        /// <returns></returns>
        public PaymentTypeStopLoss GetPaymentTypeStopLoss(PaymentTypeStopLoss paymentTypeStopLoss)
        {

            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypeStopLoss.PaymentTypeId);
            _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypeStopLoss.ContractId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypeStopLoss.ServiceTypeId);
            _db.AddInParameter(_cmd, "@ServiceLineTypeId", DbType.Int64, 0);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypeStopLoss.UserName);

            // Retrieve the results of the Stored Procedure in Data set
            DataSet paymentTypeStopLossDataSet = _db.ExecuteDataSet(_cmd);
            if (paymentTypeStopLossDataSet != null && paymentTypeStopLossDataSet.Tables.Count > 0)
            {
                //populating StopLoss data
                if (paymentTypeStopLossDataSet.Tables[0].Rows != null && paymentTypeStopLossDataSet.Tables[0] != null && paymentTypeStopLossDataSet.Tables[0].Rows.Count > 0)
                {
                    PaymentTypeStopLoss paymentStopLoss = new PaymentTypeStopLoss
                    {
                        Expression = Convert.ToString(paymentTypeStopLossDataSet.Tables[0].Rows[0]["Threshold"]),
                        Percentage = DBNull.Value == paymentTypeStopLossDataSet.Tables[0].Rows[0]["Percentage"] ? (double?)null : Convert.ToDouble(paymentTypeStopLossDataSet.Tables[0].Rows[0]["Percentage"]),
                        PaymentTypeDetailId = Convert.ToInt64(paymentTypeStopLossDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"]),
                        Days = Convert.ToString(paymentTypeStopLossDataSet.Tables[0].Rows[0]["Days"]),
                        RevCode = Convert.ToString(paymentTypeStopLossDataSet.Tables[0].Rows[0]["RevCode"]),
                        HcpcsCode = Convert.ToString(paymentTypeStopLossDataSet.Tables[0].Rows[0]["CPTCode"]),
                        StopLossConditionId = Convert.ToInt32(paymentTypeStopLossDataSet.Tables[0].Rows[0]["StopLossConditionID"]),
                        IsExcessCharge = paymentTypeStopLossDataSet.Tables[0].Rows[0]["IsExcessCharge"] != DBNull.Value && Convert.ToBoolean(paymentTypeStopLossDataSet.Tables[0].Rows[0]["IsExcessCharge"]),
                    };
                    return paymentStopLoss;
                }
            }

            //returns response to Business layer
            return null;
        }

        /// <summary>
        /// Gets the payment type stop loss conditions.
        /// </summary>
        /// <returns></returns>
        public List<StopLossCondition> GetPaymentTypeStopLossConditions()
        {
            List<StopLossCondition> stopLossConditions = new List<StopLossCondition>();

            _cmd = _db.GetStoredProcCommand("GetAllStopLossConditions");
            DataSet paymentTypeStopLossDataSet = _db.ExecuteDataSet(_cmd);

            if (paymentTypeStopLossDataSet != null && paymentTypeStopLossDataSet.Tables.Count > 0)
            {
                for (int i = 0; i < paymentTypeStopLossDataSet.Tables[0].Rows.Count; i++)
                {
                    StopLossCondition stopLossCondition = new StopLossCondition
                    {
                        StopLossConditionId = long.Parse(paymentTypeStopLossDataSet.Tables[0].Rows[i]["StopLossConditionID"].ToString()),
                        StopLossConditionName = Convert.ToString(paymentTypeStopLossDataSet.Tables[0].Rows[i]["StopLossConditionName"]).Trim()
                    };
                    stopLossConditions.Add(stopLossCondition);
                }
            }

            return stopLossConditions;
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
