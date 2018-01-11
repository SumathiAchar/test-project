using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Helpers.DataAccess;


namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ServiceLineClaimFieldRepository : IServiceLineClaimFieldRepository
    {
        private Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLineClaimFieldRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ServiceLineClaimFieldRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Adds the new claim field selection.
        /// </summary>
        /// <param name="claimFieldSelection">The claim field selection.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public long AddNewClaimFieldSelection(List<ContractServiceLineClaimFieldSelection> claimFieldSelection)
        {
            if (claimFieldSelection != null && claimFieldSelection.Count > 0)
            {
                XmlSerializer serializer = new XmlSerializer(claimFieldSelection.GetType());
                StringWriter stringWriter = new StringWriter();
                //new XmlTextWriter(stringWriter);
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
                XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
                XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                serializer.Serialize(xmlWriter, claimFieldSelection, emptyNs);
                string finalStrXml = stringWriter.ToString();
                _databaseCommandObj =
                    _databaseObj.GetStoredProcCommand("AddServiceLineClaimFields");
                _databaseObj.AddInParameter(_databaseCommandObj, "@XmlServiceLineClaimFieldSelection", DbType.String,
                                            finalStrXml);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceTypeId", DbType.Int64,
                                            claimFieldSelection[0].ContractServiceTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractId", DbType.Int64,
                                            claimFieldSelection[0].ContractId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ServiceLineTypeId", DbType.Int64,
                                            claimFieldSelection[0].ServiceLineTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, claimFieldSelection[0].UserName);
                return long.Parse(_databaseObj.ExecuteScalar(_databaseCommandObj).ToString());

               
            }
            return 0;
        }

        /// <summary>
        /// Edit the new claim field selection.
        /// </summary>
        /// <param name="claimFieldSelection">The claim field selection.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public long EditClaimFieldSelection(List<ContractServiceLineClaimFieldSelection> claimFieldSelection)
        {
            if (claimFieldSelection != null && claimFieldSelection.Count > 0)
            {
                XmlSerializer serializer = new XmlSerializer(claimFieldSelection.GetType());
                StringWriter stringWriter = new StringWriter();
                //new XmlTextWriter(stringWriter);
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
                XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
                XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                serializer.Serialize(xmlWriter, claimFieldSelection, emptyNs);
                string finalStrXml = stringWriter.ToString();
                _databaseCommandObj =
                    _databaseObj.GetStoredProcCommand("UpdateClaimFieldSelectionServiceLine");
                _databaseObj.AddInParameter(_databaseCommandObj, "@XmlServiceLineClaimFieldSelection", DbType.String,
                                            finalStrXml);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceTypeId", DbType.Int64,
                    claimFieldSelection[0].ContractServiceTypeId == 0 ? null : claimFieldSelection[0].ContractServiceTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractId", DbType.Int64,
                    claimFieldSelection[0].ContractId == 0 ? null : claimFieldSelection[0].ContractId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ServiceLineTypeId", DbType.Int64,
                                            claimFieldSelection[0].ServiceLineTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String,
                                          claimFieldSelection[0].UserName);
               return long.Parse(_databaseObj.ExecuteScalar(_databaseCommandObj).ToString());

                
            }
            return 0;
        }

        /// <summary>
        /// Gets the new claim field selection.
        /// </summary>
        /// <param name="claimFieldSelection">The claim field selection.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<ContractServiceLineClaimFieldSelection> GetClaimFieldSelection(ContractServiceLineClaimFieldSelection claimFieldSelection)
        {
            if (claimFieldSelection != null)
            {

                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@PaymentTypeID ", DbType.Int64, 0);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractID", DbType.Int64, claimFieldSelection.ContractId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceTypeID", DbType.Int64, claimFieldSelection.ContractServiceTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ServiceLineTypeId", DbType.Int64, claimFieldSelection.ServiceLineTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, claimFieldSelection.UserName);
                DataSet claimFieldSelectionDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);
                if (claimFieldSelectionDataSet.IsTableDataPopulated(0))
                {
                    List<ContractServiceLineClaimFieldSelection> serviceLineClaimFieldSelection = new List<ContractServiceLineClaimFieldSelection>();
                    for (int i = 0; i < claimFieldSelectionDataSet.Tables[0].Rows.Count; i++)
                    {
                        ContractServiceLineClaimFieldSelection claimSelection = new ContractServiceLineClaimFieldSelection
                        {
                            ClaimFieldId = Convert.ToInt64(claimFieldSelectionDataSet.Tables[0].Rows[i]["ClaimFieldID"].ToString()),
                            Operator = Convert.ToInt32(claimFieldSelectionDataSet.Tables[0].Rows[i]["OperatorID"].ToString()),
                            ClaimField = claimFieldSelectionDataSet.Tables[0].Rows[i]["ClaimField"].ToString(),
                            OperatorType = claimFieldSelectionDataSet.Tables[0].Rows[i]["OperatorType"].ToString(),
                            Values = claimFieldSelectionDataSet.Tables[0].Rows[i]["Values"].ToString(),
                        };
                        serviceLineClaimFieldSelection.Add(claimSelection);
                    }
                    return serviceLineClaimFieldSelection;
                }
            }
            return null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseCommandObj.Dispose();
            _databaseObj = null;
        }
    }
}