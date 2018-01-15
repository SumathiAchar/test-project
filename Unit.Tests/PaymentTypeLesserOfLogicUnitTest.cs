/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Payment Type Lesser Of Logic Testing
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;
using SSI.ContractManagement.Shared.Helpers;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for PaymentTypeLesserOfLogicUnitTest
    /// </summary>
    [TestClass]
    public class PaymentTypeLesserOfLogicUnitTest
    {
        //Creating object for Logic
        private PaymentTypeLesserOfLogic _target;

        /// <summary>
        /// Payments the type lesser of logic logic parameterless constructor test.
        /// </summary>
        /// //Testing AuditLog constructor without parameter
        [TestMethod]
        public void PaymentTypeLesserOfLogicLogicParameterlessConstructorTest()
        {
            _target = new PaymentTypeLesserOfLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(_target, typeof(PaymentTypeLesserOfLogic));
        }
        
        /// <summary>
        /// Payments the type lesser of repository constructor unit test2.
        /// </summary>
        [TestMethod]
        public void PaymentTypeLesserOfRepositoryConstructorUnitTest1()
        {
            var mockPaymentTypeLesserOf = new Mock<IPaymentTypeLesserOfRepository>();
            _target = new PaymentTypeLesserOfLogic(mockPaymentTypeLesserOf.Object);
            Assert.IsInstanceOfType(_target, typeof(PaymentTypeLesserOfLogic));
        }


        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// Adds the payment type lessor of if not null.
        /// </summary>
        [TestMethod]
        public void AddPaymentTypeLessorOfIfNotNull()
        {
            var mockAddPaymentTypeLessorOf = new Mock<IPaymentTypeLesserOfRepository>();
            mockAddPaymentTypeLessorOf.Setup(f => f.AddEditPaymentTypeLesserOf(It.IsAny<PaymentTypeLesserOf>())).Returns(1);
            _target = new PaymentTypeLesserOfLogic(mockAddPaymentTypeLessorOf.Object);
            PaymentTypeLesserOf objAddPaymentTypeLessorOf = new PaymentTypeLesserOf { ContractId = 1, Percentage = 50};

            _target.AddEditPaymentType(objAddPaymentTypeLessorOf);
            Assert.IsNotNull(true);
        }


        /// <summary>
        /// Adds the new payment type fee schedule if null.
        /// </summary>
        [TestMethod]
        public void AddNewPaymentTypeFeeScheduleIfNull()
        {
            var mockAddPaymentTypeLessorOf = new Mock<IPaymentTypeLesserOfRepository>();
            mockAddPaymentTypeLessorOf.Setup(f => f.AddEditPaymentTypeLesserOf(It.IsAny<PaymentTypeLesserOf>())).Returns(1);
            _target = new PaymentTypeLesserOfLogic(mockAddPaymentTypeLessorOf.Object);
            long actual = _target.AddEditPaymentType(null);
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Gets the lesser of percentage.
        /// </summary>
        [TestMethod]
        public void GetLesserOfPercentage()
        {
            Mock<IPaymentTypeLesserOfRepository> paymentTypeLesserOfRepository = new Mock<IPaymentTypeLesserOfRepository>();
            paymentTypeLesserOfRepository.Setup(x => x.GetLesserOfPercentage(It.IsAny<PaymentTypeLesserOf>()))
                .Returns(new PaymentTypeLesserOf());
            new PaymentTypeLesserOfLogic(paymentTypeLesserOfRepository.Object).GetPaymentType(new PaymentTypeLesserOf());
            paymentTypeLesserOfRepository.VerifyAll();
        }

        /// <summary>
        /// Evaluates the lesser of test.
        /// </summary>
        [TestMethod]
        public void EvaluateLesserOfTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,AdjudicatedValue = 110,ClaimTotalCharges = 110,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = 110,ClaimTotalCharges = 110,ServiceTypeId = 1}
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeLesserOfRepository> paymentTypeLesserOfRepository = new Mock<IPaymentTypeLesserOfRepository>();
            paymentTypeLesserOfRepository.Setup(x => x.GetLesserOfPercentage(It.IsAny<PaymentTypeLesserOf>()))
                .Returns(new PaymentTypeLesserOf());

            PaymentTypeLesserOf paymentTypeLesserOf = new PaymentTypeLesserOf
            {
                
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"300"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "300"
                    }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                ContractId = 1,
                IsLesserOf = true,
                Percentage = 50
            };


            _target = new PaymentTypeLesserOfLogic(paymentTypeLesserOfRepository.Object)
         {
             PaymentTypeBase = paymentTypeLesserOf
         };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110,ClaimTotalCharges = 110,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110,ClaimTotalCharges=110,ServiceTypeId = 1}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(55, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the greater of test.
        /// </summary>
        [TestMethod]
        public void EvaluateGreaterOfTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,AdjudicatedValue = 110,ClaimTotalCharges = 110,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = 110,ClaimTotalCharges = 110,ServiceTypeId = 1}
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeLesserOfRepository> paymentTypeLesserOfRepository = new Mock<IPaymentTypeLesserOfRepository>();
            paymentTypeLesserOfRepository.Setup(x => x.GetLesserOfPercentage(It.IsAny<PaymentTypeLesserOf>()))
                .Returns(new PaymentTypeLesserOf());

            PaymentTypeLesserOf paymentTypeLesserOf = new PaymentTypeLesserOf
            {

                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"300"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "300"
                    }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                ContractId = 1,
                IsLesserOf = false,
                Percentage = 50
            };
            _target = new PaymentTypeLesserOfLogic(paymentTypeLesserOfRepository.Object)
            {
                PaymentTypeBase = paymentTypeLesserOf
            };

            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110,ServiceTypeId = 1}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(220, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the lesser of test.
        /// </summary>
        [TestMethod]
        public void EvaluateLesserOfTestForApplyContractFilter()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,AdjudicatedValue = 110,ClaimTotalCharges = 110,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = 110,ClaimTotalCharges = 110,ServiceTypeId = 1}
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeLesserOfRepository> paymentTypeLesserOfRepository = new Mock<IPaymentTypeLesserOfRepository>();
            paymentTypeLesserOfRepository.Setup(x => x.GetLesserOfPercentage(It.IsAny<PaymentTypeLesserOf>()))
                .Returns(new PaymentTypeLesserOf());

            PaymentTypeLesserOf paymentTypeLesserOf = new PaymentTypeLesserOf
            {

                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"300"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "300"
                    }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                ContractId = 1,
                IsLesserOf = true,
                Percentage = 50
            };


            _target = new PaymentTypeLesserOfLogic(paymentTypeLesserOfRepository.Object)
            {
                PaymentTypeBase = paymentTypeLesserOf
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110,ClaimTotalCharges = 110,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110,ClaimTotalCharges=110,ServiceTypeId = 1}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, true);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(50, firstOrDefault.AdjudicatedValue);
            }
    }
}
