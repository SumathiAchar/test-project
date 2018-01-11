using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    public class BaseRepository
    {
        /// <summary>
        /// Updates the property and operand.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        protected static ICondition UpdatePropertyAndOperand(ICondition condition)
        {
            if (condition.OperandIdentifier != null)
                switch ((Enums.OperandIdentifier)condition.OperandIdentifier)
                {
                    case Enums.OperandIdentifier.PatientAccountNumber:
                        condition.PropertyColumnName = Constants.PropertyPatAcctNum;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.BillType:
                        condition.PropertyColumnName = Constants.PropertyBillType;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.RevCode:
                        condition.PropertyColumnName = Constants.PropertyRevCode;
                        condition.OperandType = (byte)Enums.OperandType.Numeric;
                        break;
                    case Enums.OperandIdentifier.HcpcsCode:
                        condition.PropertyColumnName = Constants.PropertyHcpcsCodeWithModifier;
                        
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.PayerName:
                        condition.PropertyColumnName = Constants.PropertyPriPayerName;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.InsuredId:
                        condition.PropertyColumnName = Constants.PropertyInsuredId;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.Drg:
                        condition.PropertyColumnName = Constants.PropertyDrg;
                        condition.OperandType = (byte)Enums.OperandType.Numeric;
                        break;
                    case Enums.OperandIdentifier.PlaceOfService:
                        condition.PropertyColumnName = Constants.PropertyPlaceOfService;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.ReferringPhysician:
                        condition.PropertyColumnName = Constants.PropertyReferringPhysician;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.RenderingPhysician:
                        condition.PropertyColumnName = Constants.PropertyRenderingPhysician;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.IcdDiagnosis:
                        condition.PropertyColumnName = Constants.PropertyIcddCode;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.IcdProcedure:
                        condition.PropertyColumnName = Constants.PropertyIcdpCode;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.AttendingPhysician:
                        condition.PropertyColumnName = Constants.PropertyAttendingPhysician;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.TotalCharges:
                        condition.PropertyColumnName = Constants.PropertyTotalCharges;
                        condition.OperandType = (byte)Enums.OperandType.Numeric;
                        break;
                    case Enums.OperandIdentifier.StatementCoversPeriodToDatesOfService:
                        condition.PropertyColumnName = Constants.PropertyStatementCoversPeriod;
                        condition.OperandType = (byte)Enums.OperandType.Date;
                        break;
                    case Enums.OperandIdentifier.ValueCodes:
                        condition.PropertyColumnName = Constants.PropertyValueCode;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.OccurrenceCode:
                        condition.PropertyColumnName = Constants.PropertyOccurenceCode;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.ConditionCodes:
                        condition.PropertyColumnName = Constants.PropertyConditionCode;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.InsuredGroup:
                        condition.PropertyColumnName = Constants.PropertyInsuredGroup;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.CustomField1:
                        condition.PropertyColumnName = Constants.PropertyCustomField1;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.CustomField2:
                        condition.PropertyColumnName = Constants.PropertyCustomField2;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.CustomField3:
                        condition.PropertyColumnName = Constants.PropertyCustomField3;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.CustomField4:
                        condition.PropertyColumnName = Constants.PropertyCustomField4;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.CustomField5:
                        condition.PropertyColumnName = Constants.PropertyCustomField5;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.CustomField6:
                        condition.PropertyColumnName = Constants.PropertyCustomField6;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.Npi:
                        condition.PropertyColumnName = Constants.PropertyNpi;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.ClaimState:
                        condition.PropertyColumnName = Constants.PropertyClaimState;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.DischargeStatus:
                        condition.PropertyColumnName = Constants.PropertyDischargeStatus;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.Icn:
                        condition.PropertyColumnName = Constants.PropertyIcn;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.Mrn:
                        condition.PropertyColumnName = Constants.PropertyMrn;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.Los:
                        condition.PropertyColumnName = Constants.PropertyLos;
                        condition.OperandType = (byte)Enums.OperandType.Numeric;
                        break;
                    case Enums.OperandIdentifier.Age:
                        condition.PropertyColumnName = Constants.PropertyAge;
                        condition.OperandType = (byte)Enums.OperandType.Numeric;
                        break;
                    case Enums.OperandIdentifier.CheckDate:
                        condition.PropertyColumnName = Constants.PropertyCheckDate;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                    case Enums.OperandIdentifier.CheckNumber:
                        condition.PropertyColumnName = Constants.PropertyCheckNumber;
                        condition.OperandType = (byte)Enums.OperandType.AlphaNumeric;
                        break;
                }

            return condition;
        }

        /// <summary>
        /// Gets the base conditions.
        /// </summary>
        /// <param name="servicetypeConditions">The service type conditions.</param>
        /// <param name="paymentTypes">The payment type list.</param>
        protected static void GetBaseConditions(List<ICondition> servicetypeConditions, IEnumerable<PaymentTypeBase> paymentTypes)
        {
            Dictionary<Enums.PaymentTypeCodes, bool> lstIsPaymentDecoupled =
           new Dictionary<Enums.PaymentTypeCodes, bool>
            {
                {Enums.PaymentTypeCodes.StopLoss, true},
                {Enums.PaymentTypeCodes.LesserOf, false},
                {Enums.PaymentTypeCodes.MedicareIp, false},
                {Enums.PaymentTypeCodes.AscFeeSchedule, false},
                {Enums.PaymentTypeCodes.Cap, false},
                {Enums.PaymentTypeCodes.DrgPayment, false},
                {Enums.PaymentTypeCodes.FeeSchedule, false},
                {Enums.PaymentTypeCodes.MedicareLabFeeSchedule, false},
                {Enums.PaymentTypeCodes.MedicareOp, false},
                {Enums.PaymentTypeCodes.PerCase, false},
                {Enums.PaymentTypeCodes.PerDiem, false},
                {Enums.PaymentTypeCodes.PerVisit, false},
                {Enums.PaymentTypeCodes.PercentageDiscountPayment, false},
                {Enums.PaymentTypeCodes.CustomTableFormulas, true},
                {Enums.PaymentTypeCodes.MedicareSequester, false},
            };
            foreach (var paymenttype in paymentTypes.Where(paymenttype => !(lstIsPaymentDecoupled[(Enums.PaymentTypeCodes)paymenttype.PaymentTypeId])))
            {
                if (paymenttype.Conditions != null)
                    paymenttype.Conditions.RemoveAll(
                        x =>
                            x.OperandIdentifier == (int)Enums.OperandIdentifier.RevCode ||
                            x.OperandIdentifier == (int)Enums.OperandIdentifier.HcpcsCode ||
                            x.OperandIdentifier == (int)Enums.OperandIdentifier.PlaceOfService);
                else
                    paymenttype.Conditions = new List<ICondition>();

                if (servicetypeConditions != null)
                {
                    var conds =
                        servicetypeConditions.FindAll(
                            x => x.OperandIdentifier == (int)Enums.OperandIdentifier.RevCode ||
                                 x.OperandIdentifier == (int)Enums.OperandIdentifier.HcpcsCode ||
                                 x.OperandIdentifier == (int)Enums.OperandIdentifier.PlaceOfService);
                    paymenttype.Conditions.AddRange(conds);
                }
            }
        }

        /// <summary>
        /// Gets the service line condition.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="isExcludedCode">if set to <c>true</c> [is excluded code].</param>
        /// <returns></returns>
        private static ICondition GetServiceLineCondition(DataRow row, bool isExcludedCode)
        {
            if (row != null)
            {
                ICondition condition = new Condition();

                switch ((Enums.ServiceLineCodes)Convert.ToInt64(row["ServiceLineTypeID"]))
                {
                    case Enums.ServiceLineCodes.BillType:
                        condition.OperandIdentifier = (byte)Enums.OperandIdentifier.BillType;
                        break;
                    case Enums.ServiceLineCodes.Cpt:
                        condition.OperandIdentifier = (byte)Enums.OperandIdentifier.HcpcsCode;
                        break;
                    case Enums.ServiceLineCodes.RevenueCode:
                        condition.OperandIdentifier = (byte)Enums.OperandIdentifier.RevCode;
                        break;
                    case Enums.ServiceLineCodes.Drg:
                        condition.OperandIdentifier = (byte)Enums.OperandIdentifier.Drg;
                        break;
                    case Enums.ServiceLineCodes.DiagnosisCode:
                        condition.OperandIdentifier = (byte)Enums.OperandIdentifier.IcdDiagnosis;
                        break;
                    case Enums.ServiceLineCodes.ProcedureCode:
                        condition.OperandIdentifier = (byte)Enums.OperandIdentifier.IcdProcedure;
                        break;
                }

                //Get Property Column name as well as OperandType based on OperandIdentifier
                condition = UpdatePropertyAndOperand(condition);

                if (isExcludedCode)
                {
                    condition.ConditionOperator = (byte)Enums.ConditionOperation.NotEqualTo;
                    condition.RightOperand = Convert.ToString(row["ExcludedCode"]);
                }
                else
                {
                    condition.ConditionOperator = (byte)Enums.ConditionOperation.EqualTo;
                    condition.RightOperand = Convert.ToString(row["IncludedCode"]);
                }

                return condition;
            }
            return null;
        }

        /// <summary>
        /// Gets the claim field document.
        /// </summary>
        /// <param name="claimFieldDocId">The claim field document identifier.</param>
        /// <param name="dtDoc">The dt document.</param>
        /// <param name="dtDocValues">The dt document values.</param>
        /// <returns></returns>
        protected static ClaimFieldDoc GetClaimFieldDoc(long claimFieldDocId, DataTable dtDoc, DataTable dtDocValues)
        {
            ClaimFieldDoc claimFieldDoc = new ClaimFieldDoc();
            if (dtDoc != null && dtDoc.Rows.Count > 0)
            {
                claimFieldDoc = (from DataRow row in dtDoc.Rows
                                 where Convert.ToInt64(row["ClaimFieldDocID"]) == claimFieldDocId
                                 select new ClaimFieldDoc
                                 {
                                     ClaimFieldDocId = Convert.ToInt64(row["ClaimFieldDocID"]),
                                     FileName = DBNull.Value == row["FileName"] ? null : Convert.ToString(row["FileName"]),
                                     TableName = DBNull.Value == row["TableName"]
                                         ? null
                                         : Convert.ToString(row["TableName"]),
                                     ColumnHeaderFirst = DBNull.Value == row["ColumnHeaderFirst"]
                                         ? null
                                         : Convert.ToInt64(row["ClaimFieldID"]) == (byte)Enums.ClaimFieldTypes.CustomPaymentType
                                             ? Convert.ToString(row["ColumnHeaderFirst"])
                                                 .Substring(0,
                                                     Convert.ToString(row["ColumnHeaderFirst"])
                                                         .IndexOf(",", StringComparison.Ordinal))
                                             : Convert.ToString(row["ColumnHeaderFirst"]),
                                     ColumnHeaderSecond = Convert.ToInt64(row["ClaimFieldID"]) == (byte)Enums.ClaimFieldTypes.CustomPaymentType
                                         ? DBNull.Value == row["ColumnHeaderFirst"]
                                             ? null
                                             : Convert.ToString(row["ColumnHeaderFirst"])
                                                 .Substring(
                                                     Convert.ToString(row["ColumnHeaderFirst"])
                                                         .IndexOf(Constants.Comma, StringComparison.Ordinal) + 1,
                                                     Convert.ToString(row["ColumnHeaderFirst"]).Length -
                                                     Convert.ToString(row["ColumnHeaderFirst"])
                                                         .IndexOf(Constants.Comma, StringComparison.Ordinal) - 1)
                                         : DBNull.Value == row["ColumnHeaderSecond"]
                                             ? null
                                             : Convert.ToString(row["ColumnHeaderSecond"]),
                                     ClaimFieldId = DBNull.Value == row["ClaimFieldID"]
                                         ? (long?)null
                                         : Convert.ToInt64(row["ClaimFieldID"]),
                                     ClaimFieldValues = GetClaimFieldValues(Convert.ToInt64(row["ClaimFieldDocID"]),
                                         dtDocValues, Convert.ToInt64(row["ClaimFieldID"]))
                                 }).FirstOrDefault();
            }

            return claimFieldDoc;
        }

        /// <summary>
        /// Gets the claim field values.
        /// </summary>
        /// <param name="claimFieldDocId">The claim field document identifier.</param>
        /// <param name="dtDocValues">The dt document values.</param>
        /// <param name="claimFieldId">claim field id</param>
        /// <returns></returns>
        private static List<ClaimFieldValue> GetClaimFieldValues(long claimFieldDocId, DataTable dtDocValues, long claimFieldId)
        {
            List<ClaimFieldValue> claimFieldValues = new List<ClaimFieldValue>();

            if (dtDocValues != null && dtDocValues.Rows.Count > 0)
            {
                if (claimFieldId == (byte)Enums.ClaimFieldTypes.CustomPaymentType)
                {
                    var dataRows = (from DataRow row in dtDocValues.Rows
                                    where Convert.ToInt64(row["ClaimFieldDocID"]) == claimFieldDocId
                                    select row).ToList();
                    string values =
                        dataRows.AsEnumerable()
                            .OrderBy(a => a.Field<long>("ClaimFieldValueID"))
                            .Aggregate(string.Empty,
                                (current, row) =>
                                    current +
                                    (DBNull.Value == row["Value"] ? string.Empty : Convert.ToString(row["Value"])));
                    if (values.EndsWith(Constants.NewLine))
                        values = values.Trim().Substring(0, values.Length - 1);
                    if (!string.IsNullOrEmpty(values))
                    {
                        string[] splitedValues = Regex.Split(values, Constants.NewLine);
                        claimFieldValues.AddRange(splitedValues.Select(line =>
                        {
                            var dataRow = dataRows.FirstOrDefault();
                            return dataRow != null
                                ? new ClaimFieldValue
                                {
                                    ClaimFieldDocId = Convert.ToInt64(dataRow["ClaimFieldDocID"]),
                                    Identifier =
                                        line.Substring(0, line.IndexOf(Constants.Comma, StringComparison.Ordinal))
                                            .Replace(Constants.Apostrophe + Constants.Apostrophe,
                                                Constants.Apostrophe)
                                            .Replace(Constants.MaskCommaInValue, Constants.Comma)
                                            .Replace(Constants.AmpercentReplaceString,
                                                Constants.AmpercentString)
                                            .Replace(Constants.LessThanReplaceString,
                                                Constants.LessThanString)
                                            .Replace(Constants.ReplaceCommaInString,
                                                Constants.Comma),
                                    Value =
                                        line.Substring(line.IndexOf(Constants.Comma, StringComparison.Ordinal) + 1,
                                            line.Length - line.IndexOf(Constants.Comma, StringComparison.Ordinal) -
                                            1)
                                }
                                : null;
                        }));
                    }
                }
                else
                {
                    claimFieldValues = (from DataRow row in dtDocValues.Rows
                                        where Convert.ToInt64(row["ClaimFieldDocID"]) == claimFieldDocId
                                        select new ClaimFieldValue
                                        {
                                            ClaimFieldDocId = Convert.ToInt64(row["ClaimFieldDocID"]),
                                            Identifier = DBNull.Value == row["Identifier"] ? null
                                                : claimFieldId == (byte)Enums.ClaimFieldTypes.CustomPaymentType
                                                ? Convert.ToString(row["Identifier"])
                                                    .Substring(0,
                                                        Convert.ToString(row["Identifier"])
                                                            .IndexOf(Constants.NewLine, StringComparison.Ordinal) + 1)
                                                : Convert.ToString(row["Identifier"]),
                                            Value = DBNull.Value == row["Value"] ? null
                                            : claimFieldId == (byte)Enums.ClaimFieldTypes.CustomPaymentType
                                            ? Convert.ToString(row["Identifier"])
                                                .Substring(
                                                    Convert.ToString(row["Identifier"])
                                                        .IndexOf(Constants.NewLine, StringComparison.Ordinal) + 1,
                                                    Convert.ToString(row["Identifier"]).Length -
                                                    Convert.ToString(row["Identifier"])
                                                        .IndexOf(Constants.NewLine, StringComparison.Ordinal) + 1)
                                            : Convert.ToString(row["Value"])
                                        }).ToList();
                }
            }

            return claimFieldValues;
        }

        /// <summary>
        /// Gets the selection conditions.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        protected static List<ICondition> GetSelectionConditions(long? contractId, long? contractServiceTypeId, DataSet dataSet)
        {
            List<ICondition> conditions = new List<ICondition>();

            //Add Service Line selection data into Conditions
            if (dataSet.Tables[3] != null && dataSet.Tables[3].Rows != null && dataSet.Tables[3].Rows.Count > 0)
                conditions.AddRange(GetServiceLineConditions(contractId, contractServiceTypeId, dataSet.Tables[3]));

            //Add ClaimField selection data into Conditions
            if (dataSet.Tables[4] != null && dataSet.Tables[4].Rows != null && dataSet.Tables[4].Rows.Count > 0)
                conditions.AddRange(GetClaimFieldConditions(contractId, contractServiceTypeId, dataSet.Tables[4]));

            //Add Table selection data into Conditions
            if (dataSet.Tables[5] != null && dataSet.Tables[5].Rows != null && dataSet.Tables[5].Rows.Count > 0)
                conditions.AddRange(GetTableSelectionConditions(contractId, contractServiceTypeId, dataSet.Tables[5], dataSet.Tables[18]));

            return conditions;
        }

        /// <summary>
        /// Gets the service line conditions.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dataTable">The data table.</param>
        /// <returns></returns>
        private static IEnumerable<ICondition> GetServiceLineConditions(long? contractId, long? contractServiceTypeId, DataTable dataTable)
        {
            List<ICondition> conditions = null;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                //Initialize condition list
                conditions = new List<ICondition>();

                foreach (DataRow row in dataTable.Rows.Cast<DataRow>()
                            .Where(
                                currentRow =>
                                    (contractId.HasValue && currentRow["ContractId"] != DBNull.Value &&
                                     Convert.ToInt64(currentRow["ContractId"]) == contractId) ||
                                    (contractServiceTypeId.HasValue &&
                                     currentRow["ContractServiceTypeID"] != DBNull.Value &&
                                     Convert.ToInt64(currentRow["ContractServiceTypeID"]) == contractServiceTypeId)))
                {
                    //Get condition for included code
                    if (!string.IsNullOrEmpty(row["IncludedCode"].ToString()))
                        conditions.Add(GetServiceLineCondition(row, false));

                    //Get condition for excluded code
                    if (!string.IsNullOrEmpty(row["ExcludedCode"].ToString()))
                        conditions.Add(GetServiceLineCondition(row, true));
                }
            }

            return conditions;
        }

        /// <summary>
        /// Gets the claim field conditions.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dataTable">The data table.</param>
        /// <returns></returns>
        private static IEnumerable<ICondition> GetClaimFieldConditions(long? contractId, long? contractServiceTypeId, DataTable dataTable)
        {
            List<ICondition> conditions = null;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                //Initialize condition list
                conditions = new List<ICondition>();

                foreach (DataRow row in dataTable.Rows.Cast<DataRow>()
                            .Where(
                                currentRow =>
                                    (contractId.HasValue && currentRow["ContractId"] != DBNull.Value &&
                                     Convert.ToInt64(currentRow["ContractId"]) == contractId) ||
                                    (contractServiceTypeId.HasValue &&
                                     currentRow["ContractServiceTypeID"] != DBNull.Value &&
                                     Convert.ToInt64(currentRow["ContractServiceTypeID"]) == contractServiceTypeId)))
                {

                    ICondition condition = new Condition();

                    condition.OperandIdentifier = (byte)
                        (Enums.OperandIdentifier)Convert.ToInt64(row["ClaimFieldId"]);
                    condition.RightOperand = Convert.ToString(row["Values"]);
                    condition.ConditionOperator = (byte)
                        (Enums.ConditionOperation)Convert.ToInt32(row["OperatorID"]);

                    //Get Property Column name as well as OperandType based on OperandIdentifier
                    condition = UpdatePropertyAndOperand(condition);

                    //Add item into condition list
                    conditions.Add(condition);

                }
            }

            return conditions;
        }


        /// <summary>
        /// Gets the table selection conditions.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="tableSelection">The table selection.</param>
        /// <param name="docValues">The document values.</param>
        /// <returns></returns>
        private static IEnumerable<ICondition> GetTableSelectionConditions(long? contractId, long? contractServiceTypeId, DataTable tableSelection, DataTable docValues)
        {
            List<ICondition> conditions = null;

            if (tableSelection != null && tableSelection.Rows.Count > 0)
            {
                //Initialize condition list
                conditions = new List<ICondition>();

                foreach (DataRow row in tableSelection.Rows.Cast<DataRow>()
                    .Where(
                        currentRow =>
                            (contractId.HasValue && currentRow["ContractId"] != DBNull.Value &&
                             Convert.ToInt64(currentRow["ContractId"]) == contractId) ||
                            (contractServiceTypeId.HasValue &&
                             currentRow["ContractServiceTypeID"] != DBNull.Value &&
                             Convert.ToInt64(currentRow["ContractServiceTypeID"]) == contractServiceTypeId)))
                {
                    //Get Claim field Values based on claimFieldDocId
                    List<string> claimFieldValues =
                            (from DataRow claimFieldValueRow in docValues.Rows
                             where
                                 Convert.ToInt64(claimFieldValueRow["ClaimFieldDocId"]) ==
                                 Convert.ToInt64(row["ClaimFieldDocId"])
                             select Convert.ToString(claimFieldValueRow["Identifier"])).ToList();

                    //FIXED-JAN16 - no need to convert row["ClaimFieldId"] to ToInt64 & then Enum & Then Byte. Directly we can convert it to  Byte using Convert.ToByte
                    //If the ClaimField type is already selected with the same operand type and same condition operator, appends the new claimFieldValues and updates the existing condition
                    if (conditions.Any(doc => doc.OperandIdentifier == Convert.ToByte(row["ClaimFieldId"])
                        && doc.ConditionOperator == Convert.ToByte(row["OperatorID"])))
                    {

                        conditions.Where(a => a.OperandIdentifier == Convert.ToByte(row["ClaimFieldId"])
                            && a.ConditionOperator == Convert.ToByte(row["OperatorID"])).ToList()
                            .ForEach(x => x.RightOperand = string.Concat(x.RightOperand, Constants.Comma + string.Join(Constants.Comma, claimFieldValues)));
                    }
                    else
                    {
                        ICondition condition = new Condition();
                        condition.OperandIdentifier = (byte)
                            (Enums.OperandIdentifier)Convert.ToInt64(row["ClaimFieldId"]);
                        condition.ConditionOperator = (byte)Enums.ConditionOperation.EqualTo;


                        //join claimFieldValues and assign it to RightOperand
                        condition.RightOperand = string.Join(Constants.Comma, claimFieldValues);

                        //remove space if table selection is type of Physician
                        if (condition.OperandIdentifier == (byte)Enums.OperandIdentifier.AttendingPhysician ||
                            condition.OperandIdentifier == (byte)Enums.OperandIdentifier.RenderingPhysician ||
                            condition.OperandIdentifier == (byte)Enums.OperandIdentifier.ReferringPhysician)
                            condition.RightOperand = condition.RightOperand.Replace(Constants.Space, string.Empty);


                        //Get Property Column name as well as OperandType based on OperandIdentifier
                        condition = UpdatePropertyAndOperand(condition);

                        //setting operator for table selection
                        condition.ConditionOperator = DBNull.Value == row["OperatorID"]
                            ? (byte)Enums.ConditionOperation.EqualTo
                            : (byte)(Enums.ConditionOperation)Convert.ToInt32(row["OperatorID"]);

                        //Add item into condition list
                        conditions.Add(condition);
                    }
                }
            }
            return conditions;
        }


        /// <summary>
        /// Gets the claims adjudicated.
        /// </summary>
        /// <param name="claimsAdjudicatedData">The claims adjudicated data.</param>
        /// <returns></returns>
        protected static List<SummaryReport> GetClaimsAdjudicated(DataTable claimsAdjudicatedData)
        {
            List<SummaryReport> claimsAdjudicated = null;
            if (claimsAdjudicatedData != null && claimsAdjudicatedData.Rows.Count > 0)
            {
                claimsAdjudicated = (from DataRow row in claimsAdjudicatedData.Rows
                                     select new SummaryReport
                                     {
                                         Scenario = Convert.ToString(row["Scenario"]),
                                         ClaimCount = Convert.ToString(row["ClaimCount"]),
                                         Percentage =
                                             Math.Round(double.Parse(row["Percentage"].ToString()), Constants.Two)
                                                 .ToString(CultureInfo.InvariantCulture)
                                     }).ToList();
            }
            return claimsAdjudicated;
        }

        /// <summary>
        /// Gets the payments linked.
        /// </summary>
        /// <param name="paymentsLinkedData">The payments linked data.</param>
        /// <returns></returns>
        protected static List<SummaryReport> GetPaymentsLinked(DataTable paymentsLinkedData)
        {
            List<SummaryReport> paymentsLinked = null;
            if (paymentsLinkedData != null && paymentsLinkedData.Rows.Count > 0)
            {
                paymentsLinked = (from DataRow row in paymentsLinkedData.Rows
                                  select new SummaryReport
                                  {
                                      Scenario = Convert.ToString(row["Scenario"]),
                                      ClaimCount = Convert.ToString(row["ClaimCount"]),
                                      Percentage = Math.Round(double.Parse(row["Percentage"].ToString()), Constants.Two).ToString(CultureInfo.InvariantCulture)
                                  }).ToList();
            }
            return paymentsLinked;
        }


        /// <summary>
        /// Gets the claim charges.
        /// </summary>
        /// <param name="claimChargesData">The claim charges data.</param>
        /// <returns></returns>
        protected static List<SummaryReport> GetClaimCharges(DataTable claimChargesData)
        {
            List<SummaryReport> claimCharges = null;
            if (claimChargesData != null && claimChargesData.Rows.Count > 0)
            {
                claimCharges = (from DataRow row in claimChargesData.Rows
                                select new SummaryReport
                                {
                                    Scenario = Convert.ToString(row["Scenario"]),
                                    Amount = DBNull.Value == row["Amount"] ? string.Empty : Convert.ToDecimal(double.Parse(row["Amount"].ToString())).ToString(Constants.AmountFormat).Replace(Constants.Minus, string.Empty)
                                }).ToList();
            }
            return claimCharges;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colummnValue">The column value.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <returns></returns>
        protected static T GetValue<T>(object colummnValue, Type dataType)
        {
            return colummnValue == DBNull.Value ? default(T) : (T)Convert.ChangeType(colummnValue, dataType);
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <param name="colummnValue">The column value.</param>
        /// <returns></returns>
        protected static string GetStringValue(object colummnValue)
        {
            return colummnValue == DBNull.Value ? string.Empty : (string)Convert.ChangeType(colummnValue, typeof(string));
        }

        protected static double? GetDoubleValue(object columnValue)
        {
           return columnValue == DBNull.Value ? (double?)null : Math.Round(double.Parse((string)Convert.ChangeType(columnValue, typeof(string))), 2);
        }

        /// <summary>
        /// Gets the claim variances.
        /// </summary>
        /// <param name="claimVariancesData">The claim variances data.</param>
        /// <returns></returns>
        protected static List<SummaryReport> GetClaimVariances(DataTable claimVariancesData)
        {
            List<SummaryReport> claimVariances = null;
            if (claimVariancesData != null && claimVariancesData.Rows.Count > 0)
            {
                claimVariances = (from DataRow row in claimVariancesData.Rows
                                  select new SummaryReport
                                  {
                                      Scenario = Convert.ToString(row["Scenario"]),
                                      ClaimCount = Convert.ToString(row["ClaimCount"]),
                                      Percentage = DBNull.Value == row["Percentage"] ? string.Empty : Math.Round(double.Parse(row["Percentage"].ToString()), Constants.Two).ToString(CultureInfo.InvariantCulture),
                                      Amount = DBNull.Value == row["Amount"] ? string.Empty : Convert.ToDecimal(double.Parse(row["Amount"].ToString())).ToString(Constants.AmountFormat).Replace(Constants.Minus, string.Empty)
                                  }).ToList();
            }
            return claimVariances;
        }

        /// <summary>
        /// Gets the variance ranges.
        /// </summary>
        /// <param name="varianceRangesData">The variance ranges data.</param>
        /// <returns></returns>
        protected static List<SummaryReport> GetVarianceRanges(DataTable varianceRangesData)
        {
            List<SummaryReport> varianceRanges = null;
            if (varianceRangesData != null && varianceRangesData.Rows.Count > 0)
            {
                varianceRanges = (from DataRow row in varianceRangesData.Rows
                                  select new SummaryReport
                                  {
                                      StartValue = Convert.ToString(row["PositiveStartValue"]),
                                      EndValue = Convert.ToString(row["PositiveEndValue"]),
                                      ClaimCount = Convert.ToString(row["ClaimCount"]),
                                      Percentage = DBNull.Value == row["Percentage"] ? string.Empty : Math.Round(double.Parse(row["Percentage"].ToString()), Constants.Two).ToString(CultureInfo.InvariantCulture)
                                  }).ToList();
            }
            return varianceRanges;
        }


        /// <summary>
        /// Check Column available or not in datarow
        /// </summary>
        /// <param name="claimDataRow"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        protected static bool IsColumnExists(DataRow claimDataRow, string columnName)
        {
            return claimDataRow.Table.Columns.Contains(columnName);
        }
    }
}
