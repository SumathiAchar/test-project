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
    //FIXED-2016-R3-S2: Do not use object name prefix with obj like objPaymentTypePercentageDiscount. Use class name in camel case.
                         //Need to change this in most of the methods.

    //FIXED-2016-R3-S2: Some method name has Test some does not. Should follow same standard of method naming convention. Postfix Test in every method.

    [TestClass]
    public class PaymentTypeMedicareSequesterLogicUnitTest
    {
        private PaymentTypeMedicareSequesterLogic _target;

        /// <summary>
        /// Payment type medicare sequester. 
        /// </summary>
        [TestMethod]
        public void PaymentTypeMedicareSequesterConstructorUnitTest1()
        {
            PaymentTypeMedicareSequesterRepository medicareSequesterRepository = new PaymentTypeMedicareSequesterRepository(Constants.ConnectionString);
            _target = new PaymentTypeMedicareSequesterLogic(medicareSequesterRepository);
            Assert.IsInstanceOfType(_target, typeof(PaymentTypeMedicareSequesterLogic));
        }

        /// <summary>
        /// Payment type medicare sequester constructor parameterless constructor test.
        /// </summary>
        //FIXED-2016-R3-S2: Rename method to PaymentTypeMedicareSequesterParameterlessConstructorUnitTest() 
        [TestMethod]
        public void PaymentTypeMedicareSequesterParameterlessConstructorUnitTest()
        {
            var target = new PaymentTypeMedicareSequesterLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTypeMedicareSequesterLogic));
        }

        /// <summary>
        /// Payment type medicare sequester constructor unit test2.
        /// </summary>
        [TestMethod]
        public void PaymentTypeMedicareSequesterConstructorUnitTest2()
        {
            var mockAddNewPaymentTypeMedicareSequester = new Mock<IPaymentTypeMedicareSequesterRepository>();
            PaymentTypeMedicareSequesterLogic target = new PaymentTypeMedicareSequesterLogic(mockAddNewPaymentTypeMedicareSequester.Object);
            Assert.IsInstanceOfType(target, typeof(PaymentTypeMedicareSequesterLogic));
        }

        /// <summary>
        /// Adds the payment type medicare sequester value if it is null
        /// </summary>
        //FIXED-2016-R3-S2: Rename AddNewPaymentTypeMedicareSequesterIfNull to AddNewPaymentTypeMedicareSequesterValueIfNullTest 
        [TestMethod]
        public void AddNewPaymentTypeMedicareSequesterValueIfNullTest()
        {
            var mockAddNewPaymentTypeMedicareSequester = new Mock<IPaymentTypeMedicareSequesterRepository>();
            mockAddNewPaymentTypeMedicareSequester.Setup(f => f.AddEditPaymentTypeMedicareSequester(It.IsAny<PaymentTypeMedicareSequester>())).Returns(0);
            PaymentTypeMedicareSequesterLogic target = new PaymentTypeMedicareSequesterLogic(mockAddNewPaymentTypeMedicareSequester.Object);
            long actual = target.AddEditPaymentType(null);
            Assert.AreEqual(0, actual);
        }

        //FIXED-2016-R3-S2: Add proper comment in summary. Correct case of sentence.  
                             //Rename AddNewPaymentTypeMedicareSequesterIfNotNull to AddNewPaymentTypeMedicareSequesterValueTest
                             //Rename objPaymentType to paymentTypeMedicareSequester
        /// <summary>
        /// Adds the payment type medicare sequester value if not null
        /// </summary>
        [TestMethod]
        public void AddNewPaymentTypeMedicareSequesterValueTest()
        {
            var mockAddNewPaymentTypeMedicareSequester = new Mock<IPaymentTypeMedicareSequesterRepository>();
            mockAddNewPaymentTypeMedicareSequester.Setup(f => f.AddEditPaymentTypeMedicareSequester(It.IsAny<PaymentTypeMedicareSequester>())).Returns(1);
            PaymentTypeMedicareSequesterLogic target = new PaymentTypeMedicareSequesterLogic(mockAddNewPaymentTypeMedicareSequester.Object);
            PaymentTypeMedicareSequester paymentTypeMedicareSequester = new PaymentTypeMedicareSequester { PaymentTypeDetailId = 1 };
            long actual = target.AddEditPaymentType(paymentTypeMedicareSequester);
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Get payment type medicare sequester
        /// </summary>
        //FIXED-2016-R3-S2: Rename result to expectedResult. Create expectedResult object with value of properties and compare from actual result.
        //                   Rename GetPaymentTypeMedicareSequesterTestIfNull to GetPaymentTypeMedicareSequesterIfParameterIsNullTest   
        [TestMethod]
        public void GetPaymentTypeMedicareSequesterIfParameterIsNullTest()
        {
            //Mock Input
            PaymentTypeMedicareSequester paymentTypePercentageDiscount = new PaymentTypeMedicareSequester { PaymentTypeId = 0, ContractId = 0, ServiceTypeId = 0 };
            //Mock output 
            PaymentTypeMedicareSequester expectedResult = new PaymentTypeMedicareSequester { Percentage = 0, PaymentTypeDetailId = 0 };
            var mockGetPaymentTypeMedicareSequester = new Mock<IPaymentTypeMedicareSequesterRepository>();
            mockGetPaymentTypeMedicareSequester.Setup(f => f.GetPaymentTypeMedicareSequester(paymentTypePercentageDiscount)).Returns(expectedResult);
            PaymentTypeMedicareSequesterLogic target = new PaymentTypeMedicareSequesterLogic(mockGetPaymentTypeMedicareSequester.Object);
            PaymentTypeMedicareSequester actual = (PaymentTypeMedicareSequester)target.GetPaymentType(paymentTypePercentageDiscount);
            Assert.AreEqual(expectedResult.Percentage, actual.Percentage);
            Assert.AreEqual(expectedResult.PaymentTypeDetailId, actual.PaymentTypeDetailId);
        }

        /// <summary>
        /// Get payment type medicare sequester with valid parameters
        /// </summary>
        //FIXED-2016-R3-S2: Rename result to expectedResult. Create expectedResult object with value of properties and compare from actual result.
        //                   Rename GetPaymentTypeMedicareSequesterTestIfNotNull to GetPaymentTypeMedicareSequesterTest  
        [TestMethod]
        public void GetPaymentTypeMedicareSequesterTest()
        {
            //Mock Input
            PaymentTypeMedicareSequester paymentTypePercentageDiscount = new PaymentTypeMedicareSequester { PaymentTypeId = 15, ContractId = 345, ServiceTypeId = null };

            //Mock output 
            PaymentTypeMedicareSequester expectedResult = new PaymentTypeMedicareSequester { Percentage = 43.56, PaymentTypeDetailId = 234 };
            var mockGetPaymentTypeMedicareSequester = new Mock<IPaymentTypeMedicareSequesterRepository>();
            mockGetPaymentTypeMedicareSequester.Setup(f => f.GetPaymentTypeMedicareSequester(paymentTypePercentageDiscount)).Returns(expectedResult);
            PaymentTypeMedicareSequesterLogic target = new PaymentTypeMedicareSequesterLogic(mockGetPaymentTypeMedicareSequester.Object);
            PaymentTypeMedicareSequester actual = (PaymentTypeMedicareSequester)target.GetPaymentType(paymentTypePercentageDiscount);
            Assert.AreEqual(expectedResult.Percentage, actual.Percentage);
            Assert.AreEqual(expectedResult.PaymentTypeDetailId, actual.PaymentTypeDetailId);
        }

        /// <summary>
        /// Evaluates if patient responsibility is not null test.
        /// </summary>
        [TestMethod]
        public void EvaluateIfPatientResponsibilityIsNotNullTest()
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
            Mock<IPaymentTypeMedicareSequesterRepository> paymentTypeMedicareSequesterRepository = new Mock<IPaymentTypeMedicareSequesterRepository>();
            paymentTypeMedicareSequesterRepository.Setup(x => x.GetPaymentTypeMedicareSequester(It.IsAny<PaymentTypeMedicareSequester>()))
                .Returns(new PaymentTypeMedicareSequester());

            PaymentTypeMedicareSequester paymentTypeMedicareSequester = new PaymentTypeMedicareSequester
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
                Percentage = 50
            };

            _target = new PaymentTypeMedicareSequesterLogic(paymentTypeMedicareSequesterRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareSequester
            };

            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                PatientResponsibility = 780
            };
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
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(-335, firstOrDefault.MedicareSequesterAmount);
            if (firstOrDefault != null)
                Assert.AreEqual(445, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates if patient responsibility is zero test.
        /// </summary>
        [TestMethod]
        public void EvaluateIfPatientResponsibilityIsZeroTest()
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
            Mock<IPaymentTypeMedicareSequesterRepository> paymentTypeMedicareSequesterRepository = new Mock<IPaymentTypeMedicareSequesterRepository>();
            paymentTypeMedicareSequesterRepository.Setup(x => x.GetPaymentTypeMedicareSequester(It.IsAny<PaymentTypeMedicareSequester>()))
                .Returns(new PaymentTypeMedicareSequester());

            PaymentTypeMedicareSequester paymentTypeMedicareSequester = new PaymentTypeMedicareSequester
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
                Percentage = 50
            };

            _target = new PaymentTypeMedicareSequesterLogic(paymentTypeMedicareSequesterRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareSequester
            };

            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                PatientResponsibility = 0
            };
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
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110, ClaimTotalCharges = 110, ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110, ClaimTotalCharges=110, ServiceTypeId = 1}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                 .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(55, firstOrDefault.MedicareSequesterAmount);
            if (firstOrDefault != null)
                Assert.AreEqual(55, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates if adjudicated value is null test.
        /// </summary>
        [TestMethod]
        public void EvaluateIfAdjudicatedValueIsNullTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,AdjudicatedValue = null,ClaimTotalCharges = 110,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ClaimTotalCharges = 110,ServiceTypeId = 1}
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeMedicareSequesterRepository> paymentTypeMedicareSequesterRepository = new Mock<IPaymentTypeMedicareSequesterRepository>();
            paymentTypeMedicareSequesterRepository.Setup(x => x.GetPaymentTypeMedicareSequester(It.IsAny<PaymentTypeMedicareSequester>()))
                .Returns(new PaymentTypeMedicareSequester());

            PaymentTypeMedicareSequester paymentTypeMedicareSequester = new PaymentTypeMedicareSequester
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
                Percentage = 50
            };

            _target = new PaymentTypeMedicareSequesterLogic(paymentTypeMedicareSequesterRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareSequester
            };

            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                PatientResponsibility = 0
            };
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
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                 .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.IsNull(firstOrDefault.MedicareSequesterAmount);
            if (firstOrDefault != null)
                Assert.IsNull(firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates if adjudicated value is not null test.
        /// </summary>
        [TestMethod]
        public void EvaluateIfAdjudicatedValueIsNotNullTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123,AdjudicatedValue = 350,ClaimTotalCharges = 110,ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = 85,ClaimTotalCharges = 110,ServiceTypeId = 1}
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeMedicareSequesterRepository> paymentTypeMedicareSequesterRepository = new Mock<IPaymentTypeMedicareSequesterRepository>();
            paymentTypeMedicareSequesterRepository.Setup(x => x.GetPaymentTypeMedicareSequester(It.IsAny<PaymentTypeMedicareSequester>()))
                .Returns(new PaymentTypeMedicareSequester());

            PaymentTypeMedicareSequester paymentTypeMedicareSequester = new PaymentTypeMedicareSequester
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
                Percentage = 50
            };

            _target = new PaymentTypeMedicareSequesterLogic(paymentTypeMedicareSequesterRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareSequester
            };

            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                PatientResponsibility = 25
            };
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
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                 .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = _target.EvaluatePaymentType(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(162.5, firstOrDefault.MedicareSequesterAmount);
            if (firstOrDefault != null)
                Assert.AreEqual(187.5, firstOrDefault.AdjudicatedValue);
        }
    }
}
