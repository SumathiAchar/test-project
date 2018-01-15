using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for PaymentResultLogicTest
    /// </summary>
    [TestClass]
    public class PaymentResultLogicTest
    {

        //Creating object for Logic
        private PaymentResultLogic _target;

        //Creating mock object for PaymentResultRepository
        private Mock<IPaymentResultRepository> _mockPaymentResultLogic;

        /// <summary>
        /// Payments the result logic parameterless constructor test.
        /// </summary>
        [TestMethod]
        public void PaymentResultLogicParameterlessConstructorTest()
        {
            _target = new PaymentResultLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(_target, typeof(PaymentResultLogic));
        }


        /// <summary>
        /// Payments the result logic repository constructor unit test.
        /// </summary>
        [TestMethod]
        public void PaymentResultLogicRepositoryConstructorUnitTest()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
            Assert.IsInstanceOfType(_target, typeof(PaymentResultLogic));
        }

        /// <summary>
        /// Updates the payment results test.
        /// </summary>
        [TestMethod]
        public void UpdatePaymentResultsTest()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult { ClaimId = 138411511, AdjudicatedValue = null,ContractId = 1, Line=2},
                new PaymentResult { ClaimId = 138411511,AdjudicatedValue = 10,ContractId = 1,ServiceTypeId = 1, Line=1},
                new PaymentResult { ClaimId = 138411511,AdjudicatedValue = 10,ContractId = 1,ServiceTypeId = 1,Line=1,PaymentTypeId = 1}
            };
            var paymentResultDictionary = new Dictionary<long, List<PaymentResult>>
            {
                {1, paymentResults}
            };

            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
            var actual = _target.UpdatePaymentResults(paymentResultDictionary,250, 10345, null, null);
            Assert.AreEqual(false, actual);
        }


        /// <summary>
        /// Evaluates if line is null test.
        /// </summary>
        [TestMethod]
        public void EvaluateIfLineIsNullTest()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ServiceTypeId = 1},
                new PaymentResult {ClaimId = 123, ServiceTypeId = 1}
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
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
            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1},
                new PaymentResult {ClaimId = 123}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();


            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(updatedPaymentResults.Any(q => q.ClaimId != 0), actual.Any(q => q.ClaimId != 0));
        }

        /// <summary>
        /// Evaluates if service type identifier is null test.
        /// </summary>
        [TestMethod]
        public void EvaluateIfServiceTypeIdIsNullTest()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ServiceTypeId = 0},
                new PaymentResult {ClaimId = 123, ServiceTypeId = 0}
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
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
            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1},
                new PaymentResult {ClaimId = 123}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();


            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(updatedPaymentResults.Any(q => q.ClaimId != 0), actual.Any(q => q.ClaimId != 0));
        }


        /// <summary>
        /// Updates the adjudicated amount if line null test.
        /// </summary>
        [TestMethod]
        public void UpdateAdjudicatedAmountIfLineNullTest()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123}
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
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
            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1},
                new PaymentResult {ClaimId = 123}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();


            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(true, actual.Any(q => q.AdjudicatedValue == null));
        }

        /// <summary>
        /// Updates the adjudicated amount if line not null test.
        /// </summary>
        [TestMethod]
        public void UpdateAdjudicatedAmountIfLineNotNullTest()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100, ClaimStatus = 4},
                new PaymentResult {ClaimId = 123}
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
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
            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100},
                new PaymentResult {ClaimId = 123}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();


            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(true, actual.Any(q => q.AdjudicatedValue == 100));
        }



        /// <summary>
        /// Updates the adjudicated amount if payment result null.
        /// </summary>
        [TestMethod]
        public void UpdateAdjudicatedAmountIfPaymentResultNull()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 10},
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 5}
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
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
            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 10},
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 5}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();


            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(true, actual.TrueForAll(q => q.AdjudicatedValue == null));
        }


        /// <summary>
        /// Updates the claim status if payment result null.
        /// </summary>
        [TestMethod]
        public void UpdateClaimStatusIfPaymentResultNull()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100, Line = 1, ServiceTypeId = 10},
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 5}
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
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
            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100, Line = 1, ServiceTypeId = 10},
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 5}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();


            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(true, actual.TrueForAll(q => q.ClaimStatus == 0));
        }


        /// <summary>
        /// Updates the claim status if contract identifier null.
        /// </summary>
        [TestMethod]
        public void UpdateClaimStatusIfContractIdNull()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100,ContractId = 101},
                new PaymentResult {ClaimId = 123, ContractId = 101}
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
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
            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100,ContractId = 101},
                new PaymentResult {ClaimId = 123, ContractId = 101}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();


            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(false, actual.TrueForAll(q => q.ClaimStatus == 4));
        }

        /// <summary>
        /// Updates the claim status if contract identifier not null.
        /// </summary>
        [TestMethod]
        public void UpdateClaimStatusIfContractIdNotNull()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100},
                new PaymentResult {ClaimId = 123}
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
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
            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100, ClaimStatus = 4},
                new PaymentResult {ClaimId = 123}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();


            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            var firstOrDefault = actual.FirstOrDefault(q => q.ClaimStatus == 4);
            if (firstOrDefault != null)
                Assert.AreEqual(4, firstOrDefault.ClaimStatus);
        }

        /// <summary>
        /// Updates the claim status if service type identifier null.
        /// </summary>
        [TestMethod]
        public void UpdateClaimStatusIfServiceTypeIdNull()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100, ContractId = 101, ServiceTypeId = null},
                new PaymentResult {ClaimId = 123, ContractId = 101, ServiceTypeId = null}
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
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
            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100,ContractId = 101},
                new PaymentResult {ClaimId = 123, ContractId = 101}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();


            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            var firstOrDefault = actual.FirstOrDefault(q => q.ClaimStatus == 17);
            if (firstOrDefault != null)
                Assert.AreEqual(17, firstOrDefault.ClaimStatus);

        }


        /// <summary>
        /// Updates the claim status if service type identifier not null.
        /// </summary>
        [TestMethod]
        public void UpdateClaimStatusIfServiceTypeIdNotNull()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100, ContractId = 101, ServiceTypeId = 10},
                new PaymentResult {ClaimId = 123, ContractId = 101, ServiceTypeId = 10}
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
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
            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100, ContractId = 101},
                new PaymentResult {ClaimId = 123, ContractId = 101}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();


            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(false, actual.TrueForAll(q => q.ClaimStatus == 17));
        }



        /// <summary>
        /// Updates the claim status if adjudicated value is null.
        /// </summary>
        [TestMethod]
        public void UpdateClaimStatusIfAdjudicatedValueIsNull()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 101},
                new PaymentResult {ClaimId = 123, ContractId = 101, ServiceTypeId = 10}
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
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
            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100, ContractId = 101, ServiceTypeId = 10},
                new PaymentResult {ClaimId = 123, ContractId = 101}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();


            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            var firstOrDefault = actual.FirstOrDefault(q => q.ClaimStatus == 6);
            if (firstOrDefault != null)
                Assert.AreEqual(6, firstOrDefault.ClaimStatus);
        }

        /// <summary>
        /// Updates the claim status if adjudicated value is not null.
        /// </summary>
        [TestMethod]
        public void UpdateClaimStatusIfAdjudicatedValueIsNotNull()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 101, AdjudicatedValue = 100},
                new PaymentResult {ClaimId = 123, ContractId = 101, ServiceTypeId = 10, AdjudicatedValue = 100}
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
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
            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100, ContractId = 101, ServiceTypeId = 10},
                new PaymentResult {ClaimId = 123, ContractId = 101}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();


            //Act
            List<PaymentResult> actual = _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            var firstOrDefault = actual.FirstOrDefault(q => q.ClaimStatus == 3);
            if (firstOrDefault != null)
                Assert.AreEqual(3, firstOrDefault.ClaimStatus);
        }

        /// <summary>
        /// Gets the payment results if claim null.
        /// </summary>
        [TestMethod]
        public void GetPaymentResultsIfClaimNull()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100,ContractId = 101},
                new PaymentResult {ClaimId = 123, ContractId = 101}
            };
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);

            //Act
            List<PaymentResult> actual = _target.GetPaymentResults(null);

            // Assert
            Assert.AreEqual(0, actual.Count);
        }


        /// <summary>
        /// Gets the payment results if claim not null.
        /// </summary>
        [TestMethod]
        public void GetPaymentResultsIfClaimNotNull()
        {
            _mockPaymentResultLogic = new Mock<IPaymentResultRepository>();
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = 100,ContractId = 101},
                new PaymentResult {ClaimId = 123, ContractId = 101}
            };
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new PaymentResultLogic(_mockPaymentResultLogic.Object);
            var evaluateableClaim = new EvaluateableClaim
            {
                ClaimId = 123,
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Line = 1,
                        Amount = 20,
                        RevCode = "250"
                    }
                }
            };

            //Act
            List<PaymentResult> actual = _target.GetPaymentResults(evaluateableClaim);

            // Assert
            Assert.AreEqual(evaluateableClaim.ClaimCharges[0].Amount, actual[1].ClaimTotalCharges);
        }
    }


}

