/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Payment Type Medicare OP DetailsLogicTesting
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
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for PaymentTypeMedicareOPDetailsLogicUnitTest
    /// </summary>
    [TestClass]
    public class PaymentTypeMedicareOpDetailsLogicUnitTest
    {
        /// <summary>
        /// Payments the type Medicare property details constructor unit test1.
        /// </summary>
        [TestMethod]
        public void PaymentTypeMedicareOpDetailsConstructorUnitTest1()
        {
            var target = new PaymentTypeMedicareOpLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTypeMedicareOpLogic));
        }

        /// <summary>
        /// Payments the type medicare property details constructor unit test2.
        /// </summary>
        [TestMethod]
        public void PaymentTypeMedicareOpDetailsConstructorUnitTest2()
        {
            var mockAddEditPaymentTypeMedicareOpPayment = new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            var target =
                new PaymentTypeMedicareOpLogic(mockAddEditPaymentTypeMedicareOpPayment.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object);
            Assert.IsInstanceOfType(target, typeof(PaymentTypeMedicareOpLogic));
        }

        /// <summary>
        /// AddEditPaymentTypeMedicareOPPayment test  For IsNull
        /// </summary>
        [TestMethod]
        public void AddEditPaymentTypeMedicareOpPaymentIfNull()
        {
            var mockAddEditPaymentTypeMedicareOpPayment = new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            mockAddEditPaymentTypeMedicareOpPayment.Setup(
                f => f.AddEditPaymentTypeMedicareOpPayment(It.IsAny<PaymentTypeMedicareOp>())).Returns(0);
            var target =
                new PaymentTypeMedicareOpLogic(mockAddEditPaymentTypeMedicareOpPayment.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object);
            long actual = target.AddEditPaymentType(null);
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        ///  AddEditPaymentTypeMedicareOPPayment test  For Null
        /// </summary>
        [TestMethod]
        public void AddEditPaymentTypeMedicareOpPaymentIfNotNull()
        {
            var mockAddEditPaymentTypeMedicareOpPayment = new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            mockAddEditPaymentTypeMedicareOpPayment.Setup(
                f => f.AddEditPaymentTypeMedicareOpPayment(It.IsAny<PaymentTypeMedicareOp>())).Returns(1);
            var target =
                new PaymentTypeMedicareOpLogic(mockAddEditPaymentTypeMedicareOpPayment.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object);
            var objPaymentTypeMedicareOpPayment = new PaymentTypeMedicareOp { PaymentTypeDetailId = 1 };

            long actual = target.AddEditPaymentType(objPaymentTypeMedicareOpPayment);
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Gets the payment type medicare op details unit test.
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeMedicareOpDetailsUnitTest()
        {
            var mockGetPaymentTypeMedicareOpDetails = new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            //Mock Input
            var objMedicareOpDetails = new PaymentTypeMedicareOp
            {
                PaymentTypeId = 5,
                ContractId = 345,
                ServiceTypeId = null
            };
            //Mock output 
            var result = new PaymentTypeMedicareOp { OutPatient = 43.56, PaymentTypeDetailId = 234 };
            mockGetPaymentTypeMedicareOpDetails.Setup(f => f.GetPaymentTypeMedicareOpDetails(objMedicareOpDetails))
                .Returns(result);
            var target =
                new PaymentTypeMedicareOpLogic(mockGetPaymentTypeMedicareOpDetails.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object);
            PaymentTypeMedicareOp actual = (PaymentTypeMedicareOp)target.GetPaymentType(objMedicareOpDetails);
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Gets the payment type medicare op details unit test if null.
        /// </summary>
        [TestMethod]
        public void GetPaymentTypeMedicareOpDetailsUnitTestIfNull()
        {
            var mockGetPaymentTypeMedicareOpDetails = new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            //Mock Input
            PaymentTypeMedicareOp objMedicareOpDetails = null;
            //Mock output 
            mockGetPaymentTypeMedicareOpDetails.Setup(f => f.GetPaymentTypeMedicareOpDetails(objMedicareOpDetails))
                .Returns((PaymentTypeMedicareOp)null);
            var target =
                new PaymentTypeMedicareOpLogic(mockGetPaymentTypeMedicareOpDetails.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object);
            PaymentTypeMedicareOp actual = (PaymentTypeMedicareOp)target.GetPaymentType(null);
            Assert.AreEqual(null, actual);
        }

        /// <summary>
        /// Evaluates at line level test.
        /// </summary>
        [TestMethod]
        public void EvaluateAtLineLevelTest()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1, AdjudicatedValue = null, ServiceTypeId = 1, ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 2, AdjudicatedValue = null, ServiceTypeId = 1, ContractId = 1},
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            // _target.Contract = contract;
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                CRecord = new CRecord
                {
                    Dob = DateTime.Now.AddDays(-1000),
                    ClaimId = "1.04.2",
                    Sex = 1,
                    FromDate = DateTime.Parse("09/12/2019"),
                    ThruDate = DateTime.Parse("10/20/2019"),
                    ConditionCodes = new List<string> { "9", "10" },
                    BillType = "331",
                    Npi = string.Empty,
                    Oscar = string.Empty,
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = new List<string> { "AB", "CD" },
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "A",
                    BeneAmount = 50.35,
                    BloodPint = 2
                },
                DRecord = new DRecord
                {
                    ClaimId = "123",
                    AdmitDiagnosisCode = "4444",
                    PrincipalDiagnosisCode = "5555",
                    SecondaryDiagnosisCodes =
                        new List<string> { "9981", "0381", "2918", "9223", "30080", "5190", "5648", "7638" }
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "123",
                        LineItemId = 1,
                        HcpcsProcedureCode = "00164",
                        HcpcsModifiers = "4455667788",
                        ServiceDate = DateTime.Parse("09/30/2019"),
                        RevenueCode = "0929",
                        UnitsofService = 1,
                        LineItemCharge = 0000000.00,
                        LineItemFlag = 4
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
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
                },

            };


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates at claim level test.
        /// </summary>
        [TestMethod]
        public void EvaluateAtClaimLevelTest()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, AdjudicatedValue = null, ServiceTypeId = 1, Line = 1, ContractId = 1},
                new PaymentResult {ClaimId = 123, AdjudicatedValue = null, ServiceTypeId = 1, ContractId = 1},
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
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
                IsMedicareOpApcEnabled = true
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                CRecord = new CRecord
                {
                    Dob = DateTime.Now.AddDays(-1000),
                    ClaimId = "1.04.2",
                    Sex = 1,
                    FromDate = DateTime.Parse("09/12/2019"),
                    ThruDate = DateTime.Parse("10/20/2019"),
                    ConditionCodes = new List<string> { "9", "10" },
                    BillType = "331",
                    Npi = string.Empty,
                    Oscar = string.Empty,
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = new List<string> { "AB", "CD" },
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "A",
                    BeneAmount = 50.35,
                    BloodPint = 2
                },
                DRecord = new DRecord
                {
                    ClaimId = "123",
                    AdmitDiagnosisCode = "4444",
                    PrincipalDiagnosisCode = "5555",
                    SecondaryDiagnosisCodes =
                        new List<string> { "9981", "0381", "2918", "9223", "30080", "5190", "5648", "7638" }
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "123",
                        LineItemId = 1,
                        HcpcsProcedureCode = "00164",
                        HcpcsModifiers = "4455667788",
                        ServiceDate = DateTime.Parse("09/30/2019"),
                        RevenueCode = "0929",
                        UnitsofService = 1,
                        LineItemCharge = 0000000.00,
                        LineItemFlag = 4
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
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


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates the payment type if is proper input data is true.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeIfIsProperInputDataIsTrue()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1, AdjudicatedValue = null, ServiceTypeId = 1, ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 2, AdjudicatedValue = null, ServiceTypeId = 1, ContractId = 1},
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            
            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                CRecord = new CRecord
                {
                    Dob = DateTime.Now.AddDays(-1000),
                    ClaimId = "1.04.2",
                    Sex = 1,
                    FromDate = DateTime.Parse("09/12/2019"),
                    ThruDate = DateTime.Parse("10/20/2019"),
                    ConditionCodes = new List<string> { "9", "10" },
                    BillType = "331",
                    Npi = string.Empty,
                    Oscar = string.Empty,
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = new List<string> { "AB", "CD" },
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "A",
                    BeneAmount = 50.35,
                    BloodPint = 2
                },
                DRecord = new DRecord
                {
                    ClaimId = "123",
                    AdmitDiagnosisCode = "4444",
                    PrincipalDiagnosisCode = "5555",
                    SecondaryDiagnosisCodes =
                        new List<string> { "9981", "0381", "2918", "9223", "30080", "5190", "5648", "7638" }
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "123",
                        LineItemId = 1,
                        HcpcsProcedureCode = "00164",
                        HcpcsModifiers = "4455667788",
                        ServiceDate = DateTime.Parse("09/30/2019"),
                        RevenueCode = "0929",
                        UnitsofService = 1,
                        LineItemCharge = 0000000.00,
                        LineItemFlag = 4
                    },
                    new LRecord
                    {
                        ClaimId = "345",
                        LineItemId = 2,
                        HcpcsProcedureCode = "00164",
                        HcpcsModifiers = "4455667788",
                        ServiceDate = DateTime.Parse("09/30/2019"),
                        RevenueCode = "0929",
                        UnitsofService = 1,
                        LineItemCharge = 0000000.00,
                        LineItemFlag = 5
                    }
                }
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

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates the payment type if is proper input data is false.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeIfIsProperInputDataIsFalse()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1, AdjudicatedValue = null, ServiceTypeId = 1, ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 2, AdjudicatedValue = null, ServiceTypeId = 1, ContractId = 1},
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                CRecord = new CRecord
                {
                    Dob = DateTime.Now.AddDays(-1000),
                    ClaimId = "1.04.2",
                    Sex = 1,
                    FromDate = DateTime.Parse("09/12/2019"),
                    ThruDate = DateTime.Parse("10/20/2019"),
                    ConditionCodes = new List<string> { "9", "10" },
                    BillType = "331",
                    Npi = string.Empty,
                    Oscar = string.Empty,
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = new List<string> { "AB", "CD" },
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "A",
                    BeneAmount = 50.35,
                    BloodPint = 2
                },
                DRecord = new DRecord
                {
                    ClaimId = "123",
                    AdmitDiagnosisCode = "4444",
                    PrincipalDiagnosisCode = "5555",
                    SecondaryDiagnosisCodes =
                        new List<string> { "9981", "0381", "2918", "9223", "30080", "5190", "5648", "7638" }
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "123",
                        LineItemId = 1,
                        HcpcsProcedureCode = "00164",
                        HcpcsModifiers = "4455667788",
                        ServiceDate = DateTime.Parse("09/30/2019"),
                        RevenueCode = "0929",
                        UnitsofService = 1,
                        LineItemCharge = 0000000.00,
                        LineItemFlag = 4
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
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

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        //To check GetMedicareOutPatientResult() if Lline is empty
        /// <summary>
        /// Evaluates the payment type if lline is empty.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeIfLlineIsEmpty()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1, AdjudicatedValue = null, ServiceTypeId = 1, ContractId = 1},
                new PaymentResult {ClaimId = 123, Line = 2, AdjudicatedValue = null, ServiceTypeId = 1, ContractId = 1},
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim();
            evaluateableClaim.ClaimId = 123;
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                CRecord = new CRecord
                {
                    Dob = DateTime.Now.AddDays(-1000),
                    ClaimId = "1.04.2",

                    Sex = 1,
                    FromDate = DateTime.Parse("09/12/2019"),
                    ThruDate = DateTime.Parse("10/20/2019"),
                    ConditionCodes = new List<string> { "9", "10" },
                    BillType = "331",
                    Npi = string.Empty,
                    Oscar = string.Empty,
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = new List<string> { "AB", "CD" },
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "A",
                    BeneAmount = 50.35,
                    BloodPint = 2
                },
                DRecord = new DRecord
                {
                    ClaimId = "123",
                    AdmitDiagnosisCode = "4444",
                    PrincipalDiagnosisCode = "5555",
                    SecondaryDiagnosisCodes =
                        new List<string> { "9981", "0381", "2918", "9223", "30080", "5190", "5648", "7638" }
                },
                LRecords = new List<LRecord>()
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

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates the payment type for edit error codes.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeForEditErrorCodes()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ClaimStatus = 1, ClaimTotalCharges = 347, ContractId = 237043},
                new PaymentResult {ClaimId = 411930180, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                ClaimStat = "Billed",
                ClaimTotal = 347,
                ClaimType = "HOSP-Hospital",
                DiagnosisCodes =
                    new List<DiagnosisCode>
                    {
                        new DiagnosisCode {ClaimId = 411930180, IcddCode = "64083", Instance = "P"}
                    },
                DischargeStatus = "1",
                Drg = "000",
                Ftn = "710561765",
                InsuredCodes =
                    new List<InsuredData> { new InsuredData { CertificationNumber = "ABC411930180", ClaimId = 411930180 } },
                LastFiledDate = DateTime.Parse("03/07/2012"),
                MedicareInPatient =
                    new MedicareInPatient
                    {
                        Charges = 347,
                        ClaimId = 411930180,
                        DischargeDate = DateTime.Parse("03/07/2012"),
                        DischargeStatus = "1",
                        Drg = "000",
                        Npi = "1881788933"
                    },

            };
            evaluateableClaim.ClaimId = 411930180;
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 411930180,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("04/15/1987"),
                    ClaimId = "411930180",

                    Sex = 2,
                    FromDate = DateTime.Parse("02/02/2012"),
                    ThruDate = DateTime.Parse("02/02/2012"),
                    ConditionCodes = new List<string> { "9", "10" },
                    BillType = "131",
                    Npi = "1881788933",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = new List<string> { "AB", "CD" },
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "A",
                    BeneAmount = 50.35,
                    BloodPint = 1
                },
                DRecord = new DRecord
                {
                    ClaimId = "411930180",
                    AdmitDiagnosisCode = "4444",
                    PrincipalDiagnosisCode = "4119",
                    SecondaryDiagnosisCodes =
                        new List<string> { "9981", "0381", "2918", "9223", "30080", "5190", "5648", "7638" }
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "411930180",
                        LineItemId = 1,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = "4455667788",
                        ServiceDate = DateTime.Parse("02/02/2012"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 22,
                        LineItemFlag = 4
                    },
                    new LRecord
                    {
                        ClaimId = "411930180",
                        LineItemId = 1,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = "4455667788",
                        ServiceDate = DateTime.Parse("02/02/2012"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 22,
                        LineItemFlag = 4
                    }
                },
                MedicareOutPatientRecord = new MedicareOutPatient { AdjustFactor = 1 }
            };

            evaluateableClaim.ClaimTotal = 100;
            evaluateableClaim.ClaimCharges = new List<ClaimCharge>
            {
                new ClaimCharge
                {
                    CoveredCharge = 22,
                    RevCode = "300",
                    ServiceFromDate = DateTime.Parse("02/02/2012"),
                    ServiceThruDate = DateTime.Parse("01/01/1900"),
                    Line = 1,
                    Amount = 22,
                    HcpcsCode = "36415"
                }
            };
            evaluateableClaim.BillDate = DateTime.Parse("09/30/2019");
            evaluateableClaim.BillType = "131";
            evaluateableClaim.ClaimDate = DateTime.Parse("02/07/2012");
            evaluateableClaim.ClaimLink = 640321;


            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 123, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates the payment type when error codes is null.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeWhenErrorCodesIsNull()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 347, ContractId = 237043},
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 424538161,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 424538161, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 424538161,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 424538161, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "45", Instance = 0}
                },
                PatAcctNum = "id424538161",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 424538161,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("03/06/2014"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 424538161,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "424538161",
                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1
                },
                DRecord = new DRecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string>()
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 1,
                        HcpcsProcedureCode = "80048",
                        HcpcsModifiers = string.Empty, // checked THIS
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 2,
                        HcpcsProcedureCode = "85025",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 3,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 31,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 4,
                        HcpcsProcedureCode = "74230",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "320",
                        UnitsofService = 1,
                        LineItemCharge = 212,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 5,
                        HcpcsProcedureCode = "92611",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "440",
                        UnitsofService = 1,
                        LineItemCharge = 242,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 424538161, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates the payment type when calculate errors is null.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeWhenCalcErrorsIsNull()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult
                {
                    ClaimId = 424538161,
                    ClaimStatus = 1,
                    ClaimTotalCharges = 347,
                    ContractId = 237043,
                    Line = 1
                },
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 424538161,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 424538161, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 424538161,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 424538161, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "45", Instance = 0}
                },
                PatAcctNum = "id424538161",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 424538161,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("03/06/2014"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 424538161,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "424538161",

                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1
                },
                DRecord = new DRecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string>()
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 1,
                        HcpcsProcedureCode = "80048",
                        HcpcsModifiers = string.Empty, // checked THIS
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 2,
                        HcpcsProcedureCode = "85025",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 3,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 31,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 4,
                        HcpcsProcedureCode = "74230",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "320",
                        UnitsofService = 1,
                        LineItemCharge = 212,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 5,
                        HcpcsProcedureCode = "92611",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "440",
                        UnitsofService = 1,
                        LineItemCharge = 242,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 424538161, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        //Error For Invalid Provider Name
        /// <summary>
        /// Evaluates the payment type when pricer errors is present.
        /// </summary>
        [TestMethod]
        public void EvaluatePaymentTypeWhenPricerErrorsIsPresent()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 422229776, ClaimStatus = 1, ClaimTotalCharges = 108, ContractId = 238046,},
                new PaymentResult
                {
                    ClaimId = 422229776,
                    ClaimStatus = 1,
                    ClaimTotalCharges = 108,
                    ContractId = 238046,
                    IsClaimChargeData = true,
                    IsInitialEntry = true,
                    Line = 1
                }
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 422229776,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 422229776, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 422229776,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 422229776, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 422229776, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 422229776, Code = "45", Instance = 0}
                },
                PatAcctNum = "id422229776",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 422229776,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("03/06/2014"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 422229776,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "422229776",

                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1
                },
                DRecord = new DRecord
                {
                    ClaimId = "422229776",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string>()
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "422229776",
                        LineItemId = 1,
                        HcpcsProcedureCode = "",
                        HcpcsModifiers = string.Empty, // checked THIS
                        ServiceDate = DateTime.Parse("07/18/2014"),
                        RevenueCode = "521",
                        UnitsofService = 1,
                        LineItemCharge = 108,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1.0,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 422229776,
                        Npi = "1245481720",
                        ServiceDate = DateTime.Parse("07/18/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 422229776, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 422229776, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Gets the line level amount when micro dyn is on.
        /// </summary>
        [TestMethod]
        public void GetLineLevelAmountWhenMicroDynIsOn()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult
                {
                    ClaimId = 424538161,
                    ClaimStatus = 1,
                    ClaimTotalCharges = 347,
                    ContractId = 237043,
                    Line = 1
                },
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                ServiceTypeId = 1
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 424538161,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 424538161, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 424538161,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 424538161, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "45", Instance = 0}
                },
                PatAcctNum = "id424538161",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 424538161,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("03/06/2014"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 424538161,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "424538161",

                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1
                },
                DRecord = new DRecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string>()
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 1,
                        HcpcsProcedureCode = "80048",
                        HcpcsModifiers = string.Empty, // checked THIS
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 2,
                        HcpcsProcedureCode = "85025",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 3,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 31,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 4,
                        HcpcsProcedureCode = "74230",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "320",
                        UnitsofService = 1,
                        LineItemCharge = 212,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 5,
                        HcpcsProcedureCode = "92611",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "440",
                        UnitsofService = 1,
                        LineItemCharge = 242,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 424538161, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates at claim level when edit and pricer success is true.
        /// </summary>
        [TestMethod]
        public void EvaluateAtClaimLevelWhenEditAndPricerSuccessIsTrue()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult
                {
                    ClaimId = 424538161,
                    ClaimStatus = 1,
                    ClaimTotalCharges = 347,
                    ContractId = 237043,
                    Line = 1
                },
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                PayAtClaimLevel = true,
                IsMedicareOpApcEnabled = true
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 424538161,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 424538161, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 424538161,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 424538161, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "45", Instance = 0}
                },
                PatAcctNum = "id424538161",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 424538161,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("03/06/2014"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 424538161,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "424538161",

                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1
                },
                DRecord = new DRecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string>()
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 1,
                        HcpcsProcedureCode = "80048",
                        HcpcsModifiers = string.Empty, // checked THIS
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 2,
                        HcpcsProcedureCode = "85025",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 3,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 31,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 4,
                        HcpcsProcedureCode = "74230",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "320",
                        UnitsofService = 1,
                        LineItemCharge = 212,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 5,
                        HcpcsProcedureCode = "92611",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "440",
                        UnitsofService = 1,
                        LineItemCharge = 242,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 424538161, Line = 1, ContractId = 1, AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates at claim level when edit and pricer success is false.
        /// </summary>
        [TestMethod]
        public void EvaluateAtClaimLevelWhenEditAndPricerSuccessIsFalse()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult
                {
                    ClaimId = 424538161,
                    ClaimStatus = 1,
                    ClaimTotalCharges = 347,
                    ContractId = 237043,
                    Line = 1
                },
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                PayAtClaimLevel = true,
                IsMedicareOpApcEnabled = true
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 424538161,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 424538161, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 424538161,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 424538161, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "45", Instance = 0}
                },
                PatAcctNum = "id424538161",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 424538161,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("03/06/2014"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 424538161,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "424538161",

                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1
                },
                DRecord = new DRecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string>()
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 1,
                        HcpcsProcedureCode = "80048",
                        HcpcsModifiers = "4455667788",
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 2,
                        HcpcsProcedureCode = "85025",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 3,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 31,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 4,
                        HcpcsProcedureCode = "74230",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "320",
                        UnitsofService = 1,
                        LineItemCharge = 212,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 5,
                        HcpcsProcedureCode = "92611",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "440",
                        UnitsofService = 1,
                        LineItemCharge = 242,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 424538161, Line = 1, ContractId = 1, AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates at claim level when edit success is false.
        /// </summary>
        [TestMethod]
        public void EvaluateAtClaimLevelWhenEditSuccessIsFalse()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult
                {
                    ClaimId = 424538161,
                    ClaimStatus = 1,
                    ClaimTotalCharges = 347,
                    ContractId = 237043,
                    Line = 1
                },
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                PayAtClaimLevel = true,
                IsMedicareOpApcEnabled = true
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 424538161,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 424538161, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 424538161,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 424538161, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "45", Instance = 0}
                },
                PatAcctNum = "id424538161",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 424538161,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("03/06/2014"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 424538161,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "424538161",

                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1
                },
                DRecord = new DRecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string>()
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 1,
                        HcpcsProcedureCode = "80048",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 2,
                        HcpcsProcedureCode = "85025",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 3,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 31,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 4,
                        HcpcsProcedureCode = "74230",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "320",
                        UnitsofService = 1,
                        LineItemCharge = 212,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 5,
                        HcpcsProcedureCode = "92611",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "440",
                        UnitsofService = 1,
                        LineItemCharge = 242,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 424538161, Line = 1, ContractId = 1, AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates at claim level when success and out patient not null.
        /// </summary>
        [TestMethod]
        public void EvaluateAtClaimLevelWhenSuccessAndOutPatientNotNull()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult
                {
                    ClaimId = 424538161,
                    ClaimStatus = 1,
                    ClaimTotalCharges = 347,
                    ContractId = 237043,
                    Line = 1
                },
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
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
                ServiceTypeId = 1,
                PayAtClaimLevel = true,
                OutPatient = 100,
                IsMedicareOpApcEnabled = true
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = Factory.CreateInstance<ICRecordLogic>();
            var mockDRecord = Factory.CreateInstance<IDRecordLogic>();
            var mockERecord = Factory.CreateInstance<IERecordLogic>();
            var mockLRecord = Factory.CreateInstance<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord, mockDRecord, mockERecord, mockLRecord)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 424538161,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 424538161, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 424538161,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 424538161, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "45", Instance = 0}
                },
                PatAcctNum = "id424538161",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 424538161,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("03/06/2014"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 424538161,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "424538161",

                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1,
                    PatientData = new PatientData
                    {
                        FirstName = "ANUPAMA",
                        LastName = "SAIRAM",
                        MiddleName = "A",
                        Medicare = "",
                        Gender = 1,
                        Status = "1",
                        Dob = DateTime.Parse("06/18/1933"),

                    }
                },
                DRecord = new DRecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string> { "43882" }
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 1,
                        HcpcsProcedureCode = "80048",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 2,
                        HcpcsProcedureCode = "85025",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 3,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 31,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 4,
                        HcpcsProcedureCode = "74230",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "320",
                        UnitsofService = 1,
                        LineItemCharge = 212,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 5,
                        HcpcsProcedureCode = "92611",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "440",
                        UnitsofService = 1,
                        LineItemCharge = 242,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 424538161, Line = 1, ContractId = 1, AdjudicatedValue = 110}
            };

            var mockContractBaseLogic = new Mock<ContractBaseLogic>();
            mockContractServiceTypeLogic.Setup(x => x.IsValidServiceType()).Returns(true);
            mockContractServiceTypeLogic.Setup(x => x.Evaluate(evaluateableClaim, paymentResults, true, false))
                .Returns(updatedPaymentResults);
            mockContractBaseLogic.SetupAllProperties();

            //Act
            List<PaymentResult> actual = target.EvaluatePaymentType(evaluateableClaim, paymentResults, true, false);

            // Assert
            Assert.AreEqual(3, actual.Count);
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNotNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates at line level if carve out.
        /// </summary>
        [TestMethod]
        public void EvaluateAtLineLevelIfCarveOut()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult
                {
                    ClaimId = 424538161,
                    ClaimStatus = 1,
                    ClaimTotalCharges = 347,
                    ContractId = 237043,
                    Line = 1,
                    ServiceTypeId = 1
                },
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                PayAtClaimLevel = false,
                OutPatient = 100
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 424538161,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 424538161, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 424538161,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 424538161, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "45", Instance = 0}
                },
                PatAcctNum = "id424538161",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 424538161,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("03/06/2014"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 424538161,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "424538161",

                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1,
                    PatientData = new PatientData
                    {
                        FirstName = "ANUPAMA",
                        LastName = "SAIRAM",
                        MiddleName = "A",
                        Medicare = "",
                        Gender = 1,
                        Status = "1",
                        Dob = DateTime.Parse("06/18/1933"),

                    }
                },
                DRecord = new DRecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string>()
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 1,
                        HcpcsProcedureCode = "80048",
                        HcpcsModifiers = string.Empty, // checked THIS
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 2,
                        HcpcsProcedureCode = "85025",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 3,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 31,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 4,
                        HcpcsProcedureCode = "74230",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "320",
                        UnitsofService = 1,
                        LineItemCharge = 212,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 5,
                        HcpcsProcedureCode = "92611",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "440",
                        UnitsofService = 1,
                        LineItemCharge = 242,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 424538161, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates at line level if line amount is not null.
        /// </summary>
        [TestMethod]
        public void EvaluateAtLineLevelIfLineAmountIsNotNull()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult
                {
                    ClaimId = 424538161,
                    ClaimStatus = 1,
                    ClaimTotalCharges = 347,
                    ContractId = 237043,
                    Line = 1
                },
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                PayAtClaimLevel = false,
                OutPatient = 100,
                IsMedicareOpApcEnabled = true
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = Factory.CreateInstance<ICRecordLogic>();
            var mockDRecord = Factory.CreateInstance<IDRecordLogic>();
            var mockERecord = Factory.CreateInstance<IERecordLogic>();
            var mockLRecord = Factory.CreateInstance<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord, mockDRecord, mockERecord, mockLRecord)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 424538161,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 424538161, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 424538161,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 424538161, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "45", Instance = 0}
                },
                PatAcctNum = "id424538161",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 424538161,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("03/06/2014"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 424538161,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "424538161",

                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1,
                    PatientData = new PatientData
                    {
                        FirstName = "ANUPAMA",
                        LastName = "SAIRAM",
                        MiddleName = "A",
                        Medicare = "",
                        Gender = 1,
                        Status = "1",
                        Dob = DateTime.Parse("06/18/1933"),

                    }
                },
                DRecord = new DRecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string> { "43882" }
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 1,
                        HcpcsProcedureCode = "80048",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 2,
                        HcpcsProcedureCode = "85025",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 3,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 31,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 4,
                        HcpcsProcedureCode = "74230",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "320",
                        UnitsofService = 1,
                        LineItemCharge = 212,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 5,
                        HcpcsProcedureCode = "92611",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "440",
                        UnitsofService = 1,
                        LineItemCharge = 242,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 424538161, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNotNull(firstOrDefault);
        }

        /// <summary>
        /// Evaluates at line level if out patient is null.
        /// </summary>
        [TestMethod]
        public void EvaluateAtLineLevelIfOutPatientIsNull()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult
                {
                    ClaimId = 424538161,
                    ClaimStatus = 1,
                    ClaimTotalCharges = 347,
                    ContractId = 237043,
                    Line = 1
                },
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                PayAtClaimLevel = false,
                OutPatient = null
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 424538161,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 424538161, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 424538161,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 424538161, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "45", Instance = 0}
                },
                PatAcctNum = "id424538161",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 424538161,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("03/06/2014"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 424538161,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "424538161",

                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1
                },
                DRecord = new DRecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string>()
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 1,
                        HcpcsProcedureCode = "80048",
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 2,
                        HcpcsProcedureCode = "85025",
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 3,
                        HcpcsProcedureCode = "36415",
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 31,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 4,
                        HcpcsProcedureCode = "74230",

                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "320",
                        UnitsofService = 1,
                        LineItemCharge = 212,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 5,
                        HcpcsProcedureCode = "92611",
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "440",
                        UnitsofService = 1,
                        LineItemCharge = 242,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 424538161, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }

        /// <summary>
        /// Gets the medicare out patient result for ICD 9.
        /// </summary>
        [TestMethod]
        public void GetMedicareOEvaluateAtClaimLevelWhenEditSuccessIsFalseutPatientResultForIcd9()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult
                {
                    ClaimId = 424538161,
                    ClaimStatus = 1,
                    ClaimTotalCharges = 347,
                    ContractId = 237043,
                    Line = 1
                },
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
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
                ServiceTypeId = 1,
                PayAtClaimLevel = false,
                OutPatient = 100,
                IsMedicareOpApcEnabled = true
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = Factory.CreateInstance<ICRecordLogic>();
            var mockDRecord = Factory.CreateInstance<IDRecordLogic>();
            var mockERecord = Factory.CreateInstance<IERecordLogic>();
            var mockLRecord = Factory.CreateInstance<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord, mockDRecord, mockERecord, mockLRecord)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 424538161,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 424538161, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 424538161,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 424538161, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "45", Instance = 0}
                },
                PatAcctNum = "id424538161",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 424538161,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("10/10/2014"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 424538161,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "424538161",

                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1,
                    PatientData = new PatientData
                    {
                        FirstName = "ANUPAMA",
                        LastName = "SAIRAM",
                        MiddleName = "A",
                        Medicare = "",
                        Gender = 1,
                        Status = "1",
                        Dob = DateTime.Parse("06/18/1933"),

                    }
                },
                DRecord = new DRecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string> { "43882" }
                },
                ERecord = new ERecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "256695669",
                    PrincipalDiagnosisCode = "43882225545",
                    SecondaryDiagnosisCodes = new List<string> { "43882225" }
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 1,
                        HcpcsProcedureCode = "80048",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 2,
                        HcpcsProcedureCode = "85025",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 3,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 31,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 4,
                        HcpcsProcedureCode = "74230",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "320",
                        UnitsofService = 1,
                        LineItemCharge = 212,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 5,
                        HcpcsProcedureCode = "92611",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "440",
                        UnitsofService = 1,
                        LineItemCharge = 242,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 424538161, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNotNull(firstOrDefault);
        }

        /// <summary>
        /// Gets the medicare out patient result for ICD 10.
        /// </summary>
        [TestMethod]
        public void GetMedicareOutPatientResultForIcd10()
        {
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult
                {
                    ClaimId = 424538161,
                    ClaimStatus = 1,
                    ClaimTotalCharges = 347,
                    ContractId = 237043,
                    Line = 1
                },
                new PaymentResult {ClaimId = 424538161, ClaimStatus = 1, ClaimTotalCharges = 22, ContractId = 237043}
            };
            var paymentTypeMedicareOp = new PaymentTypeMedicareOp
            {
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
                PayAtClaimLevel = false,
                OutPatient = 100
            };
            // Arrange
            var mockContractServiceTypeLogic = new Mock<IContractServiceTypeLogic>();
            var mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            mockPaymentResultLogic.Setup(
                x =>
                    x.Evaluate(It.IsAny<EvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), It.IsAny<bool>(),
                        It.IsAny<bool>())).Returns(paymentResults);
            var paymentTypeMedicareOpRepository =
                new Mock<IPaymentTypeMedicareOpRepository>();
            var mockCRecord = new Mock<ICRecordLogic>();
            var mockDRecord = new Mock<IDRecordLogic>();
            var mockERecord = new Mock<IERecordLogic>();
            var mockLRecord = new Mock<ILRecordLogic>();
            paymentTypeMedicareOpRepository.Setup(
                x => x.GetPaymentTypeMedicareOpDetails(It.IsAny<PaymentTypeMedicareOp>()))
                .Returns(paymentTypeMedicareOp);

            var target = new PaymentTypeMedicareOpLogic(paymentTypeMedicareOpRepository.Object, mockCRecord.Object, mockDRecord.Object, mockERecord.Object, mockLRecord.Object)
            {
                PaymentTypeBase = paymentTypeMedicareOp
            };


            IEvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                BillDate = DateTime.Parse("04/01/2014"),
                BillType = "131",
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Amount = 77,
                        ClaimId = 0,
                        CoveredCharge = 77,
                        HcpcsCode = "80048",
                        Line = 1,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 77,
                        CoveredCharge = 77,
                        ClaimId = 0,
                        HcpcsCode = "85025",
                        Line = 2,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 31,
                        ClaimId = 0,
                        CoveredCharge = 31,
                        HcpcsCode = "36415",
                        Line = 3,
                        RevCode = "300",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 212,
                        ClaimId = 0,
                        CoveredCharge = 212,
                        HcpcsCode = "74230",
                        Line = 4,
                        RevCode = "320",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    },
                    new ClaimCharge
                    {
                        Amount = 242,
                        ClaimId = 0,
                        CoveredCharge = 242,
                        HcpcsCode = "92611",
                        Line = 5,
                        RevCode = "440",
                        ServiceFromDate = DateTime.Parse("03/06/2014"),
                        ServiceThruDate = DateTime.Parse("01/01/1900"),
                        Units = 1
                    }
                },
                ClaimDate = DateTime.Parse("03/12/2014"),
                ClaimId = 424538161,
                ClaimLink = -2145398363,
                ClaimStat = "Partial Denial",
                ClaimTotal = 642,
                ClaimType = "HOSP - Hospital",
                ConditionCodes = new List<ConditionCode>(),
                DiagnosisCodes = new List<DiagnosisCode>
                {
                    new DiagnosisCode {ClaimId = 424538161, IcddCode = "43882", Instance = "P"}
                },
                DischargeStatus = "1",
                Drg = "",
                Ftn = "910729255",
                InsuredCodes =
                    new List<InsuredData>
                    {
                        new InsuredData
                        {
                            CertificationNumber = "ABC424538161",
                            ClaimId = 424538161,
                            InsuredFirstName = "First",
                            InsuredLastName = "Last",
                            InsuredMiddleName = "Z",
                            PayerName = "99999-0095-MEDICARE"
                        }
                    },
                LastFiledDate = DateTime.Parse("04/01/2014"),
                MedicareInPatient = new MedicareInPatient(),
                MedicareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Amount = 3, Hcpcs = "36415"},
                    new MedicareLabFeeSchedule {Amount = 8.71, Hcpcs = "80048"},
                    new MedicareLabFeeSchedule {Amount = 10.61, Hcpcs = "85025"}
                },
                Npi = "1710913140",
                OccurrenceCodes = new List<OccurrenceCode>
                {
                    new OccurrenceCode {ClaimId = 424538161, Code = "11", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "30", Instance = 0},
                    new OccurrenceCode {ClaimId = 424538161, Code = "45", Instance = 0}
                },
                PatAcctNum = "id424538161",
                PayerSequence = 1,
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            ClaimId = 424538161,
                            FirstName = "phyF",
                            LastName = "phyL",
                            MiddleName = "Z",
                            PhysicianId = 67762,
                            PhysicianType = "Attending"
                        }
                    },
                PriPayerName = "MEDICARE",
                SecPayerName = "TRICARE PRIME",
                SmartBox = new SmartBox { LOS = 0, TCC = 642 },
                Ssinumber = 3880,
                StatementFrom = DateTime.Parse("03/06/2014"),
                StatementThru = DateTime.Parse("10/10/2015"),
                TerPayerName = "Unknown",
                CustomField1 = "JLR",
                CustomField2 = "PT",


            };
            evaluateableClaim.MicrodynApcEditInput = new MicrodynApcEditInput
            {
                ClaimId = 424538161,
                CRecord = new CRecord
                {
                    Dob = DateTime.Parse("06/18/1933"),
                    ClaimId = "424538161",

                    Sex = 2,
                    FromDate = DateTime.Parse("03/06/2014"),
                    ThruDate = DateTime.Parse("03/06/2014"),
                    ConditionCodes = new List<string>(),
                    BillType = "131",
                    Npi = "1710913140",
                    Oscar = "12345",
                    PatientStatus = "1",
                    OppsFlag = 1,
                    OccurrenceCodes = null,
                    PatientFirstName = "ANUPAMA",
                    PatientLastName = "SAIRAM",
                    PatientMiddleInitial = "Z",
                    BeneAmount = 0.0,
                    BloodPint = 1,
                    PatientData = new PatientData
                    {
                        FirstName = "ANUPAMA",
                        LastName = "SAIRAM",
                        MiddleName = "A",
                        Medicare = "",
                        Gender = 1,
                        Status = "1",
                        Dob = DateTime.Parse("06/18/1933"),

                    }
                },
                DRecord = new DRecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "",
                    PrincipalDiagnosisCode = "43882",
                    SecondaryDiagnosisCodes = new List<string> { "43882" }
                },
                ERecord = new ERecord
                {
                    ClaimId = "424538161",
                    AdmitDiagnosisCode = "256695669",
                    PrincipalDiagnosisCode = "43882225545",
                    SecondaryDiagnosisCodes = new List<string> { "43882225" }
                },
                LRecords = new List<LRecord>
                {
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 1,
                        HcpcsProcedureCode = "80048",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 2,
                        HcpcsProcedureCode = "85025",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 77,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 3,
                        HcpcsProcedureCode = "36415",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "300",
                        UnitsofService = 1,
                        LineItemCharge = 31,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 4,
                        HcpcsProcedureCode = "74230",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "320",
                        UnitsofService = 1,
                        LineItemCharge = 212,
                        LineItemFlag = 0
                    },
                    new LRecord
                    {
                        ClaimId = "424538161",
                        LineItemId = 5,
                        HcpcsProcedureCode = "92611",
                        HcpcsModifiers = string.Empty,
                        ServiceDate = DateTime.Parse("03/06/2014"),
                        RevenueCode = "440",
                        UnitsofService = 1,
                        LineItemCharge = 242,
                        LineItemFlag = 0
                    }
                },
                MedicareOutPatientRecord =
                    new MedicareOutPatient
                    {
                        AdjustFactor = 1,
                        AdjustmentOptions = 0,
                        AllowTerminatorProvider = "false",
                        BeneDeductible = 0.0,
                        BloodDeductiblePints = 3,
                        ClaimId = 424538161,
                        Npi = "1710913140",
                        ServiceDate = DateTime.Parse("03/06/2014")
                    }
            };

            var updatedPaymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 424538161, ContractId = 1, AdjudicatedValue = 110},
                new PaymentResult {ClaimId = 424538161, Line = 1, ContractId = 1, AdjudicatedValue = 110}
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
            var firstOrDefault = paymentResults.FirstOrDefault(x => x.AdjudicatedValue != null);
            Assert.IsNull(firstOrDefault);
        }
      
    }
}