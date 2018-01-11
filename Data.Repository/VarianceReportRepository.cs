using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Globalization;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class VarianceReportRepository : BaseRepository, IVarianceReportRepository
    {

        private Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="VarianceReportRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public VarianceReportRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the excel variance report.
        /// </summary>
        /// <param name="varianceReport">The variance report.</param>
        /// <param name="maxRecordLimitForExcelReport">The maximum record limit for excel report.</param>
        /// <returns>reportInfo</returns>
        public VarianceReport GetExcelVarianceReport(VarianceReport varianceReport, int maxRecordLimitForExcelReport)
        {
            VarianceReport varianceReportData = new VarianceReport();

            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetVarianceDetailReport");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommandObj, "@ModelID ", DbType.Int64, varianceReport.NodeId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@DateType ", DbType.Int32, varianceReport.DateType);
            _databaseObj.AddInParameter(_databaseCommandObj, "@DateFrom ", DbType.DateTime, varianceReport.StartDate);
            _databaseObj.AddInParameter(_databaseCommandObj, "@DateTo ", DbType.DateTime, varianceReport.EndDate);
            _databaseObj.AddInParameter(_databaseCommandObj, "@SelectCriteria ", DbType.String, varianceReport.ClaimSearchCriteria.ToTrim());
            _databaseObj.AddInParameter(_databaseCommandObj, "@MaxRecordLimit", DbType.Int32, maxRecordLimitForExcelReport);

            //Added RequestedUserID and RequestedUserName with reference to HIPAA logging feature
            _databaseObj.AddInParameter(_databaseCommandObj, "@RequestedUserID", DbType.String, varianceReport.RequestedUserId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@RequestedUserName", DbType.String, varianceReport.RequestedUserName);

            // Retrieve the results of the Stored Procedure in Data set
            _databaseCommandObj.CommandTimeout = varianceReport.CommandTimeoutForVarianceReport;
            DataSet varianceReportResult = _databaseObj.ExecuteDataSet(_databaseCommandObj);
            if (varianceReportResult.IsTableDataPopulated())
            {
                if (varianceReportResult.Tables[0].Columns.Count == 2)
                {
                    varianceReportData.CountThreshold = Convert.ToInt32(varianceReportResult.Tables[0].Rows[0][0]);
                    return varianceReportData;
                }

                List<EvaluateableClaim> claimDataList = new List<EvaluateableClaim>();
                DataRowCollection claimDataRowCollection = varianceReportResult.Tables[0].Rows;
                for (int indexCount = 0; indexCount < claimDataRowCollection.Count; indexCount++)
                {
                    DataRow claimDataRow = claimDataRowCollection[indexCount];
                    EvaluateableClaim claimData = new EvaluateableClaim
                    {
                        PatientAccountNumber = GetStringValue(claimDataRow["PatientAccountNumber"]),
                        AdjudicatedDate = GetValue<DateTime?>(claimDataRow["AdjudicatedDate"], typeof(DateTime)),
                        Ssinumber = GetValue<int?>(claimDataRow["SSINumber"], typeof(int)),
                        ClaimType = GetStringValue(claimDataRow["ClaimType"]),
                        ClaimState = GetStringValue(claimDataRow["ClaimState"]),
                        PayerSequence = GetValue<int?>(claimDataRow["PayerSequence"], typeof(int)),
                        ClaimTotal = GetValue<double?>(claimDataRow["ClaimTotal"], typeof(double)),
                        StatementFrom = GetValue<DateTime?>(claimDataRow["StatementFrom"], typeof(DateTime)),
                        StatementThru = GetValue<DateTime?>(claimDataRow["StatementThru"], typeof(DateTime)),
                        ClaimDate = GetValue<DateTime?>(claimDataRow["ClaimDate"], typeof(DateTime)),
                        BillDate = GetValue<DateTime?>(claimDataRow["BillDate"], typeof(DateTime)),
                        LastFiledDate = GetValue<DateTime?>(claimDataRow["LastFiledDate"], typeof(DateTime)),
                        LastFiledDateValue = DBNull.Value == claimDataRow["LastFiledDate"] ? null : Convert.ToDateTime(claimDataRow["LastFiledDate"]).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                        BillType = GetStringValue(claimDataRow["BillType"]),
                        Drg = GetStringValue(claimDataRow["DRG"]),
                        PriIcdpCode = GetStringValue(claimDataRow["PriICDPCode"]),
                        PriIcddCode = GetStringValue(claimDataRow["PriICDDCode"]),
                        PriPayerName = GetStringValue(claimDataRow["PriPayerName"]),
                        SecPayerName = GetStringValue(claimDataRow["SecPayerName"]),
                        TerPayerName = GetStringValue(claimDataRow["TerPayerName"]),
                        IsRemitLinked = GetStringValue(claimDataRow["IsRemitLinked"]),
                        ClaimStat = GetStringValue(claimDataRow["ClaimStatus"]),
                        AdjudicatedValue = GetValue<double?>(claimDataRow["AdjudicatedValue"], typeof(double)),
                        ActualPayment = GetValue<double>(claimDataRow["ActualPayment"], typeof(double)),
                        PatientResponsibility = GetValue<double?>(claimDataRow["PatientResponsibility"], typeof(double)),
                        RemitAllowedAmt = GetValue<double?>(claimDataRow["RemitAllowedAmt"], typeof(double)),
                        RemitNonCovered = GetValue<double?>(claimDataRow["RemitNonCovered"], typeof(double)),
                        ClaimLink = GetValue<long?>(claimDataRow["ClaimLink"], typeof(long)),
                        LinkedRemitId = GetStringValue(claimDataRow["LinkedRemitID"]),
                        ClaimId = GetValue<long>(claimDataRow["ClaimId"], typeof(long)),
                        CalculatedAdjustment = GetValue<double?>(claimDataRow["CalculatedAdj"], typeof(double)),
                        ActualAdjustment = GetValue<double?>(claimDataRow["ActualAdj"], typeof(double)),
                        ContractualVariance = GetValue<double?>(claimDataRow["ContractualVariance"], typeof(double)),
                        PaymentVariance = GetValue<double?>(claimDataRow["PaymentVariance"], typeof(double)),
                        AdjudicatedDateValue = DBNull.Value == claimDataRow["AdjudicatedDate"] ? null : Convert.ToString(claimDataRow["AdjudicatedDate"].ToString() == "1/1/1900 12:00:00 AM" ? "" : claimDataRow["AdjudicatedDate"].ToString()),
                        StatementFromValue = DBNull.Value == claimDataRow["StatementFrom"] ? null : Convert.ToString(claimDataRow["StatementFrom"].ToString() == "1/1/1900 12:00:00 AM" ? "" : Convert.ToDateTime(claimDataRow["StatementFrom"]).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)),
                        StatementThruValue = DBNull.Value == claimDataRow["StatementThru"] ? null : Convert.ToString(claimDataRow["StatementThru"].ToString() == "1/1/1900 12:00:00 AM" ? "" : Convert.ToDateTime(claimDataRow["StatementThru"]).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)),
                        ClaimDateValue = DBNull.Value == claimDataRow["ClaimDate"] ? null : Convert.ToString(claimDataRow["ClaimDate"].ToString() == "1/1/1900 12:00:00 AM" ? "" : Convert.ToDateTime(claimDataRow["ClaimDate"]).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)),
                        BillDateValue = DBNull.Value == claimDataRow["BillDate"] ? null : Convert.ToString(claimDataRow["BillDate"].ToString() == "1/1/1900 12:00:00 AM" ? "" : Convert.ToDateTime(claimDataRow["BillDate"]).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)),
                        CustomField1 = GetStringValue(claimDataRow["CustomField1"]),
                        CustomField2 = GetStringValue(claimDataRow["CustomField2"]),
                        CustomField3 = GetStringValue(claimDataRow["CustomField3"]),
                        CustomField4 = GetStringValue(claimDataRow["CustomField4"]),
                        CustomField5 = GetStringValue(claimDataRow["CustomField5"]),
                        CustomField6 = GetStringValue(claimDataRow["CustomField6"]),
                        DischargeStatus = GetStringValue(claimDataRow["DischargeStatus"]),
                        Npi = GetStringValue(claimDataRow["NPI"]),
                        MemberId = GetStringValue(claimDataRow["MemberID"]),
                        Icn = GetStringValue(claimDataRow["ICN"]),
                        Mrn = GetStringValue(claimDataRow["MRN"]),
                        IsReviewed = GetValue<bool>(claimDataRow["IsReviewed"], typeof(bool)),
                        Los = GetValue<int>(claimDataRow["Los"], typeof(int)),
                        Age = GetValue<byte>(claimDataRow["Age"], typeof(byte)),
                        CheckDate = DBNull.Value == claimDataRow["CheckDate"] ? null : Convert.ToString(claimDataRow["CheckDate"].ToString() == Constants.DateTime1900 ? string.Empty : Convert.ToDateTime(claimDataRow["CheckDate"]).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)),
                        CheckNumber = GetValue<string>(claimDataRow["CheckNumber"], typeof(string)),
                        AdjudicatedContractName = DBNull.Value == claimDataRow["AdjudicatedContractName"] ? null : GetValue<string>(claimDataRow["AdjudicatedContractName"], typeof(string)),
                        InsuredsGroupNumber = DBNull.Value == claimDataRow["InsuredsGroupNumber"] ? null : GetValue<string>(claimDataRow["InsuredsGroupNumber"], typeof(string))
                    };
                    claimDataList.Add(claimData);
                }
                varianceReportData.ClaimData = claimDataList;
            }

            return varianceReportData;
        }

        /// <summary>
        /// Gets all variance report.
        /// </summary>
        /// <param name="varianceReport">The variance report.</param>
        /// <returns>reportInfo</returns>
        public VarianceReport GetAllVarianceReport(VarianceReport varianceReport)
        {
            VarianceReport varianceReportData = new VarianceReport();

            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetContractManagementVarianceDetailReport");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommandObj, "@ModelID ", DbType.Int64, varianceReport.NodeId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@DateType ", DbType.Int32, varianceReport.DateType);
            _databaseObj.AddInParameter(_databaseCommandObj, "@DateFrom ", DbType.DateTime, varianceReport.StartDate);
            _databaseObj.AddInParameter(_databaseCommandObj, "@DateTo ", DbType.DateTime, varianceReport.EndDate);
            _databaseObj.AddInParameter(_databaseCommandObj, "@SelectCriteria ", DbType.String, varianceReport.ClaimSearchCriteria.ToTrim());
            _databaseObj.AddInParameter(_databaseCommandObj, "@ReportLevel", DbType.Int32, varianceReport.ReportLevel);
            _databaseObj.AddInParameter(_databaseCommandObj, "@MaxRecordLimit", DbType.Int32, varianceReport.MaxLinesForPdfReport);

            //Added RequestedUserID and RequestedUserName with reference to HIPAA logging feature
            _databaseObj.AddInParameter(_databaseCommandObj, "@RequestedUserID", DbType.String, varianceReport.RequestedUserId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@RequestedUserName", DbType.String, varianceReport.RequestedUserName);

            // Retrieve the results of the Stored Procedure in Data table
            _databaseCommandObj.CommandTimeout = varianceReport.CommandTimeoutForVarianceReport;
            DataSet varianceReportInfo = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            if (varianceReportInfo.IsTableDataPopulated())
            {
                List<Contract> contracts = new List<Contract>();

                List<VarianceReport> varianceReports = new List<VarianceReport>();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                if (varianceReport.ReportLevel == 1)
                {
                    if (varianceReportInfo.Tables[0].Columns.Count == 2)
                    {
                        varianceReportData.CountThreshold = -1;
                        return varianceReportData;
                    }

                    for (int indexCount = 0; indexCount < varianceReportInfo.Tables[1].Rows.Count; indexCount++)
                    {
                        DataRow claimCharge = varianceReportInfo.Tables[1].Rows[indexCount];
                        string claimId = GetStringValue(claimCharge["Claimid"]);

                        DataRow claimTable = varianceReportInfo.Tables[0].Select(string.Format("Claimid = '{0}'", claimId)).First();

                        VarianceReport varianceReportDetails = new VarianceReport
                        {
                            ClaimId = claimId,
                            Line = GetValue<int?>(claimCharge["line"], typeof(int)),
                            RevCode = GetStringValue(claimCharge["RevCode"]),
                            HcpcsCode = GetStringValue(claimCharge["HCPCSCode"]),
                            ServiceFromDate =
                                DBNull.Value == claimCharge["ServiceFromDate"]
                                    ? new DateTime()
                                    : Convert.ToDateTime(claimCharge["ServiceFromDate"]),
                            Units = GetValue<int?>(claimCharge["Units"], typeof(int)),
                            TotalCharges =
                                DBNull.Value == claimCharge["TotalCharges"]
                                    ? (double?)null
                                    : Convert.ToDouble(double.Parse(claimCharge["TotalCharges"].ToString())),
                            CalculatedAllowedChargeLevel =
                                DBNull.Value == claimCharge["CalculatedAllowed"]
                                    ? (double?)null
                                    : Convert.ToDouble(double.Parse(claimCharge["CalculatedAllowed"].ToString())),
                            PatientAccountNumber = GetStringValue(claimTable["PatAcctNum"]),
                            ContractName = GetStringValue(claimTable["ContractName"]),
                            PriPayerName =
                                DBNull.Value == claimTable["PriPayerName"]
                                    ? string.Empty
                                    : textInfo.ToTitleCase(
                                        claimTable["PriPayerName"].ToString()),
                            BillType = GetStringValue(claimTable["BillType"]),
                            Drg =
                                DBNull.Value == claimTable["DRG"]
                                    ? string.Empty
                                    : claimTable["DRG"].ToString(),
                            Los = GetValue<int>(claimTable["LOS"], typeof(int)),
                            StatementFrom = GetValue<DateTime>(claimTable["StatementFrom"], typeof(DateTime)),
                            StatementThru = GetValue<DateTime>(claimTable["StatementThru"], typeof(DateTime)),
                            ClaimTotal =
                                DBNull.Value == claimTable["ClaimTotalCharges"]
                                    ? (double?)null
                                    : Convert.ToDouble(double.Parse(claimTable["ClaimTotalCharges"].ToString())),
                            CalculatedAdjustment =
                                DBNull.Value == claimTable["CalculatedAdj"]
                                    ? (double?)null
                                    : Convert.ToDouble(double.Parse(claimTable["CalculatedAdj"].ToString())),
                            CalculatedAllowedClaimLevel =
                                DBNull.Value == claimTable["CalculatedAllowed"]
                                    ? (double?)null
                                    : Convert.ToDouble(double.Parse(claimTable["CalculatedAllowed"].ToString())),
                            ActualPaymentClaimLevel =
                                DBNull.Value == claimTable["ActualPmt"]
                                    ? (double?)null
                                    : Convert.ToDouble(double.Parse(claimTable["ActualPmt"].ToString())),
                            ActualAdjustment =
                                DBNull.Value == claimTable["ActulAdj"]
                                    ? (double?)null
                                    : Convert.ToDouble(double.Parse(claimTable["ActulAdj"].ToString())),
                            PatientResponsibility = DBNull.Value == claimTable["PatientResponsibility"]
                                    ? (double?)null
                                    : Convert.ToDouble(double.Parse(claimTable["PatientResponsibility"].ToString())),
                            ContractualVariance = DBNull.Value == claimTable["ContractualVariance"]
                                    ? (double?)null
                                     : Convert.ToDouble(double.Parse(claimTable["ContractualVariance"].ToString())),
                            PaymentVariance = DBNull.Value == claimTable["PaymentVariance"]
                                    ? (double?)null
                                    : Convert.ToDouble(double.Parse(claimTable["PaymentVariance"].ToString())),
                            HcpcsModifier = Regex.Replace(GetStringValue(claimCharge["HCPCSModifier"]), Constants.HcpcsModifierPattern, Constants.WhiteSpace),
                            PlaceOfService = GetStringValue(claimCharge["PlaceOfService"]),
                            ClaimType = claimTable["ClaimType"].ToString()
                        };
                        varianceReports.Add(varianceReportDetails);
                    }

                    varianceReportData.VarianceReports = varianceReports.OrderBy(q => q.PatientAccountNumber).ToList();
                }

                DataTable varianceReportDataTable = varianceReport.ReportLevel == 1 ? varianceReportInfo.Tables[2] : varianceReportInfo.Tables[0];

                contracts.AddRange(varianceReportDataTable.Rows.Cast<object>().Select((t, indexCount) => new Contract
                {
                    ContractName = GetStringValue(varianceReportDataTable.Rows[indexCount]["ContractName"]),
                    ClaimCount =
                        DBNull.Value == varianceReportDataTable.Rows[indexCount]["ClaimCount"]
                            ? 0
                            : Convert.ToInt64(varianceReportDataTable.Rows[indexCount]["ClaimCount"].ToString()),
                    TotalClaimCharges =
                        DBNull.Value == varianceReportDataTable.Rows[indexCount]["ClaimTotalCharges"]
                            ? (double?)null
                            : Math.Round(double.Parse(varianceReportDataTable.Rows[indexCount]["ClaimTotalCharges"].ToString()), 2),
                    CalculatedAllowed =
                        DBNull.Value == varianceReportDataTable.Rows[indexCount]["CalculatedAllowed"]
                            ? (double?)null
                            : Math.Round(double.Parse(varianceReportDataTable.Rows[indexCount]["CalculatedAllowed"].ToString()), 2),
                    PatientResponsibility =
                        DBNull.Value == varianceReportDataTable.Rows[indexCount]["PatientResponsibility"]
                            ? (double?)null
                            : Math.Round(double.Parse(varianceReportDataTable.Rows[indexCount]["PatientResponsibility"].ToString()), 2),
                    ActualPayment =
                        DBNull.Value == varianceReportDataTable.Rows[indexCount]["ActualPmt"]
                            ? (double?)null
                            : Math.Round(double.Parse(varianceReportDataTable.Rows[indexCount]["ActualPmt"].ToString()), 2),
                    PaymentVariance =
                        DBNull.Value == varianceReportDataTable.Rows[indexCount]["PaymentVariance"]
                            ? (double?)null
                            : Math.Round(double.Parse(varianceReportDataTable.Rows[indexCount]["PaymentVariance"].ToString()), 2),
                    CalculatedAdjustment =
                        DBNull.Value == varianceReportDataTable.Rows[indexCount]["CalculatedAdj"]
                            ? (double?)null
                            : Math.Round(double.Parse(varianceReportDataTable.Rows[indexCount]["CalculatedAdj"].ToString()), 2),
                    ActualAdjustment =
                        DBNull.Value == varianceReportDataTable.Rows[indexCount]["ActulAdj"]
                            ? (double?)null
                            : Math.Round(double.Parse(varianceReportDataTable.Rows[indexCount]["ActulAdj"].ToString()), 2),
                    ContractualVariance =
                        DBNull.Value == varianceReportDataTable.Rows[indexCount]["ContractualVariance"]
                            ? (double?)null
                            : Math.Round(double.Parse(varianceReportDataTable.Rows[indexCount]["ContractualVariance"].ToString()), 2)
                }));
                Contract unadjudicatedContract = contracts.FirstOrDefault(a => a.ContractName == "Un-adjudicated");
                //Removes the unadjudicated contract from the list and add tto the bottom
                if (unadjudicatedContract != null)
                {
                    contracts.Remove(unadjudicatedContract);
                    contracts.Add(unadjudicatedContract);
                }
                //FIXED-OCT15-Make a common function in base repository so we can utilize it in adjudication as well as variance report. 
                varianceReportData.ClaimsAdjudicated = GetClaimsAdjudicated(varianceReport.ReportLevel == 1 ? varianceReportInfo.Tables[4] : varianceReportInfo.Tables[2]);
                varianceReportData.PaymentsLinked = GetPaymentsLinked(varianceReport.ReportLevel == 1 ? varianceReportInfo.Tables[5] : varianceReportInfo.Tables[3]);
                varianceReportData.ClaimCharges = GetClaimCharges(varianceReport.ReportLevel == 1 ? varianceReportInfo.Tables[6] : varianceReportInfo.Tables[4]);
                varianceReportData.ClaimVariances = GetClaimVariances(varianceReport.ReportLevel == 1 ? varianceReportInfo.Tables[7] : varianceReportInfo.Tables[5]);
                varianceReportData.VarianceRanges = GetVarianceRanges(varianceReport.ReportLevel == 1 ? varianceReportInfo.Tables[8] : varianceReportInfo.Tables[6]);
                varianceReportData.Contracts = contracts;
                varianceReportData.FacilityName = GetStringValue(varianceReportDataTable.Rows[0]["FacilityName"]);

                varianceReportData.TotalClaims = varianceReport.ReportLevel == 1
                    ? GetValue<int>(varianceReportInfo.Tables[3].Rows[0]["TotalClaims"], typeof(int))
                    : GetValue<int>(varianceReportInfo.Tables[1].Rows[0]["TotalClaims"], typeof(int));


            }

            return varianceReportData;
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
