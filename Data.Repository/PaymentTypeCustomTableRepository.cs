using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    public class PaymentTypeCustomTableRepository : BaseRepository, IPaymentTypeCustomTableRepository
    {
        private readonly Database _database;
        private DbCommand _command;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeCustomTableRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeCustomTableRepository(string connectionString)
        {
            _database = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <returns></returns>
        public string GetHeaders(long documentId)
        {
            _command = _database.GetStoredProcCommand("GetDocumentHeaders");
            _database.AddInParameter(_command, "@DocumentId", DbType.Int64, documentId);
            _database.AddOutParameter(_command, "@Headers", DbType.String, -1);
            _database.ExecuteScalar(_command);

            return Convert.ToString(_command.Parameters["@Headers"].Value);
        }

        /// <summary>
        /// Adds the edit.
        /// </summary>
        /// <param name="paymentTypeCustomTable">The payment type custom table.</param>
        /// <returns></returns>
        public long AddEdit(PaymentTypeCustomTable paymentTypeCustomTable)
        {
            if (paymentTypeCustomTable != null)
            {

                long paymentTypeDetailId;
                _command = _database.GetStoredProcCommand("AddEditCustomTablePayment");
                _database.AddInParameter(_command, "@PaymentTypeDetailID", DbType.Int64, paymentTypeCustomTable.PaymentTypeDetailId);
                _database.AddInParameter(_command, "@Expression", DbType.String, paymentTypeCustomTable.Expression);
                _database.AddInParameter(_command, "@DocumentId", DbType.Int64, paymentTypeCustomTable.DocumentId);
                _database.AddInParameter(_command, "@ClaimFieldId", DbType.Int64, paymentTypeCustomTable.ClaimFieldId);
                _database.AddInParameter(_command, "@PaymentTypeID ", DbType.Int64, paymentTypeCustomTable.PaymentTypeId);
                _database.AddInParameter(_command, "@ContractID", DbType.Int64, paymentTypeCustomTable.ContractId);
                _database.AddInParameter(_command, "@ContractServiceTypeID", DbType.Int64, paymentTypeCustomTable.ServiceTypeId);
                _database.AddInParameter(_command, "@UserName", DbType.String,
                    paymentTypeCustomTable.UserName);
               _database.AddInParameter(_command, "@MultiplierFirst", DbType.String, paymentTypeCustomTable.MultiplierFirst);
                _database.AddInParameter(_command, "@MultiplierSecond", DbType.String, paymentTypeCustomTable.MultiplierSecond);
                _database.AddInParameter(_command, "@MultiplierThird", DbType.String, paymentTypeCustomTable.MultiplierThird);
                _database.AddInParameter(_command, "@MultiplierFour", DbType.String, paymentTypeCustomTable.MultiplierFourth);
                _database.AddInParameter(_command, "@MultiplierOther", DbType.String, paymentTypeCustomTable.MultiplierOther);
                _database.AddInParameter(_command, "@IsObserveServiceUnit", DbType.Boolean, paymentTypeCustomTable.IsObserveServiceUnit);
                _database.AddInParameter(_command, "@ObserveServiceUnitLimit", DbType.String, paymentTypeCustomTable.ObserveServiceUnitLimit);
                _database.AddInParameter(_command, "@IsPerDayOfStay", DbType.Boolean, paymentTypeCustomTable.IsPerDayOfStay);
                _database.AddInParameter(_command, "@IsPerCode", DbType.Boolean, paymentTypeCustomTable.IsPerCode);
                long.TryParse(Convert.ToString(_database.ExecuteScalar(_command)), out paymentTypeDetailId);
                return paymentTypeDetailId;
            }
            return 0;
        }

        /// <summary>
        /// Gets the payment type custom table details.
        /// </summary>
        /// <param name="paymentTypeCustomTable">The payment type custom table.</param>
        /// <returns></returns>
        public PaymentTypeCustomTable GetPaymentTypeCustomTableDetails(PaymentTypeCustomTable paymentTypeCustomTable)
        {

            if (paymentTypeCustomTable != null)
            {
                // Initialize the Stored Procedure
                _command = _database.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _database.AddInParameter(_command, "@PaymentTypeID ", DbType.Int64, paymentTypeCustomTable.PaymentTypeId);
                _database.AddInParameter(_command, "@ContractID", DbType.Int64, paymentTypeCustomTable.ContractId);
                _database.AddInParameter(_command, "@ContractServiceTypeID", DbType.Int64,
                    paymentTypeCustomTable.ServiceTypeId);
                _database.AddInParameter(_command, "@ServiceLineTypeId", DbType.Int64, 0);
                _database.AddInParameter(_command, "@UserName", DbType.String, paymentTypeCustomTable.UserName);

                // Retrieve the results of the Stored Procedure in Data set
                DataSet paymentTypeCustomDataSet = _database.ExecuteDataSet(_command);
                if (paymentTypeCustomDataSet != null && paymentTypeCustomDataSet.Tables.Count > 0)
                {
                    //populating ContractBasicInfo data
                    if (paymentTypeCustomDataSet.Tables[0].Rows != null && paymentTypeCustomDataSet.Tables[0] != null && paymentTypeCustomDataSet.Tables[0].Rows.Count > 0)
                    {
                        PaymentTypeCustomTable paymentTypeCustomTableDetails = new PaymentTypeCustomTable
                        {
                            Expression = Convert.ToString(paymentTypeCustomDataSet.Tables[0].Rows[0]["Formula"]),
                            DocumentId = Convert.ToInt64(paymentTypeCustomDataSet.Tables[0].Rows[0]["ClaimFieldDocId"]),
                            PaymentTypeDetailId = Convert.ToInt64(paymentTypeCustomDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"]),
                            ClaimFieldId = Convert.ToInt64(paymentTypeCustomDataSet.Tables[0].Rows[0]["ClaimFieldId"]),
                            MultiplierFirst = Convert.ToString(paymentTypeCustomDataSet.Tables[0].Rows[0]["MultiplierFirst"]),
                            MultiplierSecond = Convert.ToString(paymentTypeCustomDataSet.Tables[0].Rows[0]["MultiplierSecond"]),
                            MultiplierThird = Convert.ToString(paymentTypeCustomDataSet.Tables[0].Rows[0]["MultiplierThird"]),
                            MultiplierFourth = Convert.ToString(paymentTypeCustomDataSet.Tables[0].Rows[0]["MultiplierFour"]),
                            MultiplierOther = Convert.ToString(paymentTypeCustomDataSet.Tables[0].Rows[0]["MultiplierOther"]),
                            IsObserveServiceUnit = Convert.ToBoolean(paymentTypeCustomDataSet.Tables[0].Rows[0]["IsObserveServiceUnit"]),
                            ObserveServiceUnitLimit = Convert.ToString(paymentTypeCustomDataSet.Tables[0].Rows[0]["ObserveServiceUnitLimit"]),
                            IsPerDayOfStay = Convert.ToBoolean(paymentTypeCustomDataSet.Tables[0].Rows[0]["IsPerDayOfStay"]),
                            IsPerCode = Convert.ToBoolean(paymentTypeCustomDataSet.Tables[0].Rows[0]["IsPerCode"])
                        };
                        return paymentTypeCustomTableDetails;
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Gets the payment type fee schedules.
        /// </summary>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dtCustomPaymentTable">The dt fee schedule table.</param>
        /// <param name="dtDoc">The dt document.</param>
        /// <param name="dtDocValues">The dt document values.</param>
        /// <returns></returns>
        public static PaymentTypeCustomTable GetPaymentType(long contractServiceTypeId, DataTable dtCustomPaymentTable,
            DataTable dtDoc, DataTable dtDocValues)
        {
            PaymentTypeCustomTable paymentTypeCustomTable = null;
            if (dtCustomPaymentTable != null && dtCustomPaymentTable.Rows.Count > 0)
            {
                paymentTypeCustomTable = (from DataRow row in dtCustomPaymentTable.Rows
                    where (Convert.ToInt64(DBNull.Value == row["ContractServiceTypeID"]
                        ? (long?) null
                        : Convert.ToInt64(
                            row["ContractServiceTypeID"])) == contractServiceTypeId)
                    select new PaymentTypeCustomTable
                    {
                        ClaimFieldDocId =
                            Convert.ToInt64(row["ClaimFieldDocID"]),
                        ContractId = DBNull.Value == row["ContractId"]
                            ? (long?) null
                            : Convert.ToInt64(
                                row["ContractId"]),
                        ServiceTypeId = DBNull.Value ==
                                        row["ContractServiceTypeID"]
                            ? (long?) null
                            : Convert.ToInt64(
                                row["ContractServiceTypeID"]),
                        PaymentTypeId = (byte) Enums.PaymentTypeCodes.CustomTableFormulas,
                        ClaimFieldId =
                            DBNull.Value == row["ClaimFieldId"] ? (long?) null : Convert.ToInt64(row["ClaimFieldId"]),
                        Expression = DBNull.Value == row["Formula"] ? null : Convert.ToString(row["Formula"]),
                        ClaimFieldDoc = GetClaimFieldDoc(
                            Convert.ToInt64(row["ClaimFieldDocID"]), dtDoc, dtDocValues),
                        MultiplierFirst = DBNull.Value == row["MultiplierFirst"] ? null : Convert.ToString(row["MultiplierFirst"]),
                        MultiplierSecond = DBNull.Value == row["MultiplierSecond"] ? null : Convert.ToString(row["MultiplierSecond"]),
                        MultiplierThird = DBNull.Value == row["MultiplierThird"] ? null : Convert.ToString(row["MultiplierThird"]),
                        MultiplierFourth = DBNull.Value == row["MultiplierFour"] ? null : Convert.ToString(row["MultiplierFour"]),
                        MultiplierOther = DBNull.Value == row["MultiplierOther"] ? null : Convert.ToString(row["MultiplierOther"]),
                        ObserveServiceUnitLimit = DBNull.Value == row["ObserveServiceUnitLimit"] ? null : Convert.ToString(row["ObserveServiceUnitLimit"]),
                        IsObserveServiceUnit = Convert.ToBoolean(row["IsObserveServiceUnit"]),
                        IsPerDayOfStay = Convert.ToBoolean(row["IsPerDayOfStay"]),
                        IsPerCode = Convert.ToBoolean(row["IsPerCode"])
                    }).FirstOrDefault();
                if (paymentTypeCustomTable != null)
                {
                    List<ICondition> conditions = new List<ICondition>();
                    ICondition conditionCode = new Condition
                    {
                        ConditionOperator = (int) Enums.ConditionOperation.EqualTo,
                        OperandIdentifier = (int?) paymentTypeCustomTable.ClaimFieldId,
                        RightOperand =
                            (int?) paymentTypeCustomTable.ClaimFieldId == 6
                                ? string.Join(";",
                                    paymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.Select(
                                        x =>
                                            x.Identifier.Replace(Constants.Apostrophe + Constants.Apostrophe,
                                                Constants.Apostrophe)
                                                .Replace(Constants.MaskCommaInValue, Constants.Comma)))
                                : string.Join(",",
                                    paymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.Select(
                                        x =>
                                            x.Identifier.Replace(Constants.Apostrophe + Constants.Apostrophe,
                                                Constants.Apostrophe)
                                                .Replace(Constants.MaskCommaInValue, Constants.Comma))).Replace(" ", "")
                    };
                    conditionCode = UpdatePropertyAndOperand(conditionCode);
                    conditions.Add(conditionCode);
                    paymentTypeCustomTable.Conditions = conditions;
                }
            }
            return paymentTypeCustomTable;
        }
    }
}