/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Medicare IP Details functionalities

/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NetPrc5Yr;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ip"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ip")]
    public class PaymentTypeMedicareIpLogic : PaymentTypeBaseLogic
    {
        private const int Icd9CodeLength = 5;
        private const int Icd10CodeLength = 7;

        /// <summary>
        /// The _medicare ip details repository
        /// </summary>
        private readonly IPaymentTypeMedicareIpRepository _medicareIpDetailsRepository;

        /// <summary>
        /// The _pricer DLL
        /// </summary>
        private NetPrcM5 _pricerDll;

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        public override PaymentTypeBase PaymentTypeBase { get; set; }

        /// <summary>
        /// Gets the payment type cap.
        /// </summary>
        /// <value>
        /// The payment type cap.
        /// </value>
        private PaymentTypeMedicareIp PaymentTypeMedicareIp { get { return PaymentTypeBase as PaymentTypeMedicareIp; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareIpLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeMedicareIpLogic(string connectionString)
        {
            _medicareIpDetailsRepository = Factory.CreateInstance<IPaymentTypeMedicareIpRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareIpLogic"/> class.
        /// </summary>
        /// <param name="paymentTypeMedicareIpDetailsRepository">The payment type medicare ip details repository.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ip"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ip")]
        public PaymentTypeMedicareIpLogic(IPaymentTypeMedicareIpRepository paymentTypeMedicareIpDetailsRepository)
        {
            if (paymentTypeMedicareIpDetailsRepository != null)
                _medicareIpDetailsRepository = paymentTypeMedicareIpDetailsRepository;
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _medicareIpDetailsRepository.AddEditPaymentTypeMedicareIpPayment((PaymentTypeMedicareIp)paymentType);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _medicareIpDetailsRepository.GetPaymentTypeMedicareIpPayment((PaymentTypeMedicareIp)paymentType);
        }

        /// <summary>
        /// Evaluates the type of the payment.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment result list.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public override List<PaymentResult> EvaluatePaymentType(IEvaluateableClaim claim,
            List<PaymentResult> paymentResults, bool isCarveOut, bool isContractFilter)
        {
            MedicareInPatientResult medicareInPatientResult = GetMedicareInPatientResult(claim);

            var claimPaymentResult = GetClaimLevelPaymentResult(paymentResults, isCarveOut);

            if (claimPaymentResult != null && medicareInPatientResult != null)
            {
                //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
                Utilities.UpdatePaymentResult(claimPaymentResult, PaymentTypeMedicareIp.ServiceTypeId,
                    PaymentTypeMedicareIp.PaymentTypeDetailId, PaymentTypeMedicareIp.PaymentTypeId);

                if (medicareInPatientResult.IsSucess)
                {
                    if (PaymentTypeMedicareIp.InPatient.HasValue)
                    {
                        claimPaymentResult.AdjudicatedValue = (PaymentTypeMedicareIp.InPatient.Value / 100) *
                                                              medicareInPatientResult.TotalPaymentAmount;
                        claimPaymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                    }
                    else
                        claimPaymentResult.ClaimStatus =
                            (byte)Enums.AdjudicationOrVarianceStatuses.AdjudicationErrorInvalidPaymentData;
                }
                else
                    claimPaymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;
            }

            return paymentResults;
        }

        /// <summary>
        /// Gets the diagnosis code string.
        /// </summary>
        /// <param name="codes">The codes.</param>
        /// <param name="statementThru">The statement thru.</param>
        /// <returns></returns>
        private static string GetDiagnosisCodeString(List<DiagnosisCode> codes, DateTime? statementThru)
        {

            if (codes != null && codes.Any())
            {
                StringBuilder codeString = new StringBuilder();
                //For ICD 10
                if (statementThru >= DateTime.Parse(Constants.IcdVersionDate, CultureInfo.InvariantCulture))
                {
                    //If code contains ".", replaces it with empty string and trims to 7 character if it is greater than 7 characters
                    codeString = codes.Select(codeValue => codeValue.IcddCode.IndexOf(Constants.Dot, StringComparison.Ordinal) != -1
                        ? codeValue.IcddCode.Replace(Constants.Dot, string.Empty).Trim()
                        : codeValue.IcddCode.Trim()).Aggregate(codeString, (current, s) => current.Append(String.Format(CultureInfo.InvariantCulture, "{0,-7}", s.Length <= Icd10CodeLength
                                                                                                                                    ? s
                                                                                                                                    : s.Substring(0, Icd10CodeLength))));
                }
                //For ICD 9
                else
                {
                    //If code contains ".", replaces it with empty string and trims to 5 character if it is greater than 5 characters
                    codeString = codes.Select(codeValue => codeValue.IcddCode.IndexOf(Constants.Dot, StringComparison.Ordinal) != -1
                        ? codeValue.IcddCode.Replace(Constants.Dot, string.Empty).Trim()
                        : codeValue.IcddCode.Trim()).Aggregate(codeString, (current, s) => current.Append(String.Format(CultureInfo.InvariantCulture, "{0,-5}", s.Length <= Icd9CodeLength
                                                                                                                            ? s
                                                                                                                            : s.Substring(0, Icd9CodeLength))));
                }
                return codeString.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the procedure code string.
        /// </summary>
        /// <param name="codes">The codes.</param>
        /// <param name="statementThru">The statement thru.</param>
        /// <returns></returns>
        private static string GetProcedureCodeString(List<ProcedureCode> codes, DateTime? statementThru)
        {

            if (codes != null && codes.Any())
            {
                StringBuilder codeString = new StringBuilder();
                //For ICD 10
                if (statementThru >= DateTime.Parse(Constants.IcdVersionDate, CultureInfo.InvariantCulture))
                {
                    //If code contains ".", replaces it with empty string and trims to 7 character if it is greater than 7 characters
                    codeString = codes.Select(codeValue => codeValue.IcdpCode.IndexOf(Constants.Dot, StringComparison.Ordinal) != -1
                        ? codeValue.IcdpCode.Replace(Constants.Dot, string.Empty).Trim()
                        : codeValue.IcdpCode.Trim()).Aggregate(codeString, (current, s) => current.Append(String.Format(CultureInfo.InvariantCulture, "{0,-7}", s.Length <= Icd10CodeLength
                                                                                                                                    ? s
                                                                                                                                    : s.Substring(0, Icd10CodeLength))));
                }
                //For ICD 9
                else
                {
                    //If code contains ".", replaces it with empty string and trims to 5 character if it is greater than 5 characters
                    codeString = codes.Select(codeValue => codeValue.IcdpCode.IndexOf(Constants.Dot, StringComparison.Ordinal) != -1
                        ? codeValue.IcdpCode.Replace(Constants.Dot, string.Empty).Trim()
                        : codeValue.IcdpCode.Trim()).Aggregate(codeString, (current, s) => current.Append(String.Format(CultureInfo.InvariantCulture, "{0,-5}", s.Length <= Icd9CodeLength
                                                                                                                                    ? s
                                                                                                                                    : s.Substring(0, Icd9CodeLength))));
                }
                return codeString.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///  Actual Method call for Microdyn In Patient output retrieval
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SSI.ContractManagement.Shared.Models.MedicareInPatientResult.set_Message(System.String)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "Microdyne"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SSI.ContractManagement.Shared.Helpers.ErrorLog.Log.LogInfo(System.String,System.String)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ClaimId"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private MedicareInPatientResult GetMedicareInPatientResult(IEvaluateableClaim claim)
        {
            MedicareInPatientResult medicareInPatientResult = new MedicareInPatientResult();
            MedicareInPatient medicarePatient = claim.MedicareInPatient;
            _pricerDll = new NetPrcM5
            {
                Provider = medicarePatient.Npi,
                DDate = medicarePatient.DischargeDate.ToString(CultureInfo.InvariantCulture),
                DRG = medicarePatient.Drg,
                DStat = medicarePatient.DischargeStatus
            };
            _pricerDll.ReviewCode = _pricerDll.DStat;
            _pricerDll.LOS = medicarePatient.LengthOfStay;
            _pricerDll.Charges = medicarePatient.Charges;

            //A string of diagnosis codes formatted as 7-Byte values. 
            //Each code is left justified and blank filled to a length of 7 bytes. 
            //Any decimal point in the diagnosis code should be removed before passing the code to PRICERActive.
            _pricerDll.DiagnosisCodes = GetDiagnosisCodeString(claim.DiagnosisCodes, claim.StatementThru);

            //A string of procedure codes formatted as 7-Byte values. 
            //Each code is left justified and blank filled to a length of 7 bytes. 
            //Any decimal point in the procedure code should be removed before passing the code to PRICERActive.
            _pricerDll.ProcedureCodes = GetProcedureCodeString(claim.ProcedureCodes, claim.StatementThru);

            //If statementThru Date is equal to or greater than 10/01/2015, then we pass "0" as ICDVersion10 else "9" as ICDVersion 9
            _pricerDll.ICDVersion = claim.StatementThru >= DateTime.Parse(Constants.IcdVersionDate, CultureInfo.InvariantCulture) ? Constants.IcdVersion10 : Constants.IcdVersion9;
            _pricerDll.DeviceCreditAmount = Constants.DeviceCreditAmount; // 0; // Source unknown.
            _pricerDll.DIFICIDOnClaim = Constants.DificidOnClaim; // false;
            _pricerDll.HMOClaim = Constants.HmoClaim; // false;
            _pricerDll.IncludePassthru = Constants.IncludePassthru; //false;
            _pricerDll.AllowTerminatedProvider = Constants.AllowTerminatedProvider; // false;
            _pricerDll.AdjustmentFactor = Constants.AdjustmentFactor; // 1;
            _pricerDll.AdjustmentOption = Constants.AdjustmentOption; // 0;

            if (GlobalConfigVariable.IsMicrodynLogEnabled)
            {
                XmlSerializer xsSubmit = new XmlSerializer(typeof(NetPrcM5));
                StringWriter sww = new StringWriter();
                XmlWriter writer = XmlWriter.Create(sww);
                xsSubmit.Serialize(writer, _pricerDll);
                var xml = sww.ToString();

                Log.LogInfo("MedicareInPatient Input : \n" + xml, "MicrodynPriceInputData");
            }

            if (GlobalConfigVariable.IsMicrodynEnabled)
            {
                _pricerDll.PRICERCalc();
                string message = string.Empty;
                medicareInPatientResult.TotalPaymentAmount = _pricerDll.PRICERSuccess ? GetTotalAmount(_pricerDll) : 0.00;
                medicareInPatientResult.IsSucess = _pricerDll.PRICERSuccess;
                message = GetPricerReturnValue(_pricerDll.ReturnCode, message, _pricerDll.PRICERSuccess);
                medicareInPatientResult.ClaimId = claim.ClaimId;
                medicareInPatientResult.ReturnCode = _pricerDll.ReturnCode;
                medicareInPatientResult.Message = message;

                if (GlobalConfigVariable.IsMicrodynLogEnabled)
                    Log.LogInfo(
                        string.Format(CultureInfo.InvariantCulture, Constants.MedicareIpClaimIdLog, medicareInPatientResult.Message,
                            medicareInPatientResult.ClaimId), Constants.MicrodynInPatientMessage);
            }
            else
            {
                medicareInPatientResult.ReturnCode = 0;
                medicareInPatientResult.TotalPaymentAmount = 0;
                medicareInPatientResult.Message = Constants.MessageMicrodyneOff;
                medicareInPatientResult.IsSucess = true;
            }
            //After processing, destroy the Reimbursement DLL object
            _pricerDll.Dispose();

            return medicareInPatientResult;
        }

        /// <summary>
        /// Gets the total amount.
        /// </summary>
        /// <param name="pricerDll">The pricer DLL.</param>
        /// <returns></returns>
        private double GetTotalAmount(NetPrcM5 pricerDll)
        {
            double amount = 0.0;
            if (!string.IsNullOrEmpty(PaymentTypeMedicareIp.Formula))
            {
                List<string> formulaProperties = PaymentTypeMedicareIp.Formula.Replace(Constants.Space, string.Empty).Split('+').ToList();
                List<string> availableProperties = GetAvailableFormulaProperties();

                bool isAllPropertySelected = (formulaProperties.Count == availableProperties.Count) &&
                                             !formulaProperties.Except(availableProperties).Any();
                if (isAllPropertySelected)
                {
                    amount = pricerDll.TotalPayment;
                }
                else
                {
                    Applyformula(formulaProperties,ref amount, pricerDll);
                }
            }
            return amount;
        }


        /// <summary>
        /// Gets the available properties.
        /// </summary>
        /// <returns></returns>
        private static List<string> GetAvailableFormulaProperties()
        {
            return new List<string>
                {
                    Constants.Fsp,
                    Constants.Hsp,
                    Constants.Co,
                    Constants.Ime,
                    Constants.Dsh,
                    Constants.PassThru,
                    Constants.TechAddOnPayment,
                    Constants.LowVolumeAddOn,
                    Constants.Hrr,
                    Constants.Vbp,
                    Constants.Ucc,
                    Constants.Hac,
                    Constants.Cap
                };
        }

        /// <summary>
        /// Gets the pricer return value.
        /// </summary>
        /// <param name="returnCode">The return code.</param>
        /// <param name="message">The message.</param>
        /// <param name="pricerSuccess">if set to <c>true</c> [pricer success].</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static string GetPricerReturnValue(int returnCode, string message, bool pricerSuccess)
        {
            switch (returnCode)
            {
                case 0:
                    message = Constants.MessagePaidDrgPayment;
                    break;
                case 2:
                    message = Constants.MessagePaidAsCostOutlier;
                    break;
                case 3:
                    message = Constants.MessageTrasferPaid;
                    break;
                case 5:
                    message = Constants.MessageTrasferPaidWithCostOutlier;
                    break;
                case 6:
                    message = Constants.MessageTrasferPaidRefused;
                    break;
                case 10:
                    message = Constants.MessagePostAcuteTransfer;
                    break;
                case 12:
                    message = Constants.MessagePostAcuteTransferFully;
                    break;
                case 14:
                    message = Constants.MessagePaidDrgDays;
                    break;
                case 16:
                    message = Constants.MessagePaidCostOutlierDays;
                    break;
                case 51:
                    message = Constants.MessageProviderNumberError;
                    break;
                case 52:
                    message = Constants.MessageInvalidMsa;
                    break;
                case 53:
                    message = Constants.MessageWaiverFlag;
                    break;
                case 54:
                    message = Constants.MessageInvalidDrg;
                    break;
                case 55:
                    message = Constants.MessageDischargeDateError;
                    break;
                case 56:
                    message = Constants.MessageInvalidLos;
                    break;
                case 57:
                    message = Constants.MessageInvalidReviewCode;
                    break;
                case 58:
                    message = Constants.MessageNoTotalChargeSubmitted;
                    break;
                case 61:
                    message = Constants.MessageMoreLtrDays;
                    break;
                case 62:
                    message = Constants.MessageInvalidCoveredDays;
                    break;
                case 65:
                    message = Constants.MessageInvalidHrrData;
                    break;
                case 68:
                    message = Constants.MessageInvalidVbp;
                    break;
                case 90:
                    message = Constants.MessagePricerActiveError;
                    break;
                case 91:
                    message = Constants.MessageReviewCodeError;
                    break;
                case 98:
                    message = Constants.MessageSqlTableError;
                    break;
                case 99:
                    message = Constants.MessagePricerActiveExpired;
                    break;
                case 100:
                    message = Constants.MessagePricerActiveDataAccessError;
                    break;
                default:
                    if (returnCode == 67)
                        message = pricerSuccess ? Constants.MessageCostOutlier : Constants.MessageCostOutlierWithLos;
                    break;
            }
            return message;
        }

        /// <summary>
        /// Applyformulas the specified formula properties.
        /// </summary>
        /// <param name="formulaProperties">The formula properties.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="pricerDll">The pricer DLL.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static void Applyformula(IEnumerable<string> formulaProperties,ref double amount, NetPrcM5 pricerDll)
        {
            foreach (string formula in formulaProperties)
            {
                switch (formula.Trim())
                {
                    case Constants.Fsp:
                        amount += pricerDll.FSP;
                        break;
                    case Constants.Hsp:
                        amount += pricerDll.HSP;
                        break;
                    case Constants.Co:
                        amount += pricerDll.CostOutlier;
                        break;
                    case Constants.Ime:
                        amount += pricerDll.IME;
                        break;
                    case Constants.Dsh:
                        amount += pricerDll.DSH;
                        break;
                    case Constants.PassThru:
                        amount += pricerDll.PassThru;
                        break;
                    case Constants.TechAddOnPayment:
                        amount += pricerDll.TechAddOnPayment;
                        break;
                    case Constants.LowVolumeAddOn:
                        amount += pricerDll.LowVolumeAddOn;
                        break;
                    case Constants.Hrr:
                        amount += pricerDll.HRR;
                        break;
                    case Constants.Vbp:
                        amount += pricerDll.VBP;
                        break;
                    case Constants.Ucc:
                        amount += pricerDll.UCC;
                        break;
                    case Constants.Hac:
                        amount += pricerDll.HACReduction;
                        break;
                    case Constants.Cap:
                        amount += pricerDll.CapitalPayment;
                        break;
                    case Constants.CapDsh:
                        amount += pricerDll.Capital_DSH_Amt;
                        break;
                    case Constants.CapExceptionsAmount:
                        amount += pricerDll.Capital_Exceptions_Amt;
                        break;
                    case Constants.CapFsp:
                        amount += pricerDll.Capital_FSP_Amt;
                        break;
                    case Constants.CapHsp:
                        amount += pricerDll.Capital_HSP_Amt;
                        break;
                    case Constants.CapIme:
                        amount += pricerDll.Capital_IME_Amt;
                        break;
                    case Constants.CapOldHarm:
                        amount += pricerDll.Capital_OldHarm_Amt;
                        break;
                    case Constants.CapOutlier:
                        amount += pricerDll.Capital_Outlier_Amt;
                        break;
                }
            }
        }
    }
}
