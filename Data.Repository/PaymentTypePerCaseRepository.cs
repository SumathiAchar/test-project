/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType PerCaseDetails DataAccess Layer

/************************************************************************************************************/
using System;
using System.Collections.Generic;
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
    public class PaymentTypePerCaseRepository : IPaymentTypePerCaseRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePerCaseRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypePerCaseRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the payment type per case.
        /// </summary>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dtPerCaseTable">The dt per case table.</param>
        /// <param name="serviceTypeConditions">The servicetype conditions.</param>
        /// <returns></returns>
        public static PaymentTypePerCase GetPaymentType(long contractServiceTypeId, DataTable dtPerCaseTable, List<ICondition> serviceTypeConditions)
        {
            PaymentTypePerCase paymentTypePerCase = null;
            if (dtPerCaseTable != null && dtPerCaseTable.Rows.Count > 0)
            {
                paymentTypePerCase = (from DataRow row in dtPerCaseTable.Rows
                                      where
                                          (row["contractServiceTypeId"] != DBNull.Value &&
                                           Convert.ToInt64(row["contractServiceTypeId"]) == contractServiceTypeId)
                                      select new PaymentTypePerCase
                                      {
                                          Rate = Convert.ToDouble(
                                                  row["Rate"]),
                                          ContractId = DBNull.Value == row["ContractId"]
                                              ? (long?)null
                                              : Convert.ToInt64(
                                                  row["ContractId"]),
                                          ServiceTypeId =
                                              DBNull.Value == row["ContractServiceTypeID"]
                                                  ? (long?)null
                                                  : Convert.ToInt64(
                                                      row["ContractServiceTypeID"]),
                                          MaxCasesPerDay = DBNull.Value == row["MaxCasesPerDay"]
                                             ? (int?)null
                                             : Convert.ToInt32(
                                                 row["MaxCasesPerDay"]),
                                          PaymentTypeDetailId = Convert.ToInt64(row["PaymentTypeDetailID"]),
                                          PaymentTypeId = (byte)Enums.PaymentTypeCodes.PerCase
                                      }).FirstOrDefault();

                if (serviceTypeConditions != null && paymentTypePerCase != null)
                {
                    var revCodeCondition =
                        serviceTypeConditions.FirstOrDefault(
                            q => q.OperandIdentifier == (byte)Enums.OperandIdentifier.RevCode);
                    if (revCodeCondition != null)
                        paymentTypePerCase.RevCode = revCodeCondition.RightOperand;

                    var cptCodeCondition =
                        serviceTypeConditions.FirstOrDefault(
                            q => q.OperandIdentifier == (byte)Enums.OperandIdentifier.HcpcsCode);
                    if (cptCodeCondition != null)
                        paymentTypePerCase.HcpcsCode = cptCodeCondition.RightOperand;
                }
            }
            return paymentTypePerCase;
        }

        /// <summary>
        /// Add and edit payment type per case.
        /// </summary>
        /// <param name="paymentTypePerCase">The payment type per case.</param>
        /// <returns></returns>
        public long AddEditPaymentTypePerCase(PaymentTypePerCase paymentTypePerCase)
        {
            //Checks if input request is not null
            if (paymentTypePerCase != null)
            {
                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("AddEditPerCasePayment");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@PaymentTypeDetailID", DbType.Int64, paymentTypePerCase.PaymentTypeDetailId);
                _db.AddInParameter(_cmd, "@Rate", DbType.Decimal, paymentTypePerCase.Rate);
                _db.AddInParameter(_cmd, "@PaymentTypeID", DbType.Int64, paymentTypePerCase.PaymentTypeId);
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypePerCase.ContractId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypePerCase.ServiceTypeId);
                _db.AddInParameter(_cmd, "@MaxCasesPerDay", DbType.Int32, paymentTypePerCase.MaxCasesPerDay);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypePerCase.UserName);

                // Retrieve the results of the Stored Procedure in Datatable
                return long.Parse(_db.ExecuteScalar(_cmd).ToString());

                
            }
           
            return 0;
        }

        /// <summary>
        /// Get Payment Type PerCase
        /// </summary>
        /// <param name="paymentTypePerCase"></param>
        /// <returns></returns>
        public PaymentTypePerCase GetPaymentTypePerCase(PaymentTypePerCase paymentTypePerCase)
        {

            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypePerCase.PaymentTypeId);
            _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypePerCase.ContractId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypePerCase.ServiceTypeId);
            _db.AddInParameter(_cmd, "@ServiceLineTypeId", DbType.Int64, 0);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypePerCase.UserName);

            // Retrieve the results of the Stored Procedure in Datatable
            DataSet paymentTypePerCaseDataSet = _db.ExecuteDataSet(_cmd);
            if (paymentTypePerCaseDataSet != null && paymentTypePerCaseDataSet.Tables.Count > 0)
            {
                //populating ContractBasicInfo data
                if (paymentTypePerCaseDataSet.Tables[0].Rows != null && paymentTypePerCaseDataSet.Tables[0] != null && paymentTypePerCaseDataSet.Tables[0].Rows.Count > 0)
                {
                    PaymentTypePerCase paymentPerCase = new PaymentTypePerCase
                    {
                        Rate = Convert.ToDouble(paymentTypePerCaseDataSet.Tables[0].Rows[0]["Rate"]),
                        PaymentTypeDetailId = Convert.ToInt64(paymentTypePerCaseDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"]),
                        MaxCasesPerDay = DBNull.Value == paymentTypePerCaseDataSet.Tables[0].Rows[0]["MaxCasesPerDay"] ? (int?)null : Convert.ToInt32(paymentTypePerCaseDataSet.Tables[0].Rows[0]["MaxCasesPerDay"])
                    };
                    return paymentPerCase;
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
