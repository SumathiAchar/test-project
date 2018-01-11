using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ClaimFieldDocRepository : IClaimFieldDocRepository
    {
        // Variables
        private Database _databaseObj;
        DbCommand _databaseCommandObj;
        private readonly SqlDatabase _databaseSqlObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimFieldDocRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ClaimFieldDocRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
            _databaseSqlObj = new SqlDatabase(connectionString);
        }

        /// <summary>
        /// Adds the claim field docs.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        public long AddClaimFieldDocs(ClaimFieldDoc claimFieldDoc)
        {
            long claimFieldDocId = 0;
            //Checks if input request is not null
            if (claimFieldDoc != null)
            {
                //holds the response
                DataTable convertToDataTable = new DataTable();
                // If we are uploading custom payment table
                if (claimFieldDoc.ClaimFieldId == (byte)Enums.ClaimFieldTypes.CustomPaymentType)
                {
                    //Looping through each claim field value
                    foreach (var claimFieldValue in claimFieldDoc.ClaimFieldValues)
                    {
                        //Checks for ClaimFieldValues
                        if (claimFieldDoc.ClaimFieldValues != null && claimFieldDoc.ClaimFieldValues.Any())
                        {
                            List<UploadTable> claimFieldValueList = new List<UploadTable>
                            {
                                new UploadTable
                                {
                                    InsertDate = null,
                                    UpdateDate = null,
                                    Identifier = null,
                                    Value = claimFieldValue.Value
                                }
                            };
                            // Converting the data to data table to insert more records 
                            convertToDataTable = Utilities.ToDataTable(claimFieldValueList);
                        }
                        // Inserting into database
                        claimFieldDocId = InsertClaimFieldDoc(claimFieldDoc, convertToDataTable);
                    }
                }
                // other than custom payment table
                else
                {
                    //Checks for ClaimFieldValues
                    if (claimFieldDoc.ClaimFieldValues != null && claimFieldDoc.ClaimFieldValues.Any())
                    {
                        List<UploadTable> claimFieldValueList =
                            (from ClaimFieldValue row in claimFieldDoc.ClaimFieldValues
                             select new UploadTable
                             {
                                 InsertDate = null,
                                 UpdateDate = null,
                                 Identifier = row.Identifier,
                                 Value = row.Value,
                                 FacilityId = null,
                                 ClaimFieldDocId = null
                             }).ToList();
                        convertToDataTable = Utilities.ToDataTable(claimFieldValueList);
                        claimFieldDocId = InsertClaimFieldDoc(claimFieldDoc, convertToDataTable);
                    }
                }
                //returns response to Business layer
                return claimFieldDocId;
            }
            return 0;
        }

        /// <summary>
        /// Inserts the claim field document.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <param name="convertToDataTable">The convert to data table.</param>
        /// <returns></returns>
        private long InsertClaimFieldDoc(ClaimFieldDoc claimFieldDoc, DataTable convertToDataTable)
        {
            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseSqlObj.GetStoredProcCommand("[AddEditClaimFieldDocs]");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseSqlObj.AddInParameter(_databaseCommandObj, "@ClaimFieldDocID", SqlDbType.BigInt,
                claimFieldDoc.ClaimFieldDocId);
            _databaseSqlObj.AddInParameter(_databaseCommandObj, "@FileName", SqlDbType.VarChar,
                claimFieldDoc.FileName.ToTrim());
            _databaseSqlObj.AddInParameter(_databaseCommandObj, "@TableName", SqlDbType.VarChar,
                 claimFieldDoc.TableName.ToTrim());
            _databaseSqlObj.AddInParameter(_databaseCommandObj, "@ClaimFieldID", SqlDbType.BigInt,
                claimFieldDoc.ClaimFieldId);
            _databaseSqlObj.AddInParameter(_databaseCommandObj, "@ColumnHeaderFirst", SqlDbType.VarChar,
                claimFieldDoc.ColumnHeaderFirst.ToTrim());
            _databaseSqlObj.AddInParameter(_databaseCommandObj, "@ColumnHeaderSecond", SqlDbType.VarChar,
                claimFieldDoc.ColumnHeaderSecond.ToTrim());
            _databaseSqlObj.AddInParameter(_databaseCommandObj, "@FacilityID", SqlDbType.BigInt,
                claimFieldDoc.FacilityId);
            _databaseSqlObj.AddInParameter(_databaseCommandObj, "@XmlClaimFieldValues",
                SqlDbType.Structured, convertToDataTable);
            _databaseSqlObj.AddInParameter(_databaseCommandObj, "@UserName", SqlDbType.VarChar,
                claimFieldDoc.UserName);
            // Retrieve the results of the Stored Procedure in Dataset
            long returnValue = Convert.ToInt64(_databaseSqlObj.ExecuteScalar(_databaseCommandObj));

            return returnValue;
        }

        /// <summary>
        /// Gets the table look up details by contract unique identifier.
        /// </summary>
        /// <param name="claimFieldDoc"></param>
        /// <returns></returns>
        public List<ClaimFieldDoc> GetClaimFieldDocs(ClaimFieldDoc claimFieldDoc)
        {
            //holds the response data
            List<ClaimFieldDoc> claimFieldDocs = new List<ClaimFieldDoc>();

            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetTableLookUpDetailsByContractID");
            _databaseObj.AddInParameter(_databaseCommandObj, "@ContractId", DbType.Int64, claimFieldDoc.ContractId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@ClaimFieldId", DbType.Int64,
                                        claimFieldDoc.ClaimFieldId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@ClaimFieldDocId", DbType.Int64,
                                        claimFieldDoc.ClaimFieldDocId);
            // Retrieve the results of the Stored Procedure 

            DataSet claimFieldDocDetails = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            //Map datatable to business objects
            // sending 0 to insure that table at index is having valid data.
            if (claimFieldDocDetails.IsTableDataPopulated(0))
            {
                DataTable dataTable = claimFieldDocDetails.Tables[0];
                for (int indexCount = 0; indexCount < dataTable.Rows.Count; indexCount++)
                {
                    ClaimFieldDoc tempData = new ClaimFieldDoc
                    {
                        TableName = Convert.ToString(dataTable.Rows[indexCount]["TableName"]),
                        ClaimFieldDocId = DBNull.Value != dataTable.Rows[indexCount]["ClaimFieldDocID"]
                                                                   ? long.Parse(dataTable.Rows[indexCount]["ClaimFieldDocID"].ToString()) : 0

                    };
                    claimFieldDocs.Add(tempData);
                }

                if (claimFieldDocs.Any())
                {
                    List<ClaimFieldValue> claimFieldValueList = new List<ClaimFieldValue>();
                    if (claimFieldDocDetails.IsTableDataPopulated(1))
                    {
                        dataTable = claimFieldDocDetails.Tables[1];
                        for (int indexCount = 0; indexCount < dataTable.Rows.Count; indexCount++)
                        {
                            ClaimFieldValue tempData = new ClaimFieldValue
                            {
                                ColumnHeaderSecond = Convert.ToString(dataTable.Rows[indexCount]["ColumnHeaderSecond"]),
                                ColumnHeaderFirst = Convert.ToString(dataTable.Rows[indexCount]["ColumnHeaderFirst"]),
                                Value = Convert.ToString(dataTable.Rows[indexCount]["Value"]),
                                Identifier = Convert.ToString(dataTable.Rows[indexCount]["Identifier"]),
                                ClaimFieldValueId = DBNull.Value == dataTable.Rows[indexCount]["ClaimFieldValueID"]
                                                                           ? long.Parse(dataTable.Rows[indexCount]["ClaimFieldValueID"].ToString()) : 0,
                                ClaimFieldDocId = DBNull.Value != dataTable.Rows[indexCount]["ClaimFieldDocID"]
                                                                           ? long.Parse(dataTable.Rows[indexCount]["ClaimFieldDocID"].ToString()) : 0

                            };
                            claimFieldValueList.Add(tempData);
                        }
                    }
                    if (true & claimFieldDocs.Any() && claimFieldValueList.Any())
                    {
                        claimFieldDocs.First().ClaimFieldValues = claimFieldValueList.ToList();
                    }
                }
            }
            //returns the response to Business layer
            return claimFieldDocs;
        }

        /// <summary>
        /// Gets all claim fields.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<ClaimField> GetAllClaimFields()
        {
            List<ClaimField> claimFieldList = new List<ClaimField>();

            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetAllClaimFields");
            _databaseObj.AddInParameter(_databaseCommandObj, "@ShowFields", DbType.Int16, 0);
            DataSet claimFieldDataset = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            if (claimFieldDataset.IsTableDataPopulated(0))
            {
                claimFieldList = (from DataRow row in claimFieldDataset.Tables[0].Rows
                                  select new ClaimField
                                        {
                                            ClaimFieldId = long.Parse(row["ClaimFieldId"].ToString()),
                                            Text = Convert.ToString(row["Text"])
                                        }).ToList();


            }
            return claimFieldList;
        }

        /// <summary>
        /// Deletes the specified claim field document.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        public bool Delete(ClaimFieldDoc claimFieldDoc)
        {
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("DeleteClaimFieldDocByID");
            _databaseObj.AddInParameter(_databaseCommandObj, "@ClaimFieldDocId", DbType.Int64,
                claimFieldDoc.ClaimFieldDocId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, claimFieldDoc.UserName);
            bool returnValue = _databaseObj.ExecuteNonQuery(_databaseCommandObj) > 0;

            //returns the response to Business layer
            return returnValue;
        }



        /// <summary>
        /// Checks the claim field document.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        public List<ContractLog> IsDocumentInUse(ClaimFieldDoc claimFieldDoc)
        {
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("IsDocumentInUse");
            _databaseObj.AddInParameter(_databaseCommandObj, "@ClaimFieldDocId", DbType.Int64, claimFieldDoc.ClaimFieldDocId);
            DataSet paymentTable = _databaseObj.ExecuteDataSet(_databaseCommandObj);
            if (paymentTable.IsTableDataPopulated(0))
            {
                List<ContractLog> claimFieldDocInfo = (from DataRow row in paymentTable.Tables[0].Rows
                                                       select new ContractLog
                                                       {
                                                           ModelName = Convert.ToString(row["ModelName"]),
                                                           ContractName = Convert.ToString(row["ContractName"]),
                                                           ServiceTypeName = Convert.ToString(row["ContractServiceTypeName"]),
                                                       }).ToList();


                //returns the response to Business layer
                return claimFieldDocInfo;
            }
            //returns null if exception occurs or no data found
            return null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseObj = null;
            _databaseCommandObj.Dispose();
        }

        //Fixed-2016-R3-S2 : In stored procedure RenameClaimFieldDocByID and RenamePaymentTable rename @FacilityId to @FacilityID.
        //                    Other variables should use pascal naming convention. id should be ID.
        /// <summary>
        /// Rename Payment Table.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        public ClaimFieldDoc RenamePaymentTable(ClaimFieldDoc claimFieldDoc)
        {
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("RenameClaimFieldDocByID");
            _databaseObj.AddInParameter(_databaseCommandObj, "@ClaimFieldDocId", DbType.Int64,
                claimFieldDoc.ClaimFieldDocId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@TableName", DbType.String,
                claimFieldDoc.TableName);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, claimFieldDoc.UserName);
            _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityId", DbType.Int64, claimFieldDoc.FacilityId);
            DataSet claimFieldDocModelDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);
            //Check if output result tables exists or not
            if (claimFieldDocModelDataSet.IsTableDataPopulated())
            {
                claimFieldDoc.ClaimFieldDocId = Convert.ToInt32(claimFieldDocModelDataSet.Tables[0].Rows[0][0]);

            }
            return claimFieldDoc;
            //returns the response to Business layer
        }
    }
}
