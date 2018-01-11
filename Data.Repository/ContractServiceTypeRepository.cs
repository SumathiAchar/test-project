using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ContractServiceTypeRepository : BaseRepository, IContractServiceTypeRepository
    {
        private Database _db;
        private DbCommand _cmd;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractServiceTypeRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractServiceTypeRepository(string connectionString)
        {
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }


                /// <summary>
        /// For getting contract service type information based on contractId
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="serviceTypeDataSet"></param>
        /// <returns>list of contract service types</returns>
        public static List<ContractServiceType> GetContractServiceTypes(long contractId, DataSet serviceTypeDataSet)
        {
            List<ContractServiceType> contractServiceTypes = new List<ContractServiceType>();

            if (serviceTypeDataSet.Tables[2] != null && serviceTypeDataSet.Tables[2].Rows.Count > 0)
            {
                contractServiceTypes = (from DataRow row in serviceTypeDataSet.Tables[2].Rows
                                        where (Convert.ToInt64(row["ContractID"]) == contractId)
                                        select new ContractServiceType
                                        {
                                            ContractServiceTypeId = Convert.ToInt64(row["ContractServiceTypeID"]),
                                            ContractServiceTypeName = Convert.ToString(row["ContractServiceTypeName"]),
                                            Notes = Convert.ToString(row["Notes"]),
                                            IsCarveOut = DBNull.Value != row["IsCarveOut"] && Convert.ToInt32(row["IsCarveOut"]) == 1,
                                            ContractId = Convert.ToInt64(row["ContractID"]),
                                            Conditions =
                                                GetSelectionConditions(null, Convert.ToInt64(row["ContractServiceTypeID"]), serviceTypeDataSet)
                                        }).ToList();

                foreach (var servicetype in contractServiceTypes)
                {
                    servicetype.PaymentTypes = GetPaymentTypes(servicetype.ContractServiceTypeId,
                        serviceTypeDataSet, servicetype.Conditions);
                }
            }

            return contractServiceTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceTypeDataTable"></param>
        /// <param name="contractServiceTypeId"></param>
        /// <returns></returns>
        static bool IsValidPayment(DataTable serviceTypeDataTable, long contractServiceTypeId)
        {
            bool isValid = serviceTypeDataTable != null &&
                           serviceTypeDataTable.Rows != null && serviceTypeDataTable.Rows.Count > 0 &&
                           serviceTypeDataTable.Rows.Cast<DataRow>().Any(row => row["contractServiceTypeId"] != DBNull.Value &&
                                                                  Convert.ToInt64(row["contractServiceTypeId"]) ==
                                                                  contractServiceTypeId);
            return isValid;

        }
        /// <summary>
        /// Gets the payment types.
        /// </summary>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="serviceTypeDataSet">The data set.</param>
        /// <param name="serviceTypeConditions"></param>
        /// <returns></returns>
        private static List<PaymentTypeBase> GetPaymentTypes(long contractServiceTypeId, DataSet serviceTypeDataSet, List<ICondition> serviceTypeConditions)
        {
            List<PaymentTypeBase> paymentTypes = new List<PaymentTypeBase>();

            //Add FeeSchedule Payment Type - use table 9
            if (IsValidPayment(serviceTypeDataSet.Tables[9], contractServiceTypeId))
                paymentTypes.Add(PaymentTypeFeeScheduleRepository.GetPaymentType(contractServiceTypeId, serviceTypeDataSet.Tables[9],
                    serviceTypeDataSet.Tables[17], serviceTypeDataSet.Tables[18]));

            //Add ASCFeeSchedule Payment Type
            if (IsValidPayment(serviceTypeDataSet.Tables[6], contractServiceTypeId))
                paymentTypes.Add(PaymentTypeAscFeeScheduleRepository.GetPaymentType(contractServiceTypeId, serviceTypeDataSet.Tables[6],
                    serviceTypeDataSet.Tables[17], serviceTypeDataSet.Tables[18]));

            //Add PerCase Payment Type
            if (IsValidPayment(serviceTypeDataSet.Tables[12], contractServiceTypeId))
                paymentTypes.Add(PaymentTypePerCaseRepository.GetPaymentType(contractServiceTypeId, serviceTypeDataSet.Tables[12], serviceTypeConditions));

            //Add custom payment table
            if (IsValidPayment(serviceTypeDataSet.Tables[22], contractServiceTypeId))
                paymentTypes.Add(PaymentTypeCustomTableRepository.GetPaymentType(contractServiceTypeId, serviceTypeDataSet.Tables[22],
                    serviceTypeDataSet.Tables[17], serviceTypeDataSet.Tables[18]));

            //Add Per Diem Payment Type - use table 14
            if (IsValidPayment(serviceTypeDataSet.Tables[14], contractServiceTypeId))
                paymentTypes.Add(PaymentTypePerDiemRepository.GetPaymentType(contractServiceTypeId, serviceTypeDataSet.Tables[14]));

            //Add DRG Schedules Type - use table 8
            if (IsValidPayment(serviceTypeDataSet.Tables[8], contractServiceTypeId))
                paymentTypes.Add(PaymentTypeDrgRepository.GetPaymentType(contractServiceTypeId, serviceTypeDataSet.Tables[8], serviceTypeDataSet.Tables[17], serviceTypeDataSet.Tables[18]));

            //Add Percentage Payment Type - use table 13
            if (IsValidPayment(serviceTypeDataSet.Tables[13], contractServiceTypeId))
                paymentTypes.Add(PaymentTypePercentageChargeRepository.GetPaymentType(contractServiceTypeId, serviceTypeDataSet.Tables[13]));

            //Add Payment Type Per Visit - use table 15
            if (IsValidPayment(serviceTypeDataSet.Tables[15], contractServiceTypeId))
                paymentTypes.Add(PaymentTypePerVisitRepository.GetPaymentType(contractServiceTypeId, serviceTypeDataSet.Tables[15]));

            //Add Payment Type Medicare Lab Fee Schedule - use table 19
            if (IsValidPayment(serviceTypeDataSet.Tables[19], contractServiceTypeId))
                paymentTypes.Add(PaymentTypeMedicareLabFeeScheduleRepository.GetPaymentType(contractServiceTypeId,
                    serviceTypeDataSet.Tables[19]));

            //Add Medicare IP Payment Type - use table 10
            if (IsValidPayment(serviceTypeDataSet.Tables[10], contractServiceTypeId))
                paymentTypes.Add(PaymentTypeMedicareIpRepository.GetPaymentType(contractServiceTypeId, serviceTypeDataSet.Tables[10]));

            //Add Medicare OP Payment Type - use table 11
            if (IsValidPayment(serviceTypeDataSet.Tables[11], contractServiceTypeId))
                paymentTypes.Add(PaymentTypeMedicareOPRepository.GetPaymentType(contractServiceTypeId, serviceTypeDataSet.Tables[11]));

            //Add Stop loss Payment Type - use table 16
            if (IsValidPayment(serviceTypeDataSet.Tables[16], contractServiceTypeId))
                paymentTypes.Add(PaymentTypeStopLossRepository.GetPaymentType(null, contractServiceTypeId, serviceTypeDataSet.Tables[16]));

            //Add Cap Payment Type - use table 7
            if (IsValidPayment(serviceTypeDataSet.Tables[7], contractServiceTypeId))
                paymentTypes.Add(PaymentTypeCapRepository.GetPaymentType(null, contractServiceTypeId, serviceTypeDataSet.Tables[7]));

            //Add Lesser OF/Greater Of.
            if (IsValidPayment(serviceTypeDataSet.Tables[20], contractServiceTypeId))
                paymentTypes.Add(PaymentTypeLesserOfRepository.GetPaymentType(null, contractServiceTypeId, serviceTypeDataSet.Tables[20]));

            GetBaseConditions(serviceTypeConditions, paymentTypes);
            return paymentTypes;
        }

        /// <summary>
        /// Returns all available Contract Service Type
        /// </summary>
        /// <param name="contractId">contractId</param>
        /// <returns>List of ContractServiceTypes object</returns>
        public List<ContractServiceType> GetAllContractServiceType(long contractId)
        {
            return GetAllContractServiceTypeList(contractId);
        }

        /// <summary>
        /// Gets all contract service type list.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <returns></returns>
        private List<ContractServiceType> GetAllContractServiceTypeList(long contractId)
        {
            List<ContractServiceType> contractServiceTypes = new List<ContractServiceType>();
            if (contractId != 0)
            {

                _cmd = _db.GetStoredProcCommand("GetAllContractServiceTypes");
                _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, contractId);

                DataTable serviceTypeDataTable = _db.ExecuteDataSet(_cmd).Tables[0];
                if (serviceTypeDataTable.Rows != null && serviceTypeDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < serviceTypeDataTable.Rows.Count; i++)
                    {
                        ContractServiceType contractServiceType = new ContractServiceType
                        {
                            ContractServiceTypeId =
                                long.Parse(
                                    serviceTypeDataTable.Rows[i]["ContractServiceTypeId"].
                                        ToString()),
                            ContractServiceTypeName =
                                Convert.ToString(
                                    serviceTypeDataTable.Rows[i]["ContractServiceTypeName"])
                        };
                        contractServiceTypes.Add(contractServiceType);
                    }
                }
            }
            return contractServiceTypes;
        }

        /// <summary>
        /// Adds the type of the edit contract service.
        /// </summary>
        /// <param name="contractServiceTypes">The contract service types.</param>
        /// <returns></returns>
        public long AddEditContractServiceType(ContractServiceType contractServiceTypes)
        {
            if (contractServiceTypes != null)
            {
                _cmd = _db.GetStoredProcCommand("AddEditContractServiceTypes");
                _db.AddInParameter(_cmd, "@ContractServiceTypeId", DbType.Int64, contractServiceTypes.ContractServiceTypeId);
                _db.AddInParameter(_cmd, "@ContractId", DbType.Int64, contractServiceTypes.ContractId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeName", DbType.String, contractServiceTypes.ContractServiceTypeName.ToTrim());
                _db.AddInParameter(_cmd, "@Notes", DbType.String, contractServiceTypes.Notes.ToTrim());
                _db.AddInParameter(_cmd, "@IsCarveOut", DbType.Int64, contractServiceTypes.IsCarveOut);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, contractServiceTypes.UserName);
                long returnValue = long.Parse(_db.ExecuteScalar(_cmd).ToString());
                return returnValue;
            }
            return 0;
        }

        /// <summary>
        /// Copies the Contract Service Type by Contract ServiceType id.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public long CopyContractServiceType(ContractServiceType data)
        {
            if (data != null)
            {
                long contractServiceTypeId;

                _cmd = _db.GetStoredProcCommand("CopyContractServiceTypeByID");
                _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, data.ContractServiceTypeId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeName", DbType.String, data.ContractServiceTypeName.ToTrim());
                _db.AddInParameter(_cmd, "@UserName", DbType.String, data.UserName);
                _cmd.CommandTimeout = data.CommandTimeoutForContractHierarchyCopyContractServiceTypeById;
                return long.TryParse(_db.ExecuteScalar(_cmd).ToString(), out contractServiceTypeId) ? contractServiceTypeId : 0;
            }
            return 0;
        }

        /// <summary>
        /// Rename the Contract Service Type by Contract ServiceType id.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public long RenameContractServiceType(ContractServiceType data)
        {
            long contractServiceTypeId;
            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("RenameContractServiceTypeByID");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, data.ContractServiceTypeId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeName", DbType.String, data.ContractServiceTypeName.ToTrim());
            _db.AddInParameter(_cmd, "@UserName", DbType.String, data.UserName);
            // Retrieve the results of the Stored Procedure in Dataset
            return long.TryParse(_db.ExecuteScalar(_cmd).ToString(), out contractServiceTypeId) ? contractServiceTypeId : 0;
        }

        /// <summary>
        /// Gets the contract service type details.
        /// </summary>
        /// <param name="contractServiceType">Type of the contract service.</param>
        /// <returns></returns>
        public ContractServiceType GetContractServiceTypeDetails(ContractServiceType contractServiceType)
        {

            _cmd = _db.GetStoredProcCommand("GetServiceTypeDetails");
            _db.AddInParameter(_cmd, "@ContractServiceTypeId", DbType.Int64, contractServiceType.ContractServiceTypeId);
            _db.AddInParameter(_cmd, "@ContractId", DbType.Int64, contractServiceType.ContractId);

            DataSet serviceTypeDataSet = _db.ExecuteDataSet(_cmd);
            if (serviceTypeDataSet != null && serviceTypeDataSet.Tables.Count > 0)
            {
                //populating ContractBasicInfo data
                if (serviceTypeDataSet.Tables[0].Rows != null && serviceTypeDataSet.Tables[0] != null && serviceTypeDataSet.Tables[0].Rows.Count > 0)
                {
                    ContractServiceType contractServiceTypeDetails = new ContractServiceType
                    {
                        ContractServiceTypeName = serviceTypeDataSet.Tables[0].Rows[0]["ContractServiceTypeName"].ToString(),
                        Notes = serviceTypeDataSet.Tables[0].Rows[0]["Notes"].ToString(),
                        IsCarveOut = serviceTypeDataSet.Tables[0].Rows[0]["IsCarveOut"] != DBNull.Value && Convert.ToBoolean(serviceTypeDataSet.Tables[0].Rows[0]["IsCarveOut"]),
                        ContractServiceTypeId = Convert.ToInt64(serviceTypeDataSet.Tables[0].Rows[0]["ContractServiceTypeId"].ToString())
                    };
                    return contractServiceTypeDetails;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks the contract service type name is unique.
        /// </summary>
        /// <param name="contractServiceTypes">The contract service types.</param>
        /// <returns></returns>
        public bool IsContractServiceTypeNameExit(ContractServiceType contractServiceTypes)
        {
            if (contractServiceTypes != null)
            {
                _cmd = _db.GetStoredProcCommand("IsContractServiceTypeNameExit");
                _db.AddInParameter(_cmd, "@ContractId", DbType.Int64, contractServiceTypes.ContractId);
                _db.AddInParameter(_cmd, "@ContractServiceTypeName", DbType.String, contractServiceTypes.ContractServiceTypeName.ToTrim());
                _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, contractServiceTypes.ContractServiceTypeId);

                return Convert.ToInt32(_db.ExecuteScalar(_cmd)) == 0;

            }
            return false;
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