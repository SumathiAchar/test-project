using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    /// <summary>
    /// Repository for the Evaluatable Claim
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class EvaluateableClaimRepository : BaseRepository, IEvaluateableClaimRepository
    {
        private Database _databaseObj;
        private DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="LetterTemplateRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public EvaluateableClaimRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, SqlClientFactory.Instance);
        }

        /// <summary>
        /// Adds the claims for a task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        public long AddClaimsForATask(long taskId)
        {
            if (taskId > 0)
            {
                //REVIEW-RAGINI-FEB16 AddClaimsForATask SP - Add comments why -1,0,132 is used
                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("AddClaimsForATask");
                //REVIEW-RAGINI-FEB16 - CommandTimeout should be set in web.config
                _databaseCommandObj.CommandTimeout = 5400;
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@TaskID", DbType.Int64, taskId);
                // Retrieve the results of the Stored Procedure in Data table
                long noOfClaim = long.Parse(_databaseObj.ExecuteScalar(_databaseCommandObj).ToString());
                return noOfClaim;
            }
            return 0;
        }


        /// <summary>
        /// Gets the evaluateable claims.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="noOfRecord">The no of record.</param>
        /// <param name="startRow">The start row.</param>
        /// <param name="endRow">The end row.</param>
        /// <returns></returns>
        public List<EvaluateableClaim> GetEvaluateableClaims(long taskId, int noOfRecord, long startRow, long endRow)
        {
            List<EvaluateableClaim> evaluateableClaims = null;
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetEvaluateableClaimsByTaskID");
                _databaseCommandObj.CommandTimeout = 1000;
                _databaseObj.AddInParameter(_databaseCommandObj, "@TaskID", DbType.Int64, taskId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@NoOfRecord", DbType.Int32, noOfRecord);
                _databaseObj.AddInParameter(_databaseCommandObj, "@StartRow", DbType.Int64, startRow);
                _databaseObj.AddInParameter(_databaseCommandObj, "@EndRow", DbType.Int64, endRow);
                DataSet dataSetClaimFullData = _databaseObj.ExecuteDataSet(_databaseCommandObj);
                if (dataSetClaimFullData.IsTableDataPopulated(0))
                {
                    evaluateableClaims = (from DataRow row in dataSetClaimFullData.Tables[0].Rows
                        select new EvaluateableClaim
                        {
                            ClaimId = GetValue<long>(row["ClaimID"], typeof (long)),
                            ClaimLink = GetValue<long>(row["ClaimLink"], typeof (long)),
                            Ssinumber = GetValue<int>(row["ssinumber"], typeof (int)),
                            ClaimType = GetStringValue(row["ClaimType"]),
                            ClaimState = GetStringValue(row["ClaimState"]),
                            PayerSequence = GetValue<int>(row["PayerSequence"], typeof (int)),
                            PatAcctNum = GetStringValue(row["PatAcctNum"]).ToTrim(),
                            Age = GetValue<byte>(row["Age"], typeof (byte)),
                            ClaimTotal = GetValue<double>(row["ClaimTotal"], typeof (double)),
                            StatementFrom = GetValue<DateTime>(row["StatementFrom"], typeof (DateTime)),
                            StatementThru = GetValue<DateTime>(row["StatementThru"], typeof (DateTime)),
                            ClaimDate = GetValue<DateTime>(row["ClaimDate"], typeof (DateTime)),
                            BillDate = GetValue<DateTime>(row["BillDate"], typeof (DateTime)),
                            LastFiledDate = GetValue<DateTime>(row["LastFiledDate"], typeof (DateTime)),
                            BillType = GetStringValue(row["BillType"]),
                            Drg = GetStringValue(row["DRG"]),
                            PriIcddCode = GetStringValue(row["PriICDDCode"]),
                            PriIcdpCode = GetStringValue(row["PriICDDCode"]),
                            PriPayerName = GetStringValue(row["PriPayerName"]).ToTrim(),
                            SecPayerName = GetStringValue(row["SecPayerName"]),
                            TerPayerName = GetStringValue(row["TerPayerName"]),
                            Ftn = GetStringValue(row["FTN"]),
                            Npi = GetStringValue(row["NPI"]).ToTrim(),
                            RenderingPhy = GetStringValue(row["RenderingPHY"]).ToTrim(),
                            RefPhy = GetStringValue(row["RefPHY"]).ToTrim(),
                            AttendingPhy = GetStringValue(row["AttendingPHY"]).ToTrim(),
                            ProviderZip = GetStringValue(row["ProviderZip"]),
                            DischargeStatus = GetStringValue(row["DischargeStatus"]).ToTrim(),
                            CustomField1 = GetStringValue(row["CustomField1"]).ToTrim(),
                            CustomField2 = GetStringValue(row["CustomField2"]).ToTrim(),
                            CustomField3 = GetStringValue(row["CustomField3"]).ToTrim(),
                            CustomField4 = GetStringValue(row["CustomField4"]).ToTrim(),
                            CustomField5 = GetStringValue(row["CustomField5"]).ToTrim(),
                            CustomField6 = GetStringValue(row["CustomField6"]).ToTrim(),
                            Mrn = GetStringValue(row["MRN"]),
                            Icn = GetStringValue(row["ICN"]),
                            ContractId = GetValue<long?>(row["ContractID"], typeof (long)),
                            ClaimCharges =
                                GetClaimCharges(dataSetClaimFullData.Tables[1],
                                    GetValue<long>(row["ClaimID"], typeof (long))),
                            DiagnosisCodes =
                                GetDiagnosisCodes(dataSetClaimFullData.Tables[2],
                                    GetValue<long>(row["ClaimID"], typeof (long))),
                            ProcedureCodes =
                                GetProcedureCodes(dataSetClaimFullData.Tables[3],
                                    GetValue<long>(row["ClaimID"], typeof (long))),
                            Physicians =
                                GetClaimPhysicians(dataSetClaimFullData.Tables[4],
                                    GetValue<long>(row["ClaimID"], typeof (long))),
                            InsuredCodes =
                                GetInsuredCodes(dataSetClaimFullData.Tables[5],
                                    GetValue<long>(row["ClaimID"], typeof (long))),
                            ValueCodes =
                                GetValueCodes(dataSetClaimFullData.Tables[6],
                                    GetValue<long>(row["ClaimID"], typeof (long))),
                            OccurrenceCodes =
                                GetOccurrenceCodes(dataSetClaimFullData.Tables[7],
                                    GetValue<long>(row["ClaimID"], typeof (long))),
                            ConditionCodes =
                                GetConditionCodes(dataSetClaimFullData.Tables[8],
                                    GetValue<long>(row["ClaimID"], typeof (long))),
                            MedicareLabFeeSchedules =
                                GetMedicareLabFeeSchedules(dataSetClaimFullData.Tables[9],
                                    GetStringValue(row["ProviderZip"])),
                            MedicareInPatient =
                                GetMedicareInPatient(dataSetClaimFullData.Tables[0],
                                    GetValue<long>(row["ClaimID"], typeof (long))),
                            PatientData =
                                GetPatientData(dataSetClaimFullData.Tables[10],
                                    GetValue<long>(row["ClaimID"], typeof (long))),
                            Los = GetValue<int>(row["Los"], typeof (int)),
                            PatientResponsibility = GetValue<double>(row["PatientResponsibility"], typeof (double)),
                            IsClaimAdjudicated = GetValue<bool>(row["IsClaimAdjudicated"], typeof (bool)),
                            LastAdjudicatedContractId = GetValue<long>(row["LastAdjudicatedContractID"], typeof (long)),
                            BackgroundContractId = GetValue<long>(row["BackgroundContractID"], typeof (long)),
                        }).ToList();
            }
            return evaluateableClaims;
        }

        /// <summary>
        /// For getting ClaimCharge information based on claimId
        /// </summary>
        /// <param name="claimChargeDataTable"></param>
        /// <param name="claimId"></param>
        /// <returns>List of ClaimCharge</returns>
        private List<ClaimCharge> GetClaimCharges(DataTable claimChargeDataTable, long claimId)
        {
            List<ClaimCharge> claimCharges = new List<ClaimCharge>();

            if (claimChargeDataTable != null && claimChargeDataTable.Rows.Count != 0)
            {
                claimCharges = (from DataRow row in claimChargeDataTable.Rows
                                where (GetValue<long>(row["ClaimID"], typeof(long)) == claimId)
                                select new ClaimCharge
                                {
                                    Line = DBNull.Value == row["Line"] ? 0 : GetValue<int>(row["Line"], typeof(int)),
                                    RevCode = GetStringValue(row["RevCode"]),
                                    HcpcsCode = GetStringValue(row["HCPCSCode"]).ToTrim(),
                                    ServiceFromDate = GetValue<DateTime>(row["ServiceFromDate"], typeof(DateTime)),
                                    ServiceThruDate = GetValue<DateTime>(row["ServiceThruDate"], typeof(DateTime)),
                                    Units = GetValue<int>(row["Units"], typeof(int)),
                                    Amount = GetValue<double>(row["Amount"], typeof(double)),
                                    NonCoveredCharge = GetValue<double>(row["NonCoveredCharge"], typeof(double)),
                                    CoveredCharge = GetValue<double>(row["CoveredCharge"], typeof(double)),
                                    HcpcsModifiers = GetStringValue(row["HCPCSmods"]).ToTrim(),
                                    PlaceOfService = GetStringValue(row["PlaceOfService"]),
                                    HcpcsCodeWithModifier =
                                        string.Format("{0}{1}", GetStringValue(row["HCPCSCode"]).ToTrim(),
                                            GetStringValue(row["HCPCSmods"]).ToTrim()).Trim()
                                }).ToList();
            }
            return claimCharges;
        }

        /// <summary>
        /// For getting DiagnosisCode based on claimId
        /// </summary>
        /// <param name="diagnosisCodeDataTable"></param>
        /// <param name="claimId"></param>
        /// <returns>List of DiagnosisCode </returns>
        private List<DiagnosisCode> GetDiagnosisCodes(DataTable diagnosisCodeDataTable, long claimId)
        {
            List<DiagnosisCode> diagnosisCodes = new List<DiagnosisCode>();

            if (diagnosisCodeDataTable != null && diagnosisCodeDataTable.Rows.Count != 0)
            {
                diagnosisCodes = (from DataRow row in diagnosisCodeDataTable.Rows
                                  where (GetValue<long>(row["ClaimId"], typeof(long)) == claimId)
                                  select new DiagnosisCode
                                  {
                                      ClaimId = GetValue<long>(row["ClaimId"], typeof(long)),
                                      Instance = GetStringValue(row["Instance"]),
                                      IcddCode = GetStringValue(row["ICDDCode"]).Trim()
                                  }).ToList();
            }

            return diagnosisCodes;
        }

        /// <summary>
        /// For getting ProcedureCode based on claimId
        /// </summary>
        /// <param name="procedureCodeDataTable"></param>
        /// <param name="claimId"></param>
        /// <returns>List of ProcedureCode</returns>
        private List<ProcedureCode> GetProcedureCodes(DataTable procedureCodeDataTable, long claimId)
        {
            List<ProcedureCode> procedureCodes = new List<ProcedureCode>();

            if (procedureCodeDataTable != null && procedureCodeDataTable.Rows.Count != 0)
            {
                procedureCodes = (from DataRow row in procedureCodeDataTable.Rows
                                  where (GetValue<long>(row["ClaimId"], typeof(long)) == claimId)
                                  select new ProcedureCode
                                  {
                                      ClaimId = GetValue<long>(row["ClaimId"], typeof(long)),
                                      Instance = GetStringValue(row["Instance"]),
                                      IcdpCode = GetStringValue(row["ICDPCode"]).ToTrim()
                                  }).ToList();
            }

            return procedureCodes;
        }

        /// <summary>
        /// Gets the claim physician data.
        /// </summary>
        /// <param name="claimPhysicianDataTable">The data table.</param>
        /// <param name="claimId">The claim identifier.</param>
        /// <returns></returns>
        private List<Physician> GetClaimPhysicians(DataTable claimPhysicianDataTable, long claimId)
        {
            List<Physician> physicians = new List<Physician>();

            if (claimPhysicianDataTable != null && claimPhysicianDataTable.Rows.Count != 0)
            {
                physicians = (from DataRow row in claimPhysicianDataTable.Rows
                              where (GetValue<long>(row["ClaimId"], typeof(long)) == claimId)
                              select new Physician
                              {
                                  ClaimId = GetValue<long>(row["ClaimId"], typeof(long)),
                                  PhysicianId = GetValue<long>(row["PhysicianID"], typeof(long)),
                                  FirstName = GetStringValue(row["FirstName"]),
                                  LastName = GetStringValue(row["LastName"]),
                                  MiddleName = GetStringValue(row["MiddleName"]),
                                  PhysicianType = GetStringValue(row["PhysicianType"])
                              }).ToList();
            }

            return physicians;
        }

        /// <summary>
        /// Gets the insured data list.
        /// </summary>
        /// <param name="insuredCodeDataTable">The dt.</param>
        /// <param name="claimId">The claim identifier.</param>
        /// <returns></returns>
        private List<InsuredData> GetInsuredCodes(DataTable insuredCodeDataTable, long claimId)
        {
            List<InsuredData> insuredCodes = new List<InsuredData>();

            if (insuredCodeDataTable != null && insuredCodeDataTable.Rows.Count != 0)
            {
                insuredCodes = (from DataRow row in insuredCodeDataTable.Rows
                                where (GetValue<long>(row["ClaimId"], typeof(long)) == claimId)
                                select new InsuredData
                                {
                                    ClaimId = GetValue<long>(row["ClaimId"], typeof(long)),
                                    PayerName = GetStringValue(row["PayerName"]),
                                    InsuredFirstName = GetStringValue(row["InsuredFirstName"]),
                                    InsuredLastName = GetStringValue(row["InsuredLastName"]),
                                    InsuredMiddleName = GetStringValue(row["InsuredMiddleName"]),
                                    CertificationNumber = GetStringValue(row["CertificationNumber"]).ToTrim(),
                                    GroupName = GetStringValue(row["GroupName"]),
                                    GroupNumber = GetStringValue(row["GroupNumber"]).ToTrim(),
                                    TreatmentAuthorization = GetStringValue(row["TreatmentAuthorization"])
                                }).ToList();
            }

            return insuredCodes;
        }

        /// <summary>
        /// Gets the value code list.
        /// </summary>
        /// <param name="valueCodeDataTable">The valueCodeDataTable.</param>
        /// <param name="claimId">The claim identifier.</param>
        /// <returns></returns>
        private List<ValueCode> GetValueCodes(DataTable valueCodeDataTable, long claimId)
        {
            List<ValueCode> valueCodes = new List<ValueCode>();

            if (valueCodeDataTable != null && valueCodeDataTable.Rows.Count != 0)
            {
                valueCodes = (from DataRow row in valueCodeDataTable.Rows
                              where (GetValue<long>(row["ClaimId"], typeof(long)) == claimId)
                              select new ValueCode
                              {
                                  ClaimId = GetValue<long>(row["ClaimId"], typeof(long)),
                                  Instance = GetValue<int>(row["Instance"], typeof(int)),
                                  Code = GetStringValue(row["ValueCode"]).ToTrim()
                              }).ToList();
            }
            return valueCodes;
        }

        /// <summary>
        /// Gets the occurrence code list.
        /// </summary>
        /// <param name="occurrenceCodeDataTable">The dt.</param>
        /// <param name="claimId">The claim identifier.</param>
        /// <returns></returns>
        private List<OccurrenceCode> GetOccurrenceCodes(DataTable occurrenceCodeDataTable, long claimId)
        {
            List<OccurrenceCode> occurrenceCodes = new List<OccurrenceCode>();

            if (occurrenceCodeDataTable != null && occurrenceCodeDataTable.Rows.Count != 0)
            {
                occurrenceCodes = (from DataRow row in occurrenceCodeDataTable.Rows
                                   where (GetValue<long>(row["ClaimId"], typeof(long)) == claimId)
                                   select new OccurrenceCode
                                   {
                                       ClaimId = GetValue<long>(row["ClaimId"], typeof(long)),
                                       Instance = GetValue<int>(row["Instance"], typeof(int)),
                                       Code = GetStringValue(row["OccurenceCode"]).ToTrim()
                                   }).ToList();
            }

            return occurrenceCodes;
        }

        /// <summary>
        /// Gets the condition code list.
        /// </summary>
        /// <param name="conditionCodeDataTable">The dt.</param>
        /// <param name="claimId">The claim identifier.</param>
        /// <returns></returns>
        private List<ConditionCode> GetConditionCodes(DataTable conditionCodeDataTable, long claimId)
        {
            List<ConditionCode> conditionCodes = new List<ConditionCode>();

            if (conditionCodeDataTable != null && conditionCodeDataTable.Rows.Count != 0)
            {
                conditionCodes = (from DataRow row in conditionCodeDataTable.Rows
                                  where (GetValue<long>(row["ClaimId"], typeof(long)) == claimId)
                                  select new ConditionCode
                                  {
                                      ClaimId = GetValue<long>(row["ClaimId"], typeof(long)),
                                      Instance = GetValue<int>(row["Instance"], typeof(int)),
                                      Code = GetStringValue(row["ConditionCode"]).ToTrim()
                                  }).ToList();
            }
            return conditionCodes;
        }

        /// <summary>
        /// Gets the Medicare lab fee schedules.
        /// </summary>
        /// <param name="medicareLabFeeDataTable">The Medicare lab fee data table.</param>
        /// <param name="providerZip">The provider zip.</param>
        /// <returns></returns>
        private List<MedicareLabFeeSchedule> GetMedicareLabFeeSchedules(DataTable medicareLabFeeDataTable,
            string providerZip)
        {
            List<MedicareLabFeeSchedule> medicareLabFeeSchedules = new List<MedicareLabFeeSchedule>();

            if (medicareLabFeeDataTable != null && medicareLabFeeDataTable.Rows.Count != 0)
            {
                for (int j = 0; j < medicareLabFeeDataTable.Rows.Count; j++)
                {
                    if (GetStringValue((medicareLabFeeDataTable.Rows[j]["ProviderZip"])).Equals(providerZip))
                    {
                        MedicareLabFeeSchedule medicareLabFeeSchedule = new MedicareLabFeeSchedule
                        {
                            Hcpcs = GetStringValue((medicareLabFeeDataTable.Rows[j]["HCPCS"])),
                            Amount = GetValue<double>((medicareLabFeeDataTable.Rows[j]["Amount"]), typeof(double)),
                            HcpcsCodeWithModifier =
                                string.Format("{0}{1}",
                                    GetStringValue(medicareLabFeeDataTable.Rows[j]["HCPCS"]).ToTrim(),
                                    GetStringValue(medicareLabFeeDataTable.Rows[j]["HCPCSmods"]).ToTrim()).Trim(),
                        };
                        medicareLabFeeSchedules.Add(medicareLabFeeSchedule);
                    }
                }
            }
            return medicareLabFeeSchedules;
        }

        /// <summary>
        /// Gets the Medicare in patient.
        /// </summary>
        /// <param name="medicareIpDataTable">The dt.</param>
        /// <param name="claimId">The claim identifier.</param>
        /// <returns></returns>
        private MedicareInPatient GetMedicareInPatient(DataTable medicareIpDataTable, long claimId)
        {
            MedicareInPatient medicareInPatient = new MedicareInPatient();
            if (medicareIpDataTable != null && medicareIpDataTable.Rows.Count != 0)
            {
                medicareInPatient = (from DataRow row in medicareIpDataTable.Rows
                                     where (GetValue<long>(row["ClaimId"], typeof(long)) == claimId)
                                     select new MedicareInPatient
                                     {
                                         ClaimId = GetValue<long>(row["ClaimId"], typeof(long)),
                                         Npi = GetStringValue(row["NPI"]),
                                         DischargeDate = GetValue<DateTime>(row["StatementThru"], typeof(DateTime)),
                                         DischargeStatus = GetStringValue(row["DischargeStatus"]),
                                         Drg = GetStringValue(row["DRG"]),
                                         LengthOfStay = GetValue<int>(row["Los"], typeof(int)),
                                         Charges = GetValue<double>(row["ClaimTotal"], typeof(double))
                                     }).First();
            }
            return medicareInPatient;
        }

        /// <summary>
        /// Gets the patient data.
        /// </summary>
        /// <param name="patientDataDataTable">The dt table.</param>
        /// <param name="claimId">The claim identifier.</param>
        /// <returns></returns>
        private PatientData GetPatientData(DataTable patientDataDataTable, long claimId)
        {
            //populating Patient data
            var patientData = (from DataRow row in patientDataDataTable.Rows
                               where (GetValue<long>(row["ClaimId"], typeof(long)) == claimId)
                               select new PatientData
                               {
                                   ClaimId = GetValue<long>(row["ClaimId"], typeof(long)),
                                   LastName = GetStringValue(row["LastName"]),
                                   FirstName = GetStringValue(row["FirstName"]),
                                   MiddleName = GetStringValue(row["Middle"]),
                                   Dob = GetValue<DateTime>(row["DOB"], typeof(DateTime)),
                                   Status = GetStringValue(row["Status"]),
                                   Gender = DBNull.Value == row["Sex"] ? 0 : GetValue<int>(row["Sex"], typeof(int)),
                                   Medicare = GetStringValue(row["Medicare"])
                               }).FirstOrDefault();

            return patientData;
        }

        /// <summary>
        /// Updates the running task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="isRunning">The is running.</param>
        /// <returns></returns>
        public void UpdateRunningTask(long taskId, byte isRunning)
        {
            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("UpdateRunningTask");
            _databaseCommandObj.CommandTimeout = 5400;
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommandObj, "@TaskID", DbType.Int64, taskId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@IsRunning", DbType.Byte, isRunning);
            // Retrieve the results of the Stored Procedure in Data table
            _databaseObj.ExecuteNonQuery(_databaseCommandObj);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseObj = null;
            _databaseCommandObj.Dispose();
        }

    }
}
