/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Payment Type PerCase Details Logic Testing
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
    /// Summary description for PaymentTypePerCaseDetailsLogicUnitTest
    /// </summary>
    [TestClass]
    public class PaymentTypePerCaseDetailsLogicUnitTest
    {

        //Creating object for Logic
        private PaymentTypePerCaseLogic _target;

        //Creating mock object 
        private Mock<IPaymentTypePerCaseRepository> _mockPaymentTypePerCaseRepository;
        private Mock<IContractServiceTypeLogic> _mockContractServiceTypeLogic;
        private Mock<IPaymentResultLogic> _mockPaymentResultLogic;
        private Mock<ContractBaseLogic> _mockContractBaseLogic;
        
        /// <summary>
        /// Payments the type per case details constructor unit test1.
        /// </summary>
        [TestMethod]
        public void PaymentTypePerCaseDetailsConstructorUnitTest1()
        {
            _target = new PaymentTypePerCaseLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(_target, typeof(PaymentTypePerCaseLogic));
        }

        /// <summary>
        /// Payments the type per case details constructor unit test2.
        /// </summary>
        [TestMethod]
        public void PaymentTypePerCaseDetailsConstructorUnitTest2()
        {
            _mockPaymentTypePerCaseRepository = new Mock<IPaymentTypePerCaseRepository>();
            PaymentTypePerCaseLogic target = new PaymentTypePerCaseLogic(_mockPaymentTypePerCaseRepository.Object);
            Assert.IsInstanceOfType(target, typeof(PaymentTypePerCaseLogic));
        }

        /// <summary>
        /// AddEdit PaymentType Per Case For IsNotNull
        /// </summary>
        [TestMethod]
        public void AddEditPaymentTypePerCaseIfNull()
        {
            _mockPaymentTypePerCaseRepository = new Mock<IPaymentTypePerCaseRepository>();
            _mockPaymentTypePerCaseRepository.Setup(f => f.AddEditPaymentTypePerCase(It.IsAny<PaymentTypePerCase>())).Returns(0);
            _target = new PaymentTypePerCaseLogic(_mockPaymentTypePerCaseRepository.Object);
            long actual = _target.AddEditPaymentType(null);
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        ///  AddEdit PaymentType Per Case For AreEqual
        /// </summary>
        [TestMethod]
        public void AddPaymentTypePerCaseIfNotNull()
        {
            _mockPaymentTypePerCaseRepository = new Mock<IPaymentTypePerCaseRepository>();
            _mockPaymentTypePerCaseRepository.Setup(f => f.AddEditPaymentTypePerCase(It.IsAny<PaymentTypePerCase>())).Returns(1);
            _target = new PaymentTypePerCaseLogic(_mockPaymentTypePerCaseRepository.Object);
            PaymentTypePerCase objPaymentTypePerCase = new PaymentTypePerCase { PaymentTypeDetailId = 1 };
            long actual = _target.AddEditPaymentType(objPaymentTypePerCase);
            Assert.AreEqual(1, actual); 
        }

        /// <summary>
        ///  AddEdit PaymentType Per Case For AreEqual
        /// </summary>
        [TestMethod]
        public void EditPaymentTypePerCaseIfNotNull()
        {
            _mockPaymentTypePerCaseRepository = new Mock<IPaymentTypePerCaseRepository>();
            _mockPaymentTypePerCaseRepository.Setup(f => f.AddEditPaymentTypePerCase(It.IsAny<PaymentTypePerCase>())).Returns(1);
            _target = new PaymentTypePerCaseLogic(_mockPaymentTypePerCaseRepository.Object);
            PaymentTypePerCase objPaymentTypePerCase = new PaymentTypePerCase { PaymentTypeDetailId = 1 };
            long actual = _target.AddEditPaymentType(objPaymentTypePerCase);
            Assert.AreEqual(1, actual);
        }


        /// <summary>
        /// Get Payment Per Case Test if Null
        /// </summary>
        [TestMethod]
        public void GetPaymentPerCaseDetailsMockTestIfNull()
        {
            PaymentTypePerCase result =new PaymentTypePerCase();
            _mockPaymentTypePerCaseRepository = new Mock<IPaymentTypePerCaseRepository>();
            _mockPaymentTypePerCaseRepository.Setup(f => f.GetPaymentTypePerCase(It.IsAny<PaymentTypePerCase>())).Returns(result);
            _target = new PaymentTypePerCaseLogic(_mockPaymentTypePerCaseRepository.Object);
            PaymentTypePerCase actual = (PaymentTypePerCase)_target.GetPaymentType(null);
            Assert.AreEqual(result, actual);

        }

        /// <summary>
        /// Get Payment Type StopLoss
        /// </summary>
        [TestMethod]
        public void GetPaymentPerCaseDetailsMockTestIfNotNull()
        {
            PaymentTypePerCase objPaymentTypePerCase = new PaymentTypePerCase { PaymentTypeId = 6, ContractId=890,ServiceTypeId = null};
            PaymentTypePerCase result = new PaymentTypePerCase { Rate = 234, PaymentTypeDetailId =678};
            _mockPaymentTypePerCaseRepository = new Mock<IPaymentTypePerCaseRepository>();
            _mockPaymentTypePerCaseRepository.Setup(f => f.GetPaymentTypePerCase(objPaymentTypePerCase)).Returns(result);
            _target = new PaymentTypePerCaseLogic(_mockPaymentTypePerCaseRepository.Object);
            PaymentTypePerCase actual = (PaymentTypePerCase)_target.GetPaymentType(objPaymentTypePerCase);
            Assert.AreEqual(result, actual);

        }

        [TestMethod]
        public void EvaluatePaymentTypeTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypePerCase paymentTypePerCase = new PaymentTypePerCase
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
                ValidLineIds = new List<int> { 1 },
                HcpcsCode = "ABCDE",
                MaxCasesPerDay = 1,
                Rate = 10
            };
            // Arrange
            _mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _mockPaymentTypePerCaseRepository  = new Mock<IPaymentTypePerCaseRepository>();
            _mockPaymentTypePerCaseRepository.Setup(x => x.GetPaymentTypePerCase(It.IsAny<PaymentTypePerCase>()))
                .Returns(paymentTypePerCase);

            _target = new PaymentTypePerCaseLogic(_mockPaymentTypePerCaseRepository.Object)
            {
                PaymentTypeBase = paymentTypePerCase
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
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            _mockContractBaseLogic = new Mock<ContractBaseLogic>();
            _mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            _mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            _mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue !=null);
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the payment type test when carve out.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeTestWhenCarveOut()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypePerCase paymentTypePerCase = new PaymentTypePerCase
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
                ValidLineIds = new List<int> { 1 },
                HcpcsCode = "ABCDE",
                MaxCasesPerDay = 2,
                Rate = 10
            };
            // Arrange
            _mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _mockPaymentTypePerCaseRepository = new Mock<IPaymentTypePerCaseRepository>();
            _mockPaymentTypePerCaseRepository.Setup(x => x.GetPaymentTypePerCase(It.IsAny<PaymentTypePerCase>()))
                .Returns(paymentTypePerCase);

            _target = new PaymentTypePerCaseLogic(_mockPaymentTypePerCaseRepository.Object)
            {
                PaymentTypeBase = paymentTypePerCase
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
                    Units = 1
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
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
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the payment type test with0 maximum cases.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeTestWith0MaxCases()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypePerCase paymentTypePerCase = new PaymentTypePerCase
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
                ValidLineIds = new List<int> { 1 },
                HcpcsCode = "ABCDE",
                MaxCasesPerDay = 0,
                Rate = 10
            };
            // Arrange
           _mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _mockPaymentTypePerCaseRepository = new Mock<IPaymentTypePerCaseRepository>();
            _mockPaymentTypePerCaseRepository.Setup(x => x.GetPaymentTypePerCase(It.IsAny<PaymentTypePerCase>()))
                .Returns(paymentTypePerCase);

            _target = new PaymentTypePerCaseLogic(_mockPaymentTypePerCaseRepository.Object)
            {
                PaymentTypeBase = paymentTypePerCase
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
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            _mockContractBaseLogic = new Mock<ContractBaseLogic>();
            _mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            _mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            _mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.IsNull(firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the payment type test with no units.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeTestWithNoUnits()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypePerCase paymentTypePerCase = new PaymentTypePerCase
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
                ValidLineIds = new List<int> {1},
                HcpcsCode = "ABCDE",
                MaxCasesPerDay = 0
            };
            // Arrange
            _mockContractServiceTypeLogic= new Mock<IContractServiceTypeLogic>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _mockPaymentTypePerCaseRepository = new Mock<IPaymentTypePerCaseRepository>();
            _mockPaymentTypePerCaseRepository.Setup(x => x.GetPaymentTypePerCase(It.IsAny<PaymentTypePerCase>()))
                .Returns(paymentTypePerCase);

            _target = new PaymentTypePerCaseLogic(_mockPaymentTypePerCaseRepository.Object)
            {
                PaymentTypeBase = paymentTypePerCase
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
                    HcpcsCode = "ABCDE"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            _mockContractBaseLogic = new Mock<ContractBaseLogic>();
            _mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            _mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            _mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.IsNull( firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the payment type pay at claim level.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypePayAtClaimLevel()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1,ServiceTypeId = 1}
            };
            PaymentTypePerCase paymentTypePerCase = new PaymentTypePerCase
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
                ValidLineIds = new List<int> { 1 },
                HcpcsCode = "ABCDE",
                MaxCasesPerDay = 1,
                Rate = 10
            };
            // Arrange
            _mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _mockPaymentTypePerCaseRepository = new Mock<IPaymentTypePerCaseRepository>();
            _mockPaymentTypePerCaseRepository.Setup(x => x.GetPaymentTypePerCase(It.IsAny<PaymentTypePerCase>()))
                .Returns(paymentTypePerCase);

            _target = new PaymentTypePerCaseLogic(_mockPaymentTypePerCaseRepository.Object)
            {
                PaymentTypeBase = paymentTypePerCase
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
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            _mockContractBaseLogic = new Mock<ContractBaseLogic>();
            _mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            _mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            _mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            if (firstOrDefault != null)
                Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }
    }
}
