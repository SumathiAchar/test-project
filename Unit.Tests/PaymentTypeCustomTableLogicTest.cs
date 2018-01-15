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
    [TestClass]
    public class PaymentTypeCustomTableLogicTest
    {
        /// <summary>
        /// Payments the type custom table logic constructor test.
        /// </summary>
        [TestMethod]
        public void PaymentTypeCustomTableLogicConstructorTest()
        {
            var target = new PaymentTypeCustomTableLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTypeCustomTableLogic));
        }

        /// <summary>
        /// Payments the type custom table logic constructor test2.
        /// </summary>
        [TestMethod]
        public void PaymentTypeCustomTableLogicConstructorTest2()
        {
            Mock<IPaymentTypeCustomTableRepository> mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            PaymentTypeCustomTableLogic target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object);
            Assert.IsInstanceOfType(target, typeof(PaymentTypeCustomTableLogic));
        }

        /// <summary>
        /// Gets the headers test.
        /// </summary>
        [TestMethod]
        public void GetHeadersTest()
        {
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            const int docId = 10110;
            const string result = "A,B";
            mockPaymentTypeCustomTable.Setup(f => f.GetHeaders(docId)).Returns(result);
            PaymentTypeCustomTableLogic target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object);
            string actual = target.GetHeaders(docId);
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Adds/edit test for custom table.
        /// </summary>
        [TestMethod]
        public void AddEditCustomTableUnitTest()
        {
            //Arrange
            Mock<IPaymentTypeCustomTableRepository> mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 3, 
                DocumentId = 10110, 
                Expression = "CAA",
                MultiplierFirst = null,
                MultiplierSecond = null,
                MultiplierThird = null,
                MultiplierFourth = null,
                MultiplierOther = null,
                IsObserveServiceUnit = false,
                ObserveServiceUnitLimit = null,
                IsPerDayOfStay = false,
                IsPerCode = false
            };
            const long expectedResult = 83;
            mockPaymentTypeCustomTable.Setup(f => f.AddEdit(It.IsAny<PaymentTypeCustomTable>())).Returns(expectedResult);
            PaymentTypeCustomTableLogic target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object);
            //Act
            long actual = target.AddEditPaymentType(paymentTypeCustomTable);
            //Assert
            Assert.AreEqual(actual, expectedResult);
        }


        /// <summary>
        /// Gets the payment type custom table test.
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeCustomTableUnitTest()
        {
            //Arrange
            Mock<IPaymentTypeCustomTableRepository> mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable { ServiceTypeId = 127, ContractId = 0, PaymentTypeId = 14 };
            PaymentTypeCustomTable expectedResult = new PaymentTypeCustomTable
            {
                ClaimFieldId = 3, 
                DocumentId = 10110, 
                Expression = "CAA",
                MultiplierFirst = null,
                MultiplierSecond = null,
                MultiplierThird = null,
                MultiplierFourth = null,
                MultiplierOther = null,
                IsObserveServiceUnit = false,
                ObserveServiceUnitLimit = null,
                IsPerDayOfStay = false,
                IsPerCode = false
            };
            mockPaymentTypeCustomTable.Setup(f => f.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(expectedResult);
            PaymentTypeCustomTableLogic target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object);
            //Act
            PaymentTypeCustomTable actual = (PaymentTypeCustomTable)target.GetPaymentType(paymentTypeCustomTable);
            //Assert
            Assert.AreEqual(actual, expectedResult);
        }

        /// <summary>
        /// Evaluates the evaluate line level for carve out test.
        /// </summary>
        [TestMethod]
        public void EvaluateEvaluateLineLevelForCarveOutTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, Line = 1,AdjudicatedValue = 51, ClaimTotalCharges = 347.0, ContractId = 67},
                new PaymentResult{AdjudicatedValue = 1.1, ClaimId = 411930180, ClaimStatus = 3, ClaimTotalCharges = 22.0, ContractId = 67, Line=1}
          };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 3,
                DocumentId = 10110,
                Expression = "5*2",
                ServiceTypeId = 127,
                ContractId = 0,
                PaymentTypeId = 14
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = null,
                    ContractId = 67,
                    PaymentTypeId = 14,
                    Expression = "5*2",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 21, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "1", Value = "1" } } },
                    ClaimFieldId = 3
                }
            };

            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimId = 640321;
            evaluateableClaim.ClaimTotal = 347.0;
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
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415"
                }
            };
            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 411930180, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            //var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(51, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the evaluate line level for valid line ids test.
        /// </summary>
        [TestMethod]
        public void EvaluateEvaluateLineLevelForValidLineIdsTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, Line = 1,AdjudicatedValue = 51, ClaimTotalCharges = 347.0, ContractId = 67, ServiceTypeId = 1},
                new PaymentResult{AdjudicatedValue = 1.1, ClaimId = 411930180, ClaimStatus = 3, ClaimTotalCharges = 22.0, ContractId = 67, Line=1, ServiceTypeId = 2}
          };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 3,
                DocumentId = 10110,
                Expression = "5*2",
                ServiceTypeId = 127,
                ContractId = 0,
                PaymentTypeId = 14
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = null,
                    ContractId = 67,
                    PaymentTypeId = 14,
                    Expression = "5*2",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 21, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "300", Value = "1" } } },
                    ClaimFieldId = 3,
                    ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },

                }

            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimId = 640321;
            evaluateableClaim.ClaimTotal = 347.0;
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
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415"
                }
            };
            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 411930180, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(51, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the evaluate line level for valid line ids with HCPCS returns true test.
        /// </summary>
        [TestMethod]
        public void EvaluateEvaluateLineLevelForValidLineIdsWithHcpcsReturnsTrueTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, Line = 1,AdjudicatedValue = 51, ClaimTotalCharges = 347.0, ContractId = 67, ServiceTypeId = 1},
                new PaymentResult{AdjudicatedValue = 1.1, ClaimId = 411930180, ClaimStatus = 3, ClaimTotalCharges = 22.0, ContractId = 67, Line=1, ServiceTypeId = 2}
          };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 4,
                DocumentId = 10110,
                Expression = "5*2",
                ServiceTypeId = 127,
                ContractId = 0,
                PaymentTypeId = 14
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = null,
                    ContractId = 67,
                    PaymentTypeId = 14,
                    Expression = "5*2",
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
                    ClaimFieldId = 4,
                    ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 }
                }

            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimId = 640321;
            evaluateableClaim.ClaimTotal = 347.0;
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
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "30001",
                    HcpcsCodeWithModifier = "3000126",
                    HcpcsModifiers = "26"
                }
            };
            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 411930180, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(51, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the evaluate line level for valid line ids with HCPCS returns false test.
        /// </summary>
        [TestMethod]
        public void EvaluateEvaluateLineLevelForValidLineIdsWithHcpcsReturnsFalseTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, Line = 1,AdjudicatedValue = 51, ClaimTotalCharges = 347.0, ContractId = 67, ServiceTypeId = 1},
                new PaymentResult{AdjudicatedValue = 1.1, ClaimId = 411930180, ClaimStatus = 3, ClaimTotalCharges = 22.0, ContractId = 67, Line=1, ServiceTypeId = 2}
          };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 4,
                DocumentId = 10110,
                Expression = "5*2",
                ServiceTypeId = 127,
                ContractId = 0,
                PaymentTypeId = 14
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = null,
                    ContractId = 67,
                    PaymentTypeId = 14,
                    Expression = "5*2",
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
                    ClaimFieldId = 4,
                    ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 }
                }

            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimId = 640321;
            evaluateableClaim.ClaimTotal = 347.0;
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
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "30001",
                    HcpcsCodeWithModifier = "30001261",
                    HcpcsModifiers = "261"
                }
            };
            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 411930180, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(51, firstOrDefault.AdjudicatedValue);
        }


        /// <summary>
        /// Evaluates the claim level test.
        /// </summary>
        [TestMethod]
        public void EvaluateClaimLevelTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, Line = 1,AdjudicatedValue = 51, ClaimTotalCharges = 347.0, ContractId = 67, ServiceTypeId = 1},
                new PaymentResult{AdjudicatedValue = 1.1, ClaimId = 411930180, ClaimStatus = 3, ClaimTotalCharges = 22.0, ContractId = 67, Line=1, ServiceTypeId = 2}
          };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 2,
                DocumentId = 10110,
                Expression = "5*2",
                ServiceTypeId = 127,
                ContractId = 0,
                PaymentTypeId = 14
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = null,
                    ContractId = 67,
                    PaymentTypeId = 14,
                    Expression = "5*2",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 2, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "131", Value = "1" } } },
                    ClaimFieldId = 2,
                    ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },

                }

            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimId = 640321;
            evaluateableClaim.ClaimTotal = 347.0;
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
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415"
                }
            };
            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 411930180, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.AdjudicatedValue != null).ToList().FirstOrDefault();
            //var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(51, firstOrDefault.AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the claim level unadjudicated test.
        /// </summary>
        [TestMethod]
        public void EvaluateClaimLevelUnadjudicatedTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, Line = 1,AdjudicatedValue = 51, ClaimTotalCharges = 347.0, ContractId = 67, ServiceTypeId = 1},
                new PaymentResult{AdjudicatedValue = 1.1, ClaimId = 411930180, ClaimStatus = 3, ClaimTotalCharges = 22.0, ContractId = 67, Line=1, ServiceTypeId = 2}
          };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 2,
                DocumentId = 10110,
                Expression = "5*2",
                ServiceTypeId = 127,
                ContractId = 0,
                PaymentTypeId = 14
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = null,
                    ContractId = 67,
                    PaymentTypeId = 14,
                    Expression = "5*2",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 2, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "131", Value = "FFF" } } },
                    ClaimFieldId = 2,
                    ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },

                }

            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimId = 640321;
            evaluateableClaim.ClaimTotal = 347.0;
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
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415"
                }
            };
            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 411930180, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.ClaimStatus == 1).ToList().FirstOrDefault();
            //var firstOrDefault = paymentResults.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(1, firstOrDefault.ClaimStatus);
        }

        /// <summary>
        /// Evaluates the evaluate line level un adjudicated test.
        /// </summary>
        [TestMethod]
        public void EvaluateEvaluateLineLevelUnadjudicatedTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, Line = 1,AdjudicatedValue = 51, ClaimTotalCharges = 347.0, ContractId = 67, ServiceTypeId = 1},
                new PaymentResult{AdjudicatedValue = 1.1, ClaimId = 411930180, ClaimStatus = 3, ClaimTotalCharges = 22.0, ContractId = 67, Line=1, ServiceTypeId = 2}
          };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 3,
                DocumentId = 10110,
                Expression = "5*2",
                ServiceTypeId = 127,
                ContractId = 0,
                PaymentTypeId = 14
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = null,
                    ContractId = 67,
                    PaymentTypeId = 14,
                    Expression = "5*2",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 21, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "300", Value = "FFF" } } },
                    ClaimFieldId = 3,
                    ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },

                }

            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimId = 640321;
            evaluateableClaim.ClaimTotal = 347.0;
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
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415"
                }
            };
            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 411930180, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.Where(x => x.ClaimStatus == 1).ToList().FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual(1, firstOrDefault.ClaimStatus);
        }

        /// <summary>
        /// Evaluates the adjudicated los is equal test.
        /// </summary>
        [TestMethod]
        public void EvaluateAdjudicatedLosIsEqualTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 412061201, Line = 1,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1}                
            };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 53,
                DocumentId = 10110,
                Expression = "10+100",
                ServiceTypeId = 422749,
                ContractId = 254079,
                PaymentTypeId = 14
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = 422749,
                    ContractId = 254079,
                    PaymentTypeId = 14,
                    Expression = "10+100",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 21, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "10", Value = "100" } } },
                    ClaimFieldId = 53,
                    ValidLineIds = null,

                }

            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimId = 640321;
            evaluateableClaim.ClaimTotal = 347.0;
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
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415"
                }
            };
            evaluateableClaim.Los = 10;

            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 412061201, ContractId = 254079,AdjudicatedValue = 110},                
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert  
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(actual[0].AdjudicatedValue, paymentResults[0].AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the adjudicated age is equal test.
        /// </summary>
        [TestMethod]
        public void EvaluateAdjudicatedAgeIsEqualTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 412061201, Line = 1,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1}                
            };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 54,
                DocumentId = 10110,
                Expression = "78+100",
                ServiceTypeId = 422749,
                ContractId = 254079,
                PaymentTypeId = 14,
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = 422749,
                    ContractId = 254079,
                    PaymentTypeId = 14,
                    Expression = "78+100",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 54, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "78", Value = "100" } } },
                    ClaimFieldId = 54,
                    ValidLineIds = null,

                }

            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimId = 640321;
            evaluateableClaim.ClaimTotal = 347.0;
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
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415"
                }
            };
            evaluateableClaim.Age = 78;

            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 412061201, ContractId = 254079,AdjudicatedValue = 110},                
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert  
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(actual[0].AdjudicatedValue, paymentResults[0].AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the adjudicated los is not equal test.
        /// </summary>
        [TestMethod]
        public void EvaluateAdjudicatedLosIsNotEqualTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 412061201, Line = 1,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1}                
            };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 53,
                DocumentId = 10110,
                Expression = "10+100",
                ServiceTypeId = 422749,
                ContractId = 254079,
                PaymentTypeId = 14
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = 422749,
                    ContractId = 254079,
                    PaymentTypeId = 14,
                    Expression = "10+100",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 21, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "10", Value = "100" } } },
                    ClaimFieldId = 53,
                    ValidLineIds = null,
                }
            };

            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimId = 640321;
            evaluateableClaim.ClaimTotal = 347.0;
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
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415"
                }
            };
            evaluateableClaim.Los = 10;

            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 412061201, ContractId = 254079,AdjudicatedValue = 110},                
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert            
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(actual[0].AdjudicatedValue, paymentResults[0].AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the adjudicated age is not equal test.
        /// </summary>
        [TestMethod]
        public void EvaluateAdjudicatedAgeIsNotEqualTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 412061201, Line = 1,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1}                
            };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 54,
                DocumentId = 10110,
                Expression = "78+100",
                ServiceTypeId = 422749,
                ContractId = 254079,
                PaymentTypeId = 14
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = 422749,
                    ContractId = 254079,
                    PaymentTypeId = 14,
                    Expression = "78+100",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 21, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "78", Value = "100" } } },
                    ClaimFieldId = 54,
                    ValidLineIds = null,
                }
            };

            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimId = 640321;
            evaluateableClaim.ClaimTotal = 347.0;
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
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415"
                }
            };
            evaluateableClaim.Age = 78;

            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 412061201, ContractId = 254079,AdjudicatedValue = 110},                
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert            
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(actual[0].AdjudicatedValue, paymentResults[0].AdjudicatedValue);
        }

        /// <summary>
        /// Add Edit PaymentType with parameter as null
        /// </summary>
        [TestMethod]
        public void AddEditPaymentTypeIfNullTest()
        {
            // Arrange
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            const int returnId = 1;
            mockPaymentTypeCustomTable.Setup(f => f.AddEdit(It.IsAny<PaymentTypeCustomTable>())).Returns(returnId);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object);
            
            // Act
            long actual = target.AddEditPaymentType(null);
            
            // Assert
            Assert.AreEqual(actual, returnId);
        }

        /// <summary>
        /// Get PaymentType CustomTable with Null Test
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeCustomTableIfNullTest()
        {
            // Arrange
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(f => f.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns((PaymentTypeCustomTable) null);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object);
           
            // Act
            var actual = (PaymentTypeCustomTable)target.GetPaymentType(null);
           
            // Assert
            Assert.IsNull(actual);
        }

        /// <summary>
        /// Adds the edit test for hcpcs/rate/hipps,place of service and revenue code.
        /// </summary>
        [TestMethod]
        public void AddEditCustomTableWithHcpcsPosRevenueCodeUnitTest()
        {
            // Arrange
            Mock<IPaymentTypeCustomTableRepository> mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 3, 
                DocumentId = 49840,
                Expression = " T_PRIYA2 ",
                MultiplierFirst = " T_PRIYA2 ",
                MultiplierSecond = " T_PRIYA2 ",
                MultiplierThird = " T_PRIYA2 ",
                MultiplierFourth = " T_PRIYA2 ",
                MultiplierOther = " T_PRIYA2 ",
                IsObserveServiceUnit = true,
                ObserveServiceUnitLimit = " T_PRIYA2 ",
                IsPerDayOfStay = true,
                IsPerCode = true
            };
            const long expectedResult = 83;
            mockPaymentTypeCustomTable.Setup(f => f.AddEdit(It.IsAny<PaymentTypeCustomTable>())).Returns(expectedResult);
            PaymentTypeCustomTableLogic target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object);
            // Act
            long actual = target.AddEditPaymentType(paymentTypeCustomTable);
            // Assert
            Assert.AreEqual(actual, expectedResult);
        }

        /// <summary>
        /// Gets the payment type custom table test for hcpcs/rate/hipps,place of service and revenue code.
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeCustomTableWithHcpcsPosRevenueCodeUnitTest()
        {
            // Arrange
            Mock<IPaymentTypeCustomTableRepository> mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ServiceTypeId = 231813, 
                ContractId = 0, 
                PaymentTypeId = 14
            };
            PaymentTypeCustomTable expectedResult = new PaymentTypeCustomTable
            {
                ClaimFieldId = 3,
                DocumentId = 49840,
                Expression = " T_PRIYA2 ",
                MultiplierFirst = " T_PRIYA2 ",
                MultiplierSecond = " T_PRIYA2 ",
                MultiplierThird = " T_PRIYA2 ",
                MultiplierFourth = " T_PRIYA2 ",
                MultiplierOther = " T_PRIYA2 ",
                IsObserveServiceUnit = true,
                ObserveServiceUnitLimit = " T_PRIYA2 ",
                IsPerDayOfStay = true,
                IsPerCode = true
            };
            mockPaymentTypeCustomTable.Setup(f => f.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(expectedResult);
            PaymentTypeCustomTableLogic target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object);
            // Act
            PaymentTypeCustomTable actual = (PaymentTypeCustomTable)target.GetPaymentType(paymentTypeCustomTable);
            // Assert
            Assert.AreEqual(actual, expectedResult);
        }

        /// <summary>
        /// Evaluates the adjudicated is per code test.
        /// </summary>
        [TestMethod]
        public void EvaluateAdjudicatedIsPerCodeTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                 new PaymentResult {ClaimId = 412061201, Line = 1,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 2,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1}  
            };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 3,
                DocumentId = 10110,
                Expression = "5*2",
                ServiceTypeId = 127,
                ContractId = 0,
                PaymentTypeId = 14,
                IsPerCode = true,
                MultiplierFirst = "2",
                MultiplierSecond = "1",
                MultiplierThird = "1",
                MultiplierFourth= "2",
                MultiplierOther = "5"
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = 127,
                    ContractId = 67,
                    PaymentTypeId = 14,
                    Expression = "T_B",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 4, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "36415", Value = "100" } } },
                    ClaimFieldId = 4,
                    ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                    IsPerCode = true,
                    MultiplierFirst = "2",
                    MultiplierSecond = "1",
                    MultiplierThird = "1",
                    MultiplierFourth = "2",
                    MultiplierOther = "5"
                }

            };

            IEvaluateableClaim evaluateableClaim = GetEvaluateableClaim();

            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180,Line=1, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 411930180, Line = 2, ContractId = 1,AdjudicatedValue = 110}
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert            
            Assert.AreEqual(1100, actual[2].AdjudicatedValue);
            Assert.AreEqual(0.00, actual[3].AdjudicatedValue);
        }

        /// <summary>
        /// Evaluates the adjudicated is perday of stay test.
        /// </summary>
        [TestMethod]
        public void EvaluateAdjudicatedIsPerdayOfStayTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                 new PaymentResult {ClaimId = 412061201, Line = 1,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 2,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 3,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 4,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 5,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1}  
            };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 3,
                DocumentId = 10110,
                Expression = "T_B",
                ServiceTypeId = 127,
                ContractId = 0,
                PaymentTypeId = 14,
                IsPerDayOfStay = true,
                MultiplierFirst = "2",
                MultiplierSecond = "1",
                MultiplierThird = "3+1",
                MultiplierFourth = "3",
                MultiplierOther = "4"
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = null,
                    ContractId = 67,
                    PaymentTypeId = 14,
                    Expression = "T_B",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 4, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "36415", Value = "100" } } },
                    ClaimFieldId = 4,
                    ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                    IsPerDayOfStay = true,
                    MultiplierFirst = "2",
                    MultiplierSecond = "1",
                    MultiplierThird = "3+1",
                    MultiplierFourth = "3",
                    MultiplierOther = "4"
                }

            };

            IEvaluateableClaim evaluateableClaim = GetEvaluateableClaim();

            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180,Line=1, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 411930180, Line = 2, ContractId = 1,AdjudicatedValue = 110}
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert            
            Assert.AreEqual(700, actual[5].AdjudicatedValue);
            Assert.AreEqual(700, actual[6].AdjudicatedValue);
            Assert.AreEqual(300, actual[7].AdjudicatedValue);
            Assert.AreEqual(0.00, actual[8].AdjudicatedValue);
        }

        [TestMethod]
        public void EvaluateAdjudicatedIsPerdayOfStayAndIsObservedServiceUnitTrueLimitNullTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                 new PaymentResult {ClaimId = 412061201, Line = 1,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 2,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 3,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 4,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 5,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1}  
            };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 3,
                DocumentId = 10110,
                Expression = "T_B",
                ServiceTypeId = 127,
                ContractId = 0,
                PaymentTypeId = 14,
                IsPerDayOfStay = true,
                MultiplierFirst = "2",
                MultiplierSecond = "1",
                MultiplierThird = "3+1",
                MultiplierFourth = "3",
                MultiplierOther = "4",
                IsObserveServiceUnit = true
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = null,
                    ContractId = 67,
                    PaymentTypeId = 14,
                    Expression = "T_B",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 4, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "36415", Value = "100" } } },
                    ClaimFieldId = 4,
                    ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                    IsPerDayOfStay = true,
                    MultiplierFirst = "2",
                    MultiplierSecond = "1",
                    MultiplierThird = "3+1",
                    MultiplierFourth = "3",
                    MultiplierOther = "4",
                    IsObserveServiceUnit = true
                }

            };

            IEvaluateableClaim evaluateableClaim = GetEvaluateableClaim();

            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180,Line=1, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 411930180, Line = 2, ContractId = 1,AdjudicatedValue = 110}
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert            
            Assert.AreEqual(700, actual[5].AdjudicatedValue);
            Assert.AreEqual(700, actual[6].AdjudicatedValue);
            Assert.AreEqual(300, actual[7].AdjudicatedValue);
            Assert.AreEqual(0.00, actual[8].AdjudicatedValue);
        }

        [TestMethod]
        public void EvaluateAdjudicatedIsPerdayOfStayAndIsObservedServiceUnitTrueLimitNotNullTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                 new PaymentResult {ClaimId = 412061201, Line = 1,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 2,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 3,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 4,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 5,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1}  
            };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 3,
                DocumentId = 10110,
                Expression = "T_B",
                ServiceTypeId = 127,
                ContractId = 0,
                PaymentTypeId = 14,
                IsPerDayOfStay = true,
                MultiplierFirst = "2",
                MultiplierSecond = "1",
                MultiplierThird = "3+1",
                MultiplierFourth = "3",
                MultiplierOther = "4",
                IsObserveServiceUnit = true,
                ObserveServiceUnitLimit = "2"
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = null,
                    ContractId = 67,
                    PaymentTypeId = 14,
                    Expression = "T_B",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 4, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "36415", Value = "100" } } },
                    ClaimFieldId = 4,
                    ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                    IsPerDayOfStay = true,
                    MultiplierFirst = "2",
                    MultiplierSecond = "1",
                    MultiplierThird = "3+1",
                    MultiplierFourth = "3",
                    MultiplierOther = "4",
                    IsObserveServiceUnit = true,
                    ObserveServiceUnitLimit = "2"
                }

            };

            IEvaluateableClaim evaluateableClaim = GetEvaluateableClaim();

            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180,Line=1, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 411930180, Line = 2, ContractId = 1,AdjudicatedValue = 110}
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert            
            Assert.AreEqual(300, actual[5].AdjudicatedValue);
            Assert.AreEqual(300, actual[6].AdjudicatedValue);
            Assert.AreEqual(300, actual[7].AdjudicatedValue);
            Assert.AreEqual(0.0, actual[8].AdjudicatedValue);
        }

        [TestMethod]
        public void EvaluateAdjudicated_IsPerdayOfStay_AllMultipliers_Null_Test()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                 new PaymentResult {ClaimId = 412061201, Line = 1,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 2,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 3,AdjudicatedValue = 110, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                 new PaymentResult {ClaimId = 412061201, Line = 4,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1},
                  new PaymentResult {ClaimId = 412061201, Line = 5,AdjudicatedValue = 50, ClaimTotalCharges = 347.0, ContractId = 254079, ServiceTypeId = 422749,ClaimStatus = 1}  
            };
            PaymentTypeCustomTable paymentTypeCustomTable = new PaymentTypeCustomTable
            {
                ClaimFieldId = 3,
                DocumentId = 10110,
                Expression = "T_B",
                ServiceTypeId = 127,
                ContractId = 0,
                PaymentTypeId = 14,
                IsPerDayOfStay = true,
                MultiplierFirst = null,
                MultiplierSecond = null,
                MultiplierThird = null,
                MultiplierFourth = null,
                MultiplierOther = null
            };
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var mockPaymentTypeCustomTable = new Mock<IPaymentTypeCustomTableRepository>();
            mockPaymentTypeCustomTable.Setup(x => x.GetPaymentTypeCustomTableDetails(It.IsAny<PaymentTypeCustomTable>()))
                .Returns(paymentTypeCustomTable);
            var target = new PaymentTypeCustomTableLogic(mockPaymentTypeCustomTable.Object)
            {
                PaymentTypeBase = new PaymentTypeCustomTable
                {
                    ServiceTypeId = null,
                    ContractId = 67,
                    PaymentTypeId = 14,
                    Expression = "T_B",
                    ClaimFieldDoc = new ClaimFieldDoc { ClaimFieldDocId = 10110, ClaimFieldId = 4, ColumnHeaderFirst = "A", ColumnHeaderSecond = "B", ClaimFieldValues = new List<ClaimFieldValue> { new ClaimFieldValue { Identifier = "36415", Value = "100" } } },
                    ClaimFieldId = 4,
                    ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                    IsPerDayOfStay = true,
                    MultiplierFirst = null,
                    MultiplierSecond = null,
                    MultiplierThird = null,
                    MultiplierFourth = null,
                    MultiplierOther = null
                }

            };

            IEvaluateableClaim evaluateableClaim = GetEvaluateableClaim();

            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180,Line=1, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 411930180, Line = 2, ContractId = 1,AdjudicatedValue = 110}
            };
            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert            
            Assert.AreEqual(100, actual[5].AdjudicatedValue);
            Assert.AreEqual(200, actual[6].AdjudicatedValue);
            Assert.AreEqual(200, actual[7].AdjudicatedValue);
            Assert.AreEqual(0.00, actual[8].AdjudicatedValue);
        }

        private static IEvaluateableClaim GetEvaluateableClaim()
        {
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimId = 640321;
            evaluateableClaim.ClaimTotal = 347.0;
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
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415",
                    HcpcsCodeWithModifier = "36415",
                    ServiceFromDate = DateTime.Today.AddDays(3),
                    Units = 3
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 33,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415",
                    HcpcsCodeWithModifier = "36415",
                    ServiceFromDate = DateTime.Today,
                    Units = 2
                },
                 new ClaimCharge
                {
                    Line = 3,
                    Amount = 22,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415",
                    HcpcsCodeWithModifier = "36415",
                    ServiceFromDate = DateTime.Today.AddDays(2),
                    Units = 1
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 33,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415",
                    HcpcsCodeWithModifier = "36415",
                    ServiceFromDate = DateTime.Today.AddDays(2),
                    Units = 1
                },
                 new ClaimCharge
                {
                    Line = 5,
                    Amount = 33,
                    RevCode = "300",
                    CoveredCharge = 22,
                    HcpcsCode = "36415",
                    HcpcsCodeWithModifier = "36415",
                    ServiceFromDate = DateTime.Today,
                    Units = 1
                }
            };
            return evaluateableClaim;
        }
    }
}
