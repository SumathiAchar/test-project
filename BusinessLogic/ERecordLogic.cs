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
    public class ERecordLogic : IERecordLogic
    {
        public string ValidationErrors { get; private set; }

        private const string InvalidSecondaryDiagnosis = "Invalid secondary diagnosis code";
        private const int ClaimIdLength = 17;
        private const string InvalidClaimId = "Invalid claimid";
        private const int StringBuilderCapacity = 8;
        private const string None = "NONE";
        private const int DiagnosisCodeLength = 8;
        private const int StringBuilderDiagnosisCapacity = 112;

        /// <summary>
        /// Gets the e record line.
        /// </summary>
        /// <param name="eRecordData">The e record data.</param>
        /// <returns></returns>
        public string GetERecordLine(ERecord eRecordData)
        {
            string eRecord = string.Empty;
            ValidationErrors = ValidateERecord(eRecordData);
            if (eRecordData != null && string.IsNullOrEmpty(ValidationErrors.Trim()))
            {
                eRecord = BuildERecordString(eRecordData);
            }
            return eRecord;
        }


        /// <summary>
        /// Validates the e record.
        /// </summary>
        /// <param name="eRecord">The e record.</param>
        /// <returns></returns>
        private static string ValidateERecord(ERecord eRecord)
        {
            if (eRecord != null)
            {
                //FIXED-NOV15 - Check null condition for eRecord else return string.Empty
                return string.Format(CultureInfo.InvariantCulture, "{0} {1}",
                    ValidateClaimId(eRecord.ClaimId),
                    ValidateSecondaryDiagnosis(eRecord.SecondaryDiagnosisCodes));
            }
            return string.Empty;
        }

        /// <summary>
        /// Validates the claim identifier.
        /// </summary>
        /// <param name="claimId">The claim identifier.</param>
        /// <returns></returns>
        private static string ValidateClaimId(string claimId)
        {
            return string.IsNullOrEmpty(claimId) || claimId.Trim().Length <= ClaimIdLength ? string.Empty : InvalidClaimId;
        }

        /// <summary>
        /// Validates the secondary diagnosis.
        /// </summary>
        /// <param name="diagCode">The diag code.</param>
        /// <returns></returns>
        private static string ValidateSecondaryDiagnosis(List<string> diagCode)
        {
            return diagCode == null ? InvalidSecondaryDiagnosis : string.Empty;
        }

        /// <summary>
        /// Builds the e record string.
        /// </summary>
        /// <param name="eRecord"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "e")]
        public static string BuildERecordString(ERecord eRecord)
        {
            StringBuilder eRecordString = new StringBuilder(Constants.Stringlength);

            if (eRecord != null)
            {
                eRecordString = eRecordString.Append(Constants.ERecordString);
                eRecordString = eRecordString.Append(BuildClaimId(eRecord.ClaimId));
                eRecordString = eRecordString.Append(BuildAdminDiagnosis(eRecord.AdmitDiagnosisCode));
                eRecordString = eRecordString.Append(BuildPrincipalDiagnosis(eRecord.PrincipalDiagnosisCode));
                eRecordString = eRecordString.Append(BuildSecondaryDiagnosis(eRecord.SecondaryDiagnosisCodes));
                eRecordString = eRecordString.Append(BuildUnUsed());

                if (eRecordString.Length == Constants.Stringlength)
                {
                    return eRecordString.ToString();
                }
                return string.Format(CultureInfo.InvariantCulture, "{0,-200}", eRecordString);

            }
            return string.Empty;
        }

        /// <summary>
        /// Builds the claim identifier.
        /// </summary>
        /// <param name="claimId">The claim identifier.</param>
        /// <returns></returns>
        private static string BuildClaimId(string claimId)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-17}", claimId ?? string.Empty);
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
            codeString = codeString.Append(adminDiagnosisCode.Trim().Equals(None)
                ? string.Empty
                : String.Format(CultureInfo.InvariantCulture, "{0,-8}", adminDiagnosisCode.Length <= DiagnosisCodeLength ? adminDiagnosisCode : adminDiagnosisCode.Substring(0, DiagnosisCodeLength)));
            return String.Format(CultureInfo.InvariantCulture, "{0,-8}", codeString);
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
                    codeString.Append(String.Format(CultureInfo.InvariantCulture, "{0,-8}",
                        principalDiagnosisCode.Length <= DiagnosisCodeLength
                            ? principalDiagnosisCode
                            : principalDiagnosisCode.Substring(0, DiagnosisCodeLength)));
            }
            return String.Format(CultureInfo.InvariantCulture, "{0,-8}", codeString);
        }

        /// <summary>
        /// Builds the secondary diagnosis.
        /// </summary>
        /// <param name="diagnosisCodes">The diagnosis codes.</param>
        /// <returns></returns>
        private static string BuildSecondaryDiagnosis(List<string> diagnosisCodes)
        {
            StringBuilder codeString = new StringBuilder(StringBuilderDiagnosisCapacity);
            //If code contains ".", replaces it with empty string and trims to 6 character if it is greater than 6 characters
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
                        : String.Format(CultureInfo.InvariantCulture, "{0,-8}", codeValue.Length <= DiagnosisCodeLength
                        ? codeValue
                        : codeValue.Substring(0, DiagnosisCodeLength)));
                    diagnosisCodesCount++;
                    if (diagnosisCodesCount == 14)
                        break;
                    if (codeString.Length == 14)
                        break;
                }
            }

            return String.Format(CultureInfo.InvariantCulture, "{0,-112}", codeString);
        }

        /// <summary>
        /// Builds the un used.
        /// </summary>
        /// <returns></returns>
        private static string BuildUnUsed()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-54}", string.Empty);
        }
    }
}
