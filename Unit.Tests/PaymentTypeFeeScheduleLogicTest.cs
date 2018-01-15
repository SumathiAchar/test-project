using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class PaymentTypeFeeScheduleLogicTest
    {

        [TestMethod]
        //Testing AuditLog constructor without parameter
        public void PaymentTypeFeeScheduleLogicParameterlessConstructorTest()
        {
            var target = new PaymentTypeFeeScheduleLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTypeFeeScheduleLogic));
        }

        /// <summary>
        /// Payments the type fee schedule constructor test1.
        /// </summary>
        [TestMethod]
        public void PaymentTypeFeeScheduleConstructorTest1()
        {
            var mockAddEditFeeScheduleDocs = new Mock<IPaymentTypeFeeScheduleRepository>();
            PaymentTypeFeeScheduleLogic target = new PaymentTypeFeeScheduleLogic(mockAddEditFeeScheduleDocs.Object);
            Assert.IsInstanceOfType(target, typeof(PaymentTypeFeeScheduleLogic));
        }

        /// <summary>
        /// Edits the payment type fee schedule case mock test1.
        /// </summary>
        [TestMethod]
        public void EditPaymentTypeFeeScheduleCaseMockTest()
        {
            var mockAddEditFeeScheduleDocs = new Mock<IPaymentTypeFeeScheduleRepository>();
            mockAddEditFeeScheduleDocs.Setup(f => f.AddEditPaymentTypeFeeSchedule(It.IsAny<PaymentTypeFeeSchedule>())).Returns(1);
            PaymentTypeFeeScheduleLogic target = new PaymentTypeFeeScheduleLogic(mockAddEditFeeScheduleDocs.Object);
            PaymentTypeFeeSchedule objAddNewPaymentTypeFeeSchedule = new PaymentTypeFeeSchedule { PaymentTypeDetailId = 1 };

            long actual = target.AddEditPaymentType(objAddNewPaymentTypeFeeSchedule);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// Adds the new payment type fee schedule for null
        /// </summary>
        [TestMethod]
        public void AddNewPaymentTypeFeeScheduleIfNull()
        {
            var mockAddEditFeeScheduleDocs = new Mock<IPaymentTypeFeeScheduleRepository>();
            mockAddEditFeeScheduleDocs.Setup(f => f.AddEditPaymentTypeFeeSchedule(It.IsAny<PaymentTypeFeeSchedule>())).Returns(1);
            PaymentTypeFeeScheduleLogic target = new PaymentTypeFeeScheduleLogic(mockAddEditFeeScheduleDocs.Object);
            long actual = target.AddEditPaymentType(null);
            Assert.AreEqual(1, actual);
        }
        /// <summary>
        ///  Add New Payment Type Fee Schedule If NotNull
        /// </summary>
        [TestMethod]
        public void AddNewPaymentTypeFeeScheduleIfNotNull()
        {
            var mockAddEditFeeScheduleDocs = new Mock<IPaymentTypeFeeScheduleRepository>();
            mockAddEditFeeScheduleDocs.Setup(f => f.AddEditPaymentTypeFeeSchedule(It.IsAny<PaymentTypeFeeSchedule>())).Returns(1);
            PaymentTypeFeeScheduleLogic target = new PaymentTypeFeeScheduleLogic(mockAddEditFeeScheduleDocs.Object);
            PaymentTypeFeeSchedule objAddEditFeeScheduleDocs = new PaymentTypeFeeSchedule { PaymentTypeDetailId = 1 };

            long actual = target.AddEditPaymentType(objAddEditFeeScheduleDocs);
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Get Payment Type Fee Schedule
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeFeeScheduleMockTest()
        {
            PaymentTypeFeeSchedule objGetPaymentTypeFeeSchedule = new PaymentTypeFeeSchedule { FeeSchedule = 1 };

            var mockGetPaymentTypeFeeSchedule = new Mock<IPaymentTypeFeeScheduleRepository>();
            mockGetPaymentTypeFeeSchedule.Setup(f => f.GetPaymentTypeFeeSchedule(It.IsAny<PaymentTypeFeeSchedule>()))
                .Returns(objGetPaymentTypeFeeSchedule);
            PaymentTypeFeeScheduleLogic target = new PaymentTypeFeeScheduleLogic(mockGetPaymentTypeFeeSchedule.Object);

            PaymentTypeFeeSchedule actual = (PaymentTypeFeeSchedule)target.GetPaymentType(null);
            Assert.AreEqual(1, actual.FeeSchedule);
            // Assert.IsNotNull(actual);
        }

        /// <summary>
        /// Evaluates the payment type test.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ServiceTypeId = 1, Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 1}
            };
            PaymentTypeFeeSchedule paymentTypeFeeSchedule = new PaymentTypeFeeSchedule
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
                ValidLineIds = new List<int> {1, 2},
                HcpcsCode = "ABCDE",
                NonFeeSchedule = 50,
                FeeSchedule = 75
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeFeeScheduleRepository> paymentTypeFeeScheduleRepository =
                new Mock<IPaymentTypeFeeScheduleRepository>();
            paymentTypeFeeScheduleRepository.Setup(x => x.GetPaymentTypeFeeSchedule(It.IsAny<PaymentTypeFeeSchedule>()))
                .Returns(paymentTypeFeeSchedule);

            var target = new PaymentTypeFeeScheduleLogic(paymentTypeFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeFeeSchedule
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
                    HcpcsCodeWithModifier ="ABCDE", 
                    Units = 2
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "ABCDF",
                     HcpcsCodeWithModifier ="ABCDF",
                    Units = 2
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
                Assert.AreEqual(100, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the payment type with HCPCS include modifiers test.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeWithHcpcsIncludeModifiersTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ServiceTypeId = 1, Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 1}
            };
            PaymentTypeFeeSchedule paymentTypeFeeSchedule = new PaymentTypeFeeSchedule
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
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                        {
                            new ClaimFieldValue
                            {
                                Identifier = "3000126",
                                Value = "100"
                            },
                            new ClaimFieldValue
                            {
                                Identifier = "3010126",
                                Value = "100"
                            }
                        }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                NonFeeSchedule = 50,
                FeeSchedule = 75
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeFeeScheduleRepository> paymentTypeFeeScheduleRepository =
                new Mock<IPaymentTypeFeeScheduleRepository>();
            paymentTypeFeeScheduleRepository.Setup(x => x.GetPaymentTypeFeeSchedule(It.IsAny<PaymentTypeFeeSchedule>()))
                .Returns(paymentTypeFeeSchedule);

            var target = new PaymentTypeFeeScheduleLogic(paymentTypeFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeFeeSchedule
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
                    HcpcsCode = "30001",
                    Units = 2,
                    HcpcsCodeWithModifier = "3010126",
                    HcpcsModifiers = "26"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "30101",
                    Units = 2,
                    HcpcsCodeWithModifier = "3010126",
                    HcpcsModifiers = "26"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
                Assert.AreEqual(75, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the payment type with HCPCS without include modifiers test.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeWithHcpcsWithoutIncludeModifiersTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ServiceTypeId = 1, Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 1}
            };
            PaymentTypeFeeSchedule paymentTypeFeeSchedule = new PaymentTypeFeeSchedule
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
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                        {
                            new ClaimFieldValue
                            {
                                Identifier = "3000126",
                                Value = "100"
                            },
                            new ClaimFieldValue
                            {
                                Identifier = "3010126",
                                Value = "100"
                            }
                        }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                NonFeeSchedule = 50,
                FeeSchedule = 75
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeFeeScheduleRepository> paymentTypeFeeScheduleRepository =
                new Mock<IPaymentTypeFeeScheduleRepository>();
            paymentTypeFeeScheduleRepository.Setup(x => x.GetPaymentTypeFeeSchedule(It.IsAny<PaymentTypeFeeSchedule>()))
                .Returns(paymentTypeFeeSchedule);

            var target = new PaymentTypeFeeScheduleLogic(paymentTypeFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeFeeSchedule
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
                    HcpcsCode = "30001",
                    Units = 2,
                    HcpcsCodeWithModifier = "3010126",
                    HcpcsModifiers = "261"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "30101",
                    Units = 2,
                    HcpcsCodeWithModifier = "3010126",
                    HcpcsModifiers = "26"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
                Assert.AreEqual(75, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the payment type with HCPCS without include modifiers returns false test.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeWithHcpcsWithoutIncludeModifiersReturnsFalseTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ServiceTypeId = 1, Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 1}
            };
            PaymentTypeFeeSchedule paymentTypeFeeSchedule = new PaymentTypeFeeSchedule
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
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                        {
                            new ClaimFieldValue
                            {
                                Identifier = "3000126",
                                Value = "100"
                            },
                            new ClaimFieldValue
                            {
                                Identifier = "3010126",
                                Value = "100"
                            }
                        }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                NonFeeSchedule = 50,
                FeeSchedule = 75
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeFeeScheduleRepository> paymentTypeFeeScheduleRepository =
                new Mock<IPaymentTypeFeeScheduleRepository>();
            paymentTypeFeeScheduleRepository.Setup(x => x.GetPaymentTypeFeeSchedule(It.IsAny<PaymentTypeFeeSchedule>()))
                .Returns(paymentTypeFeeSchedule);

            var target = new PaymentTypeFeeScheduleLogic(paymentTypeFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeFeeSchedule
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
                    HcpcsCode = "40001",
                    Units = 2,
                    HcpcsCodeWithModifier = "4010126",
                    HcpcsModifiers = "261"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "40101",
                    Units = 2,
                    HcpcsCodeWithModifier = "4010126",
                    HcpcsModifiers = "26"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
                Assert.AreEqual(100, firstOrDefault.AdjudicatedValue);
        }


        /// <summary>
        /// Evaluates the payment fee schedule null test.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentFeeScheduleNullTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1,Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypeFeeSchedule paymentTypeFeeSchedule = new PaymentTypeFeeSchedule
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
               // NonFeeSchedule = 50,
               // FeeSchedule = 75
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeFeeScheduleRepository> paymentTypeFeeScheduleRepository = new Mock<IPaymentTypeFeeScheduleRepository>();
            paymentTypeFeeScheduleRepository.Setup(x => x.GetPaymentTypeFeeSchedule(It.IsAny<PaymentTypeFeeSchedule>()))
                .Returns(paymentTypeFeeSchedule);

            var target = new PaymentTypeFeeScheduleLogic(paymentTypeFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeFeeSchedule
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
                    HcpcsCodeWithModifier = "ABCDE",
                    Units = 2
                },
                 new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "ABCDF",
                    HcpcsCodeWithModifier = "ABCDF",
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

        /// <summary>
        /// Evaluates the payment type with HCPCS with ObervervedServiceUnit true test.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeWithHcpcsWithIsObervervedServiceUnit()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ServiceTypeId = 1, Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 1}
            };
            PaymentTypeFeeSchedule paymentTypeFeeSchedule = new PaymentTypeFeeSchedule
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
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                        {
                            new ClaimFieldValue
                            {
                                Identifier = "40101",
                                Value = "100"
                            },
                            new ClaimFieldValue
                            {
                                Identifier = "3010126",
                                Value = "100"
                            }
                        }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                NonFeeSchedule = 50,
                FeeSchedule = 75,
                IsObserveUnits = true
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeFeeScheduleRepository> paymentTypeFeeScheduleRepository =
                new Mock<IPaymentTypeFeeScheduleRepository>();
            paymentTypeFeeScheduleRepository.Setup(x => x.GetPaymentTypeFeeSchedule(It.IsAny<PaymentTypeFeeSchedule>()))
                .Returns(paymentTypeFeeSchedule);

            var target = new PaymentTypeFeeScheduleLogic(paymentTypeFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeFeeSchedule
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
                    HcpcsCode = "3000126",
                    Units = 2,
                    HcpcsCodeWithModifier = "3000126",
                    HcpcsModifiers = "261"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "40101",
                    Units = 2,
                    HcpcsCodeWithModifier = "4010126",
                    HcpcsModifiers = "26"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(150, actual[0].AdjudicatedValue);
            Assert.AreEqual(2, actual.Count);
           }

        /// <summary>
        /// Evaluates the payment type with HCPCS without ObervervedServiceUnit false test.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeWithHcpcsWithoutIsObervervedServiceUnit()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ServiceTypeId = 1, Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 1}
            };
            PaymentTypeFeeSchedule paymentTypeFeeSchedule = new PaymentTypeFeeSchedule
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
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                        {
                            new ClaimFieldValue
                            {
                                Identifier = "40101",
                                Value = "100"
                            },
                            new ClaimFieldValue
                            {
                                Identifier = "3010126",
                                Value = "100"
                            }
                        }
                },
                ServiceTypeId = 1,
                ValidLineIds = new List<int> { 1, 2 },
                HcpcsCode = "ABCDE",
                NonFeeSchedule = 50,
                FeeSchedule = 75,
                IsObserveUnits = false
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeFeeScheduleRepository> paymentTypeFeeScheduleRepository =
                new Mock<IPaymentTypeFeeScheduleRepository>();
            paymentTypeFeeScheduleRepository.Setup(x => x.GetPaymentTypeFeeSchedule(It.IsAny<PaymentTypeFeeSchedule>()))
                .Returns(paymentTypeFeeSchedule);

            var target = new PaymentTypeFeeScheduleLogic(paymentTypeFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeFeeSchedule
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
                    HcpcsCode = "3000126",
                    Units = 2,
                    HcpcsCodeWithModifier = "3000126",
                    HcpcsModifiers = "261"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "40101",
                    Units = 2,
                    HcpcsCodeWithModifier = "4010126",
                    HcpcsModifiers = "26"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            if (firstOrDefault != null)
                Assert.AreEqual(75, firstOrDefault.AdjudicatedValue);
            Assert.AreEqual(2, actual.Count);
        }

    }
}
