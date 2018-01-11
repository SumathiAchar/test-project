/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Contract payers info , it add/modify multiple instances of payer contact information
/**  User Story Id  : 5.User Story Add a new contract Figure 11
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using System.Data;
using System.Data.Common;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ContractPayerInfoRepository : IContractPayerInfoRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractPayerInfoRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractPayerInfoRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        ///  Get Contract Payer Info data
        /// </summary>
        /// <param name="contractPayerInfoId"></param>
        /// <returns>contractPayerInfo</returns>
        public ContractPayerInfo GetContractPayerInfo(long contractPayerInfoId)
        {
            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetPayerInfoById");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@PayerInfoByID ", DbType.Int64, contractPayerInfoId);
            // Retrieve the results of the Stored Procedure 
            DataTable contractPayerDataTable = _db.ExecuteDataSet(_cmd).Tables[0];
            //Map datatable to business objects
            if (contractPayerDataTable != null && contractPayerDataTable.Rows != null && contractPayerDataTable.Rows.Count > 0)
            {
                ContractPayerInfo contractPayerInfo = new ContractPayerInfo
                {
                    ContractPayerInfoId =
                        Convert.ToInt64(contractPayerDataTable.Rows[0]["ContractInfoId"]),
                    InsertDate =
                        DBNull.Value == contractPayerDataTable.Rows[0]["InsertDate"]
                            ? (DateTime?)null
                            : Convert.ToDateTime(contractPayerDataTable.Rows[0]["InsertDate"]),
                    UpdateDate =
                        DBNull.Value == contractPayerDataTable.Rows[0]["UpdateDate"]
                            ? (DateTime?)null
                            : Convert.ToDateTime(contractPayerDataTable.Rows[0]["UpdateDate"]),
                    ContractId =
                        DBNull.Value == contractPayerDataTable.Rows[0]["ContractId"]
                            ? (long?)null
                            : Convert.ToInt64(contractPayerDataTable.Rows[0]["ContractId"]),
                    PayerId =
                        DBNull.Value == contractPayerDataTable.Rows[0]["ContractPayerID"]
                            ? (long?)null
                            : Convert.ToInt64(
                                contractPayerDataTable.Rows[0]["ContractPayerID"]),
                    ContractInfoPayerName =
                        Convert.ToString(
                            contractPayerDataTable.Rows[0]["ContractInfoName"]),
                    MailAddress1 =
                        Convert.ToString(contractPayerDataTable.Rows[0]["MailAddress1"]),
                    MailAddress2 =
                        Convert.ToString(contractPayerDataTable.Rows[0]["MailAddress2"]),
                    City = Convert.ToString(contractPayerDataTable.Rows[0]["City"]),
                    State = Convert.ToString(contractPayerDataTable.Rows[0]["State"]),
                    Zip = Convert.ToString(contractPayerDataTable.Rows[0]["Zip"]),
                    Phone1 = Convert.ToString(contractPayerDataTable.Rows[0]["Phone1"]),
                    Phone2 = Convert.ToString(contractPayerDataTable.Rows[0]["Phone2"]),
                    Fax = Convert.ToString(contractPayerDataTable.Rows[0]["Fax"]),
                    Email = Convert.ToString(contractPayerDataTable.Rows[0]["Email"]),
                    Website = Convert.ToString(contractPayerDataTable.Rows[0]["Website"]),
                    TaxId = Convert.ToString(contractPayerDataTable.Rows[0]["TaxId"]),
                    Npi = Convert.ToString(contractPayerDataTable.Rows[0]["NPI"]),
                    Memo = Convert.ToString(contractPayerDataTable.Rows[0]["Memo"]),
                    ProviderId = Convert.ToString(contractPayerDataTable.Rows[0]["ProvderID"]),
                    PlanId = Convert.ToString(contractPayerDataTable.Rows[0]["PlanId"])
                };
                //returns the response to Business layer
                return contractPayerInfo;
            }
           
            return null;
        }

        /// <summary>
        /// Add & Edit Contract Payer Info data
        /// </summary>
        /// <param name="contractPayerInfo"></param>
        /// <returns>return value</returns>
        public long AddEditContractPayerInfo(ContractPayerInfo contractPayerInfo)
        {
            //Checks if input request is not null
            if (contractPayerInfo != null)
            {
                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("AddEditContractPayerInfo");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@ContractPayerInfoID", DbType.Int64, contractPayerInfo.ContractPayerInfoId);
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, contractPayerInfo.ContractId);
                _db.AddInParameter(_cmd, "@ContractPayerID", DbType.Int64, contractPayerInfo.PayerId);
                _db.AddInParameter(_cmd, "@ContractPayerInfoName", DbType.String, contractPayerInfo.ContractInfoPayerName.ToTrim());
                _db.AddInParameter(_cmd, "@MailAddress1", DbType.String, contractPayerInfo.MailAddress1.ToTrim());
                _db.AddInParameter(_cmd, "@MailAddress2", DbType.String, contractPayerInfo.MailAddress2.ToTrim());
                _db.AddInParameter(_cmd, "@City", DbType.String, contractPayerInfo.City.ToTrim());
                _db.AddInParameter(_cmd, "@State", DbType.String, contractPayerInfo.State.ToTrim());
                _db.AddInParameter(_cmd, "@Zip", DbType.String, contractPayerInfo.Zip.ToTrim());
                _db.AddInParameter(_cmd, "@Phone1", DbType.String, contractPayerInfo.Phone1.ToTrim());
                _db.AddInParameter(_cmd, "@Phone2", DbType.String, contractPayerInfo.Phone2.ToTrim());
                _db.AddInParameter(_cmd, "@Fax", DbType.String, contractPayerInfo.Fax.ToTrim());
                _db.AddInParameter(_cmd, "@Email", DbType.String, contractPayerInfo.Email.ToTrim());
                _db.AddInParameter(_cmd, "@Website", DbType.String, contractPayerInfo.Website.ToTrim());
                _db.AddInParameter(_cmd, "@TaxID", DbType.String, contractPayerInfo.TaxId.ToTrim());
                _db.AddInParameter(_cmd, "@NPI", DbType.String, contractPayerInfo.Npi.ToTrim());
                _db.AddInParameter(_cmd, "@Memo", DbType.String, contractPayerInfo.Memo.ToTrim());
                _db.AddInParameter(_cmd, "@ProvderID", DbType.String, contractPayerInfo.ProviderId.ToTrim());
                _db.AddInParameter(_cmd, "@PlanId", DbType.String, contractPayerInfo.PlanId.ToTrim());
                _db.AddInParameter(_cmd, "@UserName", DbType.String, contractPayerInfo.UserName.ToTrim());
                // Retrieve the results of the Stored Procedure
                long returnValue = long.Parse(_db.ExecuteScalar(_cmd).ToString());
                    //returns response to Business layer
                return returnValue;
            } 
            return 0;
        }

        /// <summary>
        /// Delete Contract Payer Info By ID
        /// </summary>
        /// <param name="contractPayerInfo"></param>
        /// <returns>returnvalue</returns>
        public bool DeleteContractPayerInfo(ContractPayerInfo contractPayerInfo)
        {
            //holds the response data
            bool returnvalue = false;
            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("DeleteContractPayerInfoById");
            _db.AddInParameter(_cmd, "@contractPayerInfoID", DbType.Int64, contractPayerInfo.ContractPayerInfoId);
            _db.AddInParameter(_cmd, "@ContractId", DbType.Int64, contractPayerInfo.ContractId);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, contractPayerInfo.UserName.ToTrim());
            // Retrieve the results of the Stored Procedure in Datatable
            int updatedRow = _db.ExecuteNonQuery(_cmd);
            //returns response to Business layer
            if (updatedRow > 0)
                returnvalue = true;
            //returns false if any exception occurs
            return returnvalue;
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
