/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType ASCFeeScheduleDetails DataAccess Layer

/************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class PaymentTypeAscFeeScheduleRepository : BaseRepository, IPaymentTypeAscFeeScheduleRepository
    {
        /// <summary>
        /// Variable  _databaseObj
        /// </summary>
        private Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeAscFeeScheduleRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeAscFeeScheduleRepository(string connectionString)
        {
            // Create a database object
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);

        }

        /// <summary>
        /// Gets the payment type fee schedules.
        /// </summary>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="feeScheduleTable">The dt fee schedule table.</param>
        /// <param name="doc">The dt document.</param>
        /// <param name="docValues">The dt document values.</param>
        /// <returns></returns>
        public static PaymentTypeAscFeeSchedule GetPaymentType(long contractServiceTypeId, DataTable feeScheduleTable, DataTable doc, DataTable docValues)
        {
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = null;

            if (feeScheduleTable != null && feeScheduleTable.Rows.Count > 0)
            {
                paymentTypeAscFeeSchedule = (from DataRow row in feeScheduleTable.Rows
                                             where (Convert.ToInt64(DBNull.Value == row["ContractServiceTypeID"]
                                                 ? (long?)null
                                                 : Convert.ToInt64(
                                                     row["ContractServiceTypeID"])) == contractServiceTypeId)
                                             select new PaymentTypeAscFeeSchedule
                                             {
                                                 Primary = DBNull.Value == row["Primary"]
                                                     ? (double?)null
                                                     : Convert.ToDouble(row["Primary"]),
                                                 Secondary = DBNull.Value == row["Secondary"]
                                                     ? (double?)null
                                                     : Convert.ToDouble(
                                                         row["Secondary"]),
                                                 Tertiary = DBNull.Value == row["Tertiary"]
                                                     ? (double?)null
                                                     : Convert.ToDouble(row["Tertiary"]),
                                                 Quaternary = DBNull.Value == row["Quaternary"]
                                                     ? (double?)null
                                                     : Convert.ToDouble(
                                                         row["Quaternary"]),
                                                 Others = DBNull.Value == row["Others"]
                                                     ? (double?)null
                                                     : Convert.ToDouble(row["Others"]),
                                                 NonFeeSchedule = DBNull.Value == row["NonFeeSchedule"]
                                                     ? (double?)null
                                                     : Convert.ToDouble(row["NonFeeSchedule"]),
                                                 ClaimFieldDocId =
                                                     Convert.ToInt64(row["ClaimFieldDocID"]),
                                                 ContractId = DBNull.Value == row["ContractId"]
                                                     ? (long?)null
                                                     : Convert.ToInt64(
                                                         row["ContractId"]),
                                                 ServiceTypeId = DBNull.Value ==
                                                                 row["ContractServiceTypeID"]
                                                     ? (long?)null
                                                     : Convert.ToInt64(
                                                         row["ContractServiceTypeID"]),
                                                 PaymentTypeId = (byte)Enums.PaymentTypeCodes.AscFeeSchedule,
                                                 ClaimFieldDoc = GetClaimFieldDoc(
                                                     Convert.ToInt64(row["ClaimFieldDocID"]), doc, docValues),
                                                 OptionSelection = Convert.ToInt32(row["SelectedOption"])
                                             }).FirstOrDefault();
            }
            return paymentTypeAscFeeSchedule;
        }

        /// <summary>
        /// AddEdit PaymentType ASC Fee Schedule Details
        /// </summary>
        /// <param name="paymentTypeAscFeeSchedule"></param>
        /// <returns></returns>
        public long AddEditPaymentTypeAscFeeScheduleDetails(PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule)
        {
            long paymentTypeAscFeeScheduleId = 0;
            //Checks if input request is not null
            if (paymentTypeAscFeeSchedule != null)
            {
                // ReSharper disable once InconsistentNaming


                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("AddEditASCFeeSchedulePayment");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for

                _databaseObj.AddInParameter(_databaseCommandObj, "@PaymentTypeDetailID", DbType.Int64, paymentTypeAscFeeSchedule.PaymentTypeDetailId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Primary ", DbType.Decimal, paymentTypeAscFeeSchedule.Primary);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Secondary ", DbType.Decimal, paymentTypeAscFeeSchedule.Secondary);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Tertiary ", DbType.Decimal, paymentTypeAscFeeSchedule.Tertiary);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Quaternary ", DbType.Decimal, paymentTypeAscFeeSchedule.Quaternary);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Others ", DbType.Decimal, paymentTypeAscFeeSchedule.Others);
                _databaseObj.AddInParameter(_databaseCommandObj, "@NonFeeSchedule ", DbType.Decimal, paymentTypeAscFeeSchedule.NonFeeSchedule);
                _databaseObj.AddInParameter(_databaseCommandObj, "@PaymentTypeID ", DbType.Int64, paymentTypeAscFeeSchedule.PaymentTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractID", DbType.Int64, paymentTypeAscFeeSchedule.ContractId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceTypeID", DbType.Int64, paymentTypeAscFeeSchedule.ServiceTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ClaimFieldDocID", DbType.Int64, paymentTypeAscFeeSchedule.ClaimFieldDocId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SelectedOption", DbType.Int64, paymentTypeAscFeeSchedule.OptionSelection);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, paymentTypeAscFeeSchedule.UserName);
                // Retrieve the results of the Stored Procedure in Datatable
                paymentTypeAscFeeScheduleId = long.Parse(_databaseObj.ExecuteScalar(_databaseCommandObj).ToString());

            }
            //returns 0 if any exception occurs. Else returns the result to business layer
            return paymentTypeAscFeeScheduleId;
        }

        /// <summary>
        /// Get PaymentType ASC Fee Schedule Details
        /// </summary>
        /// <param name="paymentTypeAscFeeSchedule"></param>
        /// <returns></returns>
        public PaymentTypeAscFeeSchedule GetPaymentTypeAscFeeScheduleDetails(PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule)
        {
            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommandObj, "@PaymentTypeID ", DbType.Int64, paymentTypeAscFeeSchedule.PaymentTypeId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@ContractID", DbType.Int64, paymentTypeAscFeeSchedule.ContractId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceTypeID", DbType.Int64, paymentTypeAscFeeSchedule.ServiceTypeId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@ServiceLineTypeId", DbType.Int64, 0);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, paymentTypeAscFeeSchedule.UserName);

            // Retrieve the results of the Stored Procedure in Datatable
            DataSet paymentTypeDetails = _databaseObj.ExecuteDataSet(_databaseCommandObj);
            if (paymentTypeDetails != null && paymentTypeDetails.Tables.Count > 0)
            {
                var paymentTypeInfo = paymentTypeDetails.Tables[0];
                //populating ContractBasicInfo data
                if (paymentTypeInfo.Rows != null && paymentTypeInfo.Rows.Count > 0)
                {
                    PaymentTypeAscFeeSchedule ascFeeSchedule = new PaymentTypeAscFeeSchedule
                    {
                        Primary = DBNull.Value == paymentTypeInfo.Rows[0]["Primary"] ? (double?)null : Convert.ToDouble(paymentTypeInfo.Rows[0]["Primary"]),
                        Secondary = DBNull.Value == paymentTypeInfo.Rows[0]["Secondary"] ? (double?)null : Convert.ToDouble(paymentTypeInfo.Rows[0]["Secondary"]),
                        Tertiary = DBNull.Value == paymentTypeInfo.Rows[0]["Tertiary"] ? (double?)null : Convert.ToDouble(paymentTypeInfo.Rows[0]["Tertiary"]),
                        Quaternary = DBNull.Value == paymentTypeInfo.Rows[0]["Quaternary"] ? (double?)null : Convert.ToDouble(paymentTypeInfo.Rows[0]["Quaternary"]),
                        Others = DBNull.Value == paymentTypeDetails.Tables[0].Rows[0]["Others"] ? (double?)null : Convert.ToDouble(paymentTypeDetails.Tables[0].Rows[0]["Others"]),
                        NonFeeSchedule = DBNull.Value == paymentTypeInfo.Rows[0]["NonFeeSchedule"] ? (double?)null : Convert.ToDouble(paymentTypeInfo.Rows[0]["NonFeeSchedule"]),
                        ClaimFieldDocId = DBNull.Value == paymentTypeInfo.Rows[0]["ClaimFieldDocID"] ? (long?)null : Convert.ToInt64(paymentTypeInfo.Rows[0]["ClaimFieldDocID"]),
                        PaymentTypeDetailId = Convert.ToInt64(paymentTypeInfo.Rows[0]["PaymentTypeDetailID"]),
                        OptionSelection = Convert.ToInt32(paymentTypeInfo.Rows[0]["SelectedOption"])
                    };
                    //returns response to Business layer
                    return ascFeeSchedule;
                }
            }

            //if any exception occurs, returns null to Business layer 
            return null;
        }

        /// <summary>
        ///  Gets all TableName list.
        /// </summary>
        /// <returns>List of Table Names</returns>
        public List<PaymentTypeTableSelection> GetTableNameSelection(PaymentTypeAscFeeSchedule paymenttype)
        {
            List<PaymentTypeTableSelection> tableNameSelection = new List<PaymentTypeTableSelection>();
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetASCTableNames");
            _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityId", DbType.Int32, paymenttype.FacilityId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@ClaimFieldId", DbType.Int32, paymenttype.ClaimFieldId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserText", DbType.String, paymenttype.UserText);
            DataSet tableNamesDetails = _databaseObj.ExecuteDataSet(_databaseCommandObj);


            if (tableNamesDetails.IsTableDataPopulated(0))
            {
                tableNameSelection = (from DataRow row in tableNamesDetails.Tables[0].Rows
                                      select new PaymentTypeTableSelection
                                      {
                                          ClaimFieldDocId =
                                              long.Parse(row["ClaimFieldDocID"].ToString()),
                                          TableName = Convert.ToString(row["TableName"])
                                      }).ToList();
            }
            //returns response to Business layer
            return tableNameSelection;
        }

        /// <summary>
        /// Gets the asc fee schedule options.
        /// </summary>
        /// <returns></returns>
        public List<AscFeeScheduleOption> GetAscFeeScheduleOptions()
        {
            List<AscFeeScheduleOption> ascFeeScheduleOptions = new List<AscFeeScheduleOption>();

            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetASCFeeScheduleOptions");
            DataSet ascFeeScheduleOptionsInfo = _databaseObj.ExecuteDataSet(_databaseCommandObj);
            if (ascFeeScheduleOptionsInfo.IsTableDataPopulated(0))
            {
                ascFeeScheduleOptions = (from DataRow row in ascFeeScheduleOptionsInfo.Tables[0].Rows
                                         select new AscFeeScheduleOption
                                   {
                                       AscFeeScheduleOptionId = Convert.ToInt32(row["ASCFeeScheduleOptionId"]),
                                       AscFeeScheduleOptionName = Convert.ToString(row["ASCFeeScheduleOptionName"])
                                   }).ToList();
            }
            return ascFeeScheduleOptions;
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
