using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class PaymentTableRepository : IPaymentTableRepository
    {
        private Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTableRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTableRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Determines whether [is table name exists] [the specified claim field docs].
        /// </summary>
        /// <param name="claimFieldDocs">The claim field docs.</param>
        /// <returns></returns>
        public bool IsTableNameExists(ClaimFieldDoc claimFieldDocs)
        {
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("CheckDuplicateTableName");
            _databaseObj.AddInParameter(_databaseCommandObj, "@TableName", DbType.String, claimFieldDocs.TableName);
            _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityId", DbType.Int64, claimFieldDocs.FacilityId);
            return (bool)_databaseObj.ExecuteScalar(_databaseCommandObj);
        }

        /// <summary>
        /// Gets the payment table.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        public PaymentTableContainer GetPaymentTable(ClaimFieldDoc claimFieldDoc)
        {
            PaymentTableContainer paymentTableContainer = new PaymentTableContainer
            {
                ClaimFieldValues = new List<ClaimFieldValue>()
            };
            if (claimFieldDoc != null)
            {
                //holds the response
                string finalStrXml = string.Empty;
                //Checks for Payers, if payers exists stores it in DB
                if (claimFieldDoc.PageSetting != null &&
                    claimFieldDoc.PageSetting.SearchCriteriaList != null &&
                    claimFieldDoc.PageSetting.SearchCriteriaList.Any())
                {
                    finalStrXml = claimFieldDoc.XmlSerialize();
                }


                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetPaymentTable");
                _databaseCommandObj.CommandTimeout = claimFieldDoc.SessionTimeOut;
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@ClaimFieldDocID", DbType.Int64,
                    claimFieldDoc.ClaimFieldDocId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, claimFieldDoc.UserName);
                // ReSharper disable once PossibleNullReferenceException
                _databaseObj.AddInParameter(_databaseCommandObj, "@Take", DbType.Int32, claimFieldDoc.PageSetting.Take);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Skip", DbType.Int32, claimFieldDoc.PageSetting.Skip);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SortField", DbType.String, claimFieldDoc.PageSetting.SortField);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SortDirection", DbType.String, claimFieldDoc.PageSetting.SortDirection);
                _databaseObj.AddInParameter(_databaseCommandObj, "@XmlSearchCriteria", DbType.Xml, finalStrXml);
                
                // Retrieve the results of the Stored Procedure in Dataset

                DataSet paymentTableDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

                if (paymentTableDataSet.IsTableDataPopulated() && paymentTableDataSet.Tables.Count > 1)
                {
                    paymentTableContainer.Total = Convert.ToInt32(paymentTableDataSet.Tables[0].Rows[0][0]);

                    // Bind Claimfield Value Data
                    paymentTableContainer.ClaimFieldValues = new List<ClaimFieldValue>();
                    paymentTableContainer.ClaimFieldValues =
                        (from DataRow row in paymentTableDataSet.Tables[1].Rows
                         select new ClaimFieldValue
                         {
                             Identifier = Convert.ToString(row["Identifier"]),
                             Value = Convert.ToString(row["Value"])
                         }).ToList();


                }
            }
            return paymentTableContainer;
        }

        /// <summary>
        /// Gets the custom payment table.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        public PaymentTableContainer GetCustomPaymentTable(ClaimFieldDoc claimFieldDoc)
        {
            PaymentTableContainer paymentTableContainer = new PaymentTableContainer
            {
                ClaimFieldValues = new List<ClaimFieldValue>()
            };
            if (claimFieldDoc != null)
            {
                //holds the response
                string finalStrXml = string.Empty;
                //Checks for Payers, if payers exists stores it in DB
                if (claimFieldDoc.PageSetting != null &&
                    claimFieldDoc.PageSetting.SearchCriteriaList != null &&
                    claimFieldDoc.PageSetting.SearchCriteriaList.Any())
                {
                    finalStrXml = claimFieldDoc.XmlSerialize();
                }


                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetCustomPaymentTable");
                _databaseCommandObj.CommandTimeout = claimFieldDoc.SessionTimeOut;
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@ClaimFieldDocID", DbType.Int64, claimFieldDoc.ClaimFieldDocId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, claimFieldDoc.UserName);
                // ReSharper disable once PossibleNullReferenceException
                _databaseObj.AddInParameter(_databaseCommandObj, "@Take", DbType.Int32, claimFieldDoc.PageSetting.Take);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Skip", DbType.Int32, claimFieldDoc.PageSetting.Skip);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SortField", DbType.String, claimFieldDoc.PageSetting.SortField);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SortDirection", DbType.String, claimFieldDoc.PageSetting.SortDirection);
                _databaseObj.AddInParameter(_databaseCommandObj, "@XmlSearchCriteria", DbType.Xml, finalStrXml);

                // Retrieve the results of the Stored Procedure in Dataset

                DataSet paymentDataset = _databaseObj.ExecuteDataSet(_databaseCommandObj);

                if (paymentDataset.IsTableDataPopulated() && paymentDataset.Tables.Count > 1 && paymentDataset.Tables[0].Rows.Count > 0)
                {
                    paymentTableContainer.Total = Convert.ToInt32(paymentDataset.Tables[0].Rows[0][0]);

                    // Bind Claimfield Value Data
                    paymentTableContainer.ClaimFieldValues =
                        (from DataRow row in paymentDataset.Tables[1].Rows
                         select new ClaimFieldValue
                         {
                             ColumnHeaderFirst = Convert.ToString(row["Identifier"]),
                             Value = Convert.ToString(row["Value"])
                         }).ToList();
                }
            }
            return paymentTableContainer;
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseCommandObj.Dispose();
            _databaseObj = null;
        }
    }
}