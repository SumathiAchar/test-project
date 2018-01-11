using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ServiceLineTableSelectionRepository : IServiceLineTableSelectionRepository
    {
        private readonly Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLineTableSelectionRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ServiceLineTableSelectionRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseCommandObj.Dispose();
        }

        /// <summary>
        /// Get the Claimfields and Tables List
        /// </summary>
        /// <param name="contract">The contractId and contractServiceTypeId.</param>
        public List<ClaimField> GetClaimFieldAndTables(ContractServiceLineTableSelection contract)
        {
            List<ClaimField> claimfieldList = new List<ClaimField>();

            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetClaimFieldAndTableDetails");
            _databaseObj.AddInParameter(_databaseCommandObj, "@ContractID", DbType.Int64, contract.ContractId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceTypeID", DbType.Int64, contract.ContractServiceTypeId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@TableType", DbType.Int32, contract.TableType);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserText", DbType.String, contract.UserText);
            DataSet claimfieldDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            if (claimfieldDataSet.IsTableDataPopulated(0))
            {
                if (claimfieldDataSet.Tables[0].Rows != null && claimfieldDataSet.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < claimfieldDataSet.Tables[0].Rows.Count; i++)
                    {
                        ClaimField claimField = new ClaimField
                        {
                            ClaimFieldId = long.Parse(claimfieldDataSet.Tables[0].Rows[i]["ClaimFieldId"].ToString()),
                            ClaimFieldDocId = long.Parse(claimfieldDataSet.Tables[0].Rows[i]["ClaimFieldDocId"].ToString()),
                            TableName = Convert.ToString(claimfieldDataSet.Tables[0].Rows[i]["TableName"])
                        };
                        claimfieldList.Add(claimField);
                    }
                }
            }

            return claimfieldList;
        }

        /// <summary>
        /// Adds the new claim fields and Tables selection.
        /// </summary>
        /// <param name="serviceLineClaimandTableList">The claim field selection.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public long AddEditServiceLineClaimAndTables(List<ContractServiceLineTableSelection> serviceLineClaimandTableList)
        {
            if (serviceLineClaimandTableList != null && serviceLineClaimandTableList.Count > 0)
            {
                XmlSerializer serializer = new XmlSerializer(serviceLineClaimandTableList.GetType());
                StringWriter stringWriter = new StringWriter();
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
                XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
                XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                serializer.Serialize(xmlWriter, serviceLineClaimandTableList, emptyNs);
                string finalStrXml = stringWriter.ToString();
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("AddEditTableSelectionDetails");
                _databaseObj.AddInParameter(_databaseCommandObj, "@XmlServiceLineClaimandTable", DbType.String,
                                            finalStrXml);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceLineTableId", DbType.Int64, serviceLineClaimandTableList[0].ContractServiceLineId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractID", DbType.Int64, serviceLineClaimandTableList[0].ContractId == 0 ? null : serviceLineClaimandTableList[0].ContractId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceTypeID", DbType.Int64, serviceLineClaimandTableList[0].ContractServiceTypeId == 0 ? null : serviceLineClaimandTableList[0].ContractServiceTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName ", DbType.String, serviceLineClaimandTableList[0].UserName);
                // Retrieve the results of the Stored Procedure in Datatable
                return long.Parse(_databaseObj.ExecuteScalar(_databaseCommandObj).ToString());

              
            }
            
            return 0;
        }

        //To get values from database for editing puropse
        public List<ContractServiceLineTableSelection> GetServiceLineTableSelection(
            ContractServiceLineTableSelection contractServiceLineTableSelection)
        {
            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for

            _databaseObj.AddInParameter(_databaseCommandObj, "@ContractID", DbType.Int64, contractServiceLineTableSelection.ContractId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceTypeID", DbType.Int64, contractServiceLineTableSelection.ContractServiceTypeId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@ServiceLineTypeId", DbType.Int64, contractServiceLineTableSelection.ServiceLineTypeId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@PaymentTypeId", DbType.Int64, DBNull.Value);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, contractServiceLineTableSelection.UserName);

            // Retrieve the results of the Stored Procedure in DataSet
            DataSet serviceLineTableSelectionDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            if (serviceLineTableSelectionDataSet.IsTableDataPopulated())
            {
                List<ContractServiceLineTableSelection> tablesList = new List<ContractServiceLineTableSelection>();
                for (int i = 0; i < serviceLineTableSelectionDataSet.Tables[0].Rows.Count; i++)
                {
                    ContractServiceLineTableSelection contractTableSelection = new ContractServiceLineTableSelection
                    {
                        ClaimFieldId = long.Parse(serviceLineTableSelectionDataSet.Tables[0].Rows[i]["ClaimFieldId"].ToString()),
                        ClaimFieldDocId = long.Parse(serviceLineTableSelectionDataSet.Tables[0].Rows[i]["ClaimFieldDocId"].ToString()),
                        TableName = Convert.ToString(serviceLineTableSelectionDataSet.Tables[0].Rows[i]["TableName"]),
                        Text = Convert.ToString(serviceLineTableSelectionDataSet.Tables[0].Rows[i]["Text"]),
                        ContractServiceLineId = long.Parse(serviceLineTableSelectionDataSet.Tables[0].Rows[i]["ContractServiceLineId"].ToString()),
                        OperatorType = Convert.ToString(serviceLineTableSelectionDataSet.Tables[0].Rows[i]["OperatorType"]),
                        Operator = int.Parse(serviceLineTableSelectionDataSet.Tables[0].Rows[i]["Operator"].ToString())
                    };
                    tablesList.Add(contractTableSelection);
                }
                return tablesList;
            }
            //returns response to Business layer
            return null;
        }

        /// <summary>
        /// Gets the table selection claim field operators.
        /// </summary>
        /// <returns></returns>
        public List<ClaimFieldOperator> GetTableSelectionClaimFieldOperators()
        {
            List<ClaimFieldOperator> claimFieldList = new List<ClaimFieldOperator>();

            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetAllTableSelectionFieldOperators");
            DataSet claimFieldOperatorDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            if (claimFieldOperatorDataSet.IsTableDataPopulated(0))
            {
                for (int i = 0; i < claimFieldOperatorDataSet.Tables[0].Rows.Count; i++)
                {
                    ClaimFieldOperator claimField = new ClaimFieldOperator
                    {
                        OperatorId = long.Parse(claimFieldOperatorDataSet.Tables[0].Rows[i]["OperatorID"].ToString()),
                        OperatorType = Convert.ToString(claimFieldOperatorDataSet.Tables[0].Rows[i]["OperatorType"])
                    };
                    claimFieldList.Add(claimField);
                }
            }
            return claimFieldList;
        }
    }
}
