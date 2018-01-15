/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Payment Type DRG Schedules Logic Testing
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
    /// Summary description for PaymentTypeDRGPaymentLogicUnitTest
    /// </summary>
    [TestClass]
    public class PaymentTypeDrgPaymentLogicUnitTest
    {

        [TestMethod]
        //Testing AuditLog constructor without parameter
        public void PaymentTypeDrgLogicParameterlessConstructorTest()
        {
            var target = new PaymentTypeDrgLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTypeDrgLogic));
        }

        /// <summary>
        /// Payments the type DRG payment constructor unit test1.
        /// </summary>
        [TestMethod]
        public void PaymentTypeDrgPaymentConstructorUnitTest1()
        {
            IPaymentTypeDrgPaymentRepository productRepository = new PaymentTypeDrgRepository(Constants.ConnectionString);
            PaymentTypeDrgLogic target = new PaymentTypeDrgLogic(productRepository);
            Assert.IsInstanceOfType(target, typeof(PaymentTypeDrgLogic));
        }

        /// <summary>
        /// Payments the type DRG payment constructor unit test2.
        /// </summary>
        [TestMethod]
        public void PaymentTypeDrgPaymentConstructorUnitTest2()
        {
            var mockPaymentTypeDrgPayment = new Mock<IPaymentTypeDrgPaymentRepository>();
            PaymentTypeDrgLogic target = new PaymentTypeDrgLogic(mockPaymentTypeDrgPayment.Object);
            Assert.IsInstanceOfType(target, typeof(PaymentTypeDrgLogic));
        }

        /// <summary>
        /// Adds the new payment type DRG payment difference null.
        /// </summary>
        [TestMethod]
        public void AddNewPaymentTypeDrgPaymentIfNull()
        {
            var mockPaymentTypeDrgPayment = new Mock<IPaymentTypeDrgPaymentRepository>();
            mockPaymentTypeDrgPayment.Setup(f => f.AddEditPaymentTypeDrgPayment(It.IsAny<PaymentTypeDrg>())).Returns(1);
            PaymentTypeDrgLogic target = new PaymentTypeDrgLogic(mockPaymentTypeDrgPayment.Object);
            long actual = target.AddEditPaymentType(null);
            Assert.AreEqual(1, actual);
        }
        /// <summary>
        /// Adds the new payment type DRG payment for not null
        /// </summary>
        [TestMethod]
        public void AddNewPaymentTypeDrgPaymentIfNotNull()
        {
            var mockPaymentTypeDrgPayment = new Mock<IPaymentTypeFeeScheduleRepository>();
            mockPaymentTypeDrgPayment.Setup(f => f.AddEditPaymentTypeFeeSchedule(It.IsAny<PaymentTypeFeeSchedule>())).Returns(1);
            PaymentTypeFeeScheduleLogic target = new PaymentTypeFeeScheduleLogic(mockPaymentTypeDrgPayment.Object);
            PaymentTypeFeeSchedule objAddNewPaymentTypeDrgPayment = new PaymentTypeFeeSchedule { PaymentTypeDetailId = 1 };

            long actual = target.AddEditPaymentType(objAddNewPaymentTypeDrgPayment);
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Get Payment Type DRG Schedules
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeDrgPaymentMockTest1()
        {
            PaymentTypeDrg objAddNewPaymentTypeDrgPayment = new PaymentTypeDrg { PaymentTypeDetailId = 1 };

            var mockPaymentTypeDrgPayment = new Mock<IPaymentTypeDrgPaymentRepository>();
            mockPaymentTypeDrgPayment.Setup(f => f.GetPaymentTypeDrgPayment(It.IsAny<PaymentTypeDrg>())).Returns(objAddNewPaymentTypeDrgPayment);
            PaymentTypeDrgLogic target = new PaymentTypeDrgLogic(mockPaymentTypeDrgPayment.Object);

            PaymentTypeDrg actual = (PaymentTypeDrg)target.GetPaymentType(null);
            Assert.AreEqual(1, actual.PaymentTypeDetailId);
        }

        /// <summary>
        /// Evaluates the test.
        /// </summary>
        [TestMethod]
        public void EvaluateTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null,ServiceTypeId = 1},
            };
            PaymentTypeDrg paymentTypeDrg = new PaymentTypeDrg
            {
                RelativeWeightId = 10,
                BaseRate = 10,
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
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "ABCDE",
                                    Value = "100"
                                }
                    }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                ContractId = 1,

            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeDrgPaymentRepository> paymentTypeDrgPaymentRepository = new Mock<IPaymentTypeDrgPaymentRepository>();
            paymentTypeDrgPaymentRepository.Setup(x => x.GetPaymentTypeDrgPayment(It.IsAny<PaymentTypeDrg>()))
                .Returns(paymentTypeDrg);

            var target = new PaymentTypeDrgLogic(paymentTypeDrgPaymentRepository.Object)
            {
                PaymentTypeBase = paymentTypeDrg
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.Drg = "ABCDE";
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
                    HcpcsCode = "ABCDE"
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
            if (firstOrDefault != null) Assert.AreEqual(1000, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test when DRG is null.
        /// </summary>
        [TestMethod]
        public void EvaluateTestWhenDrgIsNull()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null,ServiceTypeId = 1},
            };
            PaymentTypeDrg paymentTypeDrg = new PaymentTypeDrg
            {
                RelativeWeightId = 10,
                BaseRate = 10,
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
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "ABCDF",
                                    Value = "100"
                                }
}
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                ContractId = 1,

            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeDrgPaymentRepository> paymentTypeDrgPaymentRepository = new Mock<IPaymentTypeDrgPaymentRepository>();
            paymentTypeDrgPaymentRepository.Setup(x => x.GetPaymentTypeDrgPayment(It.IsAny<PaymentTypeDrg>()))
                .Returns(paymentTypeDrg);

            var target = new PaymentTypeDrgLogic(paymentTypeDrgPaymentRepository.Object)
            {
                PaymentTypeBase = paymentTypeDrg
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.Drg = "ABCDE";
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
                    HcpcsCode = "ABCDE"
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
            if (firstOrDefault != null) Assert.AreEqual(0, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test when DRG amount is null.
        /// </summary>
        [TestMethod]
        public void EvaluateTestWhenDrgAmountIsNull()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null,ServiceTypeId = 1},
            };
            PaymentTypeDrg paymentTypeDrg = new PaymentTypeDrg
            {
                RelativeWeightId = 10,
                BaseRate = 10,
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
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "ABCDE",
                                }
}
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                ContractId = 1,

            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeDrgPaymentRepository> paymentTypeDrgPaymentRepository = new Mock<IPaymentTypeDrgPaymentRepository>();
            paymentTypeDrgPaymentRepository.Setup(x => x.GetPaymentTypeDrgPayment(It.IsAny<PaymentTypeDrg>()))
                .Returns(paymentTypeDrg);

            var target = new PaymentTypeDrgLogic(paymentTypeDrgPaymentRepository.Object)
            {
                PaymentTypeBase = paymentTypeDrg
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.Drg = "ABCDE";
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
                    HcpcsCode = "ABCDE"
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
            if (firstOrDefault != null)
            {
                Assert.AreEqual(0.0, firstOrDefault.AdjudicatedValue);
                Assert.AreEqual(3, firstOrDefault.ClaimStatus);
            }
        }

        /// <summary>
        /// Gets all relative weight list test.
        /// </summary>
        [TestMethod]
        public void GetAllRelativeWeightListTest()
        {
            var mockPaymentTypeDrgPayment = new Mock<IPaymentTypeDrgPaymentRepository>();
            List<RelativeWeight> relativeWeightList = new List<RelativeWeight>
            {
                new RelativeWeight
                {
                    RelativeWeightId = 1,
                    RelativeWeightValue = "100"
                }
            };
            mockPaymentTypeDrgPayment.Setup(f => f.GetAllRelativeWeightList()).Returns(relativeWeightList);
            PaymentTypeDrgLogic target = new PaymentTypeDrgLogic(mockPaymentTypeDrgPayment.Object);
            var actual = target.GetAllRelativeWeightList();
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
        }

        /// <summary>
        /// Evaluates if payment results line is not null test.
        /// </summary>
        [TestMethod]
        public void EvaluateIfPaymentResultsLineIsNotNullTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = null,AdjudicatedValue = null,ServiceTypeId = 1},
            };
            PaymentTypeDrg paymentTypeDrg = new PaymentTypeDrg
            {
                RelativeWeightId = 10,
                BaseRate = 10,
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
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "ABCDE",
                                    Value = "100"
                                }
                    }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                ContractId = 1,

            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeDrgPaymentRepository> paymentTypeDrgPaymentRepository = new Mock<IPaymentTypeDrgPaymentRepository>();
            paymentTypeDrgPaymentRepository.Setup(x => x.GetPaymentTypeDrgPayment(It.IsAny<PaymentTypeDrg>()))
                .Returns(paymentTypeDrg);

            var target = new PaymentTypeDrgLogic(paymentTypeDrgPaymentRepository.Object)
            {
                PaymentTypeBase = paymentTypeDrg
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.Drg = "ABCDE";
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
                    HcpcsCode = "ABCDE"
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
            if (firstOrDefault != null) Assert.AreEqual(1000, firstOrDefault.AdjudicatedValue);
        }
    }
}
