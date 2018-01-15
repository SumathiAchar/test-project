/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Contract Logic Testing
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for ContractLogicUnitTest
    /// </summary>
    [TestClass]
    public class ContractLogicTest
    {
        private static ContractLogic _target;

        /// <summary>
        /// Variances the report logic parameterless constructor test.
        /// </summary>
         [TestMethod]
         //Testing AuditLog constructor without parameter
         public void VarianceReportLogicParameterlessConstructorTest()
         {
             var target = new ContractLogic(Constants.ConnectionString);
             //Assert
             Assert.IsInstanceOfType(target, typeof(ContractLogic));
         }

        /// <summary>
        /// Contracts the logic constructor test.
        /// </summary>
        [TestMethod]
        public void ContractLogicConstructorTest()
        {
            var mockContractRepository = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();

            _target = new ContractLogic(mockContractRepository.Object, mockContractServiceTypeLogic.Object, mockPaymentResultLogic.Object);
            Assert.IsInstanceOfType(_target, typeof(ContractLogic));
        }

        /// <summary>
        /// Contracts the logic constructor test1.
        /// </summary>
        [TestMethod]
        public void ContractLogicConstructorTest1()
        {
            var mockContractrepository = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();

            ContractLogic target = new ContractLogic(mockContractrepository.Object, mockContractServiceTypeLogic.Object, mockPaymentResultLogic.Object);
            Assert.IsInstanceOfType(target, typeof(ContractLogic));
        }
        /// <summary>
        /// Adds the edit contract basic information difference null.
        /// </summary>
        [TestMethod]
        public void AddEditContractBasicInfoIfNull()
        {
            var mockAddEditContractBasicInfo = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();

            mockAddEditContractBasicInfo.Setup(f => f.AddEditContractBasicInfo(It.IsAny<Contract>())).Returns((Contract)null);
            ContractLogic target = new ContractLogic(mockAddEditContractBasicInfo.Object, mockContractServiceTypeLogic.Object, mockPaymentResultLogic.Object);
            Contract actual = target.AddEditContractBasicInfo(null);
            Assert.IsNull(actual);

        }


        /// <summary>
        /// Adds the contract modified reason difference null.
        /// </summary>
        [TestMethod]
        public void AddContractModifiedReasonIfNull()
        {
            var mockAddContractModifiedReason = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();

            mockAddContractModifiedReason.Setup(f => f.AddContractModifiedReason(It.IsAny<ContractModifiedReason>())).Returns(0);
            ContractLogic target = new ContractLogic(mockAddContractModifiedReason.Object, mockContractServiceTypeLogic.Object, mockPaymentResultLogic.Object);
            long actual = target.AddContractModifiedReason(null);
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        /// Copies the contract by unique identifier difference null.
        /// </summary>
        [TestMethod]
        public void CopyContractByIdIfNull()
        {
            var mockCopyContractById = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();

            mockCopyContractById.Setup(f => f.CopyContract(It.IsAny<Contract>())).Returns(0);
            ContractLogic target = new ContractLogic(mockCopyContractById.Object, mockContractServiceTypeLogic.Object, mockPaymentResultLogic.Object);
            long actual = target.CopyContract(null);
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        /// Renames the contract unit test.
        /// </summary>
        [TestMethod]
        public void RenameContractUnitTest()
        {
            ContractHierarchy objContractHierarchy = new ContractHierarchy { NodeId = 1, NodeText = "TestFileName", UserName = "Ragini" };

            var mockContractrepository = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();

            mockContractrepository.Setup(f => f.RenameContract(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>())).Returns(objContractHierarchy);
            ContractLogic target = new ContractLogic(mockContractrepository.Object, mockContractServiceTypeLogic.Object, mockPaymentResultLogic.Object);

            ContractHierarchy actual = target.RenameContract(1, "TestFileName", "Ragini");

            //Assert.AreNotSame(actual, objContractHierarchy);
            Assert.AreSame(actual, objContractHierarchy);
        }

        /// <summary>
        /// Gets the contract full information unit test.
        /// </summary>
        [TestMethod]
        public void GetContractFullInfoUnitTest()
        {
            ContractFullInfo objContractFullInfo = new ContractFullInfo { FacilityId = 1 };
            var mockGetContractFullInfo = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();

            mockGetContractFullInfo.Setup(f => f.GetContractFullInfo(It.IsAny<Contract>())).Returns(objContractFullInfo);
            ContractLogic target = new ContractLogic(mockGetContractFullInfo.Object, mockContractServiceTypeLogic.Object, mockPaymentResultLogic.Object);
            ContractFullInfo actual = target.GetContractFullInfo(null);
            Assert.AreEqual(1, actual.FacilityId);
        }

        /// <summary>
        /// Gets the contract full information if not null unit test.
        /// </summary>
        [TestMethod]
        public void GetContractFullInfoIfnotNullUnitTest()
        {
            //Mock Input
            Contract objContract = new Contract { FacilityId = 1 };
            //Mock output
            ContractFullInfo contractFullInfo = new ContractFullInfo { FacilityId = 1 };
            var mockGetContractFullInfo = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();

            mockGetContractFullInfo.Setup(f => f.GetContractFullInfo(It.IsAny<Contract>())).Returns(contractFullInfo);
            ContractLogic target = new ContractLogic(mockGetContractFullInfo.Object, mockContractServiceTypeLogic.Object, mockPaymentResultLogic.Object);
            ContractFullInfo actual = target.GetContractFullInfo(objContract);
            Assert.AreEqual(contractFullInfo, actual);
        }

        /// <summary>
        /// Gets the contract first level details if null test.
        /// </summary>
        [TestMethod]
        public void GetContractFirstLevelDetailsifNullTest()
        {
            // Arrange
            var mockContractRepository = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();

            _target = new ContractLogic(mockContractRepository.Object, mockContractServiceTypeLogic.Object, mockPaymentResultLogic.Object);

            //Act
            Contract actual = _target.GetContractFirstLevelDetails(null);

            // Assert
            Assert.AreEqual(null, actual);

        }

        /// <summary>
        /// Gets the contract first level details if not null test.
        /// </summary>
        [TestMethod]
        public void GetContractFirstLevelDetailsifNotNullTest()
        {
            var mockGetContractFirstLevelDetails = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            //Mock Input
            const long contractId = 1479;
            //Mock output 
            Contract result = new Contract { NodeId = 278, FacilityId = 774, ParentId = 224 };
            mockGetContractFirstLevelDetails.Setup(f => f.GetContractFirstLevelDetails(contractId)).Returns(result);
            ContractLogic target = new ContractLogic(mockGetContractFirstLevelDetails.Object, mockContractServiceTypeLogic.Object, mockPaymentResultLogic.Object);
            Contract actual = target.GetContractFirstLevelDetails(contractId);
            Assert.AreEqual(result, actual);
        }


        /// <summary>
        /// Evaluates the test if claim is null.
        /// </summary>
        [TestMethod]
        public void EvaluateTestIfClaimIsNull()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };
            // Arrange
            var mockContractRepository = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new ContractLogic(mockContractRepository.Object, mockContractServiceTypeLogic.Object,
                mockPaymentResultLogic.Object);
            Contract contract = new Contract
            {

                ContractId = 1,
                ContractName = "ABC",
                Conditions = new List<ICondition>(),
                ContractServiceTypes = new List<ContractServiceType>
                {
                    new ContractServiceType
                    {
                        ContractServiceTypeId = 12,
                        ContractServiceTypeName = "Service Type 1"
                    }
                }
                ,
                PaymentTypes = new List<PaymentTypeBase>
                {
                    new PaymentTypeAscFeeSchedule
                    {
                        PaymentTypeId = (byte) Enums.PaymentTypeCodes.AscFeeSchedule,
                        FacilityId = 1,
                        ValidLineIds = new List<int> {1},
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
                        ClaimFieldDoc = new ClaimFieldDoc
                        {
                            ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30001",
                                    Value = "1"
                                }
                            }
                        },
                    }
                }
            };
            _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = null;
            _target.AdjudicateClaims = new List<EvaluateableClaim>();
            _target.EarlyExitClaims = new List<EvaluateableClaim>();
            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            _target.Evaluate(claim: null, paymentResults: paymentResults, isCarveOut: false, isContractFilter: false);

            // Assert
            Assert.AreEqual(0, _target.AdjudicateClaims.Count);
            Assert.AreEqual(0, _target.EarlyExitClaims.Count);

        }



        /// <summary>
        /// Evaluates the test if claim is not null.
        /// </summary>
        [TestMethod]
        public void EvaluateTestIfClaimIsNotNull()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };
            // Arrange
            var mockContractRepository = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new ContractLogic(mockContractRepository.Object, mockContractServiceTypeLogic.Object,
                mockPaymentResultLogic.Object);
            Contract contract = new Contract
            {

                ContractId = 1,
                ContractName = "ABC",
                Conditions = new List<ICondition>(),
                ContractServiceTypes = new List<ContractServiceType>
                {
                    new ContractServiceType
                    {
                        ContractServiceTypeId = 12,
                        ContractServiceTypeName = "Service Type 1"
                    }
                }
                ,
                PaymentTypes = new List<PaymentTypeBase>
                {
                    new PaymentTypeAscFeeSchedule
                    {
                        PaymentTypeId = (byte) Enums.PaymentTypeCodes.AscFeeSchedule,
                        FacilityId = 1,
                        ValidLineIds = new List<int> {1},
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
                        ClaimFieldDoc = new ClaimFieldDoc
                        {
                            ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30001",
                                    Value = "1"
                                }
                            }
                        }
                    }
                }
            };
            _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Line = 1,
                        Amount = 20,
                        RevCode = "250",
                        HcpcsCodeWithModifier = "30001",
                        HcpcsCode = "30001",
                        HcpcsModifiers = "26"
                    }
                };
            _target.AdjudicateClaims = new List<EvaluateableClaim>();
            _target.EarlyExitClaims = new List<EvaluateableClaim>();
            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(1, _target.AdjudicateClaims.Count);
            Assert.AreEqual(0, _target.EarlyExitClaims.Count);
        }


        /// <summary>
        /// Evaluates the test if contract identifier is not same.
        /// </summary>
        [TestMethod]
        public void EvaluateTestIfContractIdIsNotSame()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };
            // Arrange
            var mockContractRepository = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new ContractLogic(mockContractRepository.Object, mockContractServiceTypeLogic.Object,
                mockPaymentResultLogic.Object);
            Contract contract = new Contract
            {

                ContractId = 123892,
                ContractName = "ABC",
                Conditions = new List<ICondition>(),
                ContractServiceTypes = new List<ContractServiceType>
                {
                    new ContractServiceType
                    {
                        ContractServiceTypeId = 12,
                        ContractServiceTypeName = "Service Type 1"
                    }
                }
                ,
                PaymentTypes = new List<PaymentTypeBase>
                {
                    new PaymentTypeAscFeeSchedule
                    {
                        PaymentTypeId = (byte) Enums.PaymentTypeCodes.AscFeeSchedule,
                        FacilityId = 1,
                        ValidLineIds = new List<int> {1},
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
                        ClaimFieldDoc = new ClaimFieldDoc
                        {
                            ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30001",
                                    Value = "1"
                                }
                            }
                        }
                    }
                }
            };
            _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim{LastAdjudicatedContractId = 1234};
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Line = 1,
                        Amount = 20,
                        RevCode = "250",
                        HcpcsCodeWithModifier = "30001",
                        HcpcsCode = "30001",
                        HcpcsModifiers = "26"
                    }
                };
            _target.AdjudicateClaims = new List<EvaluateableClaim>();
            _target.EarlyExitClaims = new List<EvaluateableClaim>();
            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(1, _target.AdjudicateClaims.Count);
            Assert.AreEqual(0, _target.EarlyExitClaims.Count);
        }

        /// <summary>
        /// Evaluates the test if contract identifier is same and is claim adjudicated is false.
        /// </summary>
        [TestMethod]
        public void EvaluateTestIfContractIdIsSameAndIsClaimAdjudicatedIsFalse()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };
            // Arrange
            var mockContractRepository = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new ContractLogic(mockContractRepository.Object, mockContractServiceTypeLogic.Object,
                mockPaymentResultLogic.Object);
            Contract contract = new Contract
            {

                ContractId = 123892,
                ContractName = "ABC",
                Conditions = new List<ICondition>(),
                ContractServiceTypes = new List<ContractServiceType>
                {
                    new ContractServiceType
                    {
                        ContractServiceTypeId = 12,
                        ContractServiceTypeName = "Service Type 1"
                    }
                }
                ,
                PaymentTypes = new List<PaymentTypeBase>
                {
                    new PaymentTypeAscFeeSchedule
                    {
                        PaymentTypeId = (byte) Enums.PaymentTypeCodes.AscFeeSchedule,
                        FacilityId = 1,
                        ValidLineIds = new List<int> {1},
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
                        ClaimFieldDoc = new ClaimFieldDoc
                        {
                            ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30001",
                                    Value = "1"
                                }
                            }
                        }
                    }
                }
            };
            _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim { LastAdjudicatedContractId = 123892, IsClaimAdjudicated = false};
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Line = 1,
                        Amount = 20,
                        RevCode = "250",
                        HcpcsCodeWithModifier = "30001",
                        HcpcsCode = "30001",
                        HcpcsModifiers = "26"
                    }
                };
            _target.AdjudicateClaims = new List<EvaluateableClaim>();
            _target.EarlyExitClaims = new List<EvaluateableClaim>();
            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(1, _target.AdjudicateClaims.Count);
            Assert.AreEqual(0, _target.EarlyExitClaims.Count);
        }


        /// <summary>
        /// Evaluates the test if contract identifier is same and is claim adjudicated is true.
        /// </summary>
        [TestMethod]
        public void EvaluateTestIfContractIdIsSameAndIsClaimAdjudicatedIsTrue()
        {
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };
            // Arrange
            var mockContractRepository = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            _target = new ContractLogic(mockContractRepository.Object, mockContractServiceTypeLogic.Object,
                mockPaymentResultLogic.Object);
            Contract contract = new Contract
            {

                ContractId = 123892,
                ContractName = "ABC",
                Conditions = new List<ICondition>(),
                ContractServiceTypes = new List<ContractServiceType>
                {
                    new ContractServiceType
                    {
                        ContractServiceTypeId = 12,
                        ContractServiceTypeName = "Service Type 1"
                    }
                }
                ,
                PaymentTypes = new List<PaymentTypeBase>
                {
                    new PaymentTypeAscFeeSchedule
                    {
                        PaymentTypeId = (byte) Enums.PaymentTypeCodes.AscFeeSchedule,
                        FacilityId = 1,
                        ValidLineIds = new List<int> {1},
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
                        ClaimFieldDoc = new ClaimFieldDoc
                        {
                            ClaimFieldValues = new List<ClaimFieldValue>
                            {
                                new ClaimFieldValue
                                {
                                    Identifier = "30001",
                                    Value = "1"
                                }
                            }
                        }
                    }
                }
            };
            _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim { LastAdjudicatedContractId = 123892, IsClaimAdjudicated = true };
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Line = 1,
                        Amount = 20,
                        RevCode = "250",
                        HcpcsCodeWithModifier = "30001",
                        HcpcsCode = "30001",
                        HcpcsModifiers = "26"
                    }
                };
            _target.AdjudicateClaims = new List<EvaluateableClaim>();
            _target.EarlyExitClaims = new List<EvaluateableClaim>();
            List<PaymentResult> updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1}
            };

            Mock<ContractBaseLogic> mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, false, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            _target.Evaluate(evaluateableClaim, paymentResults, false, false);

            // Assert
            Assert.AreEqual(0, _target.AdjudicateClaims.Count);
            Assert.AreEqual(1, _target.EarlyExitClaims.Count);
        }

        /// <summary>
        /// Determines whether [is contract name exist test].
        /// </summary>
        [TestMethod]
        public void IsContractNameExistTest()
        {
            var mockContractRepository = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            var contract = new Contract
            {
                ContractId = 123,
                ContractName = "ABC",
                IsProfessional = true,
                PayersList = "blue Cross",
                Conditions = new List<ICondition>(),
                Payers = new List<Payer> {new Payer {PayerName = "Aetna"}},
                CustomField = 29,
                PayerCode = "Test1"
            };
            mockContractRepository.Setup(f => f.IsContractNameExist(It.IsAny<Contract>())).Returns(true);
            var target = new ContractLogic(mockContractRepository.Object, mockContractServiceTypeLogic.Object, mockPaymentResultLogic.Object);
            bool actual = target.IsContractNameExist(contract);

            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Gets the adjudicated contracts test.
        /// </summary>
        [TestMethod]
        public void GetAdjudicatedContractsTest()
        {
            var mockContractRepository = new Mock<IContractRepository>();
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            var expected = new List<Contract>
            {
                new Contract
                {
                    ContractId = 7845,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions =  new List<ICondition>
               {
                   new Condition
                   {
                       ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                       RightOperand = "121",
                       OperandIdentifier = (byte) Enums.OperandIdentifier.BillType,
                       OperandType = (byte) Enums.OperandType.Numeric,
                       PropertyColumnName = Constants.PropertyBillType
                   }
               },
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var contract = new Contract
            {
                ContractId = 123,
                ContractName = "ABC",
                IsProfessional = true,
                PayersList = "blue Cross",
                Conditions = new List<ICondition>(),
                Payers = new List<Payer> { new Payer { PayerName = "Aetna" } },
                CustomField = 29,
                PayerCode = "Test1"
            };
            mockContractRepository.Setup(f => f.GetAdjudicatedContracts(It.IsAny<Contract>())).Returns(expected);
            var target = new ContractLogic(mockContractRepository.Object, mockContractServiceTypeLogic.Object, mockPaymentResultLogic.Object);
            List<Contract> actual = target.GetAdjudicatedContracts(contract);

            Assert.AreEqual(expected[0].ContractId, actual[0].ContractId);
        }
    }
}
