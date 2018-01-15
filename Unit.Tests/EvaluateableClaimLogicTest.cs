using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class EvaluateableClaimLogicTest
    {
        //Creating object for Logic
        private EvaluateableClaimLogic _target;

        [TestMethod]
        public void EvaluateIfClaimHasNoPayerTest()
        {
            // Arrange
            _target = new EvaluateableClaimLogic();
            IEvaluateableClaim claim = new EvaluateableClaim();
            claim.ClaimId = 123;
            List<PaymentResult> paymentResults = new List<PaymentResult>();
            PaymentResult paymentResult = new PaymentResult {ClaimId = 123};
            paymentResults.Add(paymentResult);
            PaymentResult paymentResultForLine = new PaymentResult {ClaimId = 123, Line = 1};
            paymentResults.Add(paymentResultForLine);

            //Act
            List<PaymentResult> actual = _target.Evaluate(claim, paymentResults, false,false);

            // Assert
            PaymentResult overAllClaimPaymentResult = actual.FirstOrDefault(
               payment => payment.Line == null && payment.ServiceTypeId == null);
            if (overAllClaimPaymentResult != null)
                Assert.AreEqual(overAllClaimPaymentResult.ClaimStatus, (byte)Enums.AdjudicationOrVarianceStatuses.ClaimDataError);
        }

        [TestMethod]
        public void EvaluateIfClaimHasNoLineTest()
        {
            // Arrange
            _target = new EvaluateableClaimLogic();
            IEvaluateableClaim claim = new EvaluateableClaim();
            claim.ClaimId = 123;
            claim.PriPayerName = "Blue Cross";
            claim.BillType = "141";
            claim.ClaimState = "Billed";
            claim.ClaimType = "hosp";
            claim.StatementFrom= DateTime.Now;
            claim.StatementThru = DateTime.Now;
            claim.ClaimTotal = 200;
            List<PaymentResult> paymentResults = new List<PaymentResult>();
            PaymentResult paymentResult = new PaymentResult {ClaimId = 123};
            paymentResults.Add(paymentResult);
            PaymentResult paymentResultForLine = new PaymentResult {ClaimId = 123, Line = 1};
            paymentResults.Add(paymentResultForLine);

            // Act
            List<PaymentResult> actual = _target.Evaluate(claim, paymentResults, false,false);

            // Assert
            PaymentResult overAllClaimPaymentResult = actual.FirstOrDefault(
               payment => payment.Line == null && payment.ServiceTypeId == null);
            if (overAllClaimPaymentResult != null)
                Assert.AreEqual(overAllClaimPaymentResult.ClaimStatus, (byte)Enums.AdjudicationOrVarianceStatuses.AdjudicationErrorMissingServiceLine);
        }


        [TestMethod]
        public void IsMatchTest()
        {
            // Arrange
            _target = new EvaluateableClaimLogic();
            IEvaluateableClaim claim = new EvaluateableClaim();
            Exception expectedExcetpion = null;

            // Act
            try
            {
                _target.IsMatch(claim);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
        }

        [TestMethod]
        public void UpdateEvaluateableClaimsTest()
        {
            // Arrange
            _target = new EvaluateableClaimLogic();

            ConditionCode conditionCode = new ConditionCode { ClaimId = 123, Code = "0300" };
            List<ConditionCode> conditionCodes = new List<ConditionCode> { conditionCode };

            PatientData patientData = new PatientData { FirstName = "Jim" };

            DiagnosisCode diagnosisCode = new DiagnosisCode { Instance = "P", ClaimId = 123, IcddCode = "0800" };
            List<DiagnosisCode> diagnosisCodes = new List<DiagnosisCode> { diagnosisCode };

            ClaimCharge claimCharge = new ClaimCharge
            {
                Line = 1,
                RevCode = "0123",
                Amount = 5556.2,
            };

            // Act
            List<ClaimCharge> claimCharges = new List<ClaimCharge> { claimCharge };

            EvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                ClaimId = 123,
                StatementThru = Convert.ToDateTime("2012-01-12 00:00:00.000"),
                StatementFrom = Convert.ToDateTime("2012-01-12 00:00:00.000"),
                ConditionCodes = conditionCodes,
                PatientData = patientData,
                DiagnosisCodes = diagnosisCodes,
                ClaimCharges = claimCharges

            };

            List<EvaluateableClaim> evaluateableClaimLists = new List<EvaluateableClaim> { evaluateableClaim };

            List<EvaluateableClaim> actual = _target.UpdateEvaluateableClaims(evaluateableClaimLists);

            // Assert
            EvaluateableClaim updatedEvaluateableClaim = actual.FirstOrDefault();
            if (updatedEvaluateableClaim != null)
            {
                CRecord cRecord = updatedEvaluateableClaim.MicrodynApcEditInput.CRecord;
                if (cRecord != null)
                {
                    Assert.AreEqual(cRecord.PatientData, evaluateableClaim.PatientData);
                    Assert.AreNotEqual(cRecord.ClaimId, evaluateableClaim.ClaimId);
                }

                DRecord dRecord = updatedEvaluateableClaim.MicrodynApcEditInput.DRecord;
                if (dRecord != null)
                {
                    Assert.AreEqual(dRecord.PrincipalDiagnosisCode,evaluateableClaim.DiagnosisCodes.First().IcddCode);
                }

                LRecord lRecord = updatedEvaluateableClaim.MicrodynApcEditInput.LRecords.FirstOrDefault();
                if (lRecord != null)
                {
                    Assert.AreEqual(lRecord.RevenueCode,evaluateableClaim.ClaimCharges.First().RevCode);
                    Assert.AreEqual(lRecord.LineItemId,evaluateableClaim.ClaimCharges.First().Line);
                }

                MedicareOutPatient medicareOutPatient =
                    updatedEvaluateableClaim.MicrodynApcEditInput.MedicareOutPatientRecord;
                if (medicareOutPatient != null)
                {
                    Assert.AreEqual(medicareOutPatient.ClaimId,evaluateableClaim.ClaimId);
                    Assert.AreEqual(medicareOutPatient.BeneDeductible,0);
                }
            }
        }
    }
}
