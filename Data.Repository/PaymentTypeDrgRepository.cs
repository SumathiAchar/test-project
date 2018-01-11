/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType DRGPayment DataAccess Layer

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
    public class PaymentTypeDrgRepository : BaseRepository, IPaymentTypeDrgPaymentRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeDrgRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeDrgRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractServiceTypeId"></param>
        /// <param name="dtDrg"></param>
        /// <param name="dtDoc"></param>
        /// <param name="dtDocValues"></param>
        /// <returns></returns>
        public static PaymentTypeDrg GetPaymentType(long contractServiceTypeId, DataTable dtDrg, DataTable dtDoc, DataTable dtDocValues)
        {
            PaymentTypeDrg paymentTypeDrg = null;

            if (dtDrg != null && dtDrg.Rows.Count > 0)
            {
                paymentTypeDrg = (from DataRow row in dtDrg.Rows
                                  where (Convert.ToInt64(DBNull.Value == row["contractServiceTypeId"]
                                      ? (long?)null
                                      : Convert.ToInt64(row["contractServiceTypeId"])) == contractServiceTypeId)
                                  select new PaymentTypeDrg
                                  {
                                      BaseRate = DBNull.Value == row["BaseRate"]
                                          ? (double?)null
                                          : Convert.ToDouble(row["BaseRate"]),
                                      ClaimFieldDocId =
                                          Convert.ToInt64(row["ClaimFieldDocID"]),
                                      ContractId = DBNull.Value == row["ContractId"]
                                          ? (long?)null
                                          : Convert.ToInt64(row["ContractId"]),
                                      ServiceTypeId =
                                          DBNull.Value == row["ContractServiceTypeID"]
                                              ? (long?)null
                                              : Convert.ToInt64(
                                                  row["ContractServiceTypeID"]),
                                      PaymentTypeId = (byte)Enums.PaymentTypeCodes.DrgPayment,
                                      PaymentTypeDetailId = DBNull.Value == row["PaymentTypeDetailId"]
                                       ? 0
                                       : Convert.ToInt64(
                                           row["PaymentTypeDetailId"]),
                                      ClaimFieldDoc = GetClaimFieldDoc(
                              Convert.ToInt64(row["ClaimFieldDocID"]), dtDoc, dtDocValues),
                                  }).FirstOrDefault();
            }

            return paymentTypeDrg;
        }

        /// <summary>
        /// Add Edit PaymentType DRG Schedules
        /// </summary>
        /// <param name="paymentTypeDrgPayment"></param>
        /// <returns></returns>
        public long AddEditPaymentTypeDrgPayment(PaymentTypeDrg paymentTypeDrgPayment)
        {
            //Checks if input request is not null
            if (paymentTypeDrgPayment != null)
            {
                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("AddEditDRGPayment");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@PaymentTypeDetailID", DbType.Int64, paymentTypeDrgPayment.PaymentTypeDetailId);
                _db.AddInParameter(_cmd, "@BaseRate ", DbType.Decimal, paymentTypeDrgPayment.BaseRate);
                _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypeDrgPayment.PaymentTypeId);
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypeDrgPayment.ContractId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypeDrgPayment.ServiceTypeId);
                _db.AddInParameter(_cmd, "@ClaimFieldDocID ", DbType.Int64, paymentTypeDrgPayment.ClaimFieldDocId);
                _db.AddInParameter(_cmd, "@UserName ", DbType.String, paymentTypeDrgPayment.UserName);

                // Retrieve the results of the Stored Procedure in Datatable
               return long.Parse(_db.ExecuteScalar(_cmd).ToString());

           
            }
           
            return 0;
        }

        /// <summary>
        /// Get PaymentType DRG Schedules
        /// </summary>
        /// <param name="paymentTypeDrgPayment"></param>
        /// <returns></returns>
        public PaymentTypeDrg GetPaymentTypeDrgPayment(PaymentTypeDrg paymentTypeDrgPayment)
        {
            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypeDrgPayment.PaymentTypeId);
            _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypeDrgPayment.ContractId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypeDrgPayment.ServiceTypeId);
            _db.AddInParameter(_cmd, "@ServiceLineTypeId", DbType.Int64, 0);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypeDrgPayment.UserName);

            // Retrieve the results of the Stored Procedure in Datatable
            DataSet paymentTypeDrgDataSet = _db.ExecuteDataSet(_cmd);
            if (paymentTypeDrgDataSet != null && paymentTypeDrgDataSet.Tables.Count > 0)
            {
                //populating ContractBasicInfo data
                if (paymentTypeDrgDataSet.Tables[0].Rows != null && paymentTypeDrgDataSet.Tables[0] != null && paymentTypeDrgDataSet.Tables[0].Rows.Count > 0)
                {
                    PaymentTypeDrg paymentDrgPayment = new PaymentTypeDrg
                    {
                        BaseRate = DBNull.Value == paymentTypeDrgDataSet.Tables[0].Rows[0]["BaseRate"] ? (double?)null : Convert.ToDouble(paymentTypeDrgDataSet.Tables[0].Rows[0]["BaseRate"]),
                        ClaimFieldDocId = DBNull.Value == paymentTypeDrgDataSet.Tables[0].Rows[0]["ClaimFieldDocID"] ? (int?)null : Convert.ToInt32(paymentTypeDrgDataSet.Tables[0].Rows[0]["ClaimFieldDocID"]),
                        PaymentTypeDetailId = Convert.ToInt64(paymentTypeDrgDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"])
                    };
                    return paymentDrgPayment;
                }
            }

            //returns response to Business layer
            return null;
        }

        /// <summary>
        ///  Gets all Relative Weight list.
        /// </summary>
        /// <returns>List of RelativeWeightList</returns>
        public List<RelativeWeight> GetAllRelativeWeightList()
        {
            List<RelativeWeight> relativeWeightList = new List<RelativeWeight>();

            _cmd = _db.GetStoredProcCommand("GetAllRelativeWeightList");
            DataSet relativeWeightDataSet = _db.ExecuteDataSet(_cmd);

            if (relativeWeightDataSet.IsTableDataPopulated(0))
            {
                if (relativeWeightDataSet.Tables[0].Rows != null && relativeWeightDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < relativeWeightDataSet.Tables[0].Rows.Count; i++)
                    {
                        RelativeWeight relativeWeight = new RelativeWeight
                        {
                            RelativeWeightId = long.Parse(relativeWeightDataSet.Tables[0].Rows[i]["RelativeWeightId"].ToString()),
                            RelativeWeightValue = Convert.ToString(relativeWeightDataSet.Tables[0].Rows[i]["Description"])
                        };
                        relativeWeightList.Add(relativeWeight);
                    }
                }
            }

            return relativeWeightList;
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
