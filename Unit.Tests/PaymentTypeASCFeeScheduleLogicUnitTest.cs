/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Payment Type ASC Fee Schedule Logic Testing
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Linq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Data.Repository;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;
using System.Collections.Generic;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for PaymentTypeASCFeeScheduleLogicUnitTest
    /// </summary>
    [TestClass]
    public class PaymentTypeAscFeeScheduleLogicUnitTest
    {

        [TestMethod]
        //Testing AuditLog constructor without parameter
        public void PaymentTypeAscFeeScheduleParameterlessConstructorTest()
        {
            var target = new PaymentTypeAscFeeScheduleLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTypeAscFeeScheduleLogic));
        }
       
        /// <summary>
        /// Payments the type asc fee schedule constructor test.
        /// </summary>
        [TestMethod]
        public void PaymentTypeAscFeeScheduleConstructorTest()
        {
            IPaymentTypeAscFeeScheduleRepository productRepository = new PaymentTypeAscFeeScheduleRepository(Constants.ConnectionString); 
            PaymentTypeAscFeeScheduleLogic target = new PaymentTypeAscFeeScheduleLogic(productRepository);
            Assert.IsInstanceOfType(target, typeof(PaymentTypeAscFeeScheduleLogic));
        }
        
        /// <summary>
        /// Payments the type asc fee schedule constructor test1.
        /// </summary>
        [TestMethod]
        public void PaymentTypeAscFeeScheduleConstructorTest1()
        {
            var mockPaymentTypeAscFeeSchedule = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            PaymentTypeAscFeeScheduleLogic target = new PaymentTypeAscFeeScheduleLogic(mockPaymentTypeAscFeeSchedule.Object);
            Assert.IsInstanceOfType(target, typeof(PaymentTypeAscFeeScheduleLogic));
        }
        

      /// <summary>
        /// Payments the type asc fee schedule Test for Null
        /// </summary>
        [TestMethod]
        public void AddPaymentTypeAscFeeScheduleIfNull()
        {
            var mockPaymentTypeAscFeeSchedule = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            mockPaymentTypeAscFeeSchedule.Setup(f => f.AddEditPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>())).Returns(1);
            PaymentTypeAscFeeScheduleLogic target = new PaymentTypeAscFeeScheduleLogic(mockPaymentTypeAscFeeSchedule.Object);

          long actual = target.AddEditPaymentType(null);
            Assert.IsNotNull(actual);
        }
        /// <summary>
        /// Payments the type asc fee schedule Test for notnull
        /// </summary>
        [TestMethod]
        public void AddPaymentTypeAscFeeScheduleIfNotNull()
        {
            var mockAddNewPaymentTypeFeeSchedule = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            mockAddNewPaymentTypeFeeSchedule.Setup(f => f.AddEditPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>())).Returns(1);
            PaymentTypeAscFeeScheduleLogic target = new PaymentTypeAscFeeScheduleLogic(mockAddNewPaymentTypeFeeSchedule.Object);
            PaymentTypeAscFeeSchedule objAddEditPaymentTypeMedicareIpPayment = new PaymentTypeAscFeeSchedule { PaymentTypeDetailId = 1 };

            long actual = target.AddEditPaymentType(objAddEditPaymentTypeMedicareIpPayment);
            Assert.AreEqual(1, actual);
        }


        /// <summary>
        /// Gets the payment type asc fee schedule detailsif null.
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeAscFeeScheduleDetailsifNull()
        {
            PaymentTypeAscFeeSchedule objGetPaymentTypeAscFeeScheduleDetails = new PaymentTypeAscFeeSchedule {ContractId = 1 };

            var mockGetPaymentTypeAscFeeScheduleDetails = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            mockGetPaymentTypeAscFeeScheduleDetails.Setup(f => f.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>())).Returns(objGetPaymentTypeAscFeeScheduleDetails);
            PaymentTypeAscFeeScheduleLogic target = new PaymentTypeAscFeeScheduleLogic(mockGetPaymentTypeAscFeeScheduleDetails.Object);

            PaymentTypeBase actual = target.GetPaymentType(null);
            Assert.AreEqual(1, actual.ContractId);
            // Assert.IsNotNull(actual);
        }

        /// <summary>
        /// 
        /// Deletes the contract service calculate lines and payment types by unique identifier unit test.
        /// </summary>
        [TestMethod]
        public void DeleteContractServiceLInesandPaymentTypesByIdUnitTest()
        {
            var mockProductRepository = new Mock<IContractServiceLinePaymentTypeRepository>();
            ContractServiceLinePaymentType objcontractDocs = new ContractServiceLinePaymentType { ContractId = 1 };
            mockProductRepository.Setup(f => f.DeleteContractServiceLinesAndPaymentTypes(objcontractDocs)).Returns(true);
            ContractServiceLinePaymentTypeLogic target = new ContractServiceLinePaymentTypeLogic(mockProductRepository.Object);
            bool actual = target.DeleteContractServiceLinesAndPaymentTypes(objcontractDocs);
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Gets the table name selection unit test.
        /// </summary>
        [TestMethod]
        public void GetTableNameSelectionUnitTest()
        {
            var mockGetTableNameSelection = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            //Mock Input
            PaymentTypeAscFeeSchedule inputData = new PaymentTypeAscFeeSchedule { ContractId = 234, ClaimFieldId =21};

            //Mock output 
            List<PaymentTypeTableSelection> result = new List<PaymentTypeTableSelection> {new PaymentTypeTableSelection{ClaimFieldDocId=7765,TableName="Asc FeeSchedule"}  };
            mockGetTableNameSelection.Setup(f => f.GetTableNameSelection(inputData)).Returns(result);
            PaymentTypeAscFeeScheduleLogic target = new PaymentTypeAscFeeScheduleLogic(mockGetTableNameSelection.Object);
            List<PaymentTypeTableSelection> actual = target.GetTableNameSelection(inputData);
            Assert.AreEqual(result,actual);
        }

        /// <summary>
        /// Evaluates the test.
        /// </summary>
        [TestMethod]
        public void EvaluateTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 3,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 4,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 5,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 6,AdjudicatedValue = null},
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int>{1,2,3,4,5,6},
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "30001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "30001",
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = 10,
                        ClaimFieldDoc = new ClaimFieldDoc
                        {
                            ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30001AB",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30101",
                                    Value = "150"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30201",
                                    Value = "100"
                                }
                                ,new ClaimFieldValue
                                {
                                    Identifier = "30301BC",
                                    Value = "100"
                                }
                                ,new ClaimFieldValue
                                {
                                    Identifier = "30401BC",
                                    Value = "200"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30501",
                                    Value = "100"
                                }
                            }
                        }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    HcpcsCode = "30001",
                    HcpcsCodeWithModifier = "30001AB"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "30101",
                     HcpcsCodeWithModifier = "30101"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = 20,
                    HcpcsCode = "30201",
                    HcpcsCodeWithModifier = "30201"
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                    HcpcsCode = "30301",
                    HcpcsCodeWithModifier = "30301BC"
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                    HcpcsCode = "30401",
                     HcpcsCodeWithModifier = "30401"
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "30501",
                     HcpcsCodeWithModifier = "30501"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(6, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x=>x.AdjudicatedValue ==null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates the non fee schedule test.
        /// </summary>
        [TestMethod]
        public void EvaluateNonFeeScheduleTest()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 3,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 4,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 5,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 6,AdjudicatedValue = null},
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "30001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "30001",
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = 10,
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30801",
                                    Value = "100"
                                }
                            }
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    HcpcsCode = "30001",
                    HcpcsCodeWithModifier = "30001"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "30101",
                     HcpcsCodeWithModifier = "30101"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = 20,
                    HcpcsCode = "30201",
                    HcpcsCodeWithModifier = "30201"
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                    HcpcsCode = "30301",
                    HcpcsCodeWithModifier = "30301"
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                    HcpcsCode = "30401",
                    HcpcsCodeWithModifier = "30401"
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "30501",
                    HcpcsCodeWithModifier = "30501"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(6, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue == null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Checks the claim data error.
        /// </summary>
        [TestMethod]
        public void CheckClaimDataError()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 3,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 4,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 5,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 6,AdjudicatedValue = null},
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int> {1, 2, 3, 4, 5, 6},
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "30001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "30001",
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = 10,
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                    {
                        new ClaimFieldValue
                        {
                            Identifier = "30001",
                            Value = "0"
                        },
                        new ClaimFieldValue
                        {
                            Identifier = "30101",
                            Value = "0"
                        },
                        new ClaimFieldValue
                        {
                            Identifier = "30201",
                            Value = "0"
                        }
                        ,
                        new ClaimFieldValue
                        {
                            Identifier = "30301",
                             Value ="0"
                        }
                        ,
                        new ClaimFieldValue
                        {
                            Identifier = "30401",
                            Value = "0"
                        },
                        new ClaimFieldValue
                        {
                            Identifier = "30501",
                            Value = "0"
                        }
                    }
                }
            };
           
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    HcpcsCode = "30001",
                    Amount=100,
                    HcpcsCodeWithModifier = "30001"
                },
                new ClaimCharge
                {
                    Line = 2,
                    HcpcsCode = "30101",
                    Amount=100,
                    HcpcsCodeWithModifier = "30001"
                },
                new ClaimCharge
                {
                    Line = 3,
                    HcpcsCode = "30201",
                    Amount=100,
                    HcpcsCodeWithModifier = "30201"
                },
                new ClaimCharge
                {
                    Line = 4,
                    HcpcsCode = "30301",
                    Amount=100,
                    HcpcsCodeWithModifier = "30301"
                },
                new ClaimCharge
                {
                    Line = 5,
                    HcpcsCode = "30401",
                    Amount=100,
                    HcpcsCodeWithModifier = "30401"
                },
                new ClaimCharge
                {
                    Line = 6,
                    HcpcsCode = "30501",
                    Amount=100,
                    HcpcsCodeWithModifier = "30501"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(6, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue > 0);
            Assert.IsNull(firstOrDefault);
        }

        //Get Valid Code Amounts For Schedule Amount if HCPCS and Claim field Identifier is same
        /// <summary>
        /// Gets the valid code amounts for schedule amount test1.
        /// </summary>
        [TestMethod]
        public void GetValidCodeAmountsForScheduleAmountTest1()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 3,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 4,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 5,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 6,AdjudicatedValue = null},
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "30001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "30001",
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = 10,
                OptionSelection = 1,
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30001",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30101",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30201",
                                    Value = "100"
                                }
                                ,new ClaimFieldValue
                                {
                                    Identifier = "30301",
                                    Value = "100"
                                }
                                ,new ClaimFieldValue
                                {
                                    Identifier = "30401",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30501",
                                    Value = "100"
                                }
                            }
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    HcpcsCode = "30001",
                    HcpcsCodeWithModifier = "30001"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "30101",
                    HcpcsCodeWithModifier ="30101"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = 20,
                    HcpcsCode = "30201",
                    HcpcsCodeWithModifier ="30201" 
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                    HcpcsCode = "30301",
                     HcpcsCodeWithModifier ="30301" 
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                    HcpcsCode = "30401",
                    HcpcsCodeWithModifier ="30401" 
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "30501",
                    HcpcsCodeWithModifier ="30501" 
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(6, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue == null);
            Assert.IsNull(firstOrDefault);
        }

        //Get Valid Code Amounts For Schedule Amount if HCPCS and Claim field Identifier is not same
        /// <summary>
        /// Gets the valid code amounts for schedule amount test2.
        /// </summary>
        [TestMethod]
        public void GetValidCodeAmountsForScheduleAmountTest2()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 3,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 4,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 5,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 6,AdjudicatedValue = null},
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "20001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "30001",
                
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = 10,
                OptionSelection = 1,
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30001",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30101",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30201",
                                    Value = "100"
                                }
                                ,new ClaimFieldValue
                                {
                                    Identifier = "30301",
                                    Value = "100"
                                }
                                ,new ClaimFieldValue
                                {
                                    Identifier = "30401",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30501",
                                    Value = "100"
                                }
                            }
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    HcpcsCode = "30001",
                    HcpcsCodeWithModifier = "30001"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "30101",
                     HcpcsCodeWithModifier = "30101"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = 20,
                    HcpcsCode = "30201",
                    HcpcsCodeWithModifier = "30201"
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                    HcpcsCode = "30301",
                     HcpcsCodeWithModifier = "30301"
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                    HcpcsCode = "30401",
                    HcpcsCodeWithModifier = "30401"
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "30501",
                     HcpcsCodeWithModifier = "30501"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(6, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue == null);
            Assert.IsNull(firstOrDefault);
        }


        //Get Valid Code Amounts For Line Charge if HCPCS and Claim field Identifier is same
        /// <summary>
        /// Gets the valid code amounts for line charge test1.
        /// </summary>
        [TestMethod]
        public void GetValidCodeAmountsForLineChargeTest1()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 3,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 4,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 5,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 6,AdjudicatedValue = null},
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "30001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "30001",
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = 10,
                OptionSelection = 2,
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30001",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30101",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30201",
                                    Value = "100"
                                }
                                ,new ClaimFieldValue
                                {
                                    Identifier = "30301",
                                    Value = "100"
                                }
                                ,new ClaimFieldValue
                                {
                                    Identifier = "30401",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30501",
                                    Value = "100"
                                }
                            }
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    HcpcsCode = "30001",
                    HcpcsCodeWithModifier = "30001"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "30101",
                    HcpcsCodeWithModifier = "30101"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = 20,
                    HcpcsCodeWithModifier="30201",
                    HcpcsCode = "30201"
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                     HcpcsCodeWithModifier="30301",
                    HcpcsCode = "30301"
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                     HcpcsCodeWithModifier="30401",
                    HcpcsCode = "30401"
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "30501",
                    HcpcsCodeWithModifier="30501"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(6, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue == null);
            Assert.IsNull(firstOrDefault);
        }

        //Get Valid Code Amounts For Line Charge if HCPCS and Claim field Identifier is not same
        /// <summary>
        /// Gets the valid code amounts for line charge test2.
        /// </summary>
        [TestMethod]
        public void GetValidCodeAmountsForLineChargeTest2WithIncludeModifiersReturnsTrue()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null}
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int> { 1, 2 },
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "30001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "20001",
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = 10,
                OptionSelection = 2,
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
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    HcpcsCode = "30001",
                    HcpcsModifiers = "26",
                    HcpcsCodeWithModifier = "3000126"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "30101",
                    HcpcsCodeWithModifier = "3010126",
                    HcpcsModifiers = "26"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 2, ContractId = 1,AdjudicatedValue = 100},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 100}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(2, actual.Count);
            var paymentResult = paymentResults.FirstOrDefault(x => x.AdjudicatedValue == 100);
            if (paymentResult != null)
            {
                Assert.AreEqual(100, paymentResult.AdjudicatedValue);
            }
        }


        //Get Valid Code Amounts For Line Charge if HCPCS and Claim field Identifier is not same
        /// <summary>
        /// Gets the valid code amounts for line charge test2.
        /// </summary>
        [TestMethod]
        public void GetValidCodeAmountsForLineChargeTest2()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 3,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 4,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 5,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 6,AdjudicatedValue = null},
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "30001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "20001",
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = 10,
                OptionSelection = 2,
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30001",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30101",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30201",
                                    Value = "100"
                                }
                                ,new ClaimFieldValue
                                {
                                    Identifier = "30301",
                                    Value = "100"
                                }
                                ,new ClaimFieldValue
                                {
                                    Identifier = "30401",
                                    Value = "100"
                                },
                                new ClaimFieldValue
                                {
                                    Identifier = "30501",
                                    Value = "100"
                                }
                            }
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    HcpcsCode = "30001",
                    HcpcsCodeWithModifier = "30001"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "30101",
                     HcpcsCodeWithModifier = "30101"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = 20,
                    HcpcsCode = "30201",
                     HcpcsCodeWithModifier = "30201"
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                    HcpcsCode = "30301",
                     HcpcsCodeWithModifier = "30301"
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                    HcpcsCode = "30401",
                     HcpcsCodeWithModifier = "30401"
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "30501",
                     HcpcsCodeWithModifier = "30501"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(6, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue == null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Applyings the NFS for adjudication error.
        /// </summary>
        [TestMethod]
        public void ApplyingNfsForAdjudicationError()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 3,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 4,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 5,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 6,AdjudicatedValue = null},
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "30001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "30001",
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = null ,
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30801",
                                    Value = "100"
                                }
                            }
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    HcpcsCode = "30001",
                    HcpcsCodeWithModifier = "30001"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "30101",
                     HcpcsCodeWithModifier = "30101"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = 20,
                    HcpcsCode = "30201",
                    HcpcsCodeWithModifier = "30201"
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                    HcpcsCode = "30301",
                      HcpcsCodeWithModifier = "30301"
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                    HcpcsCode = "30401",
                      HcpcsCodeWithModifier = "30401"
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "30501",
                     HcpcsCodeWithModifier = "30501"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(6, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue == null);
            Assert.IsNotNull(firstOrDefault);
        }

        /// <summary>
        /// Applyings the NFS for adjudication error with include modifers.
        /// </summary>
        [TestMethod]
        public void ApplyingNfsForAdjudicationErrorWithIncludeModifers()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 3,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 4,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 5,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 6,AdjudicatedValue = null},
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "30001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "30001",
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = null,
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30801",
                                    Value = "100"
                                }
                            }
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    HcpcsCode = "30001",
                    HcpcsCodeWithModifier = "3000126",
                    HcpcsModifiers = "26"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "",
                    HcpcsCodeWithModifier = "",
                    HcpcsModifiers = "26"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = 20,
                    HcpcsCode = "30201",
                    HcpcsCodeWithModifier = "3020126",
                    HcpcsModifiers = "26"
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                    HcpcsCode = "30301",
                    HcpcsCodeWithModifier = "3030126",
                    HcpcsModifiers = "26"
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                    HcpcsCode = "30401",
                    HcpcsCodeWithModifier = "3040126",
                    HcpcsModifiers = "26"
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "30501",
                    HcpcsCodeWithModifier = "3050126",
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
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(6, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue == null);
            Assert.IsNotNull(firstOrDefault);
        }
        
        /// <summary>
        /// Applyings the NFS for adjudication error with include modifers.
        /// </summary>
        [TestMethod]
        public void ApplyingNfsForAdjudicationErrorWithHcpcsCodeIsEmpty()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 3,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 4,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 5,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 6,AdjudicatedValue = null},
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "30001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "30001",
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = null,
                OptionSelection = 2,
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30801",
                                    Value = "100"
                                }
                            }
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    HcpcsCode = "",
                    HcpcsCodeWithModifier = "",
                    HcpcsModifiers = ""
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = 20,
                    HcpcsCode = "",
                    HcpcsCodeWithModifier = "",
                    HcpcsModifiers = ""
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(6, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue == null);
            Assert.IsNotNull(firstOrDefault);
        }

        /// <summary>
        /// Applyings the NFS for claim data error.
        /// </summary>
        [TestMethod]
        public void ApplyingNfsForClaimDataError()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 3,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 4,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 5,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 6,AdjudicatedValue = null},
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "30001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "30001",
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = 20,
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30801",
                                    Value = "100"
                                }
                            }
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    Amount = null,
                    HcpcsCode = "30001",
                    HcpcsCodeWithModifier = "30001"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = null,
                    HcpcsCode = "30101",
                    HcpcsCodeWithModifier = "30101"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = null,
                    HcpcsCode = "30201",
                    HcpcsCodeWithModifier = "30201"
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                    HcpcsCode = "30301",
                    HcpcsCodeWithModifier = "30301"
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                    HcpcsCode = "30401",
                    HcpcsCodeWithModifier = "30401"
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "30501",
                    HcpcsCodeWithModifier = "30501"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(6, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue == null);
            Assert.IsNotNull(firstOrDefault);
        }

        /// <summary>
        /// Applyings the NFS for claim data error with include modifier.
        /// </summary>
        [TestMethod]
        public void ApplyingNfsForClaimDataErrorWithIncludeModifiers()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 2,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 3,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 4,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 5,AdjudicatedValue = null},
                new PaymentResult {ClaimId = 123, Line = 6,AdjudicatedValue = null},
            };
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                Conditions = new List<ICondition>
                {
                    new Condition
                    {
                        ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                        LeftOperands = new List<string> {"30001"},
                        OperandType = (byte) Enums.OperandIdentifier.HcpcsCode,
                        RightOperand = "30001"
                    }
                },
                ContractId = 1,
                HcpcsCode = "30001",
                Primary = 100,
                Secondary = 75,
                Tertiary = 50,
                Quaternary = 25,
                Others = 15,
                NonFeeSchedule = 20,
                ClaimFieldDoc = new ClaimFieldDoc
                {
                    ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30801",
                                    Value = "100",
                                }
                            }
                }
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeAscFeeScheduleRepository> paymentTypeAscFeeScheduleRepository = new Mock<IPaymentTypeAscFeeScheduleRepository>();
            paymentTypeAscFeeScheduleRepository.Setup(x => x.GetPaymentTypeAscFeeScheduleDetails(It.IsAny<PaymentTypeAscFeeSchedule>()))
                .Returns(paymentTypeAscFeeSchedule);

            var target = new PaymentTypeAscFeeScheduleLogic(paymentTypeAscFeeScheduleRepository.Object)
            {
                PaymentTypeBase = paymentTypeAscFeeSchedule
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
                    Amount = null,
                    HcpcsCode = "30001",
                    HcpcsModifiers = "26",
                    HcpcsCodeWithModifier = "3000126"
                },
                new ClaimCharge
                {
                    Line = 2,
                    Amount = null,
                    HcpcsCode = "30101",
                    HcpcsModifiers = "26",
                    HcpcsCodeWithModifier = "3010126"
                },
                new ClaimCharge
                {
                    Line = 3,
                    Amount = null,
                    HcpcsCode = "30201",
                    HcpcsModifiers = "26",
                    HcpcsCodeWithModifier = "3020126"
                },
                new ClaimCharge
                {
                    Line = 4,
                    Amount = 20,
                    HcpcsCode = "30301",
                    HcpcsModifiers = "26",
                    HcpcsCodeWithModifier = "3030126"
                },
                new ClaimCharge
                {
                    Line = 5,
                    Amount = 20,
                    HcpcsCode = "30401",
                    HcpcsModifiers = "26",
                    HcpcsCodeWithModifier = "3040126"
                },
                new ClaimCharge
                {
                    Line = 6,
                    Amount = 20,
                    HcpcsCode = "30501",
                    HcpcsModifiers = "26",
                    HcpcsCodeWithModifier = "3050126"
                }
            };


            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1,AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1,AdjudicatedValue = 110}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(6, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue == null);
            Assert.IsNotNull(firstOrDefault);
        }
    }
}