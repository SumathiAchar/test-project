/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Payment Type PerDiem Details Logic Testing
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for PaymentTypePerDiemDetailsLogicUnitTest
    /// </summary>
    [TestClass]
    public class PaymentTypePerDiemDetailsLogicUnitTest
    {
       
        /// <summary>
        /// Payments the type per diem discount constructor unit test1.
        /// </summary>
        [TestMethod]
        public void PaymentTypePerDiemDiscountParameterConstructorTest()
        {
            var mockProductRepository = new Mock<IPaymentTypePerDiemRepository>();
            var target = new PaymentTypePerDiemLogic(mockProductRepository.Object);
            Assert.IsInstanceOfType(target, typeof(PaymentTypePerDiemLogic));
        }

        [TestMethod]
        //Testing AuditLog constructor without parameter
        public void PaymentTypePerDiemDiscountParameterlessConstructorTest()
        {
            var target = new PaymentTypePerDiemLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTypePerDiemLogic));
        }
        /// <summary>
        /// Add Payment Type PerDiem Details IfNull
        /// </summary>
        [TestMethod]
        public void AddPaymentTypePerDiemDetailsIfNull()
        {
            var mockPaymentTypePerDiemDetails = new Mock<IPaymentTypePerDiemRepository>();
            mockPaymentTypePerDiemDetails.Setup(f => f.AddEditPaymentTypePerDiemDetails(It.IsAny<PaymentTypePerDiem>())).Returns(0);
            PaymentTypePerDiemLogic target = new PaymentTypePerDiemLogic(mockPaymentTypePerDiemDetails.Object);

            var paymentTypeDiemList = new PaymentTypePerDiem();
            long actual = target.AddEditPaymentType(paymentTypeDiemList);
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        /// Add Payment Type PerDiem Details If not Null
        /// </summary>
        [TestMethod]
        public void AddPaymentTypePerDiemDetailsIfNotNull()
        {
            var mockPaymentTypePerDiemDetails = new Mock<IPaymentTypePerDiemRepository>();
            mockPaymentTypePerDiemDetails.Setup(f => f.AddEditPaymentTypePerDiemDetails(It.IsAny<PaymentTypePerDiem>())).Returns(1);
            PaymentTypePerDiemLogic target = new PaymentTypePerDiemLogic(mockPaymentTypePerDiemDetails.Object);
            PaymentTypePerDiem objPaymentTypePerVisit = new PaymentTypePerDiem { PaymentTypeDetailId = 1 };
            long actual = target.AddEditPaymentType(objPaymentTypePerVisit);
            Assert.AreEqual(1, actual);

        }

        /// <summary>
        /// Get Payment Type PerDiem Details IfNull
        /// </summary>
        [TestMethod]
        public void GetPaymentTypePerDiemIfNull()
        {
            var mockPaymentTypePerDiemDetails = new Mock<IPaymentTypePerDiemRepository>();
            mockPaymentTypePerDiemDetails.Setup(f => f.GetPaymentTypePerDiem(It.IsAny<PaymentTypePerDiem>())).Returns((PaymentTypePerDiem)null);
            PaymentTypePerDiemLogic target = new PaymentTypePerDiemLogic(mockPaymentTypePerDiemDetails.Object);
            PaymentTypePerDiem actual = (PaymentTypePerDiem)target.GetPaymentType(null);
            Assert.AreEqual(null, actual);
        }

        /// <summary>
        /// Add Payment Type PerDiem Details If not Null
        /// </summary>
        [TestMethod]
        public void GetPaymentTypePerDiemIfNotNull()
        {
            var mockPaymentTypePerDiemDetails = new Mock<IPaymentTypePerDiemRepository>();
            //Mock Input
            PaymentTypePerDiem inputData = new PaymentTypePerDiem { PaymentTypeId = 7, ContractId = 1432, ServiceTypeId = 0 };
            //Mock output 
            PaymentTypePerDiem result = new PaymentTypePerDiem { PerDiemSelections = new List<PerDiemSelection> { new PerDiemSelection { DaysFrom = 1, DaysTo = 5, Rate = 54.34 } } };

            mockPaymentTypePerDiemDetails.Setup(f => f.GetPaymentTypePerDiem(inputData)).Returns(result);
            PaymentTypePerDiemLogic target = new PaymentTypePerDiemLogic(mockPaymentTypePerDiemDetails.Object);
            PaymentTypePerDiem actual = (PaymentTypePerDiem)target.GetPaymentType(inputData);
            Assert.AreEqual(result, actual);

        }

        [TestMethod]
        public void EvaluateTestAtLineLevel()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1,Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypePerDiem paymentTypePerDiem = new PaymentTypePerDiem
            {
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"ABCDE"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "ABCDE"
                    }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                PerDiemSelections = new List<PerDiemSelection>
                {
                    new PerDiemSelection
                    {
                        DaysFrom = 1,
                        DaysTo = 50,
                        Rate = 10
                    }
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypePerDiemRepository> paymentTypePerDiemRepository = new Mock<IPaymentTypePerDiemRepository>();
            paymentTypePerDiemRepository.Setup(x => x.GetPaymentTypePerDiem(It.IsAny<PaymentTypePerDiem>()))
                .Returns(paymentTypePerDiem);

            var target = new PaymentTypePerDiemLogic(paymentTypePerDiemRepository.Object)
            {
                PaymentTypeBase = paymentTypePerDiem
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250",
                    HcpcsCode = "ABCDE",
                    Units = 2
                },
                 new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "ABCDF",
                    Units = 2
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            if (firstOrDefault != null) Assert.AreEqual(20,firstOrDefault.AdjudicatedValue);
        }

        [TestMethod]
        public void EvaluateTestAtClaimLevel()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1,Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypePerDiem paymentTypePerDiem = new PaymentTypePerDiem
            {
                PayAtClaimLevel = true,
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"ABCDE"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "ABCDE"
                    }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                PerDiemSelections = new List<PerDiemSelection>
                {
                    new PerDiemSelection
                    {
                        DaysFrom = 1,
                        DaysTo = 50,
                        Rate = 10
                    }
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypePerDiemRepository> paymentTypePerDiemRepository = new Mock<IPaymentTypePerDiemRepository>();
            paymentTypePerDiemRepository.Setup(x => x.GetPaymentTypePerDiem(It.IsAny<PaymentTypePerDiem>()))
                .Returns(paymentTypePerDiem);

            var target = new PaymentTypePerDiemLogic(paymentTypePerDiemRepository.Object)
            {
                PaymentTypeBase = paymentTypePerDiem
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.StatementFrom = DateTime.Now.AddDays(-3);
            evaluateableClaim.StatementThru = DateTime.Now;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250",
                    HcpcsCode = "ABCDE",
                    Units = 2
                },
                 new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "ABCDF",
                    Units = 2
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            if (firstOrDefault != null) Assert.AreEqual(30, firstOrDefault.AdjudicatedValue);
        }

        [TestMethod]
        public void EvaluateTestWhenSelectionIsEmpty()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1,Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypePerDiem paymentTypePerDiem = new PaymentTypePerDiem
            {
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"ABCDE"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "ABCDE"
                    }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypePerDiemRepository> paymentTypePerDiemRepository = new Mock<IPaymentTypePerDiemRepository>();
            paymentTypePerDiemRepository.Setup(x => x.GetPaymentTypePerDiem(It.IsAny<PaymentTypePerDiem>()))
                .Returns(paymentTypePerDiem);

            var target = new PaymentTypePerDiemLogic(paymentTypePerDiemRepository.Object)
            {
                PaymentTypeBase = paymentTypePerDiem
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250",
                    HcpcsCode = "ABCDE",
                    Units = 2
                },
                 new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "ABCDF",
                    Units = 2
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

    }
}
