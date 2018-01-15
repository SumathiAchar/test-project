using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Data.Repository;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class PaymentTypeMedicareLabFeeScheduleLogicTest
    {
        [TestMethod]
        //Testing AuditLog constructor without parameter
        public void PaymentTypeMedicareLabFeeScheduleParameterlessConstructorTest()
        {
            var target = new PaymentTypeMedicareLabFeeScheduleLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTypeMedicareLabFeeScheduleLogic));
        }

        /// <summary>
        /// Payments the type medicare lab fee schedule constructor test.
        /// </summary>
          [TestMethod]
        public void PaymentTypeMedicareLabFeeScheduleConstructorTest()
        {
            IPaymentTypeMedicareLabFeeScheduleRepository paymentTypeMedicareLabFeeScheduleRepository = new PaymentTypeMedicareLabFeeScheduleRepository(Constants.ConnectionString);
            PaymentTypeMedicareLabFeeScheduleLogic target = new PaymentTypeMedicareLabFeeScheduleLogic(paymentTypeMedicareLabFeeScheduleRepository);
            Assert.IsInstanceOfType(target, typeof(PaymentTypeMedicareLabFeeScheduleLogic));
        }

        /// <summary>
        /// Payments the type medicare lab fee schedule details constructor test.
        /// </summary>
        [TestMethod]
        public void PaymentTypeMedicareLabFeeScheduleConstructorTest1()
        {
            var mockPaymentTypeMedicareLabFeeSchedulePayment = new Mock<IPaymentTypeMedicareLabFeeScheduleRepository>();
            PaymentTypeMedicareLabFeeScheduleLogic target = new PaymentTypeMedicareLabFeeScheduleLogic(mockPaymentTypeMedicareLabFeeSchedulePayment.Object);
            Assert.IsInstanceOfType(target, typeof(PaymentTypeMedicareLabFeeScheduleLogic));
        }


        /// <summary>
        /// Adds the edit medicare lab fee schedule with null payment.
        /// </summary>
        [TestMethod]
        public void AddEditMedicareLabFeeScheduleWithNullPayment()
        {
            var mockAddEditPaymentTypeMedicareLabFeeSchedulePayment = new Mock<IPaymentTypeMedicareLabFeeScheduleRepository>();
            mockAddEditPaymentTypeMedicareLabFeeSchedulePayment.Setup(f => f.AddEditPaymentTypeMedicareLabFeeSchedulePayment(It.IsAny<PaymentTypeMedicareLabFeeSchedule>())).Returns(1);
            PaymentTypeMedicareLabFeeScheduleLogic target = new PaymentTypeMedicareLabFeeScheduleLogic(mockAddEditPaymentTypeMedicareLabFeeSchedulePayment.Object);
            long actual = target.AddEditPaymentType(null);
            Assert.AreEqual(1, actual);
        }


        /// <summary>
        /// Adds the edit medicare lab fee schedule with valid data test.
        /// </summary>
        [TestMethod]
        public void AddEditMedicareLabFeeScheduleWithValidDataTest()
        {
            var mockAddEditPaymentTypeMedicareLabFeeSchedulePayment = new Mock<IPaymentTypeMedicareLabFeeScheduleRepository>();
            mockAddEditPaymentTypeMedicareLabFeeSchedulePayment.Setup(f => f.AddEditPaymentTypeMedicareLabFeeSchedulePayment(It.IsAny<PaymentTypeMedicareLabFeeSchedule>())).Returns(1);
            PaymentTypeMedicareLabFeeScheduleLogic target = new PaymentTypeMedicareLabFeeScheduleLogic(mockAddEditPaymentTypeMedicareLabFeeSchedulePayment.Object);
            PaymentTypeMedicareLabFeeSchedule objAddEditPaymentTypeMedicareLabFeeSchedulePayment = new PaymentTypeMedicareLabFeeSchedule { PaymentTypeDetailId = 1 };

            long actual = target.AddEditPaymentType(objAddEditPaymentTypeMedicareLabFeeSchedulePayment);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// Gets the payment type medicare lab fee schedule test.
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeMedicareLabFeeScheduleTest()
        {
            PaymentTypeMedicareLabFeeSchedule objAddEditPaymentTypeMedicareLabFeeSchedulePayment = new PaymentTypeMedicareLabFeeSchedule { PaymentTypeDetailId = 1, PaymentTypeId = 4, ContractId = 234, ServiceTypeId = null };
            var mockGetPaymentTypeStopLoss = new Mock<IPaymentTypeMedicareLabFeeScheduleRepository>();
            PaymentTypeMedicareLabFeeSchedule result = new PaymentTypeMedicareLabFeeSchedule();
            mockGetPaymentTypeStopLoss.Setup(f => f.GetPaymentTypeMedicareLabFeeSchedulePayment(objAddEditPaymentTypeMedicareLabFeeSchedulePayment)).Returns(result);
            PaymentTypeMedicareLabFeeScheduleLogic target = new PaymentTypeMedicareLabFeeScheduleLogic(mockGetPaymentTypeStopLoss.Object);
            PaymentTypeMedicareLabFeeSchedule actual = (PaymentTypeMedicareLabFeeSchedule)target.GetPaymentType(null);
            Assert.AreEqual(null, actual);


        }

        /// <summary>
        /// Evaluates the test at line level null test.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtLineLevelNullTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1,Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedule = new PaymentTypeMedicareLabFeeSchedule
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
            Mock<IPaymentTypeMedicareLabFeeScheduleRepository> paymentTypeMedicareLabFeeScheduleRepository = new Mock<IPaymentTypeMedicareLabFeeScheduleRepository>();
            paymentTypeMedicareLabFeeScheduleRepository.Setup(x => x.GetPaymentTypeMedicareLabFeeSchedulePayment(It.IsAny<PaymentTypeMedicareLabFeeSchedule>()))
                .Returns(paymentTypeMedicareLabFeeSchedule);

            var target = new PaymentTypeMedicareLabFeeScheduleLogic(paymentTypeMedicareLabFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareLabFeeSchedule
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


        /// <summary>
        /// Evaluates the test at line level test.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtLineLevelTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1,Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedule = new PaymentTypeMedicareLabFeeSchedule
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
                Percentage = 50,
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
            Mock<IPaymentTypeMedicareLabFeeScheduleRepository> paymentTypeMedicareLabFeeScheduleRepository = new Mock<IPaymentTypeMedicareLabFeeScheduleRepository>();
            paymentTypeMedicareLabFeeScheduleRepository.Setup(x => x.GetPaymentTypeMedicareLabFeeSchedulePayment(It.IsAny<PaymentTypeMedicareLabFeeSchedule>()))
                .Returns(paymentTypeMedicareLabFeeSchedule);

            var target = new PaymentTypeMedicareLabFeeScheduleLogic(paymentTypeMedicareLabFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareLabFeeSchedule
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
            {
                new MedicareLabFeeSchedule
                {
                    Amount = 10,
                    Carrier = "ABCDE",
                    Hcpcs = "ABCDE",
                    HcpcsCodeWithModifier = "ABCDE"
                }
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250",
                    HcpcsCode = "ABCDE",
                    HcpcsCodeWithModifier="ABCDE",
                    Units = 2
                },
                 new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "ABCDF",
                     HcpcsCodeWithModifier="ABCDF",
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
            if (firstOrDefault != null) Assert.AreEqual(5,firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test at line level with HCPCS include modifiers returns true test.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtLineLevelWithHcpcsIncludeModifiersReturnsTrueTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1,Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedule = new PaymentTypeMedicareLabFeeSchedule
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
                Percentage = 50,
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
            Mock<IPaymentTypeMedicareLabFeeScheduleRepository> paymentTypeMedicareLabFeeScheduleRepository = new Mock<IPaymentTypeMedicareLabFeeScheduleRepository>();
            paymentTypeMedicareLabFeeScheduleRepository.Setup(x => x.GetPaymentTypeMedicareLabFeeSchedulePayment(It.IsAny<PaymentTypeMedicareLabFeeSchedule>()))
                .Returns(paymentTypeMedicareLabFeeSchedule);

            var target = new PaymentTypeMedicareLabFeeScheduleLogic(paymentTypeMedicareLabFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareLabFeeSchedule
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
            {
                new MedicareLabFeeSchedule
                {
                    Amount = 10,
                    Carrier = "ABCDE",
                    Hcpcs = "ABCDE",
                    HcpcsCodeWithModifier = "ABCDE26"
                }
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250",
                    HcpcsCode = "ABCDE",
                    Units = 2,
                    HcpcsCodeWithModifier = "ABCDE26",
                    HcpcsModifiers = "26"
                },
                 new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "ABCDF",
                    Units = 2,
                    HcpcsCodeWithModifier = "ABCDF26",
                    HcpcsModifiers = "26"
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
            if (firstOrDefault != null) Assert.AreEqual(5, firstOrDefault.AdjudicatedValue);
        }
        
        /// <summary>
        /// Evaluates the test at line level with HCPCS include modifiers returns false test.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtLineLevelWithHcpcsIncludeModifiersReturnsFalseTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1,Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedule = new PaymentTypeMedicareLabFeeSchedule
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
                Percentage = 50,
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
            Mock<IPaymentTypeMedicareLabFeeScheduleRepository> paymentTypeMedicareLabFeeScheduleRepository = new Mock<IPaymentTypeMedicareLabFeeScheduleRepository>();
            paymentTypeMedicareLabFeeScheduleRepository.Setup(x => x.GetPaymentTypeMedicareLabFeeSchedulePayment(It.IsAny<PaymentTypeMedicareLabFeeSchedule>()))
                .Returns(paymentTypeMedicareLabFeeSchedule);

            var target = new PaymentTypeMedicareLabFeeScheduleLogic(paymentTypeMedicareLabFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareLabFeeSchedule
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
            {
                new MedicareLabFeeSchedule
                {
                    Amount = 10,
                    Carrier = "ABCDE",
                    Hcpcs = "ABCDE",
                    HcpcsCodeWithModifier = "ABCDE16"
                }
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250",
                    HcpcsCode = "ABCDE",
                    Units = 2,
                    HcpcsCodeWithModifier = "ABCDE26",
                    HcpcsModifiers = "26"
                },
                 new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "ABCDF",
                    Units = 2,
                    HcpcsCodeWithModifier = "ABCDF26",
                    HcpcsModifiers = "26"
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
        /// Evaluates the test at line level with HCPCS without include modifiers returns true test.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtLineLevelWithHcpcsWithoutIncludeModifiersReturnsTrueTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1,Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedule = new PaymentTypeMedicareLabFeeSchedule
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
                Percentage = 50,
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
            Mock<IPaymentTypeMedicareLabFeeScheduleRepository> paymentTypeMedicareLabFeeScheduleRepository = new Mock<IPaymentTypeMedicareLabFeeScheduleRepository>();
            paymentTypeMedicareLabFeeScheduleRepository.Setup(x => x.GetPaymentTypeMedicareLabFeeSchedulePayment(It.IsAny<PaymentTypeMedicareLabFeeSchedule>()))
                .Returns(paymentTypeMedicareLabFeeSchedule);

            var target = new PaymentTypeMedicareLabFeeScheduleLogic(paymentTypeMedicareLabFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareLabFeeSchedule
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
            {
                new MedicareLabFeeSchedule
                {
                    Amount = 10,
                    Carrier = "ABCDE",
                    Hcpcs = "ABCDE",
                    HcpcsCodeWithModifier = "ABCDE26"
                }
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250",
                    HcpcsCode = "ABCDE",
                    Units = 2,
                    HcpcsCodeWithModifier = "ABCDE26",
                    HcpcsModifiers = "26"
                },
                 new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "ABCDF",
                    Units = 2,
                    HcpcsCodeWithModifier = "ABCDF26",
                    HcpcsModifiers = "26"
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
            if (firstOrDefault != null) Assert.AreEqual(5, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test at line level with only HCPC code test.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtLineLevelWithOnlyHcpcCodeTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1,Line = 2},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedule = new PaymentTypeMedicareLabFeeSchedule
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
                Percentage = 50,
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
            Mock<IPaymentTypeMedicareLabFeeScheduleRepository> paymentTypeMedicareLabFeeScheduleRepository = new Mock<IPaymentTypeMedicareLabFeeScheduleRepository>();
            paymentTypeMedicareLabFeeScheduleRepository.Setup(x => x.GetPaymentTypeMedicareLabFeeSchedulePayment(It.IsAny<PaymentTypeMedicareLabFeeSchedule>()))
                .Returns(paymentTypeMedicareLabFeeSchedule);

            var target = new PaymentTypeMedicareLabFeeScheduleLogic(paymentTypeMedicareLabFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareLabFeeSchedule
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
            {
                new MedicareLabFeeSchedule
                {
                    Amount = 10,
                    Carrier = "ABCDE",
                    Hcpcs = "ABCDE",
                    HcpcsCodeWithModifier = "ABCDE"
                }
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250",
                    HcpcsCode = "ABCDE",
                    Units = 2,
                    HcpcsCodeWithModifier = "ABCDE26",
                    HcpcsModifiers = "26"
                },
                 new ClaimCharge
                {
                    Line = 2,
                    Amount = 200,
                    RevCode = "250",
                    HcpcsCode = "ABCDF",
                    Units = 2,
                    HcpcsCodeWithModifier = "ABCDF26",
                    HcpcsModifiers = "26"
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
            Assert.IsNotNull(firstOrDefault);
        }
    }
}
