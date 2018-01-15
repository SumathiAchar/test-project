/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Payment Type Percentage Discount Details Logic Testing
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Data.Repository;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for PaymentTypePercentageDiscountDetailsLogicUnitTest
    /// </summary>
    [TestClass]
    public class PaymentTypePercentageDiscountDetailsLogicUnitTest
    {

        /// <summary>
        /// Payments the type percentage discount constructor unit test1.
        /// </summary>
        [TestMethod]
        public void PaymentTypePercentageDiscountConstructorUnitTest1()
        {
            IPaymentTypePercentageChargeRepository productRepository = new PaymentTypePercentageChargeRepository(Constants.ConnectionString);
            PaymentTypePercentageChargeLogic target = new PaymentTypePercentageChargeLogic(productRepository);
            Assert.IsInstanceOfType(target, typeof(PaymentTypePercentageChargeLogic));
        }

        [TestMethod]
        //Testing AuditLog constructor without parameter
        public void PaymentTypePercentageDiscountParameterlessConstructorTest()
        {
            var target = new PaymentTypePercentageChargeLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTypePercentageChargeLogic));
        }
        /// <summary>
        /// Payments the type percentage discount constructor unit test2.
        /// </summary>
        [TestMethod]
        public void PaymentTypePercentageDiscountConstructorUnitTest2()
        {
            var mockAddNewPaymentTypePercentageDiscount = new Mock<IPaymentTypePercentageChargeRepository>();
            PaymentTypePercentageChargeLogic target = new PaymentTypePercentageChargeLogic(mockAddNewPaymentTypePercentageDiscount.Object);
            Assert.IsInstanceOfType(target, typeof(PaymentTypePercentageChargeLogic));
        }

        /// <summary>
        /// Adds the payment type Percentage Discount test For If Null
        /// </summary>
        [TestMethod]
        public void AddNewPaymentTypePercentageDiscountIfNull()
        {
            var mockAddNewPaymentTypePercentageDiscount = new Mock<IPaymentTypePercentageChargeRepository>();
            mockAddNewPaymentTypePercentageDiscount.Setup(f => f.AddEditPaymentTypePercentageDiscountDetails(It.IsAny<PaymentTypePercentageCharge>())).Returns(0);
            PaymentTypePercentageChargeLogic target = new PaymentTypePercentageChargeLogic(mockAddNewPaymentTypePercentageDiscount.Object);
            long actual = target.AddEditPaymentType(null);
            Assert.AreEqual(0, actual);
            
        }

        /// <summary>
        /// Adds the payment type PercentageDiscount test For If Not Null
        /// </summary>
        [TestMethod]
        public void AddNewPaymentTypePercentageDiscountIfNotNull()
        {
            var mockAddNewPaymentTypePercentageDiscount = new Mock<IPaymentTypePercentageChargeRepository>();
            mockAddNewPaymentTypePercentageDiscount.Setup(f => f.AddEditPaymentTypePercentageDiscountDetails(It.IsAny<PaymentTypePercentageCharge>())).Returns(1);
            PaymentTypePercentageChargeLogic target = new PaymentTypePercentageChargeLogic(mockAddNewPaymentTypePercentageDiscount.Object);
            PaymentTypePercentageCharge objPaymentTypeStopLoss = new PaymentTypePercentageCharge { PaymentTypeDetailId = 1 };
            long actual = target.AddEditPaymentType(objPaymentTypeStopLoss);
            Assert.AreEqual(1, actual);
           
        }

        /// <summary>
        /// Get Payment Type Percentage Discount
        /// </summary>
        [TestMethod]
        public void GetPaymentTypePercentageDiscountTestIfNull()
        {
            PaymentTypePercentageCharge result = new PaymentTypePercentageCharge();

            var mockGetPaymentTypePercentageDiscount = new Mock<IPaymentTypePercentageChargeRepository>();
            mockGetPaymentTypePercentageDiscount.Setup(f => f.GetPaymentTypePercentageDiscountDetails(null)).Returns(result);
            PaymentTypePercentageChargeLogic target = new PaymentTypePercentageChargeLogic(mockGetPaymentTypePercentageDiscount.Object);

            PaymentTypePercentageCharge actual = (PaymentTypePercentageCharge)target.GetPaymentType(null);
            Assert.AreEqual(result, actual);
        }
        /// <summary>
        /// Get Payment Type Percentage Discount
        /// </summary>
        [TestMethod]
        public void GetPaymentTypePercentageDiscountTestIfNotNull()
        {
            //Mock Input
            PaymentTypePercentageCharge objPaymentTypePercentageDiscount = new PaymentTypePercentageCharge { PaymentTypeId = 8, ContractId = 345, ServiceTypeId = null};

            //Mock output 
            PaymentTypePercentageCharge result = new PaymentTypePercentageCharge { Percentage = 43.56, PaymentTypeDetailId =234};
            var mockGetPaymentTypePercentageDiscount = new Mock<IPaymentTypePercentageChargeRepository>();
            mockGetPaymentTypePercentageDiscount.Setup(f => f.GetPaymentTypePercentageDiscountDetails(objPaymentTypePercentageDiscount)).Returns(result);
            PaymentTypePercentageChargeLogic target = new PaymentTypePercentageChargeLogic(mockGetPaymentTypePercentageDiscount.Object);

            PaymentTypePercentageCharge actual = (PaymentTypePercentageCharge)target.GetPaymentType(objPaymentTypePercentageDiscount);
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Evaluates at claim level test.
        /// </summary>
        [TestMethod]
        public void EvaluateAtClaimLevelTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
            };
            PaymentTypePercentageCharge paymentTypePercentageCharge = new PaymentTypePercentageCharge
            {
                Percentage = 10,
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypePercentageChargeRepository> paymentTypePercentageChargeRepository = new Mock<IPaymentTypePercentageChargeRepository>();
            paymentTypePercentageChargeRepository.Setup(x => x.GetPaymentTypePercentageDiscountDetails(It.IsAny<PaymentTypePercentageCharge>()))
                .Returns(paymentTypePercentageCharge);

            var target = new PaymentTypePercentageChargeLogic(paymentTypePercentageChargeRepository.Object)
            {
                PaymentTypeBase = paymentTypePercentageCharge
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
            if (firstOrDefault != null) Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        [TestMethod]
        public void EvaluateAtClaimLevelClaimDataErrTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
            };
            PaymentTypePercentageCharge paymentTypePercentageCharge = new PaymentTypePercentageCharge
            {
                Percentage = 10,
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypePercentageChargeRepository> paymentTypePercentageChargeRepository = new Mock<IPaymentTypePercentageChargeRepository>();
            paymentTypePercentageChargeRepository.Setup(x => x.GetPaymentTypePercentageDiscountDetails(It.IsAny<PaymentTypePercentageCharge>()))
                .Returns(paymentTypePercentageCharge);

            var target = new PaymentTypePercentageChargeLogic(paymentTypePercentageChargeRepository.Object)
            {
                PaymentTypeBase = paymentTypePercentageCharge
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
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
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates at line level test.
        /// </summary>
        [TestMethod]
        public void EvaluateAtLineLevelTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
            };
            PaymentTypePercentageCharge paymentTypePercentageCharge = new PaymentTypePercentageCharge
            {
                Percentage = 10,
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypePercentageChargeRepository> paymentTypePercentageChargeRepository = new Mock<IPaymentTypePercentageChargeRepository>();
            paymentTypePercentageChargeRepository.Setup(x => x.GetPaymentTypePercentageDiscountDetails(It.IsAny<PaymentTypePercentageCharge>()))
                .Returns(paymentTypePercentageCharge);

            var target = new PaymentTypePercentageChargeLogic(paymentTypePercentageChargeRepository.Object)
            {
                PaymentTypeBase = paymentTypePercentageCharge
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
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            if (firstOrDefault != null) Assert.AreEqual(2, firstOrDefault.AdjudicatedValue);
        }

        [TestMethod]
        public void EvaluateAtLineLevelAmountIsNullTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
            };
            PaymentTypePercentageCharge paymentTypePercentageCharge = new PaymentTypePercentageCharge
            {
                Percentage = 10,
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypePercentageChargeRepository> paymentTypePercentageChargeRepository = new Mock<IPaymentTypePercentageChargeRepository>();
            paymentTypePercentageChargeRepository.Setup(x => x.GetPaymentTypePercentageDiscountDetails(It.IsAny<PaymentTypePercentageCharge>()))
                .Returns(paymentTypePercentageCharge);

            var target = new PaymentTypePercentageChargeLogic(paymentTypePercentageChargeRepository.Object)
            {
                PaymentTypeBase = paymentTypePercentageCharge
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
                    HcpcsCode = "300"
                },
                new ClaimCharge
                {
                    Line = 2,
                    HcpcsCode = "301"
                },
                new ClaimCharge
                {
                    Line = 3,
                    HcpcsCode = "302"
                },
                new ClaimCharge
                {
                    Line = 4,
                    HcpcsCode = "303"
                },
                new ClaimCharge
                {
                    Line = 5,
                    HcpcsCode = "304"
                },
                new ClaimCharge
                {
                    Line = 6,
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
            Assert.IsNull(firstOrDefault);
        }


    }
}
