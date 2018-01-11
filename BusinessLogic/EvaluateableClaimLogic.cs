using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// Class for the EvaluateableClaim logic
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Evaluateable")]
    public class EvaluateableClaimLogic : IEvaluateableClaimLogic
    {
        //Constants value to build C,L,D and MicrodynApcEditInput.
        private const int OppFlag = 1;
        private const double BeneAmount = 0.0;
        private const int BloodPint = 0;
        private const int LineItemFlag = 0;
        private const double BeneDeductible = 0;
        private const int BloodDeductiblePints = 3;
        private const string AllowTerminatorProvider = "false";
        private const int AdjustFactor = 1;
        private const int AdjustmentOptions = 0;
        private static readonly string[] PrincipalDiagnosisCodes = { "P", "1" };

        public bool IsMatch(IEvaluateableClaim claim)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Evaluates the specified claim.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentResults">The payment results.</param>
        /// <param name="isCarveOut">if set to <c>true</c> [is carve out].</param>
        /// <param name="isContractFilter">if set to <c>true</c> [is contract filter].</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<PaymentResult> Evaluate(IEvaluateableClaim claim, List<PaymentResult> paymentResults,
            bool isCarveOut, bool isContractFilter)
        {
            //Get claim level payment result
            PaymentResult overAllClaimPaymentResult = paymentResults.FirstOrDefault(
                payment => payment.Line == null && payment.ServiceTypeId == null);

            if (claim != null && !string.IsNullOrEmpty(claim.PriPayerName) &&
                !string.IsNullOrEmpty(claim.BillType) && !string.IsNullOrEmpty(claim.ClaimState) &&
                !string.IsNullOrEmpty(claim.ClaimType) && claim.StatementFrom.HasValue &&
                claim.StatementThru.HasValue && claim.ClaimTotal.HasValue)
            {
                if (!(claim.ClaimCharges != null && claim.ClaimCharges.Any()) && overAllClaimPaymentResult != null)
                    overAllClaimPaymentResult.ClaimStatus =
                        (byte)Enums.AdjudicationOrVarianceStatuses.AdjudicationErrorMissingServiceLine;
            }
            else if (overAllClaimPaymentResult != null)
                overAllClaimPaymentResult.ClaimStatus =
                    (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError;

            return paymentResults;
        }

        /// <summary>
        /// Updates the evaluate able claims.
        /// </summary>
        /// <param name="evaluateableClaims">The evaluate able claims.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "SSI.ContractManagement.Shared.Helpers.ErrorLog.Log.LogError(System.String,System.String,System.Exception)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "MicrodynApcEditInput"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<EvaluateableClaim> UpdateEvaluateableClaims(List<EvaluateableClaim> evaluateableClaims)
        {
            if (evaluateableClaims != null)
            {
                foreach (EvaluateableClaim evaluateableClaim in evaluateableClaims)
                {
                    //Update SmartBox values
                    evaluateableClaim.SmartBox = GetSmartBox(evaluateableClaim);

                    evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
                    {
                        ClaimId = evaluateableClaim.ClaimId
                    };
                    try
                    {
                        //Getting CRecord Data
                        GetCRecord(evaluateableClaim);
                        //Getting D,E,L
                        GetAllRecord(evaluateableClaim);

                        //Getting Medicare Out patient
                        evaluateableClaim.MicrodynApcEditInput.MedicareOutPatientRecord =
                            new MedicareOutPatient
                            {
                                ClaimId = Convert.ToInt64(evaluateableClaim.ClaimId),
                                Npi = Convert.ToString(evaluateableClaim.Npi, CultureInfo.InvariantCulture),
                                ServiceDate = Convert.ToDateTime(evaluateableClaim.StatementFrom, CultureInfo.InvariantCulture),
                                BeneDeductible = BeneDeductible,
                                BloodDeductiblePints = BloodDeductiblePints,
                                AllowTerminatorProvider = AllowTerminatorProvider,
                                AdjustFactor = AdjustFactor,
                                AdjustmentOptions = AdjustmentOptions
                            };

                    }
                    catch (Exception ex)
                    {
                        Log.LogError(string.Format(CultureInfo.InvariantCulture, Constants.MicrodynExceptionLog, evaluateableClaim.ClaimId), string.Empty, ex);
                    }

                }
            }
            return evaluateableClaims;
        }


        /// <summary>
        /// Gets the smart box.
        /// </summary>
        /// <param name="evaluateableClaim">The evaluateable claim.</param>
        /// <returns></returns>
        private static SmartBox GetSmartBox(EvaluateableClaim evaluateableClaim)
        {
            return new SmartBox
            {
                LOS = evaluateableClaim.Los,
                TCC = evaluateableClaim.ClaimTotal
            };
        }

        /// <summary>
        /// Gets the c record.
        /// </summary>
        /// <param name="evaluateableClaim">The evaluateable claim.</param>
        private static void GetCRecord(EvaluateableClaim evaluateableClaim)
        {
            evaluateableClaim.MicrodynApcEditInput.CRecord = new CRecord
            {
                ClaimId = Convert.ToString(evaluateableClaim.ClaimId, CultureInfo.InvariantCulture),
                FromDate = Convert.ToDateTime(evaluateableClaim.StatementFrom, CultureInfo.InvariantCulture),
                ThruDate = Convert.ToDateTime(evaluateableClaim.StatementThru, CultureInfo.InvariantCulture),
                ConditionCodes = evaluateableClaim.ConditionCodes != null ? evaluateableClaim.ConditionCodes.Select(q => q.Code).ToList() : null,
                OccurrenceCodes = evaluateableClaim.OccurrenceCodes != null ? evaluateableClaim.OccurrenceCodes.Select(q => q.Code).ToList() : null,
                BillType = Convert.ToString(evaluateableClaim.BillType, CultureInfo.InvariantCulture),
                Npi = Convert.ToString(evaluateableClaim.Npi, CultureInfo.InvariantCulture),
                OppsFlag = OppFlag, // Hard coded 1
                BeneAmount = BeneAmount, // Hard coded  0.0
                BloodPint = BloodPint, // Hard coded 1,
                PatientData = evaluateableClaim.PatientData
            };
        }

        /// <summary>
        /// Gets all record.
        /// </summary>
        /// <param name="evaluateableClaim">The evaluateable claim.</param>
        private static void GetAllRecord(EvaluateableClaim evaluateableClaim)
        {
            DiagnosisCode principalDiagnosisCode = evaluateableClaim.DiagnosisCodes.FirstOrDefault(q => PrincipalDiagnosisCodes.Contains(q.Instance));
            if (evaluateableClaim.StatementThru >= DateTime.Parse(Constants.IcdVersionDate, CultureInfo.InvariantCulture))
            {
                //Getting ERecord Data
                evaluateableClaim.MicrodynApcEditInput.ERecord = new ERecord
                {
                    ClaimId = Convert.ToString(evaluateableClaim.ClaimId, CultureInfo.InvariantCulture),
                    PrincipalDiagnosisCode = principalDiagnosisCode != null ? principalDiagnosisCode.IcddCode : string.Empty,
                    AdmitDiagnosisCode = principalDiagnosisCode != null ? principalDiagnosisCode.IcddCode : string.Empty,
                    SecondaryDiagnosisCodes = evaluateableClaim.DiagnosisCodes.Where(q => !PrincipalDiagnosisCodes.Contains(q.Instance)).Select(q => q.IcddCode).ToList()
                };
            }
            else
            {
                //Getting DRecord Data
                evaluateableClaim.MicrodynApcEditInput.DRecord = new DRecord
                {
                    ClaimId = Convert.ToString(evaluateableClaim.ClaimId, CultureInfo.InvariantCulture),
                    PrincipalDiagnosisCode = principalDiagnosisCode != null ? principalDiagnosisCode.IcddCode : string.Empty,
                    AdmitDiagnosisCode = principalDiagnosisCode != null ? principalDiagnosisCode.IcddCode : string.Empty,
                    SecondaryDiagnosisCodes = evaluateableClaim.DiagnosisCodes.Where(q => !PrincipalDiagnosisCodes.Contains(q.Instance)).Select(q => q.IcddCode).ToList()
                };
            }

            //Getting LRecord Data
            GetLRecord(evaluateableClaim);
        }

        /// <summary>
        /// Gets the l record.
        /// </summary>
        /// <param name="evaluateableClaim">The evaluateable claim.</param>
        private static void GetLRecord(EvaluateableClaim evaluateableClaim)
        {
            evaluateableClaim.MicrodynApcEditInput.LRecords = new List<LRecord>();
            foreach (ClaimCharge claimcharge in evaluateableClaim.ClaimCharges)
            {
                LRecord lRecord = new LRecord
                {
                    ClaimId = Convert.ToString(evaluateableClaim.ClaimId, CultureInfo.InvariantCulture),
                    LineItemId = claimcharge.Line,
                    HcpcsProcedureCode = claimcharge.HcpcsCode,
                    ServiceDate = Convert.ToDateTime(claimcharge.ServiceFromDate),
                    RevenueCode = Convert.ToString(claimcharge.RevCode, CultureInfo.InvariantCulture),
                    UnitsofService = claimcharge.Units == null ? (int?)null : Convert.ToInt32(claimcharge.Units, CultureInfo.InvariantCulture),
                    LineItemCharge = claimcharge.Amount == null ? (double?)null : Convert.ToDouble(claimcharge.Amount, CultureInfo.InvariantCulture),
                    LineItemFlag = LineItemFlag, // Hardcoded 0
                    HcpcsModifiers = claimcharge.HcpcsModifiers
                };
                evaluateableClaim.MicrodynApcEditInput.LRecords.Add(lRecord);
            }
        }
    }
}
