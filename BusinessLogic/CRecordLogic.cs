using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
    public class CRecordLogic : ICRecordLogic
    {
        public string ValidationErrors { get; private set; }

        private const string InvalidBloodPrint = "Invalid bloodprint. ";
        private const string InvalidOppsFlag = "Invalid OppsFlag. ";
        private const string InvalidGender = "Invalid gender. ";
        private const string InvalidAge = "Invalid age. ";
        private const string InvalidClaimId = "Invalid claimid";
        private const string InvalidConditionCode = "Invalid condition code";
        private const string InvalidBillType = "Invalid billtype";
        private const string InvalidNpi = "Invalid NPI";
        private const string InvalidOscar = "Invalid Oscar";
        private const string InvalidPatientStatus = "Invalid patientStatus";
        private const string InvalidOccuranceCode = "Invalid occurrence code";
        private const string InvalidBeneficiary = "Invalid Beneficiary deductible amount";
        private const string InvalidPatientData = "Invalid Patient Data ";
        private const int BloodPintMinValue = 0;
        private const int BloodPintMaxValue = 3;
        private const int OppsFlagMinValue = 1;
        private const int OppsFlagMaxValue = 2;
        private const int GenderOption = 0;
        private const int GenderOption1 = 1;
        private const int GenderOption2 = 2;
        private const int MinAge = 0;
        private const int MaxAge = 124;
        private const int ClaimIdLength = 17;
        private const int ConditionCodeLength = 2;
        private const int BillTypeLength = 3;
        private const int NpiLength = 13;
        private const int OscarLenth = 6;
        private const int PatientStatusLength = 2;
        private const int OccuranceCodeLength = 2;
        private const int FirstNameLength = 15;
        private const int LastNameLength = 20;
        private const int BeneficiaryAmountLength = 7;
        private const string DecimalFormat = "D5";
        private const string FixedPointFormat = "F0";
        private const string DecimalPoints = "00";
        private const string RegexFormat = @"\s+";
        private const string IntegralTypeFormat = "D2";

        /// <summary>
        /// Gets the c record line.
        /// </summary>
        /// <param name="cRecordDataRaw">The c record data raw.</param>
        /// <returns></returns>
        public string GetCRecordLine(CRecord cRecordDataRaw)
        {
            //We assume that cRecordDataRaw does not have any null properties. If the db returns null, the data access 
            //layer will be replacing the nulls with string.Empty or 0.
            string cRecord;
            // Validating the C record
            ValidationErrors = ValidateCRecord(cRecordDataRaw);
            if (cRecordDataRaw != null && string.IsNullOrEmpty(ValidationErrors.Trim()))
            {
                cRecord = BuildCRecordString(cRecordDataRaw);
            }
            else
            {
                cRecord = string.Empty;
            }
            return cRecord;
        }

        /// <summary>
        ///  Validations for C Line Raw Data
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static string ValidateCRecord(CRecord cRaw)
        {
            try
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4} {5} {6} {7} {8} ",
                    ValidatePatientData(cRaw.PatientData, cRaw.ThruDate),
                    ValidateOppsFlag(cRaw.OppsFlag),
                    ValidateBloodpint(cRaw.BloodPint),
                    ValidateClaimId(cRaw.ClaimId),
                    ValidateConditionCodes(cRaw.ConditionCodes),
                    ValidateBillType(cRaw.BillType),
                    ValidateNpi(cRaw.Npi),
                    ValidateOccuranceCodes(cRaw.OccurrenceCodes),
                    ValidateBeneficiaryAmount(cRaw.BeneAmount)
                    );
            }
            catch (Exception ex)
            {
                Log.LogError(string.Empty, string.Empty, ex);
                return "Exception occured";
            }
        }

        /// <summary>
        /// Validates the patient data.
        /// </summary>
        /// <param name="patientData">The patient data.</param>
        /// <param name="thruDate">The thru date.</param>
        /// <returns></returns>
        private static string ValidatePatientData(PatientData patientData, DateTime thruDate)
        {
            if (patientData == null)
            {
                return InvalidPatientData;
            }
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} ",
                ValidateAge(patientData.Dob, thruDate),
                ValidateSex(patientData.Gender),
                ValidateOscar(patientData.Medicare),
                ValidatePatientStatus(patientData.Status));
        }

        /// <summary>
        ///  Validate Blood pint
        /// </summary>
        private static string ValidateBloodpint(int bloodPint)
        {
            // bloodPint should be 0 to 3
            return bloodPint >= BloodPintMinValue && bloodPint <= BloodPintMaxValue ? string.Empty : InvalidBloodPrint;
        }

        /// <summary>
        ///  Validate OPPS/non-OPPS Flag
        /// </summary>
        private static string ValidateOppsFlag(int oppsFlag)
        {
            // oppsFlag is either 1 or 2
            return oppsFlag == OppsFlagMinValue || oppsFlag == OppsFlagMaxValue ? string.Empty : InvalidOppsFlag;
        }

        /// <summary>
        ///  Validate Sex (only 0,1 and 2 are allowed)
        /// </summary>
        private static string ValidateSex(int gender)
        {
            // gender is either 0 or 1 0r 2
            return gender.Equals(GenderOption) || gender.Equals(GenderOption1) || gender.Equals(GenderOption2) ? string.Empty : InvalidGender;
        }

        /// <summary>
        ///  Validate if Age is valid
        /// </summary>
        private static string ValidateAge(DateTime dob, DateTime thruDate)
        {
            // based on thru date age is calculating and it should be between 0 to 124
            int age = thruDate.Year - dob.Year;
            if (dob > thruDate.AddYears(-age)) age--;
            return age < MinAge || age > MaxAge ? InvalidAge : string.Empty;
        }

        /// <summary>
        /// Validates the claim identifier.
        /// </summary>
        /// <param name="claimId">The claim identifier.</param>
        /// <returns></returns>
        private static string ValidateClaimId(string claimId)
        {
            // claimId length should be less than 18
            return string.IsNullOrEmpty(claimId) || claimId.Trim().Length <= ClaimIdLength ? string.Empty : InvalidClaimId;
        }

        /// <summary>
        /// Validates the condition codes.
        /// </summary>
        /// <param name="conditionCodes">The condition codes.</param>
        /// <returns></returns>
        private static string ValidateConditionCodes(IEnumerable<string> conditionCodes)
        {
            // Condition code length should be 2 and count should be less than 7
            return conditionCodes == null || (conditionCodes.All(x => x.Trim().Length == ConditionCodeLength))
                ? string.Empty
                : InvalidConditionCode;
        }

        /// <summary>
        /// Validates the type of the bill.
        /// </summary>
        /// <param name="billType">Type of the bill.</param>
        /// <returns></returns>
        private static string ValidateBillType(string billType)
        {
            // Bill type length should be 3
            return string.IsNullOrEmpty(billType) || billType.Trim().Length == BillTypeLength
                ? string.Empty
                : InvalidBillType;
        }

        /// <summary>
        /// Validates the npi.
        /// </summary>
        /// <param name="npi">The npi.</param>
        /// <returns></returns>
        private static string ValidateNpi(string npi)
        {
            // Npi length should be less than 14
            return string.IsNullOrEmpty(npi) || npi.Trim().Length <= NpiLength ? string.Empty : InvalidNpi;
        }

        /// <summary>
        /// Validates the oscar.
        /// </summary>
        /// <param name="oscar">The oscar.</param>
        /// <returns></returns>
        private static string ValidateOscar(string oscar)
        {
            // Oscar length should be less than 7
            return string.IsNullOrEmpty(oscar) || oscar.Trim().Length <= OscarLenth ? string.Empty : InvalidOscar;
        }

        /// <summary>
        /// Validates the patient status.
        /// </summary>
        /// <param name="patientStatus">The patient status.</param>
        /// <returns></returns>
        private static string ValidatePatientStatus(string patientStatus)
        {
            //If patient Status is null or empty, provides Invalid Patient Status
            return string.IsNullOrEmpty(patientStatus) ? InvalidPatientStatus : string.Empty;
        }

        /// <summary>
        /// Validates the occurrence codes.
        /// </summary>
        /// <param name="occuranceCodes">The occurrence codes.</param>
        /// <returns></returns>
        private static string ValidateOccuranceCodes(IEnumerable<string> occuranceCodes)
        {
            // Occurrence Code length should be 2 and count should be less than 10
            return occuranceCodes == null || (occuranceCodes.All(x => x.Trim().Length == OccuranceCodeLength))
                ? string.Empty
                : InvalidOccuranceCode;
        }
        
        /// <summary>
        /// Validates the beneficiary amount.
        /// </summary>
        /// <param name="beneficiaryAmount">The beneficiary amount.</param>
        /// <returns></returns>
        private static string ValidateBeneficiaryAmount(double beneficiaryAmount)
        {
            // BeneficiaryAmount length should be less than 9
            return Convert.ToString(beneficiaryAmount, CultureInfo.InvariantCulture).Length <= BeneficiaryAmountLength ? string.Empty : InvalidBeneficiary;
        }

        /// <summary>
        /// Build C record string
        /// </summary>
        /// <param name="cRecordRaw">C record string.</param>
        /// <returns>string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "c")]
        public static string BuildCRecordString(CRecord cRecordRaw)
        {
            StringBuilder cRecordString = new StringBuilder(Constants.Stringlength);

            if (cRecordRaw != null)
            {
                try
                {
                    cRecordString = cRecordString.Append(Constants.CRecordString)
                                                .Append(BuildClaimId(cRecordRaw.ClaimId))
                                                .Append(BuildAge(cRecordRaw.PatientData.Dob, cRecordRaw.ThruDate))
                                                .Append(BuildSex(cRecordRaw.PatientData.Gender))
                                                .Append(BuildFromDate(cRecordRaw.FromDate))
                                                .Append(BuildThruDate(cRecordRaw.ThruDate))
                                                .Append(BuildConditionCodes(cRecordRaw.ConditionCodes))
                                                .Append(BuildBillType(cRecordRaw.BillType))
                                                .Append(BuildNpi(cRecordRaw.Npi))
                                                .Append(BuildOscar(cRecordRaw.PatientData.Medicare))
                                                .Append(BuildPatientStatus(Convert.ToInt32(cRecordRaw.PatientData.Status, CultureInfo.InvariantCulture)))
                                                .Append(BuildOppsFlag(cRecordRaw.OppsFlag))
                                                .Append(BuildOccurrenceCodes(cRecordRaw.OccurrenceCodes))
                                                .Append(BuildPatientLastName(cRecordRaw.PatientData.LastName))
                                                .Append(BuildPatientFirstName(cRecordRaw.PatientData.FirstName))
                                                .Append(BuildPatientMiddleInitial(cRecordRaw.PatientData.MiddleName))
                                                .Append(BuildBeneAmount(cRecordRaw.BeneAmount))
                                                .Append(BuildBloodPint(cRecordRaw.BloodPint))
                                                .Append(BuildUnUsed());

                    if (cRecordString.Length == Constants.Stringlength)
                        return cRecordString.ToString();

                    return string.Format(CultureInfo.InvariantCulture, "{0,-200}", cRecordString);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Build Unused string
        /// </summary>
        /// <returns>string</returns>
        private static string BuildUnUsed()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-59}", string.Empty);
        }

        /// <summary>
        /// Build Blood Pint string
        /// </summary>
        /// <returns>string</returns>
        private static string BuildBloodPint(int bloodPint)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-1}", bloodPint);
        }

        /// <summary>
        /// Build Beneficit Amount
        /// </summary>
        /// <returns>string</returns>
        private static string BuildBeneAmount(double beneAmount)
        {
                double decimalValue = beneAmount - Math.Truncate(beneAmount);
                string stringDecimalValue = decimalValue > 0 ? ((decimalValue % 1) * 100).ToString(FixedPointFormat, CultureInfo.InvariantCulture) : DecimalPoints;
                return Convert.ToString(Convert.ToInt32(Math.Truncate(beneAmount), CultureInfo.InvariantCulture).ToString(DecimalFormat, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) + stringDecimalValue;
        }

        /// <summary>
        /// Build Patient Middle Initial
        /// </summary>
        /// <returns>string</returns>
        private static string BuildPatientMiddleInitial(string patientMiddleInitial)
        {
            //If PatientMiddleName length is greater than !, then consider the left most 1 character
            patientMiddleInitial = Regex.Replace(patientMiddleInitial, RegexFormat, string.Empty);
            return String.Format(CultureInfo.InvariantCulture, "{0,-1}", (patientMiddleInitial.Length <= Constants.One ? patientMiddleInitial :patientMiddleInitial.Trim().Substring(0, 1)));
        }

        /// <summary>
        /// Build PatientLastName
        /// </summary>
        /// <returns>string</returns>
        private static string BuildPatientLastName(string patientLastName)
        {
            patientLastName = Regex.Replace(patientLastName, RegexFormat, string.Empty).Trim();
            return String.Format(CultureInfo.InvariantCulture, "{0,-20}", (patientLastName.Length <= LastNameLength ? patientLastName : patientLastName.Substring(0, LastNameLength)));
        }

        /// <summary>
        /// Build Patient FirstName
        /// </summary>
        /// <returns>string</returns>
        private static string BuildPatientFirstName(string patientFirstName)
        {
            patientFirstName = Regex.Replace(patientFirstName, RegexFormat, string.Empty).Trim();
            return String.Format(CultureInfo.InvariantCulture, "{0,-15}", (patientFirstName.Length <= FirstNameLength ? patientFirstName : patientFirstName.Substring(0, FirstNameLength)));

        }

        /// <summary>
        /// Build Occurrence Codes
        /// </summary>
        /// <returns>string</returns>
        private static string BuildOccurrenceCodes(List<string> occurrenceCodes)
        {
            var codeString = new StringBuilder(20);
            //Check for Null value
            if (occurrenceCodes != null && occurrenceCodes.Count > 0)
            {
                foreach (var code in occurrenceCodes)
                {
                    if (code.Length == 2)
                        codeString = codeString.Append(code.Trim());
                    if (codeString.Length == 20)
                        break;
                }
            }
            return String.Format(CultureInfo.InvariantCulture, "{0,-20}", codeString);
        }

        /// <summary>
        /// Build OPPS Flag
        /// </summary>
        /// <returns>string</returns>
        private static string BuildOppsFlag(int oppsFlag)
        {
            return oppsFlag.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Build Patient Status
        /// </summary>
        /// <returns>string</returns>
        private static string BuildPatientStatus(int patientStatus)
        {
            string patientStatusString = patientStatus.ToString(IntegralTypeFormat, CultureInfo.InvariantCulture);
            //If patient status length is greater than 2, will get the right most 2 charcters
            return patientStatusString.Length <= PatientStatusLength ? patientStatusString : patientStatusString.Substring(patientStatusString.Length - PatientStatusLength);
        }

        /// <summary>
        /// Build OSCAR
        /// </summary>
        /// <returns>string</returns>
        private static string BuildOscar(string oscar)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-6}", oscar.Trim());
        }

        /// <summary>
        /// Build NPI
        /// </summary>
        /// <returns>string</returns>
        private static string BuildNpi(string npi)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-13}", npi.Trim());
        }

        /// <summary>
        /// Build BillType
        /// </summary>
        /// <returns>string</returns>
        private static string BuildBillType(string billType)
        {
            return string.IsNullOrEmpty(billType) ? string.Empty : String.Format(CultureInfo.InvariantCulture, "{0,-3}", billType.Trim());
        }

        /// <summary>
        /// Build Condition Codes
        /// </summary>
        /// <returns>string</returns>
        private static string BuildConditionCodes(List<string> conditionCodes)
        {
            var codeString = new StringBuilder(14);
            //Check for null value
            if (conditionCodes != null && conditionCodes.Count > 0)
            {
                foreach (var code in conditionCodes)
                {
                    if (code.Length == 2)
                        codeString = codeString.Append(code.Trim());
                    if (codeString.Length == 14)
                        break;
                }
            }
            return String.Format(CultureInfo.InvariantCulture, "{0,-14}", codeString);
        }

        /// <summary>
        /// Build Through Date
        /// </summary>
        /// <returns>string</returns>
        private static string BuildThruDate(DateTime thruDate)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0:yyyyMMdd}", thruDate);
        }

        /// <summary>
        /// Build From Date
        /// </summary>
        /// <returns>string</returns>
        private static string BuildFromDate(DateTime fromDate)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0:yyyyMMdd}", fromDate);
        }

        /// <summary>
        /// Build Sex
        /// </summary>
        /// <returns>string</returns>
        private static string BuildSex(int p)
        {
            return p.ToString(CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Builds the age.
        /// </summary>
        /// <param name="dobirth">The dobirth.</param>
        /// <param name="statementThruDate">The statement thru date.</param>
        /// <returns></returns>
        private static string BuildAge(DateTime dobirth, DateTime statementThruDate)
        {
            DateTime dob = Convert.ToDateTime(dobirth);
            DateTime thruDate = Convert.ToDateTime(statementThruDate);
            int age = thruDate.Year - dob.Year;
            if (dob > thruDate.AddYears(-age)) age--;
            return String.Format(CultureInfo.InvariantCulture, "{0,3}", age);
        }

        /// <summary>
        /// Build ClaimId
        /// </summary>
        /// <returns>string</returns>
        private static string BuildClaimId(string claimId)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-17}", claimId.Trim());
        }
    }
}
