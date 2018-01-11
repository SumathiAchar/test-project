using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class LRecordLogic : ILRecordLogic
    {
        public string ValidationErrors { get; private set; }

        private const string LRecordInitial = "L";
        private const string InvalidLineNumber = "Invalid line item number";
        private const string InvalidLineFlag = "Invalid line item flag";
        private const string InvalidHcpcs = "Invalid hcpcs code";
        private const string InvalidModifier = "invalid HcpcsModifiers";
        private const string InvalidClaimId = "Invalid claimid";
        private const string InvalidRevCode = "Invalid revcode";
        private const string InvalidUnitOfService = "Invalid unit of service";
        private const string InvalidLineCharge = "Invalid line charge amount";
        private const int ClaimIdLength = 17;
        private const int LineItemLength = 3;
        private const int HcpcsCodeLength = 5;
        private const int RevCodeLength = 4;
        private const int UnitOfServiceLength = 7;
        private const int LineChargeLength = 10;
        private const double LineChargeMaxValue = 99999999.99;
        private const int ItemFlagLenth = 1;
        private const int MinItemFlag = 0;
        private const int MaxItemFlag = 5;
        private const string ThreeIntegralTypeFormat = "D3";
        private const string SevenIntegralTypeFormat = "D7";
        private const int HcpcsModifiersLength = 10;

        /// <summary>
        /// Gets the l record line.
        /// </summary>
        /// <param name="lRecordList">The l record list.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToString(System.Object)")]
        public string GetLRecordLine(IEnumerable<LRecord> lRecordList)
        {
            StringBuilder lRecordAllLines = new StringBuilder();
            if (lRecordList != null)
            {
                foreach (LRecord lRecordData in lRecordList)
                {
                    // Validating the L record
                    ValidationErrors = ValidationErrors + ValidateLRecord(lRecordData);
                    lRecordAllLines.Append(string.IsNullOrEmpty(ValidationErrors.Trim())
                        ? BuildLRecordString(lRecordData)
                        : string.Empty);
                }
            }
            return Convert.ToString(lRecordAllLines);
        }

        /// <summary>
        ///  Validation Methods for validating Data from DB
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static string ValidateLRecord(LRecord lRaw)
        {
            try
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4} {5} {6} {7}", ValidateClaimId(lRaw.ClaimId),
                    ValidateHcpcsModifiers(lRaw.HcpcsModifiers),
                    ValidateLineItemFlag(lRaw.LineItemFlag),
                    ValidateLineItemId(lRaw.LineItemId),
                    ValidateHcpcsCode(lRaw.HcpcsProcedureCode),
                    ValidateRevCode(lRaw.RevenueCode),
                    ValidateUnitOfService(lRaw.UnitsofService),
                    ValidateLineCharge(lRaw.LineItemCharge));
            }
            catch (Exception ex)
            {
                Log.LogError(string.Empty, string.Empty, ex);
                return "Exception occurred";
            }
        }

        /// <summary>
        ///  Validate LineItemFlag
        /// </summary>
        private static string ValidateLineItemId(int lineItemId)
        {
            switch (lineItemId)
            {
                // If Line Item Id is null, returns Log Message
                case 0:
                    return InvalidLineNumber;
                default:
                    // Line item code length should be less than 4
                    return Convert.ToString(lineItemId, CultureInfo.InvariantCulture).Length <= LineItemLength ? string.Empty : InvalidLineNumber;
            }

        }

        /// <summary>
        /// Validates the line item flag.
        /// </summary>
        /// <param name="itemFlag">The item flag.</param>
        /// <returns></returns>
        private static string ValidateLineItemFlag(int itemFlag)
        {
            // Item flag length should be greater than 1 and it should be between 0 to 5.
            return Convert.ToString(itemFlag, CultureInfo.InvariantCulture).Length > ItemFlagLenth || itemFlag < MinItemFlag || itemFlag > MaxItemFlag
                ? InvalidLineFlag
                : string.Empty;
        }

        /// <summary>
        /// Validates the HCPCS code.
        /// </summary>
        /// <param name="hcpcsProcedureCode">The HCPCS procedure code.</param>
        /// <returns></returns>
        private static string ValidateHcpcsCode(string hcpcsProcedureCode)
        {
            // hcpcs code length should be less than 5
            return string.IsNullOrEmpty(hcpcsProcedureCode) || hcpcsProcedureCode.Length <= HcpcsCodeLength ? string.Empty : InvalidHcpcs;
        }

        /// <summary>
        ///  Validate HCPCSModifiers
        /// </summary>
        private static string ValidateHcpcsModifiers(string hcpcsModifiers)
        {
            // hcpcs code length should be less than 6.
            return string.IsNullOrEmpty(hcpcsModifiers) || hcpcsModifiers.Length % 2 == 0
                ? string.Empty
                : InvalidModifier;
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
        /// Validates the rev code.
        /// </summary>
        /// <param name="revCode">The rev code.</param>
        /// <returns></returns>
        private static string ValidateRevCode(string revCode)
        {
            // rev Code length should be less than 5
            return !string.IsNullOrEmpty(revCode) && revCode.Trim().Length <= RevCodeLength ? string.Empty : InvalidRevCode;
        }

        /// <summary>
        /// Validates the unit of service.
        /// </summary>
        /// <param name="unitOfService">The unit of service.</param>
        /// <returns></returns>
        private static string ValidateUnitOfService(int? unitOfService)
        {
            switch (unitOfService)
            {
                // Unit of service length should not be null
                case null:
                    return InvalidUnitOfService;
                default:
                    // Unit of service length should be less than 7
                    return Convert.ToString(unitOfService, CultureInfo.InvariantCulture).Length <= UnitOfServiceLength
                        ? string.Empty
                        : InvalidUnitOfService;
            }
        }

        /// <summary>
        /// Validates the line charge.
        /// </summary>
        /// <param name="lineCharge">The line charge.</param>
        /// <returns></returns>
        private static string ValidateLineCharge(double? lineCharge)
        {
            return lineCharge == null || Math.Truncate(100 * lineCharge.Value) / 100 > LineChargeMaxValue
                ? InvalidLineCharge
                : string.Empty;
        }

        /// <summary>
        /// Build L Record String
        /// <param name="lRecordRaw">L Record</param>
        /// </summary>
        /// <returns>string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "l")]
        public static string BuildLRecordString(LRecord lRecordRaw)
        {
            StringBuilder lRecordString = new StringBuilder(Constants.Stringlength);

            if (lRecordRaw != null)
            {
                try
                {
                    //Build L string
                    lRecordString.Length = 0;
                    lRecordString = lRecordString.Append(LRecordInitial);
                    lRecordString = lRecordString.Append(BuildClaimId(lRecordRaw.ClaimId));
                    lRecordString = lRecordString.Append(BuildLineItemId(lRecordRaw.LineItemId));
                    lRecordString = lRecordString.Append(BuildHcpcsProcedureCode(lRecordRaw.HcpcsProcedureCode));
                    lRecordString = lRecordString.Append(BuildHcpcsModifiers(lRecordRaw.HcpcsModifiers));
                    lRecordString = lRecordString.Append(BuildServiceDate(lRecordRaw.ServiceDate));
                    lRecordString = lRecordString.Append(BuildRevenueCode(lRecordRaw.RevenueCode));
                    lRecordString = lRecordString.Append(BuildUnitsofService(lRecordRaw.UnitsofService));
                    lRecordString = lRecordString.Append(BuildlineItemCharge(lRecordRaw.LineItemCharge));
                    lRecordString = lRecordString.Append(BuildLineItemFlag(lRecordRaw.LineItemFlag));
                    lRecordString = lRecordString.Append(BuildUnUsed());

                    if (lRecordString.Length == Constants.Stringlength)
                    {
                        return lRecordString.ToString();
                    }
                    return string.Format(CultureInfo.InvariantCulture, "{0,-200}", lRecordString);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Build UnUsed
        /// </summary>
        /// <returns>string</returns>
        private static string BuildUnUsed()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-134}", string.Empty);
        }

        /// <summary>
        /// Build Line Item Flag
        /// <param name="itemFlag">Item Flag</param>
        /// </summary>
        /// <returns>string</returns>
        private static string BuildLineItemFlag(int itemFlag)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-1}", Convert.ToString(itemFlag, CultureInfo.InvariantCulture));
        }
        /// <summary>
        /// Build line Item Charge
        /// <param name="itemCharge">Item Charge</param>
        /// </summary>
        /// <returns>string</returns>
        private static string BuildlineItemCharge(double? itemCharge)
        {
            if (itemCharge != null)
            {
                return
                    string.Join("",
                        string.Format(CultureInfo.InvariantCulture, "{0:N2}", Math.Truncate(100 * itemCharge.Value) / 100).Where(char.IsDigit))
                        .PadLeft(LineChargeLength, '0');
            }
            return InvalidLineCharge;
        }

        /// <summary>
        /// Build Units of Service
        /// <param name="unitsofService">Units of Service</param>
        /// </summary>
        /// <returns>string</returns>
        private static string BuildUnitsofService(int? unitsofService)
        {
            switch (unitsofService)
            {
                case null:
                    return String.Format(CultureInfo.InvariantCulture, "{0,-7}", string.Empty);
                default:
                    return unitsofService.Value.ToString(SevenIntegralTypeFormat, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Build Revenue Code
        /// <param name="revenueCode">Revenue Code</param>
        /// </summary>
        /// <returns>string</returns>
        private static string BuildRevenueCode(string revenueCode)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-4}", revenueCode.Trim());
        }

        /// <summary>
        /// Build Service Date
        /// <param name="serviceDate">Service Date</param>
        /// </summary>
        /// <returns>string</returns>
        private static string BuildServiceDate(DateTime serviceDate)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0:yyyyMMdd}", serviceDate);
        }

        /// <summary>
        /// Build HCPCS Modifiers
        /// <param name="hcpcsModifiers">HCPCS Modifiers</param>
        /// </summary>
        /// <returns>string</returns>
        private static string BuildHcpcsModifiers(string hcpcsModifiers)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-10}", (hcpcsModifiers.Length <= HcpcsModifiersLength ? hcpcsModifiers : hcpcsModifiers.Substring(0, HcpcsModifiersLength)));
        }

        /// <summary>
        /// Build HCPCS Procedure Code
        /// <param name="hcpcsCode">HCPCS Code</param>
        /// </summary>
        /// <returns>string</returns>
        private static string BuildHcpcsProcedureCode(string hcpcsCode)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-5}", hcpcsCode.Trim());
        }

        /// <summary>
        /// Build Line Item Id
        /// <param name="lineItemId">Line Item ID</param>
        /// </summary>
        /// <returns>string</returns>
        private static string BuildLineItemId(int lineItemId)
        {
            return lineItemId.ToString(ThreeIntegralTypeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Build ClaimId
        /// <param name="claimId">Claim ID</param>
        /// </summary>
        /// <returns>string</returns>
        private static string BuildClaimId(string claimId)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0,-17}", claimId.Trim());
        }
    }
}
