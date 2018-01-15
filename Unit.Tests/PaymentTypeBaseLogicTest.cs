using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class PaymentTypeBaseLogicTest
    {
        //Creating object for Logic
        private PaymentTypeBaseLogic _target;

        private Mock<IPaymentTypeFeeScheduleRepository> _mockPaymentTypeFeeScheduleRepository;

        /// <summary>
        /// Determines whether [is match test].
        /// </summary>
        [TestMethod]
        public void IsMatchTest()
        {
            // Arrange
            _mockPaymentTypeFeeScheduleRepository = new Mock<IPaymentTypeFeeScheduleRepository>();
            _target = new PaymentTypeFeeScheduleLogic(_mockPaymentTypeFeeScheduleRepository.Object);

            var paymentType = new PaymentTypeFeeSchedule
            {
                ValidLineIds = new List<int> { 1 },
                NonFeeSchedule = 50,
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        RightOperand = "250",
                        OperandIdentifier = (byte) Enums.OperandIdentifier.RevCode,
                        OperandType = (byte) Enums.OperandType.Numeric,
                        PropertyColumnName = Constants.PropertyRevCode,
                        LeftOperands = new List<string> {"250", "350"}
                    }
                }
            };
            _target.PaymentTypeBase = paymentType;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250"
                }
            };

            //Act
            bool actual = _target.IsMatch(evaluateableClaim);

            // Assert
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Determines whether [is payment type condition with invalid data test].
        /// </summary>
        [TestMethod]
        public void IsPaymentTypeConditionWithInvalidDataTest()
        {
            // Arrange
            _mockPaymentTypeFeeScheduleRepository = new Mock<IPaymentTypeFeeScheduleRepository>();
            _target = new PaymentTypeFeeScheduleLogic(_mockPaymentTypeFeeScheduleRepository.Object);
            
            var paymentType = new PaymentTypeFeeSchedule
            {
                ValidLineIds = new List<int> { 1 },
                NonFeeSchedule = 50,
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        RightOperand = "K2250",
                        OperandIdentifier = (byte) Enums.OperandIdentifier.HcpcsCode,
                        OperandType = (byte) Enums.OperandType.AlphaNumeric,
                        PropertyColumnName = Constants.PropertyHcpcsCode,
                        LeftOperands = new List<string> {"J2250", "12350"}
                    }
                }
            };
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250",
                    HcpcsCode = "12545",
                    HcpcsCodeWithModifier ="12545" 
                }
            };

            //Act
            bool actual = _target.IsPaymentTypeConditions(paymentType, evaluateableClaim);

            // Assert
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Determines whether [is payment type condition with payment type null test].
        /// </summary>
        [TestMethod]
        public void IsPaymentTypeConditionWithPaymentTypeNullTest()
        {
            // Arrange
            _mockPaymentTypeFeeScheduleRepository = new Mock<IPaymentTypeFeeScheduleRepository>();
            _target = new PaymentTypeFeeScheduleLogic(_mockPaymentTypeFeeScheduleRepository.Object);

            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "250",
                    HcpcsCode = "12545"
                }
            };

            //Act
            bool actual = _target.IsPaymentTypeConditions(null, evaluateableClaim);

            // Assert
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void EvaluateTest()
        {
            // Arrange
            _mockPaymentTypeFeeScheduleRepository = new Mock<IPaymentTypeFeeScheduleRepository>();
            _target = new PaymentTypeFeeScheduleLogic(_mockPaymentTypeFeeScheduleRepository.Object);

            var paymentType = new PaymentTypeFeeSchedule
            {
                ValidLineIds = new List<int> { 1 },
                NonFeeSchedule = 50,
                ClaimFieldDoc = new ClaimFieldDoc
               {
                   ClaimFieldDocId = 12,
                   ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "J2123", Value = "50" } }
               },
                Conditions = new List<ICondition>()
            };
            _target.PaymentTypeBase = paymentType;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    HcpcsCode = "12500",
                    HcpcsCodeWithModifier = "12500"
                }
            };
            var paymentResults = new List<PaymentResult> {new PaymentResult {Line = 1}};

            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(10, actual[0].AdjudicatedValue);
        }

        [TestMethod]
        public void EvaluateWithAdjudicatedLineTest()
        {
            // Arrange
            _mockPaymentTypeFeeScheduleRepository = new Mock<IPaymentTypeFeeScheduleRepository>();
            _target = new PaymentTypeFeeScheduleLogic(_mockPaymentTypeFeeScheduleRepository.Object);

            var paymentType = new PaymentTypeFeeSchedule
            {
                ValidLineIds = new List<int> { 1 },
                NonFeeSchedule = 50,
                ServiceTypeId = 12,
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldDocId = 12,
                    ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "J2123", Value = "50" } }
                },
                Conditions = new List<ICondition>()
            };
            _target.PaymentTypeBase = paymentType;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    HcpcsCode = "12500",
                    HcpcsCodeWithModifier = "12500"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {Line = 1, AdjudicatedValue = 10, ServiceTypeId = 12}
            };

            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(paymentResults.Count, actual.Count);
        }
    }
}
