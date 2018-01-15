/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Payment Type Medicare IP Details Logic Testing
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
    /// Summary description for PaymentTypeMedicareIPDetailsLogicUnitTest
    /// </summary>
    [TestClass]
    public class PaymentTypeMedicareIpDetailsLogicUnitTest
    {

        /// <summary>
        /// Payments the type medicare ip details constructor unit test1.
        /// </summary>
        [TestMethod]
        public void PaymentTypeMedicareIpDetailsConstructorUnitTest1()
        {
            //Act
            var target = new PaymentTypeMedicareIpLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTypeMedicareIpLogic));
        }


        /// <summary>
        /// Payments the type medicare ip details constructor unit test2.
        /// </summary>
        [TestMethod]
        public void PaymentTypeMedicareIpDetailsConstructorUnitTest2()
        {
            //Arrange
            Mock<IPaymentTypeMedicareIpRepository> mockAddEditPaymentTypeMedicareIpPayment = new Mock<IPaymentTypeMedicareIpRepository>();

            //Act
            PaymentTypeMedicareIpLogic target = new PaymentTypeMedicareIpLogic(mockAddEditPaymentTypeMedicareIpPayment.Object);

            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTypeMedicareIpLogic));
        }

        /// <summary>
        ///  AddEditPaymentTypeMedicareOPPayment test for Null
        /// </summary>
        [TestMethod]
        public void AddEditPaymentTypeMedicareIpPaymentIfNull()
        {
            //Arrange
            Mock<IPaymentTypeMedicareIpRepository> mockAddEditPaymentTypeMedicareIpPayment = new Mock<IPaymentTypeMedicareIpRepository>();
            mockAddEditPaymentTypeMedicareIpPayment.Setup(f => f.AddEditPaymentTypeMedicareIpPayment(It.IsAny<PaymentTypeMedicareIp>())).Returns(1);
            PaymentTypeMedicareIpLogic target = new PaymentTypeMedicareIpLogic(mockAddEditPaymentTypeMedicareIpPayment.Object);

            //Act
            long actual = target.AddEditPaymentType(null);

            //Assert
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        ///  AddEditPaymentTypeMedicareOPPayment test for Not Null
        /// </summary>
        [TestMethod]
        public void AddEditPaymentTypeMedicareIpPaymentIfNotNull()
        {
            //Arrange
            Mock<IPaymentTypeMedicareIpRepository> mockAddEditPaymentTypeMedicareIpPayment = new Mock<IPaymentTypeMedicareIpRepository>();
            mockAddEditPaymentTypeMedicareIpPayment.Setup(f => f.AddEditPaymentTypeMedicareIpPayment(It.IsAny<PaymentTypeMedicareIp>())).Returns(1);
            PaymentTypeMedicareIpLogic target = new PaymentTypeMedicareIpLogic(mockAddEditPaymentTypeMedicareIpPayment.Object);
            PaymentTypeMedicareIp objAddEditPaymentTypeMedicareIpPayment = new PaymentTypeMedicareIp { PaymentTypeDetailId = 1 };

            //Act
            long actual = target.AddEditPaymentType(objAddEditPaymentTypeMedicareIpPayment);

            //Assert
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Edits the payment type medicare IP payment 
        /// </summary>
        [TestMethod]
        public void EditPaymentTypeMedicareIpPaymentCaseMockTest1()
        {
            //Arrange
            Mock<IPaymentTypeMedicareIpRepository> mockAddEditPaymentTypeMedicareIpPayment = new Mock<IPaymentTypeMedicareIpRepository>();
            mockAddEditPaymentTypeMedicareIpPayment.Setup(f => f.AddEditPaymentTypeMedicareIpPayment(It.IsAny<PaymentTypeMedicareIp>())).Returns(1);
            PaymentTypeMedicareIpLogic target = new PaymentTypeMedicareIpLogic(mockAddEditPaymentTypeMedicareIpPayment.Object);
            PaymentTypeMedicareIp objAddEditPaymentTypeMedicareIpPayment = new PaymentTypeMedicareIp { PaymentTypeDetailId = 1 };

            //Act
            long actual = target.AddEditPaymentType(objAddEditPaymentTypeMedicareIpPayment);

            //Assert
            Assert.IsNotNull(actual);
        }
        /// <summary>
        /// Get Payment Type Medicare IP Details
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeMedicareIpDetailsTest()
        {
            //Arrange
            PaymentTypeMedicareIp objAddEditPaymentTypeMedicareIpPayment = new PaymentTypeMedicareIp { PaymentTypeDetailId = 1, PaymentTypeId = 4, ContractId = 234, ServiceTypeId = null };
            Mock<IPaymentTypeMedicareIpRepository> mockGetPaymentTypeStopLoss = new Mock<IPaymentTypeMedicareIpRepository>();
            PaymentTypeMedicareIp result = new PaymentTypeMedicareIp();
            mockGetPaymentTypeStopLoss.Setup(f => f.GetPaymentTypeMedicareIpPayment(objAddEditPaymentTypeMedicareIpPayment)).Returns(result);
            PaymentTypeMedicareIpLogic target = new PaymentTypeMedicareIpLogic(mockGetPaymentTypeStopLoss.Object);

            //Act
            PaymentTypeMedicareIp actual = (PaymentTypeMedicareIp)target.GetPaymentType(null);

            //Assert
            Assert.AreEqual(null, actual);


        }

        /// <summary>
        /// Evaluates at line level test.
        /// </summary>
        [TestMethod]
        public void EvaluateAtLineLevelTest()
        {
            //Arrange
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
                new PaymentResult {ClaimId = 123, AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
            };
            PaymentTypeMedicareIp paymentTypeMedicareIp = new PaymentTypeMedicareIp
            {
                PayAtClaimLevel = true,
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
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
                HcpcsCode = "300",
                ServiceTypeId = 1,
                IsMedicareIpAcuteEnabled = true
            };

            Mock<IContractServiceTypeLogic> mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            Mock<IPaymentResultLogic> mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeMedicareIpRepository> paymentTypeMedicareIpRepository = new Mock<IPaymentTypeMedicareIpRepository>();
            paymentTypeMedicareIpRepository.Setup(x => x.GetPaymentTypeMedicareIpPayment(It.IsAny<PaymentTypeMedicareIp>()))
                .Returns(paymentTypeMedicareIp);

            var target = new PaymentTypeMedicareIpLogic(paymentTypeMedicareIpRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareIp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.MedicareInPatient = new MedicareInPatient
            {
                Charges = 10,
                ClaimId = 123,
                Npi = "1003814971",
                Drg = "070",
                DischargeDate = DateTime.Now.AddDays(-20),
                DischargeStatus = "01",
                LengthOfStay = 10
            };

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
                    HcpcsCode = "305"
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
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        //For ICD 10
        /// <summary>
        /// Evaluates for ICD 10 diagnosis code not null.
        /// </summary>
        [TestMethod]
        public void EvaluateForIcd10DiagnosisCodeNotNull()
        {
            //Arrange
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
                new PaymentResult {ClaimId = 123, AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
            };
            PaymentTypeMedicareIp paymentTypeMedicareIp = new PaymentTypeMedicareIp
            {
                PayAtClaimLevel = true,
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
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
                HcpcsCode = "300",
                ServiceTypeId = 1,
                IsMedicareIpAcuteEnabled = true
            };

            Mock<IContractServiceTypeLogic> mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            Mock<IPaymentResultLogic> mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeMedicareIpRepository> paymentTypeMedicareIpRepository = new Mock<IPaymentTypeMedicareIpRepository>();
            paymentTypeMedicareIpRepository.Setup(x => x.GetPaymentTypeMedicareIpPayment(It.IsAny<PaymentTypeMedicareIp>()))
                .Returns(paymentTypeMedicareIp);

            var target = new PaymentTypeMedicareIpLogic(paymentTypeMedicareIpRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareIp
            };


           
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.StatementThru = DateTime.Parse("10/01/2015 12:00:00 AM");
            evaluateableClaim.MedicareInPatient = new MedicareInPatient
            {
                Charges = 10,
                ClaimId = 123,
                Npi = "1003814971",
                Drg = "070",
                DischargeDate = DateTime.Now.AddDays(-20),
                DischargeStatus = "01",
                LengthOfStay = 10
            };

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
                    HcpcsCode = "305"
                }
            };
            evaluateableClaim.DiagnosisCodes = new List<DiagnosisCode>
            {
                new DiagnosisCode{ClaimId = 123, Instance = "O", IcddCode = "256369"},
                new DiagnosisCode {ClaimId = 123, Instance = "O", IcddCode = "25636.256"}
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
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }


        /// <summary>
        /// Evaluates for ICD 9 diagnosis code not null.
        /// </summary>
        [TestMethod]
        public void EvaluateForIcd9DiagnosisCodeNotNull()
        {
            //Arrange
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
                new PaymentResult {ClaimId = 123, AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
            };
            PaymentTypeMedicareIp paymentTypeMedicareIp = new PaymentTypeMedicareIp
            {
                PayAtClaimLevel = true,
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
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
                HcpcsCode = "300",
                ServiceTypeId = 1,
                IsMedicareIpAcuteEnabled = true
            };

            Mock<IContractServiceTypeLogic> mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            Mock<IPaymentResultLogic> mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeMedicareIpRepository> paymentTypeMedicareIpRepository = new Mock<IPaymentTypeMedicareIpRepository>();
            paymentTypeMedicareIpRepository.Setup(x => x.GetPaymentTypeMedicareIpPayment(It.IsAny<PaymentTypeMedicareIp>()))
                .Returns(paymentTypeMedicareIp);

            var target = new PaymentTypeMedicareIpLogic(paymentTypeMedicareIpRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareIp
            };


           
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.StatementThru = DateTime.Parse("10/01/2014 12:00:00 AM");
            evaluateableClaim.MedicareInPatient = new MedicareInPatient
            {
                Charges = 10,
                ClaimId = 123,
                Npi = "1003814971",
                Drg = "070",
                DischargeDate = DateTime.Now.AddDays(-20),
                DischargeStatus = "01",
                LengthOfStay = 10
            };

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
                    HcpcsCode = "305"
                }
            };
            evaluateableClaim.DiagnosisCodes = new List<DiagnosisCode>
            {
                new DiagnosisCode{ClaimId = 123, Instance = "O", IcddCode = "256369"},
                new DiagnosisCode {ClaimId = 123, Instance = "O", IcddCode = "25636.256"}
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
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates for ic d9 procedure code not null.
        /// </summary>
        [TestMethod]
        public void EvaluateForIcd9ProcedureCodeNotNull()
        {
            //Arrange
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
                new PaymentResult {ClaimId = 123, AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
            };
            PaymentTypeMedicareIp paymentTypeMedicareIp = new PaymentTypeMedicareIp
            {
                PayAtClaimLevel = true,
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
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
                HcpcsCode = "300",
                ServiceTypeId = 1,
                IsMedicareIpAcuteEnabled = true
            };

            Mock<IContractServiceTypeLogic> mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            Mock<IPaymentResultLogic> mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeMedicareIpRepository> paymentTypeMedicareIpRepository = new Mock<IPaymentTypeMedicareIpRepository>();
            paymentTypeMedicareIpRepository.Setup(x => x.GetPaymentTypeMedicareIpPayment(It.IsAny<PaymentTypeMedicareIp>()))
                .Returns(paymentTypeMedicareIp);

            var target = new PaymentTypeMedicareIpLogic(paymentTypeMedicareIpRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareIp
            };


            
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.StatementThru = DateTime.Parse("10/01/2014 12:00:00 AM");
            evaluateableClaim.MedicareInPatient = new MedicareInPatient
            {
                Charges = 10,
                ClaimId = 123,
                Npi = "1003814971",
                Drg = "070",
                DischargeDate = DateTime.Now.AddDays(-20),
                DischargeStatus = "01",
                LengthOfStay = 10
            };

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
                    HcpcsCode = "305"
                }
            };
            evaluateableClaim.ProcedureCodes = new List<ProcedureCode>
            {
                new ProcedureCode{ClaimId = 123, Instance = "O", IcdpCode = "256369"},
                new ProcedureCode {ClaimId = 123, Instance = "O", IcdpCode = "25636.256"}
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
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates for ICD 10 procedure code not null.
        /// </summary>
        [TestMethod]
        public void EvaluateForIcd10ProcedureCodeNotNull()
        {
            //Arrange
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1,AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
                new PaymentResult {ClaimId = 123, AdjudicatedValue = null,ServiceTypeId = 1,ContractId = 1},
            };
            PaymentTypeMedicareIp paymentTypeMedicareIp = new PaymentTypeMedicareIp
            {
                PayAtClaimLevel = true,
                ValidLineIds = new List<int> { 1, 2, 3, 4, 5, 6 },
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
                HcpcsCode = "300",
                ServiceTypeId = 1,
                IsMedicareIpAcuteEnabled = true
            };

            Mock<IContractServiceTypeLogic> mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            Mock<IPaymentResultLogic> mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            Mock<IPaymentTypeMedicareIpRepository> paymentTypeMedicareIpRepository = new Mock<IPaymentTypeMedicareIpRepository>();
            paymentTypeMedicareIpRepository.Setup(x => x.GetPaymentTypeMedicareIpPayment(It.IsAny<PaymentTypeMedicareIp>()))
                .Returns(paymentTypeMedicareIp);

            var target = new PaymentTypeMedicareIpLogic(paymentTypeMedicareIpRepository.Object)
            {
                PaymentTypeBase = paymentTypeMedicareIp
            };

            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.StatementThru = DateTime.Parse("10/01/2015 12:00:00 AM");
            evaluateableClaim.MedicareInPatient = new MedicareInPatient
            {
                Charges = 10,
                ClaimId = 123,
                Npi = "1003814971",
                Drg = "070",
                DischargeDate = DateTime.Now.AddDays(-20),
                DischargeStatus = "01",
                LengthOfStay = 10
            };

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
                    HcpcsCode = "305"
                }
            };
            evaluateableClaim.ProcedureCodes = new List<ProcedureCode>
            {
                new ProcedureCode{ClaimId = 123, Instance = "O", IcdpCode = "256369"},
                new ProcedureCode {ClaimId = 123, Instance = "O", IcdpCode = "25636.256"}
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
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

    }
}
