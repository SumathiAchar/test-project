/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Payment Type Cap Logic Testing
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
    /// Summary description for PaymentTypeCapLogicUnitTest
    /// </summary>
    [TestClass]
    public class PaymentTypeCapLogicUnitTest
    {
        //Creating object for Logic
        private PaymentTypeCapLogic _target;

        //Creating mock object 
        private Mock<IPaymentTypeCapRepository> _mockPaymentTypeCapRepository;
        private Mock<IContractServiceTypeLogic> _mockContractServiceTypeLogic;
        private Mock<IPaymentResultLogic> _mockPaymentResultLogic;
        private Mock<ContractBaseLogic> _mockContractBaseLogic;
        
        /// <summary>
        /// Payments the type cap constructor unit test1.
        /// </summary>
        [TestMethod]
        public void PaymentTypeCapConstructorUnitTest1()
        {
            _target = new PaymentTypeCapLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(_target, typeof(PaymentTypeCapLogic));
        }


        /// <summary>
        /// Payments the type cap constructor unit test2.
        /// </summary>
        [TestMethod]
        public void PaymentTypeCapConstructorUnitTest2()
        {
           _mockPaymentTypeCapRepository = new Mock<IPaymentTypeCapRepository>();
           _target = new PaymentTypeCapLogic(_mockPaymentTypeCapRepository.Object);
            //Assert.Inconclusive();
           Assert.IsInstanceOfType(_target, typeof(PaymentTypeCapLogic));
        }
        
        /// <summary>
        /// Adds the  payment type cap details test for null
        /// </summary>
        [TestMethod]
        public void AddEditPaymentTypeCapDetailsIfNull()
        {
            _mockPaymentTypeCapRepository = new Mock<IPaymentTypeCapRepository>();
            _mockPaymentTypeCapRepository.Setup(f => f.AddEditPaymentTypeCapDetails(It.IsAny<PaymentTypeCap>())).Returns(1);
            _target = new PaymentTypeCapLogic(_mockPaymentTypeCapRepository.Object);

            long actual = _target.AddEditPaymentType(null);
            Assert.AreEqual(1, actual);
        }
        /// <summary>
        ///  Edit payment type cap details Test for not null
        /// </summary>
        [TestMethod]
        public void AddEditPaymentTypeCapDetailsifNotNull()
        {
            _mockPaymentTypeCapRepository = new Mock<IPaymentTypeCapRepository>();
            _mockPaymentTypeCapRepository.Setup(f => f.AddEditPaymentTypeCapDetails(It.IsAny<PaymentTypeCap>())).Returns(1);
            _target = new PaymentTypeCapLogic(_mockPaymentTypeCapRepository.Object);
            PaymentTypeCap objAddEditPaymentTypeCapDetails = new PaymentTypeCap { PaymentTypeDetailId = 1 };

            long actual = _target.AddEditPaymentType(objAddEditPaymentTypeCapDetails);
            Assert.AreEqual(1, actual);

        }
        /// <summary>
        /// Get Payment Type CapDetails If null 
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeCapDetailsTestifNull()
        {
            PaymentTypeCap objGetPaymentTypeCapDetails = new PaymentTypeCap {Threshold = 1};

            _mockPaymentTypeCapRepository = new Mock<IPaymentTypeCapRepository>();
            _mockPaymentTypeCapRepository.Setup(f => f.GetPaymentTypeCapDetails(It.IsAny<PaymentTypeCap>())).Returns(objGetPaymentTypeCapDetails);
            _target = new PaymentTypeCapLogic(_mockPaymentTypeCapRepository.Object);

            PaymentTypeCap actual = (PaymentTypeCap)_target.GetPaymentType(null);
            Assert.AreEqual(1, actual.Threshold);
        }

        /// <summary>
        ///  Get Payment Type CapDetails If null Test
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeCapDetailsTestifNotNull()
        {
            _mockPaymentTypeCapRepository = new Mock<IPaymentTypeCapRepository>();
            _mockPaymentTypeCapRepository.Setup(f => f.GetPaymentTypeCapDetails(It.IsAny<PaymentTypeCap>()));
            _target = new PaymentTypeCapLogic(_mockPaymentTypeCapRepository.Object);
            PaymentTypeCap objGetPaymentTypeCapDetails = new PaymentTypeCap { PaymentTypeDetailId = 1 };

            PaymentTypeCap actual = (PaymentTypeCap)_target.GetPaymentType(objGetPaymentTypeCapDetails);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void EvaluateTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = 110,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = 110,ServiceTypeId = 1},
            };
            PaymentTypeCap paymentTypeCap = new PaymentTypeCap
            {
                Threshold = 10,
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

            };
            // Arrange
            _mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _mockPaymentTypeCapRepository = new Mock<IPaymentTypeCapRepository>();
            _mockPaymentTypeCapRepository.Setup(x => x.GetPaymentTypeCapDetails(It.IsAny<PaymentTypeCap>()))
                .Returns(paymentTypeCap);

            _target = new PaymentTypeCapLogic(_mockPaymentTypeCapRepository.Object)
            {
                PaymentTypeBase = paymentTypeCap
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

            _mockContractBaseLogic = new Mock<ContractBaseLogic>();
            _mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            _mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            _mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            if (firstOrDefault != null) Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Contracts the filter test.
        /// </summary>
        [TestMethod]
        public void ContractFilterTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = 110,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = 110,ServiceTypeId = 1},
            };
            PaymentTypeCap paymentTypeCap = new PaymentTypeCap
            {
                Threshold = 10,
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

            };
            // Arrange
            _mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _mockPaymentTypeCapRepository = new Mock<IPaymentTypeCapRepository>();
            _mockPaymentTypeCapRepository.Setup(x => x.GetPaymentTypeCapDetails(It.IsAny<PaymentTypeCap>()))
                .Returns(paymentTypeCap);

            _target = new PaymentTypeCapLogic(_mockPaymentTypeCapRepository.Object)
            {
                PaymentTypeBase = paymentTypeCap
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

            _mockContractBaseLogic = new Mock<ContractBaseLogic>();
            _mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            _mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            _mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, true);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            if (firstOrDefault != null) Assert.AreEqual(10, firstOrDefault.AdjudicatedValue);
        }
        
    }
}
