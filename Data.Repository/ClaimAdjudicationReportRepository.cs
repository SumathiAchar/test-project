using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;


namespace SSI.ContractManagement.Data.Repository
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ClaimAdjudicationReportRepository : BaseRepository, IAdjudicationReportRepository
    {
        // Variables
        private Database _databaseObj;
        DbCommand _databaseCommandObj;


        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimAdjudicationReportRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ClaimAdjudicationReportRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }
        /// <summary>
        /// Gets all claim adjudication.
        /// </summary>
        /// <param name="claimAdjudicationReport">The claim adjudication report.</param>
        /// <returns>
        /// ClaimAdjudicationReport.
        /// </returns>
        public ClaimAdjudicationReport GetClaimAdjudicationReport(ClaimAdjudicationReport claimAdjudicationReport)
        {
            ClaimAdjudicationReport claimAdjudicationReportList = new ClaimAdjudicationReport
            {
                ClaimAdjudicationReports = new List<ClaimAdjudicationReport>()
            };
            if (claimAdjudicationReport != null)
            {
                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetClaimAdjudicationReport");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@ModelID", DbType.Int64,
                    claimAdjudicationReport.NodeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@DateType", DbType.Int32,
                    claimAdjudicationReport.DateType);
                _databaseObj.AddInParameter(_databaseCommandObj, "@DateFrom", DbType.DateTime,
                    claimAdjudicationReport.StartDate);
                _databaseObj.AddInParameter(_databaseCommandObj, "@DateTo", DbType.DateTime,
                    claimAdjudicationReport.EndDate);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SelectCriteria", DbType.String,
                    claimAdjudicationReport.ClaimSearchCriteria);
                _databaseObj.AddInParameter(_databaseCommandObj, "@PageSize", DbType.Int32,
                    claimAdjudicationReport.PageSize);
                _databaseObj.AddInParameter(_databaseCommandObj, "@PageIndex", DbType.Int32,
                    claimAdjudicationReport.PageIndex);
                //Added RequestedUserID and RequestedUserName with reference to HIPAA logging feature
                _databaseObj.AddInParameter(_databaseCommandObj, "@RequestedUserID", DbType.String,
                    claimAdjudicationReport.RequestedUserId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@RequestedUserName", DbType.String,
                    claimAdjudicationReport.RequestedUserName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@MaxRecordLimit", DbType.Int32,
                    claimAdjudicationReport.MaxLinesForPdfReport);
                // Retrieve the results of the Stored Procedure in Data table=====================================================================
                _databaseCommandObj.CommandTimeout = claimAdjudicationReport.CommandTimeoutForClaimAdjudication;
                //============================================================================
                DataSet claimAdjudicationReportResult = _databaseObj.ExecuteDataSet(_databaseCommandObj);
                if (claimAdjudicationReportResult.IsTableDataPopulated(0))
                {
                    if (claimAdjudicationReportResult.Tables.Count == 2)
                    {
                        ClaimAdjudicationReport reportInfoCrossedThreshold = new ClaimAdjudicationReport
                        {
                            ReportThreshold = -1
                        };
                        return reportInfoCrossedThreshold;
                    }

                    if (claimAdjudicationReportResult.Tables.Count >= 6)
                    {
                        DataTable claimAdjudicationReportData = claimAdjudicationReportResult.Tables[6];
                        List<Contract> contracts = new List<Contract>();
                        //Set Columns
                        SetColumnsValues(claimAdjudicationReportList,claimAdjudicationReportData,claimAdjudicationReportResult);
                        DataTable varianceSummaryDataTable = claimAdjudicationReportResult.Tables[7];
                        contracts.AddRange(varianceSummaryDataTable.Rows.Cast<object>().Select((t, indexCount) => new Contract
                        {
                            ContractName = GetStringValue(varianceSummaryDataTable.Rows[indexCount]["ContractName"]),
                            ClaimCount =
                                DBNull.Value == varianceSummaryDataTable.Rows[indexCount]["ClaimCount"]
                                    ? 0
                                    : Convert.ToInt64(varianceSummaryDataTable.Rows[indexCount]["ClaimCount"].ToString()),
                            TotalClaimCharges =
                                DBNull.Value == varianceSummaryDataTable.Rows[indexCount]["ClaimTotalCharges"]
                                    ? (double?)null
                                    : Math.Round(double.Parse(varianceSummaryDataTable.Rows[indexCount]["ClaimTotalCharges"].ToString()), 2),
                            CalculatedAllowed =
                                DBNull.Value == varianceSummaryDataTable.Rows[indexCount]["CalculatedAllowed"]
                                    ? (double?)null
                                    : Math.Round(double.Parse(varianceSummaryDataTable.Rows[indexCount]["CalculatedAllowed"].ToString()), 2),
                            PatientResponsibility =
                                DBNull.Value == varianceSummaryDataTable.Rows[indexCount]["PatientResponsibility"]
                                    ? (double?)null
                                    : Math.Round(double.Parse(varianceSummaryDataTable.Rows[indexCount]["PatientResponsibility"].ToString()), 2),
                            ActualPayment =
                                DBNull.Value == varianceSummaryDataTable.Rows[indexCount]["ActualPmt"]
                                    ? (double?)null
                                    : Math.Round(double.Parse(varianceSummaryDataTable.Rows[indexCount]["ActualPmt"].ToString()), 2),
                            PaymentVariance =
                                DBNull.Value == varianceSummaryDataTable.Rows[indexCount]["PaymentVariance"]
                                    ? (double?)null
                                    : Math.Round(double.Parse(varianceSummaryDataTable.Rows[indexCount]["PaymentVariance"].ToString()), 2),
                            CalculatedAdjustment =
                                DBNull.Value == varianceSummaryDataTable.Rows[indexCount]["CalculatedAdj"]
                                    ? (double?)null
                                    : Math.Round(double.Parse(varianceSummaryDataTable.Rows[indexCount]["CalculatedAdj"].ToString()), 2),
                            ActualAdjustment =
                                DBNull.Value == varianceSummaryDataTable.Rows[indexCount]["ActulAdj"]
                                    ? (double?)null
                                    : Math.Round(double.Parse(varianceSummaryDataTable.Rows[indexCount]["ActulAdj"].ToString()), 2),
                            ContractualVariance =
                                DBNull.Value == varianceSummaryDataTable.Rows[indexCount]["ContractualVariance"]
                                    ? (double?)null
                                    : Math.Round(double.Parse(varianceSummaryDataTable.Rows[indexCount]["ContractualVariance"].ToString()), 2)
                        }));
                        Contract unadjudicatedContract = contracts.FirstOrDefault(a => a.ContractName == "Un-adjudicated");
                        //Removes the unadjudicated contract from the list and add tto the bottom
                        if (unadjudicatedContract != null)
                        {
                            contracts.Remove(unadjudicatedContract);
                            contracts.Add(unadjudicatedContract);
                        }
                        claimAdjudicationReportList.Contracts = contracts;
                        claimAdjudicationReportList.ClaimsAdjudicated =
                            GetClaimsAdjudicated(claimAdjudicationReportResult.Tables[1]);
                        //FIXED-OCT15 -- Read 2, C2, "-" from Constant 
                        claimAdjudicationReportList.PaymentsLinked =
                            GetPaymentsLinked(claimAdjudicationReportResult.Tables[2]);
                        claimAdjudicationReportList.ClaimCharges =
                            GetClaimCharges(claimAdjudicationReportResult.Tables[3]);
                        claimAdjudicationReportList.ClaimVariances =
                            GetClaimVariances(claimAdjudicationReportResult.Tables[4]);
                        claimAdjudicationReportList.VarianceRanges =
                            GetVarianceRanges(claimAdjudicationReportResult.Tables[5]);
                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                        if (claimAdjudicationReportData != null && claimAdjudicationReportData.Rows != null &&
                            claimAdjudicationReportData.Rows.Count > 0)
                        {
                            claimAdjudicationReportList.FacilityName = GetStringValue(claimAdjudicationReportData.Rows[0]["FacilityName"]);
                            claimAdjudicationReportList.TotalRecords = GetValue<int>(claimAdjudicationReportResult.Tables[0].Rows[0]["TotalClaims"], typeof(int));
                        }
                    }
                }
            }
            return claimAdjudicationReportList;
        }

        /// <summary>
        /// Set Columns Values
        /// </summary>
        /// <param name="claimAdjudicationReportList"></param>
        /// <param name="claimAdjudicationReportData"></param>
        /// <param name="claimAdjudicationReportResult"></param>
        private static void SetColumnsValues(ClaimAdjudicationReport claimAdjudicationReportList,
            DataTable claimAdjudicationReportData, DataSet claimAdjudicationReportResult)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            claimAdjudicationReportList.ClaimAdjudicationReports.AddRange(
                claimAdjudicationReportData.Rows.Cast<object>()
                    .Select((t, indexCount) => new ClaimAdjudicationReport
                    {
                        NodeId = GetValue<long>(claimAdjudicationReportData.Rows[indexCount]["ModelID"], typeof (long)),
                        ModelId = GetValue<long>(claimAdjudicationReportData.Rows[indexCount]["ModelID"], typeof (long)),
                        ClaimIdentity =
                            GetValue<long>(claimAdjudicationReportData.Rows[indexCount]["ClaimId"], typeof (long)),
                        ClaimId = GetStringValue(claimAdjudicationReportData.Rows[indexCount]["ClaimId"]),
                        PatientAccountNumber =
                            GetStringValue(claimAdjudicationReportData.Rows[indexCount]["PatAcctNum"]),
                        ContractId =
                            GetValue<long>(claimAdjudicationReportData.Rows[indexCount]["ContractID"], typeof (long)),
                        ClaimContractName =
                            GetStringValue(claimAdjudicationReportResult.Tables[6].Rows[indexCount]["ContractName"]),
                        PayerName =
                            DBNull.Value == claimAdjudicationReportData.Rows[indexCount]["PriPayerName"]
                                ? string.Empty
                                : textInfo.ToTitleCase(
                                    claimAdjudicationReportData.Rows[indexCount]["PriPayerName"]
                                        .ToString()),
                        BillType = GetStringValue(claimAdjudicationReportData.Rows[indexCount]["BillType"]),
                        Drg = GetStringValue(claimAdjudicationReportData.Rows[indexCount]["DRG"]),
                        ServiceLine =
                            DBNull.Value ==
                            claimAdjudicationReportData.Rows[indexCount]["ClaimServiceLineID"]
                                ? null
                                : claimAdjudicationReportData.Rows[indexCount]["ClaimServiceLineID"]
                                    .ToString(),
                        Los = GetValue<int>(claimAdjudicationReportData.Rows[indexCount]["LOS"], typeof (int)),
                        StatementFrom =
                            GetValue<DateTime>(claimAdjudicationReportData.Rows[indexCount]["StatementFrom"],
                                typeof (DateTime)),
                        StatementThrough =
                            GetValue<DateTime>(claimAdjudicationReportData.Rows[indexCount]["StatementThru"],
                                typeof (DateTime)),
                        ClaimTotal =
                             DBNull.Value ==
                                        claimAdjudicationReportData.Rows[indexCount]["ClaimTotalCharges"]
                                            ? (double?)null
                                            : Math.Round(
                                                double.Parse(
                                                    claimAdjudicationReportData.Rows[indexCount]["ClaimTotalCharges"
                                                        ].ToString()), 2),
                        CalculatedAdjustment =
                            DBNull.Value ==
                                        claimAdjudicationReportData.Rows[indexCount]["CalculatedAdj"]
                                            ? (double?)null
                                            : Math.Round(
                                                double.Parse(
                                                    claimAdjudicationReportData.Rows[indexCount]["CalculatedAdj"]
                                                        .ToString()), 2),
                        ActualPayment = DBNull.Value == claimAdjudicationReportData.Rows[indexCount]["ActualPmt"]
                                            ? (double?)null
                                            : Math.Round(
                                                double.Parse(
                                                    claimAdjudicationReportData.Rows[indexCount]["ActualPmt"]
                                                        .ToString()), 2),
                        Reimbursement =
                            GetValue<double?>(claimAdjudicationReportData.Rows[indexCount]["ReimbursementAmount"],
                                typeof (double)),
                        RevCode = GetStringValue(claimAdjudicationReportData.Rows[indexCount]["RevCode"]),
                        HcpcsCode = GetStringValue(claimAdjudicationReportData.Rows[indexCount]["HCPCSCode"]),
                        ServiceFromDate =
                            DBNull.Value ==
                            claimAdjudicationReportData.Rows[indexCount]["ServiceFromDate"]
                                ? new DateTime()
                                : GetValue<DateTime>(claimAdjudicationReportData.Rows[indexCount]["ServiceFromDate"],
                                    typeof (DateTime)),
                        ServiceThruDate =
                            DBNull.Value ==
                            claimAdjudicationReportData.Rows[indexCount]["ServiceThruDate"]
                                ? new DateTime()
                                : GetValue<DateTime>(claimAdjudicationReportData.Rows[indexCount]["ServiceThruDate"],
                                    typeof (DateTime)),
                        ServUnits = GetValue<int?>(claimAdjudicationReportData.Rows[indexCount]["Units"], typeof (int)),
                        ServiceType = GetStringValue(claimAdjudicationReportData.Rows[indexCount]["ServiceType"]),
                        CodeSelection =
                            DBNull.Value ==
                            claimAdjudicationReportData.Rows[indexCount]["CodeSelection"]
                                ? string.Empty
                                : Convert.ToString(
                                    claimAdjudicationReportData.Rows[indexCount]["CodeSelection"])
                                    .Replace(Environment.NewLine, string.Empty),
                        Payment =
                            DBNull.Value == claimAdjudicationReportData.Rows[indexCount]["PaymentType"]
                                ? string.Empty
                                : Convert.ToString(
                                    claimAdjudicationReportData.Rows[indexCount]["PaymentType"])
                                    .Replace(Environment.NewLine, string.Empty),
                        AdjudicationStatus =
                            GetStringValue(claimAdjudicationReportData.Rows[indexCount]["AdjudicationStatus"]),
                        IsClaimChargeData =
                            GetValue<bool>(claimAdjudicationReportData.Rows[indexCount]["IsClaimChargeData"],
                                typeof (bool)),
                        IsFormulaError =
                            GetValue<bool>(claimAdjudicationReportData.Rows[indexCount]["IsFormulaError"], typeof (bool)),
                        PatientResponsibility =
                            DBNull.Value == claimAdjudicationReportData.Rows[indexCount]["PatientResponsibility"]
                                            ? (double?)null
                                            : Math.Round(
                                                double.Parse(
                                                    claimAdjudicationReportData.Rows[indexCount]["PatientResponsibility"]
                                                        .ToString()), 2),
                        MedicareSequesterAmount =
                            DBNull.Value == claimAdjudicationReportData.Rows[indexCount]["MedicareSequesterAmount"]
                                ? (double?) null
                                : Math.Round(
                                    double.Parse(
                                        claimAdjudicationReportData.Rows[indexCount]["MedicareSequesterAmount"]
                                            .ToString()), 3),
                        ContractServiceTypeId =
                            GetValue<long>(claimAdjudicationReportData.Rows[indexCount]["ContractServiceTypeID"],
                                typeof (long)),
                        HcpcsModifier =
                            Regex.Replace(
                                GetStringValue(claimAdjudicationReportData.Rows[indexCount]["HCPCSModifier"]),
                                Constants.HcpcsModifierPattern, Constants.WhiteSpace),
                        PlaceOfService = GetStringValue(claimAdjudicationReportData.Rows[indexCount]["PlaceOfService"]),
                        ClaimType = GetStringValue(claimAdjudicationReportData.Rows[indexCount]["ClaimType"]),
                        ActualAdjustment = DBNull.Value ==
                                        claimAdjudicationReportData.Rows[indexCount]["ActulAdj"]
                                            ? (double?)null
                                            : Math.Round(
                                                double.Parse(
                                                    claimAdjudicationReportData.Rows[indexCount]["ActulAdj"]
                                                        .ToString()), 2),
                       ContractualVariance =
                            DBNull.Value ==
                                       claimAdjudicationReportData.Rows[indexCount]["ContractualVariance"]
                                           ? (double?)null
                                           : Math.Round(
                                               double.Parse(
                                                   claimAdjudicationReportData.Rows[indexCount]["ContractualVariance"]
                                                       .ToString()), 2),
                        PaymentVariance = DBNull.Value ==
                                       claimAdjudicationReportData.Rows[indexCount]["PaymentVariance"]
                                           ? (double?)null
                                           : Math.Round(
                                               double.Parse(
                                                   claimAdjudicationReportData.Rows[indexCount]["PaymentVariance"]
                                                       .ToString()), 2)
                }));
        }

        /// <summary>
        /// Get claims by request name from procedure GetClaimsByRequestName
        /// </summary>
        /// <param name="claimAdjudicationReport"></param>
        /// <param name="maxRecordLimitForExcelReport"></param>
        /// <returns></returns>
        private DataSet GetClaimsByRequestName(ClaimAdjudicationReport claimAdjudicationReport,
            int maxRecordLimitForExcelReport)
        {
            string filterSetting = string.Empty;
            //Checks for Payers, if payers exists stores it in DB
            if (claimAdjudicationReport.PageSetting != null &&
                claimAdjudicationReport.PageSetting.SearchCriteriaList != null &&
                claimAdjudicationReport.PageSetting.SearchCriteriaList.Any())
            {
                filterSetting = claimAdjudicationReport.XmlSerialize();
            }

            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetClaimsByRequestName");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommandObj, "@SelectCriteria ", DbType.String,
                claimAdjudicationReport.ClaimSearchCriteria);

            _databaseObj.AddInParameter(_databaseCommandObj, "@ModelId", DbType.Int64,
                claimAdjudicationReport.ModelId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@DateType", DbType.Int32,
                claimAdjudicationReport.DateType);
            _databaseObj.AddInParameter(_databaseCommandObj, "@StartDate", DbType.DateTime,
                claimAdjudicationReport.StartDate);
            _databaseObj.AddInParameter(_databaseCommandObj, "@EndDate", DbType.DateTime,
                claimAdjudicationReport.EndDate);

            _databaseObj.AddInParameter(_databaseCommandObj, "@MaxRecordLimit", DbType.Int32,
                maxRecordLimitForExcelReport);
            _databaseObj.AddInParameter(_databaseCommandObj, "@IsSelectClaims", DbType.Boolean,
                claimAdjudicationReport.IsSelectClaims);

            //Added RequestedUserID and RequestedUserName with reference to HIPAA logging feature
            _databaseObj.AddInParameter(_databaseCommandObj, "@RequestedUserID", DbType.String,
                claimAdjudicationReport.RequestedUserId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@RequestedUserName", DbType.String,
                claimAdjudicationReport.RequestedUserName);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserId", DbType.Int32, claimAdjudicationReport.UserId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@Take", DbType.Int32,
                claimAdjudicationReport.PageSetting != null
                    ? claimAdjudicationReport.PageSetting.Take
                    : 50);
            _databaseObj.AddInParameter(_databaseCommandObj, "@Skip", DbType.Int32,
                claimAdjudicationReport.PageSetting != null
                    ? claimAdjudicationReport.PageSetting.Skip
                    : 0);
            _databaseObj.AddInParameter(_databaseCommandObj, "@SortField", DbType.String,
                claimAdjudicationReport.PageSetting != null
                    ? claimAdjudicationReport.PageSetting.SortField
                    : string.Empty);
            _databaseObj.AddInParameter(_databaseCommandObj, "@SortDirection", DbType.String,
                claimAdjudicationReport.PageSetting != null
                    ? claimAdjudicationReport.PageSetting.SortDirection
                    : string.Empty);
            _databaseObj.AddInParameter(_databaseCommandObj, "@FilterSearchCriteria", DbType.Xml, filterSetting);

            // Retrieve the results of the Stored Procedure in Datatable
            _databaseCommandObj.CommandTimeout = claimAdjudicationReport.CommandTimeoutForClaimAdjudication;
            DataSet claimAdjudicationReportDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);
            return claimAdjudicationReportDataSet;
        }

        /// <summary>
        /// Set column data of type string
        /// </summary>
        /// <param name="claimDataRow"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string SetColumnData(DataRow claimDataRow, string column, string value)
        {
            return IsColumnExists(claimDataRow, column)
                ? GetStringValue(claimDataRow["" + column + ""])
                : value;
        }
        /// <summary>
        /// Set column data of type DateTime
        /// </summary>
        /// <param name="claimDataRow"></param>
        /// <param name="columnName"></param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        private static string SetDateColumnValue(DataRow claimDataRow, string columnName, string currentDateTime)
        {
            return
                    IsColumnExists(claimDataRow, columnName)
                        ? DBNull.Value == claimDataRow[columnName]
                            ? null
                            : Convert.ToString(claimDataRow[columnName].ToString() == Constants.DateTime1900
                                ? null
                                : Utilities.GetLocalTimeString(currentDateTime,
                                    Convert.ToDateTime(claimDataRow[columnName])))
                        : null;
        }

        /// <summary>
        /// Set column data of type DateTime
        /// </summary>
        /// <param name="claimDataRow"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private static string SetDateColumn(DataRow claimDataRow, string columnName)
        {
            return
                IsColumnExists(claimDataRow, columnName)
                    ? (DBNull.Value == claimDataRow[columnName]
                        ? null
                        : Convert.ToString(claimDataRow[columnName].ToString() ==
                                           Constants.DateTime1900
                            ? null
                            : Convert.ToDateTime(claimDataRow[columnName])
                                .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)))
                    : null;
        }

        /// <summary>
        /// Set claim columns
        /// </summary>
        /// <param name="claimDataRow"></param>
        /// <param name="claimAdjudicationReport"></param>
        /// <param name="claimDataList"></param>
        private static void SetClaimColumns(DataRow claimDataRow, ClaimAdjudicationReport claimAdjudicationReport,
            List<EvaluateableClaim> claimDataList)
        {
            EvaluateableClaim claimData = new EvaluateableClaim
            {
                PatientAccountNumber = SetColumnData(claimDataRow, "PatientAccountNumber", string.Empty),
                AdjudicatedDateValue = SetDateColumnValue(claimDataRow, "AdjudicatedDate", claimAdjudicationReport.CurrentDateTime),
                Ssinumber =
                   IsColumnExists(claimDataRow, "SSINumber")
                        ? GetValue<int?>(claimDataRow["SSINumber"], typeof(int))
                        : null,
                ClaimType = SetColumnData(claimDataRow, "ClaimType", string.Empty),
                ClaimState = SetColumnData(claimDataRow, "ClaimState", string.Empty),
                PayerSequence =
                   IsColumnExists(claimDataRow, "PayerSequence")
                        ? GetValue<int?>(claimDataRow["PayerSequence"], typeof(int))
                        : null,
                ClaimTotal =
                   IsColumnExists(claimDataRow, "ClaimTotal")
                        ? GetValue<double?>(claimDataRow["ClaimTotal"], typeof(double))
                        : null,
                StatementFromValue = SetDateColumn(claimDataRow, "StatementFrom"),
                StatementThruValue = SetDateColumn(claimDataRow, "StatementThru"),
                ClaimDateValue = SetDateColumn(claimDataRow, "ClaimDate"),
                BillDateValue = SetDateColumn(claimDataRow, "BillDate"),
                LastFiledDateValue = SetDateColumn(claimDataRow, "LastFiledDate"),
                BillType = SetColumnData(claimDataRow, "BillType", null),
                Drg = SetColumnData(claimDataRow, "DRG", null),
                PriIcdpCode = SetColumnData(claimDataRow, "PriICDPCode", null),
                PriIcddCode = SetColumnData(claimDataRow, "PriICDDCode", null),
                PriPayerName = SetColumnData(claimDataRow, "PriPayerName", null),
                SecPayerName = SetColumnData(claimDataRow, "SecPayerName", null),
                TerPayerName = SetColumnData(claimDataRow, "TerPayerName", null),
                IsRemitLinked = SetColumnData(claimDataRow, "IsRemitLinked", null),
                ClaimStat = SetColumnData(claimDataRow, "ClaimStatus", null),
                AdjudicatedValue =
                   IsColumnExists(claimDataRow, "AdjudicatedValue")
                        ? GetValue<double?>(claimDataRow["AdjudicatedValue"], typeof(double))
                        : null,
                ActualPayment =
                   IsColumnExists(claimDataRow, "ActualPayment")
                        ? GetValue<double?>(claimDataRow["ActualPayment"], typeof(double))
                        : null,
                PatientResponsibility =
                   IsColumnExists(claimDataRow, "PatientResponsibility")
                        ? GetValue<double?>(claimDataRow["PatientResponsibility"], typeof(double))
                        : null,
                RemitAllowedAmt =
                   IsColumnExists(claimDataRow, "RemitAllowedAmt")
                        ? GetValue<double?>(claimDataRow["RemitAllowedAmt"], typeof(double))
                        : null,
                RemitNonCovered =
                   IsColumnExists(claimDataRow, "RemitNonCovered")
                        ? GetValue<double?>(claimDataRow["RemitNonCovered"], typeof(double))
                        : null,
                Icn = SetColumnData(claimDataRow, "ICN", null),
                ClaimLink =
                   IsColumnExists(claimDataRow, "ClaimLink")
                        ? GetValue<long?>(claimDataRow["ClaimLink"], typeof(long))
                        : null,
                LinkedRemitId = SetColumnData(claimDataRow, "LinkedRemitID", null),
                ClaimId =
                   IsColumnExists(claimDataRow, "ClaimId")
                        ? DBNull.Value == claimDataRow["ClaimId"]
                            ? 0
                            : GetValue<long>(claimDataRow["ClaimId"], typeof(long))
                        : 0,
                ContractualVariance =
                   IsColumnExists(claimDataRow, "ContractualVariance")
                        ? GetValue<double?>(claimDataRow["ContractualVariance"], typeof(double))
                        : null,
                PaymentVariance =
                   IsColumnExists(claimDataRow, "PaymentVariance")
                        ? GetValue<double?>(claimDataRow["PaymentVariance"], typeof(double))
                        : null,
                ActualAdjustment =
                   IsColumnExists(claimDataRow, "ActualContractualAdjustment")
                        ? GetValue<double?>(claimDataRow["ActualContractualAdjustment"], typeof(double))
                        : null,
                CalculatedAdjustment =
                   IsColumnExists(claimDataRow, "ExpectedContractualAdjustment")
                        ? GetValue<double?>(claimDataRow["ExpectedContractualAdjustment"],
                            typeof(double))
                        : null,
                DischargeStatus = SetColumnData(claimDataRow, "DischargeStatus", null),
                CustomField1 = SetColumnData(claimDataRow, "CustomField1", null),
                CustomField2 = SetColumnData(claimDataRow, "CustomField2", null),
                CustomField3 = SetColumnData(claimDataRow, "CustomField3", null),
                CustomField4 = SetColumnData(claimDataRow, "CustomField4", null),
                CustomField5 = SetColumnData(claimDataRow, "CustomField5", null),
                CustomField6 = SetColumnData(claimDataRow, "CustomField6", null),
                Npi = SetColumnData(claimDataRow, "NPI", null),
                MemberId = SetColumnData(claimDataRow, "MemberID", null),
                Mrn = SetColumnData(claimDataRow, "MRN", null),
                IsReviewed = IsColumnExists(claimDataRow, "IsReviewed") &&
                                GetValue<bool>(claimDataRow["IsReviewed"], typeof(bool)),
                IsAllReviewed = IsColumnExists(claimDataRow, "IsAllReviewed") &&
                                GetValue<bool>(claimDataRow["IsAllReviewed"], typeof(bool)),
                Los =
                   IsColumnExists(claimDataRow, "Los")
                        ? GetValue<int>(claimDataRow["Los"], typeof(int))
                        : 0,
                Age =
                   IsColumnExists(claimDataRow, "Age")
                        ? GetValue<byte>(claimDataRow["Age"], typeof(byte))
                        : default(byte),
                CheckDate = SetDateColumn(claimDataRow, "CheckDate"),
                CheckNumber = IsColumnExists(claimDataRow, "CheckNumber")
                ? GetValue<string>(claimDataRow["CheckNumber"], typeof(string))
                 : default(string),
                AdjudicatedContractName =
                    IsColumnExists(claimDataRow, "AdjudicatedContractName")
                         ? GetStringValue(claimDataRow["AdjudicatedContractName"])
                         : null,
                InsuredsGroupNumber =
           IsColumnExists(claimDataRow, "InsuredsGroupNumber")
                ? GetStringValue(claimDataRow["InsuredsGroupNumber"])
                : null
            };
            claimDataList.Add(claimData);
        }

        /// <summary>
        /// Gets the selected claim.
        /// </summary>
        /// <param name="claimAdjudicationReport">The Claim Adjudication Report.</param>
        /// <param name="maxRecordLimitForExcelReport">The maximum record limit for excel report.</param>
        /// <returns></returns>
        //FIXED-2016-R3-S2: Method body is too complex. Can decompose it to multiple functions.
        public ClaimAdjudicationReport GetSelectedClaim(ClaimAdjudicationReport claimAdjudicationReport, int maxRecordLimitForExcelReport)
        {
            ClaimAdjudicationReport adjudicationReport = new ClaimAdjudicationReport();
            if (claimAdjudicationReport != null)
            {
                DataSet claimAdjudicationReportDataSet = GetClaimsByRequestName(claimAdjudicationReport,
                    maxRecordLimitForExcelReport);
                if (claimAdjudicationReportDataSet.IsTableDataPopulated())
                {
                    if (claimAdjudicationReportDataSet.Tables[0].Columns.Count == 2)
                    {
                        if (claimAdjudicationReportDataSet.Tables[0].Columns.Contains("CountThreshold"))
                        {
                            adjudicationReport.ReportThreshold =
                                GetValue<int>(claimAdjudicationReportDataSet.Tables[0].Rows[0]["CountThreshold"],
                                    typeof(int));
                            return adjudicationReport;
                        }
                    }

                    if (claimAdjudicationReportDataSet.Tables.Count > 1 &&
                        claimAdjudicationReportDataSet.Tables[1].Rows.Count > 0)
                    {
                        if (claimAdjudicationReportDataSet.Tables[1].Columns.Contains("OpenClaimCount"))
                        {
                            adjudicationReport.TotalRecords =
                                GetValue<int>(claimAdjudicationReportDataSet.Tables[1].Rows[0][0], typeof(int));
                        }
                    }

                    List<string> columnList = new List<string>();
                    if (claimAdjudicationReportDataSet.Tables.Count > 2)
                    {
                        if (claimAdjudicationReportDataSet.Tables[2].Columns.Contains("ColumnName"))
                        {
                            DataRow[] columnDataRows = claimAdjudicationReportDataSet.Tables[2].Select();
                            //FIXED-2016-R3-S2: Convert loop to Linq query
                            columnList.AddRange(
                                columnDataRows.Select(columnDataRow => GetStringValue(columnDataRow["ColumnName"])));
                        }
                    }
                    adjudicationReport.ColumnNames = columnList;

                    List<EvaluateableClaim> claimDataList = new List<EvaluateableClaim>();
                    DataRowCollection claimDataRowCollection = claimAdjudicationReportDataSet.Tables[0].Rows;
                    if (claimAdjudicationReportDataSet.Tables[0].Columns.Contains("IsReviewed"))
                    {
                        for (int indexCount = 0; indexCount < claimDataRowCollection.Count; indexCount++)
                        {
                            DataRow claimDataRow = claimDataRowCollection[indexCount];
                            SetClaimColumns(claimDataRow, claimAdjudicationReport, claimDataList);
                        }
                    }
                    adjudicationReport.ClaimData = claimDataList;
                }
            }
            return adjudicationReport;
        }

        /// <summary>
        /// Gets Open Claim Columns Names By UserId
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public ClaimAdjudicationReport GetOpenClaimColumnNamesBasedOnUserId(ClaimAdjudicationReport data)
        {
            ClaimAdjudicationReport adjudicationReport = new ClaimAdjudicationReport();
            if (data != null)
            {
                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetOpenClaimColumnNamesBasedOnUserId");
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserId", DbType.Int32, data.UserId);
                // Retrieve the results of the Stored Procedure 
                DataSet claimAdjudicationReportDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

                if (claimAdjudicationReportDataSet != null)
                {
                    if (claimAdjudicationReportDataSet.Tables.Count > 0)
                    {
                        List<string> columnNames = new List<string>();
                        DataRow[] columnDataRows = claimAdjudicationReportDataSet.Tables[0].Select();
                        columnNames.AddRange(
                            columnDataRows.Select(columnDataRow => GetStringValue(columnDataRow["ColumnName"])));
                        adjudicationReport.ColumnNames = columnNames;
                    }
                }
            }
            return adjudicationReport;
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
