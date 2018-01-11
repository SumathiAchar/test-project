/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType Medicare OP Details functionalities

/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;
using NetAPCCalc;
using NetAPCEdit;

namespace SSI.ContractManagement.BusinessLogic
{
    public class PaymentTypeMedicareOpLogic : PaymentTypeBaseLogic
    {
        /// <summary>
        /// The _apc pay DLL
        /// </summary>
        private NetAPCPay _apcPayDll;

        /// <summary>
        /// The _instance
        /// </summary>
        [ThreadStatic]
        static NetAPCDLL _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        private static NetAPCDLL Instance
        {
            get { return _instance ?? (_instance = new NetAPCDLL()); }
        }

        /// <summary>
        /// The _medicare op details repository
        /// </summary>
        private readonly IPaymentTypeMedicareOpRepository _medicareOpDetailsRepository;

        /// <summary>
        /// The _c record logic
        /// </summary>
        private readonly ICRecordLogic _cRecordLogic;

        /// <summary>
        /// The _d record logic
        /// </summary>
        private readonly IDRecordLogic _dRecordLogic;

        /// <summary>
        /// The _e record logic
        /// </summary>
        private readonly IERecordLogic _eRecordLogic;

        /// <summary>
        /// The _l record logic
        /// </summary>
        private readonly ILRecordLogic _lRecordLogic;

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
        private PaymentTypeMedicareOp PaymentTypeMedicareOp { get { return PaymentTypeBase as PaymentTypeMedicareOp; } }

        private const string DataIssueMessageFormatWithDRecord = "Data issue(has not been submitted to microdyn): ClaimId-{0} - CRecord:\"{1}\" {2} CRecord error:{3} {4} DRecord:\"{5}\" {6} DRecord error:{7} {8} LRecord:\"{9}\" {10} LRecord error:{11}";

        private const string ApcErrorMessageFormatWithDRecord = "ClaimId-{0} - {1} ResultsData:{2} EditErrorCodes:{3} {4} CRecord:\"{5}\" {6} DRecord:\"{7}\" {8} LRecord:\"{9}\"";

        private const string ApcCalcMessageFormatWithDRecord = "ClaimId-{0} - {1} : PricerErrorCodes: {2} ResultsData:{3} {4} CRecord:\"{5}\" {6} DRecord:\"{7}\" {8} LRecord:\"{9}\"";

        private const string DataIssueMessageFormatWithERecord = "Data issue(has not been submitted to microdyn): ClaimId-{0} - CRecord:\"{1}\" {2} CRecord error:{3} {4} ERecord:\"{5}\" {6} ERecord error:{7} {8} LRecord:\"{9}\" {10} LRecord error:{11}";

        private const string ApcErrorMessageFormatWithERecord = "ClaimId-{0} - {1} ResultsData:{2} EditErrorCodes:{3} {4} CRecord:\"{5}\" {6} ERecord:\"{7}\" {8} LRecord:\"{9}\"";

        private const string ApcCalcMessageFormatWithERecord = "ClaimId-{0} - {1} : PricerErrorCodes: {2} ResultsData:{3} {4} CRecord:\"{5}\" {6} ERecord:\"{7}\" {8} LRecord:\"{9}\"";


        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareOpLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeMedicareOpLogic(string connectionString)
        {
            _cRecordLogic = Factory.CreateInstance<ICRecordLogic>();
            _dRecordLogic = Factory.CreateInstance<IDRecordLogic>();
            _eRecordLogic = Factory.CreateInstance<IERecordLogic>();
            _lRecordLogic = Factory.CreateInstance<ILRecordLogic>();
            _medicareOpDetailsRepository = Factory.CreateInstance<IPaymentTypeMedicareOpRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeMedicareOpLogic"/> class.
        /// </summary>
        /// <param name="paymentTypeMedicareOpDetailsRepository">The payment type medicare property details repository.</param>
        /// <param name="cRecordLogic"></param>
        /// <param name="dRecordLogic"></param>
        /// <param name="eRecordLogic"></param>
        /// <param name="lRecordLogic"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "d"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "c"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "e"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "l")]
        public PaymentTypeMedicareOpLogic(IPaymentTypeMedicareOpRepository paymentTypeMedicareOpDetailsRepository,
            ICRecordLogic cRecordLogic, IDRecordLogic dRecordLogic, IERecordLogic eRecordLogic,
            ILRecordLogic lRecordLogic)
        {
            if (cRecordLogic != null)
                _cRecordLogic = cRecordLogic;

            if (dRecordLogic != null)
                _dRecordLogic = dRecordLogic;

            if (eRecordLogic != null)
                _eRecordLogic = eRecordLogic;

            if (lRecordLogic != null)
                _lRecordLogic = lRecordLogic;

            if (paymentTypeMedicareOpDetailsRepository != null)
                _medicareOpDetailsRepository = paymentTypeMedicareOpDetailsRepository;
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _medicareOpDetailsRepository.AddEditPaymentTypeMedicareOpPayment((PaymentTypeMedicareOp)paymentType);
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _medicareOpDetailsRepository.GetPaymentTypeMedicareOpDetails((PaymentTypeMedicareOp)paymentType);
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
            if (PaymentTypeMedicareOp != null)
            {
                if (isCarveOut ||
                    (PaymentTypeMedicareOp.PayAtClaimLevel &&
                     !paymentResults.Any(
                         currentPaymentResult => currentPaymentResult.Line == null && currentPaymentResult.ServiceTypeId == PaymentTypeMedicareOp.ServiceTypeId)) ||
                    (!PaymentTypeMedicareOp.PayAtClaimLevel && paymentResults.Any(
                        currentPaymentResult =>
                            currentPaymentResult.Line != null &&
                            PaymentTypeMedicareOp.ValidLineIds.Contains(currentPaymentResult.Line.Value) &&
                            currentPaymentResult.AdjudicatedValue == null)))
                {
                    if (PaymentTypeMedicareOp != null)
                    {
                        MedicareOutPatientResult medicareOutPatientResult = GetMedicareOutPatientResult(claim);

                        paymentResults = PaymentTypeMedicareOp.PayAtClaimLevel
                            ? EvaluateAtClaimLevel(paymentResults, medicareOutPatientResult, isCarveOut)
                            : EvaluateAtLineLevel(claim, paymentResults, isCarveOut, medicareOutPatientResult);
                    }
                }
            }
            return paymentResults;
        }

        /// <summary>
        /// Evaluates at line level.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="medicareOutPatientResult">The medicare out patient result.</param>
        /// <returns></returns>
        private List<PaymentResult> EvaluateAtLineLevel(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut, MedicareOutPatientResult medicareOutPatientResult)
        {
            foreach (ClaimCharge claimCharge in claim.ClaimCharges)
            {
                if (isCarveOut &&
                    paymentResults.Any(
                        currentPaymentResult =>
                            currentPaymentResult.Line == claimCharge.Line &&
                            currentPaymentResult.ServiceTypeId == PaymentTypeMedicareOp.ServiceTypeId))
                    break;

                if (PaymentTypeMedicareOp.ValidLineIds.Contains(claimCharge.Line) && paymentResults.Any(
                    currentPaymentResult =>
                        currentPaymentResult.Line == claimCharge.Line &&
                        (currentPaymentResult.AdjudicatedValue == null || isCarveOut)))
                {
                    PaymentResult paymentResult = GetPaymentResult(paymentResults, isCarveOut,
                        claimCharge.Line);

                    //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
                    Utilities.UpdatePaymentResult(paymentResult, PaymentTypeMedicareOp.ServiceTypeId,
                        PaymentTypeMedicareOp.PaymentTypeDetailId, PaymentTypeMedicareOp.PaymentTypeId);

                    //Get Line level Adjudciated amount from medicareOutPatientResult
                    double? resultAmount = GetLineLevelAmount(medicareOutPatientResult, claimCharge.Line);

                    if (resultAmount.HasValue)
                    {
                        if (PaymentTypeMedicareOp.OutPatient.HasValue)
                        {
                            double adjValue = (PaymentTypeMedicareOp.OutPatient.Value / 100) *
                                              resultAmount.Value;
                            paymentResult.AdjudicatedValue = adjValue;
                            paymentResult.ClaimStatus =
                                (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                        }
                        else
                            paymentResult.ClaimStatus =
                                (byte)
                                    Enums.AdjudicationOrVarianceStatuses
                                        .AdjudicationErrorInvalidPaymentData;
                    }
                    else if (!medicareOutPatientResult.IsEditSuccess)
                    {
                        paymentResult.ClaimStatus =
                            (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;
                        paymentResult.MicrodynEditErrorCodes = medicareOutPatientResult.EditErrorCodes;
                        paymentResult.MicrodynEditReturnRemarks = medicareOutPatientResult.MicrodynEditReturnRemarks;
                    }
                    else if (!medicareOutPatientResult.IsPricerSuccess)
                    {
                        paymentResult.ClaimStatus =
                            (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;
                        paymentResult.MicrodynPricerErrorCodes = medicareOutPatientResult.PricerErrorCodes;
                        paymentResult.MicrodynEditReturnRemarks = medicareOutPatientResult.MicrodynEditReturnRemarks;
                    }
                }
            }

            return paymentResults;
        }

        /// <summary>
        /// Gets the line level amount.
        /// </summary>
        /// <param name="medicareOutPatientResult">The medicare out patient result.</param>
        /// <param name="lineId">The line identifier.</param>
        /// <returns></returns>
        private static double? GetLineLevelAmount(MedicareOutPatientResult medicareOutPatientResult, int lineId)
        {
            double? resultAmount = null;

            if (medicareOutPatientResult != null && medicareOutPatientResult.IsEditSuccess && medicareOutPatientResult.IsPricerSuccess)
            {
                if (medicareOutPatientResult.LineItemMedicareDetails != null &&
                    medicareOutPatientResult.LineItemMedicareDetails.Count > 0 &&
                    medicareOutPatientResult.LineItemMedicareDetails.Any(q => q.LineItemId == lineId))
                {
                    WRecordData wRecordData =
                        medicareOutPatientResult.LineItemMedicareDetails.First(
                            q => q.LineItemId == lineId);
                    resultAmount = wRecordData.LinePaymentAmount;
                }
                else if (medicareOutPatientResult.ReturnCode == 0 &&
                         medicareOutPatientResult.Message == Constants.MessageMicrodyneOff)
                    resultAmount = 0;
            }

            return resultAmount;
        }


        /// <summary>
        /// Evaluates at claim level.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="medicareOutPatientResult">The medicare out patient result.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <returns></returns>
        private List<PaymentResult> EvaluateAtClaimLevel(List<PaymentResult> paymentResults, MedicareOutPatientResult medicareOutPatientResult, bool isCarveOut)
        {
            PaymentResult claimPaymentResult = GetClaimLevelPaymentResult(paymentResults, isCarveOut);
            if (claimPaymentResult != null && medicareOutPatientResult != null)
            {
                //Update PaymentResult and set matching ServiceTypeId,PaymentTypeDetailId & PaymentTypeId 
                Utilities.UpdatePaymentResult(claimPaymentResult, PaymentTypeMedicareOp.ServiceTypeId,
                    PaymentTypeMedicareOp.PaymentTypeDetailId, PaymentTypeMedicareOp.PaymentTypeId);

                if (medicareOutPatientResult.IsEditSuccess && medicareOutPatientResult.IsPricerSuccess)
                {
                    if (PaymentTypeMedicareOp.OutPatient.HasValue)
                    {
                        double adjValue = (PaymentTypeMedicareOp.OutPatient.Value / 100) * medicareOutPatientResult.TotalPaymentAmount;
                        claimPaymentResult.AdjudicatedValue = adjValue;
                        claimPaymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                    }
                    else
                        claimPaymentResult.ClaimStatus =
                            (byte)Enums.AdjudicationOrVarianceStatuses.AdjudicationErrorInvalidPaymentData;
                }
                else if (!medicareOutPatientResult.IsEditSuccess)
                {
                    claimPaymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;
                    claimPaymentResult.MicrodynEditErrorCodes = medicareOutPatientResult.EditErrorCodes;
                    claimPaymentResult.MicrodynEditReturnRemarks = medicareOutPatientResult.MicrodynEditReturnRemarks;
                }
                else
                {
                    claimPaymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;
                    claimPaymentResult.MicrodynPricerErrorCodes = medicareOutPatientResult.PricerErrorCodes;
                    claimPaymentResult.MicrodynEditReturnRemarks = medicareOutPatientResult.MicrodynEditReturnRemarks;
                }
            }
            return paymentResults;
        }

        /// <summary>
        ///  Actual Method call for Microdyn output retrieval
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "Microdyne"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "PricerErrorCodes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ResultsData"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "EditErrorCodes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DRecord"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SSI.ContractManagement.Shared.Helpers.ErrorLog.Log.LogInfo(System.String,System.String)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CRecord"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "microdyn"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "LRecord"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ClaimId"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ERecord"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SSI.ContractManagement.Shared.Models.MedicareOutPatientResult.set_Message(System.String)")]
        private MedicareOutPatientResult GetMedicareOutPatientResult(IEvaluateableClaim claim)
        {
            StringBuilder inputData = new StringBuilder();
            MedicareOutPatientResult medicareOutPatientResult = new MedicareOutPatientResult();

            MicrodynApcEditInput microdynApcEditInput = claim.MicrodynApcEditInput;

            string dLine = string.Empty;
            string eLine = string.Empty;
            string cLine = _cRecordLogic.GetCRecordLine(microdynApcEditInput.CRecord);
            if (claim.StatementThru >= DateTime.Parse(Constants.IcdVersionDate, CultureInfo.InvariantCulture))
            {
                eLine = _eRecordLogic.GetERecordLine(microdynApcEditInput.ERecord);
            }
            else
            {
                dLine = _dRecordLogic.GetDRecordLine(microdynApcEditInput.DRecord);
            }
            string lLine = _lRecordLogic.GetLRecordLine(microdynApcEditInput.LRecords);

            if (GlobalConfigVariable.IsMicrodynEnabled)
            {
                inputData.Append(cLine);
                string diagnosisLine = (claim.StatementThru >= DateTime.Parse(Constants.IcdVersionDate, CultureInfo.InvariantCulture) ? eLine : dLine);
                inputData.Append(diagnosisLine);
                inputData.Append(lLine);

                if (inputData.Length !=
                    Constants.CRecordLength + Constants.DRecordLength +
                    (microdynApcEditInput.LRecords.Count * Constants.LRecordLength))
                {
                    medicareOutPatientResult.ReturnCode = 1;
                    medicareOutPatientResult.Message = Constants.MessageClaimDataError;
                    if (GlobalConfigVariable.IsMicrodynLogEnabled)
                        if (claim.StatementThru >= DateTime.Parse(Constants.IcdVersionDate, CultureInfo.InvariantCulture))
                        {
                            Log.LogInfo(
                                string.Format(CultureInfo.InvariantCulture, DataIssueMessageFormatWithERecord,
                                    claim.ClaimId, cLine, Environment.NewLine,
                                    _cRecordLogic.ValidationErrors, Environment.NewLine,
                                    diagnosisLine, Environment.NewLine,
                                    _eRecordLogic.ValidationErrors, Environment.NewLine,
                                    lLine, Environment.NewLine,
                                    _lRecordLogic.ValidationErrors),
                                Constants.MicrodynApcEditDllResultsData);
                        }
                        else
                        {
                            Log.LogInfo(
                                string.Format(CultureInfo.InvariantCulture, DataIssueMessageFormatWithDRecord,
                                    claim.ClaimId, cLine, Environment.NewLine,
                                    _cRecordLogic.ValidationErrors, Environment.NewLine,
                                    diagnosisLine, Environment.NewLine,
                                    _dRecordLogic.ValidationErrors, Environment.NewLine,
                                    lLine, Environment.NewLine,
                                    _lRecordLogic.ValidationErrors),
                                Constants.MicrodynApcEditDllResultsData);
                        }
                    return medicareOutPatientResult;
                }

                Instance.InputLineCount = microdynApcEditInput.LRecords.Count;
                Instance.InputData = inputData.ToString();
                Instance.AutoDenyReject = false;

                Instance.APCEdit();

                if (Instance.APCErrorsFound)
                {
                    //Processing Unsuccessful - Edit errors found
                    //ResultsData contains claim and line item errors
                    medicareOutPatientResult.ReturnCode = Instance.ReturnCode;
                    GetInstanceMessage(true, Instance.ReturnCode, medicareOutPatientResult);
                    if (Instance.ResultsData.Length % 200 == 0)
                        medicareOutPatientResult.EditErrorCodes = GetResultMessageData(Instance.ResultsData);
                    medicareOutPatientResult.MicrodynEditReturnRemarks = "Error Code:" + medicareOutPatientResult.ReturnCode + "; " +
                                                                          medicareOutPatientResult.Message;
                    if (GlobalConfigVariable.IsMicrodynLogEnabled)
                        if (claim.StatementThru >= DateTime.Parse(Constants.IcdVersionDate, CultureInfo.InvariantCulture))
                        {
                            Log.LogInfo(
                               string.Format(CultureInfo.InvariantCulture, ApcErrorMessageFormatWithERecord, claim.ClaimId,
                                   medicareOutPatientResult.Message, Instance.ResultsData,
                                   string.IsNullOrEmpty(medicareOutPatientResult.EditErrorCodes)
                                       ? string.Empty
                                       : Constants.ErrorCodes + medicareOutPatientResult.EditErrorCodes,
                                   Environment.NewLine, cLine, Environment.NewLine, diagnosisLine, Environment.NewLine,
                                   lLine),
                               Constants.MicrodynApcEditDllResultsData);
                        }
                        else
                        {
                            Log.LogInfo(
                                string.Format(CultureInfo.InvariantCulture, ApcErrorMessageFormatWithDRecord, claim.ClaimId,
                                    medicareOutPatientResult.Message, Instance.ResultsData,
                                    string.IsNullOrEmpty(medicareOutPatientResult.EditErrorCodes)
                                        ? string.Empty
                                        : Constants.ErrorCodes + medicareOutPatientResult.EditErrorCodes,
                                    Environment.NewLine, cLine, Environment.NewLine, diagnosisLine, Environment.NewLine,
                                    lLine),
                                Constants.MicrodynApcEditDllResultsData);
                        }

                }
                else
                {
                    medicareOutPatientResult.IsEditSuccess = true;
                    string inputDataString = Instance.PaymentInputData;
                    MedicareOutPatient objPatientData = microdynApcEditInput.MedicareOutPatientRecord;
                    _apcPayDll = new NetAPCPay
                    {
                        InputLineCount = microdynApcEditInput.LRecords.Count,
                        InputData = inputDataString,
                        ProviderNumber = objPatientData.Npi,
                        ServiceFromDate = Convert.ToString(objPatientData.ServiceDate, CultureInfo.InvariantCulture),
                        BeneDeductible = Convert.ToDouble(objPatientData.BeneDeductible),
                        BloodDeductiblePints = Convert.ToInt32(objPatientData.BloodDeductiblePints),
                        AllowTerminatedProvider = Convert.ToBoolean(objPatientData.AllowTerminatorProvider, CultureInfo.InvariantCulture),
                        AdjustmentFactor = Convert.ToDouble(objPatientData.AdjustFactor),
                        AdjustmentOption = Convert.ToInt32(objPatientData.AdjustmentOptions)
                    };

                    _apcPayDll.APCCalc();
                    if (!_apcPayDll.CalcErrorsFound)
                    {
                        //No Errors found
                        //Parse the V and W records to get the out patient payment details
                        medicareOutPatientResult = GetResultData(_apcPayDll.ResultsData);
                        medicareOutPatientResult.ReturnCode = 0;
                        medicareOutPatientResult.Message = Constants.MessageNoError;
                        medicareOutPatientResult.IsPricerSuccess = true;
                    }
                    else
                    {
                        //Errors found in APC Payment calculation
                        medicareOutPatientResult.ReturnCode = _apcPayDll.ReturnCode;
                        GetInstanceMessage(false, _apcPayDll.ReturnCode, medicareOutPatientResult);
                        medicareOutPatientResult.PricerErrorCodes = medicareOutPatientResult.ReturnCode;
                        if (GlobalConfigVariable.IsMicrodynLogEnabled)
                            if (claim.StatementThru >= DateTime.Parse(Constants.IcdVersionDate, CultureInfo.InvariantCulture))
                            {
                                Log.LogInfo(
                              string.Format(CultureInfo.InvariantCulture, ApcCalcMessageFormatWithERecord, claim.ClaimId, medicareOutPatientResult.Message,
                                  medicareOutPatientResult.PricerErrorCodes == 0
                                      ? string.Empty
                                      : Constants.ErrorCodes + medicareOutPatientResult.PricerErrorCodes, _apcPayDll.ResultsData,
                              Environment.NewLine, cLine, Environment.NewLine, eLine, Environment.NewLine, lLine),
                              Constants.MicrodynApcCalcDllResultsData);
                            }
                            else
                            {
                                Log.LogInfo(
                               string.Format(CultureInfo.InvariantCulture, ApcCalcMessageFormatWithDRecord, claim.ClaimId, medicareOutPatientResult.Message,
                                   medicareOutPatientResult.PricerErrorCodes == 0
                                       ? string.Empty
                                       : Constants.ErrorCodes + medicareOutPatientResult.PricerErrorCodes, _apcPayDll.ResultsData,
                               Environment.NewLine, cLine, Environment.NewLine, dLine, Environment.NewLine, lLine),
                               Constants.MicrodynApcCalcDllResultsData);
                            }
                    }
                    _apcPayDll.Dispose();
                }
            }
            else
            {
                medicareOutPatientResult.ReturnCode = 0;
                medicareOutPatientResult.TotalPaymentAmount = 0;
                medicareOutPatientResult.Message = Constants.MessageMicrodyneOff;
                medicareOutPatientResult.IsEditSuccess = true;
            }
            return medicareOutPatientResult;
        }

        /// <summary>
        /// Gets the result message data.
        /// </summary>
        /// <param name="resultData">The result data.</param>
        /// <returns></returns>
        private static string GetResultMessageData(string resultData)
        {
            List<string> resultsDataList = new List<string>();
            List<string> errorCodes = new List<string>();

            for (int index = 0; index < resultData.Length; index = index + 200)
            {
                resultsDataList.Add(resultData.Substring(index, 200));
            }

            foreach (string result in resultsDataList)
            {

                switch (result.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture))
                {
                    case "M":
                        MRecord mRecord = new MRecord().Convert(result);
                        errorCodes = errorCodes.Union(mRecord.ClaimRejectionReasons.Union(mRecord.ClaimDenialReasons)
                            .Union(mRecord.ClaimBackToProviderReasons)).ToList();
                        break;
                    case "N":
                        NRecord nRecord = new NRecord().Convert(result);
                        errorCodes = errorCodes.Union(nRecord.ClaimSuspensionReasons.Union(nRecord.LineRejectionReasons)
                            .Union(nRecord.LineDenialReasons)).ToList();
                        break;
                    case "O":
                        ORecord oRecord = new ORecord().Convert(result);
                        errorCodes = errorCodes.Union(oRecord.Diagnosis1.Union(oRecord.Diagnosis2)
                            .Union(oRecord.Diagnosis3).Union(oRecord.Diagnosis4).Union(oRecord.Diagnosis5)).ToList();
                        break;
                    case "R":
                        RRecord rRecord = new RRecord().Convert(result);
                        errorCodes = errorCodes.Union(rRecord.ProcedureEdits).ToList();
                        break;
                    case "S":
                        SRecord sRecord = new SRecord().Convert(result);
                        errorCodes = errorCodes.Union(sRecord.Modifier1.Union(sRecord.Modifier2)
                            .Union(sRecord.Modifier3).Union(sRecord.Modifier4).Union(sRecord.Modifier5).Union(sRecord.DateEdit).Union(sRecord.RevenueEdit)).ToList();
                        break;
                }
            }
            return FilteredCodes(errorCodes);
        }

        /// <summary>
        /// Filtereds the codes.
        /// </summary>
        /// <param name="errorCodes">The error codes.</param>
        /// <returns></returns>
        private static string FilteredCodes(List<string> errorCodes)
        {
            string codes = string.Empty;
            errorCodes.RemoveAll(x => string.IsNullOrEmpty(x.Trim()));

            if (errorCodes.Any())
                codes = String.IsNullOrEmpty(codes)
                    ? string.Join(",", errorCodes)
                    : string.Format(CultureInfo.InvariantCulture, "{0},", codes) + string.Join(",", errorCodes);
            return codes;
        }

        /// <summary>
        /// Gets the instance message.
        /// </summary>
        /// <param name="isNetApc">if set to <c>true</c> [is net apc].</param>
        /// <param name="returnCode">The return code.</param>
        /// <param name="medicareOutPatientResult">The medicare out patient result.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "Pricer"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "pricer"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "APCActive"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SSI.ContractManagement.Shared.Models.MedicareOutPatientResult.set_Message(System.String)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static void GetInstanceMessage(bool isNetApc, int returnCode, MedicareOutPatientResult medicareOutPatientResult)
        {
            switch (returnCode)
            {
                case 5:
                    medicareOutPatientResult.Message = Constants.MessageMajorClaimError;
                    break;
                case 11:
                    medicareOutPatientResult.Message = Constants.MessageApcActiveError;
                    break;
                case 98:
                    medicareOutPatientResult.Message = Constants.MessageSqlTableError;
                    break;
                //Claim Level 'Error' Return Codes
                case 50:
                    medicareOutPatientResult.Message = Constants.MessageWageIndexNotLocated;
                    break;
                case 51:
                    medicareOutPatientResult.Message = Constants.MessageWageIndexZero;
                    break;
                case 52:
                    medicareOutPatientResult.Message = Constants.MessageWageIndexError;
                    break;
                case 53:
                    medicareOutPatientResult.Message = Constants.MessageServiceDateError;
                    break;
                case 54:
                    medicareOutPatientResult.Message = Constants.MessageLesserServiceDate;
                    break;
                case 55:
                    medicareOutPatientResult.Message = Constants.MessageInvalidProviderNumber;
                    break;
                case 56:
                    medicareOutPatientResult.Message = Constants.MessageInvalidBeneficiaryAmount;
                    break;
                case 90:
                    medicareOutPatientResult.Message = Constants.MessageInputNumberError;
                    break;
                case 95:
                    medicareOutPatientResult.Message = Constants.MessageApcActivePricerError;
                    break;
                //Reimbursement calculation is terminated for return codes > 50
                //Line Item Level 'Error' Return Codes
                case 30:
                    medicareOutPatientResult.Message = Constants.MessageLineItemApcError;
                    break;
                case 38:
                    medicareOutPatientResult.Message = Constants.MessageLineItemDiscountFactorError;
                    break;
                case 40:
                    medicareOutPatientResult.Message = Constants.MessageLineItemStatusIndicatorError;
                    break;
                case 48:
                    medicareOutPatientResult.Message = Constants.MessageLineItemPaymentAdjustmentFlagError;
                    break;
                //Line Item Level 'Processing' Return Codes
                case 20:
                    medicareOutPatientResult.Message = Constants.MessageLinepaymentZero;
                    break;
                case 22:
                    medicareOutPatientResult.Message = Constants.MessageLineCoInsuranceLimitation;
                    break;
                case 25:
                    medicareOutPatientResult.Message = Constants.MessageLineReducedCoInsurance;
                    break;
                case 41:
                    medicareOutPatientResult.Message = Constants.MessageStatusIndicatorNotProcessed;
                    break;
                case 42:
                    medicareOutPatientResult.Message = Constants.MessageApcZero;
                    break;
                case 43:
                    medicareOutPatientResult.Message = Constants.MessagePaymentIndicatorError;
                    break;
                case 44:
                    medicareOutPatientResult.Message = Constants.MessageStatusIndicatorError;
                    break;
                case 45:
                    medicareOutPatientResult.Message = Constants.MessagePackagingFlagError;
                    break;
                case 46:
                    medicareOutPatientResult.Message = Constants.MessageLineItemReject;
                    break;
                case 47:
                    medicareOutPatientResult.Message = Constants.MessageLineFlag;
                    break;
                case 49:
                    medicareOutPatientResult.Message = Constants.MessagePaymentMethodFlag;
                    break;
                default:
                    if (isNetApc && returnCode == 1)
                        medicareOutPatientResult.Message = Constants.MessageClaimError;
                    else if (!isNetApc && returnCode == 1)
                        medicareOutPatientResult.Message = Constants.MessageLineSuccess;
                    else if (isNetApc && returnCode == 99)
                        medicareOutPatientResult.Message = Constants.MessageApcActiveLicenseExpired;
                    else if (!isNetApc && returnCode == 99)
                        medicareOutPatientResult.Message = Constants.MessageApcActivePricerLicenseExpired;
                    else if (returnCode == 100)
                        medicareOutPatientResult.Message = Constants.MessageSqlError;
                    break;
            }
        }

        /// <summary>
        /// Gets the result data.
        /// </summary>
        /// <param name="apcResultsData">The apc results data.</param>
        /// <returns></returns>
        private static MedicareOutPatientResult GetResultData(string apcResultsData)
        {
            MedicareOutPatientResult medicareOutPatientResult = new MedicareOutPatientResult
            {
                IsEditSuccess = true,
                PricerErrorCodes = null
            };

            string vRecord = apcResultsData.Substring(0, 200);
            medicareOutPatientResult.ClaimId = vRecord.Substring(1, 17).Trim();
            medicareOutPatientResult.TotalPaymentAmount = Convert.ToDouble(vRecord.Substring(53, 9), CultureInfo.InvariantCulture);

            string wRecordFull = apcResultsData.Substring(200, apcResultsData.Length - 200);
            int lineItemsCount = (apcResultsData.Length - 200) / 200;  //This will give the number of W Records
            List<string> wRecords = SplitW(wRecordFull, 200).ToList();

            medicareOutPatientResult.ReturnCode = 0;
            medicareOutPatientResult.Message = string.Empty;
            medicareOutPatientResult.LineItemMedicareDetails = new List<WRecordData>();

            if (wRecords.Count == lineItemsCount) //This ensures that the parsing if fine
            {
                foreach (string wRecord in wRecords)
                {
                    WRecordData wRecordData = new WRecordData { LineItemId = Convert.ToInt32(wRecord.Substring(18, 3), CultureInfo.InvariantCulture) };
                    string wholeNumber = wRecord.Substring(21, 7);
                    string decimalNumber = wRecord.Substring(28, 2);
                    wRecordData.LinePaymentAmount = Convert.ToDouble(wholeNumber + "." + decimalNumber, CultureInfo.InvariantCulture);
                    wRecordData.ReturnCode = 0;
                    wRecordData.ErrorMessage = string.Empty;
                    medicareOutPatientResult.LineItemMedicareDetails.Add(wRecordData);
                }
            }
            else
                return null;

            return medicareOutPatientResult;
        }

        /// <summary>
        /// Splits the w.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <returns></returns>
        private static IEnumerable<string> SplitW(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}
