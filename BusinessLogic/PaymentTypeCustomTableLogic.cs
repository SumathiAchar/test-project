using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class PaymentTypeCustomTableLogic : PaymentTypeBaseLogic
    {
        private readonly IPaymentTypeCustomTableRepository _paymentTypeCustomTableRepository;

        public override PaymentTypeBase PaymentTypeBase { get; set; }

        /// <summary>
        /// Gets the payment type fee schedule.
        /// </summary>
        /// <value>
        /// The payment type fee schedule.
        /// </value>
        private PaymentTypeCustomTable PaymentTypeCustomTable
        {
            get { return PaymentTypeBase as PaymentTypeCustomTable; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeCustomTableLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypeCustomTableLogic(string connectionString)
        {
            _paymentTypeCustomTableRepository = Factory.CreateInstance<IPaymentTypeCustomTableRepository>(connectionString, true);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeCustomTableLogic"/> class.
        /// </summary>
        /// <param name="paymentTypeAscFeeScheduleDetailsRepository">The payment type asc fee schedule details repository.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asc")]
        public PaymentTypeCustomTableLogic(IPaymentTypeCustomTableRepository paymentTypeAscFeeScheduleDetailsRepository)
        {
            if (paymentTypeAscFeeScheduleDetailsRepository != null)
                _paymentTypeCustomTableRepository = paymentTypeAscFeeScheduleDetailsRepository;
        }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <returns></returns>
        public string GetHeaders(long documentId)
        {
            return _paymentTypeCustomTableRepository.GetHeaders(documentId);
        }

        /// <summary>
        /// Adds the type of the edit payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public override long AddEditPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeCustomTableRepository.AddEdit((PaymentTypeCustomTable)paymentType);
        }

        public override PaymentTypeBase GetPaymentType(PaymentTypeBase paymentType)
        {
            return _paymentTypeCustomTableRepository.GetPaymentTypeCustomTableDetails((PaymentTypeCustomTable)paymentType);
        }

        /// <summary>
        /// Entry point to custom adjudications
        /// </summary>
        /// <param name="claim">claim details</param>
        /// <param name="paymentResults">payment result out of adjudicated claims</param>
        /// <param name="isCarveOut">if the service type is carved out</param>
        /// <param name="isContractFilter">if there are contact filters</param>
        /// <returns>list of adjudicated payment result details</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public override List<PaymentResult> EvaluatePaymentType(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut, bool isContractFilter)
        {
            if (claim != null)
            {
                claim.SmartBox.CAA = Utilities.CalculateAllowedAmount(paymentResults);
                paymentResults = PaymentTypeCustomTable.ClaimFieldId == (byte)Enums.ClaimFieldTypes.RevenueCode || PaymentTypeCustomTable.ClaimFieldId == (byte)Enums.ClaimFieldTypes.HcpcsOrRateOrHipps || PaymentTypeCustomTable.ClaimFieldId == (byte)Enums.ClaimFieldTypes.PlaceOfService
                    ? EvaluateLineLevel(claim, paymentResults, isCarveOut)
                    : EvaluateClaimLevel(claim, paymentResults, isCarveOut);
            }
            return paymentResults;
        }



        /// <summary>
        /// Evaluates the claim at line level
        /// </summary>
        /// <param name="claim">details of the claim</param>
        /// <param name="paymentResults">payment result</param>
        /// <param name="isCarveOut">if the claim is carved out</param>
        /// <returns>list of payment results</returns>
        private List<PaymentResult> EvaluateLineLevel(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut)
        {

            List<CodeDetails> codeDetails = new List<CodeDetails>();
            foreach (
                    ClaimCharge claimCharge in
                        claim.ClaimCharges.TakeWhile(claimCharge => !isCarveOut || !paymentResults.Any(
                            currentPaymentResult =>
                                currentPaymentResult.Line == claimCharge.Line &&
                                currentPaymentResult.ServiceTypeId == PaymentTypeBase.ServiceTypeId))
                            .Where(
                                claimCharge =>
                                    PaymentTypeBase.ValidLineIds.Contains(claimCharge.Line) && paymentResults.Any(
                                        currentPaymentResult =>
                                            currentPaymentResult.Line == claimCharge.Line &&
                                            (currentPaymentResult.AdjudicatedValue == null || isCarveOut))))
            {
                EvaluateLine(paymentResults, claim, claimCharge, isCarveOut, codeDetails);
            }
            if (PaymentTypeCustomTable.IsPerCode)
                GroupLinesByCode(paymentResults);
            else if (PaymentTypeCustomTable.IsPerDayOfStay)
                GroupLinesByPerDayOfStay(paymentResults);

            return paymentResults;
        }


        /// <summary>
        /// Evaluates the type of the payment.
        /// </summary>
        /// <param name="paymentResults">The payment result.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="claimCharge">The claim charge.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="codeDetailsList">The per day of stays.</param>
        private void EvaluateLine(List<PaymentResult> paymentResults, IEvaluateableClaim claim, ClaimCharge claimCharge, bool isCarveOut, List<CodeDetails> codeDetailsList)
        {
            if (PaymentTypeCustomTable.ClaimFieldDoc != null &&
                PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues != null &&
                PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.Count > 0)
            {
                string limitExpandedExpression = PaymentTypeCustomTable.ObserveServiceUnitLimit;
                ClaimFieldValue uploadedValue = GetMatchedClaimFieldValues(claim, claimCharge);
                if (uploadedValue != null)
                {
                    PaymentResult paymentResult = GetPaymentResult(paymentResults, isCarveOut, claimCharge.Line);
                    if (paymentResult != null)
                    {
                        claim.SmartBox.LC = claimCharge.Amount;
                        Dictionary<string, string> claimFieldDocValueDictionary =
                        PrepareClaimFieldDocValueDictionary(uploadedValue);
                        int limit = 0;
                        if (!string.IsNullOrWhiteSpace(PaymentTypeCustomTable.ObserveServiceUnitLimit))
                        {
                            limitExpandedExpression = GetExpandedExpression(claimFieldDocValueDictionary, limitExpandedExpression);
                            limit = (Convert.ToInt32(Utilities.EvaluateExpression(limitExpandedExpression, claim,
                                PaymentTypeCustomTable, Constants.LimitOccurence)));
                        }
                        limit = (limit < 0) ? 0 : limit; //if limit is negative value then making it zero
                        string code = uploadedValue.Identifier;
                        if (PaymentTypeCustomTable.IsPerDayOfStay)
                            AddLimitsPerdayOfStay(claimCharge, codeDetailsList, code, limit);
                        else
                            AddLimitByCode(codeDetailsList, code, limit);

                        EvaluateAdjudicatedValue(paymentResult, uploadedValue, claim, codeDetailsList, claimCharge);
                    }
                }


            }
        }

        /// <summary>
        /// prepares a dictionary of variables and values present in excel sheet
        /// </summary>
        /// <param name="uploadedValue">uploaded values</param>
        /// <returns>a dictionary with variables and values</returns>
        private Dictionary<string, string> PrepareClaimFieldDocValueDictionary(ClaimFieldValue uploadedValue)
        {
            Dictionary<string, string> claimFieldDocValueDictionary = new Dictionary<string, string>();

            List<string> headers = Utilities.SplitCsvRowToArray(PaymentTypeCustomTable.ClaimFieldDoc.ColumnHeaderSecond);
            List<string> values = Utilities.SplitCsvRowToArrayForAdjudication(uploadedValue.Value);

            for (int index = 0; index < headers.Count; index++)
            {
                string value = Regex.Replace(
                    values == null || values.Count == 0 || values.Count < index - 1 ? string.Empty : values[index],
                    Constants.RegExPatternDoubleUnderscore, string.Empty);
                claimFieldDocValueDictionary.Add(
                    Constants.ReferenceTableTag + Regex.Replace(headers[index], Constants.RegExPatternDoubleUnderscore, Constants.Comma),
                    Regex.Replace(value, Constants.RegExPatternSpecialCharacter, string.Empty));
            }

            return claimFieldDocValueDictionary;
        }

        /// <summary>
        /// searches for the matched claims from claim details and uploaded file 
        /// </summary>
        /// <param name="claim">claim details</param>
        /// <param name="claimCharge">claim charge details</param>
        /// <returns>matched claim values </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private ClaimFieldValue GetMatchedClaimFieldValues(IEvaluateableClaim claim, ClaimCharge claimCharge = null)
        {
            ClaimFieldValue claimFieldValue;
            switch (PaymentTypeCustomTable.ClaimFieldId)
            {
                case (byte)Enums.ClaimFieldTypes.Drg:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => PrePendZeros(claimField.Identifier) == PrePendZeros(claim.Drg));
                    break;
                case (byte)Enums.ClaimFieldTypes.BillType:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claimField.Identifier.ToUpper(CultureInfo.InvariantCulture) == claim.BillType.ToUpper(CultureInfo.InvariantCulture));
                    break;
                case (byte)Enums.ClaimFieldTypes.PatientAccountNumber:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claimField.Identifier.ToUpper(CultureInfo.InvariantCulture) == claim.PatAcctNum.ToUpper(CultureInfo.InvariantCulture));
                    break;
                case (byte)Enums.ClaimFieldTypes.PayerName:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claimField.Identifier.ToUpperInvariant() == claim.PriPayerName.ToUpperInvariant());
                    break;
                case (byte)Enums.ClaimFieldTypes.InsuredId:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claim.InsuredCodes.Any(x => x.CertificationNumber.ToUpperInvariant() == claimField.Identifier.ToUpperInvariant()));
                    break;
                case (byte)Enums.ClaimFieldTypes.InsuredGroup:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claim.InsuredCodes.Any(x => x.GroupNumber.ToUpperInvariant() == claimField.Identifier.ToUpperInvariant()));
                    break;
                case (byte)Enums.ClaimFieldTypes.Icn:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claimField.Identifier.ToUpperInvariant() == claim.Icn.ToUpperInvariant());
                    break;
                case (byte)Enums.ClaimFieldTypes.Mrn:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claimField.Identifier.ToUpperInvariant() == claim.Mrn.ToUpperInvariant());
                    break;
                case (byte)Enums.ClaimFieldTypes.ReferringPhysician:
                    claimFieldValue = GetReferringPhysicianClaimFieldValue(claim);
                    break;
                case (byte)Enums.ClaimFieldTypes.RenderingPhysician:
                    claimFieldValue = GetRenderingPhysicianClaimFieldValue(claim);
                    break;
                case (byte)Enums.ClaimFieldTypes.AttendingPhysician:
                    claimFieldValue = GetAttendingPhysicianClaimFieldValue(claim);
                    break;
                case (byte)Enums.ClaimFieldTypes.IcdDiagnosis:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claim.DiagnosisCodes.Any(x => x.IcddCode.ToUpperInvariant() == claimField.Identifier.ToUpperInvariant()));
                    break;
                case (byte)Enums.ClaimFieldTypes.IcdProcedure:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claim.ProcedureCodes.Any(x => x.IcdpCode.ToUpperInvariant() == claimField.Identifier.ToUpperInvariant()));
                    break;
                case (byte)Enums.ClaimFieldTypes.ValueCodes:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claim.ValueCodes.Any(x => x.Code.ToUpperInvariant() == claimField.Identifier.ToUpperInvariant()));
                    break;
                case (byte)Enums.ClaimFieldTypes.OccurrenceCode:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claim.OccurrenceCodes.Any(x => x.Code.ToUpperInvariant() == claimField.Identifier.ToUpperInvariant()));
                    break;
                case (byte)Enums.ClaimFieldTypes.ConditionCodes:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claim.ConditionCodes.Any(x => x.Code.ToUpperInvariant() == claimField.Identifier.ToUpperInvariant()));
                    break;
                case (byte)Enums.ClaimFieldTypes.CustomField1:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.First(claimField => claim.CustomField1 == claimField.Identifier);
                    break;
                case (byte)Enums.ClaimFieldTypes.CustomField2:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.First(claimField => claim.CustomField2 == claimField.Identifier);
                    break;
                case (byte)Enums.ClaimFieldTypes.CustomField3:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.First(claimField => claim.CustomField4 == claimField.Identifier);
                    break;
                case (byte)Enums.ClaimFieldTypes.CustomField4:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.First(claimField => claim.CustomField4 == claimField.Identifier);
                    break;
                case (byte)Enums.ClaimFieldTypes.CustomField5:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.First(claimField => claim.CustomField5 == claimField.Identifier);
                    break;
                case (byte)Enums.ClaimFieldTypes.CustomField6:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.First(claimField => claim.CustomField6 == claimField.Identifier);
                    break;
                case (byte)Enums.ClaimFieldTypes.RevenueCode:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(currentClaimFieldValue => claimCharge != null && ((currentClaimFieldValue.Identifier.Length == 4 ? Regex.Replace(currentClaimFieldValue.Identifier, "^0?", string.Empty) : currentClaimFieldValue.Identifier) == claimCharge.RevCode));
                    break;
                case (byte)Enums.ClaimFieldTypes.HcpcsOrRateOrHipps:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.
                        FirstOrDefault(currentClaimFieldValue => claimCharge != null && (currentClaimFieldValue.Identifier.Equals(claimCharge.HcpcsCodeWithModifier, StringComparison.OrdinalIgnoreCase)));
                    if (claimCharge != null && (claimFieldValue == null && claimCharge.HcpcsCodeWithModifier.Trim().Length != 5))
                        claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.
                       FirstOrDefault(currentClaimFieldValue => (currentClaimFieldValue.Identifier.Equals(claimCharge.HcpcsCode, StringComparison.OrdinalIgnoreCase)));
                    break;
                case (byte)Enums.ClaimFieldTypes.PlaceOfService:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(currentClaimFieldValue => claimCharge != null && currentClaimFieldValue.Identifier.Equals(claimCharge.PlaceOfService, StringComparison.OrdinalIgnoreCase));
                    break;
                case (byte)Enums.ClaimFieldTypes.Los:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claimField.Identifier == Convert.ToString(claim.Los, CultureInfo.InvariantCulture));
                    break;
                case (byte)Enums.ClaimFieldTypes.Age:
                    claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.FirstOrDefault(claimField => claimField.Identifier == Convert.ToString(claim.Age, CultureInfo.InvariantCulture));
                    break;
                default:
                    claimFieldValue = new ClaimFieldValue();
                    break;
            }
            return claimFieldValue;
        }


        /// <summary>
        /// Get Attending physician claim field value.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        private ClaimFieldValue GetAttendingPhysicianClaimFieldValue(IEvaluateableClaim claim)
        {
            var claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.First(
                claimField =>
                    claim.Physicians.Any(
                        x =>
                            x.PhysicianType.Equals(Constants.PropertyAttendingPhysician, StringComparison.OrdinalIgnoreCase) &&
                            claimField.Identifier.Replace(Constants.Space, string.Empty)
                                .Equals(
                                    string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}",
                                        x.FirstName.Replace(Constants.Space, string.Empty),
                                        x.MiddleName.Replace(Constants.Space, string.Empty),
                                        x.LastName.Replace(Constants.Space, string.Empty)),
                                    StringComparison.OrdinalIgnoreCase)));
            return claimFieldValue;
        }


        /// <summary>
        /// Gets the rendering physician claim field value.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        private ClaimFieldValue GetRenderingPhysicianClaimFieldValue(IEvaluateableClaim claim)
        {
            var claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.First(claimField => claim.Physicians.Any(
                x =>
                    x.PhysicianType.Equals(Constants.PropertyRenderingPhysician, StringComparison.OrdinalIgnoreCase) &&
                    claimField.Identifier.Replace(Constants.Space, string.Empty)
                        .Equals(
                            string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}",
                                x.FirstName.Replace(Constants.Space, string.Empty),
                                x.MiddleName.Replace(Constants.Space, string.Empty),
                                x.LastName.Replace(Constants.Space, string.Empty)),
                            StringComparison.OrdinalIgnoreCase)));
            return claimFieldValue;
        }


        /// <summary>
        /// Gets the claim field value.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        private ClaimFieldValue GetReferringPhysicianClaimFieldValue(IEvaluateableClaim claim)
        {
            var claimFieldValue = PaymentTypeCustomTable.ClaimFieldDoc.ClaimFieldValues.First(claimField => claim.Physicians.Any(
                x =>
                    x.PhysicianType.Equals(Constants.PropertyReferringPhysician, StringComparison.OrdinalIgnoreCase) &&
                    claimField.Identifier.Replace(Constants.Space, string.Empty)
                        .Equals(
                            string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}",
                                x.FirstName.Replace(Constants.Space, string.Empty),
                                x.MiddleName.Replace(Constants.Space, string.Empty),
                                x.LastName.Replace(Constants.Space, string.Empty)),
                            StringComparison.OrdinalIgnoreCase)));
            return claimFieldValue;
        }

        /// <summary>
        /// Evaluates the claim at claim level
        /// </summary>
        /// <param name="claim">claim to be adjudicated</param>
        /// <param name="paymentResults">payment result out of adjudicated claim</param>
        /// <param name="isCarveOut">if the service type is carved out</param>
        /// <returns>list of payment results</returns>
        private List<PaymentResult> EvaluateClaimLevel(IEvaluateableClaim claim, List<PaymentResult> paymentResults, bool isCarveOut)
        {
            ClaimFieldValue uploadedValue = GetMatchedClaimFieldValues(claim);
            if (uploadedValue != null)
            {
                PaymentResult claimPaymentResult = GetClaimLevelPaymentResult(paymentResults, isCarveOut);
                if (claimPaymentResult != null)
                {
                    EvaluateAdjudicatedValue(claimPaymentResult, uploadedValue, claim);
                    //Remove PaymentResult if its not satisfied threshold condition 
                    if (claimPaymentResult.AdjudicatedValue == null &&
                        claimPaymentResult.ClaimStatus == (byte)Enums.AdjudicationOrVarianceStatuses.UnAdjudicated)
                        paymentResults.Remove(claimPaymentResult);
                }
            }
            return paymentResults;
        }

        /// <summary>
        /// calculates and adds the result to payment dictionary
        /// </summary>
        /// <param name="claimPaymentResult">payment result of the claim</param>
        /// <param name="uploadedValue">uploaded excel values</param>
        /// <param name="claim">claim details</param>
        /// <param name="codeDetailsList">The codes.</param>
        /// <param name="claimCharge">The claim charge.</param>
        private void EvaluateAdjudicatedValue(PaymentResult claimPaymentResult, ClaimFieldValue uploadedValue, IEvaluateableClaim claim, List<CodeDetails> codeDetailsList = null, ClaimCharge claimCharge = null)
        {
            Utilities.UpdatePaymentResult(claimPaymentResult, PaymentTypeCustomTable.ServiceTypeId,
                        PaymentTypeCustomTable.PaymentTypeDetailId, PaymentTypeCustomTable.PaymentTypeId);
            Dictionary<string, string> claimFieldDocValueDictionary =
                PrepareClaimFieldDocValueDictionary(uploadedValue);
            double convertedAmount;
            string[] expandedExpression = { PaymentTypeCustomTable.Expression };
            List<string> validValues = new List<string>();
            foreach (KeyValuePair<string, string> pair in claimFieldDocValueDictionary.OrderByDescending(key => key.Key.Length).Where(pair => expandedExpression[0].IndexOf(pair.Key, StringComparison.CurrentCultureIgnoreCase) >= 0))
            {
                validValues.Add(pair.Value);
                expandedExpression[0] = Regex.Replace(expandedExpression[0], pair.Key, pair.Value, RegexOptions.IgnoreCase);
            }

            if (claimCharge != null && codeDetailsList != null)
            {
                double? formula = Utilities.EvaluateExpression(expandedExpression[0], claim,
                    PaymentTypeCustomTable);
                EvaluateLineByOccurence(claimPaymentResult, claim, codeDetailsList, claimCharge, claimFieldDocValueDictionary, formula, uploadedValue);
            }
            else if (validValues.Any(x => !double.TryParse(x, out convertedAmount)))
            {
                claimPaymentResult.ClaimStatus = validValues.Any(string.IsNullOrEmpty)
                    ? (byte)Enums.AdjudicationOrVarianceStatuses.AdjudicationErrorInvalidPaymentData
                    : (byte)Enums.AdjudicationOrVarianceStatuses.UnAdjudicated;
            }
            else
            {
                claimPaymentResult.AdjudicatedValue = Utilities.EvaluateExpression(expandedExpression[0], claim,
                    PaymentTypeCustomTable);
                claimPaymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                Utilities.UpdateCustomTableDetails(claimPaymentResult, claim.SmartBox, PaymentTypeCustomTable);
            }

        }

        /// <summary>
        /// Gets the code and occurence value.
        /// </summary>
        /// <param name="uploadedValue">The uploaded value.</param>
        /// <param name="codeDetailsList">The codes.</param>
        /// <param name="claimCharge">The claim charge.</param>
        /// <param name="codeDetails">The code details.</param>
        private void GetCodeAndOccurenceValue(ClaimFieldValue uploadedValue, IEnumerable<CodeDetails> codeDetailsList,
            ClaimCharge claimCharge, CodeDetails codeDetails)
        {
            if (!PaymentTypeCustomTable.IsPerDayOfStay)
            {
                CodeDetails perCode = codeDetailsList.First(
                    pday => pday.Code == uploadedValue.Identifier);
                codeDetails.Occurence = perCode.Occurence;
                codeDetails.Code = perCode.Code;
                codeDetails.Limit = perCode.Limit;
            }
            else
            {
                CodeDetails perDayOfStay = codeDetailsList.First(
                    pday => pday.Code == uploadedValue.Identifier && pday.Day == claimCharge.ServiceFromDate);
                codeDetails.Occurence = perDayOfStay.Occurence;
                codeDetails.Code = perDayOfStay.Code;
                codeDetails.Limit = perDayOfStay.Limit;
            }


        }

        /// <summary>
        /// Evaluates the line by occurence.
        /// </summary>
        /// <param name="claimPaymentResult">The claim payment result.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="codeDetailsList">The codes.</param>
        /// <param name="claimCharge">The claim charge.</param>
        /// <param name="claimFieldDocValueDictionary">The claim field document value dictionary.</param>
        /// <param name="formula">The formula.</param>
        /// <param name="uploadedValue">The uploaded value.</param>
        private void EvaluateLineByOccurence(PaymentResult claimPaymentResult, IEvaluateableClaim claim,
            List<CodeDetails> codeDetailsList, ClaimCharge claimCharge, Dictionary<string, string> claimFieldDocValueDictionary,
            double? formula, ClaimFieldValue uploadedValue)
        {
            bool isMultiplierexist = PaymentTypeCustomTable.MultiplierFirst != null || PaymentTypeCustomTable.MultiplierSecond != null ||
                                     PaymentTypeCustomTable.MultiplierThird != null || PaymentTypeCustomTable.MultiplierFourth != null ||
                                     PaymentTypeCustomTable.MultiplierOther != null;
            for (int unit = 0; unit < ((!PaymentTypeCustomTable.IsObserveServiceUnit && !isMultiplierexist) ? 1 : claimCharge.Units); unit++)
            {
                CodeDetails codeDetails = new CodeDetails();
                GetCodeAndOccurenceValue(uploadedValue, codeDetailsList, claimCharge, codeDetails);
                if ((PaymentTypeCustomTable.IsObserveServiceUnit && codeDetails.Limit > 0) ||
                    (PaymentTypeCustomTable.IsObserveServiceUnit &&
                     PaymentTypeCustomTable.ObserveServiceUnitLimit == null) || !PaymentTypeCustomTable.IsObserveServiceUnit)
                {

                    switch (codeDetails.Occurence)
                    {
                        case 1:
                            string multiplierFirstExpandedExpression = PaymentTypeCustomTable.MultiplierFirst ??
                                                                       Constants.StringOne;
                            CalculateAdjudicatedValue(claimPaymentResult, claim, codeDetailsList,
                                claimCharge,
                                multiplierFirstExpandedExpression, claimFieldDocValueDictionary, codeDetails.Code,
                                codeDetails.Occurence,
                                formula, isMultiplierexist);
                            break;
                        case 2:
                            string multiplierSecondExpandedExpression = PaymentTypeCustomTable.MultiplierSecond ??
                                                                        Constants.StringOne;
                            CalculateAdjudicatedValue(claimPaymentResult, claim, codeDetailsList,
                                claimCharge,
                                multiplierSecondExpandedExpression, claimFieldDocValueDictionary, codeDetails.Code,
                                codeDetails.Occurence,
                                formula, isMultiplierexist);
                            break;
                        case 3:
                            string multiplierThirdExpandedExpression = PaymentTypeCustomTable.MultiplierThird ??
                                                                       Constants.StringOne;
                            CalculateAdjudicatedValue(claimPaymentResult, claim, codeDetailsList,
                                claimCharge,
                                multiplierThirdExpandedExpression, claimFieldDocValueDictionary, codeDetails.Code,
                                codeDetails.Occurence,
                                formula, isMultiplierexist);
                            break;
                        case 4:
                            string multiplierFourthExpandedExpression = PaymentTypeCustomTable.MultiplierFourth ??
                                                                        Constants.StringOne;
                            CalculateAdjudicatedValue(claimPaymentResult, claim, codeDetailsList,
                                claimCharge,
                                multiplierFourthExpandedExpression, claimFieldDocValueDictionary, codeDetails.Code,
                                codeDetails.Occurence,
                                formula, isMultiplierexist);
                            break;
                        default:
                            string multiplierOthersExpandedExpression = PaymentTypeCustomTable.MultiplierOther ??
                                                                        Constants.StringOne;
                            CalculateAdjudicatedValue(claimPaymentResult, claim, codeDetailsList,
                                claimCharge,
                                multiplierOthersExpandedExpression, claimFieldDocValueDictionary, codeDetails.Code,
                                codeDetails.Occurence,
                                formula, isMultiplierexist);
                            break;
                    }
                }
                else if (claimPaymentResult.AdjudicatedValue == null)
                {
                    claimPaymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                    claimPaymentResult.AdjudicatedValue = 0.0;
                }
            }
        }

        /// <summary>
        /// Calculates the adjudicated value.
        /// </summary>
        /// <param name="claimPaymentResult">The claim payment result.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="codeDetailsList">The per day of stays.</param>
        /// <param name="claimCharge">The claim charge.</param>
        /// <param name="multiplierExpandedExpression">The multiplier expanded expression.</param>
        /// <param name="claimFieldDocValueDictionary">The claim field document value dictionary.</param>
        /// <param name="codeValue">The code value.</param>
        /// <param name="codeOccurence">The code occurence.</param>
        /// <param name="formula">The formula.</param>
        /// <param name="isMultiplierExist">if set to <c>true</c> [is multiplier exist].</param>
        private void CalculateAdjudicatedValue(PaymentResult claimPaymentResult, IEvaluateableClaim claim,
            List<CodeDetails> codeDetailsList, ClaimCharge claimCharge, string multiplierExpandedExpression,
            Dictionary<string, string> claimFieldDocValueDictionary, string codeValue, int codeOccurence, double? formula,bool isMultiplierExist)
        {
            multiplierExpandedExpression = GetExpandedExpression(claimFieldDocValueDictionary,
                multiplierExpandedExpression);
            if (PaymentTypeCustomTable.IsPerDayOfStay)
            {
                var perDayOfStay = codeDetailsList.FirstOrDefault(
                    p => p.Code == codeValue && p.Day == claimCharge.ServiceFromDate);
                if (perDayOfStay != null)
                {
                    GetMultipliedFormulaValue(claimPaymentResult, claim, multiplierExpandedExpression, codeOccurence, formula);
                    UpdateLimit(claimPaymentResult, codeDetailsList, claimCharge, codeValue, isMultiplierExist
                        );
                }
                claimPaymentResult.ServiceLineCode = codeValue;
                claimPaymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                Utilities.UpdateCustomTableDetails(claimPaymentResult, claim.SmartBox, PaymentTypeCustomTable, codeOccurence);
            }
            else
            {
                GetMultipliedFormulaValue(claimPaymentResult, claim, multiplierExpandedExpression, codeOccurence, formula);
                UpdateLimit(claimPaymentResult, codeDetailsList, claimCharge, codeValue, isMultiplierExist);
                claimPaymentResult.ServiceLineCode = codeValue;
                claimPaymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.Adjudicated;
                Utilities.UpdateCustomTableDetails(claimPaymentResult, claim.SmartBox, PaymentTypeCustomTable, codeOccurence);
            }
        }

        /// <summary>
        /// Updates the limit.
        /// </summary>
        /// <param name="claimPaymentResult">The claim payment result.</param>
        /// <param name="codeDetailsList">The codes.</param>
        /// <param name="claimCharge">The claim charge.</param>
        /// <param name="codeValue">The code value.</param>
        /// <param name="isMultiplierExist">if set to <c>true</c> [is multiplier exist].</param>
        private void UpdateLimit(PaymentResult claimPaymentResult, IEnumerable<CodeDetails> codeDetailsList, ClaimCharge claimCharge, string codeValue,bool isMultiplierExist)
        {
            if (PaymentTypeCustomTable.IsPerDayOfStay)
                foreach (
                    CodeDetails claimCode in
                        codeDetailsList.Where(p => p.Code == codeValue && p.Day == claimCharge.ServiceFromDate))
                {
                    claimCode.Limit -= Constants.One;
                    claimPaymentResult.ServiceLineDate = claimCharge.ServiceFromDate;
                    claimCode.Occurence += ((!PaymentTypeCustomTable.IsObserveServiceUnit && !isMultiplierExist && claimCharge.Units != null) ? claimCharge.Units.Value : Constants.One);
                }
            else
            {
                foreach (CodeDetails claimCode in codeDetailsList)
                {
                    if (claimCode.Code == codeValue)
                    {
                        claimCode.Limit -= Constants.One;
                        claimCode.Occurence += ((!PaymentTypeCustomTable.IsObserveServiceUnit && !isMultiplierExist && claimCharge.Units != null) ? claimCharge.Units.Value : Constants.One);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the multiplied formula value.
        /// </summary>
        /// <param name="claimPaymentResult">The claim payment result.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="multiplierExpandedExpression">The mulpilier expanded expression.</param>
        /// <param name="codeOccurence">The code occurence.</param>
        /// <param name="formula">The formula.</param>
        private void GetMultipliedFormulaValue(PaymentResult claimPaymentResult, IEvaluateableClaim claim,
            string multiplierExpandedExpression, int codeOccurence, double? formula)
        {
            claimPaymentResult.AdjudicatedValue = (claimPaymentResult.AdjudicatedValue ?? 0) + (Utilities.EvaluateExpression(
                multiplierExpandedExpression, claim,
                PaymentTypeCustomTable, codeOccurence) * formula);
        }


        /// <summary>
        /// Groups the lines by code.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        private static void GroupLinesByCode(List<PaymentResult> paymentResults)
        {
            var perGruopCodes = (from perCode in paymentResults
                                 where perCode.ServiceLineCode != null
                                 group perCode by new { perCode.ServiceLineCode, perCode.ServiceTypeId }
                                     into grp
                                     select new
                                     {
                                         grp.Key.ServiceLineCode,
                                         grp.Key.ServiceTypeId
                                     }).ToList();
            foreach (var perGruopCode in perGruopCodes)
            {
                int firstElementLine = 0;
                for (int index = 0; index < paymentResults.Count; index++)
                {
                    if (perGruopCode.ServiceLineCode == paymentResults[index].ServiceLineCode &&
                        perGruopCode.ServiceTypeId == paymentResults[index].ServiceTypeId && firstElementLine == 0)
                    {
                        firstElementLine = index;
                    }
                    else if (perGruopCode.ServiceLineCode == paymentResults[index].ServiceLineCode &&
                             perGruopCode.ServiceTypeId == paymentResults[index].ServiceTypeId)
                    {
                        paymentResults[firstElementLine].AdjudicatedValue +=
                            paymentResults[index].AdjudicatedValue;
                        paymentResults[index].AdjudicatedValue = 0.0;
                    }
                }
            }
        }


        /// <summary>
        /// Prepend zeros For Drg Code.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static string PrePendZeros(string value)
        {
            while (value.Length < 3)
            {
                value = Constants.PrefixValue + value;
            }
            return value;
        }
        /// <summary>
        /// Groups the lines by per day of stay.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        private static void GroupLinesByPerDayOfStay(List<PaymentResult> paymentResults)
        {
            var perDayOfStayGroup = (from perDayOfStay in paymentResults
                                     where perDayOfStay.ServiceLineCode != null
                                     group perDayOfStay by
                                         new { perDayOfStay.ServiceLineDate, perDayOfStay.ServiceLineCode, perDayOfStay.ServiceTypeId }
                                         into grp
                                         select new
                                         {
                                             grp.Key.ServiceLineCode,
                                             grp.Key.ServiceLineDate,
                                             grp.Key.ServiceTypeId
                                         }).ToList();
            foreach (var perDayStay in perDayOfStayGroup)
            {
                int firstElementLine = 0;
                for (int index = 0; index < paymentResults.Count; index++)
                {
                    if (perDayStay.ServiceLineCode == paymentResults[index].ServiceLineCode &&
                        perDayStay.ServiceLineDate == paymentResults[index].ServiceLineDate &&
                        perDayStay.ServiceTypeId == paymentResults[index].ServiceTypeId && firstElementLine == 0)
                    {
                        firstElementLine = index;
                    }
                    else if (perDayStay.ServiceLineCode == paymentResults[index].ServiceLineCode &&
                             perDayStay.ServiceLineDate == paymentResults[index].ServiceLineDate &&
                             perDayStay.ServiceTypeId == paymentResults[index].ServiceTypeId)
                    {
                        paymentResults[firstElementLine].AdjudicatedValue +=
                            paymentResults[index].AdjudicatedValue;
                        paymentResults[index].AdjudicatedValue = 0.0;
                    }
                }
            }
        }

        /// <summary>
        /// Adds the limit by code.
        /// </summary>
        /// <param name="codeDetailsList">The codes.</param>
        /// <param name="code">The code.</param>
        /// <param name="limit">The limit.</param>
        private void AddLimitByCode(List<CodeDetails> codeDetailsList, string code, int limit)
        {
            if (codeDetailsList.Count == 0 || codeDetailsList.All(identifier => identifier.Code != code))
            {
                codeDetailsList.Add(new CodeDetails
                {
                    Code = code,
                    Occurence = Constants.One,
                    Limit = limit
                });
            }
        }

        /// <summary>
        /// Adds the limits by per day of stay.
        /// </summary>
        /// <param name="claimCharge">The claim charge.</param>
        /// <param name="codes">The per day of stays.</param>
        /// <param name="code">The code.</param>
        /// <param name="limit">The limit.</param>
        private static void AddLimitsPerdayOfStay(ClaimCharge claimCharge, List<CodeDetails> codes, string code, int limit)
        {
            if (codes.Count == 0 || codes.All(day => day.Day != claimCharge.ServiceFromDate ||
                                                                   day.Code != code))
            {
                codes.Add(new CodeDetails
                {
                    Code = code,
                    Day = claimCharge.ServiceFromDate,
                    Occurence = Constants.One,
                    Limit = limit
                });
            }
        }

        /// <summary>
        /// Gets the expanded expression.
        /// </summary>
        /// <param name="claimFieldDocValueDictionary">The claim field document value dictionary.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        private static string GetExpandedExpression(Dictionary<string, string> claimFieldDocValueDictionary, string expression)
        {
            foreach (KeyValuePair<string, string> pair in claimFieldDocValueDictionary.OrderByDescending(key => key.Key.Length))
            {
                if (expression.IndexOf(pair.Key, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    expression = Regex.Replace(expression, pair.Key, pair.Value, RegexOptions.IgnoreCase);
                }
            }
            return expression;
        }
    }
}