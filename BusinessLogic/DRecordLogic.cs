using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class DRecordLogic : IDRecordLogic
    {
        public string ValidationErrors { get; private set; }

        private const string InvalidSecondaryDiagnosis = "Invalid secondary diagnosis code";
        private const string InvalidClaimId = "Invalid claimid";
        private const int DiagnosisCodeLength = 6;
        private const int ClaimIdLength = 17;
        private const string None = "NONE";
        private const int StringBuilderCapacity = 6;
        private const int StringBuilderDiagnosisCapacity = 84;

        /// <summary>
        /// Gets the d record line.
        /// </summary>
        /// <param name="dRecordData"></param>
        /// <returns></returns>
        public string GetDRecordLine(DRecord dRecordData)
        {
            //We assume that dRecordDataRaw does not have any null properties. If the db returns null, the data access 
            //layer will be replacing the nulls with string.Empty or 0.
            string dRecord;
            // Validating the D record
            ValidationErrors = ValidateDRecord(dRecordData);
            if (dRecordData != null && string.IsNullOrEmpty(ValidationErrors.Trim()))
            {
                dRecord = BuildDRecordString(dRecordData);
            }
            else
            {
                dRecord = string.Empty;
            }
            return dRecord;
        }

        /// <summary>
        ///  Validations for D Line Raw Data
        /// </summary>
        private static string ValidateDRecord(DRecord dRecord)
        {
            if (dRecord != null)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} {1}",
                    ValidateClaimId(dRecord.ClaimId),
                    ValidateSecondaryDiagnosis(dRecord.SecondaryDiagnosisCodes));
            }
            return string.Empty;
        }

        /// <summary>
        ///  Validate list of Diagnosis
        /// </summary>
        private static string ValidateSecondaryDiagnosis(List<string> diagCode)
        {
            // Secondary Diagnosis code length should be less than 6 and count should be less than 14.
            return diagCode == null
                ? InvalidSecondaryDiagnosis
                : string.Empty;
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
        /// Build D Record String
        /// <param name="dRecord">D Record</param>
        /// </summary>
        /// <returns>string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "d")]
        public static string BuildDRecordString(DRecord dRecord)
        {
            StringBuilder dRecordString = new StringBuilder(Constants.Stringlength);

            if (dRecord != null)
            {
                dRecordString = dRecordString.Append(Constants.DRecordString);
                dRecordString = dRecordString.Append(BuildClaimId(dRecord.ClaimId));
                dRecordString = dRecordString.Append(BuildAdminDiagnosis(dRecord.AdmitDiagnosisCode));
                dRecordString = dRecordString.Append(BuildPrincipalDiagnosis(dRecord.PrincipalDiagnosisCode));
                dRecordString = dRecordString.Append(BuildSecondaryDiagnosis(dRecord.SecondaryDiagnosisCodes));
                dRecordString = dRecordString.Append(BuildUnUsed());

                if (dRecordString.Length == Constants.Stringlength)
                {
                    return dRecordString.ToString();
                }
                return string.Format(CultureInfo.InvariantCulture, "{0,-200}", dRecordString);
            }
            return string.Empty;
        }

        /// <summary>
        /// Builds the principal diagnosis.
        /// </summary>
        /// <param name="principalDiagnosisCode">The principal diagnosis code.</param>
        /// <returns></returns>
        private static string BuildPrincipalDiagnosis(string principalDiagnosisCode)
        {
            StringBuilder codeString = new StringBuilder(StringBuilderCapacity);
            if (principalDiagnosisCode != null && principalDiagnosisCode.Trim().Equals(None))
                codeString = codeString.Append(string.Empty);
            else if (principalDiagnosisCode != null)
            {
                //If code contains ".", replaces it with empty string and trims to 6 character if it is greater than 6 characters
                principalDiagnosisCode = principalDiagnosisCode.IndexOf(Constants.Dot, StringComparison.Ordinal) != -1
                            ? principalDiagnosisCode.Replace(Constants.Dot, string.Empty).Trim()
                            : principalDiagnosisCode.Trim();
                codeString =
                    codeString.Append(String.Format(CultureInfo.InvariantCulture, "{0,-6}",
                        principalDiagnosisCode.Length <= DiagnosisCodeLength
                            ? principalDiagnosisCode
                            : principalDiagnosisCode.Substring(0, DiagnosisCodeLength)));
            }
            return String.Format(CultureInfo.InvariantCulture, "{0,-6}", codeString);
        }

        /// <summary>
        /// Builds the admin diagnosis.
        /// </summary>
        /// <param name="adminDiagnosisCode">The admin diagnosis code.</param>
        /// <returns></returns>
        private static string BuildAdminDiagnosis(string adminDiagnosisCode)
        {
            StringBuilder codeString = new StringBuilder(StringBuilderCapacity);
            //If code contains ".", replaces it with empty string and trims to 6 character if it is greater than 6 characters
            adminDiagnosisCode = string.IsNullOrEmpty(adminDiagnosisCode)
                                ? string.Empty
                                : (adminDiagnosisCode.IndexOf(Constants.Dot, StringComparison.Ordinal) != -1
                            ? adminDiagnosisCode.Replace(Constants.Dot, string.Empty).Trim()
                            : adminDiagnosisCode).Trim();
            codeString = codeString.Append(adminDiagnosisCode.Equals(None)
                ? string.Empty
                : String.Format(CultureInfo.InvariantCulture, "{0,-6}", adminDiagnosisCode.Length <= DiagnosisCodeLength ? adminDiagnosisCode : adminDiagnosisCode.Substring(0, DiagnosisCodeLength)));
            return String.Format(CultureInfo.InvariantCulture, "{0,-6}", codeString);
        }

        /// <summary>
        /// Build UnUsed
        /// </summary>
        /// <returns>string</returns>
        private static string BuildUnUsed()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-86}", string.Empty);
        }

        /// <summary>
        /// Build Diagnosis
        /// <param name="diagnosisCodes">Diagnosis</param>
        /// </summary>
        /// <returns>string</returns>
        private static string BuildSecondaryDiagnosis(List<string> diagnosisCodes)
        {
            StringBuilder codeString = new StringBuilder(StringBuilderDiagnosisCapacity);

            if (diagnosisCodes != null && diagnosisCodes.Count > 0)
            {
                int diagnosisCodesCount = 0;
                foreach (string code in diagnosisCodes.Select(diagnosis => string.IsNullOrEmpty(diagnosis) ? string.Empty : diagnosis))
                {
                    //If code contains ".", replaces it with empty string and trims to 6 character if it is greater than 6 characters
                    var codeValue = code.IndexOf(Constants.Dot, StringComparison.Ordinal) != -1
                            ? code.Replace(Constants.Dot, string.Empty).Trim()
                            : code.Trim();
                    codeString = codeString.Append(code.Trim().Equals(None)
                        ? string.Empty
                        : String.Format(CultureInfo.InvariantCulture, "{0,-6}", codeValue.Length <= DiagnosisCodeLength
                        ? codeValue
                        : codeValue.Substring(0, DiagnosisCodeLength)));
                    diagnosisCodesCount++;
                    if (diagnosisCodesCount == 14)
                        break;
                    if (codeString.Length == 14)
                        break;
                }
            }

            return String.Format(CultureInfo.InvariantCulture, "{0,-84}", codeString);
        }

        /// <summary>
        /// Build ClaimId
        /// <param name="claimId">Claim ID</param>
        /// </summary>
        /// <returns>string</returns>
        private static string BuildClaimId(string claimId)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-17}", claimId);
        }
    }
}
