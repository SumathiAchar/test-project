/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Payment Type Stop Loss Logic Testing
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
    /// Summary description for PaymentTypeStopLossLogicUnitTest
    /// </summary>
    [TestClass]
    public class PaymentTypeStopLossLogicUnitTest
    {
        //Creating object for Logic
        private PaymentTypeStopLossLogic _target;

        //Creating mock object 
        private Mock<IPaymentTypeStopLossRepository> _mockPaymentTypeStopLossLogic;
        private Mock<IContractServiceTypeLogic> _mockContractServiceTypeLogic;
        private Mock<IPaymentResultLogic> _mockPaymentResultLogic;
        private Mock<ContractBaseLogic> _mockContractBaseLogic;


        /// <summary>
        /// Payments the type stop loss constructor test.
        /// </summary>
        [TestMethod]
        public void PaymentTypeStopLossConstructorTest()
        {
            _target = new PaymentTypeStopLossLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(_target, typeof(PaymentTypeStopLossLogic));
        }
        

        /// <summary>
        /// Adds the payment type stop loss test1 For IsNotNull
        /// </summary>
        [TestMethod]
        public void AddPaymentTypeStopLossIfNull()
        {
            _mockPaymentTypeStopLossLogic = new Mock<IPaymentTypeStopLossRepository>();
            _mockPaymentTypeStopLossLogic.Setup(f => f.AddEditPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>())).Returns(0);
            _target = new PaymentTypeStopLossLogic(_mockPaymentTypeStopLossLogic.Object);
            long actual = _target.AddEditPaymentType(null);
            Assert.AreEqual(0, actual);

        }

        /// <summary>
        /// Adds the payment type stop loss test2 For AreEqual
        /// </summary>
        [TestMethod]
        public void AddPaymentTypeStopLossIfNotNull()
        {
            _mockPaymentTypeStopLossLogic = new Mock<IPaymentTypeStopLossRepository>();
            _mockPaymentTypeStopLossLogic.Setup(f => f.AddEditPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>())).Returns(2);
            _target = new PaymentTypeStopLossLogic(_mockPaymentTypeStopLossLogic.Object);
            var objPaymentTypeStopLoss = new PaymentTypeStopLoss { PaymentTypeDetailId = 1 };
            long actual = _target.AddEditPaymentType(objPaymentTypeStopLoss);
            Assert.AreEqual(2, actual);
        }

        /// <summary>
        /// Get the payment type stop loss Info Mocktest for Null.
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeStopLossInfoMockIsNull()
        {
            var objGetPaymentTypeCapDetails = new PaymentTypeStopLoss { ContractId = 1 };
            _mockPaymentTypeStopLossLogic = new Mock<IPaymentTypeStopLossRepository>();
            _mockPaymentTypeStopLossLogic.Setup(f => f.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>())).Returns(objGetPaymentTypeCapDetails);
            _target = new PaymentTypeStopLossLogic(_mockPaymentTypeStopLossLogic.Object);
            PaymentTypeStopLoss actual = (PaymentTypeStopLoss)_target.GetPaymentType(null);
            Assert.AreEqual(1, actual.ContractId);

        }


        /// <summary>
        /// Get the payment type stop loss Info Mocktest for Not Null.
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeStopLossInfoMockIsNotNull()
        {

            //Mock Input
            var objPaymentTypeStopLoss = new PaymentTypeStopLoss { PaymentTypeId = 1, ContractId = 124, ServiceTypeId = 1489 };
            //Mock output
            var paymentTypeStopLoss = new PaymentTypeStopLoss { PaymentTypeId = 1, Percentage = 3.5, ContractId = 124, ServiceTypeId = 1489 };
            _mockPaymentTypeStopLossLogic = new Mock<IPaymentTypeStopLossRepository>();
            _mockPaymentTypeStopLossLogic.Setup(f => f.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>())).Returns(paymentTypeStopLoss);
            _target = new PaymentTypeStopLossLogic(_mockPaymentTypeStopLossLogic.Object);
            PaymentTypeStopLoss actual = (PaymentTypeStopLoss)_target.GetPaymentType(objPaymentTypeStopLoss);
            Assert.AreEqual(paymentTypeStopLoss, actual);

        }

        /// <summary>
        /// Gets the payment type stop loss conditions.
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeStopLossConditions()
        {
            _mockPaymentTypeStopLossLogic = new Mock<IPaymentTypeStopLossRepository>();
            _mockPaymentTypeStopLossLogic.Setup(x => x.GetPaymentTypeStopLossConditions())
                .Returns(new List<StopLossCondition>());

            new PaymentTypeStopLossLogic(_mockPaymentTypeStopLossLogic.Object).GetConditions();
            _mockPaymentTypeStopLossLogic.VerifyAll();
        }

        /// <summary>
        /// Evaluates the test at total charge lines.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtTotalChargeLines()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 10,
                Percentage = 50,
                Expression = "sddd",
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
                RevCode = "300",
                Days = "1"
            };
            // Arrange
            _mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _mockPaymentTypeStopLossLogic = new Mock<IPaymentTypeStopLossRepository>();
            _mockPaymentTypeStopLossLogic.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            _target = new PaymentTypeStopLossLogic(_mockPaymentTypeStopLossLogic.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 10,
                    ValidLineIds = new List<int> { 1 },
                    Expression = "sdsfsd",
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.TotalChargeLines,
                    Conditions = new List<ICondition>
                        {
                            new Condition
                            {
                                ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                                LeftOperands = new List<string> {"300"},
                                OperandType = (int) Enums.OperandIdentifier.HcpcsCode,
                                RightOperand = "300"
                            }
                        },
                    ContractId = 1,
                    RevCode = "300",
                    Days = "1",
                    
                    
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 5,
                LOS = 10,
                TCC = 15
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300"
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            _mockContractBaseLogic = new Mock<ContractBaseLogic>();
            _mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            _mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            _mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(1, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test at total charge lines null test.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtTotalChargeLinesNullTest()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 10,
                Percentage = 50,
                Expression = "5*4",
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
                RevCode = "300",
                Days = "1"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 10,
                    ValidLineIds = new List<int> { 1 },
                    Expression = "5*4",
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.TotalChargeLines,
                    Conditions = new List<ICondition>
                        {
                            new Condition
                            {
                                ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                                LeftOperands = new List<string> {"300"},
                                OperandType = (int) Enums.OperandIdentifier.HcpcsCode,
                                RightOperand = "300"
                            }
                        },
                    ContractId = 1,
                    RevCode = "300",
                    Days = "1"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 5,
                LOS = 10,
                TCC = 15
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300"
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(1, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test at total charge lines at claim level.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtTotalChargeLinesAtClaimLevel()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 10,
                Percentage = 50,
                Expression = "5*2",
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
                RevCode = "300",
                Days = "1"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    PayAtClaimLevel = true,
                    Expression = "5*2",
                    Threshold = 10,
                    ValidLineIds = new List<int> { 1 },
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.TotalChargeLines,
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
                    RevCode = "300",
                    Days = "1"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 5,
                LOS = 10,
                TCC = 15
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300"
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(1, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null) 
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }



        /// <summary>
        /// Evaluates the test at total charge lines null threshold test.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtTotalChargeLinesNullThresholdTest()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1, AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                ValidLineIds = new List<int> {1},
                Expression = "5*4",
                Percentage = 50,
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
                RevCode = "300",
                Days = "1"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository =
                new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    ValidLineIds = new List<int> {1},
                    Percentage = 50,
                    Expression = "5*4",
                    StopLossConditionId = (byte)Enums.StopLossCondition.TotalChargeLines,
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
                    RevCode = "300",
                    Days = "1"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 10, LOS = 20, TCC = 15
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300"
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(1, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10,firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test at per charge line.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtPerChargeLine()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 10,
                Percentage = 50,
                Expression = "5*4",
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
                RevCode = "300",
                Days = "1"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 10,
                    Expression = "5*4",
                    ValidLineIds = new List<int> { 1 },
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.PerChargeLine
                    ,
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
                    RevCode = "300",
                    Days = "1"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 5, LOS = 10, TCC = 15
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 30,
                    RevCode = "300"
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(1, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(15, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test at per dayof stay when threshold is l ess than total charge.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtPerDayofStayWhenThresholdIsLEssThanTotalCharge()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                 new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 1,
                Expression = "3*4",
                Percentage = 50,
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
                RevCode = "300",
                Days = "1:2"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 1,
                    Expression = "3*4",
                    ValidLineIds = new List<int> { 1 },
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.PerDayofStay
                    ,
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
                    RevCode = "300",
                    Days = "1:2"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 10,
                LOS = 15,
                TCC = 20
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    RevCode = "300"
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test at per dayof stay.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtPerDayofStay()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 10,
                Expression = "3*4",
                Percentage = 50,
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
                RevCode = "300",
                Days = "1:2"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 10,
                    Expression = "3*4",
                    ValidLineIds = new List<int> { 1 },
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.PerDayofStay
                    ,
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
                    RevCode = "300",
                    Days = "1:2"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 5,
                LOS = 10,
                TCC = 15
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300",
                    ServiceFromDate = DateTime.Now.AddDays(-1)
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    RevCode = "300",
                    ServiceFromDate = DateTime.Now
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test at per dayof stay when daysare null.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtPerDayofStayWhenDaysareNull()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                 new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 10,
                Expression = "3*4",
                Percentage = 50,
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
                RevCode = "300",
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 10,
                    Expression = "3*4",
                    ValidLineIds = new List<int> { 1 },
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.PerDayofStay
                    ,
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
                    RevCode = "300",
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 2, LOS = 5, TCC = 10
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300",
                    ServiceFromDate = DateTime.Now.AddDays(-1)
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    RevCode = "300",
                    ServiceFromDate = DateTime.Now
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test at per dayof stay negative scenario.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtPerDayofStayNegativeScenario()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1},
                 new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null,ServiceTypeId = 1}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                ServiceTypeId = 1,
                Threshold = 10,
                Percentage = 50,
                Expression = "3*4",
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
                RevCode = "300",
                Days = "1:"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    ServiceTypeId = 1,
                    Expression = "3*4",
                    Threshold = 10,
                    ValidLineIds = new List<int> { 1 },
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.PerDayofStay
                    ,
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
                    RevCode = "300",
                    Days = "1:"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 10, LOS = 15, TCC = 20
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300",
                    ServiceFromDate = DateTime.Now.AddDays(-1)
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 30,
                    RevCode = "300",
                    ServiceFromDate = DateTime.Now
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test at per dayof stay with invalid claim thru date.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtPerDayofStayWithInvalidClaimThruDate()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                 new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 10,
                Percentage = 50,
                Expression = "3*4",
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
                RevCode = "300",
                Days = "1"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 10,
                    ValidLineIds = new List<int> { 1,2 },
                    Expression = "3*4",
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.PerDayofStay
                    ,
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
                    RevCode = "300",
                    Days = "1"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 15, LOS = 12, TCC = 15
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300",
                    ServiceFromDate = DateTime.Now.AddDays(-1)
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    RevCode = "300",
                    ServiceFromDate = DateTime.Now
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test at per charge line contract level filters.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtPerChargeLineContractLevelFilters()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 1,
                Percentage = 50,
                Expression = "3*4",
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
                RevCode = "300",
                Days = "1"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 5,
                    ValidLineIds = new List<int> { 1 },
                    Expression = "3*4",
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.TotalChargeLines
                    ,
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
                    RevCode = "300",
                    Days = "1"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 10, LOS = 15, TCC = 20
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300"
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, true);

            // Assert
            Assert.AreEqual(1, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(50, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the total charge at line level.
        /// </summary>
        [TestMethod]
        public void EvaluateTotalChargeAtLineLevel()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 10,
                Percentage = 50,
                Expression = "5*4",
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
                Days = "1"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 10,
                    ValidLineIds = new List<int> { 1 },
                    Expression = "5*4",
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.TotalChargeLines
                    ,
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
                    Days = "1"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 5,
                LOS = 10,
                TCC = 15
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20
                    
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(50, firstOrDefault.AdjudicatedValue);
        }




        /// <summary>
        /// Applies the total charge at line level.
        /// </summary>
        [TestMethod]
        public void ApplyTotalChargeAtLineLevel()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 10,
                Percentage = 50,
                Expression = "5*2",
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
                RevCode = "300",
                Days = "1"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 10,
                    ValidLineIds = new List<int> { 1 },
                    Expression = "5*2",
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.TotalChargeLines,
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
                    RevCode = "300",
                    Days = "1"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 5,
                LOS = 10,
                TCC = 15
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300"
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(1, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Applies the total charge at line level with HCPCS include modifiers returns true.
        /// </summary>
        [TestMethod]
        public void ApplyTotalChargeAtLineLevelWithHcpcsIncludeModifiersReturnsTrue()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 10,
                Percentage = 50,
                Expression = "5*2",
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
                RevCode = "300",
                Days = "1"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 10,
                    ValidLineIds = new List<int> { 1 },
                    Expression = "5*2",
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.TotalChargeLines,
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
                    RevCode = "300",
                    Days = "1",
                    HcpcsCode = "3000126"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 5,
                LOS = 10,
                TCC = 15
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300",
                    HcpcsCodeWithModifier = "3000126",
                    HcpcsModifiers = "26",
                    HcpcsCode = "30001"
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(1, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Applies the total charge at line level with HCPCS without include modifiers returns true.
        /// </summary>
        [TestMethod]
        public void ApplyTotalChargeAtLineLevelWithHcpcsWithoutIncludeModifiersReturnsTrue()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 10,
                Percentage = 50,
                Expression = "5*2",
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
                RevCode = "300",
                Days = "1"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 10,
                    ValidLineIds = new List<int> { 1 },
                    Expression = "5*2",
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.TotalChargeLines,
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
                    RevCode = "300",
                    Days = "1",
                    HcpcsCode = "3000126"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 5,
                LOS = 10,
                TCC = 15
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300",
                    HcpcsCodeWithModifier = "3000426",
                    HcpcsModifiers = "16",
                    HcpcsCode = "30001"
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(1, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the apply per day for calculate per day.
        /// </summary>
        [TestMethod]
        public void EvaluateApplyPerDayForCalculatePerDay()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                 new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = 1,
                Expression = "5*2",
                Percentage = 50,
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
                RevCode = "300",
                Days = "1:2"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    Threshold = 1,
                    Expression = "5*2",
                    ValidLineIds = new List<int> { 1 },
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.PerDayofStay
                    ,
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
                    RevCode = "300",
                    Days = "1:2"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 10,
                LOS = 15,
                TCC = 20
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20,
                    RevCode = "300"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    RevCode = "300"
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the test at total charge lines at claim level test.
        /// </summary>
        [TestMethod]
        public void EvaluateTestAtTotalChargeLinesAtClaimLevelTest()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = null,AdjudicatedValue = null, ServiceTypeId=1}
            };
            var paymentTypeStopLoss = new PaymentTypeStopLoss
            {
                Threshold = null,
                Percentage = 50,
                Expression = null,
                ServiceTypeId=1,
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
                Days = "1"
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeStopLossRepository = new Mock<IPaymentTypeStopLossRepository>();
            paymentTypeStopLossRepository.Setup(x => x.GetPaymentTypeStopLoss(It.IsAny<PaymentTypeStopLoss>()))
                .Returns(paymentTypeStopLoss);

            var target = new PaymentTypeStopLossLogic(paymentTypeStopLossRepository.Object)
            {
                PaymentTypeBase = new PaymentTypeStopLoss
                {
                    PayAtClaimLevel = true,
                    Expression = null,
                    Threshold = null,
                    ValidLineIds = new List<int> { 1 },
                    Percentage = 50,
                    StopLossConditionId = (byte)Enums.StopLossCondition.TotalChargeLines
                    ,
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
                    Days = "1"
                }
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimTotal = -1;
            evaluateableClaim.SmartBox = new SmartBox
            {
                CAA = 5,
                LOS = 10,
                TCC = 15
            };
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    Line = 1,
                    Amount = 20
                }
            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }


    }
}

