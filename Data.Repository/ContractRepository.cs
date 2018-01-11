/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Add/Modify/Rename and Copy Contract functionalities
/**  User Story Id  : 5.User Story Add a new contract 
 *                    6.User Story: Copy a contract.
 *                    7.User Story: View or Modify a contract
 *                    User Story: Rename a contract
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using SSI.ContractManagement.Shared.Data.Repository;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace SSI.ContractManagement.Data.Repository
{
    /// <summary>
    /// Handles Add/Modify/Rename and Copy Contract functionalities
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ContractRepository : BaseRepository, IContractRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the contract full information.
        /// </summary>
        /// <param name="contractInfo">The contract information.</param>
        /// <returns></returns>
        public ContractFullInfo GetContractFullInfo(Contract contractInfo)
        {
            //holds the response object
            ContractFullInfo contractFullInfo = new ContractFullInfo();
            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetContractFullInfo");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@NodeID", DbType.Int64, contractInfo.ContractId);
            _db.AddInParameter(_cmd, "@FacilityId", DbType.Int32, contractInfo.FacilityId);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, contractInfo.UserName);

            // Retrieve the results of the Stored Procedure in Dataset
            DataSet contractDataSet = _db.ExecuteDataSet(_cmd);
            //Check if output result tables exists or not
            if (contractDataSet != null && contractDataSet.Tables.Count > 0)
            {
                //populating ContractBasicInfo data
                if (contractDataSet.Tables[0].Rows != null && contractDataSet.Tables[0] != null && contractDataSet.Tables[0].Rows.Count > 0)
                {
                    contractFullInfo.ContractBasicInfo = new Contract
                    {
                        ContractId =
                            Convert.ToInt64(
                                contractDataSet.Tables[0].Rows[0]["ContractId"]),
                        InsertDate =
                            DBNull.Value ==
                            contractDataSet.Tables[0].Rows[0]["InsertDate"]
                                ? (DateTime?)null
                                : Convert.ToDateTime(
                                    contractDataSet.Tables[0].Rows[0]["InsertDate"]),
                        UpdateDate =
                            DBNull.Value ==
                            contractDataSet.Tables[0].Rows[0]["UpdateDate"]
                                ? (DateTime?)null
                                : Convert.ToDateTime(
                                    contractDataSet.Tables[0].Rows[0]["UpdateDate"]),
                        ContractName =
                            Convert.ToString(
                                contractDataSet.Tables[0].Rows[0]["ContractName"]),
                        StartDate =
                            Convert.ToDateTime(
                                contractDataSet.Tables[0].Rows[0]["StartDate"]),
                        EndDate =
                            Convert.ToDateTime(
                                contractDataSet.Tables[0].Rows[0]["EndDate"]),
                        FacilityId =
                             Convert.ToInt32(
                                    contractDataSet.Tables[0].Rows[0]["FacilityId"]),
                        Status =
                            DBNull.Value != contractDataSet.Tables[0].Rows[0]["Status"] &&
                            Convert.ToInt32(contractDataSet.Tables[0].Rows[0]["Status"]) ==
                            1,
                        IsClaimStartDate =
                       DBNull.Value != contractDataSet.Tables[0].Rows[0]["IsClaimStartDate"] &&
                       Convert.ToInt32(contractDataSet.Tables[0].Rows[0]["IsClaimStartDate"]) ==
                       1,
                        IsContractServiceTypeFound =
                       DBNull.Value != contractDataSet.Tables[0].Rows[0]["IsContractServiceTypeFound"] &&
                       Convert.ToInt32(contractDataSet.Tables[0].Rows[0]["IsContractServiceTypeFound"]) ==
                       1,
                        IsProfessional =
                            DBNull.Value != contractDataSet.Tables[0].Rows[0]["IsProfessionalClaim"] &&
                            Convert.ToInt32(contractDataSet.Tables[0].Rows[0]["IsProfessionalClaim"]) ==
                            1,
                        IsInstitutional =
                      DBNull.Value != contractDataSet.Tables[0].Rows[0]["IsInstitutionalClaim"] &&
                      Convert.ToInt32(contractDataSet.Tables[0].Rows[0]["IsInstitutionalClaim"]) ==
                      1,
                        NodeId =
                            DBNull.Value == contractDataSet.Tables[0].Rows[0]["NodeId"]
                                ? (long?)null
                                : Convert.ToInt64(
                                    contractDataSet.Tables[0].Rows[0]["NodeId"]),
                        IsModified =
                            DBNull.Value ==
                            contractDataSet.Tables[0].Rows[0]["IsModified"]
                                ? (int?)null
                                : Convert.ToInt32(
                                    contractDataSet.Tables[0].Rows[0]["IsModified"]),
                        ThresholdDaysToExpireAlters =
                            DBNull.Value ==
                            contractDataSet.Tables[0].Rows[0]["ThresholdDaysToExpireAlters"]
                                ? (int?)null
                                : Convert.ToInt32(
                                    contractDataSet.Tables[0].Rows[0]["ThresholdDaysToExpireAlters"]),
                        PayerCode = Convert.ToString(
                    contractDataSet.Tables[0].Rows[0]["PayerCode"]),
                        CustomField = DBNull.Value ==
                                contractDataSet.Tables[0].Rows[0]["CustomField"]
                                    ? (int?)null
                                    : Convert.ToInt32(
                                        contractDataSet.Tables[0].Rows[0]["CustomField"]),
                    };

                }

                //Populating ContractPayer's data
                if (contractDataSet.Tables[1].Rows != null && contractDataSet.Tables[1] != null && contractDataSet.Tables[1].Rows.Count > 0)
                {
                    if (contractInfo.ContractId == 0)
                        contractFullInfo.ContractBasicInfo = new Contract();
                    contractFullInfo.ContractBasicInfo.Payers = new List<Payer>();
                    for (int i = 0; i < contractDataSet.Tables[1].Rows.Count; i++)
                    {
                        Payer payer = new Payer
                        {
                            PayerName = Convert.ToString(contractDataSet.Tables[1].Rows[i]["PayerName"]),
                            IsSelected = Convert.ToBoolean(contractDataSet.Tables[1].Rows[i]["IsSelected"]),
                        };
                        contractFullInfo.ContractBasicInfo.Payers.Add(payer);
                    }
                }

                //populating Contract payer Info ID's data
                contractFullInfo.ContractContactIds = new List<Int64>();
                if (contractDataSet.Tables[2].Rows != null && contractDataSet.Tables[2] != null && contractDataSet.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < contractDataSet.Tables[2].Rows.Count; i++)
                    {
                        Int64 contractPayerInfoId = Convert.ToInt64(contractDataSet.Tables[2].Rows[i]["ContractPayerInfoId"]);
                        contractFullInfo.ContractContactIds.Add(contractPayerInfoId);
                    }
                }

                //populating ContractNotes data
                if (contractDataSet.Tables[3].Rows != null && contractDataSet.Tables[3] != null && contractDataSet.Tables[3].Rows.Count > 0)
                {
                    contractFullInfo.ContractNotes = new List<ContractNote>();
                    for (int i = 0; i < contractDataSet.Tables[3].Rows.Count; i++)
                    {
                        string currentDateTime = Utilities.GetLocalTimeString(contractInfo.CurrentDateTime,
                            Convert.ToDateTime((contractDataSet.Tables[3].Rows[i]["InsertDate"].ToString())));
                        ContractNote contractNote = new ContractNote
                        {
                            ContractNoteId =
                                Convert.ToInt64(
                                    contractDataSet.Tables[3].Rows[i]["ContractNoteID"]),
                            ContractId =
                                DBNull.Value == contractDataSet.Tables[3].Rows[i]["ContractID"]
                                    ? (long?)null
                                    : Convert.ToInt64(
                                        contractDataSet.Tables[3].Rows[i]["ContractID"]),
                            InsertDate =
                                DBNull.Value == contractDataSet.Tables[3].Rows[i]["InsertDate"]
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(
                                        contractDataSet.Tables[3].Rows[i]["InsertDate"]),
                            ShortDateTime = DBNull.Value == contractDataSet.Tables[3].Rows[i]["InsertDate"]
                                    ? string.Empty
                                    : Convert.ToDateTime(
                                        currentDateTime).ToString("MM/dd/yyyy hh:mm:ss"),
                            UpdateDate =
                                DBNull.Value == contractDataSet.Tables[3].Rows[i]["UpdateDate"]
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(
                                        contractDataSet.Tables[3].Rows[i]["UpdateDate"]),
                            NoteText =
                                Convert.ToString(contractDataSet.Tables[3].Rows[i]["NoteText"]),
                            UserName = DBNull.Value == contractDataSet.Tables[3].Rows[i]["UserName"] ? string.Empty : Convert.ToString(contractDataSet.Tables[3].Rows[i]["UserName"])
                        };
                        contractFullInfo.ContractNotes.Add(contractNote);
                    }
                }

                //populating ContractDocs data
                if (contractDataSet.Tables[4].Rows != null && contractDataSet.Tables[4] != null && contractDataSet.Tables[4].Rows.Count > 0)
                {
                    contractFullInfo.ContractDocs = new List<ContractDoc>();
                    for (int i = 0; i < contractDataSet.Tables[4].Rows.Count; i++)
                    {
                        ContractDoc contractDocs = new ContractDoc
                        {
                            ContractDocId =
                                Convert.ToInt64(
                                    contractDataSet.Tables[4].Rows[i]["ContractDocID"]),
                            InsertDate =
                                DBNull.Value == contractDataSet.Tables[4].Rows[i]["InsertDate"]
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(
                                        contractDataSet.Tables[4].Rows[i]["InsertDate"]),
                            UpdateDate =
                                DBNull.Value == contractDataSet.Tables[4].Rows[i]["UpdateDate"]
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(
                                        contractDataSet.Tables[4].Rows[i]["UpdateDate"]),
                            ContractId =
                                DBNull.Value == contractDataSet.Tables[4].Rows[i]["ContractID"]
                                    ? (long?)null
                                    : Convert.ToInt64(
                                        contractDataSet.Tables[4].Rows[i]["ContractID"]),
                            ContractContent =
                                DBNull.Value ==
                                contractDataSet.Tables[4].Rows[i]["ContractContent"]
                                    ? null
                                    : (byte[])contractDataSet.Tables[4].Rows[i]["ContractContent"],
                            FileName =
                                Convert.ToString(contractDataSet.Tables[4].Rows[i]["FileName"])
                        };

                        contractFullInfo.ContractDocs.Add(contractDocs);
                    }
                }
            }
            //returns response to Business layer
            return contractFullInfo;
        }

        /// <summary>
        /// Copy contract and contract hierarchy
        /// </summary>
        /// <param name="contracts"></param>
        /// <returns></returns>
        public long CopyContract(Contract contracts)
        {
            long contractId;
            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("CopyContractById");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@ContractID", DbType.String, contracts.ContractId);
            // Retrieve the results of the Stored Procedure in Dataset
            contractId = long.TryParse(_db.ExecuteScalar(_cmd).ToString(), out contractId) ? contractId : 0;
            //returns response to Business layer
            return contractId;
        }

        /// <summary>
        /// Rename Contract Name
        /// </summary>
        /// <param name="nodeId"> </param>
        /// <param name="nodeText"> </param>
        /// <param name="userName"></param>
        /// <returns>return value</returns>
        public ContractHierarchy RenameContract(long nodeId, string nodeText, string userName)
        {

            long contractId;
            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("RenameContract");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@NodeId", DbType.Int64, nodeId);
            _db.AddInParameter(_cmd, "@NodeText", DbType.String, nodeText.ToTrim());
            _db.AddInParameter(_cmd, "@UserName", DbType.String, userName);
            // Retrieve the results of the Stored Procedure in Dataset
            long returnValue = long.TryParse(_db.ExecuteScalar(_cmd).ToString(), out contractId) ? contractId : 0;
            if (returnValue > 0)
            {
                return new ContractHierarchy
                {
                    NodeText = nodeText,
                    NodeId = nodeId
                };
            }

            return null;
        }

        /// <summary>
        /// Add Contract Modified Reason
        /// </summary>
        /// <param name="reason"> </param>
        /// <returns>updatedRow</returns>
        public int AddContractModifiedReason(ContractModifiedReason reason)
        {
            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("AddEditContractModifiedReason");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@ContractReasonCodeID", DbType.Int64, reason.ReasonCode);
            _db.AddInParameter(_cmd, "@ContractID", DbType.String, reason.ContractId);
            _db.AddInParameter(_cmd, "@Text", DbType.String, reason.Notes.ToTrim());
            // Retrieve the results of the Stored Procedure in Dataset
            return _db.ExecuteNonQuery(_cmd);

        }

        /// <summary>
        /// Adds the edit contract basic information.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        public Contract AddEditContractBasicInfo(Contract contract)
        {
            Contract contractinfo = new Contract();
            //Checks if input request is not null
            if (contract != null)
            {
                //  contract.FillSecurityData();
                //holds the response
                string finalStrXml = string.Empty;
                //Checks for Payers, if payers exists stores it in DB
                if (contract.Payers != null && contract.Payers.Count > 0)
                {
                    //Serializes list of payers to store it in DB
                    XmlSerializer serializer = new XmlSerializer(contract.Payers.GetType());
                    StringWriter stringWriter = new StringWriter();
                    XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
                    XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
                    XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    serializer.Serialize(xmlWriter, contract.Payers, emptyNs);
                    //holds the final payer response
                    finalStrXml = stringWriter.ToString();
                }

                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("AddEditContracts");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, contract.ContractId);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, contract.UserName.ToTrim());
                _db.AddInParameter(_cmd, "@ContractName", DbType.String, contract.ContractName.ToTrim());
                _db.AddInParameter(_cmd, "@StartDate", DbType.DateTime, contract.StartDate);
                _db.AddInParameter(_cmd, "@EndDate", DbType.DateTime, contract.EndDate);
                _db.AddInParameter(_cmd, "@FacilityID", DbType.Int64, contract.FacilityId);
                _db.AddInParameter(_cmd, "@Status", DbType.Int32, contract.Status);
                _db.AddInParameter(_cmd, "@IsClaimStartDate", DbType.Int32, contract.IsClaimStartDate);
                _db.AddInParameter(_cmd, "@IsProfessionalClaim", DbType.Int32, contract.IsProfessional);
                _db.AddInParameter(_cmd, "@IsInstitutionalClaim", DbType.Int32, contract.IsInstitutional);
                _db.AddInParameter(_cmd, "@NodeId", DbType.Int64, contract.NodeId);
                _db.AddInParameter(_cmd, "@ParentId", DbType.Int64, contract.ParentId);
                _db.AddInParameter(_cmd, "@IsModified", DbType.Int32, contract.IsModified);
                _db.AddInParameter(_cmd, "@XmlPayerData", DbType.String, finalStrXml);
                _db.AddInParameter(_cmd, "@AlertThreshold", DbType.Int32, contract.ThresholdDaysToExpireAlters);
                _db.AddInParameter(_cmd, "@PayerCode", DbType.String, contract.PayerCode);
                _db.AddInParameter(_cmd, "@CustomField", DbType.Int32, contract.CustomField);

                DataSet contractDataSet = _db.ExecuteDataSet(_cmd);
                //Check if output result tables exists or not
                // ds.Tables[0].Rows[0]["ContractID"] is not equal to DBNull then nodeID will also be present there. So no need to put one extra condition
                if (contractDataSet.IsTableDataPopulated() && contractDataSet.Tables[0].Rows[0]["ContractID"] != DBNull.Value)
                {
                    contractinfo.ContractId = Convert.ToInt64(contractDataSet.Tables[0].Rows[0]["ContractID"]);
                    contractinfo.NodeId = Convert.ToInt64(contractDataSet.Tables[0].Rows[0]["NodeID"]);
                }

                //returns response to Business layer
            }
            return contractinfo;
        }

        /// <summary>
        ///  Get Contract First Level Details
        /// </summary>
        /// <returns>contractId</returns>
        public Contract GetContractFirstLevelDetails(long? contractid)
        {
            Contract contract = new Contract();
            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetContractFirstLevelDetails");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, contractid);
            DataSet contractDataSet = _db.ExecuteDataSet(_cmd);
            if (contractDataSet.IsTableDataPopulated(0))
            {
                contract.NodeId = (long?)contractDataSet.Tables[0].Rows[0]["NodeId"];
                contract.ParentId = (long?)contractDataSet.Tables[0].Rows[0]["ParentId"];
                contract.FacilityId = (int)contractDataSet.Tables[0].Rows[0]["FacilityId"];//TODO Janaki
                contract.UserName = (string)contractDataSet.Tables[0].Rows[0]["UserName"];
            }
            return contract;
        }


        /// <summary>
        /// Gets the contracts.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="isMedicareIpAcuteEnabled">if set to <c>true</c> [is medicare ip acute enabled].</param>
        /// <param name="isMedicareOpApcEnabled">if set to <c>true</c> [is medicare op apc enabled].</param>
        /// <returns></returns>
        public List<Contract> GetContracts(long taskId, bool isMedicareIpAcuteEnabled, bool isMedicareOpApcEnabled)
        {
            List<Contract> contracts = new List<Contract>();
            if (taskId > 0)
            {
                // Initialize the Stored Procedure
                _cmd = _db.GetStoredProcCommand("GetContractsByTaskID");
                _cmd.CommandTimeout = 7200;
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@TaskId", DbType.Int64, taskId);
                _db.AddInParameter(_cmd, "@IsMedicareIpAcuteEnabled", DbType.Boolean, isMedicareIpAcuteEnabled);
                _db.AddInParameter(_cmd, "@IsMedicareOpApcEnabled", DbType.Boolean, isMedicareOpApcEnabled);
                // Retrieve the results of the Stored Procedure in DataSet
                DataSet contractDataSet = _db.ExecuteDataSet(_cmd);
                contracts = GetContracts(contractDataSet);
            }
            return contracts;
        }

        /// <summary>
        /// Gets the contracts.
        /// </summary>
        /// <param name="contractDataSet">The data set.</param>
        /// <returns></returns>
        public static List<Contract> GetContracts(DataSet contractDataSet)
        {
            return (from DataRow row in contractDataSet.Tables[0].Rows
                    select new Contract
                    {
                        ContractId = GetValue<long>(row["ContractID"], typeof(long)),
                        ContractName = GetStringValue(row["ContractName"]),
                        StartDate = GetValue<DateTime?>(row["StartDate"], typeof(DateTime)),
                        EndDate = GetValue<DateTime?>(row["EndDate"], typeof(DateTime)),
                        FacilityId = GetValue<int>(row["FacilityID"], typeof(int)),
                        IsProfessional = GetValue<bool>(row["IsProfessionalClaim"], typeof(bool)),
                        IsInstitutional =
                            DBNull.Value != row["IsInstitutionalClaim"] &&
                            Convert.ToInt32(row["IsInstitutionalClaim"]) == 1,
                        IsClaimStartDate =
                            DBNull.Value != row["IsClaimStartDate"] && Convert.ToInt32(row["IsClaimStartDate"]) == 1,
                        ContractServiceTypes =
                            ContractServiceTypeRepository.GetContractServiceTypes(GetValue<long>(row["ContractID"], typeof(long)), contractDataSet),
                        Payers = GetPayers(GetValue<long>(row["ContractID"], typeof(long)), contractDataSet.Tables[1]),
                        Conditions = GetSelectionConditions(GetValue<long>(row["ContractID"], typeof(long)), null, contractDataSet),
                        PaymentTypes = GetContractPaymentFilters(GetValue<long>(row["ContractID"], typeof(long)),
                            contractDataSet),
                        PayerCode = GetStringValue(row["PayerCode"]),
                        CustomField = DBNull.Value != row["CustomField"] ? Convert.ToInt32(row["CustomField"]) : (int?)null
                    }).ToList();
        }

        /// <summary>
        /// Gets the contract payment filters.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="contractDataSet">The data set.</param>
        /// <returns></returns>
        private static List<PaymentTypeBase> GetContractPaymentFilters(long contractId, DataSet contractDataSet)
        {
            List<PaymentTypeBase> paymentTypes = new List<PaymentTypeBase>();

            //Add Stop loss Payment Type - use table 16
            if (contractDataSet.Tables[16] != null &&
                  contractDataSet.Tables[16].Rows != null && contractDataSet.Tables[16].Rows.Count > 0 &&
                  contractDataSet.Tables[16].Rows.Cast<DataRow>().Any(row => row["ContractId"] != DBNull.Value &&
                                                                  Convert.ToInt64(row["ContractId"]) ==
                                                                  contractId))
                paymentTypes.Add(PaymentTypeStopLossRepository.GetPaymentType(contractId, null, contractDataSet.Tables[16]));

            //Add Cap Payment Type - use table 7
            if (contractDataSet.Tables[7] != null &&
                  contractDataSet.Tables[7].Rows != null && contractDataSet.Tables[7].Rows.Count > 0 &&
                  contractDataSet.Tables[7].Rows.Cast<DataRow>().Any(row => row["ContractId"] != DBNull.Value &&
                                                                  Convert.ToInt64(row["ContractId"]) ==
                                                                  contractId))
                paymentTypes.Add(PaymentTypeCapRepository.GetPaymentType(contractId, null, contractDataSet.Tables[7]));

            //Add Lesser Of Payment Type
            if (contractDataSet.Tables[20] != null &&
                  contractDataSet.Tables[20].Rows != null && contractDataSet.Tables[20].Rows.Count > 0 &&
                  contractDataSet.Tables[20].Rows.Cast<DataRow>().Any(row => row["ContractId"] != DBNull.Value &&
                                                                  Convert.ToInt64(row["ContractId"]) ==
                                                                  contractId))
                paymentTypes.Add(PaymentTypeLesserOfRepository.GetPaymentType(contractId, null, contractDataSet.Tables[20]));

            //Add Medicare Sequester Payment Type
            if (contractDataSet.Tables[23] != null &&
                  contractDataSet.Tables[23].Rows != null && contractDataSet.Tables[23].Rows.Count > 0 &&
                  contractDataSet.Tables[23].Rows.Cast<DataRow>().Any(row => row["ContractId"] != DBNull.Value &&
                                                                  Convert.ToInt64(row["ContractId"]) ==
                                                                  contractId))
                paymentTypes.Add(PaymentTypeMedicareSequesterRepository.GetPaymentType(contractId, null, contractDataSet.Tables[23]));

            return paymentTypes;
        }

        /// <summary>
        /// Gets the payer list.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="contractDataTable">The data table.</param>
        /// <returns></returns>
        private static List<Payer> GetPayers(long contractId, DataTable contractDataTable)
        {
            List<Payer> payers = new List<Payer>();

            if (contractDataTable != null && contractDataTable.Rows.Count > 0)
            {
                payers = (from DataRow row in contractDataTable.Rows
                          where Convert.ToInt64(row["ContractID"]) == contractId
                          select new Payer
                          {
                              ContractId = DBNull.Value == row["ContractId"] ? (long?)null : Convert.ToInt64(row["ContractId"]),
                              PayerName = DBNull.Value == row["PayerName"] ? null : Convert.ToString(row["PayerName"])
                          }).ToList();
            }

            return payers;
        }

        /// <summary>
        /// Checks the contract name is unique.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        public bool IsContractNameExist(Contract contract)
        {
            if (contract != null)
            {
                _cmd = _db.GetStoredProcCommand("IsContractNameExist");
                _db.AddInParameter(_cmd, "@NodeText", DbType.String, contract.ContractName.ToTrim());
                _db.AddInParameter(_cmd, "@NodeId", DbType.Int64, contract.NodeId);
                _db.AddInParameter(_cmd, "@StartDate", DbType.DateTime, contract.StartDate);
                _db.AddInParameter(_cmd, "@EndDate", DbType.DateTime, contract.EndDate);
                _db.AddInParameter(_cmd, "@ContractId", DbType.Int64, contract.ContractId);

                return Convert.ToInt32(_db.ExecuteScalar(_cmd)) == 0;
            }
            return false;
        }


        /// <summary>
        /// Gets the contracts.
        /// </summary>
        /// <param name="contractDataTable">The data table.</param>
        /// <returns></returns>
        public static IEnumerable<Contract> GetContracts(DataTable contractDataTable)
        {
            return (from DataRow row in contractDataTable.Rows
                    select new Contract
                    {
                        ContractId = Convert.ToInt64(row["ContractID"]),
                        ContractName = Convert.ToString(row["ContractName"]),
                        StartDate = Convert.ToDateTime(row["StartDate"]),
                        EndDate = Convert.ToDateTime(row["EndDate"]),
                        FacilityId = Convert.ToInt32(row["FacilityID"]),
                        IsProfessional = Convert.ToBoolean(row["IsProfessionalClaim"]),
                        IsInstitutional =
                            DBNull.Value != row["IsInstitutionalClaim"] &&
                            Convert.ToInt32(row["IsInstitutionalClaim"]) == 1,
                        IsClaimStartDate =
                            DBNull.Value != row["IsClaimStartDate"] && Convert.ToInt32(row["IsClaimStartDate"]) == 1,
                        Conditions = new List<ICondition>(),
                    }).ToList();
        }



        /// <summary>
        /// Gets the adjudicated contract names.
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        //Fixed-2016-R3-S3 : Rename @ModelId to @ModelID
        public List<Contract> GetAdjudicatedContracts(Contract contract)
        {
            List<Contract> availableContractNames = new List<Contract>();
            _cmd = _db.GetStoredProcCommand("GetAdjudicatedContractNames");
            _db.AddInParameter(_cmd, "@ModelID", DbType.Int64, contract.ModelId);
            _db.AddInParameter(_cmd, "@UserText", DbType.String, contract.Values);
            DataSet contractNameDataSet = _db.ExecuteDataSet(_cmd);
            if (contractNameDataSet != null && contractNameDataSet.Tables.Count > 0)
            {
                for (int i = 0; i < contractNameDataSet.Tables[0].Rows.Count; i++)
                {
                    availableContractNames.Add(new Contract
                    {
                        ContractId = Convert.ToInt64(contractNameDataSet.Tables[0].Rows[i]["ContractID"]),
                        ContractName = Convert.ToString(contractNameDataSet.Tables[0].Rows[i]["ContractStartEndDate"])
                    });
                }
            }
            return availableContractNames;
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
