/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 12-Aug-2013
/**  Summary        : Handles documents added , delete or view related to Contract
/**  User Story Id  : 5.User Story Add a new contract Figure 13
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
    public class ContractDocumentRepository : IContractDocumentRepository
    {

        // Variables
        private Database _databaseObj;
        DbCommand _databaseCommandObj;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractDocumentRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractDocumentRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Add Edit Contract related Documents
        /// </summary>
        /// <param name="contractDocs"></param>
        /// <returns>ResponseObject</returns>
        public ContractDoc AddEditContractDocs(ContractDoc contractDocs)
        {
            ContractDoc contractDoc = new ContractDoc();
            //Response object to hold the final results
            if (contractDocs != null)
            {
                    // Initialize the Stored Procedure
                    _databaseCommandObj = _databaseObj.GetStoredProcCommand("AddEditContractDocs");
                    // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                    _databaseObj.AddInParameter(_databaseCommandObj, "@ContractDocID ", DbType.Int64, contractDocs.ContractDocId);
                    _databaseObj.AddInParameter(_databaseCommandObj, "@ContractID ", DbType.Int64, contractDocs.ContractId);
                    _databaseObj.AddInParameter(_databaseCommandObj, "@ContractContent", DbType.Binary, contractDocs.ContractContent);
                    _databaseObj.AddInParameter(_databaseCommandObj, "@FileName ", DbType.String, contractDocs.FileName.ToTrim());
                    _databaseObj.AddInParameter(_databaseCommandObj, "@UserName ", DbType.String, contractDocs.UserName.ToTrim());
                    _databaseObj.AddInParameter(_databaseCommandObj, "@DocumentId", DbType.Guid, Guid.NewGuid());
                    // Retrieve the results of the Stored Procedure 
                    DataTable contractDocDataTable = _databaseObj.ExecuteDataSet(_databaseCommandObj).Tables[0];
                    if (contractDocDataTable != null && contractDocDataTable.Rows != null && contractDocDataTable.Rows.Count > 0)
                    {
                        contractDoc = new ContractDoc
                        {
                            ContractDocId = long.Parse(contractDocDataTable.Rows[0]["ContractDocID"].ToString()),
                            DocumentId = DBNull.Value == contractDocDataTable.Rows[0]["DocumentId"]
                                ? (Guid?)null
                                : (Guid)(contractDocDataTable.Rows[0]["DocumentId"])
                        };
                        //returns the response to Business layer
                        return contractDoc;
                    }
                //returns the Contract id to Business layer
                return contractDoc;
            }
            //returns 0 if any exception occurs
            return contractDoc;
        }

        /// <summary>
        /// Delete Contract document By Id
        /// </summary>
        /// <param name="contractDocs"></param>
        /// <returns>returnvalue</returns>
        public bool DeleteContractDoc(ContractDoc contractDocs)
        {
            //Response object to hold the final results
            bool result = false;
              // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("DeleteContractDocByID");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractDocID ", DbType.Int64, contractDocs.ContractDocId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName ", DbType.String, contractDocs.UserName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityID", DbType.String, contractDocs.FacilityId);
                // Retrieve the results of the Stored Procedure 
                int updatedRow = _databaseObj.ExecuteNonQuery(_databaseCommandObj);
                //returns the response to Business layer
                if (updatedRow > 0) result = true;
            
            //returns false if any exception occurs
            return result;
        }

        /// <summary>
        /// Get Contract Doc ById
        /// </summary>
        /// <param name="contractDocId"></param>
        /// <returns>contractDocs</returns>
        public ContractDoc GetContractDocById(long contractDocId)
        {
            
                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetContractDocById");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractDocByID ", DbType.Int64, contractDocId);
                // Retrieve the results of the Stored Procedure in Data table
                DataTable contractDocDataTable = _databaseObj.ExecuteDataSet(_databaseCommandObj).Tables[0];
                //Map data table to business objects
                if (contractDocDataTable != null && contractDocDataTable.Rows != null && contractDocDataTable.Rows.Count > 0)
                {
                    ContractDoc contractDocs = new ContractDoc
                    {
                        ContractDocId = long.Parse(contractDocDataTable.Rows[0]["ContractDocID"].ToString()),
                        InsertDate = DBNull.Value == contractDocDataTable.Rows[0]["InsertDate"]
                                         ? (DateTime?)null
                                         : Convert.ToDateTime(contractDocDataTable.Rows[0]["InsertDate"]),
                        UpdateDate = DBNull.Value == contractDocDataTable.Rows[0]["UpdateDate"]
                                         ? (DateTime?)null
                                         : Convert.ToDateTime(contractDocDataTable.Rows[0]["UpdateDate"]),
                       ContractId = DBNull.Value == contractDocDataTable.Rows[0]["ContractID"]
                                         ? (long?)null
                                         : long.Parse(
                                             contractDocDataTable.Rows[0]["ContractID"].ToString()),
                        ContractContent = DBNull.Value ==
                                          contractDocDataTable.Rows[0]["ContractContent"]
                                              ? null
                                              : (byte[])contractDocDataTable.Rows[0]["ContractContent"],
                        FileName = Convert.ToString(contractDocDataTable.Rows[0]["FileName"]),
                        DocumentId = DBNull.Value == contractDocDataTable.Rows[0]["DocumentId"]
                                     ? (Guid?)null
                                     : (Guid)(contractDocDataTable.Rows[0]["DocumentId"])
                    };
                    //returns the response to Business layer
                    return contractDocs;
                }
                
                return null;
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
