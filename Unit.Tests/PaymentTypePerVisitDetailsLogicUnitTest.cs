/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Payment Type PerVisit DetailsLogic Testing
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
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for PaymentTypePerVisitDetailsLogicUnitTest
    /// </summary>
    [TestClass]
    public class PaymentTypePerVisitDetailsLogicUnitTest
    {

        /// <summary>
        /// Payments the type per visit constructor unit test1.
        /// </summary>
        [TestMethod]
        public void PaymentTypePerVisitConstructor()
        {
            var target = new PaymentTypePerVisitLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTypePerVisitLogic));
        }
        
        /// <summary>
        /// Payments the type per visit discount constructor unit test2.
        /// </summary>
        [TestMethod]
        public void PaymentTypePerVisitDiscountConstructor2()
        {
            var mockProductRepository = new Mock<IPaymentTypePerVisitRepository>();
            PaymentTypePerVisitLogic target = new PaymentTypePerVisitLogic(mockProductRepository.Object);
            Assert.IsInstanceOfType(target, typeof(PaymentTypePerVisitLogic));
        }

        /// <summary>
        /// Adds the new payment type per visit details test1 for IsNotNull
        /// </summary>
        [TestMethod]
        public void AddNewPaymentTypePerVisitDetailsIfNull()
        {
            var mockProductRepository = new Mock<IPaymentTypePerVisitRepository>();
            mockProductRepository.Setup(f => f.AddEditPaymentTypePerVisitDetails(It.IsAny<PaymentTypePerVisit>())).Returns(0);
            PaymentTypePerVisitLogic target = new PaymentTypePerVisitLogic(mockProductRepository.Object);
            long actual = target.AddEditPaymentType(null);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        /// Adds the new payment type per visit details test1 for IsNotNull
        /// </summary>
        [TestMethod]
        public void EditPaymentTypePerVisitDetailsIfNull()
        {
            var mockProductRepository = new Mock<IPaymentTypePerVisitRepository>();
            mockProductRepository.Setup(f => f.AddEditPaymentTypePerVisitDetails(It.IsAny<PaymentTypePerVisit>())).Returns(0);
            PaymentTypePerVisitLogic target = new PaymentTypePerVisitLogic(mockProductRepository.Object);
            long actual = target.AddEditPaymentType(null);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        /// Adds the payment type Per Visit test2 For AreEqual
        /// </summary>
        [TestMethod]
        public void AddNewPaymentTypePerVisitDetailsIfNotNull()
        {
            var mockProductRepository = new Mock<IPaymentTypePerVisitRepository>();
            mockProductRepository.Setup(f => f.AddEditPaymentTypePerVisitDetails(It.IsAny<PaymentTypePerVisit>())).Returns(2);
            PaymentTypePerVisitLogic target = new PaymentTypePerVisitLogic(mockProductRepository.Object);
            PaymentTypePerVisit objPaymentTypePerVisit = new PaymentTypePerVisit { PaymentTypeDetailId = 1 };
            long actual = target.AddEditPaymentType(objPaymentTypePerVisit);
            // Assert.IsNull(actual);
            Assert.AreEqual(2, actual);
            //  Assert.IsTrue(actual.Equals(null)); 
        }

        /// <summary>
        /// Adds the payment type Per Visit test2 For AreEqual
        /// </summary>
        [TestMethod]
        public void EditNewPaymentTypePerVisitDetailsIfNotNull()
        {
            var mockProductRepository = new Mock<IPaymentTypePerVisitRepository>();
            mockProductRepository.Setup(f => f.AddEditPaymentTypePerVisitDetails(It.IsAny<PaymentTypePerVisit>())).Returns(2);
            PaymentTypePerVisitLogic target = new PaymentTypePerVisitLogic(mockProductRepository.Object);
            PaymentTypePerVisit objPaymentTypePerVisit = new PaymentTypePerVisit { PaymentTypeDetailId = 1 };
            long actual = target.AddEditPaymentType(objPaymentTypePerVisit);
            // Assert.IsNull(actual);
            Assert.AreEqual(2, actual);
        }

        /// <summary>
        /// Get Payment Type Per Visit DetailsMockTest for Null
        /// </summary>
        [TestMethod]
        public void GetPaymentTypePerVisitDetailsMockTestIsNull()
        {
            var mockProductRepository = new Mock<IPaymentTypePerVisitRepository>();
            mockProductRepository.Setup(f => f.GetPaymentTypePerVisitDetails(It.IsAny<PaymentTypePerVisit>())).Returns((PaymentTypePerVisit) null);
            PaymentTypePerVisitLogic target = new PaymentTypePerVisitLogic(mockProductRepository.Object);
            PaymentTypePerVisit actual = (PaymentTypePerVisit)target.GetPaymentType(null);
            Assert.IsNull(actual);
            // Assert.IsNotNull(actual);
        }

        /// <summary>
        /// Get Payment Type Per Visit DetailsMockTest for Not Null
        /// </summary>
        [TestMethod]
        public void GetPaymentTypePerVisitDetailsMockTestIsNotNull()
        {
            PaymentTypePerVisit objPaymentTypeVisit = new PaymentTypePerVisit { PaymentTypeId = 1 };
            var mockProductRepository = new Mock<IPaymentTypePerVisitRepository>();
            mockProductRepository.Setup(f => f.GetPaymentTypePerVisitDetails(It.IsAny<PaymentTypePerVisit>())).Returns(objPaymentTypeVisit);
            PaymentTypePerVisitLogic target = new PaymentTypePerVisitLogic(mockProductRepository.Object);
            PaymentTypePerVisit actual = (PaymentTypePerVisit)target.GetPaymentType(objPaymentTypeVisit);
            Assert.AreEqual(1, actual.PaymentTypeId);
        
        }
        [TestMethod]
        public void EvaluateTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null,ServiceTypeId = 1},
            };
            PaymentTypePerVisit paymentTypePerVisit = new PaymentTypePerVisit
            {
                ServiceTypeId = 1,
                Rate = 10,
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
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
                ContractId = 1,
                HcpcsCode = "300",

            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypePerVisitRepository> paymentTypePerVisitRepository = new Mock<IPaymentTypePerVisitRepository>();
            paymentTypePerVisitRepository.Setup(x => x.GetPaymentTypePerVisitDetails(It.IsAny<PaymentTypePerVisit>()))
                .Returns(paymentTypePerVisit);

            var target = new PaymentTypePerVisitLogic(paymentTypePerVisitRepository.Object)
            {
                PaymentTypeBase = paymentTypePerVisit
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
                    HcpcsCode = "300"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "301"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = 20,
                    HcpcsCode = "302"
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                    HcpcsCode = "303"
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                    HcpcsCode = "304"
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "305"
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
            if (firstOrDefault != null) Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        [TestMethod]
        public void EvaluateTestWhenRateIsNull()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null,ServiceTypeId = 1},
            };
            PaymentTypePerVisit paymentTypePerVisit = new PaymentTypePerVisit
            {
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
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
                ContractId = 1,
                HcpcsCode = "300",

            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypePerVisitRepository> paymentTypePerVisitRepository = new Mock<IPaymentTypePerVisitRepository>();
            paymentTypePerVisitRepository.Setup(x => x.GetPaymentTypePerVisitDetails(It.IsAny<PaymentTypePerVisit>()))
                .Returns(paymentTypePerVisit);

            var target = new PaymentTypePerVisitLogic(paymentTypePerVisitRepository.Object)
            {
                PaymentTypeBase = paymentTypePerVisit
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
                    HcpcsCode = "300"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "301"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = 20,
                    HcpcsCode = "302"
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                    HcpcsCode = "303"
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                    HcpcsCode = "304"
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "305"
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
             target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.IsNull(paymentResults.FirstOrDefault());
        }

        [TestMethod]
        public void EvaluateAtClaimLevelTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
            };
            PaymentTypePerVisit paymentTypePerVisit = new PaymentTypePerVisit
            {
                Rate = 10,
                PayAtClaimLevel = true,
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
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
                ContractId = 1,
                HcpcsCode = "300",
                 
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypePerVisitRepository> paymentTypePerVisitRepository = new Mock<IPaymentTypePerVisitRepository>();
            paymentTypePerVisitRepository.Setup(x => x.GetPaymentTypePerVisitDetails(It.IsAny<PaymentTypePerVisit>()))
                .Returns(paymentTypePerVisit);

            var target = new PaymentTypePerVisitLogic(paymentTypePerVisitRepository.Object)
            {
                PaymentTypeBase = paymentTypePerVisit
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
                    HcpcsCode = "300"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "301"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = 20,
                    HcpcsCode = "302"
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                    HcpcsCode = "303"
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                    HcpcsCode = "304"
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "305"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            if (firstOrDefault != null) Assert.AreEqual(10,firstOrDefault.AdjudicatedValue);
        }


    }
}
