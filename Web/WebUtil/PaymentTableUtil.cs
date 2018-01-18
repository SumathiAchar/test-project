using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.PaymentTable.Models;
using DataTable = System.Data.DataTable;
using Microsoft.VisualBasic.FileIO;

namespace SSI.ContractManagement.Web.WebUtil
{
    public static class PaymentTableUtil
    {
        private static readonly string[] SpecialCharacterList = { "[", "]" };

        /// <summary>
        /// Reads the CSV file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        private static DataTable ReadCsvFile(string filePath)
        {
            DataTable csvData = new DataTable();
            using (TextFieldParser csvReader = new TextFieldParser(filePath))
            {
                // ReSharper disable once RedundantExplicitParamsArrayCreation
                csvReader.SetDelimiters(new[] { Constants.Comma });
                csvReader.HasFieldsEnclosedInQuotes = true;
                string[] colFields = csvReader.ReadFields();
                if (colFields != null)
                    for (int index = 0; index < colFields.Length && index < 10; index++)
                    {
                        string column = colFields[index];
                        DataColumn datecolumn = new DataColumn(column) { AllowDBNull = true };
                        csvData.Columns.Add(datecolumn + "_#&#" + index);
                    }

                while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();
                    if (fieldData != null)
                    {
                        DataRow row = csvData.NewRow();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length && i < 10; i++)
                        {
                            if (fieldData[i].Contains(Constants.Comma))
                                fieldData[i] = Constants.CommaReplaceString +
                                               fieldData[i].Replace(Constants.Comma, Constants.ReplaceCommaInString) +
                                               Constants.CommaReplaceString;
                            fieldData[i] =
                                fieldData[i].Replace(Constants.NewLine, string.Empty)
                                    .Replace(Constants.CarriageReturn, string.Empty);
                            // ReSharper disable once CoVariantArrayConversion
                            row[i] = fieldData[i];
                        }
                        csvData.Rows.Add(row);
                    }
                }
            }

            //return csvData;
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(Constants.CodeField, typeof(string));
            dataTable.Columns.Add(Constants.AmountField, typeof(string));
            string[] columnNames = csvData.Columns.Cast<DataColumn>()
                .Select(x => x.ColumnName)
                .ToArray();
            DataRow dataRowheader = dataTable.NewRow();
            string amount = string.Empty;
            for (int i = 1; i < columnNames.Count(); i++)
            {
                if (i == 1)
                {
                    amount += columnNames[i].Replace("_#&#" + i, string.Empty);
                }
                else
                {
                    amount += Constants.Comma + columnNames[i].Replace("_#&#" + i, string.Empty);
                }

            }
            dataRowheader[Constants.CodeField] = columnNames[0].Replace("_#&#" + 0, string.Empty);
            dataRowheader[Constants.AmountField] = amount;
            dataTable.Rows.Add(dataRowheader);
            foreach (DataRow drRow in csvData.Rows)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow[Constants.CodeField] = drRow[columnNames[0]];
                for (int i = 1; i < columnNames.Length; i++)
                {
                    if (i == 1)
                        dataRow[Constants.AmountField] =
                            drRow[columnNames[i]].ToString()
                                .Replace(Constants.NewLine, string.Empty)
                                .Replace(Constants.CarriageReturn, string.Empty);
                    else
                        dataRow[Constants.AmountField] += Constants.Comma +
                                                          drRow[columnNames[i]].ToString()
                                                              .Replace(Constants.NewLine, string.Empty)
                                                              .Replace(Constants.CarriageReturn, string.Empty);
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        /// <summary>
        /// Reads the excel file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private static DataTable ReadExcelFile(string filePath, string fileExtension)
        {
            DataTable dataTable = new DataTable();
            //Set Oledb connectionstring based on fileExtension
            string excelConnectionType = (fileExtension.ToUpper() == Enums.DownloadFileType.Xls.ToString())
                ? string.Format(Constants.Oledb4Connection, filePath)
                : string.Format(Constants.Oledb12Connection, filePath);

            OleDbConnection excelConnection = new OleDbConnection(excelConnectionType);
            excelConnection.Open();
            DataTable schemaTable = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                new object[] { null, null, null, "TABLE" });

            //Getting the name of first Excel Sheet
            if (schemaTable != null)
            {
                DataRow dataRow =
                    schemaTable.Rows.Cast<DataRow>()
                        .FirstOrDefault(row => Convert.ToString(row["TABLE_NAME"]).Contains(Constants.Dollar));

                if (dataRow != null)
                {
                    string rowValue = dataRow["TABLE_NAME"].ToString();
                    string query = string.Format("SELECT * FROM [{0}]", rowValue);
                    OleDbDataAdapter myCommand = new OleDbDataAdapter(query, excelConnectionType);
                    dataTable = new DataTable();
                    myCommand.Fill(dataTable);
                    excelConnection.Close();
                }
            }

            while (dataTable.Columns.Count > 10)
            {
                dataTable.Columns.RemoveAt(10);
            }

            DataTable dtCloned = dataTable.Clone();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    dtCloned.Columns[j].DataType = typeof(string);
                }
                dtCloned.ImportRow(dataTable.Rows[i]);
            }

            //removing carriage return from data
            foreach (DataRow dr in dtCloned.Rows)
            {
                foreach (DataColumn dc in dtCloned.Columns)
                {
                    if (dr[dc].ToString() != string.Empty)
                    {
                        dr[dc] =
                            dr[dc].ToString()
                                .Replace(Constants.NewLine, string.Empty)
                                .Replace(Constants.CarriageReturn, string.Empty);
                    }
                }
            }


            return dtCloned;
        }

        /// <summary>
        /// Validates the data.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="claimFieldId">The claim field identifier.</param>
        /// <returns></returns>
        public static PaymentTableViewModel ValidateData(string filePath, string fileExtension, long claimFieldId)
        {
            //Declare paymentTableViewModel with default message as string.Empty assuming no error otherwise it will overwrite 
            PaymentTableViewModel paymentTableViewModel = new PaymentTableViewModel { Message = string.Empty };

            //Save file into local folder
            if (String.Equals(fileExtension, Enums.DownloadFileType.Csv.ToString(),
                StringComparison.CurrentCultureIgnoreCase))
            {
                // Converting to the data table from CSV file
                paymentTableViewModel.CalimFieldValues = ReadCsvFile(filePath);
               
                // Removing the last column if it is empty
                bool isLastValueEmpty = false;
                foreach (var dataRow in paymentTableViewModel.CalimFieldValues.Rows.Cast<DataRow>())
                {
                    isLastValueEmpty = dataRow.ItemArray[1].ToString().EndsWith(Constants.Comma);
                }
                if (isLastValueEmpty)
                {
                    List<DataRow> totalRows = paymentTableViewModel.CalimFieldValues.Rows.Cast<DataRow>().ToList();
                    for (int columnCount = Constants.Zero; columnCount < totalRows.Count; columnCount++)
                    {
                        paymentTableViewModel.CalimFieldValues.Rows[columnCount][Constants.One] =
                            paymentTableViewModel.CalimFieldValues.Rows[columnCount][Constants.One].ToString()
                                .TrimEnd(Constants.CommaCharacter);
                    }
                }

                // Removing the empty rows from the data table
                foreach (DataRow row in
                    paymentTableViewModel.CalimFieldValues.Rows.Cast<DataRow>()
                        .Skip(Constants.One) //Removing First row
                        .ToList()
                        .Where(
                            row =>
                                row.ItemArray[Constants.One].ToString()
                                    .Split(Constants.CommaCharacter)
                                    .All(
                                        field =>
                                            string.IsNullOrWhiteSpace(field.ToString(CultureInfo.InvariantCulture))) &&
                                string.IsNullOrWhiteSpace(row.ItemArray[Constants.Zero].ToString())))
                {
                    paymentTableViewModel.CalimFieldValues.Rows.Remove(row);
                }

                // Validating the CSV file
                paymentTableViewModel.Message = ValidateCsv(claimFieldId, paymentTableViewModel.CalimFieldValues);
            }
            else if (
                String.Equals(fileExtension, Enums.DownloadFileType.Xls.ToString(),
                    StringComparison.CurrentCultureIgnoreCase) ||
                String.Equals(fileExtension, Enums.DownloadFileType.Xlsx.ToString(),
                    StringComparison.CurrentCultureIgnoreCase))
            {
                // Converting to the data table from excel file
                paymentTableViewModel.CalimFieldValues = ReadExcelFile(filePath,
                    fileExtension);

                // Removing the empty column from the data table
                foreach (DataColumn column in paymentTableViewModel.CalimFieldValues.Columns.Cast<DataColumn>()
                    .ToArray()
                    .Where(
                        column =>
                            paymentTableViewModel.CalimFieldValues.AsEnumerable().All(dataRow => dataRow.IsNull(column)))
                    )
                {
                    paymentTableViewModel.CalimFieldValues.Columns.Remove(column);
                }

                // Removing the empty rows from the data table
                foreach (
                    DataRow row in
                        paymentTableViewModel.CalimFieldValues.Rows.Cast<DataRow>()
                            .Skip(Constants.One) //Removing First row
                            .ToList()
                            .Where(row => row.ItemArray.All(field => string.IsNullOrWhiteSpace(field.ToString()))))
                {
                    paymentTableViewModel.CalimFieldValues.Rows.Remove(row);
                }

                // Validating the Excel
                paymentTableViewModel.Message = ValidateExcel(claimFieldId, paymentTableViewModel.CalimFieldValues);
            }

            return paymentTableViewModel;
        }

        /// <summary>
        /// Validates the CSV.
        /// </summary>
        /// <param name="claimFieldId">The claim field identifier.</param>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <returns></returns>
        //FIXED-SEP15 - Method is too long. Split into multiple methods.
        private static string ValidateCsv(long claimFieldId, DataTable claimFieldValuesDataTable)
        {
            // Checking that file empty is or not
            if (
                string.IsNullOrWhiteSpace(
                    claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.Zero].ToString()) &&
                string.IsNullOrWhiteSpace(
                    claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.One].ToString()))
            {
                return Constants.EmptyFile;
            }

            // Checking that table is having minimum of 2 columns or not
            if (claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray.Length <= Constants.One)
            {
                return Constants.InsufficientColumnsErrorMessage;
            }

            // Checking that if first column has empty text
            if (
                string.IsNullOrWhiteSpace(
                    claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.Zero].ToString()))
                return string.Format(Constants.BlankValueInCell, Constants.One);

            // Checking that if except first column has empty text
            string headerEmptyMessage = ValidateHeaderEmptyCsv(claimFieldId, claimFieldValuesDataTable);
            if (!string.IsNullOrWhiteSpace(headerEmptyMessage))
                return headerEmptyMessage;

            // Checking for any duplicate headers
            // If uploaded table is for Drg or Asc or Fee schedule then we need to consider first two columns header text for duplicates or special characters
            string invalidHeaderMessage = ValidateInvalidHeaderCsv(claimFieldId, claimFieldValuesDataTable);
            if (!string.IsNullOrWhiteSpace(invalidHeaderMessage))
                return invalidHeaderMessage;

            // Checking that equal number of rows and columns
            foreach (
                DataRow dataRow in
                    from dataRow in claimFieldValuesDataTable.Rows.Cast<DataRow>().Skip(Constants.One).ToList()
                    let rowDataList =
                        dataRow.ItemArray[Constants.One].ToString().Split(Constants.CommaCharacter).ToList()
                    where string.IsNullOrWhiteSpace(rowDataList.Last())
                    select dataRow)
            {
                return string.Format(Constants.ColumnRowCount,
                    Convert.ToInt32(claimFieldValuesDataTable.Rows.IndexOf(dataRow) + Constants.One)
                        .ToString(CultureInfo.InvariantCulture));
            }

            //apart from CustomPaymentType claimFieldId
            if (claimFieldId != (byte)Enums.ClaimFieldTypes.CustomPaymentType)
            {
                for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
                {
                    // Validating that first 2 columns data having empty
                    if (
                        string.IsNullOrWhiteSpace(
                            claimFieldValuesDataTable.Rows[count].ItemArray[Constants.Zero].ToString()) ||
                        string.IsNullOrWhiteSpace(
                            claimFieldValuesDataTable.Rows[count].ItemArray[Constants.One].ToString()
                                .Split(Constants.CommaCharacter)[Constants.Zero]))
                    {
                        return string.Format(Constants.BlankValueInCell,
                            Convert.ToInt32(
                                claimFieldValuesDataTable.Rows.IndexOf(claimFieldValuesDataTable.Rows[count]) +
                                Constants.One)
                                .ToString(CultureInfo.InvariantCulture));
                    }
                }
            }

            //If claimFieldId is DrgWeightTable
            if (claimFieldId == (byte)Enums.ClaimFieldTypes.DrgWeightTable)
            {
                string validateDrgMessage = ValidateDrgCsv(claimFieldValuesDataTable);
                if (!string.IsNullOrWhiteSpace(validateDrgMessage))
                    return validateDrgMessage;
            }

            // Checking for duplicates
            List<IGrouping<string, DataRow>> duplicates = IsDuplicate(claimFieldValuesDataTable);
            if (duplicates.Any())
            {
                return string.Format(Constants.DuplicateValue,
                    duplicates.Select(duplicate => duplicate.Key).First());
            }

            //Some more validation for payment type other than CustomPaymentType
            if (claimFieldId != (byte)Enums.ClaimFieldTypes.CustomPaymentType)
            {
                // Validating quotes for other than CustomPaymentType
                string validateQuotesmessage = ValidateQuotesCsv(claimFieldValuesDataTable);
                if (!string.IsNullOrWhiteSpace(validateQuotesmessage))
                    return validateQuotesmessage;
            }
            else
            {
                // Validating quotes for CustomPaymentType
                string validateCustomTable = ValidateCustomTableCsv(claimFieldValuesDataTable);
                if (!string.IsNullOrWhiteSpace(validateCustomTable))
                    return validateCustomTable;
            }

            // Validating Asc and Fee schedule
            if (claimFieldId == (byte)(Enums.ClaimFieldTypes.AscFeeSchedule) ||
                claimFieldId == (byte)(Enums.ClaimFieldTypes.FeeSchedule))
            {
                string validateAscAndFeeMessage = ValidateAscAndFeeScheduleCsv(claimFieldValuesDataTable);
                if (!string.IsNullOrWhiteSpace(validateAscAndFeeMessage))
                    return validateAscAndFeeMessage;
            }

            //Return empty string if all validation are satisfying 
            return string.Empty;
        }

        /// <summary>
        /// Validates the excel.
        /// </summary>
        /// <param name="claimFieldId">The claim field identifier.</param>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <returns></returns>
        private static string ValidateExcel(long claimFieldId, DataTable claimFieldValuesDataTable)
        {
            // Checking that file empty is or not
            if (
                claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray.All(
                    item => string.IsNullOrWhiteSpace(item.ToString())))
            {
                return Constants.EmptyFile;
            }

            // Checking that table is having minimum of 2 columns or not
            if (claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray.Length <= Constants.One)
            {
                return Constants.InsufficientColumnsErrorMessage;
            }

            // Checking that if any column has empty text
            string headerEmptyMessage = ValidateHeaderEmptyExcel(claimFieldId, claimFieldValuesDataTable);
            if (!string.IsNullOrWhiteSpace(headerEmptyMessage))
                return headerEmptyMessage;

            // Checking for any duplicate headers
            // If uploaded table is for Drg or Asc or Fee schedule then we need to consider first two columns header text for duplicates or special characters
            string invalidHeaderMessage = ValidateInvalidHeaderExcel(claimFieldId, claimFieldValuesDataTable);
            if (!string.IsNullOrWhiteSpace(invalidHeaderMessage))
                return invalidHeaderMessage;

            // Checking that if any row length has more or less values than column length
            foreach (
                DataRow dataRow in
                    claimFieldValuesDataTable.Rows.Cast<DataRow>()
                        .Skip(Constants.One)
                        .ToList()
                        .Where(dataRow => string.IsNullOrWhiteSpace(dataRow.ItemArray.Last().ToString())))
            {
                return string.Format(Constants.ColumnRowCount,
                    Convert.ToInt32(claimFieldValuesDataTable.Rows.IndexOf(dataRow) + Constants.One)
                        .ToString(CultureInfo.InvariantCulture));
            }

            // Getting the column names
            string firstColumn = claimFieldValuesDataTable.Columns[Constants.Zero].ColumnName;
            string secondColumn = claimFieldValuesDataTable.Columns[Constants.One].ColumnName;

            if (claimFieldId != (byte)Enums.ClaimFieldTypes.CustomPaymentType)
            {
                // Validating the quotes for other than CustomPaymentType
                string validateQuotesMessage = ValidateQuotesExcel(claimFieldValuesDataTable);
                if (!string.IsNullOrWhiteSpace(validateQuotesMessage))
                    return validateQuotesMessage;
            }
            else
            {
                // Validating the quotes for CustomPaymentType
                string validatePaymentTablemessage = ValidateCustomTableExcel(claimFieldValuesDataTable, firstColumn);
                if (!string.IsNullOrWhiteSpace(validatePaymentTablemessage))
                    return validatePaymentTablemessage;
            }

            // Validating drg conditions
            if (claimFieldId == (byte)(Enums.ClaimFieldTypes.DrgWeightTable))
            {
                // Validating the drg
                string validateDrgMessage = ValidateDrgExcel(claimFieldValuesDataTable, firstColumn, secondColumn);
                if (!string.IsNullOrWhiteSpace(validateDrgMessage))
                    return validateDrgMessage;
            }

            // Checking for duplicates
            List<IGrouping<string, DataRow>> duplicates = IsDuplicate(claimFieldValuesDataTable);
            if (duplicates.Any())
            {
                return string.Format(Constants.DuplicateValue,
                    duplicates.Select(duplicate => duplicate.Key).First());
            }

            if (claimFieldId == (byte)(Enums.ClaimFieldTypes.AscFeeSchedule) ||
                claimFieldId == (byte)(Enums.ClaimFieldTypes.FeeSchedule))
            {
                // Validating the Asc and Fee schedule
                string validateAscAndFeeMessage = ValidateAscAndFeeScheduleExcel(claimFieldValuesDataTable, firstColumn,
                    secondColumn);
                if (!string.IsNullOrWhiteSpace(validateAscAndFeeMessage))
                    return validateAscAndFeeMessage;
            }
            //Return Empty string if all validation are correct
            return string.Empty;
        }

        /// <summary>
        /// Validates the asc and fee schedule.
        /// </summary>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <returns></returns>
        private static string ValidateAscAndFeeScheduleCsv(DataTable claimFieldValuesDataTable)
        {
            // Checking first column length
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                //FIXED-SEP15 move validating Hcpcs code into one method as its using other place we can place it into common place
                if (!ValidateHcpcsCodeLength(claimFieldValuesDataTable.Rows[count][Constants.Zero].ToString()))
                {
                    return string.Format(Constants.InValidCode,
                        Convert.ToInt32(count + Constants.One).ToString(CultureInfo.InvariantCulture));
                }
            }

            // Checking the second column is numeric or not
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                // Removing the $ from the string 
                List<string> values =
                    claimFieldValuesDataTable.Rows[count].ItemArray[Constants.One].ToString()
                        .Split(Constants.CommaCharacter)
                        .ToList();
                values[Constants.Zero] = values[Constants.Zero].Replace(Constants.Dollar, string.Empty).Trim();
                claimFieldValuesDataTable.Rows[count][Constants.One] = string.Join(Constants.Comma, values);

                if (
                    !claimFieldValuesDataTable.Rows[count].ItemArray[Constants.One].ToString()
                        .Split(Constants.CommaCharacter)[
                            Constants.Zero]
                        .Except(Constants.Dot)
                        .Except(Constants.ReplaceCommaInString)
                        .Except(Constants.CommaReplaceString)
                        .All(char.IsDigit))
                {
                    return string.Format(Constants.ColumnNumeric,
                        Convert.ToInt32(
                            claimFieldValuesDataTable.Rows.IndexOf(claimFieldValuesDataTable.Rows[count]) +
                            Constants.One)
                            .ToString(CultureInfo.InvariantCulture));
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Validates the asc and fee schedule excel.
        /// </summary>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <param name="firstColumn">The first column.</param>
        /// <param name="secondColumn">The second column.</param>
        /// <returns></returns>
        private static string ValidateAscAndFeeScheduleExcel(DataTable claimFieldValuesDataTable, string firstColumn,
            string secondColumn)
        {
            // Checking first column length
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                if (!ValidateHcpcsCodeLength(claimFieldValuesDataTable.Rows[count][firstColumn].ToString()))
                {
                    return string.Format(Constants.InValidCode,
                        Convert.ToInt32(count + Constants.One).ToString(CultureInfo.InvariantCulture));
                }
            }

            // Checking the second column is numeric or not
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                // Removing the $ and comma while uploading
                claimFieldValuesDataTable.Rows[count][secondColumn] =
                    claimFieldValuesDataTable.Rows[count][secondColumn].ToString()
                        .Replace(Constants.Dollar, string.Empty)
                        .Replace(Constants.Comma, string.Empty);

                //check for numeric values
                if (!claimFieldValuesDataTable.Rows[count][secondColumn].ToString()
                    .Except(Constants.Dot)
                    .All(char.IsDigit))
                {
                    return string.Format(Constants.ColumnNumeric,
                        Convert.ToInt32(
                            claimFieldValuesDataTable.Rows.IndexOf(claimFieldValuesDataTable.Rows[count]) +
                            Constants.One)
                            .ToString(CultureInfo.InvariantCulture));
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Validates the custom table CSV.
        /// </summary>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <returns></returns>
        private static string ValidateCustomTableCsv(DataTable claimFieldValuesDataTable)
        {
            // Validating that first column data having even number of quotes 
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                claimFieldValuesDataTable.Rows[count][Constants.Zero] =
                    claimFieldValuesDataTable.Rows[count][Constants.Zero].ToString()
                        .Replace(Constants.ThreeDoubleQuote, Constants.DoubleQuote)
                        .Replace(Constants.TwoDoubleQuote, Constants.DoubleQuote);
                if (
                    claimFieldValuesDataTable.Rows[count].ItemArray[Constants.Zero].ToString()
                        .Count(value => value == Constants.DoubleQuoteChar) % Constants.Two == Constants.Zero)
                {
                    claimFieldValuesDataTable.Rows[count][Constants.Zero] =
                        claimFieldValuesDataTable.Rows[count][Constants.Zero].ToString()
                            .Replace(Constants.DoubleQuote, string.Empty);
                }
                else
                {
                    return string.Format(Constants.InCorrectQuotes,
                        Convert.ToInt32(
                            claimFieldValuesDataTable.Rows.IndexOf(claimFieldValuesDataTable.Rows[count]) +
                            Constants.One)
                            .ToString(CultureInfo.InvariantCulture));
                }
            }

            // Checking first column has empty text
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                if (string.IsNullOrWhiteSpace(claimFieldValuesDataTable.Rows[count].ItemArray[Constants.Zero].ToString()))
                {
                    return string.Format(Constants.BlankValueInCell, Constants.One);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Validates the payment table excel.
        /// </summary>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <param name="firstColumn">The first column.</param>
        /// <returns></returns>
        private static string ValidateCustomTableExcel(DataTable claimFieldValuesDataTable, string firstColumn)
        {
            // Checking that each and every cell contains even number of quotes 
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                if (claimFieldValuesDataTable.Rows[count].ItemArray[Constants.Zero].ToString()
                    .Count(value => value == Constants.DoubleQuoteChar) % Constants.Two != Constants.Zero)
                {
                    return string.Format(Constants.InCorrectQuotes,
                        Convert.ToInt32(
                            claimFieldValuesDataTable.Rows.IndexOf(claimFieldValuesDataTable.Rows[count]) +
                            Constants.One)
                            .ToString(CultureInfo.InvariantCulture));
                }
                claimFieldValuesDataTable.Rows[count][Constants.Zero] =
                    claimFieldValuesDataTable.Rows[count][Constants.Zero].ToString()
                        .Replace(Constants.DoubleQuote, string.Empty);
            }

            // Checking for first column has any empty value
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                if (string.IsNullOrWhiteSpace(claimFieldValuesDataTable.Rows[count][firstColumn].ToString()))
                {
                    return string.Format(Constants.BlankValueInCell,
                        Convert.ToInt32(
                            claimFieldValuesDataTable.Rows.IndexOf(claimFieldValuesDataTable.Rows[count]) +
                            Constants.One)
                            .ToString(CultureInfo.InvariantCulture));
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Validates the quotes CSV.
        /// </summary>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <returns></returns>
        private static string ValidateQuotesCsv(DataTable claimFieldValuesDataTable)
        {
            // Checking that each and every cell contains even number of quotes 
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                claimFieldValuesDataTable.Rows[count][Constants.One] =
                    Regex.Replace(claimFieldValuesDataTable.Rows[count][Constants.One].ToString(),
                        Constants.RemoveCommaRegex, String.Empty);
                if (
                    claimFieldValuesDataTable.Rows[count].ItemArray[Constants.Zero].ToString()
                        .Count(value => value == Constants.DoubleQuoteChar) % Constants.Two != Constants.Zero ||
                    claimFieldValuesDataTable.Rows[count].ItemArray[Constants.One].ToString()
                        .Split(Constants.CommaCharacter).Any(
                            item =>
                                item.ToString(CultureInfo.InvariantCulture)
                                    .Count(value => value == Constants.DoubleQuoteChar) %
                                Constants.Two != Constants.Zero))
                {
                    return string.Format(Constants.InCorrectQuotes,
                        Convert.ToInt32(
                            claimFieldValuesDataTable.Rows.IndexOf(claimFieldValuesDataTable.Rows[count]) +
                            Constants.One)
                            .ToString(CultureInfo.InvariantCulture));
                }
            }

            // Replace quotes with string.Empty
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                if (
                    claimFieldValuesDataTable.Rows[count].ItemArray[Constants.Zero].ToString()
                        .Count(value => value == Constants.DoubleQuoteChar) % Constants.Two == Constants.Zero)
                {
                    claimFieldValuesDataTable.Rows[count][Constants.Zero] =
                        claimFieldValuesDataTable.Rows[count][Constants.Zero].ToString()
                            .Replace(Constants.DoubleQuote, string.Empty);
                }
                List<string> values =
                    claimFieldValuesDataTable.Rows[count].ItemArray[Constants.One].ToString()
                        .Split(Constants.CommaCharacter)
                        .ToList();
                for (int item = Constants.Zero; item < values.Count; item++)
                {
                    values[item] = values[item].Replace(Constants.DoubleQuote, string.Empty);
                }
                claimFieldValuesDataTable.Rows[count][Constants.One] = string.Join(Constants.Comma, values);
            }
            return string.Empty;
        }

        /// <summary>
        /// Validates the quotes excel.
        /// </summary>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <returns></returns>
        private static string ValidateQuotesExcel(DataTable claimFieldValuesDataTable)
        {
            // Checking that first two cell contains even number of quotes 
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                if (claimFieldValuesDataTable.Rows[count].ItemArray[Constants.Zero].ToString()
                    .Count(value => value == Constants.DoubleQuoteChar) % Constants.Two != Constants.Zero
                    || claimFieldValuesDataTable.Rows[count].ItemArray[Constants.One].ToString()
                        .Count(value => value == Constants.DoubleQuoteChar) % Constants.Two != Constants.Zero)
                {
                    return string.Format(Constants.InCorrectQuotes,
                        Convert.ToInt32(
                            claimFieldValuesDataTable.Rows.IndexOf(claimFieldValuesDataTable.Rows[count]) +
                            Constants.One)
                            .ToString(CultureInfo.InvariantCulture));
                }
                claimFieldValuesDataTable.Rows[count][Constants.Zero] =
                    claimFieldValuesDataTable.Rows[count][Constants.Zero].ToString()
                        .Replace(Constants.DoubleQuote, string.Empty);
                claimFieldValuesDataTable.Rows[count][Constants.One] =
                    claimFieldValuesDataTable.Rows[count][Constants.One].ToString()
                        .Replace(Constants.DoubleQuote, string.Empty);
            }

            // Checking the first two columns having empty or null
            foreach (DataRow dataRow in
                claimFieldValuesDataTable.Rows.Cast<DataRow>()
                    .Where(dataRow => string.IsNullOrWhiteSpace(dataRow.ItemArray[Constants.Zero].ToString()) ||
                                      string.IsNullOrWhiteSpace(dataRow.ItemArray[Constants.One].ToString())))
            {
                return string.Format(Constants.BlankValueInCell,
                    Convert.ToInt32(claimFieldValuesDataTable.Rows.IndexOf(dataRow) + Constants.One)
                        .ToString(CultureInfo.InvariantCulture));
            }
            return string.Empty;
        }

        /// <summary>
        /// Validates the DRG CSV.
        /// </summary>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <returns></returns>
        private static string ValidateDrgCsv(DataTable claimFieldValuesDataTable)
        {
            // Checking that first column length is valid or not
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                if (claimFieldValuesDataTable.Rows[count][Constants.Zero].ToString().Trim().Length > 3)
                {
                    return string.Format(Constants.InValidDrgCode,
                        Convert.ToInt32(
                            claimFieldValuesDataTable.Rows.IndexOf(claimFieldValuesDataTable.Rows[count]) +
                            Constants.One)
                            .ToString(CultureInfo.InvariantCulture));
                }
            }

            // Checking the length is less than 3 if uploaded document is for Drg
            for (int rowCount = Constants.One; rowCount < claimFieldValuesDataTable.Rows.Count; rowCount++)
            {
                if (claimFieldValuesDataTable.Rows[rowCount][Constants.Zero].ToString().Trim().Length < Constants.DrgCodeLength)
                {
                    for (int columnCount = Constants.Zero;
                        columnCount <=
                        Constants.DrgCodeLength -
                        claimFieldValuesDataTable.Rows[rowCount][Constants.Zero].ToString().Trim().Length;
                        columnCount++)
                    {
                        // Appending the 0's to the value
                        claimFieldValuesDataTable.Rows[rowCount][Constants.Zero] = string.Format("{0}{1}",
                            Constants.Zero,
                            claimFieldValuesDataTable.Rows[
                                rowCount][Constants.Zero].ToString().Trim());
                    }
                }
            }

            // Checking the first and second column is numeric or not
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                // Removing the $ from the string from 1st column 
                claimFieldValuesDataTable.Rows[count][Constants.Zero] =
                    claimFieldValuesDataTable.Rows[count][Constants.Zero].ToString()
                        .Replace(Constants.Dollar, string.Empty).Trim();

                // fetching other column values and removing the $ from the string from 2nd column
                List<string> values =
                    claimFieldValuesDataTable.Rows[count].ItemArray[Constants.One].ToString()
                        .Split(Constants.CommaCharacter)
                        .ToList();
                values[Constants.Zero] = values[Constants.Zero].Replace(Constants.Dollar, string.Empty).Trim();
                claimFieldValuesDataTable.Rows[count][Constants.One] = string.Join(Constants.Comma, values);

                // Checking the first and second column text - it should be numeric
                if (
                    !claimFieldValuesDataTable.Rows[count].ItemArray[Constants.Zero].ToString()
                        .Except(Constants.Dot)
                        .Except(Constants.ReplaceCommaInString)
                        .Except(Constants.CommaReplaceString)
                        .All(char.IsDigit) ||
                    !claimFieldValuesDataTable.Rows[count].ItemArray[Constants.One].ToString()
                        .Split(Constants.CommaCharacter)[
                            Constants.Zero]
                        .Except(Constants.Dot)
                        .Except(Constants.ReplaceCommaInString)
                        .Except(Constants.CommaReplaceString)
                        .All(char.IsDigit))
                {
                    return string.Format(Constants.ColumnNumeric,
                        Convert.ToInt32(
                            claimFieldValuesDataTable.Rows.IndexOf(claimFieldValuesDataTable.Rows[count]) +
                            Constants.One)
                            .ToString(CultureInfo.InvariantCulture));
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Validates the DRG excel.
        /// </summary>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <param name="firstColumn">The first column.</param>
        /// <param name="secondColumn">The second column.</param>
        /// <returns></returns>
        private static string ValidateDrgExcel(DataTable claimFieldValuesDataTable, string firstColumn,
            string secondColumn)
        {
            // Checking that first column length is valid or not
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                if (claimFieldValuesDataTable.Rows[count][firstColumn].ToString().Trim().Length > Constants.DrgCodeLength)
                {
                    return string.Format(Constants.InValidDrgCode,
                        Convert.ToInt32(
                            claimFieldValuesDataTable.Rows.IndexOf(claimFieldValuesDataTable.Rows[count]) +
                            Constants.One)
                            .ToString(CultureInfo.InvariantCulture));
                }
            }

            // Checking the length is less than 3 if uploaded document is for Drg
            for (int rowCount = Constants.One; rowCount < claimFieldValuesDataTable.Rows.Count; rowCount++)
            {
                if (claimFieldValuesDataTable.Rows[rowCount][firstColumn].ToString().Trim().Length < Constants.DrgCodeLength)
                {
                    for (int columnCount = Constants.Zero;
                        columnCount <=
                        Constants.DrgCodeLength -
                        claimFieldValuesDataTable.Rows[rowCount][firstColumn].ToString().Trim().Length;
                        columnCount++)
                    {
                        // Appending the 0's to the value
                        claimFieldValuesDataTable.Rows[rowCount][firstColumn] = string.Format("{0}{1}", Constants.Zero,
                            claimFieldValuesDataTable.Rows[
                                rowCount][firstColumn].ToString().Trim());
                    }
                }
            }

            // Checking the first and second column is numeric or not
            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
            {
                // Removing the $ and comma while uploading from 1st column
                claimFieldValuesDataTable.Rows[count][firstColumn] =
                    claimFieldValuesDataTable.Rows[count][firstColumn].ToString()
                        .Replace(Constants.Dollar, string.Empty)
                        .Replace(Constants.Comma, string.Empty).Trim();

                // Removing the $ and comma while uploading from 2nd column
                claimFieldValuesDataTable.Rows[count][secondColumn] =
                    claimFieldValuesDataTable.Rows[count][secondColumn].ToString()
                        .Replace(Constants.Dollar, string.Empty)
                        .Replace(Constants.Comma, string.Empty).Trim();

                //Check for numeric values
                if (!claimFieldValuesDataTable.Rows[count][firstColumn].ToString()
                    .Except(Constants.Dot)
                    .All(char.IsDigit) || !claimFieldValuesDataTable.Rows[count][secondColumn].ToString()
                        .Except(Constants.Dot)
                        .All(char.IsDigit))
                {
                    return string.Format(Constants.ColumnNumeric,
                        Convert.ToInt32(
                            claimFieldValuesDataTable.Rows.IndexOf(claimFieldValuesDataTable.Rows[count]) +
                            Constants.One)
                            .ToString(CultureInfo.InvariantCulture));
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Checks the header empty.
        /// </summary>
        /// <param name="claimFieldId">The claim field identifier.</param>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <returns></returns>
        private static string ValidateHeaderEmptyCsv(long claimFieldId, DataTable claimFieldValuesDataTable)
        {
            int headerCount = Constants.Zero;
            foreach (
                string column in
                    claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.One].ToString()
                        .Split(Constants.CommaCharacter)
                        .ToList())
            {
                headerCount++;
                // If uploaded table is for Drg or Asc or Fee schedule then we need to consider first two columns header text is empty or not
                if ((headerCount <= Constants.One) && (claimFieldId == (byte)(Enums.ClaimFieldTypes.DrgWeightTable) ||
                                                       claimFieldId == (byte)(Enums.ClaimFieldTypes.AscFeeSchedule) ||
                                                       claimFieldId == (byte)(Enums.ClaimFieldTypes.FeeSchedule)) &&
                    string.IsNullOrWhiteSpace(column))
                {
                    {
                        return string.Format(Constants.BlankValueInCell, Constants.One);
                    }
                }
                // If uploaded table is for custom then we need to consider first ten columns header text is empty or not
                if (headerCount <= (Constants.CustomPaymentColumnCount - Constants.One) &&
                    string.IsNullOrWhiteSpace(column) &&
                    claimFieldId == (byte)(Enums.ClaimFieldTypes.CustomPaymentType))
                {
                    {
                        return string.Format(Constants.BlankValueInCell, Constants.One);
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Validates the header empty excel.
        /// </summary>
        /// <param name="claimFieldId">The claim field identifier.</param>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <returns></returns>
        private static string ValidateHeaderEmptyExcel(long claimFieldId, DataTable claimFieldValuesDataTable)
        {
            int headerCount = Constants.Zero;
            foreach (object column in claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray)
            {
                headerCount++;
                // If uploaded table is for Drg or Asc or Fee schedule then we need to consider first two columns header text is empty or not
                if ((headerCount <= Constants.Two) && (claimFieldId == (byte)(Enums.ClaimFieldTypes.DrgWeightTable) ||
                                                       claimFieldId == (byte)(Enums.ClaimFieldTypes.AscFeeSchedule) ||
                                                       claimFieldId == (byte)(Enums.ClaimFieldTypes.FeeSchedule)) &&
                    string.IsNullOrWhiteSpace(column.ToString()))
                {
                    return string.Format(Constants.BlankValueInCell, Constants.One);
                }
                // If uploaded table is for custom then we need to consider first ten columns header text is empty or not
                if (headerCount <= Constants.CustomPaymentColumnCount &&
                    claimFieldId == (int)(Enums.ClaimFieldTypes.CustomPaymentType) &&
                    string.IsNullOrWhiteSpace(column.ToString()))
                {
                    return string.Format(Constants.BlankValueInCell, Constants.One);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Validates the invalid header.
        /// </summary>
        /// <param name="claimFieldId">The claim field identifier.</param>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <returns></returns>
        private static string ValidateInvalidHeaderCsv(long claimFieldId, DataTable claimFieldValuesDataTable)
        {
            switch (claimFieldId)
            {
                case (byte)(Enums.ClaimFieldTypes.FeeSchedule):
                case (byte)(Enums.ClaimFieldTypes.AscFeeSchedule):
                case (byte)(Enums.ClaimFieldTypes.DrgWeightTable):
                    // Checking for any duplicate headers 
                    if (claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.Zero].ToString().Trim()
                        .ToLower()
                        .Equals(
                            claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.One].ToString()
                                .Split(Constants.CommaCharacter)
                                .ToList()[Constants.Zero].ToLower().Trim()))
                    {
                        return Constants.SameHeaderName;
                    }

                    // Checking column header is having any special character [, ] and number from 0 to 9 at starting of the string
                    if (
                        Regex.IsMatch(
                            claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.Zero].ToString(),
                            Constants.RegexDigitPattern) ||
                        Regex.IsMatch(claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.One].ToString()
                            .Split(Constants.CommaCharacter)[Constants.Zero], Constants.RegexDigitPattern))
                    {
                        return Constants.InvalidHeaderErrorMessage;
                    }
                    break;
                case (byte)(Enums.ClaimFieldTypes.CustomPaymentType):
                    {
                        int length =
                            claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.One].ToString()
                                .Split(Constants.CommaCharacter).Length;
                        //FIXED-SEP15 - No need to write entire if else operator. Inside Take() only we can write ternary operator or Math.Min 
                        //Taking list of strings based on length 
                        IEnumerable<string> values =
                            claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.One].ToString()
                                .Split(Constants.CommaCharacter)
                                .Take(Math.Min(Constants.CustomPaymentColumnCount - Constants.One,
                                    length)).ToList();

                        // Checking for any duplicate headers 
                        if (values.GroupBy(item => item.Trim(), StringComparer.InvariantCultureIgnoreCase)
                            .Any(count => count.Count() > Constants.One) ||
                            values.Any(
                                value =>
                                    String.Equals(value.Trim(),
                                        claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.Zero].ToString()
                                            .Trim(),
                                        StringComparison.CurrentCultureIgnoreCase)))
                        {
                            return Constants.SameHeaderName;
                        }

                        // Checking column header is having any special character [, ] and number from 0 to 9 at starting of the string
                        if (values.Any(
                            columnHeaderText =>
                                Regex.IsMatch(columnHeaderText, Constants.RegexDigitPattern) ||
                                SpecialCharacterList.Any(columnHeaderText.Contains) ||
                                columnHeaderText.Split(Constants.CommaCharacter)
                                    .ToList()
                                    .Any(
                                        item =>
                                            Regex.IsMatch(item, Constants.RegexDigitPattern) ||
                                            SpecialCharacterList.Any(item.Contains))))
                        {
                            return Constants.InvalidHeaderErrorMessage;
                        }
                    }
                    break;
            }
            return string.Empty;
        }

        /// <summary>
        /// Validates the invalid header CSV.
        /// </summary>
        /// <param name="claimFieldId">The claim field identifier.</param>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <returns></returns>
        private static string ValidateInvalidHeaderExcel(long claimFieldId, DataTable claimFieldValuesDataTable)
        {
            switch (claimFieldId)
            {
                case (byte)(Enums.ClaimFieldTypes.FeeSchedule):
                case (byte)(Enums.ClaimFieldTypes.AscFeeSchedule):
                case (byte)(Enums.ClaimFieldTypes.DrgWeightTable):
                    if (
                        claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.Zero].ToString()
                            .ToLower()
                            .Equals(
                                claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray[Constants.One].ToString()
                                    .ToLower()))
                    {
                        return Constants.SameHeaderName;
                    }
                    // Checking column header is having any special character [, ] and number from 0 to 9 at starting of the string only for first 2 columns
                    if (claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray.Take(Constants.Two)
                        .ToList()
                        .Cast<string>()
                        .Any(
                            columnHeaderText =>
                                Regex.IsMatch(columnHeaderText, Constants.RegexDigitPattern) ||
                                SpecialCharacterList.Any(columnHeaderText.Contains)))
                    {
                        return Constants.InvalidHeaderErrorMessage;
                    }
                    break;
                case (byte)(Enums.ClaimFieldTypes.CustomPaymentType):
                    {
                        int length = claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray.Length;
                        List<string> values =
                            claimFieldValuesDataTable.Rows[Constants.Zero].ItemArray.Take(Math.Min(length,
                                Constants.CustomPaymentColumnCount)).Cast<string>().ToList();

                        if (values.GroupBy(item => item, StringComparer.InvariantCultureIgnoreCase)
                            .Any(count => count.Count() > Constants.One))
                        {
                            return Constants.SameHeaderName;
                        }
                        // Checking column header is having any special character [, ] and number from 0 to 9 at starting of the string
                        if (
                            values
                                .Any(
                                    columnHeaderText =>
                                        Regex.IsMatch(columnHeaderText, Constants.RegexDigitPattern) ||
                                        SpecialCharacterList.Any(columnHeaderText.Contains)))
                        {
                            return Constants.InvalidHeaderErrorMessage;
                        }
                    }
                    break;
            }
            return string.Empty;
        }

        /// <summary>
        /// Validates the Hcpcs Code Length.
        /// </summary>
        /// <param name="code">The Hcpcs code.</param>
        /// <returns></returns>
        private static bool ValidateHcpcsCodeLength(string code)
        {
            int length = code.Trim().Length;
            //Hcpcs code length should be 5/7/9/11/13/15
            return length == 5 || length == 7 || length == 9 || length == 11 || length == 13 || length == 15;
        }

        /// <summary>
        /// Determines whether the specified claim field values data table is duplicate.
        /// </summary>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <returns></returns>
        private static List<IGrouping<string, DataRow>> IsDuplicate(DataTable claimFieldValuesDataTable)
        {
            // Checking for duplicates
            return
                claimFieldValuesDataTable.AsEnumerable()
                    .GroupBy(rows => rows[Constants.Zero].ToString(),
                        StringComparer.InvariantCultureIgnoreCase)
                    .Where(group => group.Count() > 1)
                    .ToList();
        }

        /// <summary>
        /// Gets the claim field document.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="claimFieldId">The claim identifier.</param>
        /// <param name="facilityId">The facility identifier.</param>
        /// <param name="claimFieldValuesDataTable">The claim field values data table.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public static ClaimFieldDoc GetPaymentTableClaimFields(string tableName, string fileName, string fileExtension,
            long claimFieldId, int facilityId, DataTable claimFieldValuesDataTable,string userName)
        {
            ClaimFieldDoc claimFieldDoc = new ClaimFieldDoc();
           
           if (claimFieldValuesDataTable.Columns.Count > 0)
            {
                claimFieldDoc = new ClaimFieldDoc
                {
                    ColumnHeaderFirst = claimFieldValuesDataTable.Rows[Constants.Zero][Constants.Zero].ToString().Trim(),
                    //1st column of Header data
                    ColumnHeaderSecond =
                        claimFieldValuesDataTable.Rows == null || claimFieldValuesDataTable.Columns.Count < 2
                            ? string.Empty
                            : claimFieldValuesDataTable.Rows[Constants.Zero][Constants.One].ToString().Trim(),
                    //2nd column of Header data
                    FileName = fileName,
                    TableName = tableName,
                    ClaimFieldId = claimFieldId,
                    FacilityId = facilityId,
                    ClaimFieldValues = new List<ClaimFieldValue>(),
                    UserName = userName
                };

                if (claimFieldId != (byte)(Enums.ClaimFieldTypes.CustomPaymentType))
                {
                    //Delete Header Row (Header cant be store as content)
                    if (claimFieldValuesDataTable.Rows != null)
                    {
                        claimFieldValuesDataTable.Rows.Remove(claimFieldValuesDataTable.Rows[Constants.Zero]);
                        if (String.Equals(fileExtension, Enums.DownloadFileType.Csv.ToString(),
                            StringComparison.CurrentCultureIgnoreCase))
                        {
                            claimFieldValuesDataTable.Rows[Constants.Zero][Constants.AmountField] =
                                claimFieldValuesDataTable.Rows[Constants.Zero][Constants.AmountField].ToString()
                                    .Split(Constants.CommaCharacter)
                                    .ToList()
                                    .Take(1)
                                    .First().Trim();
                            if (claimFieldValuesDataTable.Rows != null)
                                for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
                                {
                                    claimFieldValuesDataTable.Rows[count][Constants.One] = claimFieldValuesDataTable
                                        .Rows[count].ItemArray[Constants.One].ToString()
                                        .Split(Constants.CommaCharacter)
                                        .ToList()
                                        .Take(1)
                                        .First().Trim();
                                }
                        }
                        // Logic to add ClaimField Values
                        for (int rowCount = 0; rowCount < claimFieldValuesDataTable.Rows.Count; rowCount++)
                        {
                            //Check whether row has min 2 columns or not
                            if (claimFieldValuesDataTable.Columns.Count >= Constants.MinColumnForUploadedFile)
                            {
                                ClaimFieldValue claimFieldValue = new ClaimFieldValue
                                {
                                    Identifier =
                                        claimFieldValuesDataTable.Rows[rowCount][Constants.Zero].ToString()
                                            .Trim()
                                            .Replace(Constants.ReplaceCommaInString, string.Empty)
                                            .Replace(Constants.CommaReplaceString, string.Empty),
                                    Value =
                                        claimFieldValuesDataTable.Rows[rowCount][Constants.One].ToString()
                                            .Trim()
                                            .Replace(Constants.ReplaceCommaInString, string.Empty)
                                            .Replace(Constants.CommaReplaceString, string.Empty)
                                };

                                //Check Identifier and Value are empty or not. Either one should not be empty to save data
                                if ((!string.IsNullOrEmpty(claimFieldValue.Identifier) &&
                                     !string.IsNullOrWhiteSpace(claimFieldValue.Identifier)) ||
                                    (!string.IsNullOrEmpty(claimFieldValue.Value) &&
                                     !string.IsNullOrWhiteSpace(claimFieldValue.Value)))
                                {
                                    //Remove $ sign from value (Used for amount $400 should be store as 400)
                                    if (claimFieldValue.Value.Contains(Constants.Currency))
                                        claimFieldValue.Value = claimFieldValue.Value.Replace(Constants.Currency,
                                            string.Empty).Trim();

                                    claimFieldDoc.ClaimFieldValues.Add(claimFieldValue);
                                }
                            }
                        }
                    }
                }
                else
                {
                    StringBuilder headerValues = new StringBuilder();
                    StringBuilder rowValues = new StringBuilder();
                    DataTable claimFieldValues = new DataTable();
                    if (!String.Equals(fileExtension, Enums.DownloadFileType.Csv.ToString(),
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        // Taking the first 10 columns for uploading
                        for (var count = 0; count < 10 && count < claimFieldValuesDataTable.Columns.Count; count++)
                        {
                            claimFieldValues.Columns.Add(claimFieldValuesDataTable.Columns[count].ColumnName,
                                claimFieldValuesDataTable.Columns[Constants.One].DataType);
                        }
                        claimFieldValues.BeginLoadData();
                        if (claimFieldValuesDataTable.Rows != null)
                            foreach (DataRow dataRow in claimFieldValuesDataTable.Rows)
                            {
                                claimFieldValues.Rows.Add(
                                    Enumerable.Range(0, dataRow.ItemArray.Length > 10 ? 10 : dataRow.ItemArray.Length)
                                        .Select(item => dataRow[item].ToString().Trim())
                                        .Cast<object>()
                                        .ToArray());
                            }
                        claimFieldValues.EndLoadData();
                        claimFieldValuesDataTable = claimFieldValues;
                    }
                    else
                    {
                        IEnumerable<string> columnCount =
                            claimFieldValuesDataTable.Rows[Constants.Zero][Constants.AmountField].ToString()
                                .Split(Constants.CommaCharacter)
                                .ToList()
                                .Take(9);
                        claimFieldValuesDataTable.Rows[Constants.Zero][Constants.AmountField] =
                            string.Join(Constants.Comma, columnCount.Select(item => item.Trim()));
                        if (claimFieldValuesDataTable.Rows != null)
                            for (int count = Constants.One; count < claimFieldValuesDataTable.Rows.Count; count++)
                            {
                                string rowValue =
                                    claimFieldValuesDataTable.Rows[count].ItemArray[Constants.One].ToString();
                                Regex reg = new Regex("\".*?\"");
                                MatchCollection matches = reg.Matches(rowValue);
                                rowValue = matches.Cast<object>()
                                    .Aggregate(rowValue,
                                        (current, item) =>
                                            current.Replace(item.ToString(),
                                                item.ToString().Replace(Constants.Comma, Constants.UnderScoreCharacter)));
                                Regex reg1 = new Regex("\"\".*?\"\"");
                                MatchCollection matches1 = reg1.Matches(rowValue);
                                rowValue = matches1.Cast<object>()
                                    .Aggregate(rowValue,
                                        (current, item) =>
                                            current.Replace(item.ToString(),
                                                item.ToString().Replace(Constants.Comma, Constants.UnderScoreCharacter)));
                                Regex reg2 = new Regex("#&#.*?#&#");
                                MatchCollection matches2 = reg2.Matches(rowValue);
                                rowValue = matches2.Cast<object>()
                                    .Aggregate(rowValue,
                                        (current, item) =>
                                            current.Replace(item.ToString(),
                                                item.ToString().Replace(Constants.Comma, Constants.UnderScoreCharacter)));
                                IEnumerable<string> rowItems = rowValue
                                    .Split(Constants.CommaCharacter)
                                    .ToList()
                                    .Take(9);
                                claimFieldValuesDataTable.Rows[count][Constants.One] = string.Join(Constants.Comma,
                                    rowItems.Select(item => item.Trim()));
                                claimFieldValuesDataTable.Rows[count][Constants.One] =
                                    claimFieldValuesDataTable.Rows[count][Constants.One].ToString()
                                        .Replace(Constants.ReplaceCommaInString, Constants.Comma)
                                        .Replace(Constants.UnderScoreCharacter, Constants.Comma);
                            }
                    }
                    bool isHeaderRow = true;
                    if (claimFieldValuesDataTable.Rows != null)
                        foreach (DataRow row in claimFieldValuesDataTable.Rows)
                        {
                            //Custom payment table initialization
                            for (int i = 0; i < claimFieldValuesDataTable.Columns.Count; i++)
                            {
                                if (isHeaderRow)
                                {
                                    if (String.Equals(fileExtension, Enums.DownloadFileType.Csv.ToString(),
                                        StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        headerValues.Append(string.Join(Constants.Comma,
                                            Utilities.SplitCsvRowToArray(row[i].ToString())
                                                .Select(
                                                    x =>
                                                        x.Trim().Replace(Constants.Comma, Constants.MaskCommaInValue)
                                                            .Replace(Constants.Apostrophe,
                                                                Constants.Apostrophe + Constants.Apostrophe)
                                                            .Replace(Constants.CommaReplaceString, string.Empty)
                                                            .Replace(Constants.AmpercentString,
                                                                Constants.AmpercentReplaceString)
                                                            .Replace(Constants.LessThanString,
                                                                Constants.LessThanReplaceString))));
                                    }
                                    else
                                    {
                                        headerValues.Append(
                                            row[i].ToString().Trim()
                                                .Replace(Constants.Comma, Constants.MaskCommaInValue)
                                                .Replace(Constants.Apostrophe,
                                                    Constants.Apostrophe + Constants.Apostrophe)
                                                .Replace(Constants.AmpercentString,
                                                    Constants.AmpercentReplaceString)
                                                .Replace(Constants.LessThanString,
                                                    Constants.LessThanReplaceString));
                                    }
                                    headerValues.Append(i == claimFieldValuesDataTable.Columns.Count - 1
                                        ? Constants.NewLine
                                        : Constants.Comma);
                                }
                                else
                                {
                                    //Read comma row values
                                    if (String.Equals(fileExtension, Enums.DownloadFileType.Csv.ToString(),
                                        StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        if (i > 0)
                                        {
                                            string rowValue = row[i].ToString();
                                            Regex reg = new Regex("\".*?\"");
                                            MatchCollection matches = reg.Matches(rowValue);
                                            rowValue = matches.Cast<object>()
                                                .Aggregate(rowValue,
                                                    (current, item) =>
                                                        current.Replace(item.ToString(),
                                                            item.ToString()
                                                                .Replace(Constants.Comma, Constants.UnderScoreCharacter)));
                                            Regex reg1 = new Regex("\"\".*?\"\"");
                                            MatchCollection matches1 = reg1.Matches(rowValue);
                                            rowValue = matches1.Cast<object>()
                                                .Aggregate(rowValue,
                                                    (current, item) =>
                                                        current.Replace(item.ToString(),
                                                            item.ToString()
                                                                .Replace(Constants.Comma, Constants.UnderScoreCharacter)));
                                            Regex reg2 = new Regex("#&#.*?#&#");
                                            MatchCollection matches2 = reg2.Matches(rowValue);
                                            rowValue = matches2.Cast<object>()
                                                .Aggregate(rowValue,
                                                    (current, item) =>
                                                        current.Replace(item.ToString(),
                                                            item.ToString()
                                                                .Replace(Constants.Comma, Constants.UnderScoreCharacter)));
                                            List<string> rowsList = Utilities.SplitCsvRowToArray(rowValue);
                                            rowsList = rowsList.Select(
                                                x =>
                                                    x.Replace(Constants.UnderScoreCharacter, Constants.Comma)).ToList();
                                            rowValues.Append(string.Join(Constants.Comma,
                                                rowsList
                                                    .Select(
                                                        x =>
                                                            x.Trim()
                                                                .Replace(Constants.Comma, Constants.MaskCommaInValue)
                                                                .Replace(Constants.Apostrophe,
                                                                    Constants.Apostrophe + Constants.Apostrophe)
                                                                .Replace(Constants.CommaReplaceString, string.Empty)
                                                                .Replace(Constants.AmpercentString,
                                                                    Constants.AmpercentReplaceString)
                                                                .Replace(Constants.LessThanString,
                                                                    Constants.LessThanReplaceString))));
                                        }
                                        else
                                            rowValues.Append(
                                                row[i].ToString().Trim()
                                                    .Replace(Constants.Comma, Constants.MaskCommaInValue)
                                                    .Replace(Constants.Apostrophe,
                                                        Constants.Apostrophe + Constants.Apostrophe)
                                                    .Replace(Constants.CommaReplaceString, string.Empty)
                                                    .Replace(Constants.AmpercentString,
                                                        Constants.AmpercentReplaceString)
                                                    .Replace(Constants.LessThanString,
                                                        Constants.LessThanReplaceString));
                                    }
                                    else
                                    {
                                        if (i < 10)
                                            rowValues.Append(
                                                row[i].ToString().Trim()
                                                    .Replace(Constants.Comma, Constants.MaskCommaInValue)
                                                    .Replace(Constants.Apostrophe,
                                                        Constants.Apostrophe + Constants.Apostrophe)
                                                    .Replace(Constants.AmpercentString,
                                                        Constants.AmpercentReplaceString)
                                                    .Replace(Constants.LessThanString,
                                                        Constants.LessThanReplaceString));
                                    }
                                    rowValues.Append(i == claimFieldValuesDataTable.Columns.Count - 1
                                        ? Constants.NewLine
                                        : Constants.Comma);
                                }
                            }
                            isHeaderRow = false;
                        }
                    //Set Claim field doc column header properties
                    claimFieldDoc.ColumnHeaderFirst = headerValues.ToString();
                    claimFieldDoc.ColumnHeaderSecond = null;
                    //Add Claim field value along with column header properties
                    claimFieldDoc.ClaimFieldValues.Add(new ClaimFieldValue
                    {
                        FacilityId = claimFieldDoc.FacilityId,
                        ColumnHeaderFirst = headerValues.ToString(),
                        ColumnHeaderSecond = null,
                        ClaimFieldDocId = claimFieldDoc.ClaimFieldDocId,
                        Value = rowValues.ToString(),
                        Identifier = null
                    });
                }
            }
            return claimFieldDoc;
        }
    }
}